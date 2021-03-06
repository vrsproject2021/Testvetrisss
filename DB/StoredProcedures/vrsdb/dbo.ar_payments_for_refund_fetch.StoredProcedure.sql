USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_for_refund_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_for_refund_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_for_refund_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_for_refund_fetch : fetch ar_payments
** Created By   : KC
** Created On   : 23/07/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_for_refund_fetch]
	@billing_account_id		uniqueidentifier=null,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare 
	         @user_role_id int,
	         @user_role_code nvarchar(10)

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id;

	select
		ap.id,
		ap.billing_account_id, 
		ap.payment_mode, 
		ap.payref_no, 
		ap.payref_date, 
		ap.processing_ref_no, 
		ap.processing_ref_date, 
		ap.processing_pg_name, 
		ap.processing_status, 
		ap.payment_amount,
		ap.remarks,
		a.refunded_amount,
		ap.payment_amount-a.refunded_amount refundable
	from (
	select 
		ap.id, 
		sum(isnull(ar.refund_amount,0.0)) refunded_amount
	
	from ar_payments ap
	left join [dbo].[ar_refunds] ar on ar.ar_payments_id=ap.id
	group by ap.id
	) a
	inner join ar_payments ap on ap.id=a.id
	inner join billing_account ba on ap.billing_account_id=ba.id
		inner join users u on ap.created_by=u.id
	where ap.billing_account_id=case when ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000' then ap.billing_account_id else @billing_account_id end
			  AND ap.payment_mode = '1'
			  AND ap.processing_status = '1'
	order by ap.payref_date desc


	select id,name from billing_account where is_active='Y' order by name
		
	set nocount off
end

GO
