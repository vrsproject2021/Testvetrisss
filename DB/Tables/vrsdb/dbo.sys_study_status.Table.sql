USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_study_status]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_study_status]
GO
/****** Object:  Table [dbo].[sys_study_status]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_study_status](
	[status_id] [int] NOT NULL,
	[status_desc] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO
