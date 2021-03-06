USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_device_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_device_link]
GO
/****** Object:  Table [dbo].[institution_device_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_device_link](
	[device_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[manufacturer] [nvarchar](200) NOT NULL,
	[model] [nvarchar](200) NULL,
	[serial_no] [nvarchar](20) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[modality_ae_title] [nvarchar](50) NULL,
	[modality] [nvarchar](30) NULL,
	[weight_uom] [nvarchar](10) NULL,
 CONSTRAINT [PK_institution_device_link] PRIMARY KEY CLUSTERED 
(
	[device_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
