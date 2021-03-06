USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_transfer_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dicom_router_transfer_notification_create]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_transfer_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : dicom_router_transfer_notification_create : create
                  file transfer notification 
** Created By   : Pavel Guha
** Created On   : 08/06/2020
*******************************************************/
CREATE procedure [dbo].[dicom_router_transfer_notification_create]
	@institution_code nvarchar(5),
	@import_session_id nvarchar(30),
	@file_count int,
	@upload_datetime datetime,
	@download_datetime datetime,
	@output_msg nvarchar(100)='' output,
	@return_status int = 0 output
as
begin
	set nocount on

	declare @SUPPMAILID nvarchar(200),
	        @MAILSVRUSRCODE nvarchar(200),
			@MAILSVRUSRPWD nvarchar(200),
			@institution_name nvarchar(100)

	declare @time_taken_mins int,
	        @time_taken_secs int

    declare @email_subject nvarchar(250),
	        @email_text  varchar(8000)


	if(isnull(rtrim(ltrim(@institution_code)),'')<>'')
		begin
				begin transaction
				select @institution_name = name from institutions where code = @institution_code

				select @SUPPMAILID = data_type_string from general_settings where control_code='SUPPMAILID'
				select @MAILSVRUSRCODE = data_type_string from general_settings where control_code='MAILSVRUSRCODE'
				select @MAILSVRUSRPWD = data_type_string from general_settings where control_code='MAILSVRUSRPWD'

				set @time_taken_mins = datediff(MI,@upload_datetime,@download_datetime)
				set @time_taken_secs = datediff(S,@upload_datetime,@download_datetime)

				
				/******************************************************************************************************************
				 Email Format
				******************************************************************************************************************/
			   set @email_subject = 'File(s) transfered from ' + @institution_name + ' (' + @institution_code + ')'

			   /******************************************************************************************************************
				 MAIL CREATION
				******************************************************************************************************************/
				set @email_text    = 'File(s) transfered from ' + @institution_name + ' (' + @institution_code + ')\n\n'
				set @email_text    = @email_text + 'No. of files transfered: ' + convert(varchar(10),@file_count) + '\n'
				set @email_text    = @email_text + 'Session ID             : ' + @import_session_id + '\n'
				set @email_text    = @email_text + 'Uploading Started On/At: ' + convert(varchar,@upload_datetime,107) + ' ' + convert(varchar,@upload_datetime,108)+'\n'
				set @email_text    = @email_text + 'Downloaded On/At       : ' + convert(varchar,@download_datetime,107) + ' ' + convert(varchar,@download_datetime,108) + '\n'
				
				
				if(isnull(@time_taken_mins,0)>0)
					begin
						set @email_text    = @email_text + 'Time Taken             : ' + convert(varchar,@time_taken_mins) + ' minutes\n'
					end
				else
					begin
						set @email_text    = @email_text + 'Time Taken             : ' + convert(varchar,@time_taken_secs) + ' seconds\n'
					end
				
				set @email_text    = @email_text + '\n\n'
				set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

				insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,email_type,
				                      import_session_id,sender_email_address,sender_email_password)
							  values(newid(),getdate(),@SUPPMAILID,'RAD Support',@email_subject,@email_text,'DR',
							         @import_session_id,@MAILSVRUSRCODE,@MAILSVRUSRPWD)

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
