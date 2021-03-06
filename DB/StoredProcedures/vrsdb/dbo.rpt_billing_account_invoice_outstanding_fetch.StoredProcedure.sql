USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_billing_account_invoice_outstanding_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_billing_account_invoice_outstanding_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_billing_account_invoice_outstanding_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_billing_account_invoice_outstanding_fetch : fetch outstanding invoices
** Created By   : Pavel Guha
** Created On   : 23 May 2020
*******************************************************/
--exec rpt_billing_account_invoice_outstanding_fetch '3225FD47-3FF5-4863-8E89-22E960263EB3','98A30426-BA37-4564-98D0-A2CF6A4B9929'
--exec rpt_billing_account_invoice_outstanding_fetch 'A2AE6F39-1AE6-4963-A8DD-36C0AD387FB5','1C42DA26-3B41-458D-B638-8C4792BB6D46'
CREATE procedure [dbo].[rpt_billing_account_invoice_outstanding_fetch]
    @billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier
as
begin
	 set nocount on

	 declare @start_date datetime

	 create table #tmp
	 (
	    id uniqueidentifier,
		invoice_no nvarchar(50),
		invoice_date datetime ,
		billing_cycle_id uniqueidentifier,
		billing_cycle nvarchar(30),
		total_amount money,
		adjusted money,
		balance money
	 )

	 select  @start_date= date_from from billing_cycle where id=@billing_cycle_id

	--invoice outstanding
	insert into #tmp(id,invoice_no,invoice_date,billing_cycle_id,billing_cycle,total_amount,adjusted,balance)
	(select a.id, 
		   a.invoice_no, 
		   a.invoice_date, 
		   a.billing_cycle_id,
		   a.billing_cycle,
		   a.total_amount, 
		   adjusted = sum(a.adjusted) , 
		   balance  =  a.total_amount-sum(a.adjusted) 
	from (
			select 'O' adj_source, hdr.id,hdr.invoice_no,hdr.opbal_date invoice_date,billing_cycle_id='00000000-0000-0000-0000-000000000000',billing_cycle='Opening Balance',hdr.opbal_amount total_amount, isnull(aj.adj_amount,0) adjusted 
			from ar_opening_balance hdr with(nolock) 
			left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id 
			left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
			where ba.id = @billing_account_id
			UNION ALL
			select 'I' adj_source, hdr.id,hdr.invoice_no,hdr.invoice_date,hdr.billing_cycle_id,billing_cycle=bc.name,hdr.total_amount, isnull(aj.adj_amount,0) adjusted 
			from invoice_hdr hdr with(nolock) 
			left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and ISNULL(aj.adj_source,'I')='I'
			left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
			inner join billing_cycle bc on bc.id = hdr.billing_cycle_id
			where ba.id = @billing_account_id
			and hdr.billing_cycle_id <> @billing_cycle_id
			and isnull(aj.adj_amount,0)>=0
			and hdr.approved='Y'	
	) a
	group by a.id, a.invoice_no, a.invoice_date,a.billing_cycle_id,a.billing_cycle,a.total_amount
	having round(a.total_amount-sum(a.adjusted),2)>0)
	order by a.invoice_date

	if(@@rowcount=0)
		begin
			insert into #tmp(id,invoice_no,invoice_date,total_amount,adjusted,balance)
			          values('00000000-0000-0000-0000-000000000000','-','01jan1900',0,0,0)
		end

	delete from #tmp
	where billing_cycle_id <> '00000000-0000-0000-0000-000000000000'
	and billing_cycle_id in (select id from billing_cycle where date_from > @start_date)

	select * from #tmp
	drop table #tmp
	
	
	set nocount off
end

GO
