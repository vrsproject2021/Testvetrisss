USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_user_lock]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_user_lock]
GO
/****** Object:  Table [dbo].[sys_user_lock]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user_lock](
	[user_id] [uniqueidentifier] NOT NULL,
	[session_id] [uniqueidentifier] NOT NULL,
	[last_login] [datetime] NOT NULL,
 CONSTRAINT [PK_sys_user_lock] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[session_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
