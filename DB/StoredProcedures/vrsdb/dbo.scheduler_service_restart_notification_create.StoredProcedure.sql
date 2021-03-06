USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_service_restart_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_service_restart_notification_create]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_service_restart_notification_create]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_service_restart_notification_create : 
                  create service restart notification
** Created By   : Pavel Guha
** Created On   : 28/03/2020
*******************************************************/
--exec scheduler_file_xfer_fail_notification_create
CREATE procedure [dbo].[scheduler_service_restart_notification_create]
	@service_id int,
	@restart_reason nvarchar(max)=null,
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

  select @service_name = service_name
  from scheduler_data_services
  where service_id = @service_id

  select @MAILSVRUSRCODE = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRCODE'

   select @MAILSVRUSRPWD = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRPWD'

	/******************************************************************************************************************
	 Email Format
	******************************************************************************************************************/
   set @email_subject_format = 'Restart service ''' + @service_name + ''''

   /******************************************************************************************************************
	 MAIL CREATION
	******************************************************************************************************************/

	set @email_text    = 'Please restart the service  ''' + @service_name + '''\n\n'
	set @email_text    = @email_text + 'Reason for restart: \n'
	set @email_text    = @email_text + replicate('=',20) + '\n'
	set @email_text    = @email_text + @restart_reason  + '\n'
	set @email_text    = @email_text + '\n\n'
	set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

	begin transaction

	insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,email_type,sender_email_address,sender_email_password)
			      (select newid(),getdate(),@SUPPMAILID,'RAD Support',@email_subject_format,@email_text,'SVCSTART',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

	if(@@rowcount = 0)
		begin
			rollback transaction
			set @error_msg='Failed to create notification for restart of service'
			set @return_type=0
			return 0
		end

	commit transaction


	set @error_msg=''
	set @return_type=1
	set nocount off
	return 1

end

GO
