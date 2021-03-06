USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_opening_balance_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_opening_balance_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_opening_balance_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_opening_balance_fetch 
** Created By   : KC
** Created On   : 30/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_opening_balance_fetch]
	@id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@year int,
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
			o.id,
			o.opbal_date,
			o.invoice_no,
			o.opbal_amount,
			o.isadjusted,
			ba.name,
			o.billing_account_id,
			o.created_by,
			u.name created_by_user,
			o.date_created,
			o.updated_by,
			o.date_updated
		from ar_opening_balance o
		inner join billing_account ba on o.billing_account_id=ba.id
		left join users u on u.id=o.created_by
		where 
			 o.id = case when ISNULL(@id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000' then o.id else @id end
			 and ba.id = case when ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000' then ba.id else @billing_account_id end
			 and case when ISNULL(@id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000' then YEAR(o.opbal_date) else @year end = @year
		order by ba.name;

	set nocount off
end

GO
