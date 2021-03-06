USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_dicom_router_online_status]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_dicom_router_online_status]
GO
/****** Object:  Table [dbo].[sys_dicom_router_online_status]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_dicom_router_online_status](
	[institution_id] [uniqueidentifier] NOT NULL,
	[version_no] [nvarchar](50) NOT NULL,
	[last_updated_on] [datetime] NOT NULL,
 CONSTRAINT [PK_sys_dicom_router_online_status] PRIMARY KEY CLUSTERED 
(
	[institution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
