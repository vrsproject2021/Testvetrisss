USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_StudyReport_fetch_hdr]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_StudyReport_fetch_hdr]
GO
/****** Object:  StoredProcedure [dbo].[rpt_StudyReport_fetch_hdr]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_StudyReport_fetch_hdr : fetch case list header
** Created By   : Pavel Guha
** Created On   : 20/04/2019
*******************************************************/
--exec rpt_StudyReport_fetch_hdr'28292a63-f658-48a2-94e1-9284339522dc'
CREATE procedure [dbo].[rpt_StudyReport_fetch_hdr]
    @id nvarchar(36)
    
as
begin
	 set nocount on

	select hdr.study_uid,hdr.study_date,
		   patient_id = isnull(hdr.patient_id,''),
		   patient_name = isnull(hdr.patient_name,''),
		   patient_sex = isnull(hdr.patient_sex,''),
		   patient_age_accepted = isnull(hdr.patient_age_accepted,0),
		   owner_name = rtrim(ltrim(isnull(hdr.owner_first_name,'') + ' ' +  isnull(hdr.owner_last_name,''))),
		   species_name=isnull(sp.name,''),
		   breed_name = isnull(br.name,''),
		   modality_name=isnull(m.name,''),
		   body_part_name = isnull(bp.name,''),
		   institution_name= isnull(ins.name,''),
		   physician_name = isnull(ph.name,''),
		   reason_accepted= isnull(hdr.reason_accepted,''),
		   study_type=isnull(mst.name,'')
	from study_hdr hdr
	left outer join modality m on m.id= hdr.modality_id
	left outer join body_part bp on bp.id= hdr.body_part_id
	left outer join species sp on sp.id= hdr.species_id
	left outer join breed br on br.id= hdr.breed_id
	left outer join institutions ins on ins.id= hdr.institution_id
	left outer join physicians ph on ph.id= hdr.physician_id
	left outer join modality_study_types mst on mst.id = hdr.study_type_id
	where convert(varchar(36),hdr.id)=@id


	set nocount off
end

GO
