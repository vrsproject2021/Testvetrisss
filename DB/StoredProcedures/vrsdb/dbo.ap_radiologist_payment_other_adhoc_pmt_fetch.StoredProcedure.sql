USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_other_adhoc_pmt_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_radiologist_payment_other_adhoc_pmt_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_other_adhoc_pmt_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_radiologist_payment_other_adhoc_pmt_fetch : 
                  fetch radiologist adhoc payment  details
** Created By   : Pavel Guha
** Created On   : 05/11/2020
*******************************************************/
create procedure [dbo].[ap_radiologist_payment_other_adhoc_pmt_fetch]
    @radiologist_id uniqueidentifier,
	@billing_cycle_id uniqueidentifier
as
	begin
		set nocount on
		
		select id=oap.head_id,aph.name,oap.adhoc_payment,remarks = isnull(oap.remarks,'')
		from ap_radiologist_other_adhoc_payments oap
		inner join ap_adhoc_payment_heads aph on aph.id = oap.head_id
		where oap.radiologist_id=@radiologist_id
		and oap.billing_cycle_id=@billing_cycle_id
		union
		select id,name,adhoc_payment=0,remarks = ''
		from ap_adhoc_payment_heads
		where is_active='Y'
		and id not in (select head_id from ap_radiologist_other_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id)
		order by name

		set nocount off

	end
GO
