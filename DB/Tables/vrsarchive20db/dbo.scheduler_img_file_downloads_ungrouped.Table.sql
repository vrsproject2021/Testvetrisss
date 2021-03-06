USE [vrsarchive20db]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_ungrouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_is_manual]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_ungrouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_is_stored]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_ungrouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_grouped_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_ungrouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_grouped]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_ungrouped]    Script Date: 21-09-2021 17:15:31 ******/
DROP TABLE [dbo].[scheduler_img_file_downloads_ungrouped]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_ungrouped]    Script Date: 21-09-2021 17:15:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_img_file_downloads_ungrouped](
	[id] [uniqueidentifier] NOT NULL,
	[file_name] [nvarchar](250) NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[institution_code] [nvarchar](5) NOT NULL,
	[institution_name] [nvarchar](100) NOT NULL,
	[date_downloaded] [datetime] NULL,
	[grouped] [nchar](1) NULL,
	[date_grouped] [datetime] NULL,
	[grouped_id] [uniqueidentifier] NULL,
	[import_session_id] [nvarchar](30) NULL,
	[is_stored] [nchar](1) NULL,
	[is_manual] [nchar](1) NULL,
 CONSTRAINT [PK_scheduler_img_file_downloads_ungrouped] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[file_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_ungrouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_grouped]  DEFAULT (N'N') FOR [grouped]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_ungrouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_grouped_id]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [grouped_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_ungrouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_is_stored]  DEFAULT ('N') FOR [is_stored]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_ungrouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_is_manual]  DEFAULT ('N') FOR [is_manual]
GO
