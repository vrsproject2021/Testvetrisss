USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_dispute_dicom_tags]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_dispute_dicom_tags]
GO
/****** Object:  Table [dbo].[institution_dispute_dicom_tags]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_dispute_dicom_tags](
	[institution_id] [uniqueidentifier] NOT NULL,
	[group_id] [nvarchar](5) NOT NULL,
	[element_id] [nvarchar](5) NOT NULL,
	[default_value] [nvarchar](250) NULL,
	[junk_characters] [nvarchar](100) NULL,
	[updated_by] [uniqueidentifier] NOT NULL,
	[date_updated] [datetime] NOT NULL,
 CONSTRAINT [PK_institution_dispute_dicom_tags] PRIMARY KEY CLUSTERED 
(
	[institution_id] ASC,
	[group_id] ASC,
	[element_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
