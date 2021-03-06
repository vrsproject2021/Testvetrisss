USE [vrsdb]
GO
/****** Object:  Table [dbo].[case_notification_rule_dtls]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[case_notification_rule_dtls]
GO
/****** Object:  Table [dbo].[case_notification_rule_dtls]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[case_notification_rule_dtls](
	[rule_no] [int] NOT NULL,
	[user_role_id] [int] NOT NULL,
	[scheduled] [nchar](1) NULL CONSTRAINT [DF_case_notification_rule_dtls_scheduled]  DEFAULT ('N'),
	[notify_all] [nchar](1) NULL CONSTRAINT [DF_case_notification_rule_dtls_notify_all]  DEFAULT ('N'),
	[user_id] [uniqueidentifier] NULL CONSTRAINT [DF_case_notification_rule_dtls_user_id]  DEFAULT ('00000000-0000-0000-0000-000000000000')
) ON [PRIMARY]

GO
