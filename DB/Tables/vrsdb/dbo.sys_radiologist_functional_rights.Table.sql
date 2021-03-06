USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_radiologist_functional_rights]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_radiologist_functional_rights]
GO
/****** Object:  Table [dbo].[sys_radiologist_functional_rights]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_radiologist_functional_rights](
	[right_code] [nvarchar](20) NOT NULL,
	[right_desc] [nvarchar](100) NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_sys_radiologist_functional_rights] PRIMARY KEY CLUSTERED 
(
	[right_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
