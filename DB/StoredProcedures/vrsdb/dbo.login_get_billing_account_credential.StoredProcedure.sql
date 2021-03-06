USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_get_billing_account_credential]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_get_billing_account_credential]
GO
/****** Object:  StoredProcedure [dbo].[login_get_billing_account_credential]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_get_billing_account_credential : get 
                  billing account credentials
** Created By   : Pavel Guha
** Created On   : 20/05/2020
*******************************************************/
-- exec login_get_billing_account_credential '3E9961FE-32AC-48A8-B0BE-7AEEC60E14D7','','','',0
create procedure [dbo].[login_get_billing_account_credential]
    @billing_account_id uniqueidentifier,
	@login_id nvarchar(50)='' output,
	@login_pwd nvarchar(200) ='' output,
	@login_user_id nvarchar(200) ='00000000-0000-0000-0000-000000000000' output,
	@error_code nvarchar(10)='' output,
	@return_status int =0 output
as
begin
	declare @user_role_id int,
			@user_role_code nvarchar(50)

	select @login_id      = isnull(login_id,''),
		   @login_pwd     = isnull(login_pwd,''),
		   @login_user_id = isnull(login_user_id,'00000000-0000-0000-0000-000000000000')
	from billing_account
	where id=@billing_account_id

	if(@login_id='')
		begin
			select @error_code='313',@return_status=0
			return 0
		end
	if(@login_pwd='')
		begin
			select @error_code='313',@return_status=0
			return 0
		end
	

	select @user_role_id = user_role_id from users where login_id = @login_id
	set @user_role_id= isnull(@user_role_id,0)

	select @user_role_code = code from user_roles where id=@user_role_id
	
	if(isnull(@user_role_code,'') <> 'AU')
		begin
			select @error_code='313',@return_status=0
			return 0
		end

	--print @login_id
	--print @login_pwd
    
	select @error_code='',@return_status=1
	return 1
			

	
end

GO
