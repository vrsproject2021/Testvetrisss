USE [vrsdb]
GO
ALTER TABLE [dbo].[technicians] DROP CONSTRAINT [DF_technicians_notification_pref]
GO
ALTER TABLE [dbo].[technicians] DROP CONSTRAINT [DF__technicia__updat__47C69FAC]
GO
ALTER TABLE [dbo].[technicians] DROP CONSTRAINT [DF_technicians_is_active]
GO
ALTER TABLE [dbo].[technicians] DROP CONSTRAINT [DF_technicians_default_fee]
GO
/****** Object:  Table [dbo].[technicians]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[technicians]
GO
/****** Object:  Table [dbo].[technicians]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[technicians](
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
	[default_fee] [money] NULL,
	[is_active] [nchar](1) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[granted_rights_pacs] [nvarchar](30) NULL,
	[updated_in_pacs] [nvarchar](1) NULL,
	[date_updated_in_pacs] [datetime] NULL,
	[notification_pref] [char](1) NULL,
 CONSTRAINT [PK_technicians] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[technicians] ADD  CONSTRAINT [DF_technicians_default_fee]  DEFAULT ((0)) FOR [default_fee]
GO
ALTER TABLE [dbo].[technicians] ADD  CONSTRAINT [DF_technicians_is_active]  DEFAULT (N'Y') FOR [is_active]
GO
ALTER TABLE [dbo].[technicians] ADD  DEFAULT ('N') FOR [updated_in_pacs]
GO
ALTER TABLE [dbo].[technicians] ADD  CONSTRAINT [DF_technicians_notification_pref]  DEFAULT ('B') FOR [notification_pref]
GO
