USE [vrsdb]
GO
/****** Object:  Table [dbo].[ap_radiologist_payment_hdr]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ap_radiologist_payment_hdr]
GO
/****** Object:  Table [dbo].[ap_radiologist_payment_hdr]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_radiologist_payment_hdr](
	[id] [uniqueidentifier] NOT NULL,
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[total_study_count] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_study_count]  DEFAULT ((0)),
	[total_study_count_prelim] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_study_count_prelim]  DEFAULT ((0)),
	[total_study_count_final] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_study_count_final]  DEFAULT ((0)),
	[total_study_count_prelim_std] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_study_count_prelim_std]  DEFAULT ((0)),
	[total_study_count_prelim_stat] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_study_count_prelim_stat]  DEFAULT ((0)),
	[total_study_count_final_std] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_study_count_final_std]  DEFAULT ((0)),
	[total_study_count_final_stat] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_study_count_final_stat]  DEFAULT ((0)),
	[total_std_amount] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_std_amount]  DEFAULT ((0)),
	[total_stat_amount] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_stat_amount]  DEFAULT ((0)),
	[total_adhoc_amount] [money] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_adhoc_amount]  DEFAULT ((0)),
	[total_amount] [money] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_total_amount]  DEFAULT ((0)),
	[payment_srl_no] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_payment_srl_no]  DEFAULT ((0)),
	[payment_srl_year] [int] NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_payment_srl_year]  DEFAULT ((0)),
	[payment_no] [nvarchar](50) NULL,
	[payment_date] [nvarchar](50) NULL,
	[approved] [nchar](1) NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_approved]  DEFAULT ('N'),
	[approved_by] [uniqueidentifier] NULL,
	[disapproved_by] [uniqueidentifier] NULL,
	[date_disapproved] [datetime] NULL,
	[date_approved] [datetime] NULL,
	[update_qb] [nchar](1) NULL CONSTRAINT [DF_ap_radiologist_payment_hdr_update_qb]  DEFAULT ('N'),
	[update_qb_on] [datetime] NULL,
	[qb_posting_id] [nvarchar](20) NULL,
	[qb_rev_posting_id] [nvarchar](20) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_ap_radiologist_payment_hdr] PRIMARY KEY CLUSTERED 
(
	[billing_cycle_id] ASC,
	[radiologist_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
