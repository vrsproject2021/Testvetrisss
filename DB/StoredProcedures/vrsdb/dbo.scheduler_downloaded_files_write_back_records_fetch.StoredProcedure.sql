USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_files_write_back_records_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_downloaded_files_write_back_records_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_files_write_back_records_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_downloaded_files_write_back_records_fetch :  
				 fetch downloaded case(s)
				  eligible for write back to pacs
** Created By   : Pavel Guha
** Created On   : 14/04/2019
*******************************************************/
-- exec scheduler_downloaded_files_write_back_records_fetch
create PROCEDURE [dbo].[scheduler_downloaded_files_write_back_records_fetch] 
as
begin
	set nocount on

	select id,study_uid,study_date,
		   patient_id,
		   patient_name = rtrim(ltrim(isnull(patient_lname,'') + ' ' + isnull(patient_fname,''))),
		   institution_code=isnull(institution_code,''),institution_name =isnull(institution_name,'')
	from scheduler_file_downloads
	where approve_for_pacs='Y'
	and file_xfer_count > 0
	and write_back_status= 'N'
	order by date_downloaded


	set nocount off
end

GO
