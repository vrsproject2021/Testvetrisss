USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_write_back_report_study_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_write_back_report_study_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_write_back_report_study_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_write_back_report_records_fetch :  fetch 
				  report records for write back to pacs
** Created By   : Pavel Guha
** Created On   : 31/07/2020
*******************************************************/
-- exec scheduler_write_back_report_records_fetch
CREATE PROCEDURE [dbo].[scheduler_write_back_report_study_fetch] 
as
begin
	set nocount on

	--study ids
	select study_hdr_id,study_uid,status_id=60,is_addendum='N' from study_hdr_dictated_reports where pacs_wb='Y'
	--union
	--select study_hdr_id,study_uid,status_id=60,is_addendum='N' from study_hdr_dictated_reports_archive where pacs_wb='Y'
	union
	select study_hdr_id,study_uid,status_id=80,is_addendum='N'  from study_hdr_prelim_reports where pacs_wb='Y'
	--union
	--select study_hdr_id,study_uid,status_id=80,is_addendum='N'  from study_hdr_prelim_reports_archive where pacs_wb='Y'
	union
	select study_hdr_id,study_uid,status_id=100,is_addendum='N'  from study_hdr_final_reports where pacs_wb='Y'
	union
	select study_hdr_id,study_uid,status_id=100,is_addendum='N' from study_hdr_final_reports_archive where pacs_wb='Y'
	union
	select study_hdr_id,study_uid,status_id=100,is_addendum='Y'  from study_report_addendums where pacs_wb='Y'
	union
	select study_hdr_id,study_uid,status_id=100,is_addendum='Y' from study_report_addendums_archive where pacs_wb='Y'

	
	set nocount off
end

GO
