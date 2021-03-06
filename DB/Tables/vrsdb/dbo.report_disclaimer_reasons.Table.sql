USE [vrsdb]
GO
/****** Object:  Table [dbo].[report_disclaimer_reasons]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[report_disclaimer_reasons]
GO
/****** Object:  Table [dbo].[report_disclaimer_reasons]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[report_disclaimer_reasons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [nvarchar](30) NOT NULL,
	[description] [ntext] NULL,
	[is_active] [nchar](1) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_report_disclaimer_reasons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
