USE [vrsdb]
GO
/****** Object:  Table [dbo].[ar_non_revenue_acct_control]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_non_revenue_acct_control]
GO
/****** Object:  Table [dbo].[ar_non_revenue_acct_control]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_non_revenue_acct_control](
	[control_code] [nvarchar](10) NOT NULL,
	[control_desc] [nvarchar](50) NOT NULL,
	[gl_code] [nvarchar](30) NOT NULL,
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_ar_non_revenue_acct_control] PRIMARY KEY CLUSTERED 
(
	[control_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
