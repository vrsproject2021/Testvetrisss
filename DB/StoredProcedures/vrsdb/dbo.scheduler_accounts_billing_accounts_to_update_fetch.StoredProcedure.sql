USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_accounts_billing_accounts_to_update_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_accounts_billing_accounts_to_update_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_accounts_billing_accounts_to_update_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_accounts_billing_accounts_to_update_fetch : 
                  fetch billing account records to update in quick books
** Created By   : Pavel Guha
** Created On   : 23/06/2020
*******************************************************/
--exec scheduler_accounts_billing_accounts_to_update_fetch
CREATE procedure [dbo].[scheduler_accounts_billing_accounts_to_update_fetch]
as
begin
	
	set nocount on

	select top 1
		   ba.id,ba.code,ba.name,
		   qb_name      = isnull(ba.qb_name,''),
	       address_1    = isnull(ba.address_1,''),
		   address_2    = isnull(ba.address_2,''),
		   city         = isnull(ba.city,''),
		   zip          = isnull(ba.zip,''),
		   state_name   = isnull(s.name,''),
		   country_name = isnull(c.name,''),
		   email_id     = isnull(ba.user_email_id,''),
		   phone_no     = isnull(ba.phone_no,''),
		   ba.is_active,
		   debtor_id    = isnull(ba.debtor_id,'')
	from billing_account ba
	left outer join sys_states s on s.id = ba.state_id
	left outer join sys_country c on c.id = ba.country_id
	where ba.is_active='Y'
	and ba.update_qb='Y'
	order by ba.name
	
	set nocount off


end


GO
