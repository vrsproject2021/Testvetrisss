USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_process]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_day_end_process]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_process]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_day_end_process : Process Day End Data
** Created By   : Pavel Guha
** Created On   : 07/06/2019
*******************************************************/
--exec scheduler_day_end_process
/*
	declare @file_id int

    select @file_id = file_id
	from sys.database_files
	where name ='vrsdb_log'
	
	DBCC SHRINKFILE (@file_id, TRUNCATEONLY); 
*/
CREATE procedure [dbo].[scheduler_day_end_process]
as
begin

	declare @file_id int,
	        @rc int,
			@ctr int,
			@log_message varchar(8000),
			@error_msg nvarchar(100),
			@return_type int,
			@strSQL nvarchar(100)
	
	delete from vrslogdb..sys_scheduler_log where log_date < dateadd(d,-2,getdate())
	delete from vrslogdb..sys_dicom_router_log where log_date < dateadd(d,-1,getdate())

	--Send studies to archive
	create table #tmp(id uniqueidentifier,
	                  study_uid nvarchar(100))

	--insert into #tmp(id,study_uid)
	--(select ssl.study_id,ssl.study_uid
	-- from sys_case_study_status_log ssl
	-- inner join study_hdr sh on sh.id = ssl.study_id
	-- where ssl.status_id_to=100
	-- and ssl.date_updated = (select max(sl.date_updated)
	--                     from sys_case_study_status_log sl
	--					 where sl.study_id = ssl.study_id
	--					 and datediff(d,sl.date_updated,getdate()) >= 2))

	insert into #tmp(id,study_uid)
	(select id,study_uid
	 from study_hdr 
	 where study_status_pacs=100
	 and datediff(d,status_last_updated_on,getdate())>=2)

	insert into study_hdr_archive(id,study_uid,study_date,received_date,accession_no_pacs,accession_no,study_type_id,
			                      institution_name_pacs,institution_id,manufacturer_name,manufacturer_model_no,device_serial_no,
								  referring_physician_pacs,physician_id,
								  patient_id_pacs,patient_id,patient_name_pacs,patient_name,patient_fname,patient_lname,
								  patient_sex_pacs,patient_sex,patient_sex_neutered_pacs,patient_sex_neutered,patient_weight_pacs,patient_weight,
								  patient_dob_pacs,patient_dob_accepted,patient_age_pacs,patient_age_accepted,sex_neutered_pacs,sex_neutered_accepted,
								  patient_country_id,patient_state_id,patient_city,
								  owner_name_pacs,owner_first_name,owner_last_name,species_pacs,species_id,breed_pacs,breed_id,modality_pacs,modality_id,
								  body_part_pacs,body_part_id,study_desc,reason_pacs,reason_accepted,img_count_pacs,img_count,img_count_accepted,
								  study_status_pacs,study_status,pacs_wb,prelim_rpt_updated,final_rpt_updated,radiologist_pacs,modality_ae_title,prelim_sms_updated,final_sms_updated,
								  patient_weight_kgs,wt_uom,priority_id_pacs,priority_id,salesperson_id,deleted,merge_status,merge_status_desc,physician_note,
								  object_count,object_count_pacs,radiologist_id,invoiced,discount_per,discount_amount,discount_type,is_free,promo_reason_id,promo_applied_by,promo_applied_on,service_codes,
								  dict_radiologist_pacs,dict_radiologist_id,prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,
								  rpt_approve_date,rpt_record_date,consult_applied,priority_charged,rad_assigned_on,
								  category_id,dict_tanscriptionist_id,manually_assigned,received_via_dicom_router,sync_mode,assign_accepted,mark_to_teach,archive_file_count,log_available,
								  synched_on,finishing_datetime,transcription_finishing_datetime,beyond_hour_stat,final_rpt_released,final_rpt_release_datetime,final_rpt_released_on,
								  updated_by,date_updated,status_last_updated_on,archived_by,date_archived)
					     (select id,study_uid,study_date,received_date,accession_no_pacs,accession_no,study_type_id,
			                    institution_name_pacs,institution_id,manufacturer_name,manufacturer_model_no,device_serial_no,
								referring_physician_pacs,physician_id,
								patient_id_pacs,patient_id,patient_name_pacs,patient_name,patient_fname,patient_lname,
								patient_sex_pacs,patient_sex,patient_sex_neutered_pacs,patient_sex_neutered,patient_weight_pacs,patient_weight,
								patient_dob_pacs,patient_dob_accepted,patient_age_pacs,patient_age_accepted,sex_neutered_pacs,sex_neutered_accepted,
								patient_country_id,patient_state_id,patient_city,
								owner_name_pacs,owner_first_name,owner_last_name,species_pacs,species_id,breed_pacs,breed_id,modality_pacs,modality_id,
								body_part_pacs,body_part_id,study_desc,reason_pacs,reason_accepted,img_count_pacs,img_count,img_count_accepted,
								study_status_pacs,5,pacs_wb,prelim_rpt_updated,final_rpt_updated,radiologist_pacs,modality_ae_title,prelim_sms_updated,final_sms_updated,
								patient_weight_kgs,wt_uom,priority_id_pacs,priority_id,salesperson_id,deleted,merge_status,merge_status_desc,physician_note,
								object_count,object_count_pacs,radiologist_id,invoiced,discount_per,discount_amount,discount_type,is_free,promo_reason_id,promo_applied_by,promo_applied_on,service_codes,
								dict_radiologist_pacs,dict_radiologist_id,prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,rpt_approve_date,
								rpt_record_date,consult_applied,priority_charged,rad_assigned_on,
								category_id,dict_tanscriptionist_id,manually_assigned,received_via_dicom_router,sync_mode,assign_accepted,mark_to_teach,archive_file_count,log_available,
								synched_on,finishing_datetime,transcription_finishing_datetime,beyond_hour_stat,final_rpt_released,final_rpt_release_datetime,final_rpt_released_on,
								updated_by,date_updated,status_last_updated_on,'00000000-0000-0000-0000-000000000000',getdate()
						from study_hdr
						where id in (select id from #tmp))

	set @rc= @@rowcount

	insert into study_hdr_study_types_archive(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
					                (select study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated
									from study_hdr_study_types
									where study_hdr_id in (select id from #tmp))

	delete from study_hdr_study_types where study_hdr_id in (select id from #tmp)

	insert into study_hdr_documents_archive(study_hdr_id,document_id,document_name,document_srl_no,document_link,
					                        document_file_type,document_file,created_by,date_created,updated_by,date_updated)
									(select study_hdr_id,document_id,document_name,document_srl_no,document_link,
											document_file_type,document_file,created_by,date_created,updated_by,date_updated
									from study_hdr_documents
									where study_hdr_id in (select id from #tmp))

	delete from study_hdr_documents where study_hdr_id in (select id from #tmp)

	--insert into study_hdr_dcm_files_archive(study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,
	--				                        dcm_file,created_by,date_created,updated_by,date_updated)
	--								(select study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,
	--				                        dcm_file,created_by,date_created,updated_by,date_updated
	--								from study_hdr_dcm_files
	--								where study_hdr_id in (select id from #tmp))

	delete from study_hdr_dcm_files where study_hdr_id in (select id from #tmp)

	insert into study_hdr_merged_studies_archive(study_hdr_id,study_id,study_uid,image_count,merge_compare_none,updated_by,date_updated)
									       (select study_hdr_id,study_id,study_uid,image_count,merge_compare_none,updated_by,date_updated
									        from study_hdr_merged_studies
									        where study_hdr_id in (select id from #tmp))

	delete from study_hdr_merged_studies where study_hdr_id in (select id from #tmp)

	insert into study_hdr_dictated_reports_archive(report_id,study_hdr_id,study_uid,report_text,report_text_html,
					                               rating,pacs_wb,created_by,date_created,updated_by,date_updated,
												   trans_report_text,trans_report_text_html,transcribed_by,date_transcribed,
												   translate_report_text,translate_report_text_html,disclaimer_reason_id,disclaimer_text,rating_reason_id)
											(select report_id,study_hdr_id,study_uid,report_text,report_text_html,
					                                rating,pacs_wb,created_by,date_created,updated_by,date_updated,
													trans_report_text,trans_report_text_html,transcribed_by,date_transcribed,
												   translate_report_text,translate_report_text_html,disclaimer_reason_id,disclaimer_text,rating_reason_id
											from study_hdr_dictated_reports
											where study_hdr_id in (select id from #tmp))

	delete from study_hdr_dictated_reports where study_hdr_id in (select id from #tmp)
	

	insert into study_hdr_prelim_reports_archive(report_id,study_hdr_id,study_uid,report_text,report_file,email_updated,sms_updated,
					                            report_text_html,rating,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created,updated_by,date_updated)
											(select report_id,study_hdr_id,study_uid,report_text,report_file,email_updated,sms_updated,
													report_text_html,rating,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created,updated_by,date_updated
											from study_hdr_prelim_reports
											where study_hdr_id in (select id from #tmp))

	delete from study_hdr_prelim_reports where study_hdr_id in (select id from #tmp)

	insert into study_report_addendums_archive(report_id,addendum_srl,study_hdr_id,study_uid,addendum_text,addendum_text_html,pacs_wb,
	                                           created_by,date_created,updated_by,date_updated,archived_by,date_archived)
										(select report_id,addendum_srl,study_hdr_id,study_uid,addendum_text,addendum_text_html,pacs_wb,
										        created_by,date_created,updated_by,date_updated,'00000000-0000-0000-0000-000000000000',getdate()
										from study_report_addendums
										where study_hdr_id in (select id from #tmp))

	delete from study_report_addendums where study_hdr_id in (select id from #tmp)
	
	insert into study_hdr_final_reports_archive(report_id,study_hdr_id,study_uid,report_text,report_file,email_updated,sms_updated,
					                            report_text_html,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created,updated_by,date_updated)
											(select report_id,study_hdr_id,study_uid,report_text,report_file,email_updated,sms_updated,
													report_text_html,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created,updated_by,date_updated
											from study_hdr_final_reports
											where study_hdr_id in (select id from #tmp)) 

	delete from study_hdr_final_reports where study_hdr_id in (select id from #tmp)
	delete from study_hdr where id in (select id from #tmp)
	--delete from study_synch_dump where study_uid in (select study_uid from #tmp)

	
	insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
								 (select id,study_uid,100,11,getdate() from #tmp)

	set @log_message = convert(varchar,@rc) + ' study(ies) archived'
	set @error_msg=''
	set @return_type=0

	exec scheduler_log_save
		 @is_error    = 0,
		 @service_id  = 5,
		 @log_message = @log_message,
		 @error_msg   = @error_msg,
		 @return_type = @return_type	
	--Send studies to archive

	--START -- Send UNVIEWED Studies for more than 2 days to archive (having storage_applicable='Y')
	truncate table #tmp

	insert into #tmp(id,study_uid)
	(select id,study_uid
	 from study_hdr
	 where study_status_pacs=0
	 and study_status=1
	 and institution_id in (select id from institutions where isnull(storage_applicable,'N') = 'Y' union select id='00000000-0000-0000-0000-000000000000')
	 and datediff(d,synched_on,getdate()) >= 2)

	 insert into study_hdr_archive(id,study_uid,study_date,received_date,accession_no_pacs,accession_no,study_type_id,
			                              institution_name_pacs,institution_id,manufacturer_name,manufacturer_model_no,device_serial_no,
										  referring_physician_pacs,physician_id,
										  patient_id_pacs,patient_id,patient_name_pacs,patient_name,patient_fname,patient_lname,
										  patient_sex_pacs,patient_sex,patient_sex_neutered_pacs,patient_sex_neutered,patient_weight_pacs,patient_weight,
										  patient_dob_pacs,patient_dob_accepted,patient_age_pacs,patient_age_accepted,sex_neutered_pacs,sex_neutered_accepted,
										  patient_country_id,patient_state_id,patient_city,
										  owner_name_pacs,owner_first_name,owner_last_name,species_pacs,species_id,breed_pacs,breed_id,modality_pacs,modality_id,
										  body_part_pacs,body_part_id,study_desc,reason_pacs,reason_accepted,img_count_pacs,img_count,img_count_accepted,
										  study_status_pacs,study_status,pacs_wb,prelim_rpt_updated,final_rpt_updated,radiologist_pacs,modality_ae_title,prelim_sms_updated,final_sms_updated,
										  patient_weight_kgs,wt_uom,priority_id_pacs,priority_id,salesperson_id,deleted,merge_status,merge_status_desc,physician_note,
										  object_count,object_count_pacs,radiologist_id,invoiced,discount_per,discount_amount,discount_type,is_free,promo_reason_id,promo_applied_by,promo_applied_on,
										  service_codes,beyond_hour_stat,priority_charged,
										  prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,rpt_approve_date,rpt_record_date,consult_applied,
										  category_id,manually_assigned,received_via_dicom_router,sync_mode,dict_tanscriptionist_id,
										  synched_on,finishing_datetime,transcription_finishing_datetime,archive_file_count,
										  updated_by,date_updated,status_last_updated_on,archived_by,date_archived)
								(select id,study_uid,study_date,received_date,accession_no_pacs,accession_no,study_type_id,
			                              institution_name_pacs,institution_id,manufacturer_name,manufacturer_model_no,device_serial_no,
										  referring_physician_pacs,physician_id,
										  patient_id_pacs,patient_id,patient_name_pacs,patient_name,patient_fname,patient_lname,
										  patient_sex_pacs,patient_sex,patient_sex_neutered_pacs,patient_sex_neutered,patient_weight_pacs,patient_weight,
										  patient_dob_pacs,patient_dob_accepted,patient_age_pacs,patient_age_accepted,sex_neutered_pacs,sex_neutered_accepted,
										  patient_country_id,patient_state_id,patient_city,
										  owner_name_pacs,owner_first_name,owner_last_name,species_pacs,species_id,breed_pacs,breed_id,modality_pacs,modality_id,
										  body_part_pacs,body_part_id,study_desc,reason_pacs,reason_accepted,img_count_pacs,img_count,img_count_accepted,
										  study_status_pacs,5,'Y',prelim_rpt_updated,final_rpt_updated,radiologist_pacs,modality_ae_title,prelim_sms_updated,final_sms_updated,
										  patient_weight_kgs,wt_uom,priority_id_pacs,priority_id,salesperson_id,deleted,merge_status,merge_status_desc,physician_note,
										  object_count,object_count_pacs,radiologist_id,invoiced,discount_per,discount_amount,discount_type,is_free,promo_reason_id,promo_applied_by,promo_applied_on,
										  case
											when charindex('STORAGE',isnull(service_codes,''))>0 then service_codes
											when isnull(service_codes,'') ='' then 'STORAGE'
											when isnull(service_codes,'') <> '' then rtrim(ltrim(service_codes)) + ',STORAGE'
										  end service_codes,beyond_hour_stat,priority_charged,
										  prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,rpt_approve_date,rpt_record_date,consult_applied,
										  category_id,manually_assigned,received_via_dicom_router,sync_mode,dict_tanscriptionist_id,
										  synched_on,finishing_datetime,transcription_finishing_datetime,archive_file_count,
										  updated_by,date_updated,status_last_updated_on,'00000000-0000-0000-0000-000000000000',getdate()
								 from study_hdr
								 where id in (select id from #tmp))

	set @rc= @@rowcount

	insert into study_hdr_study_types_archive(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
					                (select study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated
									from study_hdr_study_types
									where study_hdr_id in (select id from #tmp))

	delete from study_hdr_study_types where study_hdr_id in (select id from #tmp)

	insert into study_hdr_documents_archive(study_hdr_id,document_id,document_name,document_srl_no,document_link,
					                        document_file_type,document_file,created_by,date_created,updated_by,date_updated)
									(select study_hdr_id,document_id,document_name,document_srl_no,document_link,
											document_file_type,document_file,created_by,date_created,updated_by,date_updated
									from study_hdr_documents
									where study_hdr_id in (select id from #tmp))

	delete from study_hdr_documents where study_hdr_id in (select id from #tmp)

	insert into study_hdr_dcm_files_archive(dcm_file_id,study_hdr_id,study_uid,dcm_file_srl_no,dcm_file_name,
					                        dcm_file,created_by,date_created,updated_by,date_updated)
									(select dcm_file_id,study_hdr_id,study_uid,dcm_file_srl_no,dcm_file_name,
											dcm_file,created_by,date_created,updated_by,date_updated
									from study_hdr_dcm_files
									where study_hdr_id in (select id from #tmp))

	delete from study_hdr_dcm_files where study_hdr_id in (select id from #tmp)

	delete from study_hdr where id in (select id from #tmp)

	insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated)
								 (select id,study_uid,0,11,getdate() from #tmp)

	set @log_message = convert(varchar,@rc) + ' UNVIEWED study(ies) archived'
	set @error_msg=''
	set @return_type=0

	exec scheduler_log_save
		 @is_error    = 0,
		 @service_id  = 5,
		 @log_message = @log_message,
		 @error_msg   = @error_msg,
		 @return_type = @return_type	

	--select * from study_hdr_archive where id in (select * from #tmp)

	--END -- Send UNVIEWED Studies for more than 3 days to archive (having storage_applicable='Y')

	

	--START -- Mark DCM UNVIEWED Studies for deletion (synched for more than 2 days, having storage_applicable='N')
	truncate table #tmp

	insert into #tmp(id,study_uid)
	(select id,study_uid
	 from study_hdr
	 where study_status_pacs=0
	 and institution_id in (select id from institutions where isnull(storage_applicable,'N') = 'N')
	 and datediff(d,synched_on,getdate()) >= 3)

	 insert into scheduler_study_to_delete(study_id,study_uid,study_status_id,received_via_dicom_router,date_created)
	 (select id,study_uid,study_status_pacs,received_via_dicom_router,getdate()
	  from study_hdr
	  where id in (select id from #tmp))

	 set @rc= @@rowcount
	 
	 set @log_message = convert(varchar,@rc) + ' UNVIEWED study(ies) marked for deletion'
	 set @error_msg=''
	 set @return_type=0

	 exec scheduler_log_save
			@is_error    = 0,
			@service_id  = 5,
			@log_message = @log_message,
			@error_msg   = @error_msg,
			@return_type = @return_type

	--END -- Mark DCM UNVIEWED Studies for deletion (synched for more than 2 days, having storage_applicable='N')
	
	truncate table #tmp

	--START -- Mark Image file records for deletion (synched for more than 3 days, having storage_applicable='Y')

	update scheduler_img_file_downloads_ungrouped
	set is_stored='Y'
	where datediff(d,date_downloaded,getdate()) >= 3
	and grouped='N'
	and is_stored='N'
	and institution_id in (select id from institutions where isnull(storage_applicable,'N') = 'N')

	--END -- Mark Image file records for deletion (synched for more than 2 days, having storage_applicable='Y')

	--START -- Mark Image file records for deletion (synched for more than 2 days, having storage_applicable='N')

	insert into scheduler_image_files_to_delete(id,file_name,import_session_id,institution_id,date_downloaded,date_created)
	(select id,file_name,import_session_id,institution_id,date_downloaded,getdate()
	 from scheduler_img_file_downloads_ungrouped
	 where datediff(d,date_downloaded,getdate()) >= 2
	 and grouped='N'
	 and is_stored='N'
	 and institution_id in (select id from institutions where isnull(storage_applicable,'N') = 'N'))

	 set @rc= @@rowcount
	 
	 set @log_message = convert(varchar,@rc) + ' Image file record(s) marked for deletion'
	 set @error_msg=''
	 set @return_type=0

	 exec scheduler_log_save
			@is_error    = 0,
			@service_id  = 5,
			@log_message = @log_message,
			@error_msg   = @error_msg,
			@return_type = @return_type

	--END -- Mark Image file records for deletion (synched for more than 2 days, having storage_applicable='N')

	drop table #tmp

	--deactivate promotions
	update ar_promotions set is_active='N' where valid_till < convert(datetime,convert(varchar(11),getdate(),106)) and is_active='Y'

	set @rc= @@rowcount

	if(@rc>0)
		begin
			set @log_message =  convert(varchar,@rc) + ' promotion(s) deactivated'
			set @error_msg=''
			set @return_type=0

			exec scheduler_log_save
				 @is_error    = 0,
				 @service_id  = 5,
				 @log_message = @log_message,
				 @error_msg   = @error_msg,
				 @return_type = @return_type
		end

	--deactivate promotions

	--remove user locks acquired for more than 24 hrs
	create table #tmpUsers
	(
	  rec_id int identity(1,1),
	  user_id uniqueidentifier,
	  session_id  uniqueidentifier,
	  last_login datetime
	)

	declare @user_id uniqueidentifier,
	        @session_id uniqueidentifier,
			@return_status int

	insert into #tmpUsers(user_id,session_id,last_login)
	(select user_id,session_id,last_login
	 from sys_user_lock
	 where datediff(hour,last_login,getdate())>24 )

	 select @rc= @@rowcount,
	        @ctr = 1 
     
	 while(@ctr<=@rc)
		begin
			select @user_id     = user_id,
			       @session_id = session_id
			from #tmpUsers
			where rec_id = @ctr

			set @return_status=0
			exec common_unlock_user
			     @user_id       = @user_id,
				 @session_id    = @session_id,
				 @return_status = @return_status output

			set @ctr = @ctr + 1
		end

	drop table #tmpUsers
	--remove user locks acquired for more than 24 hrs

	
	--purge email log
	delete from vrslogdb..email_log
	where datediff(month,email_log_datetime,getdate())>3
	--purge email log

	--purge sms log
	delete from vrslogdb..sms_log
	where datediff(month,sms_log_datetime,getdate())>3
	--purge sms log

	--purge fax log
	delete from vrslogdb..fax_log
	where datediff(month,log_datetime,getdate())>3
	--purge fax log

	--delete web service sessions
	delete from sys_ws8_session where date_created <> (select max(date_created) from sys_ws8_session)
	--delete web service sessions

	--purge user activity log
	delete from vrslogdb..sys_user_activity_log
	where datediff(month,activity_datetime,getdate())>7
	--purge user activity log

	--purge study activity log
	create table #tmpActivityID(study_hdr_id uniqueidentifier)
	create table #tmpArch(rec_id int identity(1,1),db_name nvarchar(50))

	insert into #tmpActivityID(study_hdr_id)
	(select distinct study_hdr_id from vrslogdb..sys_study_user_activity_trail
	 where datediff(day,activity_datetime,getdate())>90)

	delete from vrslogdb..sys_study_user_activity_trail
	where study_hdr_id in (select study_hdr_id from #tmpActivityID)

	update study_hdr set log_available='N' where id in (select study_hdr_id from #tmpActivityID)
	update study_hdr_archive set log_available='N' where id in (select study_hdr_id from #tmpActivityID)

	declare @db_name nvarchar(50)

	insert into #tmpArch(db_name)
	(select db_name
	 from sys_archive_db)

	 select @rc= @@rowcount,
	        @ctr = 1 
     
	 while(@ctr<=@rc)
		begin
			select @db_name     = db_name
			from #tmpArch
			where rec_id = @ctr

			set @strSQL='update ' + @db_name + '..study_hdr_archive set log_available=''N'' where id in (select study_hdr_id from #tmpActivityID)'

			set @ctr = @ctr + 1
		end

	drop table #tmpActivityID
	drop table #tmpArch
	--purge study activity log

	--purge the missing session files checked
	delete from scheduler_checked_missing_session_files where date_checked<getdate()
	--purge the missing session files checked
	
	select @file_id = file_id
	from sys.database_files
	where name ='vrsdb_log'
	
	DBCC SHRINKFILE (@file_id, TRUNCATEONLY); 
	
		

end

GO
