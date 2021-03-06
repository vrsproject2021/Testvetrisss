USE [vrsdb]
GO
/****** Object:  Table [dbo].[study_hdr_merged_studies_archive]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[study_hdr_merged_studies_archive]
GO
/****** Object:  Table [dbo].[study_hdr_merged_studies_archive]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[study_hdr_merged_studies_archive](
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NULL,
	[merge_compare_none] [nchar](1) NULL,
	[image_count] [int] NULL,
	[date_updated] [datetime] NULL,
	[updated_by] [uniqueidentifier] NULL,
 CONSTRAINT [PK_study_hdr_merged_studies_archive] PRIMARY KEY CLUSTERED 
(
	[study_hdr_id] ASC,
	[study_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
