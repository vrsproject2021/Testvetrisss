USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'day_end_vetris_account_posting', @level2type=N'COLUMN',@level2name=N'ref_type'

GO
/****** Object:  Table [dbo].[day_end_vetris_account_posting]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[day_end_vetris_account_posting]
GO
/****** Object:  Table [dbo].[day_end_vetris_account_posting]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[day_end_vetris_account_posting](
	[day_end_date] [datetime] NOT NULL,
	[ref_no] [nvarchar](30) NOT NULL,
	[ref_date] [datetime] NOT NULL,
	[ref_type] [nvarchar](10) NOT NULL,
	[gl_code] [nvarchar](5) NOT NULL,
	[gl_desc] [nvarchar](100) NOT NULL,
	[dr_amount] [money] NULL,
	[cr_amount] [money] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_day_end_vetris_account_posting] PRIMARY KEY CLUSTERED 
(
	[day_end_date] ASC,
	[ref_no] ASC,
	[ref_type] ASC,
	[gl_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[INV]Invoice,[INVREV] Invoice Reversal,[PMTREC]Payment Received,[PMTREF]Payment Refunded,[PMTRAD] Paid to radiologist,[PMTRADREV] Radiologist payment reversed,[PMTTRS] Paid to transcriptionist,[PMTTRSREV] Transcriptionist payment reversed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'day_end_vetris_account_posting', @level2type=N'COLUMN',@level2name=N'ref_type'
GO
