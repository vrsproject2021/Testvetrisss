USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_adjustments_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_adjustments_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_adjustments_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_adjustments_fetch : fetch ar_payments with adjustments by payment id
** Created By   : KC
** Created On   : 23/07/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_adjustments_fetch]
	@id		uniqueidentifier=null
as
begin
	set nocount on

	select 
		adj.invoice_header_id id, 
		adj.invoice_no,
		adj.invoice_date, 
		pmt.payref_no, 
		pmt.payref_date, 
		pmt.processing_ref_no, 
		pmt.processing_ref_date, 
		pmt.processing_pg_name, 
		pmt.processing_status, 
		pmt.payment_mode,
		pmt.payment_amount,
		sum(adj.adj_amount) adj_amount
	from ar_payments_adj adj with(nolock) 
	inner join ar_payments pmt with(nolock) on pmt.id=adj.ar_payments_id 
	inner join billing_account ba on ba.id = pmt.billing_account_id
	inner join users u with(nolock) on pmt.created_by=u.id
	where pmt.id=@id
	group by adj.invoice_header_id, adj.invoice_no, adj.invoice_date,pmt.payref_no,	pmt.payref_date, pmt.processing_ref_no, 
		pmt.processing_ref_date,pmt.processing_pg_name, pmt.processing_status,pmt.payment_mode,	pmt.payment_amount
	order by adj.invoice_date,adj.invoice_no
		
	set nocount off
end

GO
