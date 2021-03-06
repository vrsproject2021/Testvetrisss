USE [vrsdb]
GO
/****** Object:  Table [dbo].[radiologists]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[radiologists]
GO
/****** Object:  Table [dbo].[radiologists]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[radiologists](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](10) NULL,
	[fname] [nvarchar](80) NULL,
	[lname] [nvarchar](80) NULL,
	[name] [nvarchar](200) NOT NULL,
	[address_1] [nvarchar](100) NULL,
	[address_2] [nvarchar](100) NULL,
	[city] [nvarchar](100) NULL,
	[state_id] [int] NULL CONSTRAINT [DF_radiologists_state_id]  DEFAULT ((0)),
	[country_id] [int] NULL CONSTRAINT [DF_radiologists_country_id]  DEFAULT ((0)),
	[zip] [nvarchar](20) NULL,
	[email_id] [nvarchar](50) NOT NULL,
	[phone_no] [nvarchar](30) NULL,
	[mobile_no] [nvarchar](20) NULL,
	[login_user_id] [uniqueidentifier] NULL,
	[login_id] [nvarchar](50) NULL,
	[login_pwd] [nvarchar](200) NULL,
	[reader_type_prelim] [nchar](1) NULL,
	[reader_type_final] [nchar](1) NULL,
	[default_fee_prelim] [money] NULL CONSTRAINT [DF_radiologists_default_fee_prelim]  DEFAULT ((0)),
	[default_fee_final] [money] NULL CONSTRAINT [DF_radiologists_default_fee_final]  DEFAULT ((0)),
	[is_active] [nchar](1) NULL CONSTRAINT [DF_radiologists_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[credentials] [nvarchar](30) NULL,
	[granted_rights_pacs] [nvarchar](30) NULL,
	[updated_in_pacs] [nvarchar](1) NULL DEFAULT ('N'),
	[date_updated_in_pacs] [datetime] NULL,
	[identity_color] [nvarchar](10) NULL,
	[notification_pref] [char](1) NULL CONSTRAINT [DF_radiologists_notification_pref]  DEFAULT ('B'),
	[signage] [ntext] NULL,
	[schedule_view] [nchar](1) NULL CONSTRAINT [DF_radiologists_schedule_view]  DEFAULT ('O'),
	[notes] [ntext] NULL,
	[transcription_required] [nchar](1) NULL CONSTRAINT [DF_radiologists_transcription_required]  DEFAULT ('N'),
	[acct_group_id] [int] NULL,
	[creditor_id] [nvarchar](20) NULL,
	[update_qb] [nchar](1) NULL,
	[update_qb_on] [datetime] NULL,
	[qb_name] [nvarchar](200) NULL,
	[assign_merged_study] [nchar](1) NULL CONSTRAINT [DF_radiologists_assign_merged_study]  DEFAULT ('N'),
	[timezone_id] [int] NULL CONSTRAINT [DF_radiologists_timezone_id]  DEFAULT ((0)),
	[max_wu_per_hr] [int] NULL CONSTRAINT [DF_radiologists_max_wu_per_shift]  DEFAULT ((0)),
	[pacs_user_id] [nvarchar](50) NULL,
	[pacs_user_pwd] [nvarchar](200) NULL,
 CONSTRAINT [PK_radiologists] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
