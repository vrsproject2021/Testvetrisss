USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[case_study_study_type]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[case_study_study_type]
GO
/****** Object:  UserDefinedTableType [dbo].[case_study_study_type]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[case_study_study_type] AS TABLE(
	[study_type_id] [uniqueidentifier] NOT NULL,
	[srl_no] [nvarchar](100) NOT NULL
)
GO
