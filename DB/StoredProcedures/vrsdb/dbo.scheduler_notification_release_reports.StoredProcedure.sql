USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_release_reports]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_release_reports]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_release_reports]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_case_study_notification_create : 
                  create case study notifications under 
				  notification rules
** Created By   : Pavel Guha
** Created On   : 09/02/2021
*******************************************************/
--exec scheduler_notification_release_reports
CREATE procedure [dbo].[scheduler_notification_release_reports]

as
begin
   set nocount on

   declare  @rc int,
			@log_message varchar(8000),
			@error_msg nvarchar(100),
			@return_type int

    update vrslogdb..email_log
	set release_email ='Y'
	where email_type='RPT'
	and release_email='N'
	and study_hdr_id in (select id 
						  from study_hdr
						  where study_status_pacs=100
						  and final_rpt_released='N'
						  and isnull(final_rpt_release_datetime,'01jan1900')<getdate())

	set @rc= @@rowcount
	set @log_message = convert(varchar,@rc) + ' final report email record(s) released'
	set @error_msg=''
	set @return_type=0

	exec scheduler_log_save
		 @is_error    = 0,
		 @service_id  = 4,
		 @log_message = @log_message,
		 @error_msg   = @error_msg,
		 @return_type = @return_type	

    update vrslogdb..sms_log
	set release_sms ='Y'
	where sms_type='RPT'
	and release_sms='N'
	and study_hdr_id in (select id 
						  from study_hdr
						  where study_status_pacs=100
						  and final_rpt_released='N'
						  and isnull(final_rpt_release_datetime,'01jan1900')<getdate())

	set @rc= @@rowcount
	set @log_message = convert(varchar,@rc) + ' final report sms record(s) released'
	set @error_msg=''
	set @return_type=0

	exec scheduler_log_save
		 @is_error    = 0,
		 @service_id  = 4,
		 @log_message = @log_message,
		 @error_msg   = @error_msg,
		 @return_type = @return_type	

	update vrslogdb..fax_log
	set release_fax ='Y'
	where fax_type='RPT'
	and release_fax='N'
	and study_hdr_id in (select id 
						  from study_hdr
						  where study_status_pacs=100
						  and final_rpt_released='N'
						  and isnull(final_rpt_release_datetime,'01jan1900')<getdate())

	set @rc= @@rowcount
	set @log_message = convert(varchar,@rc) + ' final report fax record(s) released'
	set @error_msg=''
	set @return_type=0

	exec scheduler_log_save
		 @is_error    = 0,
		 @service_id  = 4,
		 @log_message = @log_message,
		 @error_msg   = @error_msg,
		 @return_type = @return_type	


	update study_hdr
	set final_rpt_released      = 'Y',
		final_rpt_released_on   = getdate()
	where id  in (select id 
	              from study_hdr
				  where study_status_pacs=100
				  and final_rpt_released='N'
				  and isnull(final_rpt_release_datetime,'01jan1900')<getdate())

	set @rc= @@rowcount
	set @log_message = convert(varchar,@rc) + ' final report(s) released'
	set @error_msg=''
	set @return_type=0

	exec scheduler_log_save
		 @is_error    = 0,
		 @service_id  = 4,
		 @log_message = @log_message,
		 @error_msg   = @error_msg,
		 @return_type = @return_type	


   
   set nocount off
end

GO
