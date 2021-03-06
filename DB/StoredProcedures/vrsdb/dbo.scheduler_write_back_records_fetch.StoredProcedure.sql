USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_write_back_records_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_write_back_records_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_write_back_records_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_write_back_records_fetch :  fetch case(s)
				  eligible fro write back to pacs
** Created By   : Pavel Guha
** Created On   : 14/04/2019
*******************************************************/
-- exec scheduler_write_back_records_fetch
CREATE PROCEDURE [dbo].[scheduler_write_back_records_fetch] 
as
begin
	set nocount on

	--study header
	select sh.id,sh.study_uid,sh.received_date,sh.accession_no,
		   sh.patient_id,sh.patient_name,sh.patient_weight,sh.patient_weight_kgs,
		   sh.patient_sex,sh.patient_sex_neutered,
		   sh.patient_dob_accepted,sh.patient_age_accepted,
	       species_name=sp.name,breed_name = br.name,
		   owner_name =  isnull(sh.owner_last_name,'') + ' ' + isnull(sh.owner_first_name,''),
		   modality_name=md.code,body_part_name = isnull(bp.name,''),
		   institution_code=isnull(ins.code,''),
		   --institution_name =isnull(ins.name,''),
		   institution_name =isnull(ins.code,''),
		   --physician_name = rtrim(ltrim(isnull(phys.lname,'') + '^' +  isnull(phys.fname,'') + '^^^' + isnull(phys.credentials,''))),
		   physician_name = rtrim(ltrim(isnull(phys.code,''))),
		   reason_accepted=replace(replace(replace(isnull(sh.reason_accepted,''),char(13),' '),char(9),' '),char(10),' '),
		   sh.img_count,sh.priority_id,sh.priority_id_pacs,
		   salesperson = isnull(sps.name,''),merge_status_desc=isnull(sh.merge_status_desc,''),
		   study_status=sh.study_status_pacs,
		   physician_note = replace(replace(replace(isnull(sh.physician_note,''),char(13),' '),char(9),' '),char(10),' '),
		   service_codes= upper(isnull(sh.service_codes,'')),
		   rpt_record_date = isnull(sh.rpt_record_date,'01jan1900'),
		   rpt_approve_date = isnull(sh.rpt_approve_date,'01jan1900'),
		   dict_radiologist_pacs    = isnull(r1.fname,'') + '^' + isnull(r1.lname,'') + '^^^' + isnull(r1.credentials,''),
		   prelim_radiologist_pacs  = isnull(r2.fname,'') + '^' + isnull(r2.lname,'') + '^^^' + isnull(r2.credentials,''),
		   final_radiologist_pacs   = isnull(r3.fname,'') + '^' + isnull(r3.lname,'') + '^^^' + isnull(r3.credentials,''),
		   radiologist_pacs         = isnull(radiologist_pacs,''),
		   submit_on                = isnull((select max(date_updated) from sys_case_study_status_log where study_id=sh.id and status_id_to=50),'01jan1900')
	from study_hdr sh
	inner join species sp on sp.id = sh.species_id
	inner join breed br on br.id = sh.breed_id
	inner join modality md on md.id = sh.modality_id
	left outer join body_part bp on bp.id = sh.body_part_id
	inner join institutions ins on ins.id=sh.institution_id
	left outer join physicians phys	on phys.id = sh.physician_id
	left outer join salespersons sps on sps.id = sh.salesperson_id
	left outer join study_hdr_dictated_reports dr on dr.study_hdr_id = sh.id
	left outer join study_hdr_prelim_reports pr on pr.study_hdr_id = sh.id
	left outer join study_hdr_final_reports fr on fr.study_hdr_id = sh.id
	left outer join radiologists r1 on r1.login_user_id = dr.created_by
	left outer join radiologists r2 on r2.login_user_id = pr.created_by
	left outer join radiologists r3 on r3.login_user_id = fr.created_by
	where sh.pacs_wb='Y'
	and sh.received_via_dicom_router in ('N','M')
	--and sh.study_status_pacs=0
	--union
	--select sh.id,sh.study_uid,sh.received_date,accession_no = isnull(sh.accession_no,isnull(sh.accession_no_pacs,'')),
	--	   patient_id= isnull(sh.patient_id,isnull(sh.patient_id_pacs,'')),patient_name= isnull(sh.patient_name,isnull(sh.patient_name_pacs,'')),
	--	   patient_weight=isnull(sh.patient_weight,isnull(sh.patient_weight,0)),patient_weight_kgs= isnull(sh.patient_weight_kgs,0),
	--	   patient_sex = isnull(sh.patient_sex,isnull(sh.patient_sex_pacs,'')),patient_sex_neutered= isnull(sh.patient_sex_neutered,isnull(sh.patient_sex_neutered_pacs,'')),
	--	   patient_dob_accepted = isnull(sh.patient_dob_accepted,isnull(sh.patient_dob_pacs,'01Jan1900')),patient_age_accepted = isnull(sh.patient_age_accepted,isnull(sh.patient_age_pacs,'')),
	--       species_name=isnull(sp.name,isnull(sh.breed_pacs,'')),breed_name = isnull(br.name,isnull(sh.breed_pacs,'')),
	--	   case when rtrim(ltrim(isnull(sh.owner_last_name,'') + ' ' + isnull(sh.owner_first_name,'')))='' then isnull(sh.owner_name_pacs,'') else isnull(sh.owner_last_name,'') + ' ' + isnull(sh.owner_first_name,'') end owner_name,
	--	   modality_name=isnull(md.code,isnull(sh.modality_pacs,'')),body_part_name = isnull(bp.name,''),
	--	   institution_code=isnull(ins.code,''),
	--	   --institution_name =isnull(ins.name,isnull(sh.institution_name_pacs,'')),
	--	   institution_name =isnull(ins.code,''),
	--	   --physician_name = rtrim(ltrim(isnull(phys.lname,'') + '^' +  isnull(phys.fname,'') + '^^^' + isnull(phys.credentials,''))),
	--	   physician_name = rtrim(ltrim(isnull(phys.code,''))),
	--	   reason_accepted=replace(replace(replace(isnull(sh.reason_accepted,''),char(13),' '),char(9),' '),char(10),' '),
	--	   sh.img_count,sh.priority_id,sh.priority_id_pacs,
	--	   salesperson = isnull(sps.name,''),merge_status_desc=isnull(sh.merge_status_desc,''),
	--	   study_status=sh.study_status_pacs,
	--	   physician_note = replace(replace(replace(isnull(sh.physician_note,''),char(13),' '),char(9),' '),char(10),' '),
	--	   service_codes= upper(isnull(sh.service_codes,'')),
	--	   rpt_record_date = isnull(sh.rpt_record_date,'01jan1900'),
	--	   rpt_approve_date = isnull(sh.rpt_approve_date,'01jan1900'),
	--	   dict_radiologist_pacs    = isnull(r1.fname,'') + '^' + isnull(r1.lname,'') + '^^^' + isnull(r1.credentials,''),
	--	   prelim_radiologist_pacs  = isnull(r2.fname,'') + '^' + isnull(r2.lname,'') + '^^^' + isnull(r2.credentials,''),
	--	   final_radiologist_pacs   = isnull(r3.fname,'') + '^' + isnull(r3.lname,'') + '^^^' + isnull(r3.credentials,''),
	--	   radiologist_pacs         = isnull(radiologist_pacs,''),
	--	   submit_on                = isnull((select max(date_updated) from sys_case_study_status_log where study_id=sh.id and status_id_to=50),'01jan1900')
	--from study_hdr_archive sh
	--left outer join species sp on sp.id = sh.species_id
	--left outer join breed br on br.id = sh.breed_id
	--left outer join modality md on md.id = sh.modality_id
	--left outer join body_part bp on bp.id = sh.body_part_id
	--left outer join institutions ins on ins.id=sh.institution_id
	--left outer join physicians phys	on phys.id = sh.physician_id
	--left outer join salespersons sps on sps.id = sh.salesperson_id
	--left outer join study_hdr_dictated_reports_archive dr on dr.study_hdr_id = sh.id
	--left outer join study_hdr_prelim_reports_archive pr on pr.study_hdr_id = sh.id
	--left outer join study_hdr_final_reports_archive fr on fr.study_hdr_id = sh.id
	--left outer join radiologists r1 on r1.login_user_id = dr.created_by
	--left outer join radiologists r2 on r2.login_user_id = pr.created_by
	--left outer join radiologists r3 on r3.login_user_id = fr.created_by
	--left outer join radiologists r4 on r4.id = sh.radiologist_id
	--where sh.pacs_wb='Y'
	--and sh.received_via_dicom_router='N'
	----and sh.study_status_pacs=0
	--order by sh.received_date

	select shst.study_hdr_id,shst.study_type_id,study_type_name=st.name,shst.write_back_tag
	from study_hdr_study_types shst
	inner join modality_study_types st on st.id = shst.study_type_id
	where shst.study_hdr_id in (select id
	                       from study_hdr
						   where pacs_wb='Y'
						   --and study_status_pacs=0
						   )
	--union
	--select shst.study_hdr_id,shst.study_type_id,study_type_name=st.name,shst.write_back_tag
	--from study_hdr_study_types_archive shst
	--inner join modality_study_types st on st.id = shst.study_type_id
	--where shst.study_hdr_id in (select id
	--                       from study_hdr_archive
	--					   where pacs_wb='Y'
	--					   --and study_status_pacs=0
	--					   )

	select study_hdr_id,document_id,document_name,document_srl_no,document_link,document_file_type,document_file
	from study_hdr_documents
	where study_hdr_id in (select id 
	                      from study_hdr 
						  where pacs_wb='Y' 
						  --and study_status_pacs=0
						  )
	--union
	--select study_hdr_id,document_id,document_name,document_srl_no,document_link,document_file_type,document_file
	--from study_hdr_documents_archive
	--where study_hdr_id in (select id 
	--                      from study_hdr_archive 
	--					  where pacs_wb='Y' 
	--					  --and study_status_pacs=0
	--					  )
	order by study_hdr_id,document_srl_no

	select study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,dcm_file
	from study_hdr_dcm_files
	where study_hdr_id in (select id 
	                      from study_hdr 
						  where pacs_wb='Y' 
						  --and study_status_pacs=0
						  )
	--union
	--select study_hdr_id,study_uid,dcm_file_id,dcm_file_name,dcm_file_srl_no,dcm_file
	--from study_hdr_dcm_files
	--where study_hdr_id in (select id 
	--                      from study_hdr_archive 
	--					  where pacs_wb='Y' 
	--					  --and study_status_pacs=0
	--					  )
	order by study_hdr_id,dcm_file_srl_no


	select field_code
	from sys_pacs_query_fields
	where service_id=2
	and is_study_type_field='Y'



	set nocount off
end

GO
