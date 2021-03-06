USE [vrsdb]
GO
/****** Object:  Table [dbo].[radiologist_schedule]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[radiologist_schedule]
GO
/****** Object:  Table [dbo].[radiologist_schedule]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[radiologist_schedule](
	[id] [uniqueidentifier] NOT NULL,
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[start_datetime] [datetime] NOT NULL,
	[end_datetime] [datetime] NOT NULL,
	[duration_in_ms] [bigint] NOT NULL,
	[notes] [nvarchar](300) NULL CONSTRAINT [DF_radiologist_schedule_notes]  DEFAULT (''),
	[updated_by] [uniqueidentifier] NOT NULL,
	[date_updated] [datetime] NOT NULL,
 CONSTRAINT [PK_radiologist_schedule] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
