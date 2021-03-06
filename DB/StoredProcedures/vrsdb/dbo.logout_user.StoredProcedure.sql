USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[logout_user]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[logout_user]
GO
/****** Object:  StoredProcedure [dbo].[logout_user]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : logout_user : Delete data from sys_user_lock when an user logs out
** Created By   : Pavel Guha
** Created On   : 11/04/2019
*******************************************************/
--exec logout_user 2,1,'',''
CREATE procedure [dbo].[logout_user]
	@user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000' output,
	@error_code nvarchar(10)='' output,
	@return_status int =0 output
as
begin

	declare @record_id uniqueidentifier,
		    @activity_text nvarchar(max),
			@menu_id int,
			@menu_text nvarchar(100),
			@ctr int,
			@rc int

	begin transaction

	delete from sys_user_lock	
	where [user_id] = @user_id 
	and session_id = @session_id
	
	
	if(@@rowcount = 0)
		begin
			/* Unable to delete sys_logged_user table */
			select @error_code='012',@return_status=0
			rollback transaction
			return 0
			
		end 
	
	exec common_unlock_record
		@user_id       = @user_id,
		@session_id    = @session_id,
		@error_code    = @error_code output,
		@return_status = @return_status output	
		
	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end
		
	
	if(select count(record_id) from sys_record_lock_ui where menu_id in (20,21,22,76) and user_id=@user_id and session_id=@session_id)>0
		begin
			create table #tmp
			(
				rec_id int identity(1,1),
				menu_id int,
				record_id uniqueidentifier
			)

			insert into #tmp(menu_id,record_id) (select menu_id,record_id from sys_record_lock_ui where menu_id in (20,21,22,76) and user_id=@user_id and session_id=@session_id)

			select @rc=@@rowcount,@ctr=1

			while(@ctr <= @rc)
				begin
					select @menu_id   = menu_id,
							@record_id = record_id
					from #tmp
					where rec_id = @ctr

					set @activity_text =  'Lock released and logged out'
					set @error_code=''
					set @return_status=0

					exec common_study_user_activity_trail_save
						@study_hdr_id  = @record_id,
						@study_uid     = '',
						@menu_id       = @menu_id,
						@activity_text = @activity_text,
						@activity_by   = @user_id,
						@session_id    = @session_id,
						@error_code    = @error_code output,
						@return_status = @return_status output

					select @menu_text= menu_desc from sys_menu where menu_id=@menu_id
					set  @activity_text =  @menu_text  + '==> Lock released => Study UID ' + (select t.study_uid from (select study_uid from study_hdr where id =@record_id union select study_uid from study_hdr_archive where id =@record_id) t)
					
					exec common_user_activity_log
						 @user_id       = @user_id,
						 @activity_text = @activity_text,
						 @session_id    = @session_id,
						 @menu_id       = 0,
						 @error_code    = @error_code output,
						 @return_status = @return_status output

					
					set @ctr = @ctr + 1
				end

			drop table #tmp
		end


	exec common_unlock_record_ui
		@user_id       = @user_id,
		@session_id    = @session_id,
		@error_code    = @error_code output,
		@return_status = @return_status output	
		
	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end
		
	exec common_user_activity_log
		 @user_id       = @user_id,
		 @activity_text = 'Logged out',
		 @session_id    = @session_id,
		 @menu_id       = 0,
		 @error_code    = @error_code output,
		 @return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end
		
	select @error_code='',@return_status=1
	commit transaction
	return 1
	
	
end

GO
