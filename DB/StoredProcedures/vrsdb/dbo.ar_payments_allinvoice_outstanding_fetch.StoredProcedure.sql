USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_allinvoice_outstanding_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_allinvoice_outstanding_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_allinvoice_outstanding_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_allinvoice_outstanding_fetch : fetch outstanding for all invoices with adjusted amount 0
** Created By   : KC
** Created On   : 11/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_allinvoice_outstanding_fetch]
	--@mode	int=1,
	@billing_account_id		uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @menu_id int,
    @user_id uniqueidentifier= '00000000-0000-0000-0000-000000000000'
    --@error_code nvarchar(10)='' output,
    --@return_status int = 0 output
as
begin
	 set nocount on

	--if(@mode=1) -- by user_id
	--	select a.id, a.invoice_no, a.invoice_date, a.total_amount, sum(a.adjusted) adjusted, a.total_amount-sum(a.adjusted) balance, cast(0 as bit) selected  from (
	--		select 'O' adj_source, hdr.id,hdr.invoice_no,hdr.opbal_date invoice_date,hdr.opbal_amount total_amount, isnull(aj.adj_amount,0) adjusted 
	--		from ar_opening_balance hdr with(nolock) 
	--		left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and aj.adj_source='O'
	--		left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
	--		where ba.login_user_id = @user_id
	--		UNION ALL
	--		select 'I' adj_source, hdr.id,hdr.invoice_no,hdr.invoice_date,hdr.total_amount, isnull(aj.adj_amount,0) adjusted from invoice_hdr hdr with(nolock) 
	--		left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and ISNULL(aj.adj_source,'I')='I'
	--		left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
	--		where ba.login_user_id = @user_id
	--		and hdr.approved='Y'  
	--	) a
	--	group by a.id, a.invoice_no, a.invoice_date, a.total_amount
	--	order by a.invoice_date desc;
	--else -- by billing account id
		select a.id, a.invoice_no, a.invoice_date, a.total_amount, a.billing_cycle_id,a.billing_cycle_name,
		sum(a.adjusted) adjusted,
		sum(a.refunded) refunded,
		a.total_amount-sum(a.adjusted+a.refunded) balance,  
		cast(0 as bit) selected  
		from (
			select 'O' adj_source, hdr.id,hdr.invoice_no,hdr.opbal_date invoice_date,
				    billing_cycle_id='00000000-0000-0000-0000-000000000000',billing_cycle_name = '',
			       hdr.opbal_amount total_amount, 
				   --isnull(aj.adj_amount,0) adjusted,
				   case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end adjusted, 
				   case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded 
			from ar_opening_balance hdr with(nolock) 
			left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and aj.adj_source='O'
			inner join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
			where hdr.billing_account_id = @billing_account_id
			UNION ALL
			select 'I' adj_source, hdr.id,hdr.invoice_no,hdr.invoice_date,
			       hdr.billing_cycle_id,billing_cycle_name=bc.name,
			       hdr.total_amount, 
				   --isnull(aj.adj_amount,0) adjusted,
				   case when aj.ar_refunds_id is null then isnull(aj.adj_amount,0) else 0.00 end adjusted, 
				   case when aj.ar_refunds_id is not null then isnull(aj.adj_amount,0) else 0.00 end refunded 
			from invoice_hdr hdr with(nolock) 
			left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and ISNULL(aj.adj_source,'I')='I'
			inner join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
			inner join billing_cycle bc on bc.id = hdr.billing_cycle_id
			where hdr.billing_account_id = @billing_account_id
			and hdr.approved='Y'
			  
		) a
		group by a.id, a.invoice_no, a.invoice_date, a.billing_cycle_id,a.billing_cycle_name,a.total_amount
		order by a.invoice_date desc;
	set nocount off
end

GO
