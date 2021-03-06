USE [vrsdb]
GO
/****** Object:  Table [dbo].[message_role_links]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[message_role_links]
GO
/****** Object:  Table [dbo].[message_role_links]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[message_role_links](
	[user_role_id] [int] NOT NULL,
	[linked_user_role_id] [int] NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_message_role_links] PRIMARY KEY CLUSTERED 
(
	[user_role_id] ASC,
	[linked_user_role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
