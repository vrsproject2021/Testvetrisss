USE [vrsdb]
GO
/****** Object:  Table [dbo].[institution_alt_name_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institution_alt_name_link]
GO
/****** Object:  Table [dbo].[institution_alt_name_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[institution_alt_name_link](
	[institution_id] [uniqueidentifier] NOT NULL,
	[alternate_name] [nvarchar](200) NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL
) ON [PRIMARY]

GO
