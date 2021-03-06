USE [vrsmisdb]
GO
/****** Object:  Table [dbo].[studies]    Script Date: 20-08-2021 20:50:12 ******/
DROP TABLE [dbo].[studies]
GO
/****** Object:  Table [dbo].[studies]    Script Date: 20-08-2021 20:50:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[studies](
	[id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[modality_id] [int] NOT NULL,
	[category_id] [int] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[priority_id] [int] NOT NULL,
	[synched_on] [datetime] NOT NULL,
	[study_date] [datetime] NOT NULL,
	[patient_name] [nvarchar](100) NOT NULL,
	[species_id] [int] NOT NULL,
	[breed_id] [uniqueidentifier] NOT NULL,
	[patient_dob] [datetime] NULL,
	[patient_age] [nvarchar](50) NULL,
	[patient_sex] [nvarchar](10) NULL,
	[image_count] [int] NULL CONSTRAINT [DF_studies_image_count]  DEFAULT ((0)),
	[object_count] [int] NULL CONSTRAINT [DF_studies_object_count]  DEFAULT ((0)),
	[file_archive_count] [int] NULL CONSTRAINT [DF_studies_file_archive_count]  DEFAULT ((0)),
	[submitted_on] [datetime] NULL,
	[assigned_radiologist_id] [uniqueidentifier] NULL CONSTRAINT [DF_studies_assigned_radiologist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[assigned_radiologist_on] [datetime] NULL,
	[dict_radiologist_id] [uniqueidentifier] NULL CONSTRAINT [DF_studies_dict_radiologist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[report_dictated_on] [datetime] NULL,
	[prelim_radiologist_id] [uniqueidentifier] NULL CONSTRAINT [DF_studies_prelim_radiologist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[report_prelim_on] [datetime] NULL,
	[final_radiologist_id] [uniqueidentifier] NULL CONSTRAINT [DF_studies_final_radiologist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[report_final_on] [datetime] NULL,
	[report_addendum_count] [int] NULL CONSTRAINT [DF_studies_report_addendum_count]  DEFAULT ((0)),
	[report_addendum_last_added_on] [datetime] NULL,
	[invoiced] [nchar](1) NULL CONSTRAINT [DF_studies_invoiced]  DEFAULT ('N'),
	[invoiced_amount] [money] NULL CONSTRAINT [DF_studies_invoiced_amount]  DEFAULT ('0'),
	[payable_amount] [money] NULL CONSTRAINT [DF_studies_payable_amount]  DEFAULT ('0'),
	[status_last_updated_on] [datetime] NULL,
	[mis_updated_by] [uniqueidentifier] NULL CONSTRAINT [DF_studies_mis_updated_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[mis_updated_on] [datetime] NULL,
 CONSTRAINT [PK_studies] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[study_uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
