USE [vrsarchive20db]
GO
/****** Object:  Table [dbo].[scheduler_file_downloads]    Script Date: 21-09-2021 17:15:31 ******/
DROP TABLE [dbo].[scheduler_file_downloads]
GO
/****** Object:  Table [dbo].[scheduler_file_downloads]    Script Date: 21-09-2021 17:15:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_file_downloads](
	[id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[study_date] [datetime] NULL,
	[file_count] [int] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[institution_code] [nvarchar](5) NOT NULL,
	[institution_name] [nvarchar](100) NOT NULL,
	[patient_id] [nvarchar](20) NULL,
	[patient_fname] [nvarchar](80) NULL,
	[patient_lname] [nvarchar](80) NULL,
	[file_xfer_count] [int] NULL CONSTRAINT [DF_scheduler_file_downloads_file_xfer_count]  DEFAULT ((0)),
	[write_back_status] [nchar](1) NULL CONSTRAINT [DF_scheduler_file_downloads_write_back_status]  DEFAULT (N'N'),
	[received_date] [datetime] NULL,
	[accession_no] [nvarchar](20) NULL,
	[modality] [nvarchar](10) NULL,
	[reason] [nvarchar](2000) NULL,
	[manufacturer_name] [nvarchar](100) NULL,
	[manufacturer_model_no] [nvarchar](100) NULL,
	[device_serial_no] [nvarchar](100) NULL,
	[modality_ae_title] [nvarchar](50) NULL,
	[referring_physician] [nvarchar](100) NULL,
	[patient_sex] [nvarchar](10) NULL,
	[patient_dob] [datetime] NULL,
	[patient_age] [nvarchar](50) NULL,
	[priority_id] [int] NULL,
	[req_action_created] [nchar](1) NULL CONSTRAINT [DF_scheduler_file_downloads_req_action_created]  DEFAULT ('N'),
	[import_session_id] [nvarchar](30) NULL,
	[is_add_on] [nchar](1) NULL CONSTRAINT [DF_scheduler_file_downloads_is_add_on]  DEFAULT ('N'),
	[is_manual] [nchar](1) NULL CONSTRAINT [DF_scheduler_file_downloads_is_manual]  DEFAULT ('N'),
	[study_found] [nchar](1) NULL CONSTRAINT [DF_scheduler_file_downloads_study_found]  DEFAULT ('Y'),
	[date_downloaded] [datetime] NOT NULL,
	[approve_for_pacs] [nchar](1) NULL CONSTRAINT [DF_scheduler_file_downloads_approve_for_pacs]  DEFAULT (N'N'),
	[approved_by] [uniqueidentifier] NULL CONSTRAINT [DF_scheduler_file_downloads_approved_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[date_approved] [datetime] NULL,
	[updated_by] [uniqueidentifier] NULL CONSTRAINT [DF_scheduler_file_downloads_updated_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_scheduler_file_downloads] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[study_uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
