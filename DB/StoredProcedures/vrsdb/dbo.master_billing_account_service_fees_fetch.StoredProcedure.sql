USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_service_fees_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_service_fees_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_service_fees_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_service_fees_fetch : fetch 
                  billing account service fees schedule
** Created By   : Pavel Guha
** Created On   : 26/02/2021
*******************************************************/
--exec master_billing_account_service_fees_fetch '1BC396CD-E25B-4C81-8354-0D05F5743089','N'
CREATE procedure [dbo].[master_billing_account_service_fees_fetch]
    @id uniqueidentifier,
	@apply_default nchar(1)='N'
as
begin
	 set nocount on

	  create table #tmp
	 (
		row_id int identity(1,1),
		rate_id uniqueidentifier,
		service_id int,
		modality_id int,
		service_name nvarchar(50),
		modality_name nvarchar(50),
		invoice_by nchar(1),
		invoice_by_desc nvarchar(20),
		default_count_from int ,
		default_count_to int ,
		fee_amount money,
		fee_amount_after_hrs money
	 )

	if(@apply_default='N')
		begin
			insert into #tmp(rate_id,service_id,modality_id,service_name,modality_name,invoice_by,invoice_by_desc,default_count_from,default_count_to,
			                 fee_amount,fee_amount_after_hrs)
			(select basfs.rate_id,sfst.service_id,sfst.modality_id,service_name=s.name,modality_name=isnull(m.name,''),sfst.invoice_by,
					case 
						when sfst.invoice_by='B' then 'Body Part'
						when sfst.invoice_by='I' then 'Image'
						when sfst.invoice_by='M' then 'Minute'
					end invoice_by_desc,
					sfst.default_count_from,sfst.default_count_to,
			        basfs.fee_amount, 
					basfs.fee_amount_after_hrs
			from billing_account_service_fee_schedule basfs
			inner join ar_service_fee_schedule_template sfst on sfst.id = basfs.rate_id
			inner join services s on s.id = sfst.service_id
			left outer join modality m on m.id = sfst.modality_id
			where basfs.billing_account_id=@id
			and sfst.deleted='N'
			union
			select rate_id = sfst.id,sfst.service_id,sfst.modality_id,service_name= s.name,modality_name=isnull(m.name,''),sfst.invoice_by,
					case 
						when sfst.invoice_by='B' then 'Body Part'
						when sfst.invoice_by='I' then 'Image'
						when sfst.invoice_by='M' then 'Minute'
					end invoice_by_desc,
					sfst.default_count_from,sfst.default_count_to,
				   fee_amount = 0,
				   fee_amount_after_hrs = 0
			from ar_service_fee_schedule_template sfst
			inner join services s on s.id = sfst.service_id
			left outer join modality m on m.id = sfst.modality_id
			where sfst.deleted='N'
			and sfst.id not in (select rate_id from billing_account_service_fee_schedule where billing_account_id=@id))
			order by service_name,modality_name,default_count_from,default_count_to
		end
	else
		begin
			insert into #tmp(rate_id,service_id,modality_id,service_name,modality_name,invoice_by,invoice_by_desc,default_count_from,default_count_to,
			                 fee_amount,fee_amount_after_hrs)
			(select rate_id = sfst.id,sfst.service_id,sfst.modality_id,service_name= s.name,modality_name=isnull(m.name,''),sfst.invoice_by,
					case 
						when sfst.invoice_by='B' then 'Body Part'
						when sfst.invoice_by='I' then 'Image'
						when sfst.invoice_by='M' then 'Minute'
					end invoice_by_desc,
					sfst.default_count_from,sfst.default_count_to,
				    fee_amount = isnull(fee_amount,0),
				    fee_amount_after_hrs = isnull(fee_amount_after_hrs,0)
			from ar_service_fee_schedule_template sfst
			inner join services s on s.id = sfst.service_id
			left outer join modality m on m.id = sfst.modality_id
			where sfst.deleted='N')
			order by service_name,modality_name,default_count_from,default_count_to
		end

	
	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
