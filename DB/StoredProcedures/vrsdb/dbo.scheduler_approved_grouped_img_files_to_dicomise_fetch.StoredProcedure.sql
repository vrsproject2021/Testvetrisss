USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_approved_grouped_img_files_to_dicomise_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_approved_grouped_img_files_to_dicomise_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_approved_grouped_img_files_to_dicomise_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_approved_grouped_img_files_to_transfer_fetch : 
                  fetch file download records to finalise
** Created By   : Pavel Guha
** Created On   : 02/08/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_approved_grouped_img_files_to_dicomise_fetch]
as
begin
	
	set nocount on

	select fdd.id,fdd.file_name,fdd.study_uid,fdd.series_instance_uid,fdd.series_no,
	       fd.institution_code,fd.institution_name,fd.modality,
		   fd.study_date,fd.patient_id,fd.patient_fname,fd.patient_lname,
		   fdd.import_session_id
	from scheduler_img_file_downloads_grouped_dtls fdd
	inner join scheduler_img_file_downloads_grouped fd on fd.id = fdd.id
	where fdd.sent_to_pacs='N'
	and fdd.dicomised='N'
	and fd.approve_for_pacs='Y'
	

	set nocount off


end


GO
