USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_salesperson_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_salesperson_link]
GO
/****** Object:  Table [dbo].[institution_salesperson_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_salesperson_link](
	[salesperson_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[salesperson_name] [nvarchar](200) NULL,
	[salesperson_fname] [nvarchar](80) NULL,
	[salesperson_lname] [nvarchar](80) NULL,
	[salesperson_credentials] [nvarchar](30) NULL,
	[salesperson_login_email] [nvarchar](50) NULL,
	[salesperson_email] [nvarchar](50) NULL,
	[salesperson_mobile] [nvarchar](50) NULL,
	[salesperson_user_id] [uniqueidentifier] NULL CONSTRAINT [DF_institution_salesperson_link_salesperson_user_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[salesperson_pacs_user_id] [nvarchar](20) NULL,
	[salesperson_pacs_password] [nvarchar](200) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[commission_1st_yr] [decimal](5, 2) NULL DEFAULT ((0.00)),
	[commission_2nd_yr] [decimal](5, 2) NULL DEFAULT ((0.00)),
	[billing_account_id] [uniqueidentifier] NULL CONSTRAINT [DF_institution_salesperson_link_billing_account_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
 CONSTRAINT [PK_institution_salesperson_link] PRIMARY KEY CLUSTERED 
(
	[salesperson_id] ASC,
	[institution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
