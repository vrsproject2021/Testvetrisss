USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_studies_to_merge_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_studies_to_merge_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_studies_to_merge_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_studies_to_merge_fetch : fetch case list header
** Created By   : Pavel Guha
** Created On   : 14/12/2020
*******************************************************/
--exec case_list_studies_to_merge_fetch 'c6141fdc-f151-4ca7-aabb-81b99d46d72c','1.3.6.1.4.1.11157.2002478011572111.1607102310.1546569'
create procedure [dbo].[case_list_studies_to_merge_fetch]
    @id uniqueidentifier,	
	@study_uid nvarchar(100)
as
begin
	 set nocount on

	 declare @patient_sex_pacs nvarchar(10),
			 @patient_name_pacs nvarchar(100),
			 @institution_id uniqueidentifier

	select @patient_name_pacs = isnull(patient_name_pacs,''),
	       @patient_sex_pacs = isnull(patient_sex_pacs,''),
		   @institution_id = isnull(institution_id,'00000000-0000-0000-0000-000000000000')
	from study_hdr 
	where id=@id

	 select sh.id,sh.study_uid,sh.synched_on,sh.study_date,
			patient_name = case when sh.patient_name like '%[^a-zA-Z0-9.( )_-]%' then '' else isnull(sh.patient_fname,'') + ' ' +  isnull(sh.patient_lname,'') end,
			modality     = isnull(m.name,''),
			img_count    = isnull(sh.img_count,0),
			status_desc  = s.vrs_status_desc,
			merge_compare_none = ''
	 from study_hdr sh
	 left outer join modality m on m.id = sh.modality_id
	 inner join sys_study_status_pacs s on s.status_id = sh.study_status_pacs
	 where isnull(sh.patient_name_pacs,'') = @patient_name_pacs
	 and isnull(sh.patient_sex_pacs,'')= @patient_sex_pacs
	 and sh.institution_id             = @institution_id
	 and sh.study_status_pacs          = 0
	 and isnull(sh.merge_status,'N')  = 'N'
	 and sh.study_uid <> @study_uid
	 union
	 select id=shms.study_id,shms.study_uid,sh.synched_on,sh.study_date,
	        patient_name  = isnull(sh.patient_fname,'') + ' ' +  isnull(sh.patient_lname,''),
			modality      = isnull(m.name,''),
			img_count     = isnull(sh.img_count,0),
			status_desc   = s.vrs_status_desc,
			shms.merge_compare_none
	 from study_hdr_merged_studies shms
	 inner join study_hdr sh on sh.id = shms.study_hdr_id
	 left outer join modality m on m.id = sh.modality_id
	 inner join sys_study_status_pacs s on s.status_id = sh.study_status_pacs
	 where shms.study_hdr_id=@id
	 order by sh.synched_on

	set nocount off
end

GO
