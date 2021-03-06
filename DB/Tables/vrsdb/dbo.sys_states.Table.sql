USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_states]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_states]
GO
/****** Object:  Table [dbo].[sys_states]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_states](
	[id] [int] NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[country_id] [int] NOT NULL,
 CONSTRAINT [PK_sys_states] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
