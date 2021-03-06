USE [vrslogdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sms_log', @level2type=N'COLUMN',@level2name=N'sms_type'

GO
/****** Object:  Table [dbo].[sms_log]    Script Date: 20-08-2021 20:49:16 ******/
DROP TABLE [dbo].[sms_log]
GO
/****** Object:  Table [dbo].[sms_log]    Script Date: 20-08-2021 20:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sms_log](
	[sms_log_id] [uniqueidentifier] NOT NULL,
	[sms_log_datetime] [datetime] NOT NULL,
	[recipient_no] [nvarchar](100) NOT NULL,
	[recipient_name] [nvarchar](100) NULL,
	[sender_no] [nvarchar](100) NULL,
	[sequence_no] [int] NULL,
	[sms_type] [nvarchar](10) NULL,
	[sms_text] [nvarchar](max) NULL,
	[study_hdr_id] [uniqueidentifier] NULL CONSTRAINT [DF_sms_log_study_hdr_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[study_uid] [nvarchar](100) NULL,
	[creation_rule_no] [int] NULL CONSTRAINT [DF_sms_log_creation_rule_no]  DEFAULT ((0)),
	[release_sms] [nchar](1) NULL CONSTRAINT [DF_sms_log_release_sms]  DEFAULT ('N'),
	[for_sms_verification] [char](1) NULL CONSTRAINT [DF_sms_log_for_sms_verification]  DEFAULT ('N'),
	[attempts] [int] NULL CONSTRAINT [DF_sms_log_attempts]  DEFAULT ((0)),
	[sms_processed] [nchar](1) NULL CONSTRAINT [DF_sms_log_sms_processed]  DEFAULT ('N'),
	[sms_processed_time] [datetime] NULL,
	[message_sid] [nvarchar](200) NULL,
	[date_updated] [datetime] NULL,
	[updated_by] [uniqueidentifier] NULL,
 CONSTRAINT [PK_sms_log] PRIMARY KEY CLUSTERED 
(
	[sms_log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[RPT]Report sending,[NRULE] Radiologist notification rule,[RADAUTOASN] Auto assignment of radiologist' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sms_log', @level2type=N'COLUMN',@level2name=N'sms_type'
GO
