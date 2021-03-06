USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_unlock_record]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_unlock_record]
GO
/****** Object:  StoredProcedure [dbo].[common_unlock_record]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_unlock_record : unlock record
** Created By   : Pavel Guha 
** Created On   : 11/04/2019
*******************************************************/
CREATE procedure [dbo].[common_unlock_record]
    @menu_id int=0,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier= '00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on
	declare @user_role_code nvarchar(5)

	select @user_role_code = code from user_roles where id =(select user_role_id from users where id=@user_id)

	  begin transaction
	  if(@menu_id=0)
		begin
			if(select count(record_id) from sys_record_lock where user_id=@user_id)>0
				begin
						if(@user_role_code='RDL')
							begin
								if(select count(record_id) from sys_record_lock where user_id=@user_id and session_id=@session_id)>0
									begin
										delete from sys_record_lock where user_id=@user_id and session_id=@session_id

										 if(@@rowcount=0)
											begin
												rollback transaction
												select @error_code='030',@return_status=0
												return 0
											end
									end
							end	
						else
							begin
								delete from sys_record_lock where user_id=@user_id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='030',@return_status=0
										return 0
									end
							end
				end
		end
	  else if(select count(record_id) from sys_record_lock where user_id=@user_id and menu_id=@menu_id)>0
		 begin
			  if(@user_role_code='RDL')
				 begin
					if(select count(record_id) from sys_record_lock where user_id=@user_id and menu_id=@menu_id and session_id = @session_id)>0
						begin
							delete from sys_record_lock where menu_id=@menu_id and user_id=@user_id and session_id=@session_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='030',@return_status=0
									return 0
								end
						end
					else
						begin
							delete from sys_record_lock where menu_id=@menu_id and user_id=@user_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='030',@return_status=0
									return 0
								end
						end
				 end
		 end
	
	set nocount off
	commit transaction
	select @return_status=1
	return 1
		

	
end

GO
