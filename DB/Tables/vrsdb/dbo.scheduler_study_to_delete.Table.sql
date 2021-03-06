USE [vrsdb]
GO
/****** Object:  Table [dbo].[scheduler_study_to_delete]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[scheduler_study_to_delete]
GO
/****** Object:  Table [dbo].[scheduler_study_to_delete]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_study_to_delete](
	[study_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[study_status_id] [int] NOT NULL,
	[received_via_dicom_router] [nchar](1) NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_scheduler_study_to_delete] PRIMARY KEY CLUSTERED 
(
	[study_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
