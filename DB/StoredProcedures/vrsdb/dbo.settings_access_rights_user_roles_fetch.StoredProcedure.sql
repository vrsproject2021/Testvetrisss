USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_access_rights_user_roles_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_access_rights_user_roles_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_access_rights_user_roles_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : access_rights_user_roles_fetch : fetch user roles
** Created By   : Pavel Guha
** Created On   : 02/05/2019
*******************************************************/
--exec settings_fetch_access_rights_user_roles 2,10,1
create procedure [dbo].[settings_access_rights_user_roles_fetch]

as
begin
		
		
	-- user roles	
	select id,name
	from user_roles
	where is_visible='Y'
	and is_active='Y'
	order by name
	
	
end

GO
