USE [vrslogdb]
GO
/****** Object:  Table [dbo].[radiologist_assignment_log]    Script Date: 20-08-2021 20:49:16 ******/
DROP TABLE [dbo].[radiologist_assignment_log]
GO
/****** Object:  Table [dbo].[radiologist_assignment_log]    Script Date: 20-08-2021 20:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[radiologist_assignment_log](
	[scheduled_date] [date] NOT NULL,
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[modality_id] [int] NOT NULL,
	[category_id] [int] NOT NULL,
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[assign_datetime] [datetime] NOT NULL
) ON [PRIMARY]

GO
