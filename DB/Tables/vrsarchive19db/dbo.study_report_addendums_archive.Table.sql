USE [vrsarchive19db]
GO
ALTER TABLE [dbo].[study_report_addendums_archive] DROP CONSTRAINT [DF_study_report_addendums_archive_pacs_wb]
GO
/****** Object:  Table [dbo].[study_report_addendums_archive]    Script Date: 21-09-2021 17:14:12 ******/
DROP TABLE [dbo].[study_report_addendums_archive]
GO
/****** Object:  Table [dbo].[study_report_addendums_archive]    Script Date: 21-09-2021 17:14:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[study_report_addendums_archive](
	[report_id] [uniqueidentifier] NOT NULL,
	[addendum_srl] [int] NOT NULL,
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[addendum_text] [ntext] NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[archived_by] [uniqueidentifier] NOT NULL,
	[date_archived] [datetime] NOT NULL,
	[addendum_text_html] [ntext] NULL,
	[pacs_wb] [nchar](1) NULL,
 CONSTRAINT [PK_study_report_addendums_archive] PRIMARY KEY CLUSTERED 
(
	[study_hdr_id] ASC,
	[report_id] ASC,
	[addendum_srl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[study_report_addendums_archive] ADD  CONSTRAINT [DF_study_report_addendums_archive_pacs_wb]  DEFAULT ('N') FOR [pacs_wb]
GO
