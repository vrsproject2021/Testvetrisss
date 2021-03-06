USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_rule_notification_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_notification_rule_notification_create]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_notification_rule_notification_create]    Script Date: 28-09-2021 19:36:35 ******/
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
--exec scheduler_notification_rule_notification_create
CREATE procedure [dbo].[scheduler_notification_rule_notification_create]
	@email_count int = 0 output,
	@sms_count int = 0 output
as
begin
   set nocount on
   declare @study_hdr_id uniqueidentifier,
		   @study_uid nvarchar(100),
		   @study_status_pacs int,
		   @status_name nvarchar(30),
		   @priority_id int,
		   @priority_name nvarchar(30),
		   @time_ellapsed_mins int,
		   @time_left_mins int,
		   @notify_by_time nchar(1),
		   @modality_id int,
		   @modality_code nvarchar(5),
		   @modality_name nvarchar(30),
		   @species_id int,
		   @species_name nvarchar(30),
		   @service_codes nvarchar(250),
		   @study_type_id uniqueidentifier,
		   @institution_id uniqueidentifier,
		   @institution_name nvarchar(100),
		   @received_date datetime,
		   @synched_on datetime,
		   @patient_name nvarchar(250),
		   @rule_no int,
		   @queue_name nvarchar(30),
		   @scheduled nchar(1),
		   @institution_users nchar(1),
		   @institution_user_role_id int,
		   @submitted_date datetime

   declare  @email_subject_format nvarchar(250),
		    @email_text_format  varchar(8000),
		    @sms_text_format  varchar(8000),
		    @email_subject nvarchar(250),
		    @email_text varchar(8000),
		    @sms_text varchar(8000),
		    @SMSSENDERNO nvarchar(200),
		    @rowcount int,
		    @counter int,
			@rc int,
			@ctr int,
			@del nchar(1)

   declare @recipient_id uniqueidentifier,
           @role_code nvarchar(5),
		   @radiologist_id uniqueidentifier

   declare @MAILSVRUSRCODE nvarchar(100),
           @MAILSVRUSRPWD nvarchar(100)


   create table #tmpStudy
   (
	  row_id int identity(1,1),
	  study_hdr_id uniqueidentifier,
	  study_uid nvarchar(100),
	  study_status_pacs int,
	  priority_id int,
	  time_ellapsed_mins int,
	  time_left_mins int,
	  modality_id int,
	  species_id int,
	  institution_id uniqueidentifier,
	  received_date  datetime,
	  synched_on datetime,
	  patient_name nvarchar(250),
	  submitted_date  datetime
   )
   create table #tmpStudyRule
   (
	  row_id int identity(1,1),
	  study_hdr_id uniqueidentifier,
	  study_uid nvarchar(100),
	  study_status_pacs int,
	  priority_id int,
	  time_ellapsed_mins int,
	  time_left_mins int,
	  modality_id int,
	  species_id int,
	  institution_id uniqueidentifier,
	  received_date  datetime,
	  synched_on datetime,
	  patient_name nvarchar(250),
	  rule_no int,
	  notify_by_time nchar(1),
	  submitted_date  datetime
   )
   create table #tmpUsers
   (
	  row_id int identity(1,1),
	  rule_no int,
	  study_hdr_id uniqueidentifier,
	  study_uid nvarchar(100),
	  user_role_id int,
	  user_role_code nvarchar(10),
	  user_id uniqueidentifier default '00000000-0000-0000-0000-000000000000',
	  user_code nvarchar(10)   COLLATE SQL_Latin1_General_CP1_CI_AS null default '',
	  user_name  nvarchar(100) null default '',
	  email_id nvarchar(100) null default '',
	  mobile_no nvarchar(100) null default '',
	  notification_pref nchar(1) default 'B',
	  email_subject nvarchar(250) null,
	  email_text ntext null,
	  sms_text nvarchar(1000) null
   )
   create table #tmpRadiologist
   (
		rec_id int identity(1,1),
		radiologist_id uniqueidentifier,
		rad_user_id uniqueidentifier
   )   
   create table #tmpEmail
   (
	  row_id int identity(1,1),
	  rule_no int,
	  study_hdr_id uniqueidentifier,
	  study_uid nvarchar(100),
	  role_code nvarchar(10),
	  recepient_id uniqueidentifier,
	  recepient_name  nvarchar(100) null default '',
	  email_id nvarchar(100) null default '',
	  email_subject nvarchar(250) null,
	  email_text ntext null,
	  del nchar (1) null default 'N'
   )
   create table #tmpSMS
   (
	  row_id int identity(1,1),
	  rule_no int,
	  study_hdr_id uniqueidentifier,
	  study_uid nvarchar(100),
	  role_code nvarchar(10),
	  recepient_id uniqueidentifier,
	  recepient_name  nvarchar(100) null default '',
	  mobile_no nvarchar(100) null default '',
	  sms_text nvarchar(1000) null,
	  del nchar (1) null default 'N'
   )
   create table #tmpStudyTypes
   (
		row_id int identity(1,1),
		study_type_id uniqueidentifier
   )

   select @SMSSENDERNO = data_type_string
   from general_settings
   where control_code ='SMSSENDERNO'

   select @MAILSVRUSRCODE = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRCODE'

   select @MAILSVRUSRPWD = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRPWD'

   /******************************************************************************************************************
	List the scheduled radiologists
	******************************************************************************************************************/
    insert into #tmpRadiologist(radiologist_id,rad_user_id)
	(select rs.radiologist_id,u.id
	 from radiologist_schedule rs
	 inner join radiologists r on r.id = rs.radiologist_id
	 inner join users u on u.code=r.code
	 where getdate() between rs.start_datetime and rs.end_datetime
	 and r.is_active='Y')

	 insert into #tmpRadiologist(radiologist_id,rad_user_id)
	(select distinct cnrd.radiologist_id,u.id
	 from case_notification_rule_radiologist_dtls cnrd
	 inner join radiologists r on r.id = cnrd.radiologist_id
	 inner join users u on u.code=r.code
	 where radiologist_id not in (select radiologist_id from #tmpRadiologist)
	 and cnrd.notify_always='Y'
	 and r.is_active='Y')

	/******************************************************************************************************************
	 Email Format
	******************************************************************************************************************/
   --set @email_subject_format = 'Study queued in [QUEUE] List for [INST_NAME], Status : [STATUS]'
   set @email_subject_format = 'Study queued in [QUEUE] List, Status : [STATUS]'

   set @email_text_format    = 'Summary of study :- \n\n'
   set @email_text_format    = @email_text_format + ' Received Date/Time	: [REC_DATE] \n'
   set @email_text_format    = @email_text_format + ' Submitted Date/Time	: [SUBMITTED_DATE] \n'
   set @email_text_format    = @email_text_format + ' Patient				: [PATIENT_NAME] \n'
   set @email_text_format    = @email_text_format + ' Species				: [SPECIES] \n'
   set @email_text_format    = @email_text_format + ' Modality				: [MODALITY] \n'
   set @email_text_format    = @email_text_format + ' Priority				: [PRIORITY] \n'
   set @email_text_format    = @email_text_format + ' Time Left				: [TIME_LEFT] minutes\n'
   set @email_text_format    = @email_text_format + '\n\n'
   set @email_text_format    = @email_text_format +'This is an automated message from VETRIS.Please do not reply to the message.\n'
   set @email_text_format    = @email_text_format +'(#[RULE_NO])\n'

   /******************************************************************************************************************
	 SMS Format
	******************************************************************************************************************/
	--set @sms_text_format = 'Study queued in [QUEUE] List for [INST_NAME]/[STUDY_DATE]/[PATIENT_NAME]/[MODALITY]/[PRIORITY]/[TIME_LEFT] mins left Status : [STATUS]'
	set @sms_text_format = 'Study queued in [QUEUE] List for [PATIENT_NAME]/[SPECIES]/[SUBMITTED_DATE]/[MODALITY]/[PRIORITY]/[TIME_LEFT] mins left Status : [STATUS]'
	/********************************************************************************************************************************************************
	 LIST STUDIES FOR NOTIFICATION
	*********************************************************************************************************************************************************/
	insert into #tmpStudy(study_hdr_id,study_uid,time_ellapsed_mins,time_left_mins,
						  study_status_pacs,priority_id,modality_id,species_id,institution_id,
						  received_date,synched_on,patient_name,submitted_date)
	(select hdr.id,hdr.study_uid,datediff(mi,hdr.status_last_updated_on,getdate()),datediff(mi,getdate(),hdr.finishing_datetime),
	        hdr.study_status_pacs,hdr.priority_id,isnull(hdr.modality_id,0),isnull(hdr.species_id,0),hdr.institution_id,
			hdr.received_date,hdr.synched_on, rtrim(ltrim(isnull(hdr.patient_fname,'') + ' ' + isnull(hdr.patient_lname,''))),
			submitted_date = isnull((select max(date_updated) from sys_case_study_status_log where study_id=hdr.id and status_id_to=10),'01Jan1900')
	 from study_hdr hdr 
	 where hdr.study_status_pacs in (select distinct pacs_status_id from case_notification_rule_hdr where is_active='Y')
	 and hdr.priority_id in (select distinct priority_id from case_notification_rule_hdr where is_active='Y' union select priority_id=0))

	/********************************************************************************************************************************************************
	 ASSIGN RULE NO TO STUDIES
	*********************************************************************************************************************************************************/
	select @counter = 1,
	       @rowcount = @@rowcount

	while(@counter<=@rowcount)
		begin
			create table #tmpRule(rule_no int,notify_by_time nchar(1))

			select @study_hdr_id      = study_hdr_id,
				   @study_uid         = study_uid,
				   @study_status_pacs = study_status_pacs ,
		           @priority_id       = priority_id,
		           @time_ellapsed_mins= time_ellapsed_mins,
				   @time_left_mins    = time_left_mins,
				   @modality_id       = modality_id,
				   @species_id        = species_id,
				   @institution_id    = institution_id,
				   @received_date     = received_date,
				   @synched_on        = synched_on,
				   @patient_name      = patient_name,
				   @submitted_date    = submitted_date
			from #tmpStudy
			where row_id = @counter

			if(@priority_id=0) set @priority_id=20
			
			insert into #tmpRule(rule_no,notify_by_time)
			(select rule_no,notify_by_time
			 from case_notification_rule_hdr
			 where pacs_status_id     = @study_status_pacs
			 and   priority_id        = @priority_id
			 and   time_ellapsed_mins <= @time_ellapsed_mins
			 and   notify_by_time     = 'E'
			 and   is_active          = 'Y')

			 if(@@rowcount > 0)
				begin
					insert into #tmpStudyRule(study_hdr_id,study_uid,time_ellapsed_mins,time_left_mins,study_status_pacs,priority_id,modality_id,institution_id,received_date,synched_on,patient_name,rule_no,notify_by_time,submitted_date,species_id)
					(select @study_hdr_id,@study_uid,@time_ellapsed_mins,0,@study_status_pacs,@priority_id,@modality_id,@institution_id,@received_date,@synched_on,@patient_name,rule_no,notify_by_time,@submitted_date,@species_id
					 from #tmpRule)
				end

			insert into #tmpRule(rule_no,notify_by_time)
			(select rule_no,notify_by_time
			 from case_notification_rule_hdr
			 where pacs_status_id     = @study_status_pacs
			 and   priority_id        = @priority_id
			 and   time_left_mins     >= @time_left_mins
			 and   notify_by_time     = 'L'
			 and   is_active          = 'Y')

			 if(@@rowcount > 0)
				begin
					insert into #tmpStudyRule(study_hdr_id,study_uid,time_ellapsed_mins,time_left_mins,study_status_pacs,priority_id,modality_id,institution_id,received_date,synched_on,patient_name,rule_no,notify_by_time,submitted_date,species_id)
					(select @study_hdr_id,@study_uid,0,@time_left_mins,@study_status_pacs,@priority_id,@modality_id,@institution_id,@received_date,@synched_on,@patient_name,rule_no,notify_by_time,@submitted_date,@species_id
					 from #tmpRule)
				end

			drop table #tmpRule
			set @counter = @counter + 1
		end

    /********************************************************************************************************************************************************
	Filter rule wise recipient list and creating notifications
	*********************************************************************************************************************************************************/
	select @rowcount = count(row_id),
	       @counter  = 1
	from #tmpStudyRule

	while(@counter <= @rowcount)
		begin

			select @study_hdr_id      = study_hdr_id,
				   @study_uid         = study_uid,
				   @study_status_pacs = study_status_pacs ,
		           @priority_id       = priority_id,
		           @time_ellapsed_mins= time_ellapsed_mins,
				   @time_left_mins    = time_left_mins,
				   @notify_by_time    = notify_by_time,
				   @modality_id       = modality_id,
				   @species_id        = species_id,
				   @institution_id    = institution_id,
				   @received_date     = received_date,
				   @patient_name      = patient_name,
				   @rule_no           = rule_no,
				   @notify_by_time    = notify_by_time,
				   @submitted_date    = submitted_date
			from #tmpStudyRule
			where row_id = @counter 

			select @status_name = status_desc,
			       @queue_name   = vrs_study_queue
			from sys_study_status_pacs 
			where status_id = @study_status_pacs

			select @priority_name    = priority_desc from sys_priority where priority_id = @priority_id
			select @institution_name = name from institutions where id=@institution_id
			
			select @modality_code    = code,
			       @modality_name    = name
			from modality
			where id = @modality_id

			select @species_name = name from species where id=@species_id 

		  --  if(select count(rule_no)  from case_notification_rule_dtls where rule_no=@rule_no and scheduled = 'Y')>0
				--begin
				--	set @scheduled = 'Y'
				--end

			select @institution_user_role_id = id from user_roles where code='IU'
			if(select count(rule_no)  
			   from case_notification_rule_dtls 
			   where rule_no=@rule_no 
			   and user_role_id = @institution_user_role_id)>0
				begin
					set @institution_users='Y'
				end


			set @email_subject = @email_subject_format
			set @email_subject = replace(@email_subject,'[QUEUE]',@queue_name)
			--set @email_subject = replace(@email_subject,'[INST_NAME]',@institution_name)
			set @email_subject = replace(@email_subject,'[STATUS]',@status_name)

			if(@notify_by_time='E')
				begin
					set @email_text = @email_text_format
					set @email_text = replace(@email_text,'[REC_DATE]',convert(varchar(10),@synched_on,101) + ' ' + convert(varchar(5),@synched_on,114))
					set @email_text = replace(@email_text,'[SUBMITTED_DATE]',convert(varchar(10),@submitted_date,101) + ' ' + convert(varchar(5),@submitted_date,114))
					set @email_text = replace(@email_text,'[PATIENT_NAME]',@patient_name)
					set @email_text = replace(@email_text,'[SPECIES]',isnull(@species_name,''))
					set @email_text = replace(@email_text,'[MODALITY]',@modality_name)
					set @email_text = replace(@email_text,'[PRIORITY]',@priority_name)
					set @email_text = replace(@email_text,' Time Left				: [TIME_LEFT] minutes\n','')
					set @email_text = replace(@email_text,'[RULE_NO]',convert(varchar,@rule_no))

					set @sms_text = @sms_text_format
					set @sms_text = replace(@sms_text,'[QUEUE]',@queue_name)
					--set @sms_text = replace(@sms_text,'[INST_NAME]',@institution_name)
					set @sms_text = replace(@sms_text,'[SUBMITTED_DATE]',convert(varchar(10),@submitted_date,101) + ' ' + convert(varchar(5),@submitted_date,114))
					set @sms_text = replace(@sms_text,'[PATIENT_NAME]',@patient_name)
					set @sms_text = replace(@sms_text,'[SPECIES]',isnull(@species_name,''))
					set @sms_text = replace(@sms_text,'[MODALITY]',@modality_code)
					set @sms_text = replace(@sms_text,'[PRIORITY]',@priority_name)
					set @sms_text = replace(@sms_text,'/[TIME_LEFT] mins left','')
					set @sms_text = replace(@sms_text,'[STATUS]',@status_name)

					insert into #tmpUsers(rule_no,study_hdr_id,study_uid,
										  user_role_id,user_role_code,user_id,user_code,user_name,
										  email_id,mobile_no,notification_pref,
										  email_subject,email_text,sms_text)
					(select @rule_no,@study_hdr_id,@study_uid,
						   rd.user_role_id,ur.code, rd.user_id,u.code,u.name,
						   isnull(u.email_id,''),isnull(u.contact_no,''),isnull(u.notification_pref,''),
						   @email_subject,@email_text,@sms_text
					 from case_notification_rule_dtls rd
					 inner join case_notification_rule_hdr rh on rh.rule_no = rd.rule_no
					 inner join user_roles ur on ur.id = rd.user_role_id
					 inner join users u on u.id = rd.user_id
					 where rd.rule_no = @rule_no
					 and rh.notify_by_time='E'
					 and ur.code <>'RDL'
					 and rd.user_id<>'00000000-0000-0000-0000-000000000000')
					 order by ur.code

					if(@institution_users='Y')
						begin
							insert into #tmpUsers(rule_no,study_hdr_id,study_uid,
											user_role_id,user_role_code,user_id,user_code,user_name,
											email_id,mobile_no,notification_pref,
											email_subject,email_text,sms_text)
							(select @rule_no,@study_hdr_id,@study_uid,
									u.user_role_id,ur.code, u.id,u.code,u.name,
									isnull(u.email_id,''),isnull(u.contact_no,''),isnull(u.notification_pref,'B'),
									@email_subject,@email_text,@sms_text
								from institution_user_link iul
								inner join users u on u.id = iul.user_id 
								inner join user_roles ur on ur.id = u.user_role_id
								where user_role_id = @institution_user_role_id
								and  u.is_active='Y'
								and iul.institution_id=@institution_id)
								order by ur.code
						end

					insert into #tmpUsers(rule_no,study_hdr_id,study_uid,
										user_role_id,user_role_code,user_id,user_code,user_name,
										email_id,mobile_no,notification_pref,
										email_subject,email_text,sms_text)
					(select  @rule_no,@study_hdr_id,@study_uid,
							u.user_role_id,ur.code,cnrd.user_id,u.code,u.name,
							isnull(u.email_id,''),isnull(u.contact_no,''),isnull(u.notification_pref,'B'),
							@email_subject,@email_text,@sms_text
						from case_notification_rule_radiologist_dtls cnrd
						inner join case_notification_rule_hdr cnrh on cnrh.rule_no = cnrd.rule_no
						inner join users u on u.id = cnrd.user_id
						inner join user_roles ur on ur.id = u.user_role_id
						where cnrd.rule_no = @rule_no
						and cnrh.notify_by_time='E'
						and cnrd.notify_if_scheduled ='Y'
						and cnrd.radiologist_id in (select radiologist_id
													from radiologist_modality_link
													where modality_id = @modality_id)
						and cnrd.radiologist_id in (select radiologist_id
													from #tmpRadiologist))
					    order by ur.code  

					insert into #tmpUsers(rule_no,study_hdr_id,study_uid,
										  user_role_id,user_role_code,user_id,user_code,user_name,
										  email_id,mobile_no,notification_pref,
										  email_subject,email_text,sms_text)
					   (select  @rule_no,@study_hdr_id,@study_uid,
								u.user_role_id,ur.code,cnrd.user_id,u.code,u.name,
								isnull(u.email_id,''),isnull(u.contact_no,''),isnull(u.notification_pref,'B'),
								@email_subject,@email_text,@sms_text
							from case_notification_rule_radiologist_dtls cnrd
							inner join case_notification_rule_hdr cnrh on cnrh.rule_no = cnrd.rule_no
							inner join users u on u.id = cnrd.user_id
							inner join user_roles ur on ur.id = u.user_role_id
							where cnrd.rule_no = @rule_no
							and cnrh.notify_by_time='E'
							and cnrd.notify_always ='Y'
							and cnrd.user_id not in (select distinct user_id from #tmpUsers where study_hdr_id=@study_hdr_id and rule_no=@rule_no))
						order by ur.code  
				end
			else if(@notify_by_time='L')
				begin
					set @email_text = @email_text_format
					set @email_text = replace(@email_text,'[REC_DATE]',convert(varchar(10),@synched_on,101) + ' ' + convert(varchar(5),@synched_on,114))
					set @email_text = replace(@email_text,'[SUBMITTED_DATE]',convert(varchar(10),@submitted_date,101) + ' ' + convert(varchar(5),@submitted_date,114))
					set @email_text = replace(@email_text,'[PATIENT_NAME]',@patient_name)
					set @email_text = replace(@email_text,'[SPECIES]',isnull(@species_name,''))
					set @email_text = replace(@email_text,'[MODALITY]',@modality_name)
					set @email_text = replace(@email_text,'[PRIORITY]',@priority_name)
					set @email_text = replace(@email_text,'[TIME_LEFT]',convert(varchar,@time_left_mins))
					set @email_text = replace(@email_text,'[RULE_NO]',convert(varchar,@rule_no))

					set @sms_text = @sms_text_format
					set @sms_text = replace(@sms_text,'[QUEUE]',@queue_name)
					--set @sms_text = replace(@sms_text,'[INST_NAME]',@institution_name)
					set @sms_text = replace(@sms_text,'[SUBMITTED_DATE]',convert(varchar(10),@submitted_date,101) + ' ' + convert(varchar(5),@submitted_date,114))
					set @sms_text = replace(@sms_text,'[PATIENT_NAME]',@patient_name)
					set @sms_text = replace(@sms_text,'[SPECIES]',isnull(@species_name,''))
					set @sms_text = replace(@sms_text,'[MODALITY]',@modality_code)
					set @sms_text = replace(@sms_text,'[PRIORITY]',@priority_name)
					set @sms_text = replace(@sms_text,'[TIME_LEFT]',convert(varchar,@time_left_mins))
					set @sms_text = replace(@sms_text,'[STATUS]',@status_name)

					insert into #tmpUsers(rule_no,study_hdr_id,study_uid,
										  user_role_id,user_role_code,user_id,user_code,user_name,
										  email_id,mobile_no,notification_pref,
										  email_subject,email_text,sms_text)
					(select @rule_no,@study_hdr_id,@study_uid,
						   rd.user_role_id,ur.code, rd.user_id,u.code,u.name,
						   isnull(u.email_id,''),isnull(u.contact_no,''),isnull(u.notification_pref,''),
						   @email_subject,@email_text,@sms_text
					 from case_notification_rule_dtls rd
					 inner join case_notification_rule_hdr rh on rh.rule_no = rd.rule_no
					 inner join user_roles ur on ur.id = rd.user_role_id
					 inner join users u on u.id = rd.user_id
					 where rd.rule_no = @rule_no
					 and rh.notify_by_time='L'
					 and ur.code <>'RDL'
					 and rd.user_id<>'00000000-0000-0000-0000-000000000000')
					 order by ur.code

					if(@institution_users='Y')
						begin
							insert into #tmpUsers(rule_no,study_hdr_id,study_uid,
											user_role_id,user_role_code,user_id,user_code,user_name,
											email_id,mobile_no,notification_pref,
											email_subject,email_text,sms_text)
							(select @rule_no,@study_hdr_id,@study_uid,
									u.user_role_id,ur.code, u.id,u.code,u.name,
									isnull(u.email_id,''),isnull(u.contact_no,''),isnull(u.notification_pref,'B'),
									@email_subject,@email_text,@sms_text
								from institution_user_link iul
								inner join users u on u.id = iul.user_id 
								inner join user_roles ur on ur.id = u.user_role_id
								where user_role_id = @institution_user_role_id
								and  u.is_active='Y'
								and iul.institution_id=@institution_id)
								order by ur.code
						end

					insert into #tmpUsers(rule_no,study_hdr_id,study_uid,
										user_role_id,user_role_code,user_id,user_code,user_name,
										email_id,mobile_no,notification_pref,
										email_subject,email_text,sms_text)
					(select  @rule_no,@study_hdr_id,@study_uid,
							u.user_role_id,ur.code,cnrd.user_id,u.code,u.name,
							isnull(u.email_id,''),isnull(u.contact_no,''),isnull(u.notification_pref,'B'),
							@email_subject,@email_text,@sms_text
						from case_notification_rule_radiologist_dtls cnrd
						inner join case_notification_rule_hdr cnrh on cnrh.rule_no = cnrd.rule_no
						inner join users u on u.id = cnrd.user_id
						inner join user_roles ur on ur.id = u.user_role_id
						where cnrd.rule_no = @rule_no
						and cnrh.notify_by_time='L'
						and cnrd.notify_if_scheduled ='Y'
						and cnrd.radiologist_id in (select radiologist_id
													from radiologist_modality_link
													where modality_id = @modality_id)
						and cnrd.radiologist_id in (select radiologist_id
													from #tmpRadiologist))
					order by ur.code  

					insert into #tmpUsers(rule_no,study_hdr_id,study_uid,
										  user_role_id,user_role_code,user_id,user_code,user_name,
										  email_id,mobile_no,notification_pref,
										  email_subject,email_text,sms_text)
					   (select  @rule_no,@study_hdr_id,@study_uid,
								u.user_role_id,ur.code,cnrd.user_id,u.code,u.name,
								isnull(u.email_id,''),isnull(u.contact_no,''),isnull(u.notification_pref,'B'),
								@email_subject,@email_text,@sms_text
							from case_notification_rule_radiologist_dtls cnrd
							inner join case_notification_rule_hdr cnrh on cnrh.rule_no = cnrd.rule_no
							inner join users u on u.id = cnrd.user_id
							inner join user_roles ur on ur.id = u.user_role_id
							where cnrd.rule_no = @rule_no
							and cnrh.notify_by_time='L'
							and cnrd.notify_always ='Y'
							and cnrd.user_id not in (select distinct user_id from #tmpUsers where study_hdr_id=@study_hdr_id and rule_no=@rule_no))
						order by ur.code  
				end

			set @counter = @counter + 1
		end

	/******************************************************************************************************************
	 REMOVING DEPLICATE NOTIFICATIONS
	******************************************************************************************************************/
	insert into #tmpEmail(rule_no,study_hdr_id,study_uid,
	                      role_code,recepient_id,recepient_name,
	                      email_id,email_subject,email_text)
				 (select rule_no,study_hdr_id,study_uid,
						 user_role_code,user_id,user_name,
	                     email_id,email_subject,email_text
				  from #tmpUsers
				  where isnull(email_id,'')<>''
				  and notification_pref in ('B','E'))

	select @rowcount=@@rowcount,@counter=1,@del='N'

	while(@counter<=@rowcount)
		begin
			select @study_hdr_id = study_hdr_id,
			       @role_code    = role_code,
			       @recipient_id = recepient_id,
			       @rule_no      = rule_no
			from #tmpEmail
			where row_id = @counter

			set @del='N'

			if(select count(email_log_id) from vrslogdb..email_log where study_hdr_id=@study_hdr_id and creation_rule_no=@rule_no)>0
				begin
					update #tmpEmail set del='Y' where row_id=@counter
					set @del='Y'
				end

			if(@role_code = 'RDL' and @del='N') 
				begin
					select @radiologist_id = id from radiologists where login_user_id=@recipient_id
					select @species_id = isnull(species_id,0),
					       @modality_id = isnull(modality_id,0),
					       @service_codes = isnull(service_codes,''),
						   @institution_id = isnull(institution_id,'00000000-0000-0000-0000-000000000000')
					from study_hdr 
					where id = @study_hdr_id

					insert into #tmpStudyTypes(study_type_id)
					(select study_type_id from study_hdr_study_types where study_hdr_id=@study_hdr_id)
					
					select @rc=@@rowcount,@ctr=1

					if(charindex('CONSULT',@service_codes))>0
						begin
							if(select count(radiologist_id) from radiologist_functional_rights_assigned where radiologist_id=@radiologist_id and right_code='WRKCNSLTCASE')=0
								begin
									update #tmpEmail set del='Y' where row_id=@counter
								end
						end
					if(charindex('CONSULT',@service_codes))<=0
						begin
							if(select count(radiologist_id) from radiologist_functional_rights_assigned where radiologist_id=@radiologist_id and right_code='WRKCNSLTCASE')>0
								begin
									update #tmpEmail set del='Y' where row_id=@counter
								end
						end

					if(select count(radiologist_id) from radiologist_functional_rights_species where radiologist_id=@radiologist_id and species_id=@species_id)=0
						begin
							update #tmpEmail set del='Y' where row_id=@counter
						end

					if(select count(radiologist_id) from radiologist_functional_rights_modality where radiologist_id=@radiologist_id and modality_id=@modality_id)=0
						begin
							update #tmpEmail set del='Y' where row_id=@counter
						end

					if(select count(radiologist_id) from radiologist_functional_rights_exception_institution where radiologist_id=@radiologist_id and institution_id=@institution_id)>0
						begin
							update #tmpEmail set del='Y' where row_id=@counter
						end
					
					while(@ctr<@rc)
						begin
							select @study_type_id = study_type_id
							from #tmpStudyTypes
							where row_id = @ctr

							if(select count(radiologist_id) from radiologist_functional_rights_exception_study_type where radiologist_id=@radiologist_id and study_type_id=@study_type_id)>0
								begin
									update #tmpEmail set del='Y' where row_id=@counter
									break
								end

							set @ctr = @ctr + 1
						end
					
					truncate table #tmpStudyTypes
				end

			set @counter = @counter + 1
		end

	--insert into #tmpEmail(rule_no,study_hdr_id,study_uid,recepient_name,
	--                      email_id,email_subject,email_text)
	--			 (select rule_no,study_hdr_id,study_uid,recepient_name,
	--                     'pguha@rad365tech.com',email_subject,email_text
	--			  from #tmpEmail
	--			  where del='N')

	insert into #tmpSMS(rule_no,study_hdr_id,study_uid,
	                    mobile_no,role_code,recepient_id,recepient_name,sms_text)
				 (select rule_no,study_hdr_id,study_uid,
	                     mobile_no,user_role_code,user_id,user_name,sms_text
				  from #tmpUsers
				  where isnull(mobile_no,'')<>''
				  and notification_pref in ('B','S'))

	select @rowcount=@@rowcount,@counter=1,@del='N'

	while(@counter<=@rowcount)
		begin
			select @study_hdr_id = study_hdr_id,
			       @role_code    = role_code,
			       @recipient_id = recepient_id,
			       @rule_no      = rule_no
			from #tmpSMS
			where row_id = @counter

			set @del='N'
			if(select count(sms_log_id) from vrslogdb..sms_log where study_hdr_id=@study_hdr_id and creation_rule_no=@rule_no)>0
				begin
					update #tmpSMS set del='Y' where row_id=@counter
					set @del='Y'
				end

			if(@role_code = 'RDL' and @del='N') 
				begin
					select @radiologist_id = id from radiologists where login_user_id=@recipient_id
					select @modality_id = isnull(modality_id,0),
					       @service_codes = isnull(service_codes,''),
						   @institution_id = isnull(institution_id,'00000000-0000-0000-0000-000000000000')
					from study_hdr 
					where id = @study_hdr_id

					insert into #tmpStudyTypes(study_type_id)
					(select study_type_id from study_hdr_study_types where study_hdr_id=@study_hdr_id)
					
					select @rc=@@rowcount,@ctr=1

					if(select charindex('CONSULT',@service_codes))>0
						begin
							if(select count(radiologist_id) from radiologist_functional_rights_assigned where radiologist_id=@radiologist_id and right_code='WRKCNSLTCASE')=0
								begin
									update #tmpSMS set del='Y' where row_id=@counter
								end
						end
					if(select charindex('CONSULT',@service_codes))<=0
						begin
							if(select count(radiologist_id) from radiologist_functional_rights_assigned where radiologist_id=@radiologist_id and right_code='WRKCNSLTCASE')>0
								begin
									update #tmpSMS set del='Y' where row_id=@counter
								end
						end
					if(select count(radiologist_id) from radiologist_functional_rights_modality where radiologist_id=@radiologist_id and modality_id=@modality_id)=0
						begin
							update #tmpSMS set del='Y' where row_id=@counter
						end

					if(select count(radiologist_id) from radiologist_functional_rights_exception_institution where radiologist_id=@radiologist_id and institution_id=@institution_id)>0
						begin
							update #tmpSMS set del='Y' where row_id=@counter
						end
					
					while(@ctr<@rc)
						begin
							select @study_type_id = study_type_id
							from #tmpStudyTypes
							where row_id = @ctr

							if(select count(radiologist_id) from radiologist_functional_rights_exception_study_type where radiologist_id=@radiologist_id and study_type_id=@study_type_id)>0
								begin
									update #tmpSMS set del='Y' where row_id=@counter
									break
								end

							set @ctr = @ctr + 1
						end
					
					truncate table #tmpStudyTypes
				end

			set @counter = @counter + 1
		end

	--insert into #tmpSMS(rule_no,study_hdr_id,study_uid,
	--                    mobile_no,recepient_name,sms_text)
	--			 (select rule_no,study_hdr_id,study_uid,
	--                     '+19543426962',recepient_name,sms_text
	--			  from #tmpSMS
	--			   where del='N')

	--select * from #tmpStudy
	--select * from #tmpStudyRule
	--select * from #tmpUsers

	/******************************************************************************************************************
	 FINAL CREATION OF NOTIFICATIONS
	******************************************************************************************************************/
	begin transaction
	--email log
	insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,cc_address,
	                                email_subject,email_text,study_hdr_id,study_uid,creation_rule_no,email_type,sender_email_address,sender_email_password)
				 (select newid(),getdate(),email_id,recepient_name,'',
				         email_subject,isnull(email_text,'') email_text,study_hdr_id,study_uid,rule_no,'NRULE',@MAILSVRUSRCODE,@MAILSVRUSRPWD
				  from #tmpEmail
				  where del='N')

	select @email_count= @@ROWCOUNT

	

	--sms log for default users
	insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,sender_no,recipient_no,recipient_name,sms_text,sequence_no,
	                     study_hdr_id,study_uid,creation_rule_no,sms_type)
				 (select newid(),getdate(),@SMSSENDERNO,mobile_no,recepient_name,sms_text,1,
				         study_hdr_id,study_uid,rule_no,'NRULE'
				  from #tmpSMS
				  where del='N')

	select @sms_count= @@ROWCOUNT


	
	commit transaction

	drop table #tmpRadiologist
	drop table #tmpStudy
	drop table #tmpStudyRule
	drop table #tmpUsers
	drop table #tmpEmail
	drop table #tmpSMS
	drop table #tmpStudyTypes
	set nocount off
end

GO
