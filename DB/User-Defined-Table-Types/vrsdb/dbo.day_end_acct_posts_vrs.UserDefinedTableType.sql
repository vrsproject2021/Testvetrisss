USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[day_end_acct_posts_vrs]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[day_end_acct_posts_vrs]
GO
/****** Object:  UserDefinedTableType [dbo].[day_end_acct_posts_vrs]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[day_end_acct_posts_vrs] AS TABLE(
	[srl_no] [int] NOT NULL,
	[gl_code] [nvarchar](5) NOT NULL,
	[dr_amount] [money] NULL,
	[cr_amount] [money] NULL
)
GO
