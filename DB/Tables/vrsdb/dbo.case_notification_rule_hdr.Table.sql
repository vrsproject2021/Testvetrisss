USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'case_notification_rule_hdr', @level2type=N'COLUMN',@level2name=N'notify_by_time'

GO
/****** Object:  Table [dbo].[case_notification_rule_hdr]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[case_notification_rule_hdr]
GO
/****** Object:  Table [dbo].[case_notification_rule_hdr]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[case_notification_rule_hdr](
	[rule_no] [int] NOT NULL,
	[rule_desc] [nvarchar](500) NOT NULL,
	[pacs_status_id] [int] NOT NULL,
	[priority_id] [int] NOT NULL,
	[time_ellapsed_mins] [int] NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_case_notification_rule_hdr_is_active]  DEFAULT ('Y'),
	[created_by] [uniqueidentifier] NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[for_sms_verification] [char](1) NULL,
	[time_left_mins] [int] NULL CONSTRAINT [DF_case_notification_rule_hdr_time_left_mins]  DEFAULT ((0)),
	[notify_by_time] [nchar](1) NULL,
 CONSTRAINT [PK_case_notification_rule_hdr] PRIMARY KEY CLUSTERED 
(
	[rule_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[E]llapsed,[L]eft' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'case_notification_rule_hdr', @level2type=N'COLUMN',@level2name=N'notify_by_time'
GO
