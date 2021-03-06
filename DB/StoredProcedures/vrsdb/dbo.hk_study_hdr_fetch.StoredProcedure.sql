USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_study_hdr_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_study_hdr_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_study_hdr_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_study_hdr_fetch : fetch case list header
** Created By   : Pavel Guha
** Created On   : 29/05/2019
*******************************************************/
--exec hk_study_hdr_fetch '3BE93540-F0C9-4947-9E46-7A7B37F8D9FD',1,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[hk_study_hdr_fetch]
    @id uniqueidentifier

as
begin
	 set nocount on

	 if(select count(id) from study_hdr where id=@id)> 0
		begin
			select hdr.study_uid,hdr.study_date,
				   study_desc=isnull(hdr.study_desc,''),
				   accession_no= isnull(hdr.accession_no,''),
				   patient_id = isnull(hdr.patient_id,''),
				   patient_name = isnull(hdr.patient_name,''),
				   patient_sex = case when isnull(hdr.patient_sex,'') = 'M' then 'Male' when isnull(hdr.patient_sex,'') = 'F' then 'Female' when isnull(hdr.patient_sex,'') = 'O' then 'Unknown' else '' end,
				   sex_neutered_accepted =isnull(hdr.patient_sex_neutered,''),
				   patient_weight_pacs= isnull(hdr.patient_weight_pacs,0),
				   case
						when isnull(wt_uom,'') = 'lbs' then isnull(hdr.patient_weight,0)
						when isnull(wt_uom,'') = 'kgs' then isnull(hdr.patient_weight_kgs,0)
						when isnull(wt_uom,'') = '' then isnull(hdr.patient_weight_pacs,0)
				   end patient_weight,
				   wt_uom = isnull(hdr.wt_uom,''),
				   patient_dob_accepted = isnull(hdr.patient_dob_accepted,'01jan1900'),
				   patient_age_accepted = isnull(hdr.patient_age_accepted,''),
				   owner_first_name = isnull(hdr.owner_first_name,''), owner_last_name = isnull(hdr.owner_last_name,''),
				   species_id = isnull(hdr.species_id,0),species_name=isnull(sp.name,''),
				   breed_id = isnull(hdr.breed_id,'00000000-0000-0000-0000-000000000000'),breed_name = isnull(br.name,''),
				   modality_id  = isnull(hdr.modality_id,0),modality_name=isnull(m.name,''),
				   institution_id = isnull(hdr.institution_id,'00000000-0000-0000-0000-000000000000'),institution_name= isnull(ins.name,''),
				   physician_id = isnull(hdr.physician_id,'00000000-0000-0000-0000-000000000000'),physician_name = isnull(ph.name,''),
				   reason_accepted= isnull(hdr.reason_accepted,''),
				   physician_note = isnull(hdr.physician_note,''),
				   img_count= isnull(hdr.img_count,0),object_count=isnull(hdr.object_count,0),
				   img_count_accepted=isnull(hdr.img_count_accepted,'N'),
				   priority_id = isnull(hdr.priority_id,0),priority_desc = isnull(pr.priority_desc,'')
			from study_hdr hdr
			left outer join modality m on m.id= hdr.modality_id
			left outer join species sp on sp.id= hdr.species_id
			left outer join breed br on br.id= hdr.breed_id
			left outer join institutions ins on ins.id= hdr.institution_id
			left outer join physicians ph on ph.id= hdr.physician_id
			left outer join sys_priority pr on pr.priority_id = hdr.priority_id 
			where hdr.id=@id
		end
	 else  if(select count(id) from study_hdr_archive where id=@id)> 0
		begin
			select hdr.study_uid,hdr.study_date,
				   study_desc=isnull(hdr.study_desc,''),
				   accession_no= isnull(hdr.accession_no,''),
				   patient_id = isnull(hdr.patient_id,''),
				   patient_name = isnull(hdr.patient_name,''),
				   patient_sex = case when isnull(hdr.patient_sex,'') = 'M' then 'Male' when isnull(hdr.patient_sex,'') = 'F' then 'Female' when isnull(hdr.patient_sex,'') = 'O' then 'Unknown' else '' end,
				   sex_neutered_accepted =isnull(hdr.patient_sex_neutered,''),
				   patient_weight_pacs= isnull(hdr.patient_weight_pacs,0),
				   case
						when isnull(wt_uom,'') = 'lbs' then isnull(hdr.patient_weight,0)
						when isnull(wt_uom,'') = 'kgs' then isnull(hdr.patient_weight_kgs,0)
						when isnull(wt_uom,'') = '' then isnull(hdr.patient_weight_pacs,0)
				   end patient_weight,
				   wt_uom = isnull(hdr.wt_uom,''),
				   patient_dob_accepted = isnull(hdr.patient_dob_accepted,'01jan1900'),
				   patient_age_accepted = isnull(hdr.patient_age_accepted,''),
				   owner_first_name = isnull(hdr.owner_first_name,''), owner_last_name = isnull(hdr.owner_last_name,''),
				   species_id = isnull(hdr.species_id,0),species_name=isnull(sp.name,''),
				   breed_id = isnull(hdr.breed_id,'00000000-0000-0000-0000-000000000000'),breed_name = isnull(br.name,''),
				   modality_id  = isnull(hdr.modality_id,0),modality_name=isnull(m.name,''),
		  
				   institution_id = isnull(hdr.institution_id,'00000000-0000-0000-0000-000000000000'),institution_name= isnull(ins.name,''),
				   physician_id = isnull(hdr.physician_id,'00000000-0000-0000-0000-000000000000'),physician_name = isnull(ph.name,''),
				   reason_accepted= isnull(hdr.reason_accepted,''),
				    physician_note = isnull(hdr.physician_note,''),
				   img_count= isnull(hdr.img_count,0),
				   object_count=isnull(hdr.object_count,0),
				   img_count_accepted=isnull(hdr.img_count_accepted,'N'),
				   priority_id = isnull(hdr.priority_id,0),priority_desc = isnull(pr.priority_desc,'')
			from study_hdr_archive hdr
			left outer join modality m on m.id= hdr.modality_id
			left outer join species sp on sp.id= hdr.species_id
			left outer join breed br on br.id= hdr.breed_id
			left outer join institutions ins on ins.id= hdr.institution_id
			left outer join physicians ph on ph.id= hdr.physician_id
			left outer join sys_priority pr on pr.priority_id = hdr.priority_id 
			where hdr.id=@id
		end

	
	
		
	set nocount off
end


GO
