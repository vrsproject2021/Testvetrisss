USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_fee_schedule_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_fee_schedule_delete]
GO
/****** Object:  StoredProcedure [dbo].[ar_fee_schedule_delete]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_fee_schedule_delete : save 
                  rate fee schedule template
** Created By   : Pavel Guha
** Created On   : 24/02/2021
*******************************************************/
create procedure [dbo].[ar_fee_schedule_delete]
(
	@id                     uniqueidentifier,
	@type                   nchar(1),
	@updated_by             uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(700) = '' output,
	@error_code				nvarchar(10)	= '' output,
    @return_status			int				= 0  output
)
as
begin
	set nocount on 

	exec common_check_record_lock
		@menu_id       = @menu_id,
		@record_id     = @menu_id,
		@user_id       = @updated_by,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output
		
	if(@return_status=0)
		begin
			return 0
		end


	begin transaction
	
	if(@type ='M')
		begin
			update ar_modality_fee_schedule_template 
			set deleted      ='Y',
				deleted_by = @updated_by,
				date_deleted = getdate()
			where id=@id
		end
	else if(@type ='S')
		begin
			update ar_service_fee_schedule_template 
			set deleted      ='Y',
				deleted_by = @updated_by,
				date_deleted = getdate()
			where id=@id
		end 

	if(@@rowcount =0)
		begin
			rollback transaction
			select @error_code='129',@return_status=0
			return 0
		end

	
	commit transaction

	set @return_status=1
	set @error_code='130'
	set nocount off

	return 1
end



GO
