USE [vrsdb]
GO
/****** Object:  Table [dbo].[billing_account]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[billing_account]
GO
/****** Object:  Table [dbo].[billing_account]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[billing_account](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](5) NULL,
	[name] [nvarchar](100) NOT NULL,
	[address_1] [nvarchar](100) NULL,
	[address_2] [nvarchar](100) NULL,
	[city] [nvarchar](100) NULL,
	[state_id] [int] NULL CONSTRAINT [DF_billing_account_state_id]  DEFAULT ((0)),
	[country_id] [int] NULL CONSTRAINT [DF_billing_account_country_id]  DEFAULT ((0)),
	[zip] [nvarchar](20) NULL,
	[email_id] [nvarchar](100) NULL,
	[phone_no] [nvarchar](30) NULL,
	[fax_no] [nvarchar](20) NULL,
	[contact_person_name] [nvarchar](100) NULL,
	[contact_person_mobile] [nvarchar](20) NULL,
	[contact_person_email_id] [nvarchar](50) NULL,
	[salesperson_id] [uniqueidentifier] NULL,
	[commission_1st_yr] [decimal](5, 2) NULL DEFAULT ((0.00)),
	[commission_2nd_yr] [decimal](5, 2) NULL DEFAULT ((0.00)),
	[login_user_id] [uniqueidentifier] NULL,
	[login_id] [nvarchar](50) NOT NULL,
	[login_pwd] [nvarchar](200) NOT NULL,
	[user_email_id] [nvarchar](100) NULL,
	[user_mobile_no] [nvarchar](20) NULL,
	[notification_pref] [nchar](1) NULL,
	[accountant_name] [nvarchar](250) NULL,
	[discount_per] [decimal](5, 2) NULL,
	[discount_updated_by] [uniqueidentifier] NULL,
	[discount_updated_on] [datetime] NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_billing_account_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[is_new] [nchar](1) NULL CONSTRAINT [DF_billing_account_is_new]  DEFAULT ('Y'),
	[debtor_id] [nvarchar](20) NULL,
	[update_qb] [nchar](1) NULL CONSTRAINT [DF_billing_account_update_qb]  DEFAULT ('N'),
	[update_qb_on] [datetime] NULL,
	[qb_name] [nvarchar](100) NULL,
 CONSTRAINT [PK_billing_account] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
