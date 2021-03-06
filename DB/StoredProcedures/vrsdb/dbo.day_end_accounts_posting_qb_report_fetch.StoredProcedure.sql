USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_qb_report_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[day_end_accounts_posting_qb_report_fetch]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_qb_report_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : day_end_accounts_posting_qb_report_fetch : 
				  fetch accounts data posting QB end at day end
** Created By   : Pavel Guha
** Created On   : 24/12/2020
*******************************************************/
--exec day_end_accounts_posting_qb_report_fetch '05Jan2021'
CREATE procedure [dbo].[day_end_accounts_posting_qb_report_fetch]
	@day_end_date datetime
as
begin
	set nocount on

	--Vouchers Posted
	create table #tmpData
	(
		rec_id int identity(1,1),
		date_created datetime null,
		ref_no nvarchar(20),
		txn_id nvarchar(30),
		txn_type nvarchar(50),
		txn_ref_no nvarchar(30),
		dr_cr_name nvarchar(100),
		gl_code nvarchar(5),
		gl_desc nvarchar(100),
		dr_amount money,
		cr_amount money,
		initialise_sub_total nchar(1) default 'Y',
		print_sub_total nchar(1) default 'N'
	)
	create table #tmpGLSummary
	(
		gl_code nvarchar(5),
		gl_desc nvarchar(100),
		dr_amount_qb money null default 0,
		cr_amount_qb money null default 0,
		dr_amount_vrs money null default 0,
		cr_amount_vrs money null default 0,
		dr_amount_variance money null default 0,
		cr_amount_variance money null default 0
	)
	create table #tmpGLSummaryInv
	(
		gl_code nvarchar(5),
		gl_desc nvarchar(100),
		dr_amount_qb money null default 0,
		cr_amount_qb money null default 0,
		dr_amount_vrs money null default 0,
		cr_amount_vrs money null default 0,
		dr_amount_variance money null default 0,
		cr_amount_variance money null default 0
	)
	create table #tmpGLSummaryPmt
	(
		gl_code nvarchar(5),
		gl_desc nvarchar(100),
		dr_amount_qb money null default 0,
		cr_amount_qb money null default 0,
		dr_amount_vrs money null default 0,
		cr_amount_vrs money null default 0,
		dr_amount_variance money null default 0,
		cr_amount_variance money null default 0
	)
	--Voucher
	insert into #tmpData(date_created,ref_no,txn_id,txn_type,txn_ref_no,dr_cr_name,gl_code,gl_desc,dr_amount,cr_amount)
	(select date_created,ref_no,txn_id,
		   case
				when txn_type='INV' then 'Invoice'
				when txn_type='INVREV' then 'Invoice Reversal'
				when txn_type='PMTREC' then 'Payment Received'
				when txn_type='PMTREF' then 'Payment Refunded'
				when txn_type='PMTRAD' then 'Paid To Radiologist'
				when txn_type='PMTRADREV' then 'Radiologist Payment Reversed'
				when txn_type='PMTTRS' then 'Paid To Transcriptionist'
				when txn_type='PMTTRSREV' then 'Transcriptionist Payment Reversed'
		   end txn_type,
		   txn_ref_no,dr_cr_name,
		   gl_code,gl_desc,dr_amount,cr_amount
	from day_end_accounts_postings
	where day_end_date=@day_end_date)
	order by date_created

	select case when (select count(rec_id) from #tmpData as t where t.rec_id<#tmpData.rec_id and txn_id=#tmpData.txn_id)>0 then null else date_created end  date_created,
	       case when (select count(rec_id) from #tmpData as t where t.rec_id<#tmpData.rec_id and txn_id=#tmpData.txn_id)>0 then '' else ref_no end  ref_no,
		   case when (select count(rec_id) from #tmpData as t where t.rec_id<#tmpData.rec_id and txn_id=#tmpData.txn_id)>0 then '' else txn_id end txn_id,
		   case when (select count(rec_id) from #tmpData as t where t.rec_id<#tmpData.rec_id and txn_id=#tmpData.txn_id)>0 then '' else txn_type end txn_type,
		   case when (select count(rec_id) from #tmpData as t where t.rec_id<#tmpData.rec_id and txn_id=#tmpData.txn_id)>0 then '' else txn_ref_no end txn_ref_no,
		   case when (select count(rec_id) from #tmpData as t where t.rec_id<#tmpData.rec_id and txn_id=#tmpData.txn_id)>0 then '' else dr_cr_name end dr_cr_name,
		   gl_code,gl_desc,dr_amount,cr_amount,
		   case when (select count(rec_id) from #tmpData as t where t.rec_id<#tmpData.rec_id and txn_id=#tmpData.txn_id)>0 then 'N' else 'Y' end initialise_sub_total,
		   case when (select count(rec_id) from #tmpData as t where t.rec_id>#tmpData.rec_id and txn_id=#tmpData.txn_id)>0 then 'N' else 'Y' end print_sub_total
	from #tmpData

	--Account Summary
	insert into #tmpGLSummary(gl_code,gl_desc,dr_amount_qb,cr_amount_qb)
	(select gl_code,gl_desc,
	       sum(dr_amount),
		   sum(cr_amount)
	from day_end_accounts_postings
	where day_end_date=@day_end_date
	group by gl_code,gl_desc)
	--select * from #tmpGLSummary order by gl_code
	insert into #tmpGLSummary(gl_code,gl_desc)
	(select distinct gl_code,gl_desc 
	 from day_end_vetris_account_posting 
	 where day_end_date=@day_end_date
	 and gl_code not in (select gl_code from #tmpGLSummary))

	 update #tmpGLSummary set dr_amount_vrs= isnull((select sum(dr_amount) from day_end_vetris_account_posting where gl_code = #tmpGLSummary.gl_code and day_end_date=@day_end_date),0)
	 update #tmpGLSummary set cr_amount_vrs= isnull((select sum(cr_amount) from day_end_vetris_account_posting where gl_code = #tmpGLSummary.gl_code and day_end_date=@day_end_date),0)
	 update #tmpGLSummary 
	 set dr_amount_variance = dr_amount_vrs - dr_amount_qb,
	     cr_amount_variance = cr_amount_vrs - cr_amount_qb

	select * from #tmpGLSummary order by gl_code

	--Account Summary (Invoicing)
	insert into #tmpGLSummaryInv(gl_code,gl_desc,dr_amount_qb,cr_amount_qb)
	(select gl_code,gl_desc,
	       sum(dr_amount),
		   sum(cr_amount)
	from day_end_accounts_postings
	where day_end_date=@day_end_date
	and txn_type in ('INV','INVREV')
	group by gl_code,gl_desc)

	insert into #tmpGLSummaryInv(gl_code,gl_desc)
	(select distinct gl_code,gl_desc 
	 from day_end_vetris_account_posting 
	 where day_end_date=@day_end_date
	 and gl_code not in (select gl_code from #tmpGLSummary)
	 and ref_type  in ('INV','INVREV'))

	 update #tmpGLSummaryInv set dr_amount_vrs= isnull((select sum(dr_amount) from day_end_vetris_account_posting where gl_code = #tmpGLSummaryInv.gl_code and day_end_date=@day_end_date and ref_type  in ('INV','INVREV')),0)
	 update #tmpGLSummaryInv set cr_amount_vrs= isnull((select sum(cr_amount) from day_end_vetris_account_posting where gl_code = #tmpGLSummaryInv.gl_code and day_end_date=@day_end_date and ref_type  in ('INV','INVREV')),0)
	 update #tmpGLSummaryInv 
	 set dr_amount_variance = dr_amount_vrs - dr_amount_qb,
	     cr_amount_variance = cr_amount_vrs - cr_amount_qb

	select * from #tmpGLSummaryInv order by gl_code

	--Account Summary (Payments)
	insert into #tmpGLSummaryPmt(gl_code,gl_desc,dr_amount_qb,cr_amount_qb)
	(select gl_code,gl_desc,
	       sum(dr_amount),
		   sum(cr_amount)
	from day_end_accounts_postings
	where day_end_date=@day_end_date
	and txn_type in ('PMTREC','PMTREF')
	group by gl_code,gl_desc)

	insert into #tmpGLSummaryPmt(gl_code,gl_desc)
	(select distinct gl_code,gl_desc 
	 from day_end_vetris_account_posting 
	 where day_end_date=@day_end_date
	 and gl_code not in (select gl_code from #tmpGLSummary)
	 and ref_type  in ('PMTREC','PMTREF'))

	 update #tmpGLSummaryPmt set dr_amount_vrs= isnull((select sum(dr_amount) from day_end_vetris_account_posting where gl_code = #tmpGLSummaryPmt.gl_code and day_end_date=@day_end_date and ref_type  in ('PMTREC','PMTREF')),0)
	 update #tmpGLSummaryPmt set cr_amount_vrs= isnull((select sum(cr_amount) from day_end_vetris_account_posting where gl_code = #tmpGLSummaryPmt.gl_code and day_end_date=@day_end_date and ref_type  in ('PMTREC','PMTREF')),0)
	 update #tmpGLSummaryPmt 
	 set dr_amount_variance = dr_amount_vrs - dr_amount_qb,
	     cr_amount_variance = cr_amount_vrs - cr_amount_qb

	select * from #tmpGLSummaryPmt order by gl_code

	--Invoice Approved
	select  invoice_no     = h.invoice_no,
	        invoice_date   = h.invoice_date,
			amount         = h.total_amount,
			billing_account = ba.name,
			process_date    = h.date_approved
	from invoice_hdr h
	inner join billing_account ba on ba.id=h.billing_account_id
	where h.approved='Y' 
	and convert(datetime,convert(varchar(11),h.date_approved,106)) =@day_end_date
	
	--Invoice Disapproved
	select  ref_no     = h.invoice_no,
	        ref_date   = h.invoice_date,
			amount     = h.total_amount,
			dr_cr_name = ba.name,
			process_date    = h.date_disapproved
	from invoice_hdr h
	inner join billing_account ba on ba.id=h.billing_account_id
	where h.approved='N' 
	and convert(datetime,convert(varchar(11),h.date_disapproved,106)) =@day_end_date
	order by process_date

	--Payment Received
	select  p.payref_no,
	        p.payref_date,
			case
				when p.payment_mode = 1 then 'ONLINE'
				when p.payment_mode = 2 then 'OFFLINE'
			end payment_mode,
			p.processing_ref_no,
			p.processing_ref_date,
			payment_gateway = isnull(p.processing_pg_name,''),
			p.payment_amount,
			billing_account = ba.name
	from ar_payments p
	inner join billing_account ba on ba.id=p.billing_account_id
	where p.processing_status=1
	and convert(datetime,convert(varchar(11),p.payref_date,106)) =@day_end_date

	--Payment Refunded
	select  r.refundref_no,
	        r.refundref_date,
			case
				when r.refund_mode = 1 then 'ONLINE'
				when r.refund_mode = 2 then 'OFFLINE'
			end payment_mode,
			r.processing_ref_no,
			r.processing_ref_date,
			payment_gateway = isnull(r.processing_pg_name,''),
			r.refund_amount,
			billing_account = ba.name
	from ar_refunds r
	inner join billing_account ba on ba.id=r.billing_account_id
	where r.processing_status=1
	and convert(datetime,convert(varchar(11),r.refundref_date,106)) =@day_end_date

	--Payments Made
	select  ref_type   ='Radiologist',
			ref_no     = h.payment_no,
	        ref_date   = h.payment_date,
			amount     = h.total_amount,
			dr_cr_name = r.name
	from ap_radiologist_payment_hdr h
	inner join radiologists r on r.id=h.radiologist_id
	where h.approved='Y'
	and convert(datetime,convert(varchar(11),h.date_approved,106)) =@day_end_date
	union
	select  ref_type   ='Transcriptionist',
			ref_no     = h.payment_no,
	        ref_date   = h.payment_date,
			amount     = h.total_amount,
			dr_cr_name = t.name
	from ap_transcriptionist_payment_hdr h
	inner join radiologists t on t.id=h.transcriptionist_id
	where h.approved='Y'
	and convert(datetime,convert(varchar(11),h.date_approved,106)) =@day_end_date
	order by ref_date


	--Posting Failures
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
	and ar.processing_status=1
	union
	select  ref_type ='Payment Refunded',
			ref_no   = ar.refundref_no,
	        ref_date = ar.refundref_date,
			amount   = ar.refund_amount,
			dr_cr_name = ba.name
	from ar_refunds ar
	inner join billing_account ba on ba.id=ar.billing_account_id
	where ar.post_to_qb='F'
	and ar.processing_status=1
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

	drop table #tmpData
	drop table #tmpGLSummary
	drop table #tmpGLSummaryInv
	drop table #tmpGLSummaryPmt
    

	set nocount off
	
end

GO
