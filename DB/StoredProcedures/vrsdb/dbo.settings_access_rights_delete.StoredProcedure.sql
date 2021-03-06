USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_access_rights_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_access_rights_delete]
GO
/****** Object:  StoredProcedure [dbo].[settings_access_rights_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_delete_access_rights : delete access rights
** Created By   : Pavel Guha
** Created On   : 02/05/2019
*******************************************************/
--exec settings_delete_access_rights 5,4,217,9,2,10,1,'','',''
create procedure [dbo].[settings_access_rights_delete]
    @id int=0 output,
    @del_menu_id int=0,
    @updated_by UniqueIdentifier,
    @menu_id int,
    @user_name nvarchar(30) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	declare @rowcount int, 
			@counter int,
			@module_name nvarchar(30),
			@menu_name nvarchar(30),
			@parent_id1 int,
			@parent_id2 int,
			@new_value nvarchar(max)
			
	exec common_check_record_lock
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @updated_by,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output
		
	if(@return_status=0)
		begin
			return 0
		end
	
	begin transaction
	
	
	
   if(@del_menu_id > 0)
		begin
			
			select @parent_id1=0,@parent_id2=0
			
			select @parent_id1 = parent_id from sys_menu where menu_id=@del_menu_id
			
			if(isnull(@parent_id1,0)=0)
			   begin
					select @menu_name = menu_desc from sys_menu where menu_id=@del_menu_id
			   end
			else
				begin
					select @parent_id2 = parent_id from sys_menu where menu_id=@parent_id1
					
				end
				
				
			delete from user_role_menu_rights 
			where menu_id in (select menu_id from sys_menu where parent_id in (select menu_id from sys_menu where parent_id=@del_menu_id))
			and user_role_id=@id 
			
			delete from user_role_menu_rights 
			where menu_id in (select menu_id from sys_menu where parent_id=@del_menu_id)
			and user_role_id=@id 
			
			delete from user_role_menu_rights 
			where menu_id=@del_menu_id
			and user_role_id=@id 
			

			
		end
		

	select @error_code='034',@return_status=1
	commit transaction
	return 1
	
end

GO
