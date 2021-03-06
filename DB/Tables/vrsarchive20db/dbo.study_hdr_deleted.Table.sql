USE [vrsarchive20db]
GO
/****** Object:  Table [dbo].[study_hdr_deleted]    Script Date: 21-09-2021 17:15:31 ******/
DROP TABLE [dbo].[study_hdr_deleted]
GO
/****** Object:  Table [dbo].[study_hdr_deleted]    Script Date: 21-09-2021 17:15:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[study_hdr_deleted](
	[id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[study_date] [datetime] NOT NULL,
	[patient_name] [nvarchar](100) NULL,
	[institution_name] [nvarchar](100) NULL,
	[deleted_on] [datetime] NULL,
	[deleted_by] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_deleted_deleted_by]  DEFAULT (N'00000000-0000-0000-0000-000000000000'),
	[received_date] [datetime] NULL,
	[synched_on] [datetime] NULL,
	[remarks] [nvarchar](250) NULL
) ON [PRIMARY]

GO
