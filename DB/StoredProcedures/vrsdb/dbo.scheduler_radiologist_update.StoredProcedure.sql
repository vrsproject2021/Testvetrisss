USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_radiologist_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_radiologist_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_radiologist_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_radiologist_update : 
				  update radiologist's quick books table
** Created By   : Pavel Guha
** Created On   : 24/11/2020
*******************************************************/
create procedure [dbo].[scheduler_radiologist_update]
	@radiologist_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
	@creditor_id nvarchar(20)= '',
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	set nocount on

	declare @radiologist_name nvarchar(100),
	        @old_creditor_id nvarchar(20)
	
	    begin transaction

		select @radiologist_name = name,
		       @old_creditor_id = creditor_id
		from radiologists 
		where id=@radiologist_id

		if(@old_creditor_id<>@creditor_id)
			begin
				update radiologists
				set creditor_id   = @creditor_id,
					qb_name       = @radiologist_name,
					update_qb     = 'N', 
					update_qb_on  = getdate()
				where id=@radiologist_id
			end
		else
			begin
				update radiologists
				set creditor_id   = @creditor_id,
					update_qb   = 'N', 
					update_qb_on=getdate()
				where id=@radiologist_id
			end

		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_msg='Failed to update the billing account : '+ @radiologist_name,
				       @return_type=0
				return 0
			end

		commit transaction

	set nocount off
	select @error_msg='',@return_type=1
	return 1

end

GO
