USE [vrsarchive19db]
GO
/****** Object:  Table [dbo].[scheduler_file_downloads_dtls]    Script Date: 21-09-2021 17:14:12 ******/
DROP TABLE [dbo].[scheduler_file_downloads_dtls]
GO
/****** Object:  Table [dbo].[scheduler_file_downloads_dtls]    Script Date: 21-09-2021 17:14:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_file_downloads_dtls](
	[id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[file_name] [nvarchar](250) NOT NULL,
	[sent_to_pacs] [nchar](1) NULL CONSTRAINT [DF_scheduler_file_downloads_dtls_sent_to_pacs]  DEFAULT (N'N'),
	[date_sent] [datetime] NULL,
	[import_session_id] [nvarchar](30) NULL,
	[series_uid] [nvarchar](100) NULL,
	[instance_no] [nvarchar](100) NULL,
 CONSTRAINT [PK_scheduler_file_downloads_dtls] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[study_uid] ASC,
	[file_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
