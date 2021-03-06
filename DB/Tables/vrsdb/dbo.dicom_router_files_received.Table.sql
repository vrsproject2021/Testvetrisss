USE [vrsdb]
GO
/****** Object:  Table [dbo].[dicom_router_files_received]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[dicom_router_files_received]
GO
/****** Object:  Table [dbo].[dicom_router_files_received]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dicom_router_files_received](
	[study_uid] [nvarchar](100) NOT NULL,
	[import_session_id] [nvarchar](30) NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[institution_code] [nvarchar](5) NOT NULL,
	[file_name] [nvarchar](250) NOT NULL,
	[file_type] [nchar](1) NOT NULL,
	[file_series_uid] [nvarchar](100) NOT NULL,
	[file_instance_no] [nvarchar](100) NOT NULL,
	[date_received] [datetime] NOT NULL
) ON [PRIMARY]

GO
