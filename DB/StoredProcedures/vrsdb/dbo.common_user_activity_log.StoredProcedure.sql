USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_user_activity_log]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_user_activity_log]
GO
/****** Object:  StoredProcedure [dbo].[common_user_activity_log]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_user_activity_log : save user activity
** Created By   : Pavel Guha 
** Created On   : 17/05/2021
*******************************************************/
CREATE procedure [dbo].[common_user_activity_log]
    @user_id uniqueidentifier,
	@activity_text nvarchar(max),
	@session_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @menu_id int=0,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on
	 
    insert into vrslogdb..sys_user_activity_log(user_id,session_id,menu_id,activity_text,activity_datetime)
	                                     values(@user_id,@session_id,@menu_id,@activity_text,getdate())

    if(@@rowcount=0)
		begin
			select @error_code='482',@return_status=0
			return 0
		end

	set nocount off

	select @error_code='',@return_status=1
	return 1
		

	
end

GO
