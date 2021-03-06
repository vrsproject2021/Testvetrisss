USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_send_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_send_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_send_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 
** Procedure    : scheduler_settings_fetch : fetch scheduler settings
** Created By   : Pavel Guha
** Created On   : 01/05/2019
*******************************************************/
-- exec scheduler_notification_send_fetch 
CREATE procedure [dbo].[scheduler_notification_send_fetch]

as
begin
	set nocount on
	create table #tmpEmail
	(
		email_log_id uniqueidentifier,
		email_log_datetime datetime,
		recipient_address nvarchar(500),
		recipient_name nvarchar(100),
		cc_address varchar(max),
		email_subject varchar(250),
		email_text ntext,
		study_hdr_id uniqueidentifier,
		study_uid nvarchar(100),
		sender_email_address nvarchar(100),
		sender_email_password nvarchar(100),
		file_name nvarchar(4000),
		email_type nvarchar(100),
		is_custom_rpt nchar(1) default 'N',
		patient_name nvarchar(100) null default ''
	)

	insert into #tmpEmail(email_log_id,email_log_datetime,recipient_address,recipient_name,cc_address,
	                     email_subject,email_text,study_hdr_id,study_uid,sender_email_address,sender_email_password,file_name,email_type)
	(select  email_log_id , email_log_datetime , recipient_address , recipient_name ,  cc_address=isnull(cc_address,''),
		    email_subject , email_text , 
			study_hdr_id= isnull(study_hdr_id,'00000000-0000-0000-0000-000000000000'),  
			study_uid = isnull(study_uid,''), 
			sender_email_address,sender_email_password,file_name= isnull(file_name,''),email_type
	from vrslogdb..email_log 
	where email_processed='N' 
	and email_type<>'RPT')
	
	insert into #tmpEmail(email_log_id,email_log_datetime,recipient_address,recipient_name,cc_address,
	                     email_subject,email_text,study_hdr_id,study_uid,sender_email_address,sender_email_password,file_name,email_type,is_custom_rpt,patient_name)
	(select el.email_log_id , el.email_log_datetime , el.recipient_address , el.recipient_name , cc_address=isnull(el.cc_address,''),
		    el.email_subject , el.email_text , 
			study_hdr_id= isnull(el.study_hdr_id,'00000000-0000-0000-0000-000000000000'),  
			study_uid = isnull(el.study_uid,''), 
			el.sender_email_address,el.sender_email_password,file_name= isnull(el.file_name,''),el.email_type,i.custom_report,
			patient_name = isnull(sh.patient_name,'')
	from vrslogdb..email_log el
	inner join study_hdr sh on sh.id = el.study_hdr_id
	inner join institutions i on i.id = sh.institution_id
	where el.email_processed='N' 
	and el.email_type='RPT'
	and el.release_email='Y')

	insert into #tmpEmail(email_log_id,email_log_datetime,recipient_address,recipient_name,cc_address,
	                     email_subject,email_text,study_hdr_id,study_uid,sender_email_address,sender_email_password,file_name,email_type,is_custom_rpt,patient_name)
	(select el.email_log_id , el.email_log_datetime , el.recipient_address , el.recipient_name , cc_address=isnull(el.cc_address,''),
		    el.email_subject , el.email_text , 
			study_hdr_id= isnull(el.study_hdr_id,'00000000-0000-0000-0000-000000000000'),  
			study_uid = isnull(el.study_uid,''), 
			el.sender_email_address,el.sender_email_password,file_name= isnull(el.file_name,''),el.email_type,i.custom_report,
			patient_name = isnull(sh.patient_name,'')
	from vrslogdb..email_log el
	inner join study_hdr_archive sh on sh.id = el.study_hdr_id
	inner join institutions i on i.id = sh.institution_id
	where el.email_processed='N' 
	and el.email_type='RPT'
	and el.release_email='Y')

	select * from #tmpEmail order by email_log_datetime

    select  sms_log_id , sms_log_datetime , recipient_no , recipient_name ,  
		    sequence_no , sms_text , study_hdr_id= isnull(study_hdr_id,'00000000-0000-0000-0000-000000000000'), 
			study_uid = isnull(study_uid,'')
	from vrslogdb..sms_log 
	where sms_processed='N' 
	and attempts<3
	and sms_type<>'RPT'
	union
	select  sms_log_id , sms_log_datetime , recipient_no , recipient_name ,  
		    sequence_no , sms_text , study_hdr_id= isnull(study_hdr_id,'00000000-0000-0000-0000-000000000000'), 
			study_uid = isnull(study_uid,'')
	from vrslogdb..sms_log 
	where sms_processed='N' 
	and attempts<3
	and sms_type='RPT'
	and release_sms='Y'
	order by sms_log_datetime,sequence_no

	select  id, log_datetime,recipient_no, 
	study_hdr_id= isnull(study_hdr_id,'00000000-0000-0000-0000-000000000000'),
	study_uid = isnull(study_uid,''),file_name,
	report_type,custom_report
	from vrslogdb..fax_log
	where fax_sent='N' 
	and fax_type<>'RPT'
	union
	select  id, log_datetime,recipient_no, 
	study_hdr_id= isnull(study_hdr_id,'00000000-0000-0000-0000-000000000000'),
	study_uid = isnull(study_uid,''),file_name,
	report_type,custom_report
	from vrslogdb..fax_log
	where fax_sent='N' 
	and fax_type='RPT'
	and release_fax='Y'
	order by log_datetime

set nocount off
end

GO
