USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_register_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_register_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_register_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************  
*******************************************************  
** Version  : 1.0.0.0  
** Procedure    : ar_payments_register : fetch ar_payments  
** Created By   : KC  
** Created On   : 14/05/2020  
*******************************************************/  
--exec ar_payments_register_fetch '01jun2020','30jun2020',null
CREATE procedure [dbo].[ar_payments_register_fetch]  
 @from_date  date,  
 @to_date  date,  
 @payment_mode           nvarchar(1) = NULL  
as  
begin  
  set nocount on  
  IF OBJECT_ID('tempdb.dbo.#TP1', 'U') IS NOT NULL  
  DROP TABLE #TP1;   
  IF OBJECT_ID('tempdb.dbo.#TP2', 'U') IS NOT NULL  
  DROP TABLE #TP2;   
  
  select * into #TP1 from (  
  select   
   ap.id,   
   ap.billing_account_id,   
   case when ap.payment_mode='0' then 'OFFLINE' else 'ONLINE' end payment_mode_name,   
   ap.payref_no,   
   ap.payref_date,   
   ap.processing_ref_no,   
   ap.processing_ref_date,   
   ap.processing_pg_name,   
   ap.auth_code,   
   ap.cvv_response,   
   ap.avs_response,   
   ba.name billing_account_name,  
   ap.payment_amount,  
   ap.date_created  
  from ar_payments ap with(nolock)   
  inner join billing_account ba with(nolock) on ap.billing_account_id=ba.id  
  where ap.payment_mode = case when ISNULL(@payment_mode,'A')='A' then ap.payment_mode else @payment_mode end  
  and convert(date, payref_date) >= @from_date and convert(date, payref_date) <= @to_date  
  and ap.processing_status='1'  
  ) a  
 
    
  select * into #TP2 from (  
   select adj.id as adj_id,adj.ar_payments_id,adj.invoice_no,adj.invoice_date,ar.refundref_no,adj.adj_amount from ar_payments_adj adj with(nolock) 
   left join ar_refunds ar with(nolock) on ar.id=adj.ar_refunds_id
   inner join #TP1 p on  p.id=adj.ar_payments_id
   --where adj.adj_amount>0
  ) b;  
  
  select * from #TP1 a order by a.id  
  select ROW_NUMBER() over(order by b.ar_payments_id, b.invoice_date, b.invoice_no) id, b.* from #TP2 b  
  
 set nocount off  
  
   
end
GO
