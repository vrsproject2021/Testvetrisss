USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[manual_submission_dcm_files]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[manual_submission_dcm_files]
GO
/****** Object:  UserDefinedTableType [dbo].[manual_submission_dcm_files]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[manual_submission_dcm_files] AS TABLE(
	[dcm_file_srl_no] [int] NOT NULL,
	[dcm_file_name] [nvarchar](250) NOT NULL,
	[dcm_file] [varbinary](max) NOT NULL
)
GO
