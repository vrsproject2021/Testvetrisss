USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[case_study_doc_type]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[case_study_doc_type]
GO
/****** Object:  UserDefinedTableType [dbo].[case_study_doc_type]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[case_study_doc_type] AS TABLE(
	[document_id] [uniqueidentifier] NOT NULL,
	[document_name] [nvarchar](100) NOT NULL,
	[document_srl_no] [int] NOT NULL,
	[document_link] [nvarchar](100) NOT NULL,
	[document_file_type] [nvarchar](5) NOT NULL,
	[document_file] [varbinary](max) NOT NULL
)
GO
