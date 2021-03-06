USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_ws_session_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_ws_session_create]
GO
/****** Object:  StoredProcedure [dbo].[login_ws_session_create]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_ws_session_create : unlock user session
** Created By   : Pavel Guha 
** Created On   : 02/04/2020
*******************************************************/
create procedure [dbo].[login_ws_session_create]
	@session_id nvarchar(30),
    @user_id uniqueidentifier,
	@error_code nvarchar(10)='' output,
	@return_status int =0 output
as
begin
	
	set nocount on

	begin transaction
	
	insert into sys_ws8_session(session_id,date_created,created_by)
	                     values(@session_id,getdate(),@user_id)
	
	
	if(@@rowcount = 0)
		begin
			select @error_code='294',@return_status=0
			rollback transaction
			return 0
			
		end 

		
	select @error_code='',@return_status=1
	commit transaction
	set nocount off
	return 1
		

	
end

GO
