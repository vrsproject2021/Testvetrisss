USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_unassigned_study_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_unassigned_study_notification_create]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_unassigned_study_notification_create]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_notification_unassigned_study_notification_create : 
                  create case study notifications not assined in thresh hold time
** Created By   : Pavel Guha
** Created On   : 24/05/2021
*******************************************************/
--exec scheduler_notification_rule_notification_create
CREATE procedure [dbo].[scheduler_notification_unassigned_study_notification_create]
	@id uniqueidentifier,
	@study_uid nvarchar(100)
as
begin
   set nocount on

   declare  @email_subject_format nvarchar(250),
		    @email_text_format  varchar(8000)

  declare @MAILSVRUSRCODE nvarchar(100),
          @MAILSVRUSRPWD nvarchar(100),
		  @SUPPSYUASMAILID nvarchar(200)

  declare  @modality_name nvarchar(30),
           @institution_name nvarchar(100),
		   @synched_on datetime,
		   @study_date datetime,
		   @patient_name nvarchar(250),
		   @time_left_mins int,
		   @priority_name nvarchar(30)

  declare @error_code nvarchar(10),
		  @return_status int,
		  @activity_text nvarchar(max)
   

   select @MAILSVRUSRCODE = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRCODE'

   select @MAILSVRUSRPWD = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRPWD'

   select @SUPPSYUASMAILID = data_type_string
   from general_settings
   where control_code ='@SUPPSYUASMAILID'

   select @modality_name    = isnull(m.name,'Unknown'),
          @institution_name = isnull(i.name,'Unknown'),
		  @priority_name    = isnull(p.priority_desc,'Unknown'),
		  @patient_name     = rtrim(ltrim(isnull(patient_fname,'') + ' ' + isnull(patient_lname,''))),
		  @study_date       = sh.study_date,
		  @synched_on       = sh.synched_on,
		  @time_left_mins   = datediff(mi,getdate(),sh.finishing_datetime)
   from study_hdr sh
   left outer join modality m on m.id = sh.modality_id
   left outer join institutions i on i.id = sh.institution_id
   left outer join sys_priority p on p.priority_id = sh.priority_id
   where sh.id = @id
   and sh.study_uid = @study_uid

	/******************************************************************************************************************
	 Email Format
	******************************************************************************************************************/
   set @email_subject_format = 'Study of the patient ' + @patient_name + ' is still UNASSIGNED'

   set @email_text_format    = 'Summary of study :- \n\n'
   set @email_text_format    = @email_text_format + ' Study Date/Time    : ' + convert(varchar(10),@study_date,101) + ' ' + convert(varchar(5),@study_date,114) + ' \n'
   set @email_text_format    = @email_text_format + ' Received Date/Time : ' + convert(varchar(10),@synched_on,101) + ' ' + convert(varchar(5),@synched_on,114) + ' \n'
   set @email_text_format    = @email_text_format + ' Institution        : ' + @institution_name + ' \n'
   set @email_text_format    = @email_text_format + ' Patient            : ' + @patient_name + ' \n'
   set @email_text_format    = @email_text_format + ' Modality           : ' + @modality_name + ' \n'
   set @email_text_format    = @email_text_format + ' Priority           : ' + @priority_name + ' \n'
   set @email_text_format    = @email_text_format + ' Time Left          : ' + convert(varchar,@time_left_mins) + ' minutes\n'
   set @email_text_format    = @email_text_format + '\n\n'
   set @email_text_format    = @email_text_format +'This is an automated message from VETRIS.Please do not reply to the message.\n'
	
	/******************************************************************************************************************
	 FINAL CREATION OF NOTIFICATION
	******************************************************************************************************************/

	if(select count(email_log_id) from vrslogdb..email_log where email_type='UNASNSY' and study_hdr_id=@id and study_uid=@study_uid)>0
		begin
			begin transaction
			insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,
								  study_hdr_id,study_uid,email_type,sender_email_address,sender_email_password)
						 values(newid(),getdate(),@SUPPSYUASMAILID,'',@email_subject_format,@email_text_format,
								 @id,@study_uid,'UNASNSY',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

		
			set @activity_text =  'Email for the study remaining unassigned after thesh hold time queued'
			exec common_study_user_activity_trail_save
				@study_hdr_id  = @id,
				@study_uid     = @study_uid,
				@menu_id       = 0,
				@activity_text = @activity_text,
				@activity_by   = '00000000-0000-0000-0000-000000000000',
				@error_code    = @error_code output,
				@return_status = @return_status output

			if(@return_status=0)
				begin
					rollback transaction
					return 0
				end

			commit transaction
		end
	
	set nocount off
end

GO
