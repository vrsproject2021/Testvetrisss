USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_fees]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_fetch_fees]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_fees]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_fetch_fees : fetch 
                  billing account fees schedule
** Created By   : Pavel Guha
** Created On   : 22/10/2019
*******************************************************/
--exec master_billing_account_fetch_fees '00000000-0000-0000-0000-000000000000'
create procedure [dbo].[master_billing_account_fetch_fees]
    @id uniqueidentifier
as
begin
	 set nocount on

	  create table #tmp
	 (
		row_id int identity(1,1),
		rate_id uniqueidentifier,
		head_id int,
		head_type nvarchar(20),
		head_name nvarchar(50),
		img_count_from int ,
		img_count_to int ,
		fee_amount money,
		ba_fee_amount money null default 0
	 )


	insert into #tmp(rate_id,head_id,head_type,head_name,img_count_from,img_count_to,fee_amount,ba_fee_amount)
	(select barfs.rate_id,rfst.head_id,
	        case 
				when rfst.head_type='M' then 'Modality'
				when rfst.head_type='S' then 'Service'
			end head_type,
			case 
				when rfst.head_type='M' then (select name from modality where id = rfst.head_id)
				when rfst.head_type='S' then (select name from services where id = rfst.head_id)
			end head_name,
			rfst.img_count_from,rfst.img_count_to,rfst.fee_amount, ba_fee_amount = barfs.fee_amount
	from billing_account_rates_fee_schedule barfs
	inner join rates_fee_schedule_template rfst on rfst.id = barfs.rate_id
	where barfs.billing_account_id=@id
	and rfst.deleted='N'
	union
	select rate_id = id,head_id,
		  case 
				when head_type='M' then 'Modality'
				when head_type='S' then 'Service'
		   end head_type,
		   case
				when head_type='M' then (select name from modality where id=rates_fee_schedule_template.head_id)
				when head_type='S' then (select name from services where id=rates_fee_schedule_template.head_id)
		   end head_name,
	       
		   img_count_from = isnull(img_count_from,0),
		   img_count_to = isnull(img_count_to,0),
		   fee_amount = isnull(fee_amount,0),
		   ba_fee_amount = 0
	from rates_fee_schedule_template
	where deleted='N'
	and id not in (select rate_id from billing_account_rates_fee_schedule where billing_account_id=@id))
	order by head_type,head_name

	
	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
