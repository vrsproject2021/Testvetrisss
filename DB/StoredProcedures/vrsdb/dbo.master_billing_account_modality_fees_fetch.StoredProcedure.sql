USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_modality_fees_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_modality_fees_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_modality_fees_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_modality_fees_fetch : fetch 
                  billing account fees schedule
** Created By   : Pavel Guha
** Created On   : 26/02/2021
*******************************************************/
--exec master_billing_account_modality_fees_fetch '1BC396CD-E25B-4C81-8354-0D05F5743089','Y'
CREATE procedure [dbo].[master_billing_account_modality_fees_fetch]
    @id uniqueidentifier,
	@apply_default nchar(1)='N'
as
begin
	 set nocount on

	  create table #tmp
	 (
		row_id int identity(1,1),
		rate_id uniqueidentifier,
		category_id int,
		modality_id int,
		category_name nvarchar(50),
		modality_name nvarchar(50),
		invoice_by nchar(1),
		invoice_by_desc nvarchar(20),
		default_count_from int ,
		default_count_to int ,
		fee_amount money,
		fee_amount_per_unit money,
		study_max_amount money
	 )

	if(@apply_default='N')
		begin
			insert into #tmp(rate_id,category_id,modality_id,category_name,modality_name,invoice_by,invoice_by_desc,default_count_from,default_count_to,
							 fee_amount,fee_amount_per_unit,study_max_amount)
			(select bamfs.rate_id,mfst.category_id,mfst.modality_id,category=c.name,modality=m.name,mfst.invoice_by,
					case 
						when mfst.invoice_by='B' then 'Body Part'
						when mfst.invoice_by='I' then 'Image'
						when mfst.invoice_by='M' then 'Minute'
					end invoice_by_desc,
					mfst.default_count_from,mfst.default_count_to,bamfs.fee_amount, bamfs.fee_amount_per_unit,bamfs.study_max_amount
			from billing_account_modality_fee_schedule bamfs
			inner join ar_modality_fee_schedule_template mfst on mfst.id = bamfs.rate_id
			inner join sys_study_category c on c.id = mfst.category_id
			inner join modality m on m.id = mfst.modality_id
			where bamfs.billing_account_id=@id
			and mfst.deleted='N'
			union
			select rate_id = mfst.id,mfst.category_id,mfst.modality_id,category_name= c.name,modality_name=m.name,mfst.invoice_by,
				   case 
						when mfst.invoice_by='B' then 'Body Part'
						when mfst.invoice_by='I' then 'Image'
						when mfst.invoice_by='M' then 'Minute'
					end invoice_by_desc,
				   default_count_from  = isnull(mfst.default_count_from,0),
				   default_count_to    = isnull(mfst.default_count_to,0),
				   fee_amount          = 0,
				   fee_amount_per_unit = 0,
				   study_max_amount    = 0
			from ar_modality_fee_schedule_template mfst
			inner join sys_study_category c on c.id = mfst.category_id
			inner join modality m on m.id = mfst.modality_id
			where mfst.deleted='N'
			and mfst.id not in (select rate_id from billing_account_modality_fee_schedule where billing_account_id=@id))
			order by c.name,m.name
		end
	else
		begin
			insert into #tmp(rate_id,category_id,modality_id,category_name,modality_name,invoice_by,invoice_by_desc,default_count_from,default_count_to,
							 fee_amount,fee_amount_per_unit,study_max_amount)
		    (select rate_id = mfst.id,mfst.category_id,mfst.modality_id,category_name= c.name,modality_name=m.name,mfst.invoice_by,
				   case 
						when mfst.invoice_by='B' then 'Body Part'
						when mfst.invoice_by='I' then 'Image'
						when mfst.invoice_by='M' then 'Minute'
					end invoice_by_desc,
				   default_count_from  = isnull(mfst.default_count_from,0),
				   default_count_to    = isnull(mfst.default_count_to,0),
				   fee_amount          = isnull(mfst.fee_amount,0),
				   fee_amount_per_unit = isnull(mfst.fee_amount_per_unit,0),
				   study_max_amount    = isnull(mfst.study_max_amount,0)
			from ar_modality_fee_schedule_template mfst
			inner join sys_study_category c on c.id = mfst.category_id
			inner join modality m on m.id = mfst.modality_id
			where mfst.deleted='N')
			order by c.name,m.name
		end

	
	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
