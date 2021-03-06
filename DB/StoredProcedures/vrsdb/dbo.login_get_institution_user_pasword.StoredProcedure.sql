USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_get_institution_user_pasword]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_get_institution_user_pasword]
GO
/****** Object:  StoredProcedure [dbo].[login_get_institution_user_pasword]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_get_institution_user_pasword : get 
                  institution user password
** Created By   : Pavel Guha
** Created On   : 02/09/2019
*******************************************************/
-- exec login_get_institution_user_pasword 'SEVI','IpsO24R1JzQ=','00000000-0000-0000-0000-000000000000','','',0
CREATE procedure [dbo].[login_get_institution_user_pasword]
    @login_id nvarchar(100),
	@institution_code nvarchar(10),
	@password nvarchar(200)='' output,
	@error_code nvarchar(10)='' output,
	@return_status int =0 output
as
begin
	
	declare @id  uniqueidentifier,
	        @institution_id uniqueidentifier,
			@user_role_id int,
			@user_role_code nvarchar(10)

   
   if (select count(id) from institutions where upper(code)=upper(@institution_code))=0
		begin
			select @error_code='183',@return_status=0 /*invalid login id*/
			return 0
		end
	else
		begin
			select  @institution_id=id from institutions where upper(code)=upper(@institution_code) 
		end

	select @user_role_id=user_role_id,
	       @id = id
	from users
	where login_id = @login_id

	if(select isnull(is_active,'N') from users  where id=@id)='N'
		begin
			select @error_code='002',@return_status=0 /*user blocked*/
			return 0
		end
		
	--if (select count(user_id) from institution_user_link where upper(user_login_id)=upper(@login_id) and institution_id=@institution_id)=0
	--	begin
	--		select @error_code='000',@return_status=0 /*invalid login id*/
	--		return 0
	--	end
	--else
	--	begin
	--		select top 1 @id=user_id from institution_user_link where upper(user_login_id)=upper(@login_id) and institution_id=@institution_id
	--	end


	
		
    select @password = password
	from users 
	where id=@id
    
	select @error_code='',@return_status=1
	return 1
			

	
end

GO
