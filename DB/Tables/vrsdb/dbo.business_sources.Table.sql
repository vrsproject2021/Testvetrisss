USE [vrsdb]
GO
/****** Object:  Table [dbo].[business_sources]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[business_sources]
GO
/****** Object:  Table [dbo].[business_sources]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[business_sources](
	[id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_business_sources_is_active]  DEFAULT ('Y'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_business_sources] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
