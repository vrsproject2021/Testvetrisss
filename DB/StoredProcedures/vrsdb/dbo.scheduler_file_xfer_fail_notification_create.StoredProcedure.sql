USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_file_xfer_fail_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_file_xfer_fail_notification_create]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_file_xfer_fail_notification_create]    Script Date: 28-09-2021 19:36:35 ******/
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
** Created On   : 27/09/2019
*******************************************************/
--exec scheduler_file_xfer_fail_notification_create
CREATE procedure [dbo].[scheduler_file_xfer_fail_notification_create]
	@study_uid nvarchar(100),
	@institution_code nvarchar(5),
	@institution_name nvarchar(100),
	@file_name nvarchar(100),
	@failure_reason nvarchar(max)=null,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
   set nocount on
  

  declare  @email_subject_format nvarchar(250),
		   @email_text_format  varchar(8000),
		   @email_subject nvarchar(250),
		   @email_text varchar(8000),
		   @SUPPMAILID nvarchar(200),
		   @MAILSVRUSRCODE nvarchar(100),
           @MAILSVRUSRPWD nvarchar(100)


  select @SUPPMAILID = data_type_string
  from general_settings
  where control_code='SUPPMAILID'

  select @MAILSVRUSRCODE = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRCODE'

   select @MAILSVRUSRPWD = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRPWD'

	/******************************************************************************************************************
	 Email Format
	******************************************************************************************************************/
   set @email_subject_format = 'File transfer to PACS failed'

   set @email_text_format    = 'File Details :- \n\n'
   set @email_text_format    = @email_text_format + ' File Name          : [FILE_NAME] \n'
   set @email_text_format    = @email_text_format + ' Study UID          : [STUDY_UID] \n'
   set @email_text_format    = @email_text_format + ' Institution Code   : [INST_CODE] \n'
   set @email_text_format    = @email_text_format + ' Institution Name   : [INST_NAME] \n'
   set @email_text_format    = @email_text_format + '\n\n'
   set @email_text_format    = @email_text_format + 'Reason For Failure: \n'
   set @email_text_format    = @email_text_format + replicate('=',20) + '\n'
   set @email_text_format    = @email_text_format + ' [REASON] \n'
   set @email_text_format    = @email_text_format + '\n\n'
   set @email_text_format    = @email_text_format +'This is an automated message from VETRIS.Please do not reply to the message.\n'

   /******************************************************************************************************************
	 MAIL CREATION
	******************************************************************************************************************/

	if(select count(email_log_id) from vrslogdb..email_log where email_type='XFERFAIL' and study_uid=@study_uid and isnull(file_name,'')= @file_name)=0
		begin
			set @email_text = @email_text_format
			set @email_text = replace(@email_text,'[FILE_NAME]',@file_name)
			set @email_text = replace(@email_text,'[STUDY_UID]',@study_uid)
			set @email_text = replace(@email_text,'[INST_CODE]',@institution_code)
			set @email_text = replace(@email_text,'[INST_NAME]',@institution_name)
			set @email_text = replace(@email_text,'[REASON]',isnull(@failure_reason,''))

			begin transaction

			insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,
	                              study_uid,email_type,file_name,sender_email_address,sender_email_password)
				    (select newid(),getdate(),@SUPPMAILID,'RAD Support',@email_subject_format,@email_text,
				            @study_uid,'XFERFAIL',@file_name,@MAILSVRUSRCODE,@MAILSVRUSRPWD)

			if(@@rowcount = 0)
				begin
					rollback transaction
					set @error_msg='Failed to create notification for file transfer to PACS failure'
					set @return_type=0
					return 0
				end

			commit transaction
		end

	set @error_msg=''
	set @return_type=1
	set nocount off
	return 1

end

GO
