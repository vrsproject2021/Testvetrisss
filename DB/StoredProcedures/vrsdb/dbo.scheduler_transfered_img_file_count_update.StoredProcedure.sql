USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_transfered_img_file_count_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_transfered_img_file_count_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_transfered_img_file_count_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_transfered_img_file_count_update : 
                  update downloaded image file transfer to PACS update
** Created By   : Pavel Guha
** Created On   : 20/08/2019
*******************************************************/
--exec scheduler_transfered_img_file_count_update '6b3e44db-0d2b-41a3-bda3-833c8e99387b','00074_S1D122619104404CY3_MORTHLAND_VETERINARY_F7wUpK_SimonMingLiRad2.jpg','',0
CREATE procedure [dbo].[scheduler_transfered_img_file_count_update]
	@id uniqueidentifier,
	@file_name nvarchar(250),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on
	set datefirst 1

	declare @xfer_file_count int,
			@file_count int,
	        @study_uid nvarchar(100),
			@consult_applied nchar(1),
			@storage_applied nchar(1),
			@service_codes nvarchar(250),
			@received_date datetime,
			@priority_id int,
			@finishing_datetime datetime,
			@finishing_time_hrs int,
			@beyond_hour_stat nchar(1),
			@sender_time_offset_mins int,
			@final_rpt_release_datetime datetime,
			@final_rpt_release_hrs int,
			@FNLRPTAUTORELHR int,
			@is_stat nchar(1),
			@in_exp_list nchar(1),
			@modality_id int,
			@species_id int,
			@institution_id uniqueidentifier,
			@delv_time nvarchar(130),
			@next_operation_time nvarchar(130),
			@activity_text nvarchar(max),
			@error_code nvarchar(10),
			@return_status int

	begin transaction 

	--select * from scheduler_img_file_downloads_grouped_dtls where id='DEBE6B56-A41D-4DAD-9949-466C3876B89E'

	update scheduler_img_file_downloads_grouped_dtls
	set sent_to_pacs='Y',
	    date_sent   = getdate()
	where id = @id
	and file_name = @file_name

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='Failed to update sent to PACS of file ' + @file_name
			print @error_msg
			return 0
		end

	
	select @xfer_file_count = count(file_name)
	from scheduler_img_file_downloads_grouped_dtls
	where sent_to_pacs='Y'
	and id = @id

	select @study_uid = study_uid,
	       @file_count = file_count
	from scheduler_img_file_downloads_grouped
	where id = @id

	update scheduler_img_file_downloads_grouped
	set file_xfer_count = @xfer_file_count
	where id = @id

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='1-Failed to update count of transfered file(s) to PACS of Study UID ' + @study_uid
			return 0
		end

	

	if(@xfer_file_count > @file_count)
		begin
			update scheduler_img_file_downloads_grouped
			set file_count = @xfer_file_count
			where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='1-Failed to update count of recived file(s) to PACS of Study UID ' + @study_uid
					return 0
				end

			update study_hdr
			set img_count = @xfer_file_count
			where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='2-Failed to update count of recived file(s) to PACS of Study UID ' + @study_uid
					return 0
				end
		end

	if(@xfer_file_count >=1)
		begin
			
			if(select count(study_uid) from study_hdr where study_uid=@study_uid)=0
				begin
					--exec scheduler_log_save
					--	@is_error=0,
					--	@service_id=7,
					--	@log_message = 'Creating Study ID ' + Convert(varchar(36),@id),
					--	@error_msg 

					select @consult_applied  = consult_applied,
					       @storage_applied  = storage_applied,
					       @priority_id      = priority_id,
						   @modality_id      = modality_id,
						   @institution_id   = institution_id,
						   @beyond_hour_stat = beyond_hour_stat,
						   @species_id       = species_id
					from scheduler_img_file_downloads_grouped
					where id=@id

					set @service_codes=''
					if(@consult_applied = 'Y') 
						begin
							set @service_codes='CONSULT'
						end
					if(@storage_applied = 'Y') 
						begin
							if(@service_codes='') set @service_codes='STORAGE'
							else if(@service_codes<>'') set @service_codes=@service_codes + ',STORAGE'
						end

					set @received_date = getdate()
					select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id = @priority_id
					set @finishing_time_hrs = isnull(@finishing_time_hrs,0)
					select @FNLRPTAUTORELHR = data_type_string from general_settings where control_code='FNLRPTAUTORELHR'
					set @in_exp_list='N'
					set @beyond_hour_stat ='N'
					set @error_msg =''
					set @return_type=0

					 if(@beyond_hour_stat ='Y' and @is_stat='Y')--Beyond operation time
						begin
							exec common_service_availability_check
								@species_id            = @species_id,
								@modality_id           = @modality_id,
								@institution_id        = @institution_id,
								@priority_id           = @priority_id,
								@beyond_operation_time = @beyond_hour_stat output,
								@in_exp_list           = @in_exp_list output,
								@error_code            = @error_msg output,
								@return_status         = @return_type output

							if(@return_type=0)
								begin
									rollback transaction
									select @return_type=0,@error_msg='Failure checking the service availability Study UID ' + @study_uid + '.'
									return 0
								end
						end

					select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id = @priority_id
				    set @finishing_time_hrs = isnull(@finishing_time_hrs,0)

					exec common_check_operation_time
						@priority_id             = @priority_id,
						@sender_time_offset_mins = @sender_time_offset_mins,
						@next_operation_time     = @next_operation_time output,
						@delv_time               = @delv_time output,
						@error_code              = @error_msg output,
						@return_status           = @return_type output

				if( @in_exp_list='N') 
					begin
						set @finishing_datetime = convert(datetime,@delv_time)
						
					end
				else
					begin
						set @finishing_datetime = dateadd(HH,@finishing_time_hrs,getdate())
					end

				if(@is_stat='Y')
					begin
						set @final_rpt_release_datetime = dateadd(mi,(select final_report_release_time_mins from sys_priority where priority_id=@priority_id),getdate())
					end
				else
					begin
						select @final_rpt_release_datetime = dateadd(HH,@FNLRPTAUTORELHR,getdate())
					end
					
					

					insert into study_hdr(id,study_uid,study_date,received_date,
										  accession_no_pacs,accession_no,
										  reason_pacs,reason_accepted,physician_note,
										  institution_id,institution_name_pacs,manufacturer_name,device_serial_no,modality_ae_title,
										  physician_id,referring_physician_pacs,
										  patient_id_pacs,patient_id,
										  patient_name_pacs,patient_name,patient_fname,patient_lname,
										  patient_country_id,patient_state_id,patient_city,
										  patient_sex_pacs,patient_sex,
										  patient_sex_neutered_pacs,patient_sex_neutered,
										  patient_dob_pacs,patient_dob_accepted,
										  patient_age_pacs,patient_age_accepted,
										  patient_weight_pacs,patient_weight,
										  owner_name_pacs,owner_first_name,owner_last_name,
										  species_pacs,species_id,breed_pacs,breed_id,modality_pacs,modality_id,category_id,
										  salesperson_id,body_part_pacs,
										  img_count_pacs,img_count,object_count,object_count_pacs,study_desc,
										  priority_id_pacs,priority_id,study_status,study_status_pacs,sync_mode,
										  consult_applied,service_codes,finishing_datetime,final_rpt_release_datetime,beyond_hour_stat,priority_charged,
										  synched_on,date_updated,status_last_updated_on,pacs_wb)
							  (select fdg.id,fdg.study_uid,fdg.study_date,fdg.date_created,
									  fdg.accession_no,fdg.accession_no,
									  fdg.reason,fdg.reason,isnull(fdg.physician_note,''),
									  fdg.institution_id,fdg.institution_name,'','','',
									  fdg.physician_id,'',
									  fdg.patient_id,fdg.patient_id,
									  rtrim(ltrim(fdg.patient_fname + ' ' + fdg.patient_lname)),rtrim(ltrim(fdg.patient_fname + ' ' + fdg.patient_lname)),fdg.patient_fname,fdg.patient_lname,
									  fdg.patient_country_id,fdg.patient_state_id,fdg.patient_city,
									  fdg.patient_sex,fdg.patient_sex,
									  fdg.spayed_neutered,fdg.spayed_neutered,
									  fdg.patient_dob,fdg.patient_dob,
									  fdg.patient_age,fdg.patient_age,
									  fdg.patient_weight,fdg.patient_weight,
									  rtrim(ltrim(fdg.owner_first_name + ' ' + fdg.owner_last_name)),fdg.owner_first_name,fdg.owner_last_name,
									  species= s.name,fdg.species_id,breed = b.name,fdg.breed_id,m.code,fdg.modality_id,fdg.category_id,
									  isnull(fdg.salesperson_id,'00000000-0000-0000-0000-000000000000'),'',	
									  fdg.file_count,fdg.file_count,@xfer_file_count,fdg.file_count,'',
									  fdg.priority_id,fdg.priority_id,2,50,'DR',
									  fdg.consult_applied,@service_codes,@finishing_datetime,@final_rpt_release_datetime,beyond_hour_stat,priority_charged,
									  getdate(),getdate(),getdate(),'Y'
								from scheduler_img_file_downloads_grouped  fdg
								inner join physicians p on p.id = fdg.physician_id
								inner join species s on s.id= fdg.species_id
								inner join breed b on b.id= fdg.breed_id
								inner join modality m on m.id=fdg.modality_id
								inner join institutions i on i.id= fdg.institution_id
								--left outer join institution_salesperson_link isl on isl.institution_id = i.id
								where fdg.id=@id)	

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to synch Study UID ' + @study_uid
							return 0
						end

					--print '222'
					
					insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
														   values(@id,@study_uid,0,50,getdate())
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update status log of Study UID ' + @study_uid
							return 0
						end

					insert into study_hdr_study_types(study_hdr_id,study_type_id,write_back_tag,srl_no,updated_by,date_updated)
											  (select study_hdr_id,study_type_id,write_back_tag,srl_no,updated_by,date_updated
											   from scheduler_img_file_downloads_grouped_study_types
											   where study_hdr_id=@id)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to update study type(s) of Study UID ' + @study_uid
							return 0
						end								                 

					insert into study_hdr_documents(study_hdr_id,document_id,document_name,document_srl_no,
													document_link,document_file_type,document_file,created_by,date_created)
											(select study_hdr_id,document_id,document_name,document_srl_no,
													document_link,document_file_type,document_file,created_by,getdate()
											  from scheduler_img_file_downloads_grouped_docs
											  where study_hdr_id=@id)
				end
			else
				begin
					update study_hdr 
					set pacs_wb ='Y' , 
					    received_via_dicom_router='N'
					where study_uid=@study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_type=0,@error_msg='Failed to synch Study UID ' + @study_uid
							return 0
						end
				end
		end

	if(@xfer_file_count=1)
		begin
			select @error_code='',@return_status=0
			set @activity_text= 'Transfer of files to PACS started'
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
		end

	if(@xfer_file_count>0)
		begin
			  update study_hdr
			  set object_count = @xfer_file_count
			  where id=@id

			  if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='2-Failed to update count of transfered file(s) to PACS of Study UID ' + @study_uid
					return 0
				end
		end

	if(select object_count_pacs - object_count from study_hdr where id = @id)<=1
		begin
			select @error_code='',@return_status=0
			set @activity_text= 'Transfer of files to PACS completed'
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
		end


	commit transaction
	set @return_type=1
	set @error_msg=''
	set nocount off
	return 1

end


GO
