USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_dcm_files]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_dcm_files]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_dcm_files]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_dcm_files : fetch single task header docs.
** Created By   : Pavel Guha
** Created On   : 20/06/2020
*******************************************************/
--exec case_list_fetch_dcm_files 'faded955-300e-4517-8f6c-e6513b1889f6'
create procedure [dbo].[case_list_fetch_dcm_files]
    @id uniqueidentifier
as
begin
	 set nocount on
	
	select dcm_file_srl_no,dcm_file_id,dcm_file_name,dcm_file
	from study_hdr_dcm_files
	where study_hdr_id=@id
	order by dcm_file_srl_no
		
	set nocount off
end

GO
