USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_refunds', @level2type=N'COLUMN',@level2name=N'processing_status'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_refunds', @level2type=N'COLUMN',@level2name=N'refund_mode'

GO
/****** Object:  Table [dbo].[ar_refunds]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_refunds]
GO
/****** Object:  Table [dbo].[ar_refunds]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_refunds](
	[id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[ar_payments_id] [uniqueidentifier] NOT NULL,
	[refund_mode] [nvarchar](1) NOT NULL,
	[refundref_no] [nvarchar](50) NOT NULL,
	[refundref_date] [datetime] NOT NULL,
	[processing_ref_no] [nvarchar](100) NOT NULL,
	[processing_ref_date] [datetime] NOT NULL,
	[processing_pg_name] [nvarchar](50) NULL,
	[processing_status] [nchar](1) NOT NULL,
	[remarks] [nvarchar](150) NULL,
	[refund_amount] [money] NULL CONSTRAINT [DF_ar_refunds_refund_amount]  DEFAULT ((0)),
	[post_to_qb] [nchar](1) NULL CONSTRAINT [DF_ar_refunds_post_to_qb]  DEFAULT ('N'),
	[qb_posting_id] [nvarchar](20) NULL,
	[qb_posted_on] [datetime] NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_ar_refunds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[1]On-Line,[2]Off-Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_refunds', @level2type=N'COLUMN',@level2name=N'refund_mode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[1]Success,[0]Failed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_refunds', @level2type=N'COLUMN',@level2name=N'processing_status'
GO
