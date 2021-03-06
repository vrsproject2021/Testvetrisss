USE [vrsarchive20db]
GO
/****** Object:  Table [dbo].[sys_case_study_status_log]    Script Date: 21-09-2021 17:15:31 ******/
DROP TABLE [dbo].[sys_case_study_status_log]
GO
/****** Object:  Table [dbo].[sys_case_study_status_log]    Script Date: 21-09-2021 17:15:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_case_study_status_log](
	[study_id] [uniqueidentifier] NOT NULL,
	[study_uid] [nvarchar](100) NOT NULL,
	[status_id_from] [int] NULL,
	[status_id_to] [int] NULL,
	[date_updated] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL CONSTRAINT [DF_sys_case_study_status_log_updated_by]  DEFAULT (N'00000000-0000-0000-0000-000000000000')
) ON [PRIMARY]

GO
