USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_status_update_records_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_status_update_records_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_status_update_records_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_status_update_records_fetch :  fetch case(s)
				  for updating the status
** Created By   : Pavel Guha
** Created On   : 19/04/2019
*******************************************************/
-- exec scheduler_status_update_records_fetch
CREATE PROCEDURE [dbo].[scheduler_status_update_records_fetch] 
as
begin
	set nocount on
	
	select id,study_uid,prelim_rpt_updated,final_rpt_updated,prelim_sms_updated,final_sms_updated,status_last_updated_on,archived='N',study_status_pacs
	from study_hdr 
	where study_status_pacs in (50,60,80,100)
	and study_status <5
	and isnull(received_via_dicom_router,'N')='N'
	and pacs_wb='N'
	and datediff(day,status_last_updated_on,getdate())<=1
	--union
	--select id,study_uid,prelim_rpt_updated,final_rpt_updated,prelim_sms_updated,final_sms_updated,status_last_updated_on,archived='Y',study_status_pacs
	--from study_hdr_archive 
	--where study_status_pacs = 0
	--and isnull(received_via_dicom_router,'N')='N'
	----where study_uid in ('2.25.92063370018945411224647997333063023646','2.25.181036689401192067129457527896772480564','2.25.24532829245046370553249181819011906261','2.25.249463354520595501729764671897293782046','2.25.20551767649367458943471636022242371673')
	----and study_uid not in ('1.3.6.1.4.1.11157.2002478011572111.1594224180.31','1.3.76.2.2.2.169.14.193.20200709142807')
	----union
	----select id,study_uid,prelim_rpt_updated,final_rpt_updated,prelim_sms_updated,final_sms_updated,received_date
	----from study_hdr_archive
	order by status_last_updated_on desc,study_status_pacs

	

	

	set nocount off
end

GO
