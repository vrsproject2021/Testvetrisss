USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[notification_study_file_sync_pending_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[notification_study_file_sync_pending_create]
GO
/****** Object:  StoredProcedure [dbo].[notification_study_file_sync_pending_create]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : notification_study_file_sync_pending_create : 
                  create notification for study file sync pending
** Created By   : Pavel Guha
** Created On   : 01/06/2021
*******************************************************/

CREATE procedure [dbo].[notification_study_file_sync_pending_create]
    @id uniqueidentifier,
	@is_image nchar(1)='N',
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	
	declare @study_uid nvarchar(100),
	        @institution_name nvarchar(100),
			@patient_fname nvarchar(40),
	        @patient_lname nvarchar(40)

   declare @email_subject nvarchar(250),
		   @email_text varchar(8000),
		   @SUPPMAILID nvarchar(200),
		   @SENDLFTOPACS nvarchar(200),
		   @SUPPFWDSYNMAILID nvarchar(200),
		   @MAILSVRUSRCODE nvarchar(200),
		   @MAILSVRUSRPWD nvarchar(200),
		   @email_log_id uniqueidentifier

	if(select count(email_log_id) from vrslogdb..email_log where study_hdr_id = @id and study_uid=@study_uid and email_type='SYFWDND')=0
		begin
		     select @SUPPMAILID = data_type_string
			 from general_settings
			 where control_code='SUPPMAILID'

			 select @SUPPFWDSYNMAILID = data_type_string
			 from general_settings
			 where control_code='SUPPFWDSYNMAILID'

			 select @SENDLFTOPACS = data_type_string
			 from general_settings
			 where control_code='SENDLFTOPACS'

			 select @MAILSVRUSRCODE = data_type_string
			 from general_settings
			 where control_code ='MAILSVRUSRCODE'

			 select @MAILSVRUSRPWD = data_type_string
			 from general_settings
			 where control_code ='MAILSVRUSRPWD'

			 if(@is_image='N')
				begin
					 select @study_uid        = sh.study_uid,
							@patient_fname    = isnull(sh.patient_fname,''),
							@patient_lname    = isnull(sh.patient_lname,''),
							@institution_name = isnull(name,'')
					 from study_hdr sh
					 left outer join institutions i on i.id = sh.institution_id
					 where sh.id=@id
				end
			else
				begin
					select  @study_uid        = sh.study_uid,
							@patient_fname    = isnull(sh.patient_fname,''),
							@patient_lname    = isnull(sh.patient_lname,''),
							@institution_name = isnull(name,'')
					 from scheduler_img_file_downloads_grouped sh
					 left outer join institutions i on i.id = sh.institution_id
					 where sh.id=@id
				end
		    
			/******************************************************************************************************************
				Email Format
			******************************************************************************************************************/
			set @email_subject = 'Study file(s) forwarding from PACS pending for ' + isnull(@patient_fname,'') + ' ' + isnull(@patient_lname,'') + ' of ' + isnull(@institution_name,'')

			set @email_text    = 'Study Details :- \n\n'
			set @email_text    = @email_text + ' Patient Name       : ' + isnull(@patient_fname,'') + ' ' + isnull(@patient_lname,'') + '\n'
			set @email_text    = @email_text + ' Study UID          : ' + isnull(@study_uid,'') +'\n'
			set @email_text    = @email_text + ' Institution Name   : ' + isnull(@institution_name,'') + '\n'
			set @email_text    = @email_text + '\n\n'
			set @email_text    = @email_text + 'Please check whether it is available in both eRAD \n'
			set @email_text    = @email_text + '\n\n'
			set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

		    set @email_log_id=newid()	
			insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,
								  study_hdr_id,study_uid,email_type,sender_email_address,sender_email_password)
							values(@email_log_id,getdate(),@SUPPFWDSYNMAILID,'RAD Support',@email_subject,@email_text,
								@id,@study_uid,'SYFWDND',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

			if(@@rowcount = 0)
				begin
					
					set @error_msg='Failed to create notification for pending study image forwarding for ' + @patient_fname + ' ' + @patient_lname + ' of ' + @institution_name
					set @return_type=0
					return 0
				end
		end

	select @error_msg='',@return_type=1

	return 1

end


GO
