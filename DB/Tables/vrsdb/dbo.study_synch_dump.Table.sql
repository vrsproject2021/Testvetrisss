USE [vrsdb]
GO
/****** Object:  Table [dbo].[study_synch_dump]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[study_synch_dump]
GO
/****** Object:  Table [dbo].[study_synch_dump]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[study_synch_dump](
	[study_uid] [nvarchar](100) NOT NULL,
	[study_date] [datetime] NULL,
	[received_date] [datetime] NULL,
	[accession_no] [nvarchar](20) NULL,
	[study_type] [nvarchar](10) NULL,
	[reason] [nvarchar](2000) NULL,
	[institution_name] [nvarchar](100) NULL,
	[manufacturer_name] [nvarchar](100) NULL,
	[manufacturer_model_no] [nvarchar](100) NULL,
	[device_serial_no] [nvarchar](20) NULL,
	[referring_physician] [nvarchar](200) NULL,
	[patient_id] [nvarchar](36) NULL,
	[patient_name] [nvarchar](100) NULL,
	[patient_sex] [nvarchar](10) NULL,
	[patient_dob] [datetime] NULL,
	[patient_age] [nvarchar](50) NULL,
	[patient_weight] [decimal](12, 3) NULL,
	[owner_name] [nvarchar](200) NULL,
	[sex_neutered] [nvarchar](30) NULL,
	[species] [nvarchar](30) NULL,
	[breed] [nvarchar](50) NULL,
	[modality] [nvarchar](50) NULL,
	[body_part] [nvarchar](50) NULL,
	[study_desc] [nvarchar](500) NULL,
	[study_status] [nvarchar](5) NULL CONSTRAINT [DF_study_synch_dump_study_status]  DEFAULT ((0)),
	[img_count] [int] NULL CONSTRAINT [DF_study_synch_dump_img_count]  DEFAULT ((0)),
	[synched_on] [datetime] NULL,
	[modality_ae_title] [nvarchar](50) NULL,
	[deleted] [char](1) NULL CONSTRAINT [DF_study_synch_dump_deleted]  DEFAULT (N'N'),
	[priority_id] [int] NULL,
	[object_count] [int] NULL CONSTRAINT [DF_study_synch_dump_object_count]  DEFAULT ((0)),
	[received_via_dicom_router] [nchar](1) NULL,
 CONSTRAINT [PK_study_synch_dump] PRIMARY KEY CLUSTERED 
(
	[study_uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
