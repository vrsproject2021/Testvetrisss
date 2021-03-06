USE [vrsdb]
GO
/****** Object:  Table [dbo].[user_roles]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[user_roles]
GO
/****** Object:  Table [dbo].[user_roles]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_roles](
	[id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[code] [nvarchar](10) NOT NULL,
	[name] [nvarchar](30) NOT NULL,
	[is_visible] [nchar](1) NULL CONSTRAINT [DF_user_roles_is_visible]  DEFAULT (N'Y'),
	[is_active] [nchar](1) NOT NULL CONSTRAINT [DF_user_roles_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[update_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[sys_defined] [nchar](1) NULL CONSTRAINT [DF_user_roles_sys_defined]  DEFAULT (N'N'),
 CONSTRAINT [PK_user_roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
