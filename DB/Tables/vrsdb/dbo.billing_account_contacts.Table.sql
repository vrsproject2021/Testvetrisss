USE [vrsdb]
GO
/****** Object:  Table [dbo].[billing_account_contacts]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[billing_account_contacts]
GO
/****** Object:  Table [dbo].[billing_account_contacts]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[billing_account_contacts](
	[billing_account_id] [uniqueidentifier] NOT NULL,
	[institution_id] [uniqueidentifier] NOT NULL,
	[phone_no] [nvarchar](30) NULL,
	[fax_no] [nvarchar](20) NULL,
	[contact_person_name] [nvarchar](100) NULL,
	[contact_person_mobile] [nvarchar](20) NULL,
	[contact_person_email_id] [nvarchar](50) NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK_billing_account_contacts] PRIMARY KEY CLUSTERED 
(
	[billing_account_id] ASC,
	[institution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
