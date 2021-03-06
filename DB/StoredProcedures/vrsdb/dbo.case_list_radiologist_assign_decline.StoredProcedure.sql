USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_radiologist_assign_decline]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_radiologist_assign_decline]
GO
/****** Object:  StoredProcedure [dbo].[case_list_radiologist_assign_decline]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_radiologist_assign_decline : release
                  case from list
** Created By   : Pavel Guha
** Created On   : 15/05/2021
*******************************************************/
CREATE procedure [dbo].[case_list_radiologist_assign_decline]
    @study_id uniqueidentifier,
	@updated_by uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on
	declare @study_uid nvarchar(100),
	        @activity_text nvarchar(max),
	        @dict_radiologist_id uniqueidentifier,
			@schedule_date date,
			@modality_id int,
			@category_id int

	declare @email_subject_format nvarchar(250),
		    @email_text_format  varchar(8000)

   declare @MAILSVRUSRCODE nvarchar(100),
           @MAILSVRUSRPWD nvarchar(100),
		   @SUPPSYUASMAILID nvarchar(200)

   declare  @radiologist_name nvarchar(200),
		    @modality_name nvarchar(30),
            @institution_name nvarchar(100),
		    @synched_on datetime,
		    @study_date datetime,
		    @patient_name nvarchar(250),
		    @time_left_mins int,
		    @priority_name nvarchar(30)

  select @study_uid   = study_uid ,
	     @modality_id = modality_id,
		 @category_id = category_id
  from study_hdr 
  where id=@study_id

  select @dict_radiologist_id = id,
	     @radiologist_name    = name
  from radiologists 
  where login_user_id=@updated_by

   if(select count(id) from study_hdr where id=@study_id and study_uid=@study_uid)=0
	begin
		select @return_status=0,@error_code='430'
		return 0
	end

   if(isnull((select radiologist_id from study_hdr where id=@study_id and study_uid=@study_uid),'00000000-0000-0000-0000-000000000000'))<> isnull(@dict_radiologist_id,'00000000-0000-0000-0000-000000000000')
	begin
		select @return_status=0,@error_code='479'
		return 0
	end

	begin transaction

	update study_hdr
	set assign_accepted       = 'N',
		dict_radiologist_id   = '00000000-0000-0000-0000-000000000000',
		dict_radiologist_pacs = '',
		radiologist_id        = '00000000-0000-0000-0000-000000000000',
		radiologist_pacs      = '',
		manually_assigned     = 'N',
		pacs_wb               = 'Y',
		rad_assigned_on       = null
	where id      = @study_id
	and study_uid = @study_uid

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_status=0,@error_code='478'
			return 0
		end

   if(select count(radiologist_id) from vrslogdb..radiologist_assignment_log where study_hdr_id=@study_id and study_uid=@study_uid and radiologist_id=@dict_radiologist_id) >0
	begin
		select top 1 @schedule_date = scheduled_date
		from vrslogdb..radiologist_assignment_log
		where study_hdr_id=@study_id 
		and study_uid=@study_uid 
		and radiologist_id=@dict_radiologist_id
	end
  else
	begin
		select @schedule_date = convert(date,synched_on)
		from study_hdr
		where id      = @study_id 
		and study_uid = @study_uid 
	end

	insert into vrslogdb..radiologist_assignment_release_log(study_hdr_id,study_uid,scheduled_date,radiologist_id,modality_id,category_id,date_released)
	                                        values(@study_id,@study_uid,@schedule_date,@dict_radiologist_id,@modality_id,@category_id,getdate())

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_status=0,@error_code='478'
			return 0
		end

	set @activity_text = 'Assignment released'
	exec common_study_user_activity_trail_save
		@study_hdr_id  = @study_id,
		@study_uid     = @study_uid,
		@menu_id       = @menu_id,
		@activity_text = @activity_text,
		@session_id    = @session_id,
		@activity_by   = @updated_by,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end

	/******************************************************************************************************************
	 Generate email notification
	******************************************************************************************************************/
   select @MAILSVRUSRCODE = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRCODE'

   select @MAILSVRUSRPWD = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRPWD'

   select @SUPPSYUASMAILID = data_type_string
   from general_settings
   where control_code ='SUPPSYUASMAILID'

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
   where sh.id = @study_id
   and sh.study_uid = @study_uid

   set @email_subject_format = 'Assignment of the study of the patient ' + @patient_name + ' has been released'

   set @email_text_format    = 'Assignment of the study of the patient ' + @patient_name + ' has been released by ' + @radiologist_name + ' \n\n'
   set @email_text_format    = @email_text_format + 'Summary of study :- \n\n'
   set @email_text_format    = @email_text_format + ' Study Date/Time    : ' + convert(varchar(10),@study_date,101) + ' ' + convert(varchar(5),@study_date,114) + ' \n'
   set @email_text_format    = @email_text_format + ' Received Date/Time : ' + convert(varchar(10),@synched_on,101) + ' ' + convert(varchar(5),@synched_on,114) + ' \n'
   set @email_text_format    = @email_text_format + ' Institution        : ' + @institution_name + ' \n'
   set @email_text_format    = @email_text_format + ' Patient            : ' + @patient_name + ' \n'
   set @email_text_format    = @email_text_format + ' Modality           : ' + @modality_name + ' \n'
   set @email_text_format    = @email_text_format + ' Priority           : ' + @priority_name + ' \n'
   set @email_text_format    = @email_text_format + ' Time Left          : ' + convert(varchar,@time_left_mins) + ' minutes\n'
   set @email_text_format    = @email_text_format + '\n\n'
   set @email_text_format    = @email_text_format +'This is an automated message from VETRIS.Please do not reply to the message.\n'

 
	insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,
							study_hdr_id,study_uid,email_type,sender_email_address,sender_email_password)
					values(newid(),getdate(),@SUPPSYUASMAILID,'',@email_subject_format,@email_text_format,
						   @study_id,@study_uid,'ASNREL',@MAILSVRUSRCODE,@MAILSVRUSRPWD)

		
	set @activity_text =  'Email for the release of study asignment has been queued for sending'
	exec common_study_user_activity_trail_save
		@study_hdr_id  = @study_id,
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
	set @return_status=1
	set @error_code=''
	set nocount off
	return 1

end


GO
