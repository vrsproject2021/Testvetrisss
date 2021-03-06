USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_study_files]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_study_files]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_study_files]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_study_files : fetch study files
** Created By   : Pavel Guha
** Created On   : 03/09/2020
*******************************************************/
--exec case_list_fetch_study_files 'faded955-300e-4517-8f6c-e6513b1889f6'
create procedure [dbo].[case_list_fetch_study_files]
    @id uniqueidentifier
as
begin
	 set nocount on

	 create table #tmp
	 (
		id uniqueidentifier,
		file_srl_no int identity(1,1),
		file_name nvarchar(250),
		file_type nchar(1),
		file_type_desc nvarchar(30),
		file_content varbinary(max)
	 )
	
	insert into #tmp(id,file_name,file_type,file_type_desc,file_content)
	(select dcm_file_id,dcm_file_name,'D','DICOM',dcm_file
	from study_hdr_dcm_files
	where study_hdr_id=@id)
	order by dcm_file_srl_no

	insert into #tmp(id,file_name,file_type,file_type_desc,file_content)
	(select document_id,document_link,
			case
				when upper(document_file_type) ='.JPG' then 'I'
				when upper(document_file_type) ='.JPEG' then 'I'
				when upper(document_file_type) ='.GIF' then 'I'
				when upper(document_file_type) ='.PNG' then 'I'
				when upper(document_file_type) ='.BMP' then 'I'
				when upper(document_file_type) ='.PDF' then 'P'
			end file_type,
			case
				when upper(document_file_type) ='.JPG' then 'JPG/JPEG'
				when upper(document_file_type) ='.JPEG' then 'JPG/JPEG'
				when upper(document_file_type) ='.GIF' then 'GIF'
				when upper(document_file_type) ='.PNG' then 'PNG'
				when upper(document_file_type) ='.BMP' then 'BMP'
				when upper(document_file_type) ='.PDF' then 'PDF'
			end file_type_desc,
			document_file
	from study_hdr_documents
	where study_hdr_id=@id)
	order by document_srl_no

	select * from #tmp order by file_srl_no

	set nocount off
end
GO
