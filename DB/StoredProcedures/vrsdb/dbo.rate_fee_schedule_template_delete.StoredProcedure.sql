USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rate_fee_schedule_template_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rate_fee_schedule_template_delete]
GO
/****** Object:  StoredProcedure [dbo].[rate_fee_schedule_template_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rate_fee_schedule_template_delete : save 
                  rate fee schedule template
** Created By   : Pavel Guha
** Created On   : 24/06/2019
*******************************************************/
create procedure [dbo].[rate_fee_schedule_template_delete]
(
	@id                     uniqueidentifier,
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
	
	update rates_fee_schedule_template 
	set deleted      ='Y',
	    deleted_by = @updated_by,
		date_deleted = getdate()
	where id=@id

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
