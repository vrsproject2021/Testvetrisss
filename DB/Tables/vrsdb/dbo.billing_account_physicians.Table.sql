USE [vrsdb]
GO
/****** Object:  Table [dbo].[billing_account_physicians]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[billing_account_physicians]
GO
/****** Object:  Table [dbo].[billing_account_physicians]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[billing_account_physicians](
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[physician_id] [uniqueidentifier] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_billing_account_physicians] PRIMARY KEY CLUSTERED 
(
	[billing_account_id] ASC,
	[institution_id] ASC,
	[physician_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
