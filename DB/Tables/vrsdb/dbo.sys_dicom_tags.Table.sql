USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_dicom_tags]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_dicom_tags]
GO
/****** Object:  Table [dbo].[sys_dicom_tags]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_dicom_tags](
	[group_id] [nvarchar](5) NOT NULL,
	[element_id] [nvarchar](5) NOT NULL,
	[tag_desc] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_sys_dicom_tags] PRIMARY KEY CLUSTERED 
(
	[group_id] ASC,
	[element_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
