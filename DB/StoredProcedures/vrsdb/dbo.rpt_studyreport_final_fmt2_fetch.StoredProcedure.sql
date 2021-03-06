USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_final_fmt2_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_studyreport_final_fmt2_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_final_fmt2_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_studyreport_final_fmt2_fetch : fetch 
                  final report details (format 2)
** Created By   : Pavel Guha
** Created On   : 28/07/2020
*******************************************************/
--exec rpt_studyreport_final_fmt2_fetch 'BF3BB925-7A4C-4007-88F9-12C9B643C5AE'

create procedure [dbo].[rpt_studyreport_final_fmt2_fetch]
    @id nvarchar(36)
    
as
begin
	 set nocount on

	 declare @study_types varchar(max)
	 set @study_types=''

	 if(select count(id) from study_hdr where id= @id)>0
		begin
			
			if(select count(study_type_id) from study_hdr_study_types where study_hdr_id=@id)=1
				begin
					select @study_types= name
					from modality_study_types
					where id in (select study_type_id from study_hdr_study_types where study_hdr_id=@id) 
				end
			else
				begin
					select @study_types = isnull(@study_types,'') + name +','
					from modality_study_types
					where id in (select study_type_id from study_hdr_study_types where study_hdr_id=@id)

					set @study_types = substring(@study_types,0,len(@study_types))
				end
				

			select hdr.study_uid,
				   hdr.study_date,
				   hdr.received_date,
				   patient_id = isnull(hdr.patient_id,''),
				   patient_name = isnull(hdr.patient_name,''),
				   patient_sex = isnull(hdr.patient_sex,''),
				   patient_spayed_neutered = isnull(hdr.patient_sex_neutered,''),
				   patient_age_accepted = isnull(hdr.patient_age_accepted,0),
				   owner_name = rtrim(ltrim(isnull(hdr.owner_first_name,'') + ' ' +  isnull(hdr.owner_last_name,''))),
				   species_name=isnull(sp.name,''),
				   breed_name = isnull(br.name,''),
				   modality_name=isnull(m.name,''),
				   institution_code = isnull(ins.code,''),
				   institution_name= isnull(ins.name,''),
				   physician_name = isnull(ph.name,''),
				   reason_accepted= isnull(hdr.reason_accepted,''),
				   accession_no = isnull(hdr.accession_no,''),
				   image_count = isnull(hdr.img_count,0),
				   study_types = isnull(@study_types,''),
				   study_text =isnull(sr.study_text,''),
				   report_findings =isnull(sr.report_findings,''),
				   report_conclusion = isnull(sr.report_conclusion,''),
				   final_radiologist = isnull(r.name,''),
				   rpt_approve_date = isnull(sr.date_updated,sr.date_created)
			from study_hdr hdr
			left outer join modality m on m.id= hdr.modality_id
			left outer join body_part bp on bp.id= hdr.body_part_id
			left outer join species sp on sp.id= hdr.species_id
			left outer join breed br on br.id= hdr.breed_id
			left outer join institutions ins on ins.id= hdr.institution_id
			left outer join physicians ph on ph.id= hdr.physician_id
			inner join study_reports sr on sr.study_hdr_id = hdr.id
			left outer join radiologists r on r.id = hdr.assigned_final_radiologist_id
			where convert(varchar(36),hdr.id)=@id
			and sr.report_type='F'
		end
	else
		begin
			if(select count(study_type_id) from study_hdr_study_types_archive where study_hdr_id=@id)=1
				begin
					select @study_types= name
					from modality_study_types
					where id in (select study_type_id from study_hdr_study_types_archive where study_hdr_id=@id) 
				end
			else
				begin
					select @study_types = isnull(@study_types,'') + name +','
					from modality_study_types
					where id in (select study_type_id from study_hdr_study_types_archive where study_hdr_id=@id)

					set @study_types = substring(@study_types,0,len(@study_types))
				end
				

			select hdr.study_uid,
				   hdr.study_date,
				   hdr.received_date,
				   patient_id = isnull(hdr.patient_id,''),
				   patient_name = isnull(hdr.patient_name,''),
				   patient_sex = isnull(hdr.patient_sex,''),
				   patient_spayed_neutered = isnull(hdr.patient_sex_neutered,''),
				   patient_age_accepted = isnull(hdr.patient_age_accepted,0),
				   owner_name = rtrim(ltrim(isnull(hdr.owner_first_name,'') + ' ' +  isnull(hdr.owner_last_name,''))),
				   species_name=isnull(sp.name,''),
				   breed_name = isnull(br.name,''),
				   modality_name=isnull(m.name,''),
				   institution_code = isnull(ins.code,''),
				   institution_name= isnull(ins.name,''),
				   physician_name = isnull(ph.name,''),
				   reason_accepted= isnull(hdr.reason_accepted,''),
				   accession_no = isnull(hdr.accession_no,''),
				   image_count = isnull(hdr.img_count,0),
				   study_types = isnull(@study_types,''),
				   study_text =isnull(sr.study_text,''),
				   report_findings =isnull(sr.report_findings,''),
				   report_conclusion = isnull(sr.report_conclusion,''),
				   final_radiologist = isnull(r.name,''),
				   rpt_approve_date = isnull(sr.date_updated,sr.date_created)
			from study_hdr_archive hdr
			left outer join modality m on m.id= hdr.modality_id
			left outer join body_part bp on bp.id= hdr.body_part_id
			left outer join species sp on sp.id= hdr.species_id
			left outer join breed br on br.id= hdr.breed_id
			left outer join institutions ins on ins.id= hdr.institution_id
			left outer join physicians ph on ph.id= hdr.physician_id
			left outer join study_hdr_final_reports_archive fr on fr.study_hdr_id = hdr.id
			left outer join radiologists r on r.id = hdr.final_radiologist_id
			inner join study_reports_archive sr on sr.study_hdr_id = hdr.id
			where convert(varchar(36),hdr.id)=@id
			and sr.report_type='F'
		end

	set nocount off
end

GO
