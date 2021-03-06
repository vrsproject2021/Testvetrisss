USE [vrsdb]
GO
/****** Object:  Table [dbo].[billing_account_vault]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[billing_account_vault]
GO
/****** Object:  Table [dbo].[billing_account_vault]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[billing_account_vault](
	[id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[vault_id] [uniqueidentifier] NOT NULL,
	[vault_type] [nvarchar](5) NOT NULL,
	[vault_card] [nvarchar](19) NULL,
	[vault_card_type] [nvarchar](10) NULL,
	[vault_exp] [nvarchar](5) NULL,
	[vault_name] [nvarchar](50) NULL,
	[vault_account] [nvarchar](30) NULL,
	[vault_aba] [nvarchar](30) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[holder_name] [nvarchar](100) NULL,
 CONSTRAINT [PK_billing_account_vault] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
