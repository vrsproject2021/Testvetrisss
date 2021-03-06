USE [vrsdb]
GO
/****** Object:  Table [dbo].[invoicing_charges_discount]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[invoicing_charges_discount]
GO
/****** Object:  Table [dbo].[invoicing_charges_discount]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoicing_charges_discount](
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[discount_perc] [decimal](5, 2) NULL CONSTRAINT [DF_invoicing_charges_discount_discount_perc]  DEFAULT ((0)),
	[updated_by] [uniqueidentifier] NOT NULL,
	[date_updated] [datetime] NOT NULL,
 CONSTRAINT [PK_invoicing_charges_discount] PRIMARY KEY CLUSTERED 
(
	[billing_account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
