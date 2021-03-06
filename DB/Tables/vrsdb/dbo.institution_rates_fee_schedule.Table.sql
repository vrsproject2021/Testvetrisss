USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_rates_fee_schedule]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_rates_fee_schedule]
GO
/****** Object:  Table [dbo].[institution_rates_fee_schedule]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_rates_fee_schedule](
	[institution_id] [uniqueidentifier] NOT NULL,
	[rate_id] [uniqueidentifier] NOT NULL,
	[fee_amount] [money] NULL,
	[discount_per] [decimal](5, 2) NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_institution_rates_fee_schedule] PRIMARY KEY CLUSTERED 
(
	[institution_id] ASC,
	[rate_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
