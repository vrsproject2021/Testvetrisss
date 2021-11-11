USE [vrsarchive20db]
GO
/****** Object:  Table [dbo].[image_files_deleted]    Script Date: 21-09-2021 17:15:31 ******/
DROP TABLE [dbo].[image_files_deleted]
GO
/****** Object:  Table [dbo].[image_files_deleted]    Script Date: 21-09-2021 17:15:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[image_files_deleted](
	[id] [uniqueidentifier] NOT NULL,
	[file_name] [nvarchar](250) NOT NULL,
	[import_session_id] [nvarchar](30) NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[institution_code] [nvarchar](5) NULL,
	[institution_name] [nvarchar](100) NULL,
	[date_downloaded] [datetime] NOT NULL,
	[remarks] [nvarchar](250) NULL,
	[deleted_by] [uniqueidentifier] NOT NULL,
	[date_deleted] [datetime] NOT NULL
) ON [PRIMARY]

GO
