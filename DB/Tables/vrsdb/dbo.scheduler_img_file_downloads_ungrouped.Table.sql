USE [vrsdb]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_ungrouped]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[scheduler_img_file_downloads_ungrouped]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_ungrouped]    Script Date: 21-09-2021 17:12:53 ******/
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
	[grouped] [nchar](1) NULL CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_grouped]  DEFAULT (N'N'),
	[date_grouped] [datetime] NULL,
	[grouped_id] [uniqueidentifier] NULL CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_grouped_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[import_session_id] [nvarchar](30) NULL,
	[is_stored] [nchar](1) NULL CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_is_stored]  DEFAULT ('N'),
	[is_manual] [nchar](1) NULL CONSTRAINT [DF_scheduler_img_file_downloads_ungrouped_is_manual]  DEFAULT ('N'),
 CONSTRAINT [PK_scheduler_img_file_downloads_ungrouped] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[file_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
