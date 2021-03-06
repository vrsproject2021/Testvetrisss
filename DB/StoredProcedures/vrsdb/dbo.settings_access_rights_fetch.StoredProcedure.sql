USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_access_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_access_rights_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_access_rights_fetch]    Script Date: 28-09-2021 19:36:35 ******/
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
CREATE procedure [dbo].[settings_access_rights_fetch]
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

	
	if(@id>0)
		begin
			-- Menu Rights 
			select menu_id,menu_desc,parent_id
			from sys_menu  
			where is_enabled='Y'
			and menu_id not in (select menu_id
								from user_role_menu_rights
								where user_role_id=@id)
			union
			select menu_id,menu_desc,parent_id
			from sys_menu  
			where menu_id in (select parent_id
							  from sys_menu  
							  where is_enabled='Y'
							  and parent_id>0
							  and menu_id not in (select menu_id
												  from user_role_menu_rights
												   where user_role_id=@id))
			
		
		
		end
	else
		begin
			--Menu Rights 
			select menu_id,menu_desc,parent_id
			from sys_menu  
			where is_enabled='Y'
			order by display_index
			
	
		
		end
		
	
	
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
