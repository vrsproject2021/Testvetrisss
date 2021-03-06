USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_user_pref_theme_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_user_pref_theme_update]
GO
/****** Object:  StoredProcedure [dbo].[login_user_pref_theme_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_user_pref_theme_update : unlock user session
** Created By   : Pavel Guha 
** Created On   : 17/04/2019
*******************************************************/
create procedure [dbo].[login_user_pref_theme_update]
    @theme_pref nvarchar(10),
	@user_id uniqueidentifier,
	@error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
		set nocount on
	    begin transaction

		update users
		set theme_pref = @theme_pref
		where id =@user_id

		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_code='498',@return_status=0
				return 0
			end
	
		
		commit transaction
		set nocount off
		select @return_status=1,@error_code=''
		return 1
		

	
end

GO
