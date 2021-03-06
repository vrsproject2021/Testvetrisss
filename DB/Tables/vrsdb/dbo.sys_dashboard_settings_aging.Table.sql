USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_dashboard_settings_aging]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_dashboard_settings_aging]
GO
/****** Object:  Table [dbo].[sys_dashboard_settings_aging]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_dashboard_settings_aging](
	[id] [int] NOT NULL,
	[dashboard_menu_id] [int] NULL,
	[key] [nvarchar](100) NULL,
	[slot_count] [int] NULL,
	[slot_1] [int] NULL,
	[slot_2] [int] NULL,
	[slot_3] [int] NULL,
	[slot_4] [int] NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_sys_dashboard_settings_aging] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
