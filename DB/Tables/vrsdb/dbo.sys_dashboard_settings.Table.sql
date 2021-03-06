USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_dashboard_settings]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_dashboard_settings]
GO
/****** Object:  Table [dbo].[sys_dashboard_settings]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sys_dashboard_settings](
	[id] [int] NOT NULL,
	[parent_id] [int] NULL CONSTRAINT [DF_sys_dashboard_settings_parent_id]  DEFAULT ((0)),
	[menu_desc] [nvarchar](150) NULL,
	[nav_url] [nvarchar](500) NULL,
	[icon] [nvarchar](150) NULL,
	[display_index] [int] NOT NULL CONSTRAINT [DF_sys_dashboard_settings_display_index]  DEFAULT ((0)),
	[is_enabled] [varchar](1) NULL,
	[is_default] [varchar](1) NULL,
	[refresh_time] [int] NULL,
	[is_refresh_button] [varchar](1) NULL CONSTRAINT [DF_sys_dashboard_settings_is_refresh_button]  DEFAULT ('N'),
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[title] [nvarchar](250) NULL,
 CONSTRAINT [PK_sys_dashboard_settings] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
