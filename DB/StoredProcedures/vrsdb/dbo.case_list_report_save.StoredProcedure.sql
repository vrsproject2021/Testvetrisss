USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_report_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_report_save]
GO
/****** Object:  StoredProcedure [dbo].[case_list_report_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_report_save : save
                  case report
** Created By   : Pavel Guha
** Created On   : 24/04/2020
*******************************************************/
CREATE procedure [dbo].[case_list_report_save]
	@id uniqueidentifier ='00000000-0000-0000-0000-000000000000' output,
    @study_hdr_id uniqueidentifier,
	@report_text ntext=null,
	@report_text_html ntext=null,
	@disclaimer_reason_id int =0,
	@rating_reason_id uniqueidentifier ='00000000-0000-0000-0000-000000000000',
	@status_id int=0,
	@write_back nchar(1) ='N',
	@rating nchar(1)='',
	@mark_to_teach nchar(1)='N',
	@disclaimer_text ntext=null,
	@updated_by uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier ='00000000-0000-0000-0000-000000000000',
	@user_name nvarchar(130)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	declare @study_uid nvarchar(100),
	        @report_type nchar(1),
			@curr_status_id int,
			@curr_status_desc nvarchar(30),
			@current_report_type nchar(1),
			@radiologist_id uniqueidentifier,
			@radiologist_name nvarchar(100),
			@reading_radiologist_id uniqueidentifier,
			@reading_radiologist_name nvarchar(100),
			@curr_radiologist_id uniqueidentifier,
			@rad_id uniqueidentifier,
			@status_desc nvarchar(30),
			@vrs_status_id int,
			@user_role_id int,
			@user_role_code nvarchar(5),
			@ver int,
			@date_prepared datetime,
			@institution_id uniqueidentifier,
			@physician_id uniqueidentifier,
			@report_type_text nvarchar(20),
			@activity_text nvarchar(max),
			@transcription_finishing_time_mins int,
			@transcription_finishing_datetime datetime,
			@priority_id int,
			@pacs_wb nchar(1)

	select @study_uid = study_uid,
	       @curr_status_id = study_status_pacs,
		   @institution_id = isnull(institution_id,'00000000-0000-0000-0000-000000000000'),
		   @physician_id   = isnull(physician_id,'00000000-0000-0000-0000-000000000000'),
		   @priority_id    = isnull(priority_id,0)
	from study_hdr
	where id = @study_hdr_id

	select @user_role_id = user_role_id from users where id=@updated_by
	select @user_role_code = code from user_roles where id=@user_role_id

	if(@status_id < @curr_status_id)
		begin
			select @user_name= status_desc from sys_study_status_pacs where status_id=@curr_status_id
			if(@curr_status_id=60)
				begin
					select @radiologist_name = name from radiologists where login_user_id = (select created_by from study_hdr_dictated_reports where study_hdr_id=@study_hdr_id)
				end
			else if(@curr_status_id=80)
				begin
					select @radiologist_name = name from radiologists where login_user_id = (select created_by from study_hdr_prelim_reports where study_hdr_id=@study_hdr_id)
				end
			else if(@curr_status_id=80)
				begin
					select @radiologist_name = name from radiologists where login_user_id = (select created_by from study_hdr_final_reports where study_hdr_id=@study_hdr_id)
				end

			set @user_name = @user_name + ' by ' + isnull(@radiologist_name,'None')
			select @error_code = '296',@return_status=0
			return 0
		end

	exec common_check_record_lock_ui
		@menu_id       = @menu_id,
		@record_id     = @study_hdr_id,
		@user_id       = @updated_by,
		@session_id    = @session_id,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			if(@user_role_code='RDL')
				begin
					set @error_code='386'
				end
			return 0
		end

	select @status_desc = vrs_status_desc
	from sys_study_status_pacs 
	where status_id = @curr_status_id

	select @radiologist_id = id from radiologists where login_user_id=@updated_by
	set @radiologist_id = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')

	if(@user_role_code='RDL')
		begin
			--check record locking
			if(@status_id = 50 or @status_id=60)
				begin
					select @curr_radiologist_id= prelim_radiologist_id from study_hdr where id=@id
					if(@curr_radiologist_id<> @radiologist_id)
						begin
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='ACCLOCKSTUDY')=0
								begin
									select @error_code = '297',@return_status=0
									return 0
								end
						end
				end
			else if(@status_id = 80 or @status_id=100)
				begin
					select @curr_radiologist_id= final_radiologist_id from study_hdr where id=@id
					if(@curr_radiologist_id<> @radiologist_id)
						begin
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='ACCLOCKSTUDY')=0
								begin
									select @error_code = '297',@return_status=0
									return 0
								end
						end
				end
		end

	if(@status_id=60) set @report_type='D'
	else if(@status_id=80)
		begin 
			set @report_type='P'
			set @report_type_text ='Preliminary'
		end
	else if(@status_id=100) 
		begin
			set @report_type='F'
			set @report_type_text ='Final'
		end
	else set @report_type='R'

	if(@curr_status_id=60) set @current_report_type='D'
	else if(@curr_status_id=80) set @current_report_type='P'
	else if(@curr_status_id=100) set @current_report_type='F'
	else set @current_report_type='R'

	if(@priority_id >0 and @status_id=60)
		begin
			select @transcription_finishing_time_mins = transcription_finishing_time_mins from sys_priority where priority_id=@priority_id
			select @transcription_finishing_datetime = dateadd(mi,@transcription_finishing_time_mins,getdate())
		end

	if(@curr_status_id=50 and @status_id=50)
		begin
			select @error_code = '210',@return_status=0
			return 0
		end

	begin transaction
		
	if(@report_type='D' and @current_report_type='R')
		begin
			set @id=newid()
			if(isnull((select transcription_required from radiologists where id=@radiologist_id),'N'))='Y'
				begin
					set @pacs_wb ='N'
					update study_hdr set transcription_finishing_datetime=@transcription_finishing_datetime where id=@study_hdr_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='035'
							return 0
						end
				end
			else
				begin
					set @pacs_wb ='Y'
				end

			insert into study_hdr_dictated_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,rating,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created)
									        values(@id,@study_hdr_id,@study_uid,@report_text,@report_text_html,@pacs_wb,@rating,@disclaimer_reason_id,@disclaimer_text,@rating_reason_id,@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id = @study_hdr_id,
				@study_uid    ='',
				@menu_id      = @menu_id,
				@activity_text = 'Created Dictated Report',
				@session_id    = @session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

		   if(@return_status=0)
			begin
				rollback transaction
				return 0
			end

		end
	else if(@report_type='D' and @current_report_type='D')
		begin
			if(isnull((select transcription_required from radiologists where id=@radiologist_id),'N'))='Y'
				begin
					set @pacs_wb ='N'
					update study_hdr set transcription_finishing_datetime=@transcription_finishing_datetime where id=@study_hdr_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='035'
							return 0
						end
				end
			else
				begin
					set @pacs_wb ='Y'
				end

			update study_hdr_dictated_reports
			set report_text          = @report_text,
				report_text_html     = @report_text_html,
				pacs_wb              = @pacs_wb,
				disclaimer_reason_id = @disclaimer_reason_id,
				disclaimer_text      = @disclaimer_text,
				rating_reason_id     = @rating_reason_id,
				updated_by           = @updated_by,
				date_updated         = @date_prepared
			where study_hdr_id = @study_hdr_id 
			--and report_id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id = @study_hdr_id,
				@study_uid    ='',
				@menu_id      = @menu_id,
				@activity_text = 'Updated Dictated Report',
				@session_id    = @session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

		   if(@return_status=0)
			begin
				rollback transaction
				return 0
			end

			
		end
	else if(@report_type='P' and (@current_report_type='D' or @current_report_type='R'))
		begin
			set @id=newid()
			insert into study_hdr_prelim_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,rating,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created)
									        values(@id,@study_hdr_id,@study_uid,@report_text,@report_text_html,@rating,'Y',@disclaimer_reason_id,@disclaimer_text,@rating_reason_id,@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id = @study_hdr_id,
				@study_uid    ='',
				@menu_id      = @menu_id,
				@activity_text = 'Created Preliminary Report',
				@session_id    = @session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

		   if(@return_status=0)
			begin
				rollback transaction
				return 0
			end
		end
	else if(@report_type='F' and (@current_report_type='P' or @current_report_type='D' or @current_report_type='R'))
		begin
			set @id=newid()
			insert into study_hdr_final_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created)
									        values(@id,@study_hdr_id,@study_uid,@report_text,@report_text_html,'Y',@disclaimer_reason_id,@disclaimer_text,@rating_reason_id,@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id = @study_hdr_id,
				@study_uid    ='',
				@menu_id      = @menu_id,
				@activity_text = 'Created Final Report',
				@session_id    = @session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

		   if(@return_status=0)
			begin
				rollback transaction
				return 0
			end
		end
	else if(@report_type='F' and @current_report_type='F')
		begin
			if(select count(study_hdr_id) from study_hdr_final_reports where study_hdr_id=@study_hdr_id)>0
				begin
					update study_hdr_final_reports
					set report_text          = @report_text,
						report_text_html     = @report_text_html,
						disclaimer_reason_id = @disclaimer_reason_id,
						disclaimer_text      = @disclaimer_text,
						updated_by           = @updated_by,
						date_updated         = getdate()
					where study_hdr_id=@id
				end
			else
				begin
					set @id=newid()
					insert into study_hdr_final_reports(report_id,study_hdr_id,study_uid,report_text,report_text_html,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created)
									             values(@id,@study_hdr_id,@study_uid,@report_text,@report_text_html,'Y',@disclaimer_reason_id,@disclaimer_text,'00000000-0000-0000-0000-000000000000',@updated_by,getdate())
				end

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='035'
					return 0
				end


			exec common_study_user_activity_trail_save
				@study_hdr_id = @study_hdr_id,
				@study_uid    ='',
				@menu_id      = @menu_id,
				@activity_text = 'Updated Final Report',
				@session_id    = @session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

		   if(@return_status=0)
			begin
				rollback transaction
				return 0
			end
		end

	if(@report_type='D')
		begin
			if(select count(report_id) from study_hdr_dictated_reports where study_hdr_id = @study_hdr_id)>0
				begin
					update study_hdr_dictated_reports 
					set rating = @rating,
					    rating_reason_id=@rating_reason_id
					where study_hdr_id = @study_hdr_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='376'
							return 0
						end

					set @activity_text = 'Report rated as ' + @rating
					exec common_study_user_activity_trail_save
						@study_hdr_id  = @study_hdr_id,
						@study_uid     ='',
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

					select @rad_id =prelim_radiologist_id from study_hdr where id=@study_hdr_id

					if(isnull(@rad_id,'00000000-0000-0000-0000-000000000000'))= '00000000-0000-0000-0000-000000000000'
						begin
							select @radiologist_name = name from radiologists where id = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')
							
							update study_hdr
							set prelim_radiologist_id = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000'),
							    prelim_radiologist_pacs = isnull(@radiologist_name,''),
								radiologist_id         = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000'),
						        radiologist_pacs	   = isnull(@radiologist_name,''),
								rpt_record_date         = getdate()
							where id= @study_hdr_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='377'
									return 0
								end
						end
				end
			
		end
	else if(@report_type='P')
		begin
			if(select count(report_id) from study_hdr_prelim_reports where study_hdr_id = @study_hdr_id)>0
				begin
					update study_hdr_prelim_reports 
					set rating = @rating,
					    rating_reason_id =@rating_reason_id
					where study_hdr_id = @study_hdr_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='376'
							return 0
						end

					set @activity_text = 'Report rated as ' + @rating
					exec common_study_user_activity_trail_save
						@study_hdr_id  = @study_hdr_id,
						@study_uid     ='',
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

					select @radiologist_name = name from radiologists where id = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')
					select @rad_id =prelim_radiologist_id from study_hdr where id=@study_hdr_id

					if(isnull(@rad_id,'00000000-0000-0000-0000-000000000000'))= '00000000-0000-0000-0000-000000000000'
						begin
							
							update study_hdr
							set prelim_radiologist_id = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000'),
								prelim_radiologist_pacs = isnull(@radiologist_name,''),
								rpt_record_date         = getdate()
							where id= @study_hdr_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='377'
									return 0
								end
								
						end

					select @rad_id =radiologist_id from study_hdr where id=@study_hdr_id
					if(isnull(@rad_id,'00000000-0000-0000-0000-000000000000'))= '00000000-0000-0000-0000-000000000000'
						begin
							
							update study_hdr
							set radiologist_id = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000'),
								radiologist_pacs = isnull(@radiologist_name,'')
							where id= @study_hdr_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='377'
									return 0
								end
								
						end
				end
			
		end
	else if(@report_type='F')
		begin
			if(select count(report_id) from study_hdr_prelim_reports where study_hdr_id = @study_hdr_id)>0
				begin
					select @reading_radiologist_id = isnull((select id from radiologists where login_user_id = (select created_by from study_hdr_prelim_reports where study_hdr_id = @study_hdr_id)),'00000000-0000-0000-0000-000000000000')
					
					update study_hdr_prelim_reports set rating =@rating where study_hdr_id = @study_hdr_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='376'
							return 0
						end

					set @activity_text = 'Report rated as ' + @rating
					exec common_study_user_activity_trail_save
						@study_hdr_id  = @study_hdr_id,
						@study_uid     ='',
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
				end
			else if(select count(report_id) from study_hdr_dictated_reports where study_hdr_id = @study_hdr_id)>0
				begin
					select @reading_radiologist_id = isnull((select id from radiologists where login_user_id = (select created_by from study_hdr_dictated_reports where study_hdr_id = @study_hdr_id)),'00000000-0000-0000-0000-000000000000')

					update study_hdr_dictated_reports set rating =@rating,rating_reason_id=@rating_reason_id where study_hdr_id = @study_hdr_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='376'
							return 0
						end

					set @activity_text = 'Report rated as ' + @rating
					exec common_study_user_activity_trail_save
						@study_hdr_id  = @study_hdr_id,
						@study_uid     ='',
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
				end

			select @rad_id = final_radiologist_id from study_hdr where id=@study_hdr_id
			if(isnull(@rad_id,'00000000-0000-0000-0000-000000000000'))= '00000000-0000-0000-0000-000000000000'
				begin
					select @radiologist_name = name from radiologists where id = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')

					update study_hdr
					set final_radiologist_id   = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000'),
						final_radiologist_pacs = isnull(@radiologist_name,''),
						rpt_approve_date       = getdate()
					where id= @study_hdr_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='378'
							return 0
						end

					if(isnull(@reading_radiologist_id,'00000000-0000-0000-0000-000000000000')<>'00000000-0000-0000-0000-000000000000')
						begin
							select @reading_radiologist_name = name from radiologists where id = @reading_radiologist_id

							if(isnull((select radiologist_id from study_hdr where id=@study_hdr_id),'00000000-0000-0000-0000-000000000000'))='00000000-0000-0000-0000-000000000000'
								begin
									update study_hdr
									set radiologist_id   = isnull(@reading_radiologist_id,'00000000-0000-0000-0000-000000000000'),
										radiologist_pacs = isnull(@reading_radiologist_name,'')
									where id= @study_hdr_id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_status=0,@error_code='384'
											return 0
										end
							    end

							select @rad_id = prelim_radiologist_id from study_hdr where id=@study_hdr_id
							if(isnull(@rad_id,'00000000-0000-0000-0000-000000000000'))='00000000-0000-0000-0000-000000000000'
								begin
									update study_hdr
									set prelim_radiologist_id   = isnull(@reading_radiologist_id,'00000000-0000-0000-0000-000000000000'),
										prelim_radiologist_pacs = isnull(@reading_radiologist_name,'')
									where id= @study_hdr_id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_status=0,@error_code='377'
											return 0
										end
							    end
						end
					else
						begin
							update study_hdr
							set radiologist_id   = isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000'),
								radiologist_pacs = isnull(@radiologist_name,'')
							where id= @study_hdr_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='384'
									return 0
								end
						end 
				end
		end

	if(@write_back='Y')
		begin
			

			if(@curr_status_id <> @status_id)
				begin
					select @vrs_status_id = vrs_status_id
					from sys_study_status_pacs 
					where status_id = @status_id

					update study_hdr
					set study_status      = @vrs_status_id,
						study_status_pacs = @status_id,
						mark_to_teach     = @mark_to_teach,
						pacs_wb ='Y',
						status_last_updated_on = getdate(),
						updated_by = @updated_by,
						date_updated = getdate()
					where id = @study_hdr_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='299'
							return 0
						end
				end

			insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,updated_by,date_updated)
			                              values(@study_hdr_id,@study_uid,@curr_status_id,@status_id,@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='142'
					return 0
				end

			select @curr_status_desc = status_desc
			from sys_study_status_pacs 
			where status_id = @curr_status_id

			select @status_desc = status_desc
			from sys_study_status_pacs 
			where status_id = @status_id

			set @activity_text = 'Updated status from ' + @curr_status_desc + ' to ' + @status_desc
			exec common_study_user_activity_trail_save
				@study_hdr_id  = @study_hdr_id,
				@study_uid     ='',
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

		   if(@status_id = 100)
			begin
				exec common_study_mis_data_update
					@id            = @study_hdr_id,
					@updated_by    = @updated_by,
					@error_code    = @error_code output,
					@return_status = @return_status output

				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end
			end 
			
				

			/*************************GENERATE NOTIFICATION*****************************/
			declare @recipient_address nvarchar(500),
					@recipient_name nvarchar(200),
					@recipient_mobile nvarchar(500),
					@recipient_fax_no nvarchar(30),
					@custom_report nchar(1),
					@fax_report nchar(1),
					@fax_file_name nvarchar(max),
					@patient_name nvarchar(200),
					@VRSPACSLINKURL nvarchar(200),
					@study_types varchar(max),
					@email_subject varchar(250),
			        @email_text nvarchar(max),
					@sms_text nvarchar(max),
					@RPTMAILUSRACCT nvarchar(100),
				    @RPTMAILUSRPWD nvarchar(100),
					@SMSSENDERNO nvarchar(200),
					@FAXENABLE nchar(1),
					@mobile_no nvarchar(20),
         			@final_rpt_release_datetime datetime,
			        @release nchar(1),
					@counter int,
					@rowcount int

			select @RPTMAILUSRACCT = data_type_string from general_settings where control_code='RPTMAILUSRACCT'
			select @RPTMAILUSRPWD = data_type_string from general_settings where control_code='RPTMAILUSRPWD'
			

			if(@report_type = 'P' or @report_type='F')
				begin
					select @recipient_address = isnull(physician_email,''),
						   @recipient_name    = isnull(physician_name,''),
						   @recipient_mobile  = isnull(physician_mobile,'')
					from institution_physician_link
					where institution_id = @institution_id
					and physician_id = @physician_id

					select @VRSPACSLINKURL = data_type_string
					from general_settings
					where control_code ='VRSPACSLINKURL'

					select @FAXENABLE = data_type_string
					from general_settings
					where control_code ='FAXENABLE'

					select @patient_name               = patient_name,
					       @final_rpt_release_datetime = final_rpt_release_datetime
					from study_hdr
					where id= @study_hdr_id
					and study_uid = @study_uid

				    if(@report_type='P')
						begin
							set @release ='Y'
						end
					else if(@report_type='F')
						begin
							if(@final_rpt_release_datetime<getdate())
								begin
									set @release='Y'
								end
							else
								begin
									set @release='N'
								end
						end

					if(isnull(@recipient_address,'') <> '')
						begin
							set @VRSPACSLINKURL = @VRSPACSLINKURL + convert(varchar(36),@study_hdr_id) + '&vw=rpt&fmt=1'

							set @study_types=''
							select @study_types = @study_types +  convert(varchar,st.name) + ','
							from study_hdr_study_types shst
							inner join  modality_study_types st on st.id= shst.study_type_id
							where shst.study_hdr_id=@study_hdr_id 

							if(isnull(@study_types,'')<>'') select @study_types = substring(@study_types,1,len(@study_types)-1)
						

							set @email_subject = @report_type_text + ' Report Available For ' + isnull(@patient_name,'')
						
							set @email_text    = @report_type_text + ' report is available for :- \n\n'
							set @email_text    = @email_text + ' Patient    : ' + @patient_name + '\n'
							set @email_text    = @email_text + ' Species    : ' + isnull((select name from species where id = (select species_id from study_hdr where id=@study_hdr_id and study_uid = @study_uid)),'') + '\n'
							set @email_text    = @email_text + ' Breed      : ' + isnull((select name from breed where id = (select breed_id from study_hdr where id=@study_hdr_id and study_uid = @study_uid)),'') + '\n'
							set @email_text    = @email_text + ' Modality   : ' + isnull((select name from modality where id = (select modality_id from study_hdr where id=@study_hdr_id and study_uid = @study_uid)),'') + '\n'
							set @email_text    = @email_text + ' Study Type : ' + isnull(@study_types,'') + '\n\n'
							set @email_text    = @email_text +'<a href=''' + @VRSPACSLINKURL + ''' target=''_blank''>Click here to view the report</a>\n\n'
							set @email_text    = @email_text + 'Also, please check the PDF version of the report attached with this email.'
							set @email_text    = @email_text + '\n\n'
							set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'


							insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,
												  email_subject,email_text,study_hdr_id,study_uid,sender_email_address,sender_email_password,
												  email_type,release_email,date_updated)
										   values(newid(),getdate(),@recipient_address,@recipient_name,
												  @email_subject,@email_text,@study_hdr_id,@study_uid,@RPTMAILUSRACCT,@RPTMAILUSRPWD,
												  'RPT',@release,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_status=0,@error_code='371'
									return 0
								end

							set @activity_text =  @report_type_text + ' report email queued for sending'
							exec common_study_user_activity_trail_save
								@study_hdr_id  = @study_hdr_id,
								@study_uid     ='',
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

							if(@report_type = 'P')
								begin
									update study_hdr set prelim_rpt_updated = 'Y' where id =@study_hdr_id
								end
							else if(@report_type = 'F')
								begin
									update study_hdr set final_rpt_updated = 'Y' where id =@study_hdr_id
								end
						end

					if(isnull(@recipient_mobile,'') <> '')
						begin
							create table #tmpMobile
							(
								rec_id int identity(1,1),
								mobile_no nvarchar(20)
							)

							select @patient_name = patient_name
							from study_hdr
							where id= @study_hdr_id
							and study_uid = @study_uid

							select @SMSSENDERNO = data_type_string
							from general_settings
							where control_code ='SMSSENDERNO'

							if(charindex(convert(varchar(36),@study_hdr_id),@VRSPACSLINKURL)=0)
								begin
									set @VRSPACSLINKURL = @VRSPACSLINKURL + convert(varchar(36),@study_hdr_id) + '&vw=rpt&fmt=1' 
								end

							set @sms_text    = @report_type_text + ' report available for ' + isnull(@patient_name,'')

							if(charindex(';',@recipient_mobile) = 0 )
								begin
									insert into #tmpMobile(mobile_no) values(@recipient_mobile)
								end
							else
								begin
									while(charindex(';',@recipient_mobile) > 0)
										begin
											select @mobile_no = substring(@recipient_mobile,1,charindex(';',@recipient_mobile)-1)
											insert into #tmpMobile(mobile_no) values(rtrim(ltrim(@mobile_no)))
											select @recipient_mobile = substring(@recipient_mobile,charindex(';',@recipient_mobile) + 1,len(@recipient_mobile) - len(substring(@recipient_mobile,1,CHARINDEX(';',@recipient_mobile)-1)))
										end
								end

							select @counter=1,
								   @rowcount = count(rec_id) 
							from #tmpMobile

							set @mobile_no=''

							while(@counter <= @rowcount)
								begin
									select @mobile_no = mobile_no
									from #tmpMobile
									where rec_id = @counter

									insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,recipient_no,recipient_name,sender_no,sequence_no,sms_text,study_hdr_id,study_uid,
									                    sms_type,release_sms,date_updated)
											values(newid(),getdate(),@mobile_no,@recipient_name,@SMSSENDERNO,1,@sms_text,@study_hdr_id,@study_uid,
											       'RPT',@release,getdate())

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_status=0,@error_code='372'
											return 0
										end

									set @sms_text    = 'Click ' + @VRSPACSLINKURL + ' to view the report'

									insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,recipient_no,recipient_name,sender_no,sequence_no,sms_text,study_hdr_id,study_uid,
									                    sms_type,release_sms,date_updated)
												values(newid(),getdate(),@mobile_no,@recipient_name,@SMSSENDERNO,2,@sms_text,@study_hdr_id,@study_uid,
												       'RPT',@release,getdate())

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_status=0,@error_code='372'
											return 0
										end

									set @activity_text =  @report_type_text + ' report sms queued for sending'
									exec common_study_user_activity_trail_save
										@study_hdr_id  = @study_hdr_id,
										@study_uid     ='',
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

									set @counter= @counter + 1
								end

							if(@report_type = 'P')
								begin
									update study_hdr set prelim_sms_updated = 'Y' where id =@study_hdr_id
								end
							else if(@report_type = 'F')
								begin
									update study_hdr set final_sms_updated = 'Y' where id =@study_hdr_id
								end

							drop table #tmpMobile

						end

					if(@FAXENABLE='Y')
						begin
							select @fax_report = fax_rpt,
							       @recipient_fax_no = fax_no,
								   @custom_report= custom_report
							from institutions
							where id = @institution_id
							
							if(@fax_report='Y' and rtrim(ltrim(isnull(@recipient_fax_no,'')))<>'')
								begin
									set @patient_name = replace(@patient_name,' ','_')
									set @patient_name = replace(@patient_name,'''','')
									set @patient_name = replace(@patient_name,',','_')
									set @patient_name = replace(@patient_name,'__','_')

									set @fax_file_name = upper(@report_type_text) + '_REPORT_' + upper(@patient_name) +'.pdf'

									insert into vrslogdb..fax_log(id,log_datetime,recipient_no,study_hdr_id,study_uid,file_name,institution_id,report_type,custom_report,
									                    fax_type,release_fax,date_updated)
											     values(newid(),getdate(),@recipient_fax_no,@study_hdr_id,@study_uid,@fax_file_name,@institution_id,@report_type,@custom_report,
												        'RPT',@release,getdate())

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_status=0,@error_code='447'
											return 0
										end

									set @activity_text =  @report_type_text + ' report fax queued for sending'
									exec common_study_user_activity_trail_save
										@study_hdr_id  = @study_hdr_id,
										@study_uid     ='',
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
								end



						end
				end
			/*************************GENERATE NOTIFICATION*****************************/

		end

	commit transaction
	set @return_status=1
	set @error_code='034'
	set nocount off
	return 1	

end


GO
