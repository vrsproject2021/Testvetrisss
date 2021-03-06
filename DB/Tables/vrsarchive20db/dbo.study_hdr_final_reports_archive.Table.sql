USE [vrsarchive20db]
GO
/****** Object:  Table [dbo].[study_hdr_final_reports_archive]    Script Date: 21-09-2021 17:15:31 ******/
DROP TABLE [dbo].[study_hdr_final_reports_archive]
GO
/****** Object:  Table [dbo].[study_hdr_final_reports_archive]    Script Date: 21-09-2021 17:15:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[study_hdr_final_reports_archive](
	[report_id] [uniqueidentifier] NOT NULL,
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[report_text] [ntext] NULL,
	[report_file] [varbinary](max) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[email_updated] [nchar](1) NULL CONSTRAINT [DF_study_hdr_final_reports_archive_email_updated]  DEFAULT ('N'),
	[sms_updated] [nchar](1) NULL CONSTRAINT [DF_study_hdr_final_reports_archive_sms_updated]  DEFAULT ('N'),
	[report_text_html] [ntext] NULL,
	[pacs_wb] [nchar](1) NULL,
	[disclaimer_reason_id] [int] NULL,
	[rating_reason_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_final_reports_archive_rating_reason_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[disclaimer_text] [ntext] NULL,
 CONSTRAINT [PK_study_hdr_final_reports_archive] PRIMARY KEY CLUSTERED 
(
	[report_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
