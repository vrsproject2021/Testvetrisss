USE [vrsdb]
GO
/****** Object:  Table [dbo].[services]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[services]
GO
/****** Object:  Table [dbo].[services]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[services](
	[id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_services_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[code] [nvarchar](10) NOT NULL,
	[priority_id] [int] NULL CONSTRAINT [DF_services_priority_id]  DEFAULT ((0)),
	[sys_defined] [nchar](1) NULL CONSTRAINT [DF_services_sys_defined]  DEFAULT ('N'),
	[gl_code] [nvarchar](5) NULL,
	[gl_code_after_hrs] [nvarchar](5) NULL,
 CONSTRAINT [PK_services] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
