USE [vrsdb]
GO
/****** Object:  Table [dbo].[ap_radiologist_adhoc_payments]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ap_radiologist_adhoc_payments]
GO
/****** Object:  Table [dbo].[ap_radiologist_adhoc_payments]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_radiologist_adhoc_payments](
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[billing_cycle_id] [uniqueidentifier] NOT NULL,
	[study_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[adhoc_payment] [money] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_ap_radiologist_adhoc_payments] PRIMARY KEY CLUSTERED 
(
	[radiologist_id] ASC,
	[billing_cycle_id] ASC,
	[study_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
