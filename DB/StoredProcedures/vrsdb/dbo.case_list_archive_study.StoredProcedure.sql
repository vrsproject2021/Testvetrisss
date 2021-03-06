USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_archive_study]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_archive_study]
GO
/****** Object:  StoredProcedure [dbo].[case_list_archive_study]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_archive_study : save
                  case finalinary report
** Created By   : Pavel Guha
** Created On   : 23/04/2019
*******************************************************/
CREATE procedure [dbo].[case_list_archive_study]
    @id uniqueidentifier,
	@study_uid nvarchar(100),
	@updated_by uniqueidentifier,
	@menu_id int,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	begin transaction
	
	if(select count(id) from study_hdr where id=@id and study_status=5)= 0
		begin
			update study_hdr
			set study_status  = 5,
				updated_by   = @updated_by,
				date_updated = getdate()
			where id = @id 
			and study_uid =@study_uid

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='070'
					return 0
				end

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
										  patient_weight_kgs,wt_uom,priority_id_pacs,priority_id,salesperson_id,deleted,merge_status,merge_status_desc,
										  object_count,object_count_pacs,radiologist_id,invoiced,discount_per,discount_amount,discount_type,is_free,promo_reason_id,promo_applied_by,promo_applied_on,service_codes,
										  dict_radiologist_pacs,dict_radiologist_id,prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,
										  rpt_approve_date,rpt_record_date,consult_applied,priority_charged,rad_assigned_on,
										  physician_note,category_id,dict_tanscriptionist_id,manually_assigned,sync_mode,assign_accepted,mark_to_teach,archive_file_count,log_available,
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
										  patient_weight_kgs,wt_uom,priority_id_pacs,priority_id,salesperson_id,deleted,merge_status,merge_status_desc,
										  object_count,object_count_pacs,radiologist_id,invoiced,discount_per,discount_amount,discount_type,is_free,promo_reason_id,promo_applied_by,promo_applied_on,service_codes,
										  dict_radiologist_pacs,dict_radiologist_id,prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,
										  rpt_approve_date,rpt_record_date,consult_applied,priority_charged,rad_assigned_on,
										  physician_note,category_id,dict_tanscriptionist_id,manually_assigned,sync_mode,assign_accepted,mark_to_teach,archive_file_count,log_available,
										  synched_on,finishing_datetime,transcription_finishing_datetime,beyond_hour_stat,final_rpt_released,final_rpt_release_datetime,final_rpt_released_on,
										  updated_by,date_updated,status_last_updated_on,@updated_by,getdate()
								 from study_hdr
								 where id = @id
								 and study_uid = @study_uid)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='070'
					return 0
				end

			if(select count(study_hdr_id) from  study_hdr_study_types where study_hdr_id = @id)>0
				begin
					insert into study_hdr_study_types_archive(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
					                                  (select study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated
												       from study_hdr_study_types
													   where study_hdr_id= @id)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end

					delete from study_hdr_study_types where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_hdr_documents where study_hdr_id = @id)>0
				begin
					insert into study_hdr_documents_archive(study_hdr_id,document_id,document_name,document_srl_no,document_link,
					                                        document_file_type,document_file,created_by,date_created,updated_by,date_updated)
													(select study_hdr_id,document_id,document_name,document_srl_no,document_link,
													        document_file_type,document_file,created_by,date_created,updated_by,date_updated
													from study_hdr_documents
													where study_hdr_id= @id)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end

					delete from study_hdr_documents where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_hdr_dcm_files where study_hdr_id = @id)>0
				begin
					--insert into study_hdr_dcm_files_archive(study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,
					--                                        dcm_file,created_by,date_created,updated_by,date_updated)
					--								(select study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,
					--                                        dcm_file,created_by,date_created,updated_by,date_updated
					--								from study_hdr_dcm_files
					--								where study_hdr_id= @id)
					--if(@@rowcount=0)
					--	begin
					--		rollback transaction
					--		select @return_status=0,@error_code='070'
					--		return 0
					--	end

					delete from study_hdr_dcm_files where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_hdr_merged_studies where study_hdr_id = @id)>0
				begin
					insert into study_hdr_merged_studies_archive(study_hdr_id,study_id,study_uid,image_count,merge_compare_none,updated_by,date_updated)
													     (select study_hdr_id,study_id,study_uid,image_count,merge_compare_none,updated_by,date_updated
													      from study_hdr_merged_studies
													      where study_hdr_id= @id)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end

					delete from study_hdr_merged_studies where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_hdr_dictated_reports where study_hdr_id = @id and study_uid=@study_uid)>0
				begin
					insert into study_hdr_dictated_reports_archive(report_id,study_hdr_id,study_uid,report_text,report_text_html,
					                                               rating,pacs_wb,created_by,date_created,updated_by,date_updated,
																   trans_report_text,trans_report_text_html,transcribed_by,date_transcribed,
												                   translate_report_text,translate_report_text_html,disclaimer_reason_id,disclaimer_text,rating_reason_id)
													     (select report_id,study_hdr_id,study_uid,report_text,report_text_html,
														         rating,pacs_wb,created_by,date_created,updated_by,date_updated,
																 trans_report_text,trans_report_text_html,transcribed_by,date_transcribed,
												                 translate_report_text,translate_report_text_html,disclaimer_reason_id,disclaimer_text,rating_reason_id
													      from study_hdr_dictated_reports
													      where study_hdr_id = @id
														  and study_uid      = @study_uid)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end

					delete from study_hdr_dictated_reports where study_hdr_id = @id and study_uid=@study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_hdr_prelim_reports where study_hdr_id = @id and study_uid=@study_uid)>0
				begin
					insert into study_hdr_prelim_reports_archive(report_id,study_hdr_id,study_uid,report_text,report_file,email_updated,sms_updated,
					                                             report_text_html,rating,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created,updated_by,date_updated)
													     (select report_id,study_hdr_id,study_uid,report_text,report_file,email_updated,sms_updated,
														         report_text_html,rating,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created,updated_by,date_updated
													      from study_hdr_prelim_reports
													      where study_hdr_id = @id
														  and study_uid      = @study_uid)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end

					delete from study_hdr_prelim_reports where study_hdr_id = @id and study_uid=@study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_report_addendums where study_hdr_id = @id and study_uid=@study_uid)>0
				begin
					insert into study_report_addendums_archive(report_id,addendum_srl,study_hdr_id,study_uid,addendum_text,addendum_text_html,pacs_wb,
					                                           created_by,date_created,updated_by,date_updated,archived_by,date_archived)
											           (select report_id,addendum_srl,study_hdr_id,study_uid,addendum_text,addendum_text_html,pacs_wb,
													           created_by,date_created,updated_by,date_updated,@updated_by,getdate()
											            from study_report_addendums
											            where study_hdr_id = @id
											            and study_uid = @study_uid) 
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end

					delete from study_report_addendums where study_hdr_id = @id and study_uid=@study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_hdr_final_reports where study_hdr_id = @id and study_uid=@study_uid)>0
				begin
					insert into study_hdr_final_reports_archive(report_id,study_hdr_id,study_uid,report_text,report_file,email_updated,sms_updated,
					                                            report_text_html,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created,updated_by,date_updated)
													     (select report_id,study_hdr_id,study_uid,report_text,report_file,email_updated,sms_updated,
														         report_text_html,pacs_wb,disclaimer_reason_id,disclaimer_text,rating_reason_id,created_by,date_created,updated_by,date_updated
													      from study_hdr_final_reports
													      where study_hdr_id = @id
														  and study_uid      = @study_uid)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end

					delete from study_hdr_final_reports where study_hdr_id = @id and study_uid=@study_uid

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='070'
							return 0
						end
				end

			delete from study_hdr where id = @id and study_uid = @study_uid

			

			delete from study_hdr where id = @id and study_uid = @study_uid

			insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated,updated_by)
									      values(@id,@study_uid,100,11,getdate(),@updated_by)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='142',@return_status=0
					return 0
				end
		end
	else
		begin
			rollback transaction
			set @return_status=1
			set @error_code='072'
			return 1
		end

	commit transaction
	set @return_status=1
	set @error_code='071'
	set nocount off
	return 1

end


GO
