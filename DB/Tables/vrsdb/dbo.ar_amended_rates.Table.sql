USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_amended_rates', @level2type=N'COLUMN',@level2name=N'head_type'

GO
/****** Object:  Table [dbo].[ar_amended_rates]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_amended_rates]
GO
/****** Object:  Table [dbo].[ar_amended_rates]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_amended_rates](
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[rate] [money] NOT NULL,
	[head_type] [nchar](1) NOT NULL,
	[head_id] [int] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[category_id] [int] NULL,
	[addon_rate_per_unit] [money] NULL CONSTRAINT [DF_ar_amended_rates_addon_rate_per_unit]  DEFAULT ((0)),
	[study_max_amount] [money] NULL CONSTRAINT [DF_ar_amended_rates_study_max_amount]  DEFAULT ((0)),
	[rate_after_hrs] [money] NULL CONSTRAINT [DF_ar_amended_rates_rate_after_hrs]  DEFAULT ((0)),
 CONSTRAINT [PK_ar_amended_rates] PRIMARY KEY CLUSTERED 
(
	[billing_cycle_id] ASC,
	[billing_account_id] ASC,
	[institution_id] ASC,
	[study_hdr_id] ASC,
	[study_uid] ASC,
	[head_type] ASC,
	[head_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[M]odality,[S]ervice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_amended_rates', @level2type=N'COLUMN',@level2name=N'head_type'
GO
