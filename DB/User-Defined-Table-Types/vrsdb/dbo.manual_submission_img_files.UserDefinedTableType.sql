USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[manual_submission_img_files]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[manual_submission_img_files]
GO
/****** Object:  UserDefinedTableType [dbo].[manual_submission_img_files]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[manual_submission_img_files] AS TABLE(
	[img_file_srl_no] [int] NOT NULL,
	[img_file_name] [nvarchar](250) NOT NULL,
	[img_file] [varbinary](max) NOT NULL
)
GO
