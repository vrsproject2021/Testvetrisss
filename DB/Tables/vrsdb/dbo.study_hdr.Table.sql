USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'study_hdr', @level2type=N'COLUMN',@level2name=N'discount_type'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'study_hdr', @level2type=N'COLUMN',@level2name=N'manually_assigned'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'study_hdr', @level2type=N'COLUMN',@level2name=N'sync_mode'

GO
/****** Object:  Table [dbo].[study_hdr]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[study_hdr]
GO
/****** Object:  Table [dbo].[study_hdr]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[study_hdr](
	[id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[synched_on] [datetime] NULL,
	[sync_mode] [nvarchar](5) NULL CONSTRAINT [DF_study_hdr_sync_mode]  DEFAULT ('PACS'),
	[study_date] [datetime] NOT NULL,
	[received_date] [datetime] NULL,
	[accession_no_pacs] [nvarchar](20) NULL,
	[accession_no] [nvarchar](20) NULL,
	[study_type_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_study_type_id]  DEFAULT (N'00000000-0000-0000-0000-000000000000'),
	[institution_name_pacs] [nvarchar](100) NULL,
	[institution_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_institution_id]  DEFAULT (N'00000000-0000-0000-0000-000000000000'),
	[manufacturer_name] [nvarchar](100) NULL,
	[manufacturer_model_no] [nvarchar](100) NULL,
	[device_serial_no] [nvarchar](20) NULL,
	[modality_ae_title] [nvarchar](50) NULL,
	[referring_physician_pacs] [nvarchar](200) NULL,
	[physician_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_physician_id]  DEFAULT (N'00000000-0000-0000-0000-000000000000'),
	[patient_id_pacs] [nvarchar](20) NULL,
	[patient_id] [nvarchar](36) NULL,
	[patient_name_pacs] [nvarchar](100) NULL,
	[patient_name] [nvarchar](100) NULL,
	[patient_fname] [nvarchar](40) NULL,
	[patient_lname] [nvarchar](40) NULL,
	[patient_country_id] [int] NULL,
	[patient_state_id] [int] NULL,
	[patient_city] [nvarchar](100) NULL,
	[patient_sex_pacs] [nvarchar](10) NULL,
	[patient_sex] [nvarchar](10) NULL,
	[patient_sex_neutered_pacs] [nvarchar](30) NULL,
	[patient_sex_neutered] [nvarchar](30) NULL,
	[patient_weight_pacs] [decimal](12, 2) NULL,
	[patient_weight] [decimal](12, 3) NULL,
	[patient_dob_pacs] [datetime] NULL CONSTRAINT [DF_study_hdr_patient_dob_pacs]  DEFAULT ((0)),
	[patient_dob_accepted] [datetime] NULL,
	[patient_age_pacs] [nvarchar](50) NULL,
	[patient_age_accepted] [nvarchar](50) NULL,
	[sex_neutered_pacs] [nvarchar](30) NULL,
	[sex_neutered_accepted] [nvarchar](30) NULL,
	[patient_weight_kgs] [decimal](12, 3) NULL,
	[wt_uom] [nvarchar](5) NULL CONSTRAINT [DF_study_hdr_wt_uom]  DEFAULT (N''),
	[owner_name_pacs] [nvarchar](200) NULL,
	[owner_first_name] [nvarchar](100) NULL,
	[owner_last_name] [nvarchar](100) NULL,
	[species_pacs] [nvarchar](30) NULL,
	[species_id] [int] NULL CONSTRAINT [DF_study_hdr_species_id]  DEFAULT ((0)),
	[breed_pacs] [nvarchar](50) NULL,
	[breed_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_breed_id]  DEFAULT (N'00000000-0000-0000-0000-000000000000'),
	[modality_pacs] [nvarchar](50) NULL,
	[modality_id] [int] NULL CONSTRAINT [DF_study_hdr_modality_id]  DEFAULT ((0)),
	[category_id] [int] NULL CONSTRAINT [DF_study_hdr_category_id]  DEFAULT ((0)),
	[body_part_pacs] [nvarchar](50) NULL,
	[body_part_id] [int] NULL CONSTRAINT [DF_study_hdr_body_part_id]  DEFAULT ((0)),
	[priority_id_pacs] [int] NULL,
	[priority_id] [int] NULL,
	[study_desc] [nvarchar](500) NULL,
	[reason_pacs] [nvarchar](2000) NULL,
	[reason_accepted] [nvarchar](2000) NULL,
	[img_count_pacs] [int] NULL,
	[img_count] [int] NULL CONSTRAINT [DF_study_hdr_img_count]  DEFAULT ((0)),
	[img_count_accepted] [nchar](1) NULL,
	[object_count] [int] NULL CONSTRAINT [DF_study_hdr_object_count]  DEFAULT ((0)),
	[object_count_pacs] [int] NULL CONSTRAINT [DF_study_hdr_object_count_pacs]  DEFAULT ((0)),
	[service_codes] [nvarchar](250) NULL,
	[consult_applied] [nchar](1) NULL CONSTRAINT [DF_study_hdr_consult_applied]  DEFAULT ('N'),
	[physician_note] [nvarchar](2000) NULL,
	[merge_status] [nchar](1) NULL,
	[merge_status_desc] [nvarchar](max) NULL,
	[study_status_pacs] [int] NULL,
	[study_status] [int] NULL CONSTRAINT [DF_study_hdr_study_status]  DEFAULT ((1)),
	[pacs_wb] [nchar](1) NULL CONSTRAINT [DF_study_hdr_pacs_wb]  DEFAULT ('N'),
	[prelim_rpt_updated] [nchar](1) NULL CONSTRAINT [DF_study_hdr_prelim_rpt_updated]  DEFAULT ('N'),
	[final_rpt_updated] [nchar](1) NULL CONSTRAINT [DF_study_hdr_final_rpt_updated]  DEFAULT ('N'),
	[radiologist_pacs] [nvarchar](250) NULL,
	[radiologist_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_radiologist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[prelim_sms_updated] [nchar](1) NULL CONSTRAINT [DF_study_hdr_prelim_sms_updated]  DEFAULT ('N'),
	[final_sms_updated] [nchar](1) NULL CONSTRAINT [DF_study_hdr_final_sms_updated]  DEFAULT ('N'),
	[salesperson_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_salesperson_id]  DEFAULT (N'00000000-0000-0000-0000-000000000000'),
	[dict_radiologist_pacs] [nvarchar](250) NULL,
	[dict_radiologist_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_dict_radiologist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[dict_tanscriptionist_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_dict_tanscriptionist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[transcription_finishing_datetime] [datetime] NULL,
	[prelim_radiologist_pacs] [nvarchar](250) NULL,
	[prelim_radiologist_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_prelim_radiologist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[final_radiologist_pacs] [nvarchar](250) NULL,
	[final_radiologist_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_final_radiologist_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[finishing_datetime] [datetime] NULL,
	[discount_per] [decimal](5, 2) NULL CONSTRAINT [DF_study_hdr_discount_per]  DEFAULT ((0)),
	[is_free] [nchar](1) NULL CONSTRAINT [DF_study_hdr_is_free]  DEFAULT ('N'),
	[promo_reason_id] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_promo_reason_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[promo_applied_by] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_promo_applied_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[promo_applied_on] [datetime] NULL,
	[priority_charged] [nchar](1) NULL CONSTRAINT [DF_study_hdr_priority_charged]  DEFAULT ('Y'),
	[status_last_updated_on] [datetime] NULL,
	[rpt_approve_date] [datetime] NULL,
	[rpt_record_date] [datetime] NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[manually_assigned] [nchar](1) NULL CONSTRAINT [DF_study_hdr_manually_assigned]  DEFAULT ('N'),
	[deleted] [nchar](1) NULL CONSTRAINT [DF_study_hdr_deleted]  DEFAULT (N'N'),
	[invoiced] [nchar](1) NULL CONSTRAINT [DF_study_hdr_invoiced]  DEFAULT ('N'),
	[received_via_dicom_router] [nchar](1) NULL CONSTRAINT [DF_study_hdr_received_via_dicom_router]  DEFAULT ('N'),
	[beyond_hour_stat] [nchar](1) NULL CONSTRAINT [DF_study_hdr_archive_beyond_hour_stat]  DEFAULT ('N'),
	[final_rpt_released] [nchar](1) NULL CONSTRAINT [DF_study_hdr_final_rpt_released]  DEFAULT ('N'),
	[final_rpt_release_datetime] [datetime] NULL,
	[final_rpt_released_on] [datetime] NULL,
	[final_rpt_released_by] [uniqueidentifier] NULL CONSTRAINT [DF_study_hdr_final_rpt_released_by]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[assign_accepted] [nchar](1) NULL CONSTRAINT [DF_study_hdr_assign_accepted]  DEFAULT ('Y'),
	[mark_to_teach] [nchar](1) NULL CONSTRAINT [DF_study_hdr_mark_to_teach]  DEFAULT ('N'),
	[archive_file_count] [int] NULL CONSTRAINT [DF_study_hdr_archive_file_count]  DEFAULT ((0)),
	[log_available] [nchar](1) NULL CONSTRAINT [DF_study_hdr_log_available]  DEFAULT ('Y'),
	[rad_assigned_on] [datetime] NULL,
	[discount_type] [nchar](1) NULL CONSTRAINT [DF_study_hdr_discount_type]  DEFAULT ('N'),
	[discount_amount] [money] NULL CONSTRAINT [DF_study_hdr_discount_amount]  DEFAULT ((0)),
 CONSTRAINT [PK_study_hdr] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[PACS]PACS,[MS]Manual Submission,[DR]DICOM Router,[FD]File Distribution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'study_hdr', @level2type=N'COLUMN',@level2name=N'sync_mode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Y]es,[N]o,[A]uto,[S]elf' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'study_hdr', @level2type=N'COLUMN',@level2name=N'manually_assigned'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[N]one,[P]ercentage,[A]mount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'study_hdr', @level2type=N'COLUMN',@level2name=N'discount_type'
GO
