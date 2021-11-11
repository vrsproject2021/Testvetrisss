USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[case_study_report_addendum]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[case_study_report_addendum]
GO
/****** Object:  UserDefinedTableType [dbo].[case_study_report_addendum]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[case_study_report_addendum] AS TABLE(
	[srl_no] [int] NOT NULL,
	[addendum_text] [ntext] NOT NULL,
	[addendum_text_html] [ntext] NULL
)
GO
