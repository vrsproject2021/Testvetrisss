USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_billing_account_info_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_billing_account_info_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_billing_account_info_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : customer info fetch
** Created By   : KC
** Created On   : 25/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_billing_account_info_fetch]
    @billing_account_id uniqueidentifier
as
begin
	 set nocount on



	-- billing account info for online payment
	select 
		ba.[name],ba.address_1, ba.address_2, ba.city, ba.zip, c.code country, s.name [state], 
		ba.user_email_id email_id, u.contact_no 
	from 
		billing_account ba with(nolock) 
		left join sys_country c with(nolock) on c.id=ba.country_id
		left join sys_states s with(nolock) on s.id=ba.state_id
		inner join users u with(nolock) on u.id=ba.login_user_id
	where ba.id = @billing_account_id
	
	set nocount off
end

GO
