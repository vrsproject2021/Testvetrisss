USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_fee_shcedule_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_fee_shcedule_params_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_fee_shcedule_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_fee_shcedule_params_fetch : fetch rate
                  browser parameters 
** Created By   : Pavel Guha
** Created On   : 24/02/2021
*******************************************************/
--exec ar_fee_shcedule_params_fetch 27,'11111111-1111-1111-1111-111111111111','',0
create PROCEDURE [dbo].[ar_fee_shcedule_params_fetch] 
    @menu_id int,
    @user_id uniqueidentifier,
	@error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id
	
	select id,name from sys_study_category order by name
	select id,name,invoice_by from modality where is_active='Y' order by name
	select id,name,priority_id= isnull(priority_id,0) from services where is_active='Y' order by name

	exec common_lock_record
		@menu_id       = @menu_id,
		@record_id     = @menu_id,
		@user_id       = @user_id,
		@error_code    = @error_code output,
		@return_status = @return_status output	
						
	if(@return_status=0)
		begin
			return 0
		end

	set nocount off
end


GO
