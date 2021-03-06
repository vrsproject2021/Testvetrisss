USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_unlock_user_locked_records]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_unlock_user_locked_records]
GO
/****** Object:  StoredProcedure [dbo].[common_unlock_user_locked_records]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_unlock_user_locked_record : unlock
                  user locked records
** Created By   : Pavel Guha 
** Created On   : 12/11/2019
*******************************************************/
CREATE procedure [dbo].[common_unlock_user_locked_records]
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
begin
	set nocount on
	begin transaction

	delete from sys_record_lock where  user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000'
	delete from sys_record_lock_ui where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')='00000000-0000-0000-0000-000000000000'

	set nocount off
	commit transaction
	return 1
end

GO
