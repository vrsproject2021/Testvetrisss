USE [vrsdb]
GO
/****** Object:  Table [dbo].[case_notification_rule_radiologist_dtls]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[case_notification_rule_radiologist_dtls]
GO
/****** Object:  Table [dbo].[case_notification_rule_radiologist_dtls]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[case_notification_rule_radiologist_dtls](
	[rule_no] [int] NOT NULL,
	[radiologist_id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NULL,
	[notify_if_scheduled] [nchar](1) NULL,
	[notify_always] [nchar](1) NULL
) ON [PRIMARY]

GO
