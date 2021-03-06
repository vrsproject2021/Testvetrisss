USE [vrsarchive20db]
GO
/****** Object:  Table [dbo].[study_hdr_study_types_archive]    Script Date: 21-09-2021 17:15:31 ******/
DROP TABLE [dbo].[study_hdr_study_types_archive]
GO
/****** Object:  Table [dbo].[study_hdr_study_types_archive]    Script Date: 21-09-2021 17:15:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[study_hdr_study_types_archive](
	[study_hdr_id] [uniqueidentifier] NOT NULL,
	[study_type_id] [uniqueidentifier] NOT NULL,
	[srl_no] [int] NOT NULL,
	[write_back_tag] [nvarchar](5) NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_study_hdr_study_types_archive] PRIMARY KEY CLUSTERED 
(
	[study_hdr_id] ASC,
	[study_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
