USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_user_activity]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_user_activity]
GO
/****** Object:  Table [dbo].[sys_user_activity]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_user_activity](
	[activity_id] [int] NOT NULL,
	[activity_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_sys_user_activity] PRIMARY KEY CLUSTERED 
(
	[activity_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
