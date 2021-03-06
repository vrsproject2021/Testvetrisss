USE [vrsdb]
GO
/****** Object:  Table [dbo].[service_gl_code_link]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[service_gl_code_link]
GO
/****** Object:  Table [dbo].[service_gl_code_link]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[service_gl_code_link](
	[service_id] [int] NOT NULL,
	[modality_id] [int] NOT NULL,
	[gl_code_default] [nvarchar](5) NULL,
	[gl_code_after_hrs] [nvarchar](5) NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_service_gl_code_link] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC,
	[modality_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
