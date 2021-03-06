USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_upload_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dicom_router_upload_notification_create]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_upload_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : dicom_router_upload_notification_create : create
                  file upload notification 
** Created By   : Pavel Guha
** Created On   : 08/06/2020
*******************************************************/
CREATE procedure [dbo].[dicom_router_upload_notification_create]
	@institution_code nvarchar(5),
	@import_session_id nvarchar(30),
	@file_count int,
	@proc_start_datetime datetime,
	@output_msg nvarchar(100)='' output,
	@return_status int = 0 output
as
begin
	set nocount on

	declare @SUPPMAILID nvarchar(200),
	        @MAILSVRUSRCODE nvarchar(200),
			@MAILSVRUSRPWD nvarchar(200),
			@institution_name nvarchar(100)

    declare @email_subject nvarchar(250),
	        @email_text  varchar(8000)


	if(isnull(rtrim(ltrim(@institution_code)),'')<>'')
		begin
				begin transaction
				select @institution_name = name from institutions where code = @institution_code

				select @SUPPMAILID = data_type_string from general_settings where control_code='SUPPMAILID'
				select @MAILSVRUSRCODE = data_type_string from general_settings where control_code='MAILSVRUSRCODE'
				select @MAILSVRUSRPWD = data_type_string from general_settings where control_code='MAILSVRUSRPWD'
				
				/******************************************************************************************************************
				 Email Format
				******************************************************************************************************************/
			   set @email_subject = 'Uploading of file(s) from ' + @institution_name + ' (' + @institution_code + ') started'

			   /******************************************************************************************************************
				 MAIL CREATION
				******************************************************************************************************************/
				set @email_text    = 'Uploading of file(s) started from ' + @institution_name + ' (' + @institution_code + ')\n\n'
				set @email_text    = @email_text + 'No. of files uploaded: ' + convert(varchar(10),@file_count) + '\n'
				set @email_text    = @email_text + 'Session ID           : ' + @import_session_id + '\n'
				set @email_text    = @email_text + 'Upload Started On/At : ' +  convert(varchar,@proc_start_datetime,107) + ' ' + convert(varchar,@proc_start_datetime,108) +  '(' + convert(varchar,getdate(),107) + ' ' + convert(varchar,getdate(),108) + ' CST)\n'
				set @email_text    = @email_text + 'Please monitor the download process at VETRIS.\n'
				set @email_text    = @email_text + '\n\n'
				set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

				insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,email_type,
				                      sender_email_address,sender_email_password)
							  values(newid(),getdate(),@SUPPMAILID,'RAD Support',@email_subject,@email_text,'DR',
							         @MAILSVRUSRCODE,@MAILSVRUSRPWD)

				if(@@rowcount =0)
					begin
						rollback transaction
						select @output_msg='FAILURE', @return_status=0
					end

			    commit transaction
		end

	
	select @output_msg='SUCCESS', @return_status=1

	set nocount off
end
GO
