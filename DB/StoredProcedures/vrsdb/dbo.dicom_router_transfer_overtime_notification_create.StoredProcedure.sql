USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_transfer_overtime_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dicom_router_transfer_overtime_notification_create]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_transfer_overtime_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : dicom_router_transfer_overtime_notification_create : create
                  file transfer notification 
** Created By   : Pavel Guha
** Created On   : 16/06/2020
*******************************************************/
CREATE procedure [dbo].[dicom_router_transfer_overtime_notification_create]
	@institution_code nvarchar(5),
	@import_session_id nvarchar(30),
	@imported_file_count int,
	@transfer_file_count int,
	@upload_datetime datetime,
	@time_taken_mins int,
	@output_msg nvarchar(100)='' output,
	@return_status int = 0 output
as
begin
	set nocount on

	declare @SUPPMAILID nvarchar(200),
	        @MAILSVRUSRCODE nvarchar(200),
			@MAILSVRUSRPWD nvarchar(200),
			@institution_name nvarchar(100),
			@count int


    declare @email_subject nvarchar(250),
	        @email_text  varchar(8000)

	select @count = count(import_session_id) from vrslogdb..email_log where import_session_id=@import_session_id


	if(isnull(rtrim(ltrim(@institution_code)),'')<>'' and @count<10)
		begin
				begin transaction
				select @institution_name = name from institutions where code = @institution_code

				select @SUPPMAILID = data_type_string from general_settings where control_code='SUPPMAILID'
				select @MAILSVRUSRCODE = data_type_string from general_settings where control_code='MAILSVRUSRCODE'
				select @MAILSVRUSRPWD = data_type_string from general_settings where control_code='MAILSVRUSRPWD'

				/******************************************************************************************************************
				 Email Format
				******************************************************************************************************************/
			   set @email_subject = 'More time taken for file(s) transfer from ' + @institution_name + ' (' + @institution_code + ') '

			   /******************************************************************************************************************
				 MAIL CREATION
				******************************************************************************************************************/
				set @email_text    = 'File(s) transferring from ' + @institution_name + ' (' + @institution_code + ') is taking more time than expected\n\n'
				set @email_text    = @email_text + 'Session ID                 : ' + @import_session_id + '\n'
				set @email_text    = @email_text + 'No. of file(s) to upload   : ' + convert(varchar(10),@imported_file_count) + '\n'
				set @email_text    = @email_text + 'No. of file(s) transferred : ' + convert(varchar(10),@transfer_file_count) + '\n'
				set @email_text    = @email_text + 'Uploading Started On/At    : ' + convert(varchar,@upload_datetime,107) + ' ' + convert(varchar,@upload_datetime,108) + ' (' + convert(varchar,getdate(),107) + ' ' + convert(varchar,getdate(),108) + ' CST)\n'
				set @email_text    = @email_text + 'Time passed                : ' + convert(varchar,@time_taken_mins) + ' minutes\n\n'
				set @email_text    = @email_text + 'Please monitor this transfer. Please check VETRIS for number of file(s) transfered'
				set @email_text    = @email_text + '\n\n'
				set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

				insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,email_type,
				                      import_session_id,sender_email_address,sender_email_password)
							  values(newid(),getdate(),@SUPPMAILID,'RAD Support',@email_subject,@email_text,'DROT',
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
