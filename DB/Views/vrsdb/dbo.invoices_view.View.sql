USE [vrsdb]
GO
/****** Object:  View [dbo].[invoices_view]    Script Date: 20-08-2021 20:59:58 ******/
DROP VIEW [dbo].[invoices_view]
GO
/****** Object:  View [dbo].[invoices_view]    Script Date: 20-08-2021 20:59:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[invoices_view] AS 	select a.id,a.billing_cycle_id,a.billing_cycle,a.billing_account_id,a.billing_account, a.invoice_no, a.invoice_date, a.total_amount, 
	sum(a.adjusted) adjusted_amount,
	sum(a.refunded) refunded_amount,
	a.total_amount-sum(a.adjusted+a.refunded) balance_amount
	from (
			select 'O' adj_source, hdr.id,hdr.billing_account_id,hdr.invoice_no,hdr.opbal_date invoice_date,
			       billing_cycle_id='00000000-0000-0000-0000-000000000000',billing_cycle = '',
			       hdr.opbal_amount total_amount, 
				   case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end adjusted, 
				   case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded,
				   ba.name billing_account
			from ar_opening_balance hdr with(nolock) 
			left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id
			left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
			UNION ALL
			select 'I' adj_source, hdr.id,hdr.billing_account_id,hdr.invoice_no,hdr.invoice_date,
					hdr.billing_cycle_id,billing_cycle=bc.name,
			        hdr.total_amount, 
				   case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end adjusted, 
				   case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded,
				   ba.name billing_account
			from invoice_hdr hdr with(nolock) 
			left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id
			left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
			inner join billing_cycle bc on bc.id = hdr.billing_cycle_id
			and hdr.approved='Y'	
	) a
	group by a.id,a.billing_cycle_id,a.billing_cycle,a.billing_account_id,a.billing_account, a.invoice_no, a.invoice_date,a.total_amount
	
;
GO
