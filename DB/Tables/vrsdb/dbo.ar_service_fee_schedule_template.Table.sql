USE [vrsdb]
GO
/****** Object:  Table [dbo].[ar_service_fee_schedule_template]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[ar_service_fee_schedule_template]
GO
/****** Object:  Table [dbo].[ar_service_fee_schedule_template]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ar_service_fee_schedule_template](
	[id] [uniqueidentifier] NOT NULL,
	[service_id] [int] NOT NULL,
	[modality_id] [int] NOT NULL,
	[invoice_by] [nchar](1) NOT NULL,
	[default_count_from] [int] NOT NULL CONSTRAINT [DF_ar_service_fee_schedule_template_default_count_from]  DEFAULT ((0)),
	[default_count_to] [int] NOT NULL CONSTRAINT [DF_ar_service_fee_schedule_template_default_count_to]  DEFAULT ((0)),
	[fee_amount] [money] NULL CONSTRAINT [DF_ar_service_fee_schedule_template_fee_amount]  DEFAULT ((0)),
	[fee_amount_after_hrs] [money] NULL CONSTRAINT [DF_ar_service_fee_schedule_template_fee_amount_after_hrs]  DEFAULT ((0)),
	[gl_code] [nvarchar](5) NULL,
	[deleted] [nchar](1) NULL CONSTRAINT [DF_ar_service_fee_schedule_template_deleted]  DEFAULT ('N'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[deleted_by] [uniqueidentifier] NULL,
	[date_deleted] [datetime] NULL,
 CONSTRAINT [PK_ar_service_fee_schedule_template] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC,
	[modality_id] ASC,
	[default_count_from] ASC,
	[default_count_to] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
