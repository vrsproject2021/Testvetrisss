USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_version]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_version]
GO
/****** Object:  Table [dbo].[sys_version]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_version](
	[version_no] [nvarchar](50) NOT NULL,
	[last_updated] [datetime] NULL,
 CONSTRAINT [PK_sys_version] PRIMARY KEY CLUSTERED 
(
	[version_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
