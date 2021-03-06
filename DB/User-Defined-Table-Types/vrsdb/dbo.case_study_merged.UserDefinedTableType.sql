USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[case_study_merged]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[case_study_merged]
GO
/****** Object:  UserDefinedTableType [dbo].[case_study_merged]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[case_study_merged] AS TABLE(
	[srl_no] [int] NOT NULL,
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[merge_compare_none] [nchar](1) NOT NULL,
	[image_count] [int] NOT NULL
)
GO
