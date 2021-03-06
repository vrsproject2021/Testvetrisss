USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_acquire_record_lock_ui]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_acquire_record_lock_ui]
GO
/****** Object:  StoredProcedure [dbo].[common_acquire_record_lock_ui]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_acquire_record_lock_ui : 
				  acquire record lock 
** Created By   : Pavel Guha 
** Created On   : 08-Aug-2020
*******************************************************/
CREATE procedure [dbo].[common_acquire_record_lock_ui]
    @menu_id int,
    @record_id uniqueidentifier,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	  begin transaction

	  declare @lock_user_id uniqueidentifier,
	          @lock_session_id uniqueidentifier,
	          @activity_text nvarchar(max)
	  
	  if(select count(menu_id) from  sys_record_lock_ui where menu_id=@menu_id and record_id=@record_id)>0
		begin
			  select @lock_user_id = user_id ,
			         @lock_session_id = isnull(session_id,'00000000-0000-0000-0000-000000000000')
			  from sys_record_lock_ui 
			  where menu_id=@menu_id 
			  and record_id=@record_id

			  set @activity_text =  'Locked by ' + (select name from users where id=@lock_user_id) + '/Session ' + Convert(varchar(36),@lock_session_id) + ' is broken'
			  exec common_study_user_activity_trail_save
					@study_hdr_id  = @record_id,
					@study_uid     = '',
					@menu_id       = @menu_id,
					@activity_text = @activity_text,
					@session_id    = @session_id,
					@activity_by   = @user_id,
					@error_code    = @error_code output,
					@return_status = @return_status output

				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end

			  delete from sys_record_lock_ui where user_id = @lock_user_id and session_id=@lock_session_id

			  insert into sys_record_lock_ui(record_id,addl_record_id_ui,user_id,menu_id,locked_on,session_id)
								      values(@record_id,'00000000-0000-0000-0000-000000000000',@user_id,@menu_id,getdate(),@session_id)

			  if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='029',@return_status=0
					return 0
				end

			 set @activity_text =  'Lock accquired'
			  exec common_study_user_activity_trail_save
					@study_hdr_id  = @record_id,
					@study_uid     = '',
					@menu_id       = @menu_id,
					@activity_text = @activity_text,
					@session_id    = @session_id,
					@activity_by   = @user_id,
					@error_code    = @error_code output,
					@return_status = @return_status output

				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end
		end

	commit transaction
	select @return_status=1
	return 1
end

GO
