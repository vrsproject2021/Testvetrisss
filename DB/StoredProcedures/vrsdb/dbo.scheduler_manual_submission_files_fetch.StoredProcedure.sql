USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_manual_submission_files_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_manual_submission_files_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_manual_submission_files_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_manual_submission_files_fetch : 
                  fetch list of files submitted manually
** Created By   : Pavel Guha
** Created On   : 30/06/2020
*******************************************************/
-- exec scheduler_manual_submission_files_fetch 
create procedure [dbo].[scheduler_manual_submission_files_fetch]

as
begin
	select file_id,session_id,file_name,file_content,institution_id,institution_code,institution_name,file_type
	from study_manual_upload_files
	order by date_uploaded
end

GO
