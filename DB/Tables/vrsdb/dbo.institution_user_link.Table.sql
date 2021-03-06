USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_user_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_user_link]
GO
/****** Object:  Table [dbo].[institution_user_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_user_link](
	[user_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[user_login_id] [nvarchar](50) NULL,
	[user_pwd] [nvarchar](200) NULL,
	[user_pacs_user_id] [nvarchar](20) NULL,
	[user_pacs_password] [nvarchar](200) NULL,
	[user_email] [nvarchar](50) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[granted_rights_pacs] [nvarchar](30) NULL,
	[updated_in_pacs] [nchar](1) NULL CONSTRAINT [DF_institution_user_link_updated_in_pacs]  DEFAULT ('N'),
	[date_updated_in_pacs] [datetime] NULL,
	[user_contact_no] [nvarchar](20) NULL,
 CONSTRAINT [PK_institution_user_link] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[institution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
