USE [vrsarchive19db]
GO
ALTER TABLE [dbo].[study_hdr_dictated_reports_archive] DROP CONSTRAINT [DF_study_hdr_dictated_reports_archive_rating_reason_id]
GO
ALTER TABLE [dbo].[study_hdr_dictated_reports_archive] DROP CONSTRAINT [DF_study_hdr_dictated_reports_archive_transcribed_by]
GO
ALTER TABLE [dbo].[study_hdr_dictated_reports_archive] DROP CONSTRAINT [DF_study_hdr_dictated_reports_archive_rating]
GO
/****** Object:  Table [dbo].[study_hdr_dictated_reports_archive]    Script Date: 21-09-2021 17:14:12 ******/
DROP TABLE [dbo].[study_hdr_dictated_reports_archive]
GO
/****** Object:  Table [dbo].[study_hdr_dictated_reports_archive]    Script Date: 21-09-2021 17:14:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[study_hdr_dictated_reports_archive](
	[report_id] [uniqueidentifier] NOT NULL,
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[report_text] [ntext] NULL,
	[report_text_html] [ntext] NULL,
	[rating] [nchar](1) NULL,
	[pacs_wb] [nchar](1) NULL,
	[transcribed_by] [uniqueidentifier] NULL,
	[date_transcribed] [datetime] NULL,
	[trans_report_text] [ntext] NULL,
	[trans_report_text_html] [ntext] NULL,
	[translate_report_text] [ntext] NULL,
	[translate_report_text_html] [ntext] NULL,
	[disclaimer_reason_id] [int] NULL,
	[rating_reason_id] [uniqueidentifier] NULL,
	[disclaimer_text] [ntext] NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_study_hdr_dictated_reports_archive] PRIMARY KEY CLUSTERED 
(
	[report_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[study_hdr_dictated_reports_archive] ADD  CONSTRAINT [DF_study_hdr_dictated_reports_archive_rating]  DEFAULT ('N') FOR [rating]
GO
ALTER TABLE [dbo].[study_hdr_dictated_reports_archive] ADD  CONSTRAINT [DF_study_hdr_dictated_reports_archive_transcribed_by]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [transcribed_by]
GO
ALTER TABLE [dbo].[study_hdr_dictated_reports_archive] ADD  CONSTRAINT [DF_study_hdr_dictated_reports_archive_rating_reason_id]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [rating_reason_id]
GO
