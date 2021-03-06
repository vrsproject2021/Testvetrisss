USE [vrsdb]
GO
/****** Object:  Table [dbo].[promo_reasons]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[promo_reasons]
GO
/****** Object:  Table [dbo].[promo_reasons]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[promo_reasons](
	[id] [uniqueidentifier] NOT NULL,
	[reason] [nvarchar](250) NOT NULL,
	[is_active] [nchar](1) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_promo_reasons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
