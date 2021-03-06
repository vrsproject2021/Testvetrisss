USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_transcription_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_transcription_save]
GO
/****** Object:  StoredProcedure [dbo].[case_list_transcription_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_transcription_save : save
                  transcription
** Created By   : Pavel Guha
** Created On   : 09/09/2020
*******************************************************/
CREATE procedure [dbo].[case_list_transcription_save]
	@id uniqueidentifier ='00000000-0000-0000-0000-000000000000' output,
    @study_hdr_id uniqueidentifier,
	@report_text ntext=null,
	@report_text_html ntext=null,
	@translated_report_text ntext=null,
	@translated_report_text_html ntext=null,
	@updated_by uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier ='00000000-0000-0000-0000-000000000000',
	@user_name nvarchar(130)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	declare @transcripstionist_id uniqueidentifier

    declare @activity_text nvarchar(max)
	exec common_check_record_lock_ui
		@menu_id       = @menu_id,
		@record_id     = @study_hdr_id,
		@user_id       = @updated_by,
		@session_id    = @session_id,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			return 0
		end


	begin transaction

	select @transcripstionist_id= isnull(id,'00000000-0000-0000-0000-000000000000')
	from transciptionists
	where login_user_id =@updated_by

	update study_hdr_dictated_reports
	set trans_report_text          = @report_text,
		trans_report_text_html     = @report_text_html,
		translate_report_text      = @translated_report_text,
		translate_report_text_html = @translated_report_text_html,
		pacs_wb                    = 'Y',
		transcribed_by             = @updated_by,
		date_transcribed           = getdate()
	where study_hdr_id = @study_hdr_id 

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_status=0,@error_code='035'
			return 0
		end

	update study_hdr
	set dict_tanscriptionist_id     = @transcripstionist_id
	where id = @study_hdr_id 

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_status=0,@error_code='035'
			return 0
		end

	exec common_study_user_activity_trail_save
		@study_hdr_id = @study_hdr_id,
		@study_uid    ='',
		@menu_id      = @menu_id,
		@activity_text = 'Report Transcribed',
		@session_id    = @session_id,
		@activity_by   = @updated_by,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
	begin
		rollback transaction
		return 0
	end
		
	commit transaction
	set @return_status=1
	set @error_code='034'
	set nocount off
	return 1	

end


GO
