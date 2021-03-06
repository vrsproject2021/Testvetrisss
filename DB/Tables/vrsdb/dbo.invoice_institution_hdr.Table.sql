USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'invoice_institution_hdr', @level2type=N'COLUMN',@level2name=N'approved'

GO
/****** Object:  Table [dbo].[invoice_institution_hdr]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[invoice_institution_hdr]
GO
/****** Object:  Table [dbo].[invoice_institution_hdr]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoice_institution_hdr](
	[id] [uniqueidentifier] NOT NULL,
	[hdr_id] [uniqueidentifier] NOT NULL,
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[total_study_count] [int] NULL CONSTRAINT [DF_invoice_institution_hdr_total_study_count]  DEFAULT ((0)),
	[total_study_count_std] [int] NULL CONSTRAINT [DF_invoice_institution_hdr_total_study_count_std]  DEFAULT ((0)),
	[total_study_count_stat] [int] NULL CONSTRAINT [DF_invoice_institution_hdr_total_study_count_stat]  DEFAULT ((0)),
	[free_read_count] [int] NULL CONSTRAINT [DF_invoice_institution_hdr_free_read_count]  DEFAULT ((0)),
	[free_scan_count] [int] NULL CONSTRAINT [DF_invoice_institution_hdr_free_scan_count]  DEFAULT ((0)),
	[total_amount] [money] NULL CONSTRAINT [DF_invoice_institution_hdr_total_amount]  DEFAULT ((0)),
	[invoice_no] [nvarchar](50) NULL,
	[invoice_date] [datetime] NULL,
	[invoice_due_date] [datetime] NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[approved] [nchar](1) NULL CONSTRAINT [DF_invoice_institution_hdr_approved]  DEFAULT ('N'),
	[approved_by] [uniqueidentifier] NULL CONSTRAINT [DF_invoice_institution_hdr_approved_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[date_approved] [datetime] NULL,
	[total_disc_amount] [money] NULL CONSTRAINT [DF_invoice_institution_hdr_total_disc_amount]  DEFAULT ((0)),
	[disapproved_by] [uniqueidentifier] NULL CONSTRAINT [DF_invoice_institution_hdr_disapproved_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[date_disapproved] [datetime] NULL,
 CONSTRAINT [PK_invoice_institution_hdr] PRIMARY KEY CLUSTERED 
(
	[hdr_id] ASC,
	[billing_cycle_id] ASC,
	[billing_account_id] ASC,
	[institution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Y]es,[N]o,[X]Cancelled' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'invoice_institution_hdr', @level2type=N'COLUMN',@level2name=N'approved'
GO
