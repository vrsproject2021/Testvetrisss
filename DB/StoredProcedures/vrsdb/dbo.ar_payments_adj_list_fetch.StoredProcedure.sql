USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_adj_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_adj_list_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_adj_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_adj_list_fetch : fetch ar_payments with adjustments by invoice id
** Created By   : KC
** Created On   : 11/04/2020
*******************************************************/
--exec [dbo].[ar_payments_adj_list_fetch] @invoice_id='9bbe199a-7f17-4b77-9c2e-9f19a5f00acd',@menu_id=24,@user_id='fb56cd18-9a9d-45af-8689-3d3a17cc3b68'
CREATE procedure [dbo].[ar_payments_adj_list_fetch]
	@invoice_id		uniqueidentifier=null,
	@payment_mode           nvarchar(1) = NULL,
	@processing_status      nvarchar(1) = NULL,
    @menu_id int,
    @user_id uniqueidentifier
as
begin
	set nocount on

	select 
		adj.invoice_header_id id, 
		adj.invoice_no,
		adj.invoice_date,
		ISNULL(ref.refund_mode, pmt.payment_mode) payment_mode, 
		case when ISNULL(ref.refund_mode, pmt.payment_mode)='0' then 'OFFLINE' else 'ONLINE' end payment_mode_name, 
		pmt.payref_no,
		ref.refundref_no,
		ISNULL(ref.refundref_date, pmt.payref_date) payref_date, 
		ISNULL(ref.processing_ref_no, pmt.processing_ref_no) processing_ref_no, 
		ISNULL(ref.processing_ref_date, pmt.processing_ref_date) processing_ref_date, 
		ISNULL(ref.processing_status, pmt.processing_status) processing_status, 
		case when ISNULL(ref.refund_mode, pmt.payment_mode)='0' then 'Pass' else (case when ISNULL(ref.processing_status, pmt.processing_status)='1' then 'Pass' else 'Failed' end) end processing_status_name, 
		pmt.payment_amount,
		adj.adj_amount,
		pmt.created_by, 
		pmt.created_by user_id, 
		ISNULL(ur.name, u.name) user_name, 
		ISNULL(ref.date_created, pmt.date_created) date_created, 
		pmt.updated_by, 
		pmt.date_updated,
		ba.name billing_account_name, 
		ba.code billing_account_code
		
	from ar_payments_adj adj with(nolock) 
	inner join ar_payments pmt with(nolock) on pmt.id=adj.ar_payments_id 
	left join ar_refunds ref with(nolock) on ref.id=adj.ar_refunds_id 
	inner join billing_account ba on ba.id = pmt.billing_account_id
	inner join users u with(nolock) on pmt.created_by=u.id
	left join users ur with(nolock) on ref.created_by=ur.id
	where adj.invoice_header_id=@invoice_id
		--and pmt.processing_status='1'
	order by pmt.id desc, adj.invoice_header_id desc 
		
	set nocount off
end

GO
