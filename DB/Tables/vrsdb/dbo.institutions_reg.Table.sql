USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'institutions_reg', @level2type=N'COLUMN',@level2name=N'preferred_pmt_method'

GO
/****** Object:  Table [dbo].[institutions_reg]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[institutions_reg]
GO
/****** Object:  Table [dbo].[institutions_reg]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[institutions_reg](
	[id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](5) NULL,
	[name] [nvarchar](100) NOT NULL,
	[address_1] [nvarchar](100) NULL,
	[address_2] [nvarchar](100) NULL,
	[city] [nvarchar](100) NULL,
	[state_id] [int] NULL,
	[country_id] [int] NULL,
	[zip] [nvarchar](20) NULL,
	[email_id] [nvarchar](50) NULL,
	[phone_no] [nvarchar](30) NULL,
	[mobile_no] [nvarchar](20) NULL,
	[contact_person_name] [nvarchar](100) NULL,
	[contact_person_mobile] [nvarchar](20) NULL,
	[login_id] [nvarchar](20) NOT NULL,
	[login_password] [nvarchar](200) NOT NULL,
	[login_email_id] [nvarchar](100) NULL,
	[login_mobile_no] [nvarchar](20) NULL,
	[is_email_verified] [char](1) NULL,
	[is_mobile_verified] [char](1) NULL,
	[preferred_pmt_method] [nvarchar](5) NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_institutions_reg] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[CK]Check,[CC]Credit Card,[OP]Online Payment,[MI] Mail Invoice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'institutions_reg', @level2type=N'COLUMN',@level2name=N'preferred_pmt_method'
GO
