USE [vrslogdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'email_log', @level2type=N'COLUMN',@level2name=N'email_type'

GO
/****** Object:  Table [dbo].[email_log]    Script Date: 20-08-2021 20:49:16 ******/
DROP TABLE [dbo].[email_log]
GO
/****** Object:  Table [dbo].[email_log]    Script Date: 20-08-2021 20:49:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[email_log](
	[email_log_id] [uniqueidentifier] NOT NULL,
	[email_log_datetime] [datetime] NOT NULL,
	[recipient_address] [nvarchar](500) NULL,
	[recipient_name] [nvarchar](100) NULL,
	[cc_address] [varchar](max) NULL CONSTRAINT [DF_email_log_cc_address]  DEFAULT (''),
	[sender_address] [nvarchar](100) NULL,
	[sender_name] [nvarchar](100) NULL,
	[email_subject] [varchar](250) NULL,
	[email_text] [ntext] NOT NULL,
	[email_type] [nvarchar](10) NULL CONSTRAINT [DF_email_log_email_type]  DEFAULT ('RPT'),
	[study_hdr_id] [uniqueidentifier] NULL CONSTRAINT [DF_email_log_study_hdr_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[study_uid] [nvarchar](100) NULL,
	[creation_rule_no] [int] NULL CONSTRAINT [DF_email_log_creation_rule_no]  DEFAULT ((0)),
	[file_name] [nvarchar](4000) NULL,
	[invoice_hdr_id] [uniqueidentifier] NULL CONSTRAINT [DF_email_log_invoice_hdr_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[institution_name] [nvarchar](100) NULL,
	[release_email] [nchar](1) NULL CONSTRAINT [DF_email_log_release_email]  DEFAULT ('N'),
	[import_session_id] [nvarchar](30) NULL,
	[sender_email_address] [nvarchar](100) NULL,
	[sender_email_password] [nvarchar](100) NULL,
	[for_email_verification] [char](1) NULL CONSTRAINT [DF_email_log_for_email_verification]  DEFAULT ('N'),
	[email_processed] [nchar](1) NULL CONSTRAINT [DF_email_log_email_processed]  DEFAULT ('N'),
	[email_processed_time] [datetime] NULL,
	[date_updated] [datetime] NULL,
	[updated_by] [uniqueidentifier] NULL,
 CONSTRAINT [PK_email_log] PRIMARY KEY CLUSTERED 
(
	[email_log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[RPT]Report sending,[SVCSTART]Service Restart,[XFERFAIL] Transfer of dm files failed,[NSSF] Failed to synch study from PACS,[NRULE] Radiologist notification rule,[OPSCSS] Online payment successful,[ORSCSS] Online refund successful,[TEMP] Temporary mailing,[ACCTINV] Mail Account Invoice,[CONFBA] Configure Billing Account,[DR] Dicom Router,[DROT] Dicom Router Over Time,[NEWREG] New Registration Mail,[NEWINST] New Institution Create,[MGSYRDASN] default radiologist assignment for merged study,[MRGREQ] Merging required,[SYFWDND] Study file forwarding not done,[UNASNSY] Unassigned study,[ASNREL] Study assignment released' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'email_log', @level2type=N'COLUMN',@level2name=N'email_type'
GO
