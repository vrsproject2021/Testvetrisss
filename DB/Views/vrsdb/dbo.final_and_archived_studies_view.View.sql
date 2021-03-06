USE [vrsdb]
GO
/****** Object:  View [dbo].[final_and_archived_studies_view]    Script Date: 20-08-2021 20:59:58 ******/
DROP VIEW [dbo].[final_and_archived_studies_view]
GO
/****** Object:  View [dbo].[final_and_archived_studies_view]    Script Date: 20-08-2021 20:59:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[final_and_archived_studies_view] AS SELECT                   a.id,
                         a.study_uid, a.received_date, a.patient_name, b.name AS modality, n.name category, d.name AS institution, c.name AS physician, h.name AS radiologist, m.status_desc AS study_status, usr.name approved_by, a.study_date, a.accession_no_pacs, a.accession_no, a.institution_name_pacs, a.manufacturer_name, a.manufacturer_model_no, a.device_serial_no, 
                         a.referring_physician_pacs, a.patient_id_pacs, a.patient_name_pacs, a.patient_sex_pacs, a.patient_sex, a.patient_sex_neutered_pacs, a.patient_sex_neutered, a.patient_weight_pacs, 
                         a.patient_weight, a.patient_dob_pacs, a.patient_dob_accepted, a.patient_age_pacs, a.patient_age_accepted, a.sex_neutered_pacs, a.sex_neutered_accepted, a.owner_name_pacs, a.owner_first_name, a.owner_last_name, 
                         a.species_pacs, e.name AS species, a.breed_pacs, l.name AS breed, a.modality_pacs, a.body_part_pacs, f.name AS body_part, a.study_desc, a.reason_pacs, a.reason_accepted, a.img_count_pacs, 
                         a.img_count, a.img_count_accepted, a.study_status_pacs, a.synched_on, a.updated_by, a.date_updated, a.pacs_wb, a.prelim_rpt_updated, a.final_rpt_updated, a.radiologist_pacs, 
                         a.modality_ae_title, a.prelim_sms_updated, a.final_sms_updated, a.patient_weight_kgs, a.wt_uom, a.priority_id_pacs, a.priority_id, a.salesperson_id, a.deleted, a.patient_fname, a.patient_lname, a.merge_status, 
                         a.merge_status_desc, a.object_count, a.status_last_updated_on, a.invoiced, a.received_via_dicom_router, a.service_codes, a.discount_per, a.is_free, a.promo_reason_id, a.promo_applied_by, 
                         a.promo_applied_on, a.physician_note, a.prelim_radiologist_pacs, i.name AS prelim_radiologist, a.final_radiologist_pacs, k.name AS final_radiologist, a.rpt_approve_date, a.consult_applied, a.finishing_datetime, 
                         a.rpt_record_date, a.manually_assigned, a.dict_radiologist_pacs, j.name AS dict_radiologist, a.dict_tanscriptionist_id, a.transcription_finishing_datetime, a.beyond_hour_stat,
                         d.custom_report 
FROM            dbo.study_hdr AS a WITH (NOLOCK) INNER JOIN
                         dbo.modality AS b WITH (NOLOCK) ON a.modality_id = b.id LEFT OUTER JOIN
						 dbo.study_hdr_final_reports fr  WITH (NOLOCK) ON a.id = fr.study_hdr_id LEFT OUTER JOIN
						 dbo.users usr  WITH (NOLOCK) ON fr.created_by = usr.id LEFT OUTER JOIN
                         dbo.physicians AS c  WITH (NOLOCK) ON a.physician_id = c.id LEFT OUTER JOIN
                         dbo.institutions AS d  WITH (NOLOCK) ON a.institution_id = d.id LEFT OUTER JOIN
                         dbo.species AS e  WITH (NOLOCK) ON a.species_id = e.id LEFT OUTER JOIN
                         dbo.body_part AS f  WITH (NOLOCK) ON a.body_part_id = f.id LEFT OUTER JOIN
                         dbo.body_part AS g  WITH (NOLOCK) ON a.body_part_id = g.id LEFT OUTER JOIN
                         dbo.radiologists AS h  WITH (NOLOCK) ON a.radiologist_id = h.id LEFT OUTER JOIN
                         dbo.radiologists AS i  WITH (NOLOCK) ON a.prelim_radiologist_id = i.id LEFT OUTER JOIN
                         dbo.radiologists AS j  WITH (NOLOCK) ON a.dict_radiologist_id = j.id LEFT OUTER JOIN
                         dbo.radiologists AS k  WITH (NOLOCK) ON a.final_radiologist_id = k.id LEFT OUTER JOIN
                         dbo.breed AS l  WITH (NOLOCK) ON a.breed_id = l.id LEFT OUTER JOIN
                         dbo.sys_study_status AS m  WITH (NOLOCK) ON a.study_status = m.status_id LEFT OUTER JOIN
                         dbo.sys_study_category AS n  WITH (NOLOCK) ON a.category_id = n.id
where a.study_status=4
UNION ALL
SELECT                   a.id,
                         a.study_uid, a.received_date, a.patient_name, b.name AS modality, n.name category, d.name AS institution, c.name AS physician, h.name AS radiologist, m.status_desc AS study_status, usr.name approved_by, a.study_date, a.accession_no_pacs, a.accession_no, a.institution_name_pacs, a.manufacturer_name, a.manufacturer_model_no, a.device_serial_no, 
                         a.referring_physician_pacs, a.patient_id_pacs, a.patient_name_pacs, a.patient_sex_pacs, a.patient_sex, a.patient_sex_neutered_pacs, a.patient_sex_neutered, a.patient_weight_pacs, 
                         a.patient_weight, a.patient_dob_pacs, a.patient_dob_accepted, a.patient_age_pacs, a.patient_age_accepted, a.sex_neutered_pacs, a.sex_neutered_accepted, a.owner_name_pacs, a.owner_first_name, a.owner_last_name, 
                         a.species_pacs, e.name AS species, a.breed_pacs, l.name AS breed, a.modality_pacs, a.body_part_pacs, f.name AS body_part, a.study_desc, a.reason_pacs, a.reason_accepted, a.img_count_pacs, 
                         a.img_count, a.img_count_accepted, a.study_status_pacs, a.synched_on, a.updated_by, a.date_updated, a.pacs_wb, a.prelim_rpt_updated, a.final_rpt_updated, a.radiologist_pacs, 
                         a.modality_ae_title, a.prelim_sms_updated, a.final_sms_updated, a.patient_weight_kgs, a.wt_uom, a.priority_id_pacs, a.priority_id, a.salesperson_id, a.deleted, a.patient_fname, a.patient_lname, a.merge_status, 
                         a.merge_status_desc, a.object_count, a.status_last_updated_on, a.invoiced, a.received_via_dicom_router, a.service_codes, a.discount_per, a.is_free, a.promo_reason_id, a.promo_applied_by, 
                         a.promo_applied_on, a.physician_note, a.prelim_radiologist_pacs, i.name AS prelim_radiologist, a.final_radiologist_pacs, k.name AS final_radiologist, a.rpt_approve_date, a.consult_applied, a.finishing_datetime, 
                         a.rpt_record_date, a.manually_assigned, a.dict_radiologist_pacs, j.name AS dict_radiologist, a.dict_tanscriptionist_id, a.transcription_finishing_datetime, a.beyond_hour_stat,
                         d.custom_report
FROM            dbo.study_hdr_archive AS a WITH (NOLOCK) INNER JOIN
                         dbo.modality AS b  WITH (NOLOCK) ON a.modality_id = b.id LEFT OUTER JOIN
						 dbo.study_hdr_final_reports fr  WITH (NOLOCK) ON a.id = fr.study_hdr_id LEFT OUTER JOIN
						 dbo.users usr  WITH (NOLOCK) ON fr.created_by = usr.id LEFT OUTER JOIN
                         dbo.physicians AS c  WITH (NOLOCK) ON a.physician_id = c.id LEFT OUTER JOIN
                         dbo.institutions AS d  WITH (NOLOCK) ON a.institution_id = d.id LEFT OUTER JOIN
                         dbo.species AS e  WITH (NOLOCK) ON a.species_id = e.id LEFT OUTER JOIN
                         dbo.body_part AS f  WITH (NOLOCK) ON a.body_part_id = f.id LEFT OUTER JOIN
                         dbo.body_part AS g  WITH (NOLOCK) ON a.body_part_id = g.id LEFT OUTER JOIN
                         dbo.radiologists AS h  WITH (NOLOCK) ON a.radiologist_id = h.id LEFT OUTER JOIN
                         dbo.radiologists AS i  WITH (NOLOCK) ON a.prelim_radiologist_id = i.id LEFT OUTER JOIN
                         dbo.radiologists AS j  WITH (NOLOCK) ON a.dict_radiologist_id = j.id LEFT OUTER JOIN
                         dbo.radiologists AS k  WITH (NOLOCK) ON a.final_radiologist_id = k.id LEFT OUTER JOIN
                         dbo.breed AS l  WITH (NOLOCK) ON a.breed_id = l.id LEFT OUTER JOIN
                         dbo.sys_study_status AS m  WITH (NOLOCK) ON a.study_status = m.status_id LEFT OUTER JOIN
                         dbo.sys_study_category AS n  WITH (NOLOCK) ON a.category_id = n.id
;
GO
