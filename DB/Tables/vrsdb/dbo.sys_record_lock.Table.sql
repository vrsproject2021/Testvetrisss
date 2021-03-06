USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_record_lock]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_record_lock]
GO
/****** Object:  Table [dbo].[sys_record_lock]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_record_lock](
	[record_id] [bigint] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[menu_id] [int] NOT NULL,
	[locked_on] [datetime] NULL,
	[session_id] [uniqueidentifier] NULL CONSTRAINT [DF_sys_record_lock_session_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
 CONSTRAINT [PK_sys_record_lock] PRIMARY KEY CLUSTERED 
(
	[record_id] ASC,
	[menu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
