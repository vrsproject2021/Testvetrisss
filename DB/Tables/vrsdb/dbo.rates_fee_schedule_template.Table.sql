USE [vrsdb]
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'rates_fee_schedule_template', @level2type=N'COLUMN',@level2name=N'head_type'

GO
/****** Object:  Table [dbo].[rates_fee_schedule_template]    Script Date: 21-09-2021 17:12:53 ******/
DROP TABLE [dbo].[rates_fee_schedule_template]
GO
/****** Object:  Table [dbo].[rates_fee_schedule_template]    Script Date: 21-09-2021 17:12:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rates_fee_schedule_template](
	[id] [uniqueidentifier] NOT NULL,
	[head_id] [int] NOT NULL,
	[head_type] [nchar](1) NOT NULL,
	[img_count_from] [int] NULL CONSTRAINT [DF_rates_fee_schedule_template_img_count_from]  DEFAULT ((0)),
	[img_count_to] [int] NULL CONSTRAINT [DF_rates_fee_schedule_template_img_count_to]  DEFAULT ((0)),
	[fee_amount] [money] NULL,
	[deleted] [nchar](1) NULL CONSTRAINT [DF_rates_fee_schedule_template_deleted]  DEFAULT ('N'),
	[created_by] [uniqueidentifier] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[updated_by] [uniqueidentifier] NULL,
	[date_updated] [datetime] NULL,
	[deleted_by] [uniqueidentifier] NULL,
	[date_deleted] [datetime] NULL,
 CONSTRAINT [PK_rates_fee_schedule_template] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'(M)odality,(S)ervice' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'rates_fee_schedule_template', @level2type=N'COLUMN',@level2name=N'head_type'
GO
