USE [vrsdb]
GO
/****** Object:  Table [dbo].[transciptionists]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[transciptionists]
GO
/****** Object:  Table [dbo].[transciptionists]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[transciptionists](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](10) NULL,
	[fname] [nvarchar](80) NULL,
	[lname] [nvarchar](80) NULL,
	[name] [nvarchar](200) NOT NULL,
	[address_1] [nvarchar](100) NULL,
	[address_2] [nvarchar](100) NULL,
	[city] [nvarchar](100) NULL,
	[state_id] [int] NULL,
	[country_id] [int] NULL,
	[zip] [nvarchar](20) NULL,
	[email_id] [nvarchar](50) NOT NULL,
	[phone_no] [nvarchar](30) NULL,
	[mobile_no] [nvarchar](20) NULL,
	[login_user_id] [uniqueidentifier] NULL,
	[login_id] [nvarchar](50) NULL,
	[login_pwd] [nvarchar](200) NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_transciptionists_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[granted_rights_pacs] [nvarchar](30) NULL,
	[updated_in_pacs] [nvarchar](1) NULL DEFAULT ('N'),
	[date_updated_in_pacs] [datetime] NULL,
	[notification_pref] [char](1) NULL CONSTRAINT [DF_transciptionists_notification_pref]  DEFAULT ('B'),
	[notes] [nvarchar](500) NULL,
 CONSTRAINT [PK_transciptionists] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
