USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[manual_submission_files]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[manual_submission_files]
GO
/****** Object:  UserDefinedTableType [dbo].[manual_submission_files]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[manual_submission_files] AS TABLE(
	[file_srl_no] [int] NOT NULL,
	[file_name] [nvarchar](250) NOT NULL,
	[file] [varbinary](max) NOT NULL,
	[file_type] [nchar](1) NOT NULL
)
GO
