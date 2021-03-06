USE [vrsdb]
GO
/****** Object:  Table [dbo].[physicians]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[physicians]
GO
/****** Object:  Table [dbo].[physicians]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[physicians](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](10) NULL,
	[fname] [nvarchar](80) NULL,
	[lname] [nvarchar](80) NULL,
	[credentials] [nvarchar](30) NULL,
	[name] [nvarchar](200) NOT NULL,
	[address_1] [nvarchar](100) NULL,
	[address_2] [nvarchar](100) NULL,
	[city] [nvarchar](100) NULL,
	[state_id] [int] NULL CONSTRAINT [DF_physicians_state_id]  DEFAULT ((0)),
	[country_id] [int] NULL CONSTRAINT [DF_physicians_country_id]  DEFAULT ((0)),
	[zip] [nvarchar](20) NULL,
	[email_id] [nvarchar](500) NULL,
	[phone_no] [nvarchar](30) NULL,
	[mobile_no] [nvarchar](500) NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_physicians_is_active]  DEFAULT (N'Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[institution_id] [uniqueidentifier] NULL CONSTRAINT [DF_physicians_institution_id]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
 CONSTRAINT [PK_physicians] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
