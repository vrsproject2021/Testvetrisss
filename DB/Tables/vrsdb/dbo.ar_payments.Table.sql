USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_payments', @level2type=N'COLUMN',@level2name=N'payment_tool'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_payments', @level2type=N'COLUMN',@level2name=N'processing_status'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_payments', @level2type=N'COLUMN',@level2name=N'payment_mode'

GO
/****** Object:  Table [dbo].[ar_payments]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_payments]
GO
/****** Object:  Table [dbo].[ar_payments]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_payments](
	[id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[payment_mode] [nvarchar](1) NOT NULL,
	[payref_no] [nvarchar](50) NOT NULL,
	[payref_date] [datetime] NOT NULL,
	[processing_ref_no] [nvarchar](100) NOT NULL,
	[processing_ref_date] [datetime] NOT NULL,
	[processing_pg_name] [nvarchar](50) NULL,
	[processing_status] [nchar](1) NOT NULL,
	[payment_amount] [money] NULL CONSTRAINT [DF_ar_payments_payment_amount]  DEFAULT ((0)),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[remarks] [nvarchar](150) NULL,
	[payment_tool] [nchar](1) NULL,
	[auth_code] [nvarchar](50) NULL,
	[cvv_response] [nvarchar](50) NULL,
	[avs_response] [nvarchar](50) NULL,
	[payment_tool_holder_name] [nvarchar](100) NULL,
	[post_to_qb] [nchar](1) NULL CONSTRAINT [DF_ar_payments_post_to_qb]  DEFAULT ('N'),
	[qb_posting_id] [nvarchar](20) NULL,
	[qb_posted_on] [datetime] NULL,
 CONSTRAINT [PK_ar_payments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[1]On-Line,[2]Off-Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_payments', @level2type=N'COLUMN',@level2name=N'payment_mode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[1]Success,[0]Failed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_payments', @level2type=N'COLUMN',@level2name=N'processing_status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[C]ard,[A]Check' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ar_payments', @level2type=N'COLUMN',@level2name=N'payment_tool'
GO
