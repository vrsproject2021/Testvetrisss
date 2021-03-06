USE [vrsdb]
GO
/****** Object:  Table [dbo].[ap_radiologist_payment_dtls]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ap_radiologist_payment_dtls]
GO
/****** Object:  Table [dbo].[ap_radiologist_payment_dtls]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_radiologist_payment_dtls](
	[id] [uniqueidentifier] NOT NULL,
	[hdr_id] [uniqueidentifier] NOT NULL,
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[study_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[modality_id] [int] NOT NULL,
	[priority_id] [int] NOT NULL,
	[is_reading_prelim] [nchar](1) NULL CONSTRAINT [DF_ap_radiologist_payment_dtls_is_reading_prelim]  DEFAULT ('N'),
	[is_reading_final] [nchar](1) NULL CONSTRAINT [DF_ap_radiologist_payment_dtls_is_reading_final]  DEFAULT ('N'),
	[prelim_rate] [money] NULL CONSTRAINT [DF_ap_radiologist_payment_dtls_prelim_rate]  DEFAULT ((0)),
	[prelim_amount] [money] NULL CONSTRAINT [DF_ap_radiologist_payment_dtls_prelim_amount]  DEFAULT ((0)),
	[final_rate] [money] NULL CONSTRAINT [DF_ap_radiologist_payment_dtls_final_rate]  DEFAULT ((0)),
	[final_amount] [money] NULL CONSTRAINT [DF_ap_radiologist_payment_dtls_final_amount]  DEFAULT ((0)),
	[addl_stat_rate] [money] NULL CONSTRAINT [DF_ap_radiologist_payment_dtls_addl_stat_rate]  DEFAULT ((0)),
	[adhoc_amount] [money] NULL,
	[total_amount] [money] NULL CONSTRAINT [DF_ap_radiologist_payment_dtls_total_amount]  DEFAULT ((0)),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_radiologist_payment_dtls] PRIMARY KEY CLUSTERED 
(
	[hdr_id] ASC,
	[billing_cycle_id] ASC,
	[radiologist_id] ASC,
	[study_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
