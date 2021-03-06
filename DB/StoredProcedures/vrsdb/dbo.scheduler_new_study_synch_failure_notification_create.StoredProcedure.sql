USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_new_study_synch_failure_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_new_study_synch_failure_notification_create]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_new_study_synch_failure_notification_create]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_new_study_synch_failure_notification_create : 
                  create case study notifications under 
				  notification rules
** Created By   : Pavel Guha
** Created On   : 27/09/2019
*******************************************************/
--exec scheduler_new_study_synch_failure_notification_create
CREATE procedure [dbo].[scheduler_new_study_synch_failure_notification_create]
	@study_uid nvarchar(100),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
   set nocount on


  declare  @email_subject nvarchar(250),
		   @email_text varchar(8000),
		   @SUPPMAILID nvarchar(200),
		   @email_log_id uniqueidentifier

   declare @MAILSVRUSRCODE nvarchar(100),
           @MAILSVRUSRPWD nvarchar(100)

  select @SUPPMAILID = data_type_string
  from general_settings
  where control_code ='SUPPMAILID' 

  select @MAILSVRUSRCODE = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRCODE'

   select @MAILSVRUSRPWD = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRPWD'


	/******************************************************************************************************************
	 Email Format
	******************************************************************************************************************/
   set @email_subject = 'Failed to synch a study from PACS'

   set @email_text    = 'Study UID : ' + @study_uid +' (UNVIEWED)\n'
   set @email_text    = @email_text + ' could not be synched from PACS into VETRIS \n\n'
   set @email_text    = @email_text + ' One or more fields of this study might have data or data type that is not supported. \n'
   set @email_text    = @email_text + ' So, please log into PACS, serach for this Study UID, open the study in edit mode, check nd rectify the disputing fields.\n'
   set @email_text    = @email_text + ' Save the study and then check whether it synched into vetris.\n'
   set @email_text    = @email_text + '\n\n'
   set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'


   begin transaction 

   if(select count(email_log_id) from vrslogdb..email_log where study_uid=@study_uid and email_type='NSSF')=0
		begin
			set @email_log_id= newid()
			insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,
	                              study_uid,email_type,sender_email_address,sender_email_password)
				        values (@email_log_id,getdate(),@SUPPMAILID,'VETRIS Support',@email_subject,@email_text,
				                @study_uid,'NSSF',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg ='Error creating synch failure notification of Study UID : ' + @study_uid,
						   @return_type =0
					return 0
				end

		end
	

	commit transaction
	select @error_msg ='Synch failure notification of Study UID : ' + @study_uid + ' created',
		   @return_type =1

	set nocount off
	return 1
end

GO
