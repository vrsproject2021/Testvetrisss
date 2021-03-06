USE [vrsdb]
GO
/****** Object:  Table [dbo].[ar_opening_balance]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_opening_balance]
GO
/****** Object:  Table [dbo].[ar_opening_balance]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_opening_balance](
	[id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[opbal_date] [date] NOT NULL,
	[invoice_no] [nvarchar](30) NOT NULL,
	[opbal_amount] [money] NULL CONSTRAINT [DF_ar_opening_balance_opbal_amount]  DEFAULT ((0)),
	[isadjusted] [bit] NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_ar_opening_balance] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
