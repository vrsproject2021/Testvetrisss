USE [vrsdb]
GO
/****** Object:  Table [dbo].[sys_us_time_zones]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[sys_us_time_zones]
GO
/****** Object:  Table [dbo].[sys_us_time_zones]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_us_time_zones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[standard_name] [nvarchar](100) NOT NULL,
	[gmt_diff] [decimal](5, 2) NULL,
	[is_default] [nchar](1) NULL,
	[gmt_diff_mins] [int] NULL,
 CONSTRAINT [PK_sys_us_time_zones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
