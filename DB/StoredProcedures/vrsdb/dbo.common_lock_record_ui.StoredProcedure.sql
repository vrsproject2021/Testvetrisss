USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_lock_record_ui]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_lock_record_ui]
GO
/****** Object:  StoredProcedure [dbo].[common_lock_record_ui]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hp_common_lock_record_ui : lock record
** Created By   : Pavel Guha 
** Created On   : 10/04/2019
*******************************************************/
CREATE procedure [dbo].[common_lock_record_ui]
    @menu_id int,
    @record_id uniqueidentifier,
	@addl_record_id_ui uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	  begin transaction
	  
	  if(select count(menu_id) from  sys_record_lock_ui where menu_id=@menu_id and record_id=@record_id and addl_record_id_ui = isnull(@addl_record_id_ui,'00000000-0000-0000-0000-000000000000'))=0
		begin
			  insert into sys_record_lock_ui(record_id,addl_record_id_ui,user_id,menu_id,locked_on,session_id)
								      values(@record_id,isnull(@addl_record_id_ui,'00000000-0000-0000-0000-000000000000'),@user_id,@menu_id,getdate(),@session_id)
			  if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='029',@return_status=0
					return 0
				end
		end

	commit transaction
	select @return_status=1
	return 1
end

GO
