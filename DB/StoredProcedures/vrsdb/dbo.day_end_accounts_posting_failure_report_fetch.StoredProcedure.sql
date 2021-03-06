USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_failure_report_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[day_end_accounts_posting_failure_report_fetch]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_failure_report_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : day_end_accounts_posting_failure_report_fetch : 
				  fetch account wise summary at day end
** Created By   : Pavel Guha
** Created On   : 24/12/2020
*******************************************************/
create procedure [dbo].[day_end_accounts_posting_failure_report_fetch]
	@day_end_date datetime
as
begin
	set nocount on

	select  ref_type   ='Invoice',
			ref_no     = h.invoice_no,
	        ref_date   = h.invoice_date,
			amount     = h.total_amount,
			dr_cr_name = ba.name
	from invoice_hdr h
	inner join billing_account ba on ba.id=h.billing_account_id
	where h.update_qb='F'
	union
	select  ref_type ='Payment Received',
			ref_no   = ar.payref_no,
	        ref_date = ar.payref_date,
			amount   = ar.payment_amount,
			dr_cr_name = ba.name
	from ar_payments ar
	inner join billing_account ba on ba.id=ar.billing_account_id
	where ar.post_to_qb='F'
	union
	select  ref_type ='Payment Refunded',
			ref_no   = ar.refundref_no,
	        ref_date = ar.refundref_date,
			amount   = ar.refund_amount,
			dr_cr_name = ba.name
	from ar_refunds ar
	inner join billing_account ba on ba.id=ar.billing_account_id
	where ar.post_to_qb='F'
	union
	select  ref_type   ='Radiologist Payment',
			ref_no     = h.payment_no,
	        ref_date   = h.payment_date,
			amount     = h.total_amount,
			dr_cr_name = r.name
	from ap_radiologist_payment_hdr h
	inner join radiologists r on r.id=h.radiologist_id
	where h.update_qb='F'
	union
	select  ref_type   ='Transcriptionist Payment',
			ref_no     = h.payment_no,
	        ref_date   = h.payment_date,
			amount     = h.total_amount,
			dr_cr_name = t.name
	from ap_transcriptionist_payment_hdr h
	inner join radiologists t on t.id=h.transcriptionist_id
	where h.update_qb='F'
	order by ref_date

	set nocount off
	
end

GO
