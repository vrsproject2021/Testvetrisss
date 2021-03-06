USE [vrsdb]
GO
/****** Object:  Table [dbo].[user_role_menu_rights]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[user_role_menu_rights]
GO
/****** Object:  Table [dbo].[user_role_menu_rights]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_role_menu_rights](
	[user_role_id] [int] NOT NULL,
	[menu_id] [int] NOT NULL,
	[update_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_user_role_menu_rights] PRIMARY KEY CLUSTERED 
(
	[user_role_id] ASC,
	[menu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
