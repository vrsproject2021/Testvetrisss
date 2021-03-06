USE [vrsdb]
GO
ALTER TABLE [dbo].[ar_promotion_modality] DROP CONSTRAINT [DF_ar_promotion_modality_free_credits]
GO
/****** Object:  Table [dbo].[ar_promotion_modality]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_promotion_modality]
GO
/****** Object:  Table [dbo].[ar_promotion_modality]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_promotion_modality](
	[id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[modality_id] [int] NOT NULL,
	[free_credits] [int] NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ar_promotion_modality] ADD  CONSTRAINT [DF_ar_promotion_modality_free_credits]  DEFAULT ((0)) FOR [free_credits]
GO
