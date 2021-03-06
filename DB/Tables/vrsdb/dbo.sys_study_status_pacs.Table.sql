USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_study_status_pacs]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_study_status_pacs]
GO
/****** Object:  Table [dbo].[sys_study_status_pacs]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_study_status_pacs](
	[status_id] [int] NOT NULL,
	[status_desc] [nvarchar](50) NOT NULL,
	[vrs_status_id] [int] NOT NULL,
	[vrs_status_desc] [nvarchar](30) NULL,
	[vrs_study_queue] [nvarchar](50) NULL,
	[menu_id] [int] NULL
) ON [PRIMARY]

GO
