USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'nav_method'

GO
/****** Object:  Table [dbo].[sys_menu]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_menu]
GO
/****** Object:  Table [dbo].[sys_menu]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_menu](
	[menu_id] [int] NOT NULL,
	[menu_desc] [nvarchar](50) NOT NULL,
	[display_index] [int] NULL,
	[parent_id] [int] NOT NULL,
	[is_dropdown] [nchar](1) NULL CONSTRAINT [DF_sys_menu_is_dropdown]  DEFAULT (N'Y'),
	[is_enabled] [nchar](1) NULL CONSTRAINT [DF_sys_menu_is_enabled]  DEFAULT (N'Y'),
	[nav_url] [nvarchar](100) NOT NULL,
	[is_browser] [nchar](1) NULL CONSTRAINT [DF_sys_menu_is_browser]  DEFAULT (N'Y'),
	[menu_level] [int] NULL,
	[menu_icon] [nvarchar](30) NULL,
	[show_rec_count] [nchar](1) NULL CONSTRAINT [DF_sys_menu_show_rec_count]  DEFAULT (N'N'),
	[nav_method] [nvarchar](5) NULL,
 CONSTRAINT [PK_sys_menu] PRIMARY KEY CLUSTERED 
(
	[menu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[NT]New Tab,[WA]Within Application,[PA]Popup Application' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'nav_method'
GO
