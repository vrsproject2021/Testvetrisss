USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'invoice_institution_dtls', @level2type=N'COLUMN',@level2name=N'disc_type_applied'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'invoice_institution_dtls', @level2type=N'COLUMN',@level2name=N'approved'

GO
/****** Object:  Table [dbo].[invoice_institution_dtls]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[invoice_institution_dtls]
GO
/****** Object:  Table [dbo].[invoice_institution_dtls]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoice_institution_dtls](
	[id] [uniqueidentifier] NOT NULL,
	[hdr_id] [uniqueidentifier] NOT NULL,
	[institution_hdr_id] [uniqueidentifier] NOT NULL,
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[study_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[modality_id] [int] NOT NULL,
	[image_count] [int] NOT NULL,
	[rate] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_rate]  DEFAULT ((0)),
	[amount] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_amount]  DEFAULT ((0)),
	[is_free_read] [nchar](1) NULL CONSTRAINT [DF_invoice_institution_dtls_is_free_read]  DEFAULT ('N'),
	[is_free_scan] [nchar](1) NULL CONSTRAINT [DF_invoice_iinvoice_institution_dtls_is_free_scan]  DEFAULT ('N'),
	[billed] [nchar](1) NULL CONSTRAINT [DF_invoice_institution_dtls_billed]  DEFAULT ('Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[approved] [nchar](1) NULL CONSTRAINT [DF_invoice_institution_dtls_approved]  DEFAULT ('N'),
	[approved_by] [uniqueidentifier] NULL CONSTRAINT [DF_invoice_institution_dtls_approved_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[date_approved] [datetime] NULL,
	[service_amount] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_service_amount]  DEFAULT ((0)),
	[total_amount] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_total_amount]  DEFAULT ((0)),
	[disc_per_applied] [decimal](5, 2) NULL CONSTRAINT [DF_invoice_institution_dtls_disc_per_applied]  DEFAULT ((0)),
	[disc_amount] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_disc_amount]  DEFAULT ((0)),
	[is_free] [nchar](1) NULL CONSTRAINT [DF_invoice_institution_dtls_is_free]  DEFAULT ('N'),
	[promotion_id] [uniqueidentifier] NULL CONSTRAINT [DF_invoice_institution_dtls_promotion_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[study_price] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_study_price]  DEFAULT ((0)),
	[service_price] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_service_price]  DEFAULT ((0)),
	[disapproved_by] [uniqueidentifier] NULL CONSTRAINT [DF_invoice_institution_dtls_disapproved_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[date_disapproved] [datetime] NULL,
	[gl_code] [nvarchar](5) NULL,
	[category_id] [int] NULL CONSTRAINT [DF_invoice_institution_dtls_category_id]  DEFAULT ((0)),
	[rate_per_unit] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_rate_per_unit]  DEFAULT ((0)),
	[study_max_amount] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_study_max_amount]  DEFAULT ((0)),
	[disc_amt_applied] [money] NULL CONSTRAINT [DF_invoice_institution_dtls_disc_amt_applied]  DEFAULT ((0)),
	[disc_type_applied] [nchar](1) NULL CONSTRAINT [DF_invoice_institution_dtls_disc_type_applied]  DEFAULT ('N'),
 CONSTRAINT [PK_invoice_institution_dtls] PRIMARY KEY CLUSTERED 
(
	[hdr_id] ASC,
	[billing_cycle_id] ASC,
	[billing_account_id] ASC,
	[institution_id] ASC,
	[study_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Y]es,[N]o,[X]Cancelled' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'invoice_institution_dtls', @level2type=N'COLUMN',@level2name=N'approved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[N]one,[P]ercentage,[A]mount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'invoice_institution_dtls', @level2type=N'COLUMN',@level2name=N'disc_type_applied'
GO
