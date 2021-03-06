USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_archive_ra_revert_study]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_archive_ra_revert_study]
GO
/****** Object:  StoredProcedure [dbo].[case_list_archive_ra_revert_study]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_archive_ra_revert_study : revert a 
                  unviewed study to required action
** Created By   : Pavel Guha
** Created On   : 13/05/2020
*******************************************************/
CREATE procedure [dbo].[case_list_archive_ra_revert_study]
    @id uniqueidentifier,
	@study_uid nvarchar(100),
	@updated_by uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on

	begin transaction
	
	if(select count(id) from study_hdr_archive where id=@id and study_uid=@study_uid)> 0
		begin

			insert into study_hdr(id,study_uid,study_date,received_date,accession_no_pacs,accession_no,study_type_id,
			                              institution_name_pacs,institution_id,manufacturer_name,manufacturer_model_no,device_serial_no,
										  referring_physician_pacs,physician_id,
										  patient_id_pacs,patient_id,patient_name_pacs,patient_name,patient_fname,patient_lname,
										  patient_sex_pacs,patient_sex,patient_sex_neutered_pacs,patient_sex_neutered,patient_weight_pacs,patient_weight,
										  patient_dob_pacs,patient_dob_accepted,patient_age_pacs,patient_age_accepted,sex_neutered_pacs,sex_neutered_accepted,
										  owner_name_pacs,owner_first_name,owner_last_name,species_pacs,species_id,breed_pacs,breed_id,modality_pacs,modality_id,
										  body_part_pacs,body_part_id,study_desc,reason_pacs,reason_accepted,img_count_pacs,img_count,img_count_accepted,
										  study_status_pacs,study_status,pacs_wb,prelim_rpt_updated,final_rpt_updated,radiologist_pacs,modality_ae_title,prelim_sms_updated,final_sms_updated,
										  patient_weight_kgs,wt_uom,priority_id_pacs,priority_id,salesperson_id,deleted,merge_status,merge_status_desc,
										  object_count,radiologist_id,invoiced,discount_per,discount_amount,discount_type,is_free,promo_reason_id,promo_applied_by,promo_applied_on,service_codes,
										  prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,rpt_approve_date,rpt_record_date,consult_applied,
										  physician_note,category_id,manually_assigned,received_via_dicom_router,
										  synched_on,finishing_datetime,updated_by,date_updated,status_last_updated_on)
								(select id,study_uid,study_date,received_date,accession_no_pacs,accession_no,study_type_id,
			                              institution_name_pacs,institution_id,manufacturer_name,manufacturer_model_no,device_serial_no,
										  referring_physician_pacs,physician_id,
										  patient_id_pacs,patient_id,patient_name_pacs,patient_name,patient_fname,patient_lname,
										  patient_sex_pacs,patient_sex,patient_sex_neutered_pacs,patient_sex_neutered,patient_weight_pacs,patient_weight,
										  patient_dob_pacs,patient_dob_accepted,patient_age_pacs,patient_age_accepted,sex_neutered_pacs,sex_neutered_accepted,
										  owner_name_pacs,owner_first_name,owner_last_name,species_pacs,species_id,breed_pacs,breed_id,modality_pacs,modality_id,
										  body_part_pacs,body_part_id,study_desc,reason_pacs,reason_accepted,img_count_pacs,img_count,img_count_accepted,
										  study_status_pacs,1,pacs_wb,prelim_rpt_updated,final_rpt_updated,radiologist_pacs,modality_ae_title,prelim_sms_updated,final_sms_updated,
										  patient_weight_kgs,wt_uom,priority_id_pacs,priority_id,salesperson_id,deleted,merge_status,merge_status_desc,
										  object_count,radiologist_id,invoiced,discount_per,discount_amount,discount_type,is_free,promo_reason_id,promo_applied_by,promo_applied_on,service_codes,
										  prelim_radiologist_pacs,prelim_radiologist_id,final_radiologist_pacs,final_radiologist_id,rpt_approve_date,rpt_record_date,consult_applied,
										  physician_note,category_id,manually_assigned,received_via_dicom_router,
										  synched_on,finishing_datetime,@updated_by,getdate(),status_last_updated_on
								 from study_hdr_archive
								 where id = @id
								 and study_uid = @study_uid)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status=0,@error_code='308'
					return 0
				end

			if(select count(study_hdr_id) from  study_hdr_study_types_archive where study_hdr_id = @id)>0
				begin
					insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
														(select study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated
														from study_hdr_study_types_archive
														where study_hdr_id= @id)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='308'
							return 0
						end

					delete from study_hdr_study_types_archive where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='308'
							return 0
						end
				end

			if(select count(study_hdr_id) from  study_hdr_documents_archive where study_hdr_id = @id)>0
				begin
					insert into study_hdr_documents(study_hdr_id,document_id,document_name,document_srl_no,document_link,
					                                        document_file_type,document_file,created_by,date_created,updated_by,date_updated)
													(select study_hdr_id,document_id,document_name,document_srl_no,document_link,
													        document_file_type,document_file,created_by,date_created,updated_by,date_updated
													from study_hdr_documents_archive
													where study_hdr_id= @id)
					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='308'
							return 0
						end

					delete from study_hdr_documents_archive where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='308'
							return 0
						end
				end

			delete from study_hdr_archive where id = @id and study_uid = @study_uid

			if(select count(study_hdr_id) from  study_hdr_dcm_files_archive where study_hdr_id = @id)>0
				begin
					insert into study_hdr_dcm_files(dcm_file_id,study_hdr_id,study_uid,dcm_file_srl_no,dcm_file_name,
													dcm_file,created_by,date_created,updated_by,date_updated)
											(select dcm_file_id,study_hdr_id,study_uid,dcm_file_srl_no,dcm_file_name,
													dcm_file,created_by,date_created,updated_by,date_updated
											from study_hdr_dcm_files_archive
											where study_hdr_id = @id)

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='308'
							return 0
						end

					delete from study_hdr_dcm_files where study_hdr_id = @id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @return_status=0,@error_code='308'
							return 0
						end
				end

			insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated,updated_by)
									      values(@id,@study_uid,11,0,getdate(),@updated_by)

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
			set @error_code='309'
			return 1
		end

	commit transaction
	set @return_status=1
	set @error_code='310'
	set nocount off
	return 1

end


GO
