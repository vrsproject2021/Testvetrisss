USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_qb_voucher_data_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_qb_voucher_data_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_qb_voucher_data_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_qb_voucher_data_fetch : 
                  fetch voucher records to update in quick books
** Created By   : Pavel Guha
** Created On   : 24/06/2020
*******************************************************/
--exec invoicing_qb_voucher_data_fetch
CREATE procedure [dbo].[invoicing_qb_voucher_data_fetch]
as
begin
	
	set nocount on

	--invoice 
	select top 3
		   ih.billing_account_id, billing_account_name = ba.name,
		   ih.billing_cycle_id,billing_cycle_name = bc.name
	from invoice_hdr ih
	inner join billing_account ba on ba.id = ih.billing_account_id
	inner join billing_cycle bc on bc.id = ih.billing_cycle_id
	where ih.update_qb='Y'
	and ih.approved='Y'
	and ih.total_study_count>0
	and ih.total_amount + ih.total_disc_amount>=0
	order by ba.name

	--invoice (reverse)
	select top 1
		  ih.billing_account_id, billing_account_name = ba.name,
		  ih.billing_cycle_id,billing_cycle_name = bc.name
	from invoice_hdr ih
	inner join billing_account ba on ba.id = ih.billing_account_id
	inner join billing_cycle bc on bc.id = ih.billing_cycle_id
	where ih.update_qb='R'
	order by ba.name

	-- payment
	select top 1
		   arp.id,arp.billing_account_id,billing_account_name = ba.name,
	       arp.payref_no,arp.payref_date
	from ar_payments arp
	inner join billing_account ba on ba.id = arp.billing_account_id
	where arp.post_to_qb='Y'
	and arp.processing_status=1
	order by ba.name,arp.payref_no,arp.payref_date

    -- refund
	select top 1
		   arr.id,arr.billing_account_id,billing_account_name = ba.name,
	       arr.refundref_no,arr.refundref_date
	from ar_refunds arr
	inner join billing_account ba on ba.id = arr.billing_account_id
	where arr.post_to_qb='Y'
	and arr.processing_status=1
	order by ba.name,arr.refundref_no,arr.refundref_date

	set nocount off


end


GO
