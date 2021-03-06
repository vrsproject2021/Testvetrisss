USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_change_password]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_change_password]
GO
/****** Object:  StoredProcedure [dbo].[login_change_password]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_change_password : change user password
** Created By   : Pavel Guha
** Created On   : 07/05/2019
*******************************************************
*******************************************************/
CREATE PROCEDURE [dbo].[login_change_password]

@user_id uniqueidentifier,
@old_password nvarchar(200),	
@new_password nvarchar(200),
@error_code varchar(10) = '' output,
@return_status int = 0 output

as
begin
	
	declare @existing_password nvarchar(200),
	        @user_code nvarchar(5),
			@user_role_id int,
			@user_role_code nvarchar(5),
			@login_id nvarchar(100)

	select @existing_password = password,
	       @user_role_id      = user_role_id
	from users 
	where id = @user_id 

	select @user_role_code = code
	from user_roles
	where id = @user_role_id


	-- check if old password supplied matches existing password..
	if(rtrim(ltrim(@existing_password))<> rtrim(ltrim(@old_password)))
		begin
			select @error_code='013',@return_status=0
			return 0
		end
	
	--user is valid, update password..
	begin transaction

	select @login_id = login_id from users where id = @user_id

	update users
	set password		    = @new_password,
		update_by		    = @user_id,
		date_updated	    = getdate()
	where id = @user_id
	
	if(@@rowcount<0)
		begin
			select @error_code='014',@return_status=0
			rollback transaction
			return 0
		end

	if(@user_role_code<>'SUPP' or @user_role_code<> 'SYSADMIN')
		begin
			if(@user_role_code='RDL')
				 begin
					update radiologists
					set login_pwd     = @new_password,
						updated_by    = @user_id,
						date_updated  = getdate()
					where login_user_id = @user_id
				 end
			else if(@user_role_code='IU')
				 begin
					update institution_user_link
					set user_pwd           = @new_password,
						user_pacs_password = @new_password,
						updated_by         = @user_id,
						date_updated       = getdate()
					where user_id = @user_id
				 end
			else if(@user_role_code='AU')
				 begin
					update billing_account
					set login_pwd          = @new_password,
						updated_by         = @user_id,
						date_updated       = getdate()
					where login_user_id = @user_id
				 end

			if(@@rowcount<0)
				begin
					select @error_code='014',@return_status=0
					rollback transaction
					return 0
				end
		end

   
		
		
	select @error_code='015',@return_status=1
	commit transaction
	return 1
end

GO
