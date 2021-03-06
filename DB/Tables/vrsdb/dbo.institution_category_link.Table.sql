USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_category_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_category_link]
GO
/****** Object:  Table [dbo].[institution_category_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_category_link](
	[institution_id] [uniqueidentifier] NOT NULL,
	[category_id] [int] NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_institution_category_link] PRIMARY KEY CLUSTERED 
(
	[institution_id] ASC,
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
