USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_invoice_balance_age]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_invoice_balance_age]
GO
/****** Object:  StoredProcedure [dbo].[ar_invoice_balance_age]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_invoice_balance_age
** Created By   : KC
** Created On   : 17/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_invoice_balance_age]
(
	@billing_account_id		uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@user_id				uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @menu_id                int,
	@error_code				nvarchar(10)		= '' output,
    @return_status			int					= 0  output
)
as
	begin
		if object_id('tempdb.dbo.#t_data', 'U') is not null
			drop table #t_data;
		if object_id('tempdb.dbo.#t_data2', 'U') is not null
			drop table #t_data2;

		select 
			ROW_NUMBER() over(
						partition by b.billing_account_id
						order by b.invoice_date) line,b.line rline,
			b.billing_account_id, b.id, b.invoice_no, b.invoice_date,b.total_amount,b.paydate,case b.payment_mode when '1' then 'Online' when '0' then 'Offline' else null end payment_mode, b.payref_no,
			b.adjusted, case when b.line=b.last_line then b.total_amount-isnull(b.total_adjusted,0) else null end balance, 
			case when b.line=b.last_line then DATEDIFF(d, b.invoice_date, GETDATE()) else b.age end age
		into #t_data
		from (
			select a.billing_account_id, a.id, a.invoice_no, a.invoice_date, a.total_amount,a.paydate, a.payment_mode, a.payref_no, a.adjusted,a.age,
						ROW_NUMBER() over(
						partition by a.billing_account_id, a.id, a.invoice_no, a.invoice_date, a.total_amount
						order by a.invoice_date desc) line,
						sum(1) over(partition by a.billing_account_id, a.id, a.invoice_no, a.invoice_date, a.total_amount) last_line,
						sum(a.adjusted) over(partition by a.billing_account_id, a.id, a.invoice_no, a.invoice_date, a.total_amount) total_adjusted
						from (
			select hdr.billing_account_id, hdr.id,hdr.invoice_no,convert(date,hdr.invoice_date) invoice_date,hdr.total_amount, 
					null paydate,null payment_mode, null payref_no, null adjusted, null age from invoice_hdr hdr with(nolock)
					where hdr.invoice_no is not null AND hdr.billing_account_id = case when ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000' then hdr.billing_account_id else @billing_account_id end
			union
			select hdr.billing_account_id, hdr.id,hdr.invoice_no,convert(date,hdr.invoice_date) invoice_date,hdr.total_amount, 
					convert(date,isnull(pmt.payref_date, getdate())) paydate, pmt.payment_mode, pmt.payref_no, aj.adj_amount adjusted, DATEDIFF(d,convert(date,hdr.invoice_date), convert(date,isnull(pmt.payref_date, getdate()))) age from invoice_hdr hdr with(nolock) 
					inner join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id
					inner join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
					inner join ar_payments pmt with(nolock) on aj.ar_payments_id=pmt.id
					where hdr.invoice_no is not null AND ba.id = case when ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000' then ba.id else @billing_account_id end
			) a
		) b;

		select ROW_NUMBER() over(order by c.name, d.line) id,
			d.rline line, 
			case when line=1 then c.name else null end name,  
			d.billing_account_id, 
			case when (d.rline=1 or summary=1) then d.invoice_no else null end invoice_no, 
			case when d.rline=1  then d.invoice_date else null end invoice_date,
			case when d.rline=1  then d.total_amount else null end total_amount,
			d.paydate,
			d.payment_mode,d.payref_no,d.adjusted,d.balance,d.age, d.summary
			into #t_data2
			from (
			select a.line, a.rline, a.billing_account_id,a.id,a.invoice_no,a.invoice_date,a.total_amount,a.paydate,a.payment_mode,a.payref_no,a.adjusted,a.balance,a.age, 0 summary from #t_data a 
			inner join (select distinct id from #t_data where balance>0) b on b.id=a.id
			union
			select a.line, a.line+a.rline rline,a.billing_account_id, cast(null as uniqueidentifier) id, 'Total:' invoice_no, cast(null as datetime) invoice_date, cast(null as decimal(18,3)) total_amount,cast(null as datetime)  paydate, null payment_mode, null payref_no, cast(null as decimal(18,3))  adjusted, a.balance,cast(null as smallint)  age, 1 summary  from (
				select  max(t.line)+1 line, sum(rline) rline, t.billing_account_id, sum(t.balance) balance  from #t_data t where t.balance>0 group by t.billing_account_id
			) a
		) d
		inner join billing_account c with(nolock) on c.id = d.billing_account_id
		where c.is_active = 'Y' and c.id = case when ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000' then c.id else @billing_account_id end;

		insert into #t_data2(id,line,billing_account_id,invoice_no,balance,summary)
		select a.id, a.line, '00000000-0000-0000-0000-000000000000' billing_account_id, 'Grand Total:' invoice_no, a.balance, 1 summary  from (
				select max(t.id)+1 id, max(t.line)+1 line, sum(isnull(t.balance,0)) balance  from #t_data2 t where t.balance>0 and t.summary=1
		) a;

		select * from #t_data2 order by id;

		set @return_status=1
		set @error_code='034'
		set nocount off

		return 1
	end
GO
