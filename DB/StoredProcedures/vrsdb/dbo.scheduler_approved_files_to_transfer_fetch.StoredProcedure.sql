USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_approved_files_to_transfer_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_approved_files_to_transfer_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_approved_files_to_transfer_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_approved_files_to_transfer_fetch : 
                  fetch file download records to finalise
** Created By   : Pavel Guha
** Created On   : 02/08/2019
*******************************************************/
--exec [scheduler_approved_files_to_transfer_fetch]
CREATE procedure [dbo].[scheduler_approved_files_to_transfer_fetch]
as
begin
	
	set nocount on

	select fdd.id,fdd.study_uid,fdd.file_name,
	       fd.institution_id,fd.institution_code,fd.institution_name,
		   fd.study_date,fd.patient_id,fd.patient_fname,fd.patient_lname,
		   i.format_dcm_files,fdd.import_session_id,fd.date_downloaded
	from scheduler_file_downloads_dtls fdd
	inner join scheduler_file_downloads fd on fd.id = fdd.id
	inner join institutions i on i.id = fd.institution_id
	where fdd.sent_to_pacs='N'
	and fd.approve_for_pacs='Y'
	and fdd.import_session_id not like 'S1DXXX%'
	order by fd.date_downloaded
	
	

	set nocount off


end


GO
