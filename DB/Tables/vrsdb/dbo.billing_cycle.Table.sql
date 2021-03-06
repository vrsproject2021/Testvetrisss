USE [vrsdb]
GO
/****** Object:  Table [dbo].[billing_cycle]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[billing_cycle]
GO
/****** Object:  Table [dbo].[billing_cycle]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[billing_cycle](
	[id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](30) NOT NULL,
	[date_from] [datetime] NOT NULL,
	[date_till] [datetime] NOT NULL,
	[locked] [nchar](1) NULL CONSTRAINT [DF_billing_cycle_locked]  DEFAULT ('N'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[archived] [nchar](1) NULL,
	[arch_db_name] [nvarchar](30) NULL,
 CONSTRAINT [PK_billing_cycle] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
