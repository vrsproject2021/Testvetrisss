USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_userwise_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_userwise_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_userwise_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_userwise_fetch : fetch ar_payments
** Created By   : KC
** Created On   : 08/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_userwise_fetch]
    @billing_account_id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier
as
begin
	 set nocount on



	-- all payments
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
			ap.date_created, 
			ap.updated_by, 
			ap.date_updated,
			ba.name billing_account_name, 
			ba.code billing_account_code
		from ar_payments ap
		inner join billing_account ba on ap.billing_account_id=ba.id
		where ba.id=@billing_account_id
		order by ap.date_created desc;

	set nocount off
end

GO
