USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_archive_db]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_archive_db]
GO
/****** Object:  Table [dbo].[sys_archive_db]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_archive_db](
	[db_year] [int] NOT NULL,
	[db_name] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO
