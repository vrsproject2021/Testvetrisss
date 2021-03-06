USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_list_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_list_fetch : fetch ar_payments
** Created By   : KC
** Created On   : 08/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_list_fetch]
	@billing_account_id		uniqueidentifier=null,
	@payment_mode           nvarchar(1) = NULL,
	@processing_status      nvarchar(1) = NULL,
    @menu_id int,
    @user_id uniqueidentifier,
	@from_date  date,  
	@to_date  date,
	@payment_ref nvarchar(100) ='',
    @external_payment_ref nvarchar(100) ='',
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
	where u.id = @user_id
			select 
			ap.id, 
			ap.billing_account_id, 
			ap.payment_mode, 
			case when ap.payment_mode='0' then 'OFFLINE' else 'ONLINE' end payment_mode_name, 
			ap.payref_no, 
			ap.payref_date, 
			ap.processing_ref_no, 
			ap.processing_ref_date, 
			ap.processing_pg_name, 
			ap.processing_status, 
			case when ap.payment_mode='0' then 'Pass' else (case when ap.processing_status='1' then 'Pass' else 'Failed' end) end processing_status_name, 
			ap.payment_amount,
			ap.remarks,
			ap.created_by, 
			ap.created_by user_id, 
			u.name user_name, 
			ap.date_created, 
			ap.updated_by, 
			ap.date_updated,
			ba.name billing_account_name, 
			ba.code billing_account_code
		from ar_payments ap
		inner join billing_account ba on ap.billing_account_id=ba.id
		inner join users u on ap.created_by=u.id
		where ap.billing_account_id=case when ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000' then ap.billing_account_id else @billing_account_id end
			  AND ap.payment_mode = case when ISNULL(@payment_mode,'A')='A' then ap.payment_mode else @payment_mode end
			  AND ap.processing_status = case when ISNULL(@processing_status,'A')='A' then ap.processing_status else @processing_status end
			  AND (CONVERT(date,ap.payref_date) between @from_date and @to_date)
			  AND (ap.payref_no  like '%'+@payment_ref+'%' OR Coalesce(@payment_ref,'') = '')
			  AND (ap.processing_ref_no  like '%'+@external_payment_ref+'%' OR Coalesce(@external_payment_ref,'') = '')
	    order by ap.date_created desc

	select id,name from billing_account where is_active='Y' order by name
		
	set nocount off
end

GO
