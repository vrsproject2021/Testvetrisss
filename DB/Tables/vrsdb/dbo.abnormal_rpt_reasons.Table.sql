USE [vrsdb]
GO
/****** Object:  Table [dbo].[abnormal_rpt_reasons]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[abnormal_rpt_reasons]
GO
/****** Object:  Table [dbo].[abnormal_rpt_reasons]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[abnormal_rpt_reasons](
	[id] [uniqueidentifier] NOT NULL,
	[reason] [nvarchar](250) NOT NULL,
	[is_active] [nchar](1) NULL CONSTRAINT [DF_abnormal_rpt_reasons_is_active]  DEFAULT ('N'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_abnormal_rpt_reasons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
