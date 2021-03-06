USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_unlock_user]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_unlock_user]
GO
/****** Object:  StoredProcedure [dbo].[hk_unlock_user]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_unlock_user : unlock user
** Created By   : BK
** Created On   : 29/07/2019
*******************************************************/
--exec hk_unlockuser_fetch 3,1,'',0
CREATE procedure [dbo].[hk_unlock_user]
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@updated_by uniqueidentifier,
	@update_session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	declare @user_name nvarchar(200),
			@record_id uniqueidentifier,
		    @activity_text nvarchar(max),
			@menu_id int,
			@study_uid nvarchar(100),
			@ctr int,
			@rc int
	
	--set @session_id = isnull(@session_id,'00000000-0000-0000-0000-000000000000')
	begin transaction
	if(select count(user_id) from sys_user_lock where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0 
		begin
			
			if(select count(record_id) from sys_record_lock where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
				begin
					delete from sys_record_lock where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
					  
					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_code='170',@return_status=0
							return 0
						end
				end	

			if(select count(record_id) from sys_record_lock_ui where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
				begin
					select @user_name = name from users where id=@user_id

					create table #tmp
					(
						rec_id int identity(1,1),
						menu_id int,
						record_id uniqueidentifier
					)

					insert into #tmp(menu_id,record_id) 
					(select menu_id,record_id 
					 from sys_record_lock_ui 
					 where menu_id in (20,21,22,76) 
					 and user_id=@user_id 
					 and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)

					select @rc=@@rowcount,@ctr=1

					while(@ctr <= @rc)
						begin
							select @menu_id   = menu_id,
								   @record_id = record_id
							from #tmp
							where rec_id = @ctr

							if(select count(id) from study_hdr where id=@record_id)>0
								begin
									select @study_uid=study_uid from study_hdr where id=@record_id
								end
							else if(select count(id) from study_hdr_archive where id=@record_id)>0
								begin
									select @study_uid=study_uid from study_hdr_archive where id=@record_id
								end

							delete from sys_record_lock_ui where user_id=@user_id and menu_id=@menu_id and record_id=@record_id  and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
							  
							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='170',@return_status=0
									return 0
								end

							set @activity_text =  'Lock of Study UID : ' + isnull(@study_uid,'') + ' under session ' + convert(nvarchar(36),@session_id) + ' released'
							set @error_code=''
							set @return_status=0

							exec common_study_user_activity_trail_save
								@study_hdr_id  = @record_id,
								@study_uid     = '',
								@menu_id       = @menu_id,
								@activity_text = @activity_text,
								@session_id    = @update_session_id,
								@activity_by   = @updated_by,
								@error_code    = @error_code output,
								@return_status = @return_status output

							set @ctr = @ctr + 1
						end

					drop table #tmp

					
				end  

		    delete from	 sys_user_lock where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
					
			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='172',@return_status=0
					return 0
				end

			set @activity_text =  'Lock released and user ' + @user_name + ' and session ' + convert(nvarchar(36),@session_id) + ' unlocked'
			set @error_code=''
			set @return_status=0

			exec common_user_activity_log
				@user_id       = @user_id,
				@activity_text = @activity_text,
				@session_id    = @update_session_id,
				@menu_id       = @menu_id,
				@error_code    = @error_code output,
				@return_status = @return_status output
	    end
	    
	else
		begin
			  commit transaction
			  select @error_code='171',@return_status=1
			  return 1
		end
	
	commit transaction
	select @error_code='173',@return_status=1
	return 1
		

	
end

GO
