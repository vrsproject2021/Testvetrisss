USE [vrslogdb]
GO
/****** Object:  Table [dbo].[sys_dicom_router_log]    Script Date: 20-08-2021 20:49:16 ******/
DROP TABLE [dbo].[sys_dicom_router_log]
GO
/****** Object:  Table [dbo].[sys_dicom_router_log]    Script Date: 20-08-2021 20:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sys_dicom_router_log](
	[institution_id] [uniqueidentifier] NOT NULL,
	[institution_code] [nvarchar](5) NOT NULL,
	[institution_name] [nvarchar](100) NOT NULL,
	[service_id] [int] NOT NULL,
	[service_name] [nvarchar](100) NOT NULL,
	[log_date] [datetime] NOT NULL,
	[log_message] [varchar](max) NULL,
	[is_error] [bit] NULL,
	[date_synched] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
