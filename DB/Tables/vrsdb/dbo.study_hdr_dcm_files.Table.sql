USE [vrsdb]
GO
ALTER TABLE [dbo].[study_hdr_dcm_files] DROP CONSTRAINT [DF_study_hdr_dcm_files_xfer]
GO
/****** Object:  Table [dbo].[study_hdr_dcm_files]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[study_hdr_dcm_files]
GO
/****** Object:  Table [dbo].[study_hdr_dcm_files]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[study_hdr_dcm_files](
	[dcm_file_id] [uniqueidentifier] NOT NULL,
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[dcm_file_srl_no] [int] NOT NULL,
	[dcm_file_name] [nvarchar](250) NOT NULL,
	[dcm_file] [varbinary](max) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[xfer] [nchar](1) NULL,
 CONSTRAINT [PK_study_hdr_dcm_files] PRIMARY KEY CLUSTERED 
(
	[dcm_file_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[study_hdr_dcm_files] ADD  CONSTRAINT [DF_study_hdr_dcm_files_xfer]  DEFAULT ('N') FOR [xfer]
GO
