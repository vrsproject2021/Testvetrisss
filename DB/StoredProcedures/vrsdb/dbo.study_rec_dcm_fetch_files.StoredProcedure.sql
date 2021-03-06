USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_dcm_fetch_files]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[study_rec_dcm_fetch_files]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_dcm_fetch_files]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : study_rec_dcm_fetch_files : fetch dcm file list
** Created By   : Pavel Guha
** Created On   : 05/08/2019
*******************************************************/
--exec study_rec_dcm_fetch_files '8372EE06-B7BD-4D4B-AB6A-0128F742B108'
create procedure [dbo].[study_rec_dcm_fetch_files]
    @id uniqueidentifier	
as
begin
	 set nocount on
	create table #tmp
	(
		srl_no int identity(1,1),
		file_name nvarchar(250),
		sent_to_pacs nchar(1)
	)

	insert into #tmp(file_name,sent_to_pacs)
	(select file_name,sent_to_pacs
	from scheduler_file_downloads_dtls
	where id=@id)
	order by file_name

	select * from #tmp order by srl_no
		
	set nocount off
end

GO
