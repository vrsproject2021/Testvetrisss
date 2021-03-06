USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_file_details_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_downloaded_file_details_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_file_details_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_downloaded_file_details_save : 
                  save downloaded file details
** Created By   : Pavel Guha
** Created On   : 12/04/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_downloaded_file_details_save]
    @study_uid nvarchar(100),
	@study_date datetime='01jan1900',
	@institution_code nvarchar(5),
	@institution_name nvarchar(100),
	@patient_id nvarchar(20) = null,
	@patient_fname nvarchar(40) = null,
	@patient_lname nvarchar(40) = null,
	@file_name nvarchar(250),
	@accession_no nvarchar(20)=null,
	@reason nvarchar(500)=null,
	@modality nvarchar(50)=null,
	@manufacturer_name nvarchar(100)=null,
	@device_serial_no nvarchar(20)=null,
	@manufacturer_model_no nvarchar(100)=null,
	@modality_ae_title nvarchar(50)=null,
	@referring_physician nvarchar(200)=null,
	@patient_sex nvarchar(10)=null,
	@patient_dob datetime=null,
	@patient_age varchar(50)=null,
	@priority_id int = 0,
	@import_session_id nvarchar(30) = null, 
	@is_manual nchar(1)='N',
	@is_listener_file nchar(1)='N',
	@series_uid nvarchar(100),
	@sop_instance_uid nvarchar(100),
	@delete_file nchar(1)='N' output,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @institution_id uniqueidentifier,
	        @file_count int,
			@id uniqueidentifier,
			@modality_id int,
			@device_id uniqueidentifier,
			@physician_id uniqueidentifier,
			@dcm_file_xfer_pacs_mode nchar(1),
			@finishing_datetime datetime,
			@finishing_time_hrs int,
			@is_add_on nchar(1),
			@received_via_dicom_router nchar(1),
			@FNLRPTAUTORELHR int,
			@is_stat nchar(1),
			@sender_time_offset_mins int,
			@delv_time nvarchar(130),
			@next_operation_time nvarchar(130),
			@display_message nvarchar(250),
			@synched_on datetime,
			@final_rpt_release_datetime datetime,
			@final_rpt_release_hrs int,
			@is_forwarded nchar(1),
			@via_DR nchar(1),
			@sync_mode nvarchar(5),
			@error_code nvarchar(10),
			@return_status int

	-- Temporarirly Added
	declare @email_subject nvarchar(250),
		    @email_text varchar(8000),
		    @SUPPMAILID nvarchar(200),
			@email_log_id uniqueidentifier,
			@approve_for_pacs nchar(1),
			@approved_by uniqueidentifier,
			@date_approved datetime


	select @SUPPMAILID = data_type_string
	from general_settings
	where control_code='SUPPMAILID'
	-- Temporarirly Added

	set @is_add_on='N'

	begin transaction

	if(select count(study_hdr_id) from study_hdr_dcm_files where study_uid=@study_uid)>0
		begin
			set @is_add_on='Y'
		end

	set @is_forwarded='N'
	if(@is_manual = 'N' and @is_add_on='N') 
		begin
			if(select charindex('_S1DXXX',@file_name))>0
				begin
					set @is_forwarded= 'Y'
				end
		end

	set @via_DR ='N'
	if(@is_manual = 'N' and @is_add_on='N' and @is_forwarded='N') 
		begin
			if(select charindex('_S1D',@file_name))>0
				begin
					set @via_DR= 'Y'
				end
		end

	set @institution_id = isnull((select id 
	                        from institutions
							where code = @institution_code),'00000000-0000-0000-0000-000000000000')

	set @institution_name = isnull((select name 
									from institutions
									where id = @institution_id),'')

	--set @dcm_file_xfer_pacs_mode = (select dcm_file_xfer_pacs_mode  
	--								from institutions
	--								where id = @institution_id)

    set @delete_file='N'

	--if(@is_listener_file = 'Y') 
	--	begin
	--		set @approve_for_pacs='N'
	--		set @date_approved = null

	--		if(select count(study_uid) from dicom_router_files_received where study_uid=@study_uid  and file_series_uid=@series_uid and file_instance_no=@sop_instance_uid)=0
	--			begin
	--				insert into dicom_router_files_received(study_uid,import_session_id,institution_id,institution_code,
	--														file_name,file_type,file_series_uid,file_instance_no,date_received)
	--												values(@study_uid,@import_session_id,@institution_id,@institution_code,
	--													   @file_name,'D',@series_uid,@sop_instance_uid,getdate())

	--				if(@@rowcount=0)
	--					begin
	--						rollback transaction
	--						select @return_type=0,@error_msg='Failed to import study file record - SUID ' + @study_uid
	--						return 0
	--					end
	--			end
	--		else
	--			begin
	--				rollback transaction
	--				select @return_type=0,@error_msg='' + @study_uid,@delete_file='Y'
	--				return 0
	--			end
	--	end
	--else 
	if(@is_manual = 'Y') 
		begin
			set @approve_for_pacs='Y'
			set @date_approved = getdate()

			if(select count(study_uid) from dicom_router_files_received where study_uid=@study_uid  and file_series_uid=@series_uid and file_instance_no=@sop_instance_uid)=0
				begin
					insert into dicom_router_files_received(study_uid,import_session_id,institution_id,institution_code,
															file_name,file_type,file_series_uid,file_instance_no,date_received)
													values(@study_uid,@import_session_id,@institution_id,@institution_code,
														   @file_name,'D',@series_uid,@sop_instance_uid,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to import study file record - SUID ' + @study_uid
							return 0
						end
				end
			else
				begin
					rollback transaction
					select @return_type=0,@error_msg='' + @study_uid,@delete_file='Y'
					return 0
				end
		end
	else if(@is_forwarded = 'Y') 
		begin

			if(select count(study_uid) from dicom_router_files_received where study_uid=@study_uid and file_series_uid=@series_uid and file_instance_no=@sop_instance_uid)=0
				begin
					insert into dicom_router_files_received(study_uid,import_session_id,institution_id,institution_code,
															file_name,file_type,file_series_uid,file_instance_no,date_received)
													values(@study_uid,@import_session_id,@institution_id,@institution_code,
														   @file_name,'D',@series_uid,@sop_instance_uid,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to import study file record - SUID ' + @study_uid
							return 0
						end
				end
			else
				begin
					rollback transaction
					select @return_type=0,@error_msg='' + @study_uid,@delete_file='Y'
					return 0
				end
		end
	else if(@via_DR = 'Y') 
		begin
			set @approve_for_pacs='N'
			set @date_approved = null

			if(select count(study_uid) from dicom_router_files_received where study_uid=@study_uid and file_series_uid=@series_uid and file_instance_no=@sop_instance_uid)=0
				begin
					insert into dicom_router_files_received(study_uid,import_session_id,institution_id,institution_code,
															file_name,file_type,file_series_uid,file_instance_no,date_received)
													values(@study_uid,@import_session_id,@institution_id,@institution_code,
														   @file_name,'D',@series_uid,@sop_instance_uid,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to import study file record - SUID ' + @study_uid
							return 0
						end
				end
			else
				begin
					rollback transaction
					select @return_type=0,@error_msg='' + @study_uid,@delete_file='Y'
					return 0
				end
		end
	else 
		begin
			set @approve_for_pacs='Y'
			set @date_approved = getdate()
		end

	if(select count(study_uid) from scheduler_file_downloads where study_uid=@study_uid)=0
		begin
			if(@is_add_on='Y')
				begin
					insert into scheduler_file_downloads(id,study_uid,study_date,file_count,received_date,
														institution_id,institution_code,institution_name,
														patient_id,patient_fname,patient_lname,patient_sex,patient_dob,patient_age,
														accession_no,reason,modality,
														manufacturer_name,device_serial_no,manufacturer_model_no,modality_ae_title,
														referring_physician,priority_id,date_downloaded,approve_for_pacs,approved_by,date_approved,
														import_session_id,is_manual)
												 (select sh.id,study_uid,sh.study_date,1,sh.received_date,
														 sh.institution_id,isnull(i.code,''),isnull(i.name,''),
														 sh.patient_id,sh.patient_fname,sh.patient_lname,sh.patient_sex,sh.patient_dob_accepted,sh.patient_age_accepted,
														 sh.accession_no,sh.reason_accepted,isnull(m.code,''),
														 sh.manufacturer_name,sh.device_serial_no,sh.manufacturer_model_no,sh.modality_ae_title,
														 isnull(p.name,''),sh.priority_id,getdate(),'Y',@approved_by,@date_approved,
														 @import_session_id,@is_manual
												  from study_hdr sh
												  left outer join institutions i on i.id = sh.institution_id
												  left outer join modality m on m.id = sh.modality_id
												  left outer join physicians p on p.id = sh.physician_id
												  where study_uid=@study_uid)	
				end
			else if(@is_manual='Y')
				begin
					select  @id          = id,
							@approved_by = updated_by 
					from study_hdr 
					where study_uid =@study_uid
					
					insert into scheduler_file_downloads(id,study_uid,study_date,file_count,received_date,
														institution_id,institution_code,institution_name,
														patient_id,patient_fname,patient_lname,patient_sex,patient_dob,patient_age,
														accession_no,reason,modality,
														manufacturer_name,device_serial_no,manufacturer_model_no,modality_ae_title,
														referring_physician,priority_id,date_downloaded,approve_for_pacs,approved_by,date_approved,
														import_session_id,is_manual)
												 (select sh.id,study_uid,sh.study_date,1,sh.received_date,
														 sh.institution_id,isnull(i.code,''),isnull(i.name,''),
														 sh.patient_id,sh.patient_fname,sh.patient_lname,sh.patient_sex,sh.patient_dob_accepted,sh.patient_age_accepted,
														 sh.accession_no,sh.reason_accepted,isnull(m.code,''),
														 sh.manufacturer_name,sh.device_serial_no,sh.manufacturer_model_no,sh.modality_ae_title,
														 isnull(p.name,''),sh.priority_id,getdate(),'Y',@approved_by,@date_approved,
														 @import_session_id,@is_manual
												  from study_hdr sh
												  left outer join institutions i on i.id = sh.institution_id
												  left outer join modality m on m.id = sh.modality_id
												  left outer join physicians p on p.id = sh.physician_id
												  where study_uid=@study_uid)	
				end
			--else if(@is_listener_file='Y')
			--	begin
			--		set @id = newid()
			--	    insert into scheduler_file_downloads(id,study_uid,study_date,file_count,received_date,
			--											institution_id,institution_code,institution_name,
			--											patient_id,patient_fname,patient_lname,patient_sex,patient_dob,patient_age,
			--											accession_no,reason,modality,
			--											manufacturer_name,device_serial_no,manufacturer_model_no,modality_ae_title,
			--											referring_physician,priority_id,date_downloaded,approve_for_pacs,approved_by,date_approved,
			--											import_session_id,is_manual)
			--									 values(@id,@study_uid,@study_date,1,getdate(),
			--											@institution_id,@institution_code,@institution_name,
			--											@patient_id,@patient_fname,@patient_lname,@patient_sex,@patient_dob,@patient_age,
			--											@accession_no,@reason,@modality,
			--											@manufacturer_name,@device_serial_no,@manufacturer_model_no,@modality_ae_title,
			--											@referring_physician,@priority_id,getdate(),@approve_for_pacs,'00000000-0000-0000-0000-000000000000',@date_approved,
			--											@import_session_id,@is_manual)	
			--	end
			else if(@via_DR='Y')
				begin
					set @id = newid()
				    insert into scheduler_file_downloads(id,study_uid,study_date,file_count,received_date,
														institution_id,institution_code,institution_name,
														patient_id,patient_fname,patient_lname,patient_sex,patient_dob,patient_age,
														accession_no,reason,modality,
														manufacturer_name,device_serial_no,manufacturer_model_no,modality_ae_title,
														referring_physician,priority_id,date_downloaded,approve_for_pacs,approved_by,date_approved,
														import_session_id,is_manual)
												 values(@id,@study_uid,@study_date,1,getdate(),
														@institution_id,@institution_code,@institution_name,
														@patient_id,@patient_fname,@patient_lname,@patient_sex,@patient_dob,@patient_age,
														@accession_no,@reason,@modality,
														@manufacturer_name,@device_serial_no,@manufacturer_model_no,@modality_ae_title,
														@referring_physician,@priority_id,getdate(),@approve_for_pacs,'00000000-0000-0000-0000-000000000000',@date_approved,
														@import_session_id,@is_manual)	
				end
		   

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to create details of Study UID ' + @study_uid
					return 0
				end

			insert into scheduler_file_downloads_dtls(id,study_uid,file_name,series_uid,instance_no,import_session_id)
			                                   values(@id,@study_uid,@file_name,@series_uid,@sop_instance_uid,@import_session_id)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to create file details of Study UID ' + @study_uid
					return 0
				end


			--update study_hdr
			--if(((@dcm_file_xfer_pacs_mode = 'M' or @is_listener_file='Y') and @is_add_on='N' and @is_manual='N'))
			if(	@via_DR='Y')
				begin
					
					if(select count(study_uid) from study_synch_dump where study_uid=@study_uid)=0
						begin
							set @priority_id = isnull(@priority_id,0)
							insert into study_synch_dump(study_uid,study_date,received_date,accession_no,reason,
														institution_name,manufacturer_name,manufacturer_model_no,device_serial_no,modality_ae_title,referring_physician,
														patient_id,patient_name,patient_sex,patient_dob,patient_age,patient_weight,sex_neutered,
														owner_name,species,breed,modality,body_part,img_count,study_desc,priority_id,object_count,synched_on)
												 values(@study_uid,@study_date,getdate(),@accession_no,@reason,
														@institution_name,@manufacturer_name,@manufacturer_model_no,@device_serial_no,@modality_ae_title,@referring_physician,
														@patient_id,rtrim(ltrim(@patient_fname + ' ' + @patient_lname)),@patient_sex,@patient_dob,@patient_age,0,'',
														'','','',@modality,'',1,'',@priority_id,1,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to synch Study UID ' + @study_uid
									return 0
								end

							select @finishing_time_hrs = finishing_time_hrs,
							       @is_stat = is_stat
							from sys_priority 
							where priority_id = @priority_id

							set @finishing_time_hrs = isnull(@finishing_time_hrs,0)
							select @synched_on = getdate()
							select @FNLRPTAUTORELHR = data_type_string from general_settings where control_code='FNLRPTAUTORELHR'
							select @sender_time_offset_mins = id from sys_us_time_zones where is_default='Y'

							exec common_check_operation_time
								@priority_id             = @priority_id,
								@sender_time_offset_mins = @sender_time_offset_mins,
								@submission_date         = @synched_on,
								@next_operation_time     = @next_operation_time output,
								@delv_time               = @delv_time output,
								@display_message         = @display_message output,
								@error_code              = @error_msg output,
								@return_status           = @return_type output

						   if(@is_stat='Y')
								begin
									 set @finishing_datetime = convert(datetime,@delv_time)
									 set @final_rpt_release_datetime = dateadd(mi,(select final_report_release_time_mins from sys_priority where priority_id=@priority_id),getdate())
								end
						   else
								begin
									--set @finishing_datetime = dateadd(HH,@finishing_time_hrs,@synched_on)
									--set @final_rpt_release_hrs = @finishing_time_hrs - @FNLRPTAUTORELHR
									--select @final_rpt_release_datetime = dateadd(HH,@final_rpt_release_hrs * -1,@finishing_datetime)
									select @final_rpt_release_datetime = dateadd(HH,@FNLRPTAUTORELHR,@synched_on)
								end

							if(@via_DR='Y') 
								begin
									set @received_via_dicom_router='Y'
									set @sync_mode='DR'
								end
							else 
								begin
									set @received_via_dicom_router='N'
									set @sync_mode='PACS'
								end

							insert into study_hdr(id,study_uid,study_date,received_date,
													accession_no_pacs,accession_no,
													reason_pacs,reason_accepted,
													institution_name_pacs,institution_id,manufacturer_name,device_serial_no,modality_ae_title,referring_physician_pacs,
													patient_id_pacs,patient_id,
													patient_name_pacs,patient_name,patient_fname,patient_lname,
													patient_sex_pacs,patient_sex,
													patient_sex_neutered_pacs,patient_sex_neutered,
													patient_dob_pacs,patient_dob_accepted,
													patient_age_pacs,patient_age_accepted,
													patient_weight_pacs,patient_weight,
													owner_name_pacs,owner_first_name,owner_last_name,
													species_pacs,breed_pacs,modality_pacs,body_part_pacs,study_status_pacs,
													img_count_pacs,img_count,object_count,study_desc,
													priority_id_pacs,priority_id,received_via_dicom_router,sync_mode,
													finishing_datetime,final_rpt_release_datetime,
													synched_on,date_updated,status_last_updated_on)
											values(@id,@study_uid,@study_date,getdate(),
												@accession_no,@accession_no,
												@reason,@reason,
												@institution_name,@institution_id,@manufacturer_name,@device_serial_no,@modality_ae_title,@referring_physician,
												@patient_id,@patient_id,
												rtrim(ltrim(@patient_fname + ' ' + @patient_lname)),rtrim(ltrim(@patient_fname + ' ' + @patient_lname)),@patient_fname,@patient_lname,
												@patient_sex,@patient_sex,
												'','',
												@patient_dob,@patient_dob,
												@patient_age,@patient_age,
												0,0,
												'','','',
												'','',@modality,'',0,
												1,1,1,'',
												@priority_id,0,@received_via_dicom_router,@sync_mode,
												@finishing_datetime,@final_rpt_release_datetime,
												@synched_on,@synched_on,@synched_on)	

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to synch Study UID ' + @study_uid
									return 0
								end

							insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
														   values(@id,@study_uid,0,0,getdate())
			
							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update status log of Study UID ' + @study_uid
									return 0
								end

							if(rtrim(ltrim(@modality)) <> '')
								begin
									set @modality_id= isnull((select id from
									(select top 1 id,name
									from modality
									where is_active='Y'
									and (upper(name)=upper(@modality)
											or upper(name) like '%' + upper(@modality) + '%'
											or code = upper(@modality)
											or code like '%' + @modality + '%'
											or upper(dicom_tag) = upper(@modality)
											or upper(dicom_tag) like '%' + upper(@modality) + '%') order by name)t),0)

									update study_hdr set modality_id = @modality_id where id=@id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @return_type=0,@error_msg='Failed to update modality id for Study UID ' + @study_uid
											return 0
										end
								end

							if(@manufacturer_name<>'')
								begin
									if(select count(device_id) from  institution_device_link where upper(manufacturer)=upper(@manufacturer_name) and upper(modality)=upper(@modality) and upper(modality_ae_title)= upper(@modality_ae_title) and institution_id = @institution_id)=0
										begin
											set @device_id=newid()

											insert into institution_device_link(device_id,institution_id,manufacturer,modality,serial_no,modality_ae_title,created_by,date_created)
																			values(@device_id,@institution_id,@manufacturer_name,@modality,@device_serial_no,@modality_ae_title,'00000000-0000-0000-0000-000000000000',getdate())

											if(@@rowcount=0)
												begin
													rollback transaction
													select @return_type=0,@error_msg='Failed to create manufucaturer details for Study UID ' + @study_uid
													return 0
												end
										end
								end		
				
							if(rtrim(ltrim(@referring_physician))<>'')
								begin
									if(select count(id) from physicians where upper(name) = upper(@referring_physician) and is_active = 'Y') >0
										begin
											select @physician_id=isnull((select id from physicians
																			where upper(name) = upper(@referring_physician)
																			and is_active = 'Y'),'00000000-0000-0000-0000-000000000000')
											update study_hdr set physician_id = @physician_id where id=@id

											if(@@rowcount=0)
												begin
													rollback transaction
													select @return_type=0,@error_msg='Failed to update physician id for Study UID ' + @study_uid
													return 0
												end

							
											if(select count(institution_id) from institution_physician_link where institution_id=@institution_id and physician_id =@physician_id)=0
												begin
													insert into institution_physician_link(institution_id,physician_id,created_by,date_created)
																					values(@institution_id,@physician_id,'00000000-0000-0000-0000-000000000000',getdate())
												end
								
										end
								end	

							--if(@is_listener_file='Y')
							--	begin
							--		exec common_study_user_activity_trail_save
							--			@study_hdr_id = @id,
							--			@study_uid    = @study_uid,
							--			@menu_id      = 0,
							--			@activity_text = 'Received Via Listener',
							--			@activity_by   = '00000000-0000-0000-0000-000000000000',
							--			@error_code    = @error_code output,
							--			@return_status = @return_status output
							--	end
							if(@via_DR='Y')
								begin
									exec common_study_user_activity_trail_save
										@study_hdr_id = @id,
										@study_uid    = @study_uid,
										@menu_id      = 0,
										@activity_text = 'Received Via DICOM Router',
										@activity_by   = '00000000-0000-0000-0000-000000000000',
										@error_code    = @error_code output,
										@return_status = @return_status output
								end

						   if(@return_status=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to create activity log  of Study UID ' + @study_uid + '.'
								return 0
							end
					    end
					--else --if(@is_manual <>'Y')
					--	begin
					--		rollback transaction
					--		select @return_type=2,@error_msg='Failed to create details of Study UID ' + @study_uid + '. This Study UID already exists.'
					--		return 0
					--	end	
				end	
		end
	else
		begin

			 select @id= id 
			 from scheduler_file_downloads
			 where study_uid    = @study_uid
			 --and institution_id = @institution_id

			 set @id =isnull(@id,'00000000-0000-0000-0000-000000000000')

			 if(select count(id) from scheduler_file_downloads_dtls where id=@id and study_uid=@study_uid and series_uid=@series_uid and instance_no=@sop_instance_uid) = 0
				begin
					insert into scheduler_file_downloads_dtls(id,study_uid,file_name,series_uid,instance_no,import_session_id)
			                                           values(@id,@study_uid,@file_name,@series_uid,@sop_instance_uid,@import_session_id)

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=2,@error_msg='Failed to create file details of Study UID ' + @study_uid
							return 0
						end
				end
			else
				begin
					set @delete_file='Y'
					set @return_type=1
					set @error_msg=''

					return 0
				end

			
			select @file_count = count(file_name)
			from scheduler_file_downloads_dtls
			where id = @id

			update scheduler_file_downloads
			set study_date       = @study_date,
				file_count       = @file_count,
				date_downloaded  = getdate()
			where study_uid    = @study_uid
			--and institution_id = @institution_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update file count of Study UID ' + @study_uid
					return 0
				end 


			--if(((@dcm_file_xfer_pacs_mode = 'M' or @is_listener_file='Y') and @is_add_on='N'))
		    if(@via_DR='Y')
				begin
					if(select count(study_uid) from study_hdr where study_uid = @study_uid)>0
						begin
							update study_synch_dump
							set   img_count           = @file_count,
								  object_count        = @file_count
							where study_uid      = @study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update file count of Study UID ' + @study_uid
									return 0
								end

							update study_hdr
							set   img_count_pacs      = @file_count,
								  img_count           = @file_count,
								  object_count        = @file_count,
								  object_count_pacs   = @file_count
							where study_uid    = @study_uid

							if(@@rowcount=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failed to update file count of Study UID ' + @study_uid
									return 0
								end
						end
				end
		end

	---- Temporarirly Added
	--if(@institution_id='9305A40D-706B-47B9-8E2F-A4422E462053')
	--	begin
	--		/******************************************************************************************************************
	--				Email Format
	--			******************************************************************************************************************/
	--			set @email_subject = 'DICOM files uploaded from institution :' + @institution_name

	--			set @email_text    = 'File Details :- \n\n'
	--			set @email_text    = @email_text + ' File Name          : ' + @file_name + '\n'
	--			set @email_text    = @email_text + ' Study UID          : ' + @study_uid +'\n'
	--			set @email_text    = @email_text + ' Institution Code   : ' + @institution_code + '\n'
	--			set @email_text    = @email_text + ' Institution Name   : ' + @institution_name + '\n'
	--			set @email_text    = @email_text + '\n\n'
	--			set @email_text    = @email_text + 'Please check whether it is available in both eRAD and VETRIS\n'
	--			set @email_text    = @email_text + 'If not, please check the corrupted tags of the file in the VETRIS, path is :\n'
	--			set @email_text    = @email_text + 'D:\\VRSApp\\PACS_ARCHIVE\\' + @institution_code + '_' + @institution_name + '_' + @study_uid + '\n'
	--			set @email_text    = @email_text + 'ALSO LOOK FOR DATE OF BIRTH TAG\n'
	--			set @email_text    = @email_text + '\n\n'
	--			set @email_text    = @email_text +'This is an automated message from VETRIS.Please do not reply to the message.\n'

	--		set @email_log_id=newid()	
	--		insert into email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,
	--                              study_uid,email_type,file_name)
	--			            values(@email_log_id,getdate(),@SUPPMAILID,'RAD Support',@email_subject,@email_text,
	--			                   @study_uid,'TEMP',@file_name)

	--		if(@@rowcount = 0)
	--			begin
	--				rollback transaction
	--				set @error_msg='Failed to create notification for file received for ' + isnull(@institution_name,'')
	--				set @return_type=0
	--				return 0
	--			end
	--	end
	---- Temporarirly Added


    commit transaction
	set @return_type=1
	set @error_msg=''

	set nocount off
	return 1

end


GO
