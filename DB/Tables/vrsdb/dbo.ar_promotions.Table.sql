USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_promotions', @level2type=N'COLUMN',@level2name=N'promotion_type'

GO
/****** Object:  Table [dbo].[ar_promotions]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_promotions]
GO
/****** Object:  Table [dbo].[ar_promotions]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_promotions](
	[id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[promotion_type] [nchar](1) NOT NULL,
	[valid_from] [datetime] NULL,
	[valid_till] [datetime] NOT NULL,
	[is_active] [nchar](1) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[reason_id] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ar_promotions] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[billing_account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[D]iscount,[F]ree Credits' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_promotions', @level2type=N'COLUMN',@level2name=N'promotion_type'
GO
