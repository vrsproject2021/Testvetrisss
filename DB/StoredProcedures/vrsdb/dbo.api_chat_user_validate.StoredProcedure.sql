USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[api_chat_user_validate]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[api_chat_user_validate]
GO
/****** Object:  StoredProcedure [dbo].[api_chat_user_validate]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : api_chat_user_validate : validate
                  chat user
** Created By   : Pavel Guha
** Created On   : 20/05/2021
*******************************************************/
--EXEC api_chat_user_validate '11111111-1111-1111-1111-111111111111','','','','','','',0
create procedure [dbo].[api_chat_user_validate]
	@user_id uniqueidentifier,
	@user_role nvarchar(5)='' output,
	@user_role_desc nvarchar(30)='' output,
	@name nvarchar(100) = '' output,
	@email_id nvarchar(100)='' output,
	@contact_no nvarchar(100)='' output,
	@output_msg nvarchar(100)='' output,
	@return_status int = 0 output
as
begin
	set nocount on


	if(select count(id) from users where id=@user_id)>0
		begin
			if(select is_active from users where id=@user_id)='Y'
				begin
					select @user_role      = ur.code,
					       @user_role_desc = ur.name,
					       @name           = u.name,
						   @email_id       = u.email_id,
						   @contact_no     = u.contact_no
					from users u 
					inner join user_roles ur on ur.id = u.user_role_id
					where u.id= @user_id

					--select @user_role,@user_role_desc,@name,@email_id,@contact_no
					select @output_msg='SUCCESS', @return_status=1
				end
			else
				begin
					select @output_msg='FAIL : This user has been deactivated',@return_status=0
				end
		end 
	else 
		begin
			select @output_msg='FAIL : User does not exists',@return_status=0
		end

	set nocount off
	return 1
	
end
GO
