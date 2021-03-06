USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_accounts_radiologist_to_update_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_accounts_radiologist_to_update_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_accounts_radiologist_to_update_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_accounts_radiologist_to_update_fetch : 
                  fetch radiologist records to update in quick books
** Created By   : Pavel Guha
** Created On   : 23/11/2020
*******************************************************/
--exec scheduler_accounts_radiologist_to_update_fetch
create procedure [dbo].[scheduler_accounts_radiologist_to_update_fetch]
as
begin
	
	set nocount on

	select top 1
		   r.id,r.code,r.name,
		   qb_name      = isnull(r.qb_name,''),
	       address_1    = isnull(r.address_1,''),
		   address_2    = isnull(r.address_2,''),
		   city         = isnull(r.city,''),
		   zip          = isnull(r.zip,''),
		   state_name   = isnull(s.name,''),
		   country_name = isnull(c.name,''),
		   email_id     = isnull(r.email_id,''),
		   phone_no     = isnull(r.phone_no,''),
		   r.is_active,
		   crefitor_id    = isnull(r.creditor_id,'')
	from radiologists r
	left outer join sys_states s on s.id = r.state_id
	left outer join sys_country c on c.id = r.country_id
	where r.is_active='Y'
	and r.update_qb='Y'
	order by r.name
	
	set nocount off


end


GO
