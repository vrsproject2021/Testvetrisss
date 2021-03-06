USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'institutions', @level2type=N'COLUMN',@level2name=N'rpt_format'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'institutions', @level2type=N'COLUMN',@level2name=N'dcm_file_xfer_pacs_mode'

GO
/****** Object:  Table [dbo].[institutions]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institutions]
GO
/****** Object:  Table [dbo].[institutions]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[institutions](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](5) NULL,
	[name] [nvarchar](100) NOT NULL,
	[address_1] [nvarchar](100) NULL,
	[address_2] [nvarchar](100) NULL,
	[city] [nvarchar](100) NULL,
	[state_id] [int] NULL CONSTRAINT [DF_institutions_state_id]  DEFAULT ((0)),
	[country_id] [int] NULL CONSTRAINT [DF_institutions_country_id]  DEFAULT ((0)),
	[zip] [nvarchar](20) NULL,
	[email_id] [nvarchar](50) NULL,
	[phone_no] [nvarchar](30) NULL,
	[mobile_no] [nvarchar](20) NULL,
	[contact_person_name] [nvarchar](100) NULL,
	[contact_person_mobile] [nvarchar](20) NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_institutions_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[is_new] [char](1) NULL CONSTRAINT [DF_institutions_is_new]  DEFAULT (N'Y'),
	[notes] [nvarchar](250) NULL,
	[discount_per] [decimal](5, 2) NULL,
	[discount_updated_by] [uniqueidentifier] NULL,
	[discount_updated_on] [datetime] NULL,
	[info_source] [nvarchar](50) NULL,
	[business_source_id] [int] NULL CONSTRAINT [DF_institutions_business_source_id]  DEFAULT ((0)),
	[old_code] [nvarchar](5) NULL,
	[accountant_name] [nvarchar](250) NULL,
	[patient_id_srl] [int] NULL CONSTRAINT [DF_institutions_patient_id_srl]  DEFAULT ((0)),
	[is_online] [char](1) NULL CONSTRAINT [DF_institutions_is_online]  DEFAULT ('N'),
	[is_email_verified] [char](1) NULL CONSTRAINT [DF_institutions_is_email_verified]  DEFAULT ('N'),
	[is_mobile_verified] [char](1) NULL CONSTRAINT [DF_institutions_is_mobile_verified]  DEFAULT ('N'),
	[link_existing_bill_acct] [nchar](1) NULL CONSTRAINT [DF_institutions_link_existing_bill_acct]  DEFAULT ('N'),
	[billing_account_id] [uniqueidentifier] NULL CONSTRAINT [DF_institutions_billing_account_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[format_dcm_files] [nchar](1) NULL CONSTRAINT [DF_institutions_format_dcm_files]  DEFAULT ('N'),
	[dcm_file_xfer_pacs_mode] [nchar](1) NULL CONSTRAINT [DF_institutions_dcm_file_xfer_pacs_mode]  DEFAULT ('N'),
	[study_img_manual_receive_path] [nvarchar](250) NULL,
	[consult_applicable] [nchar](1) NULL CONSTRAINT [DF_institutions_consult_applicable]  DEFAULT ('N'),
	[custom_report] [nchar](1) NULL CONSTRAINT [DF_institutions_custom_report]  DEFAULT ('N'),
	[logo_img] [image] NULL,
	[image_content_type] [nvarchar](20) NULL,
	[storage_applicable] [nchar](1) NULL CONSTRAINT [DF_institutions_storage_applicable]  DEFAULT ('N'),
	[xfer_files_compress] [nchar](1) NULL,
	[submitted_by] [nvarchar](100) NULL,
	[img_software_name] [nvarchar](100) NULL,
	[fax_rpt] [nchar](1) NULL,
	[fax_no] [nvarchar](30) NULL CONSTRAINT [DF_institutions_fax_no]  DEFAULT ('N'),
	[rpt_format] [nchar](1) NULL CONSTRAINT [DF_institutions_rpt_format]  DEFAULT ('P'),
 CONSTRAINT [PK_institutions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[A]utomatic,[M]anual,[N]ot registered for DICOM Router' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'institutions', @level2type=N'COLUMN',@level2name=N'dcm_file_xfer_pacs_mode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[P]df,[R]tf,[B]oth' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'institutions', @level2type=N'COLUMN',@level2name=N'rpt_format'
GO
