USE [vrsdb]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_update_qb]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_approved]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_payment_srl_year]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_payment_srl_no]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_adhoc_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_stat_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_std_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_study_count_stat]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_study_count_std]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] DROP CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_study_count]
GO
/****** Object:  Table [dbo].[ap_transcriptionist_payment_hdr]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ap_transcriptionist_payment_hdr]
GO
/****** Object:  Table [dbo].[ap_transcriptionist_payment_hdr]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_transcriptionist_payment_hdr](
	[id] [uniqueidentifier] NOT NULL,
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[transcriptionist_id] [uniqueidentifier] NOT NULL,
	[total_study_count] [int] NULL,
	[total_study_count_std] [int] NULL,
	[total_study_count_stat] [int] NULL,
	[total_std_amount] [int] NULL,
	[total_stat_amount] [int] NULL,
	[total_adhoc_amount] [money] NULL,
	[total_amount] [money] NULL,
	[payment_srl_no] [int] NULL,
	[payment_srl_year] [int] NULL,
	[payment_no] [nvarchar](50) NULL,
	[payment_date] [nvarchar](50) NULL,
	[approved] [nchar](1) NULL,
	[approved_by] [uniqueidentifier] NULL,
	[disapproved_by] [uniqueidentifier] NULL,
	[date_disapproved] [datetime] NULL,
	[date_approved] [datetime] NULL,
	[update_qb] [nchar](1) NULL,
	[update_qb_on] [datetime] NULL,
	[qb_posting_id] [nvarchar](20) NULL,
	[qb_rev_posting_id] [nvarchar](20) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_ap_transcriptionist_payment_hdr] PRIMARY KEY CLUSTERED 
(
	[billing_cycle_id] ASC,
	[transcriptionist_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_study_count]  DEFAULT ((0)) FOR [total_study_count]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_study_count_std]  DEFAULT ((0)) FOR [total_study_count_std]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_study_count_stat]  DEFAULT ((0)) FOR [total_study_count_stat]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_std_amount]  DEFAULT ((0)) FOR [total_std_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_stat_amount]  DEFAULT ((0)) FOR [total_stat_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_adhoc_amount]  DEFAULT ((0)) FOR [total_adhoc_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_total_amount]  DEFAULT ((0)) FOR [total_amount]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_payment_srl_no]  DEFAULT ((0)) FOR [payment_srl_no]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_payment_srl_year]  DEFAULT ((0)) FOR [payment_srl_year]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_approved]  DEFAULT ('N') FOR [approved]
GO
ALTER TABLE [dbo].[ap_transcriptionist_payment_hdr] ADD  CONSTRAINT [DF_ap_transcriptionist_payment_hdr_update_qb]  DEFAULT ('N') FOR [update_qb]
GO
