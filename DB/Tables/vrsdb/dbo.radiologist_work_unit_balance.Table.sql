USE [vrsdb]
GO
/****** Object:  Table [dbo].[radiologist_work_unit_balance]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[radiologist_work_unit_balance]
GO
/****** Object:  Table [dbo].[radiologist_work_unit_balance]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[radiologist_work_unit_balance](
	[scheduled_date] [date] NOT NULL,
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[mins_scheduled_on_date] [int] NOT NULL,
	[work_unit_on_date] [int] NOT NULL,
	[work_unit_consumed_on_date] [int] NOT NULL,
	[work_unit_balance_on_date] [int] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_radiologist_work_unit_balance] PRIMARY KEY CLUSTERED 
(
	[scheduled_date] ASC,
	[radiologist_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
