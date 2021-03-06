USE [vrsdb]
GO
/****** Object:  Table [dbo].[case_notification_rules]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[case_notification_rules]
GO
/****** Object:  Table [dbo].[case_notification_rules]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[case_notification_rules](
	[rule_no] [int] NOT NULL,
	[rule_desc] [nvarchar](500) NOT NULL,
	[pacs_status_id] [int] NOT NULL,
	[priority_id] [int] NOT NULL,
	[time_ellapsed_mins] [int] NULL CONSTRAINT [DF_case_notification_rules_time_ellapsed_mins]  DEFAULT ((0)),
	[default_user_role] [nvarchar](10) NOT NULL,
	[second_level_alert_receipient_id] [uniqueidentifier] NULL CONSTRAINT [DF_case_notification_rules_second_level_alert_receipient_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[is_active] [char](1) NULL,
 CONSTRAINT [PK_case_notification_rules] PRIMARY KEY CLUSTERED 
(
	[rule_no] ASC,
	[pacs_status_id] ASC,
	[priority_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
