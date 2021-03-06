USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_fetch_studies_to_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_day_end_fetch_studies_to_delete]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_fetch_studies_to_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 
** Procedure    : scheduler_day_end_fetch_studies_to_delete : 
				  fetch dicom records to delete
** Created By   : Pavel Guha
** Created On   : 02/05/2020
*******************************************************/
-- exec scheduler_day_end_fetch_studies_to_delete 
CREATE procedure [dbo].[scheduler_day_end_fetch_studies_to_delete]

as
begin
	select  ssd.study_id,ssd.study_uid,ssd.received_via_dicom_router,inst_code= i.code,inst_name= i.name
	from scheduler_study_to_delete ssd
	inner join study_hdr sh on sh.id = ssd.study_id
	inner join institutions i on i.id = sh.institution_id
	order by ssd.study_id

	select id,study_uid,file_name 
	from scheduler_file_downloads_dtls
	where id in (select study_id from scheduler_study_to_delete where received_via_dicom_router='Y')
	order by id

end

GO
