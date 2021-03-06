USE [vrsdb]
GO
/****** Object:  Table [dbo].[users]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[users]
GO
/****** Object:  Table [dbo].[users]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[users](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](10) NULL,
	[name] [nvarchar](200) NULL,
	[password] [nvarchar](200) NOT NULL,
	[email_id] [nvarchar](100) NULL,
	[contact_no] [nvarchar](20) NULL,
	[user_role_id] [int] NOT NULL,
	[first_login] [nchar](1) NULL CONSTRAINT [DF_users_first_login]  DEFAULT (N'Y'),
	[pacs_user_id] [nvarchar](50) NULL,
	[pacs_password] [nvarchar](200) NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[update_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_users_is_active]  DEFAULT (N'Y'),
	[is_visible] [nchar](1) NULL CONSTRAINT [DF_users_is_visible]  DEFAULT (N'Y'),
	[login_id] [nvarchar](50) NULL,
	[notification_pref] [char](1) NULL CONSTRAINT [DF_users_notification_pref]  DEFAULT ('B'),
	[allow_manual_submission] [nchar](1) NULL CONSTRAINT [DF_users_allow_manual_submission]  DEFAULT ('N'),
	[allow_dashboard_view] [nchar](1) NULL,
	[theme_pref] [nvarchar](10) NULL CONSTRAINT [DF_users_theme_pref]  DEFAULT ('DEFAULT'),
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
