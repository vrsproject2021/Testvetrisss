USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_refunds_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_refunds_list_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_refunds_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_refunds_list_fetch : fetch ar_refunds
** Created By   : KC
** Created On   : 22/07/2020
*******************************************************/
CREATE procedure [dbo].[ar_refunds_list_fetch]
	@billing_account_id		uniqueidentifier=null,
	@refund_mode           nvarchar(1) = NULL,
	@processing_status      nvarchar(1) = NULL,
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
	where u.id = @user_id
			select 
			ar.id, 
			ar.billing_account_id, 
			ar.refund_mode, 
			case when ar.refund_mode='0' then 'OFFLINE' else 'ONLINE' end refund_mode_name, 
			ap.payref_no, 
			ap.payref_date, 
			ar.refundref_no, 
			ar.refundref_date, 
			ar.processing_ref_no, 
			ar.processing_ref_date, 
			ar.processing_pg_name, 
			ar.processing_status, 
			case when ar.refund_mode='0' then 'Pass' else (case when ar.processing_status='1' then 'Pass' else 'Failed' end) end processing_status_name, 
			ar.refund_amount,
			ar.remarks,
			ar.created_by, 
			ar.created_by user_id, 
			u.name user_name, 
			ar.date_created, 
			ar.updated_by, 
			ar.date_updated,
			ba.name billing_account_name, 
			ba.code billing_account_code
		from ar_refunds ar
		inner join ar_payments ap on ap.id=ar.ar_payments_id
		inner join billing_account ba on ap.billing_account_id=ba.id
		inner join users u on ap.created_by=u.id
		where ar.billing_account_id=case when ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000' then ar.billing_account_id else @billing_account_id end
			  AND ar.refund_mode = case when ISNULL(@refund_mode,'A')='A' then ar.refund_mode else @refund_mode end
			  AND ar.processing_status = case when ISNULL(@processing_status,'A')='A' then ar.processing_status else @processing_status end
	    order by ar.date_created desc


	select id,name from billing_account where is_active='Y' order by name
		
	set nocount off
end

GO
