USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_docs]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_docs]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_docs]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_docs : fetch single task header docs.
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
--exec case_list_fetch_docs '7bcb0fdc-2c3e-4762-be11-3959b5135570'
CREATE procedure [dbo].[case_list_fetch_docs]
    @id uniqueidentifier
as
begin
	 set nocount on

	  declare @strSQL varchar(max),
	          @db_name nvarchar(50)
	
	if(select count(study_hdr_id) from study_hdr_documents where study_hdr_id=@id)>0
		begin
			select document_id,document_name,document_srl_no,document_link,document_file_type,document_file,del_doc=''
			from study_hdr_documents
			where study_hdr_id=@id
			order by document_srl_no
		end
	else if(select count(study_hdr_id) from study_hdr_documents_archive where study_hdr_id=@id)>0
		begin
			select document_id,document_name,document_srl_no,document_link,document_file_type,document_file,del_doc=''
			from study_hdr_documents_archive
			where study_hdr_id=@id
			order by document_srl_no
		end
	else
		begin
			set @db_name=''
			exec common_get_study_database
				@id      = @id,
				@db_name = @db_name output

			set @strSQL ='select document_id,document_name,document_srl_no,document_link,document_file_type,document_file,del_doc='''' '
			set @strSQL =@strSQL + 'from ' + @db_name + '..study_hdr_documents_archive '
			set @strSQL =@strSQL + 'where study_hdr_id=''' + convert(varchar(36),@id) + ''' '
			set @strSQL =@strSQL + 'order by document_srl_no '

			--print @strSQL
			exec(@strSQL)
		end
		
	set nocount off
end

GO
