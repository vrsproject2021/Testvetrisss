USE [vrsdb]
GO
/****** Object:  Table [dbo].[billing_account_modality_fee_schedule]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[billing_account_modality_fee_schedule]
GO
/****** Object:  Table [dbo].[billing_account_modality_fee_schedule]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[billing_account_modality_fee_schedule](
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[rate_id] [uniqueidentifier] NOT NULL,
	[fee_amount] [money] NULL,
	[fee_amount_per_unit] [money] NULL,
	[study_max_amount] [money] NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_[billing_account_modality_fee_schedule] PRIMARY KEY CLUSTERED 
(
	[billing_account_id] ASC,
	[rate_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
