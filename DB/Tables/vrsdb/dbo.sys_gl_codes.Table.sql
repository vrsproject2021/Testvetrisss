USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_gl_codes]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_gl_codes]
GO
/****** Object:  Table [dbo].[sys_gl_codes]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_gl_codes](
	[gl_code] [nvarchar](5) NOT NULL,
	[gl_desc] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_sys_gl_codes] PRIMARY KEY CLUSTERED 
(
	[gl_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
