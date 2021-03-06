USE [vrsarchive19db]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_updated_by]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_patient_state_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_patient_country_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_priority_charged]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_sender_time_offset_mins]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_is_manual]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_storage_applied]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_ischeduler_img_file_downloads_grouped_category_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_consult_applied]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_salesperson_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_merge_status]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_file_xfer_count]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_approved_by]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_approve_for_pacs]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] DROP CONSTRAINT [DF_scheduler_img_file_downloads_grouped_modality_id]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_grouped]    Script Date: 21-09-2021 17:14:12 ******/
DROP TABLE [dbo].[scheduler_img_file_downloads_grouped]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_grouped]    Script Date: 21-09-2021 17:14:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_img_file_downloads_grouped](
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
	[modality_id] [int] NULL,
	[modality] [nvarchar](30) NULL,
	[series_instance_uid] [nvarchar](100) NULL,
	[series_no] [nvarchar](100) NULL,
	[approve_for_pacs] [nchar](1) NULL,
	[approved_by] [uniqueidentifier] NULL,
	[date_approved] [datetime] NULL,
	[file_xfer_count] [int] NULL,
	[accession_no] [nvarchar](20) NULL,
	[reason] [nvarchar](2000) NULL,
	[physician_id] [uniqueidentifier] NULL,
	[patient_dob] [datetime] NULL,
	[patient_age] [nvarchar](50) NULL,
	[patient_sex] [nvarchar](10) NULL,
	[spayed_neutered] [nvarchar](30) NULL,
	[patient_weight] [decimal](12, 3) NULL,
	[wt_uom] [nvarchar](5) NULL,
	[owner_first_name] [nvarchar](100) NULL,
	[owner_last_name] [nvarchar](100) NULL,
	[species_id] [int] NULL,
	[breed_id] [uniqueidentifier] NULL,
	[priority_id] [int] NULL,
	[merge_status] [nchar](1) NULL,
	[merge_status_desc] [nvarchar](max) NULL,
	[salesperson_id] [uniqueidentifier] NULL,
	[physician_note] [nvarchar](2000) NULL,
	[consult_applied] [nchar](1) NULL,
	[category_id] [int] NULL,
	[storage_applied] [nchar](1) NULL,
	[is_manual] [nchar](1) NULL,
	[beyond_hour_stat] [nchar](1) NULL,
	[sender_time_offset_mins] [int] NULL,
	[priority_charged] [nchar](1) NULL,
	[patient_country_id] [int] NULL,
	[patient_state_id] [int] NULL,
	[patient_city] [nvarchar](100) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_scheduler_img_file_downloads_grouped] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[study_uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_modality_id]  DEFAULT ((0)) FOR [modality_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_approve_for_pacs]  DEFAULT (N'N') FOR [approve_for_pacs]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_approved_by]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [approved_by]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_file_xfer_count]  DEFAULT ((0)) FOR [file_xfer_count]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_merge_status]  DEFAULT ('N') FOR [merge_status]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_salesperson_id]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [salesperson_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_consult_applied]  DEFAULT ('N') FOR [consult_applied]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_ischeduler_img_file_downloads_grouped_category_id]  DEFAULT ((0)) FOR [category_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_storage_applied]  DEFAULT ('N') FOR [storage_applied]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_is_manual]  DEFAULT ('N') FOR [is_manual]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_sender_time_offset_mins]  DEFAULT ((0)) FOR [sender_time_offset_mins]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_priority_charged]  DEFAULT ('Y') FOR [priority_charged]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_patient_country_id]  DEFAULT ((0)) FOR [patient_country_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_patient_state_id]  DEFAULT ((0)) FOR [patient_state_id]
GO
ALTER TABLE [dbo].[scheduler_img_file_downloads_grouped] ADD  CONSTRAINT [DF_scheduler_img_file_downloads_grouped_updated_by]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [updated_by]
GO
