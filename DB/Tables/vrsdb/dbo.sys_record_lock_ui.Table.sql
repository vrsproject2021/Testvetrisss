USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_record_lock_ui]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_record_lock_ui]
GO
/****** Object:  Table [dbo].[sys_record_lock_ui]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_record_lock_ui](
	[record_id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[menu_id] [int] NOT NULL,
	[locked_on] [datetime] NULL,
	[session_id] [uniqueidentifier] NULL CONSTRAINT [DF_sys_record_lock_ui_session_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[addl_record_id_ui] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_sys_record_lock_ui] PRIMARY KEY CLUSTERED 
(
	[menu_id] ASC,
	[record_id] ASC,
	[addl_record_id_ui] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
