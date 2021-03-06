USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_brw_params_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_case_notification_brw_params_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_brw_params_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_case_notification_brw_params_fetch : fetch 
				  notification rule browser parameters 
** Created By   : Pavel Guha
** Created On   : 26/09/2019
*******************************************************/

--exec settings_case_notification_brw_params_fetch
create PROCEDURE [dbo].[settings_case_notification_brw_params_fetch] 
as
begin
	set nocount on
	
	select u.id,u.name 
	from users u
	inner join user_roles ur on ur.id=u.user_role_id
	where ur.code='SYSADMIN'
	and u.is_active='Y'
	order by u.name

	select u.id,u.name 
	from users u
	inner join user_roles ur on ur.id=u.user_role_id
	where ur.code='RDL'
	and u.is_active='Y'
	order by u.name

	set nocount off
end


GO
