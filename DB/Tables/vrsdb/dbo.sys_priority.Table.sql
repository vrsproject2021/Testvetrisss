USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_priority]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_priority]
GO
/****** Object:  Table [dbo].[sys_priority]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_priority](
	[priority_id] [int] NOT NULL,
	[priority_desc] [nvarchar](30) NOT NULL,
	[is_active] [nchar](1) NOT NULL,
	[finishing_time_hrs] [int] NULL,
	[transcription_finishing_time_mins] [int] NULL,
	[is_stat] [nchar](1) NULL,
	[final_report_release_time_mins] [int] NULL,
	[short_desc] [nvarchar](20) NULL
) ON [PRIMARY]

GO
