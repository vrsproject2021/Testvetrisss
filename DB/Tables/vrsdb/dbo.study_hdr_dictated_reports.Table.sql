USE [vrsdb]
GO
/****** Object:  Table [dbo].[study_hdr_dictated_reports]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[study_hdr_dictated_reports]
GO
/****** Object:  Table [dbo].[study_hdr_dictated_reports]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[study_hdr_dictated_reports](
	[report_id] [uniqueidentifier] NOT NULL,
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[report_text] [ntext] NULL,
	[synch_from_pacs] [nchar](1) NULL CONSTRAINT [DF_study_hdr_dictated_reports_synch_from_pacs]  DEFAULT ('N'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[report_text_html] [ntext] NULL,
	[rating] [nchar](1) NULL,
	[pacs_wb] [nchar](1) NULL CONSTRAINT [DF_study_hdr_dictated_reports_pacs_wb]  DEFAULT ('N'),
	[transcribed_by] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_dictated_reports_transcribed_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[date_transcribed] [datetime] NULL,
	[trans_report_text] [ntext] NULL,
	[trans_report_text_html] [ntext] NULL,
	[translate_report_text] [ntext] NULL,
	[translate_report_text_html] [ntext] NULL,
	[disclaimer_reason_id] [int] NULL,
	[rating_reason_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_dictated_reports_rating_reason_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[disclaimer_text] [ntext] NULL,
 CONSTRAINT [PK_study_hdr_dictated_reports] PRIMARY KEY CLUSTERED 
(
	[report_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
