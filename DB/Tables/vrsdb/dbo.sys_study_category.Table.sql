USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_study_category]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_study_category]
GO
/****** Object:  Table [dbo].[sys_study_category]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_study_category](
	[id] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[gl_code] [nvarchar](5) NULL,
	[is_default] [nchar](1) NULL,
 CONSTRAINT [PK_sys_study_category] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
