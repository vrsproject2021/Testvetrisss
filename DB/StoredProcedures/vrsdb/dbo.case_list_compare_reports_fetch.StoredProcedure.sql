USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_compare_reports_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_compare_reports_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_compare_reports_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_compare_reports_fetch : 
                  fetch compare report details
** Created By   : Pavel Guha 
** Created On   : 02/01/2020
*******************************************************/
--exec case_list_compare_reports_fetch 'F9362A19-8C32-4D2C-AB3E-B3EFFB285EDB'
CREATE procedure [dbo].[case_list_compare_reports_fetch]
	@study_id uniqueidentifier
	
as
	begin
		set nocount on

		if(select count(id) from study_hdr where id=@study_id)> 0
			begin
				select prelim_radiologist_id = isnull(sh.radiologist_id,'00000000-0000-0000-0000-000000000000'),
				       prelim_radiologist = isnull(r1.name,'None'),
					   sh.final_radiologist_id,
				       final_radiologist = r2.name,
					   dict_tanscriptionist_id = isnull(sh.dict_tanscriptionist_id,'00000000-0000-0000-0000-000000000000'),
					   dict_tanscriptionist_name = isnull(t.name,''),
					   custom_report = isnull(i.custom_report,'N'),
					   patient_name = isnull(sh.patient_name,''),
					   rating_reason = isnull(arr.reason,'')
				from study_hdr sh
				left outer join radiologists r1 on r1.id = sh.radiologist_id
				inner join radiologists r2 on r2.id = sh.final_radiologist_id
				left outer join transciptionists t on t.id = sh.dict_tanscriptionist_id
				inner join institutions i on i.id = sh.institution_id
				inner join study_hdr_final_reports fr on fr.study_hdr_id = sh.id
				left outer join abnormal_rpt_reasons arr on arr.id = fr.rating_reason_id
				where sh.id=@study_id
			end
		else if(select count(id) from study_hdr_archive where id=@study_id)> 0
			begin
				select prelim_radiologist_id = isnull(sh.radiologist_id,'00000000-0000-0000-0000-000000000000'),
				       prelim_radiologist = isnull(r1.name,'None'),
					   sh.final_radiologist_id,
				       final_radiologist = r2.name,
					   dict_tanscriptionist_id = isnull(sh.dict_tanscriptionist_id,'00000000-0000-0000-0000-000000000000'),
					   dict_tanscriptionist_name = isnull(t.name,''),
					   custom_report = isnull(i.custom_report,'N'),
					   patient_name = isnull(sh.patient_name,''),
					   rating_reason = isnull(arr.reason,'')
				from study_hdr_archive sh
				left outer join radiologists r1 on r1.id = sh.radiologist_id
				inner join radiologists r2 on r2.id = sh.final_radiologist_id
				left outer join transciptionists t on t.id = sh.dict_tanscriptionist_id
				inner join institutions i on i.id = sh.institution_id
				inner join study_hdr_final_reports_archive fr on fr.study_hdr_id = sh.id
				left outer join abnormal_rpt_reasons arr on arr.id = fr.rating_reason_id
				where sh.id=@study_id
			end
		
		set nocount off

	end
GO
