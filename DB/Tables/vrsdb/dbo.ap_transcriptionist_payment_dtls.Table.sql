USE [vrsdb]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] DROP CONSTRAINT [DF_ap_transcriptionist_payment_dtls_total_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] DROP CONSTRAINT [DF_ap_transcriptionist_payment_dtls_adhoc_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] DROP CONSTRAINT [DF_ap_transcriptionist_payment_dtls_addl_stat_rate]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] DROP CONSTRAINT [DF_ap_transcriptionist_payment_dtls_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] DROP CONSTRAINT [DF_ap_transcriptionist_payment_dtls_rate]
GO
/****** Object:  Table [dbo].[ap_transcriptionist_payment_dtls]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ap_transcriptionist_payment_dtls]
GO
/****** Object:  Table [dbo].[ap_transcriptionist_payment_dtls]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_transcriptionist_payment_dtls](
	[id] [uniqueidentifier] NOT NULL,
	[hdr_id] [uniqueidentifier] NOT NULL,
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[transcriptionist_id] [uniqueidentifier] NOT NULL,
	[study_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[modality_id] [int] NOT NULL,
	[priority_id] [int] NOT NULL,
	[rate] [money] NULL,
	[amount] [money] NULL,
	[addl_stat_rate] [money] NULL,
	[adhoc_amount] [money] NULL,
	[total_amount] [money] NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_transcriptionist_payment_dtls] PRIMARY KEY CLUSTERED 
(
	[hdr_id] ASC,
	[billing_cycle_id] ASC,
	[transcriptionist_id] ASC,
	[study_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_dtls_rate]  DEFAULT ((0)) FOR [rate]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_dtls_amount]  DEFAULT ((0)) FOR [amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_dtls_addl_stat_rate]  DEFAULT ((0)) FOR [addl_stat_rate]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_dtls_adhoc_amount]  DEFAULT ((0)) FOR [adhoc_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_dtls] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_dtls_total_amount]  DEFAULT ((0)) FOR [total_amount]
GO
