USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_unlock_user]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_unlock_user]
GO
/****** Object:  StoredProcedure [dbo].[login_unlock_user]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_unlock_user : unlock user session
** Created By   : Pavel Guha 
** Created On   : 17/04/2019
*******************************************************/
CREATE procedure [dbo].[login_unlock_user]
    @email_id nvarchar(100),
    @return_status int =0 output
as
begin
		set nocount on

		declare @user_id uniqueidentifier

		declare @record_id uniqueidentifier,
		        @activity_text nvarchar(max),
		        @error_code nvarchar(10),
				@menu_id int,
			    @menu_text nvarchar(100),
				@ctr int,
				@rc int


		select @user_id = id
		from users
		where login_id = @email_id


	    begin transaction

		if(select count(record_id) from sys_record_lock_ui where menu_id in (20,21,22,76) and user_id=@user_id)>0
			begin
				create table #tmp
				(
					rec_id int identity(1,1),
					menu_id int,
					record_id uniqueidentifier
				)

				insert into #tmp(menu_id,record_id) (select menu_id,record_id from sys_record_lock_ui where menu_id in (20,21,22,76) and user_id=@user_id)

				select @rc=@@rowcount,@ctr=1

				while(@ctr <= @rc)
					begin
						select @menu_id   = menu_id,
						       @record_id = record_id
						from #tmp
						where rec_id = @ctr

						set @activity_text =  'Logged in and lock released'
						set @error_code=''
						set @return_status=0

						exec common_study_user_activity_trail_save
							@study_hdr_id  = @record_id,
							@study_uid     = '',
							@menu_id       = @menu_id,
							@activity_text = @activity_text,
							@activity_by   = @user_id,
							@error_code    = @error_code output,
							@return_status = @return_status output

						
					select @menu_text= menu_desc from sys_menu where menu_id=@menu_id
					set  @activity_text =  @menu_text  + '==> Lock released => Study UID ' + (select t.study_uid from (select study_uid from study_hdr where id =@record_id union select study_uid from study_hdr_archive where id =@record_id) t)
					
					exec common_user_activity_log
						 @user_id       = @user_id,
						 @activity_text = @activity_text,
						 @menu_id       = 0,
						 @error_code    = @error_code output,
						 @return_status = @return_status output

						set @ctr = @ctr + 1
					end

				drop table #tmp
			end
	
		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id
		delete from sys_user_lock where user_id=@user_id

	
		
		commit transaction
		set nocount off
		select @return_status=1
		return 1
		

	
end

GO
