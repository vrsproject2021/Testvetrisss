USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_get_menu]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_get_menu]
GO
/****** Object:  StoredProcedure [dbo].[login_get_menu]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_get_menu : get menu
** Created By   : Pavel Guha
** Created On   : 09/04/2019
*******************************************************
*****************************x**************************/
--exec login_get_menu 1,1
CREATE procedure [dbo].[login_get_menu]
	@user_role_id int,	
	@user_id uniqueidentifier
as
 
begin

	
	if(select COUNT(record_id) from sys_record_lock where user_id=@user_id)>0
		 begin
			  delete from sys_record_lock where user_id=@user_id
		 end
    if(select COUNT(record_id) from sys_record_lock_ui where user_id=@user_id)>0
		 begin
			  delete from sys_record_lock where user_id=@user_id
		 end
	
	select urmr.menu_id,m.menu_desc,m.parent_id,m.menu_level,m.nav_url,m.is_browser,m.menu_icon,m.display_index,m.is_dropdown
	from user_role_menu_rights urmr
	inner join sys_menu m on m.menu_id=urmr.menu_id
	where m.is_enabled='Y'
	and m.is_dropdown='Y'
	and urmr.user_role_id = @user_role_id
	order by m.parent_id,m.display_index
	
end

GO
