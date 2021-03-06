USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_user_email_id_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_user_email_id_fetch]
GO
/****** Object:  StoredProcedure [dbo].[login_user_email_id_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_user_email_id_fetch : validate
                  chat user
** Created By   : Pavel Guha
** Created On   : 20/05/2021
*******************************************************/
--EXEC login_user_email_id_fetch 'support@rad365.com','','',0
create procedure [dbo].[login_user_email_id_fetch]
	@login_id nvarchar(50),
	@email_id nvarchar(100)='' output,
	@error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on
	declare @user_id uniqueidentifier

	if(select count(id) from users where login_id=@login_id)>0
		begin
		
			select @user_id =  id from users where login_id = @login_id
			if(isnull((select is_active from users where id=@user_id),'N'))='Y'
				begin
					select @email_id = email_id
					from users 
					where id = @user_id
				end
			else
				begin
					select @error_code='002',@return_status=0
				end
		end 
	else 
		begin
			select @error_code ='000',@return_status=0
		end

    select @error_code ='',@return_status=1
	set nocount off
	return 1
	
end
GO
