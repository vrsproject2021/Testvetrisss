USE [vrslogdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'fax_log', @level2type=N'COLUMN',@level2name=N'fax_type'

GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'fax_log', @level2type=N'COLUMN',@level2name=N'report_type'

GO
/****** Object:  Table [dbo].[fax_log]    Script Date: 20-08-2021 20:49:16 ******/
DROP TABLE [dbo].[fax_log]
GO
/****** Object:  Table [dbo].[fax_log]    Script Date: 20-08-2021 20:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[fax_log](
	[id] [uniqueidentifier] NOT NULL,
	[log_datetime] [datetime] NOT NULL,
	[recipient_no] [nvarchar](100) NOT NULL,
	[study_hdr_id] [uniqueidentifier] NULL CONSTRAINT [DF_fax_log_study_hdr_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[study_uid] [nvarchar](100) NULL,
	[file_name] [nvarchar](max) NULL,
	[institution_id] [uniqueidentifier] NULL CONSTRAINT [DF_fax_log_institution_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[report_type] [nchar](1) NULL,
	[custom_report] [nchar](1) NULL,
	[fax_sent] [nchar](1) NULL CONSTRAINT [DF_fax_log_fax_sent]  DEFAULT ('N'),
	[fax_sent_time] [datetime] NULL,
	[date_updated] [datetime] NULL,
	[fax_type] [nvarchar](10) NULL,
	[release_fax] [nchar](1) NULL CONSTRAINT [DF_fax_log_release_fax]  DEFAULT ('N'),
 CONSTRAINT [PK_fax_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[P]reliminary,[F]inal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'fax_log', @level2type=N'COLUMN',@level2name=N'report_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[RPT]Report sending' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'fax_log', @level2type=N'COLUMN',@level2name=N'fax_type'
GO
