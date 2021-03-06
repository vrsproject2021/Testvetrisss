USE [vrsdb]
GO
/****** Object:  Table [dbo].[scheduler_checked_missing_session_files]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[scheduler_checked_missing_session_files]
GO
/****** Object:  Table [dbo].[scheduler_checked_missing_session_files]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[scheduler_checked_missing_session_files](
	[file_name] [nvarchar](250) NULL,
	[institution_code] [nvarchar](5) NULL,
	[institution_name] [nvarchar](100) NULL,
	[study_uid] [nvarchar](100) NULL,
	[sent_to_pacs] [nchar](1) NULL,
	[date_checked] [datetime] NULL
) ON [PRIMARY]

GO
