USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_pacs_query_fields]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_pacs_query_fields]
GO
/****** Object:  Table [dbo].[sys_pacs_query_fields]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_pacs_query_fields](
	[field_code] [nvarchar](10) NOT NULL,
	[field_desc] [nvarchar](30) NOT NULL,
	[query] [nchar](1) NULL CONSTRAINT [DF_sys_pacs_query_fields_query]  DEFAULT (N'Y'),
	[service_id] [int] NOT NULL,
	[display_index] [int] NULL,
	[is_study_type_field] [nchar](1) NULL,
 CONSTRAINT [PK_sys_pacs_query_fields] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC,
	[field_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
