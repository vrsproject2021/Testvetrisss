USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_physician_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_physician_link]
GO
/****** Object:  Table [dbo].[institution_physician_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_physician_link](
	[physician_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[physician_name] [nvarchar](200) NULL,
	[physician_fname] [nvarchar](80) NULL,
	[physician_lname] [nvarchar](80) NULL,
	[physician_credentials] [nvarchar](30) NULL,
	[physician_login_email] [nvarchar](50) NULL,
	[physician_email] [nvarchar](500) NULL,
	[physician_mobile] [nvarchar](500) NULL,
	[physician_user_id] [uniqueidentifier] NULL CONSTRAINT [DF_institution_physician_link_physician_user_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[physician_pacs_user_id] [nvarchar](10) NULL,
	[physician_pacs_password] [nvarchar](200) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[billing_account_id] [uniqueidentifier] NULL CONSTRAINT [DF_institution_physician_link_billing_account_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
 CONSTRAINT [PK_institution_physician_link] PRIMARY KEY CLUSTERED 
(
	[physician_id] ASC,
	[institution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
