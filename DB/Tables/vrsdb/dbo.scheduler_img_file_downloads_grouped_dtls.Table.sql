USE [vrsdb]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_grouped_dtls]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[scheduler_img_file_downloads_grouped_dtls]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_grouped_dtls]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_img_file_downloads_grouped_dtls](
	[id] [uniqueidentifier] NOT NULL,
	[ungrouped_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[file_name] [nvarchar](250) NOT NULL,
	[dcm_file_name] [nvarchar](250) NULL,
	[dicomised] [nchar](1) NULL CONSTRAINT [DF_scheduler_img_file_downloads_grouped_dtls_dicomised]  DEFAULT (N'N'),
	[series_instance_uid] [nvarchar](100) NULL,
	[series_no] [nvarchar](100) NULL,
	[sent_to_pacs] [nchar](1) NULL CONSTRAINT [DF_scheduler_img_file_downloads_grouped_dtls_sent_to_pacs]  DEFAULT (N'N'),
	[date_sent] [datetime] NULL,
	[import_session_id] [nvarchar](30) NULL,
 CONSTRAINT [PK_scheduler_img_file_downloads_grouped_dtls] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[study_uid] ASC,
	[file_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
