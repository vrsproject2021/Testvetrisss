USE [vrsdb]
GO
/****** Object:  UserDefinedTableType [dbo].[day_end_acct_posts]    Script Date: 20-08-2021 20:58:05 ******/
DROP TYPE [dbo].[day_end_acct_posts]
GO
/****** Object:  UserDefinedTableType [dbo].[day_end_acct_posts]    Script Date: 20-08-2021 20:58:05 ******/
CREATE TYPE [dbo].[day_end_acct_posts] AS TABLE(
	[srl_no] [int] NOT NULL,
	[ref_no] [nvarchar](20) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_modified] [datetime] NULL,
	[date_txn] [datetime] NOT NULL,
	[txn_id] [nvarchar](30) NOT NULL,
	[gl_code] [nvarchar](5) NOT NULL,
	[dr_amount] [money] NULL,
	[cr_amount] [money] NULL,
	[dr_cr_name] [nvarchar](100) NULL
)
GO
