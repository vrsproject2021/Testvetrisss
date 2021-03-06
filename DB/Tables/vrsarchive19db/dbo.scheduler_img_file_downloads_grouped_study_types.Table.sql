USE [vrsarchive19db]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_grouped_study_types]    Script Date: 21-09-2021 17:14:12 ******/
DROP TABLE [dbo].[scheduler_img_file_downloads_grouped_study_types]
GO
/****** Object:  Table [dbo].[scheduler_img_file_downloads_grouped_study_types]    Script Date: 21-09-2021 17:14:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_img_file_downloads_grouped_study_types](
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_type_id] [uniqueidentifier] NOT NULL,
	[srl_no] [int] NOT NULL,
	[write_back_tag] [nvarchar](5) NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_scheduler_img_file_downloads_grouped_study_types] PRIMARY KEY CLUSTERED 
(
	[study_hdr_id] ASC,
	[study_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
