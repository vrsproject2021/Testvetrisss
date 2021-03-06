USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_assign_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_assign_rights_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_assign_rights_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_fetch_access_rights : fetch access rights
** Created By   : Pavel Guha
** Created On   : 02/05/2019
*******************************************************/
--exec settings_access_rights_fetch 0,11,1,'22222222-2222-2222-2222-222222222222','',''
create procedure [dbo].[settings_assign_rights_fetch]
	@id int=0,
	@menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as 
begin
	--unlock records
	if(select count(record_id) from sys_record_lock where user_id=@user_id)>0
		 begin
			  delete from sys_record_lock where user_id=@user_id
		 end
	if(select count(record_id) from sys_record_lock_ui where user_id=@user_id)>0
		 begin
			  delete from sys_record_lock_ui where user_id=@user_id
		 end


		
	-- Menu Rights Assigned
	select urmr.menu_id,m.menu_desc,m.parent_id 
	from user_role_menu_rights urmr
	inner join sys_menu m on m.menu_id=urmr.menu_id
	where urmr.user_role_id=@id
	and m.is_enabled='Y'
	order by m.display_index
	
	
	
	--lock records
	if(@id>0)
		begin
			if(select count(record_id) from sys_record_lock where record_id=@id and menu_id=@menu_id)=0
				begin
					exec common_lock_record
						@menu_id       = @menu_id,
						@record_id     = @id,
						@user_id       = @user_id,
						@error_code    = @error_code output,
						@return_status = @return_status output	
						
					if(@return_status=0)
						begin
							return 0
						end
				end
		end
	
end

GO
