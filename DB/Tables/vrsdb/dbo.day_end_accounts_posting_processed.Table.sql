USE [vrsdb]
GO
/****** Object:  Table [dbo].[day_end_accounts_posting_processed]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[day_end_accounts_posting_processed]
GO
/****** Object:  Table [dbo].[day_end_accounts_posting_processed]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[day_end_accounts_posting_processed](
	[day_end_date] [datetime] NOT NULL,
	[record_count] [int] NOT NULL,
	[process_completed] [nchar](1) NOT NULL,
	[notified] [nchar](1) NULL CONSTRAINT [DF_day_end_accounts_posting_processed_notified]  DEFAULT ('N'),
 CONSTRAINT [PK_day_end_accounts_posting_processed] PRIMARY KEY CLUSTERED 
(
	[day_end_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
