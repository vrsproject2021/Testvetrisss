USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_general_settings_group]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_general_settings_group]
GO
/****** Object:  Table [dbo].[sys_general_settings_group]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_general_settings_group](
	[group_id] [int] NOT NULL,
	[group_name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_sys_general_settings_group] PRIMARY KEY CLUSTERED 
(
	[group_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
