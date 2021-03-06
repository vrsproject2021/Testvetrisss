USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_unlock_user]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_unlock_user]
GO
/****** Object:  StoredProcedure [dbo].[common_unlock_user]    Script Date: 28-09-2021 19:36:34 ******/
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
CREATE procedure [dbo].[common_unlock_user]
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @return_status int =0 output
as
begin
	set nocount on
	begin transaction

	delete from sys_record_lock where user_id = @user_id  and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
	delete from sys_record_lock_ui where user_id = @user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
	delete from sys_user_lock where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id

	commit transaction
	set nocount off
	select @return_status=1
	return 1

end

GO
