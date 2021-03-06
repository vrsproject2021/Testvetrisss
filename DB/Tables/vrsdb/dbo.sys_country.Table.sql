USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_country]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_country]
GO
/****** Object:  Table [dbo].[sys_country]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_country](
	[id] [int] NOT NULL,
	[code] [nvarchar](5) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[is_default] [nchar](1) NOT NULL CONSTRAINT [DF_sys_country_is_default]  DEFAULT (N'N'),
 CONSTRAINT [PK_sys_country] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
