USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_rolewise_access_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_user_rolewise_access_rights_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_rolewise_access_rights_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_fetch_access_rights : fetch user access rights
** Created By   : Pavel Guha
** Created On   : 06/06/2019
*******************************************************/
--exec settings_user_rolewise_access_rights_fetch 3
create procedure [dbo].[settings_user_rolewise_access_rights_fetch]
	@id int
as 
begin

	-- Menu Rights 
	select menu_id,menu_desc,parent_id,display_index,assigned='Y'
	from sys_menu  
	where is_enabled='Y'
	and menu_id in (select menu_id
					from user_role_menu_rights
					where user_role_id=@id)
	union
	select menu_id,menu_desc,parent_id,display_index,assigned='N'
	from sys_menu  
	where menu_id in (select menu_id
						from sys_menu  
						where is_enabled='Y'
						and menu_id not in (select menu_id
											from user_role_menu_rights
											where user_role_id=@id))
   order by parent_id,display_index
			
end

GO
