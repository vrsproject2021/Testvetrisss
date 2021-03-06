USE [vrslogdb]
GO
/****** Object:  Table [dbo].[sys_study_user_activity_trail]    Script Date: 20-08-2021 20:49:16 ******/
DROP TABLE [dbo].[sys_study_user_activity_trail]
GO
/****** Object:  Table [dbo].[sys_study_user_activity_trail]    Script Date: 20-08-2021 20:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_study_user_activity_trail](
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[menu_id] [int] NOT NULL,
	[activity_text] [nvarchar](max) NULL,
	[activity_by] [uniqueidentifier] NOT NULL,
	[activity_datetime] [datetime] NOT NULL,
	[session_id] [uniqueidentifier] NULL CONSTRAINT [DF_sys_study_user_activity_trail_session_id]  DEFAULT ('00000000-0000-0000-0000-000000000000')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
