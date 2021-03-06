USE [vrslogdb]
GO
/****** Object:  Table [dbo].[sys_user_activity_log]    Script Date: 20-08-2021 20:49:16 ******/
DROP TABLE [dbo].[sys_user_activity_log]
GO
/****** Object:  Table [dbo].[sys_user_activity_log]    Script Date: 20-08-2021 20:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user_activity_log](
	[user_id] [uniqueidentifier] NOT NULL,
	[menu_id] [int] NULL,
	[activity_text] [nvarchar](max) NULL,
	[activity_datetime] [datetime] NOT NULL,
	[session_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_session_id]  DEFAULT ('00000000-0000-0000-0000-000000000000')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
