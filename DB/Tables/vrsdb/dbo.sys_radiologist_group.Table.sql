USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_radiologist_group]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_radiologist_group]
GO
/****** Object:  Table [dbo].[sys_radiologist_group]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_radiologist_group](
	[id] [int] NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[gl_code] [nvarchar](5) NULL,
	[group_color] [nvarchar](10) NULL,
	[display_order] [int] NULL CONSTRAINT [DF_sys_radiologist_group_display_order]  DEFAULT ((0)),
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL
) ON [PRIMARY]

GO
