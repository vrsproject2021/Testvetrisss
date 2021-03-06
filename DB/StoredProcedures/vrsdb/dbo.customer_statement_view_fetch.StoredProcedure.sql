USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[customer_statement_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[customer_statement_view_fetch]
GO
/****** Object:  StoredProcedure [dbo].[customer_statement_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : customer statment drildown
** Created By   : K.C. 
** Created On   : 07/05/2020
*******************************************************/
--exec customer_statement_view_fetch '49F20F70-4B3D-482C-8BA7-59FDDB2D9463'
CREATE procedure [dbo].[customer_statement_view_fetch]
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
	begin
		set nocount on

		IF OBJECT_ID('tempdb.dbo.#T1', 'U') IS NOT NULL
			DROP TABLE #T1; 
		IF OBJECT_ID('tempdb.dbo.#T2', 'U') IS NOT NULL
			DROP TABLE #T2; 
		IF OBJECT_ID('tempdb.dbo.#T3', 'U') IS NOT NULL
			DROP TABLE #T3; 

		-- total applicable records 
		select * into #T1 from (
			select hdr.billing_account_id, hdr.id invoice_id,hdr.invoice_no,hdr.opbal_date invoice_date,hdr.opbal_amount total_amount, pmt.id payment_id,ref.id refund_id, pmt.payref_date, pmt.payref_no,pmt.payment_mode ,ref.refundref_date, ref.refundref_no, isnull(aj.adj_amount,0) adjusted 
				from ar_opening_balance hdr with(nolock) 
				left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and aj.adj_source='O'
				left join ar_payments pmt with(nolock) on pmt.id=aj.ar_payments_id --and aj.adj_source='O'
				left join ar_refunds ref with(nolock) on ref.id=aj.ar_refunds_id
				where hdr.billing_account_id = case when @billing_account_id='00000000-0000-0000-0000-000000000000' then hdr.billing_account_id else @billing_account_id end  
					  and isnull(hdr.invoice_no,'')<>''
					  and hdr.opbal_amount>0
				union all
			select hdr.billing_account_id, hdr.id invoice_id,hdr.invoice_no,hdr.invoice_date,hdr.total_amount, pmt.id payment_id,ref.id refund_id, pmt.payref_date, pmt.payref_no,pmt.payment_mode ,ref.refundref_date, ref.refundref_no, isnull(aj.adj_amount,0) adjusted 
				from invoice_hdr hdr with(nolock) 
				left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id
				left join ar_payments pmt with(nolock) on pmt.id=aj.ar_payments_id
				left join ar_refunds ref with(nolock) on ref.id=aj.ar_refunds_id
				left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
				where hdr.billing_account_id = case when @billing_account_id='00000000-0000-0000-0000-000000000000' then hdr.billing_account_id else @billing_account_id end 
				  and hdr.approved='Y'
				  and hdr.total_amount>0
		) a;

	

		-- records having balance outstanding
		select a.billing_account_id,b.name, sum(a.total_amount-a.total_adj) balance into #T2 from (
			select billing_account_id, invoice_id, total_amount, sum(adjusted) total_adj from #T1 
			--where adjusted>0 --added by pavel 14/07/2021
			group by billing_account_id, invoice_id, total_amount
		) a
		inner join billing_account b with(nolock) on a.billing_account_id=b.id
		group by a.billing_account_id, b.name;
		
		select a.billing_account_id,b.name,a.balance from #T2 a
			inner join billing_account b with(nolock) on b.id=a.billing_account_id
		order by a.balance desc,b.name

		-- invoice having balance outstanding
		select  a.billing_account_id,a.invoice_id, a.invoice_no, a.invoice_date, a.total_amount, sum(a.total_amount-a.adjusted) balance into #T3 from (
			select billing_account_id,invoice_id, invoice_no, invoice_date, total_amount, sum(adjusted) adjusted from #T1
			--where adjusted>0 --added by pavel 14/07/2021
			group by billing_account_id,invoice_id, invoice_no, invoice_date, total_amount
		) a
		
		group by a.billing_account_id,a.invoice_id, a.invoice_no, a.invoice_date,a.total_amount
		having sum(a.total_amount-a.adjusted)>0
		order by a.invoice_date, invoice_no

		select ROW_NUMBER() over(order by a.billing_account_id, a.invoice_date, a.invoice_no) id, a.* from #T3 a
		
		-- payment adjustments only for invoices having balance outstanding
		select ROW_NUMBER() over(order by a.billing_account_id, a.invoice_no, a.payref_date, a.payref_no) id, a.billing_account_id, a.invoice_id, a.payment_id, case when a.payment_mode=1 then 'Online' else 'Offline' end mode, isnull(a.refundref_date,a.payref_date) payref_date, a.payref_no,a.refundref_no, a.adjusted from #T1 a
		inner join #T3 b on b.invoice_id=a.invoice_id
		where payment_id is not null
		
		set nocount off

	end

GO
