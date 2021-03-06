USE [vrsdb]
GO
/****** Object:  Table [dbo].[ar_promotion_institution]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_promotion_institution]
GO
/****** Object:  Table [dbo].[ar_promotion_institution]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_promotion_institution](
	[id] [uniqueidentifier] NOT NULL,
	[hdr_id] [uniqueidentifier] NOT NULL,
	[line_no] [int] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[modality_id] [int] NOT NULL,
	[free_credits] [int] NULL CONSTRAINT [DF_ar_promotion_institution_free_credits]  DEFAULT ((0)),
	[discount_percent] [decimal](5, 2) NULL CONSTRAINT [DF_ar_promotion_institution_discount_percent]  DEFAULT ((0)),
	[updated_by] [uniqueidentifier] NOT NULL,
	[date_updated] [datetime] NOT NULL
) ON [PRIMARY]

GO
