USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_institution_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_institution_create]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_institution_create]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_notification_institution_create : 
                  create service restart notification
** Created By   : Pavel Guha
** Created On   : 28/03/2020
*******************************************************/
--exec scheduler_file_xfer_fail_notification_create
CREATE procedure [dbo].[scheduler_notification_institution_create]
	@institution_name nvarchar(100),
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
		   @service_name nvarchar(50),
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
   set @email_subject_format = 'New Institution ''' + @institution_name + ''' created, Study sync pending'

   /******************************************************************************************************************
	 MAIL CREATION
	******************************************************************************************************************/

	set @email_text    = 'New institution  ''' + upper(@institution_name) + ''' has been created while synching studies from DICOM Listener.\n\n'
	set @email_text    = @email_text + 'Please check VETRIS for the institution.\n'
	set @email_text    = @email_text + 'Check whether this institution is required to link to an existing institution or it has to be configured as a new institution\n'
	set @email_text    = @email_text + 'Studies are pending to be synched for this institution.\n'
	set @email_text    = @email_text + '\n\n'
	set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

	

	if(rtrim(ltrim(@institution_name))<>'')
		begin
			begin transaction
			insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,institution_name,email_type,sender_email_address,sender_email_password)
						  (select newid(),getdate(),@SUPPMAILID,'RAD Support',@email_subject_format,@email_text,@institution_name,'NEWINST',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

			if(@@rowcount = 0)
				begin
					rollback transaction
					set @error_msg='Failed to create notification for createion of new institution'
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
