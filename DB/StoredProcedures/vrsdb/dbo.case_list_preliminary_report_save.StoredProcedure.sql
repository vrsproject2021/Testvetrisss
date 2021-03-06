USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_preliminary_report_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_preliminary_report_save]
GO
/****** Object:  StoredProcedure [dbo].[case_list_preliminary_report_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_preliminary_report_save : save
                  case preliminary report
** Created By   : Pavel Guha
** Created On   : 20/04/2019
*******************************************************/
CREATE procedure [dbo].[case_list_preliminary_report_save]
    @study_id uniqueidentifier,
	@study_uid nvarchar(100),
	@report_text ntext,
	@updated_by uniqueidentifier,
	@menu_id int,
	@user_name nvarchar(130)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	exec common_check_record_lock_ui
		@menu_id       = @menu_id,
		@record_id     = @study_id,
		@user_id       = @updated_by,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end
	
	begin transaction

	update study_hdr_prelim_reports
	set report_text  = @report_text,
		updated_by   = @updated_by,
		date_updated = getdate()
	where study_hdr_id = @study_id 
	and study_uid =@study_uid

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_status=0,@error_code='059'
			return 0
		end

	
	commit transaction
	set @return_status=1
	set @error_code='058'
	set nocount off
	return 1

end


GO
