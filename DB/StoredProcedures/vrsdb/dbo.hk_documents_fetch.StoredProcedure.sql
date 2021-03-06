USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_documents_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_documents_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_documents_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_documents_fetch : fetch docs
** Created By   : Pavel Guha
** Created On   : 29/05/2019
*******************************************************/
--exec hk_documents_fetch 'faded955-300e-4517-8f6c-e6513b1889f6'
create procedure [dbo].[hk_documents_fetch]
    @id uniqueidentifier
as
begin
	 set nocount on
	
	select document_id,document_name,document_srl_no,document_link,document_file_type,document_file
	from study_hdr_documents
	where study_hdr_id=@id
	order by document_srl_no
		
	set nocount off
end

GO
