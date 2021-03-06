USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_radiologist_assign]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_radiologist_assign]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_radiologist_assign]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_radiologist_assign : auto
                  assign a study to aradiologist
** Created By   : Pavel Guha
** Created On   : 30/04/2021
*******************************************************/
--exec scheduler_radiologist_assign 'EF1B7EB8-C290-48EB-95B2-42CB46C40BD9','',0
CREATE procedure [dbo].[scheduler_radiologist_assign]
	@id              uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	set nocount on 

	declare @schedule_date date,
	        @study_uid nvarchar(100),
	        @modality_id int,
			@species_id int,
			@category_id int,
			@institution_id uniqueidentifier,
			@error_code nvarchar(10),
			@return_status int,
			@activity_text nvarchar(max),
			@rad_id uniqueidentifier,
			@rad_name nvarchar(250),
			@modality_name nvarchar(30),
			@modality_code nvarchar(5),
			@assign nchar(1),
			@rowcount int,
			@counter int,
			@rc int,
			@ctr int,
			@total_consumed int,
			@modality_work_units int,
			@case_released int


	--select @return_type=0,@error_msg='Failed to import study file record - SUID ' + @study_uid
	--				return 0

    create table #tmpRad
	(
		id uniqueidentifier,
		name nvarchar(250)
	)
	create table #tmpST
	(
		study_type_id uniqueidentifier
	)
	create table #tmpAssign
	(
		rec_id int identity(1,1),
		radiologist_id uniqueidentifier,
		name nvarchar(250),
		work_unit_balance int
	)
	create table #tmp1
	(rec_id int identity(1,1),
	rad_id uniqueidentifier,
	rad_name nvarchar(250),
	modality_wu int,
	wu_ondate int,
	wu_consumed int,
	wu_balance int)

	select @schedule_date = convert(date,getdate())

	select @study_uid                      = study_uid,
		   @institution_id                 = institution_id,
		   @category_id                    = category_id,
	       @modality_id                    = modality_id,
		   @species_id                     = species_id
	from study_hdr
	where id = @id

	select @modality_name = name,
	       @modality_code = code
	from modality 
	where id=@modality_id

	insert into #tmpST(study_type_id) 
	(select study_type_id
	from study_hdr_study_types
	where study_hdr_id=@id)

	insert into #tmpRad(id,name) 
	(select r.id,r.name 
	 from radiologists r 
	 inner join radiologist_schedule rs on rs.radiologist_id = r.id
	 where r.is_active='Y'
	 and getdate() between rs.start_datetime and rs.end_datetime) 
	 order by name
	 /******Filter out based on skills and funcytional rights**************/
	 delete 
	 from #tmpRad
	 where id in (select radiologist_id from radiologist_functional_rights_exception_institution where institution_id=@institution_id)

	 delete 
	 from #tmpRad
	 where id not in (select radiologist_id from radiologist_functional_rights_modality where modality_id=@modality_id)

	 delete 
	 from #tmpRad
	 where id not in (select radiologist_id from radiologist_functional_rights_species where species_id=@species_id)

	 if(@category_id=3)
		begin
			delete 
			from #tmpRad
			where id not in (select radiologist_id from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE')
		end

	delete 
	from #tmpRad
	where id in (select radiologist_id from radiologist_functional_rights_exception_study_type where study_type_id in (select study_type_id from #tmpST))

	delete 
	from #tmpRad
	where id not in (select radiologist_id from radiologist_functional_rights_assigned where right_code ='DICTRPT')
	/******Filter out based on skills and funcytional rights**************/

	/******Final list of radiologists**************/
	insert into #tmpAssign(radiologist_id,name,work_unit_balance) 
	(select t.id,t.name,r.max_wu_per_hr
	 from #tmpRad t
	 inner join radiologists r on r.id=t.id)
	 order by r.max_wu_per_hr desc
	/******Final list of radiologists**************/

	select @rowcount = @@rowcount,@counter = 1

	--select * from #tmpAssign


	begin transaction

	set @assign='N'
	set @rad_id ='00000000-0000-0000-0000-000000000000'
	while(@counter <= @rowcount)
		begin
			select @rad_id = radiologist_id,
			       @rad_name = name
			from #tmpAssign
			where rec_id = @counter

			--print @rad_id
			--print @rad_name


			if(select count(radiologist_id) from vrslogdb..radiologist_assignment_log where scheduled_date=@schedule_date and modality_id=@modality_id and category_id=@category_id) =0
				begin
					set @assign	= 'Y'
					break
				end
			else if(select count(radiologist_id) from vrslogdb..radiologist_assignment_log where scheduled_date=@schedule_date and modality_id=@modality_id and category_id=@category_id)>0
				begin
					if(select count(radiologist_id) from vrslogdb..radiologist_assignment_log where scheduled_date=@schedule_date and modality_id=@modality_id and category_id=@category_id and radiologist_id=@rad_id)=0
						begin
							set @assign	= 'Y'
							break
						end
					else
						begin
							select @total_consumed = work_unit_consumed_on_date
							from radiologist_work_unit_balance 
							where radiologist_id = @rad_id
							and scheduled_date   = @schedule_date

							select @case_released = count(study_hdr_id)
							from vrslogdb..radiologist_assignment_release_log
							where study_hdr_id     = @id
							and radiologist_id = @rad_id

							if(@total_consumed >0)
								begin
									if(select count(radiologist_id)
									   from radiologist_work_unit_balance
									   where radiologist_id <> @rad_id
									   and scheduled_date            = @schedule_date
									   and work_unit_consumed_on_date < @total_consumed
									   and radiologist_id in (select radiologist_id from #tmpAssign where radiologist_id<>@rad_id)
									   and work_unit_balance_on_date>0)=0
										begin
											if(@case_released =0)
												begin
													set @assign	= 'Y'
													break
												end
										end
									else
										begin
											declare @tmpRadID uniqueidentifier,
											        @tmpRadName nvarchar(250),
													@modality_wu int,
													@wu_ondate int,
													@wu_consumed int,
													@wu_balance int

											 insert into #tmp1(rad_id,rad_name,modality_wu,
											                   wu_ondate,wu_consumed,wu_balance)
											           (select wub.radiologist_id,r.name,rml.work_unit,
													           wub.work_unit_on_date,wub.work_unit_consumed_on_date,wub.work_unit_balance_on_date
													    from radiologist_work_unit_balance wub
														inner join radiologists r on r.id = wub.radiologist_id
														inner join radiologist_modality_link rml on rml.radiologist_id=wub.radiologist_id
														where wub.radiologist_id <> @rad_id
													    and wub.scheduled_date            = @schedule_date
														and wub.work_unit_consumed_on_date < @total_consumed
														and wub.radiologist_id in (select radiologist_id from #tmpAssign where radiologist_id<>@rad_id)
														and wub.work_unit_balance_on_date>0
														and rml.modality_id =@modality_id)
														order by wub.work_unit_on_date,r.name

											
											 select @rc=@@rowcount,@ctr=1

											 if(@rc>0)
												begin
													 while(@ctr<= @rc)
														begin
															select @tmpRadID    = rad_id,
															       @modality_wu = modality_wu,
													               @wu_ondate   = wu_ondate,
													               @wu_consumed = wu_consumed,
													               @wu_balance  = wu_balance
															from #tmp1
															where rec_id = @ctr

															if((@wu_ondate - (@wu_consumed + @modality_wu))>=0)
																begin
																	select @case_released = count(study_hdr_id)
																	from vrslogdb..radiologist_assignment_release_log
																	where study_hdr_id = @id
																	and radiologist_id = @tmpRadID

																	if(@case_released=0)
																		begin
																			set @rad_id = @tmpRadID
																			truncate table #tmp1
																			set @assign	= 'Y'
																			break 	
																		end
																end

															set @ctr=@ctr + 1
														end
												end
											else
												begin
													if(@case_released=0)
														begin
															truncate table #tmp1
															set @assign	= 'Y'
															break
														end
												end
										end
								end
						end
				end

			set @counter = @counter + 1
		end


	if(@assign='Y')
		begin
			insert into vrslogdb..radiologist_assignment_log(scheduled_date,radiologist_id,modality_id,category_id,study_hdr_id,study_uid,assign_datetime)
			                                          values(@schedule_date,@rad_id,@modality_id,@category_id,@id,@study_uid,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg   ='Failed to assign radiologist for Study UID ' + @study_uid,
							@return_type =0
					return 0
				end

			select @rad_name = name from radiologists where id=@rad_id
			select @modality_work_units = work_unit from radiologist_modality_link where radiologist_id = @rad_id

			update radiologist_work_unit_balance
			set work_unit_consumed_on_date = work_unit_consumed_on_date + @modality_work_units
			where radiologist_id = @rad_id
			and scheduled_date = @schedule_date 

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg    ='Failed to update work unit consumption of  ' + @rad_name,
							@return_type = 0
					return 0
				end

			update radiologist_work_unit_balance
			set work_unit_balance_on_date = work_unit_balance_on_date - work_unit_consumed_on_date,
			    date_updated = getdate()
			where radiologist_id = @rad_id
			and scheduled_date = @schedule_date 

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg    ='Failed to update work unit balance of  ' + @rad_name,
							@return_type = 0
					return 0
				end

			update study_hdr
			set dict_radiologist_id   = @rad_id,
			    dict_radiologist_pacs = @rad_name,
			    radiologist_id        = @rad_id,
				radiologist_pacs      = @rad_name,
				assign_accepted       = 'Y',
				manually_assigned     = 'A',
				pacs_wb               = 'Y',
				rad_assigned_on       = getdate()
			where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg    ='Failed to assign  ' + @rad_name + ' to Study UID ' + @study_uid,
							@return_type = 0
					return 0
				end

			set @activity_text = @rad_name +  ' assigned'
			exec common_study_user_activity_trail_save
				@study_hdr_id = @id,
				@study_uid    = @study_uid,
				@menu_id      = 0,
				@activity_text = @activity_text,
				@activity_by   = '00000000-0000-0000-0000-000000000000',
				@error_code    = @error_msg output,
				@return_status = @return_type output

			if(@return_type=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to create activity log  of Study UID ' + @study_uid + '.'
					return 0
				end


			/*************************GENERATE NOTIFICATION*****************************/
			declare @recipient_address nvarchar(500),
					@recipient_name nvarchar(200),
					@patient_name nvarchar(200),
					@email_subject varchar(250),
			        @email_text nvarchar(max),
					@MAILSVRUSRCODE nvarchar(100),
				    @MAILSVRUSRPWD nvarchar(100),
					@instituion_name nvarchar(100),
					@sms_text nvarchar(max),
					@mobile_no nvarchar(20),
					@SMSSENDERNO nvarchar(200),
					@notification_pref nchar(1)

			select @MAILSVRUSRCODE = data_type_string from general_settings where control_code='MAILSVRUSRCODE'
			select @MAILSVRUSRPWD = data_type_string from general_settings where control_code='MAILSVRUSRPWD'
			
			select @instituion_name = name from institutions where id = @institution_id

			select @recipient_address = isnull(email_id,''),
				   @mobile_no         = isnull(mobile_no,''),
				   @notification_pref = notification_pref
			from radiologists
			where id = @rad_id

			select @patient_name               = patient_name
			from study_hdr
			where id= @id
			and study_uid = @study_uid

		    if(isnull(@recipient_address,'') <> '' and (@notification_pref='B' or @notification_pref='E'))
				begin
					set @email_subject = 'Study of patient ' + isnull(@patient_name,'') + ' has been assigned to you'
						
					set @email_text    = 'Following are the study details :- \n\n'
					set @email_text    = @email_text + ' Patient     : ' + @patient_name + '\n'
					set @email_text    = @email_text + ' Institution : ' + @instituion_name + '\n'
					set @email_text    = @email_text + ' Modality    : ' + @modality_name + '\n'
					set @email_text    = @email_text + '\n\n'
					set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'


					insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,
											email_subject,email_text,study_hdr_id,study_uid,sender_email_address,sender_email_password,
											email_type,release_email,date_updated)
									values(newid(),getdate(),@recipient_address,@recipient_name,
											@email_subject,@email_text,@id,@study_uid,@MAILSVRUSRCODE,@MAILSVRUSRPWD,
											'RADAUTOASN','Y',getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_msg    ='Failed to queue up the assignment mail to  ' + @rad_name + ' (Study UID :' + @study_uid +')',
							       @return_type = 0
							return 0
						end

					set @activity_text =  'Auto assignment email queued for sending'
					exec common_study_user_activity_trail_save
						@study_hdr_id  = @id,
						@study_uid     ='',
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
				end

			if(isnull(@mobile_no,'') <> '' and (@notification_pref='B' or @notification_pref='S'))
				begin
					select @SMSSENDERNO = data_type_string
					from general_settings
					where control_code ='SMSSENDERNO'

					set @sms_text    = 'Study of ' + isnull(@patient_name,'') + '/' + @modality_code + ' has been assigned to you'
				    
					insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,recipient_no,recipient_name,sender_no,sequence_no,sms_text,study_hdr_id,study_uid,
									    sms_type,release_sms,date_updated)
								 values(newid(),getdate(),@mobile_no,@rad_name,@SMSSENDERNO,1,@sms_text,@id,@study_uid,
										'RADAUTOASN','Y',getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_msg    ='Failed to queue up the assignment sms to  ' + @rad_name + ' (Study UID :' + @study_uid +')',
									@return_type = 0
							return 0
						end

					set @activity_text =  ' Auto assignment sms queued for sending'
					exec common_study_user_activity_trail_save
						@study_hdr_id  = @id,
						@study_uid     ='',
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

				end

			/*************************GENERATE NOTIFICATION*****************************/
		end
	
	commit transaction
	select @error_msg   ='',@return_type =1
	set nocount off
	drop table #tmpRad
	drop table #tmpST
	drop table #tmpAssign
	drop table #tmp1

	return 1
end



GO
