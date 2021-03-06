USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_mail_send_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_mail_send_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_mail_send_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 
** Procedure    : scheduler_settings_fetch : fetch scheduler settings
** Created By   : Pavel Guha
** Created On   : 21/04/2019
*******************************************************/
-- exec scheduler_mail_send_fetch 
CREATE procedure [dbo].[scheduler_mail_send_fetch]

as
begin
	select  email_log_id , email_log_datetime , recipient_address , recipient_name , cc_address = isnull(cc_address,''),
		    email_subject , email_text , study_hdr_id , study_uid,file_name= isnull(file_name,'')
	from vrslogdb..email_log 
	where email_processed='N' 
	order by email_log_datetime
end

GO
