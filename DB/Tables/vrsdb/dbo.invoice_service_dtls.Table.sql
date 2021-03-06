USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'invoice_service_dtls', @level2type=N'COLUMN',@level2name=N'disc_type_applied'

GO
/****** Object:  Table [dbo].[invoice_service_dtls]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[invoice_service_dtls]
GO
/****** Object:  Table [dbo].[invoice_service_dtls]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoice_service_dtls](
	[id] [uniqueidentifier] NOT NULL,
	[hdr_id] [uniqueidentifier] NOT NULL,
	[institution_hdr_id] [uniqueidentifier] NOT NULL,
	[institution_dtls_id] [uniqueidentifier] NOT NULL,
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[study_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[service_id] [int] NOT NULL,
	[priority_id] [int] NULL CONSTRAINT [DF_invoice_service_dtls_priority_id]  DEFAULT ((0)),
	[amount] [money] NULL CONSTRAINT [DF_invoice_service_dtls_amount]  DEFAULT ((0)),
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[gl_code] [nvarchar](5) NULL,
	[service_price] [money] NULL CONSTRAINT [DF_invoice_service_dtls_service_price]  DEFAULT ((0)),
	[disc_per_applied] [decimal](5, 2) NULL CONSTRAINT [DF_invoice_service_dtls_disc_per_applied]  DEFAULT ((0)),
	[is_free] [nchar](1) NULL CONSTRAINT [DF_invoice_service_dtls_is_free]  DEFAULT ('N'),
	[service_price_after_hrs] [money] NULL CONSTRAINT [DF_invoice_service_dtls_service_price_after_hrs]  DEFAULT ((0)),
	[is_after_hrs] [nchar](1) NULL CONSTRAINT [DF_invoice_service_dtls_is_after_hrs]  DEFAULT ('N'),
	[modality_id] [int] NULL,
	[disc_amt_applied] [money] NULL CONSTRAINT [DF_invoice_service_dtls_disc_amt_applied]  DEFAULT ((0)),
	[disc_type_applied] [nchar](1) NULL CONSTRAINT [DF_invoice_service_dtls_disc_type_applied]  DEFAULT ('N'),
 CONSTRAINT [PK_invoice_service_dtls] PRIMARY KEY CLUSTERED 
(
	[hdr_id] ASC,
	[institution_hdr_id] ASC,
	[institution_dtls_id] ASC,
	[billing_cycle_id] ASC,
	[billing_account_id] ASC,
	[institution_id] ASC,
	[service_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[N]one,[P]ercentage,[A]mount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'invoice_service_dtls', @level2type=N'COLUMN',@level2name=N'disc_type_applied'
GO
