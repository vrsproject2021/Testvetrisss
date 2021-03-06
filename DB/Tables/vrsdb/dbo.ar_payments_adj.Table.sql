USE [vrsdb]
GO
/****** Object:  Table [dbo].[ar_payments_adj]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_payments_adj]
GO
/****** Object:  Table [dbo].[ar_payments_adj]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_payments_adj](
	[id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[ar_payments_id] [uniqueidentifier] NOT NULL,
	[invoice_header_id] [uniqueidentifier] NOT NULL,
	[adj_amount] [money] NULL CONSTRAINT [DF_ar_payments_adj_adj_amount]  DEFAULT ((0)),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[invoice_no] [nvarchar](50) NULL,
	[invoice_date] [datetime] NULL,
	[ar_refunds_id] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ar_payments_adj] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
