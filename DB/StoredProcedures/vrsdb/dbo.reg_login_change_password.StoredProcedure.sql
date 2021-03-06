USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[reg_login_change_password]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[reg_login_change_password]
GO
/****** Object:  StoredProcedure [dbo].[reg_login_change_password]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************  
*******************************************************  
** Version  : 1.0.0.0  
** Procedure    : reg_login_change_password : change reg login password  
** Created By   : AM 
** Created On   : 18-08-2020 
*******************************************************  
*******************************************************/  
Create PROCEDURE [dbo].[reg_login_change_password]  
  
@reg_id uniqueidentifier,  
@login_id  nvarchar(200),  
@old_password nvarchar(200),   
@new_password nvarchar(200),
@email_id nvarchar(200),
@error_code varchar(10) = '' output,  
@return_status int = 0 output  
  
as  
begin  
   
 declare @existing_password nvarchar(200),  
         @user_code nvarchar(5),
		 @user_id uniqueidentifier
  
 select @existing_password = login_password  
 from institutions_reg   
 where id = @reg_id   
  
 -- check if old password supplied matches existing password..  
 if(rtrim(ltrim(@existing_password))<> rtrim(ltrim(@old_password)))  
  begin  
   select @error_code='013',@return_status=0  
   return 0  
  end  
   
 --user is valid, update password..  
 begin transaction  
  
 select @user_id = id from users where email_id = @email_id and login_id=@login_id
  
 update users  
 set password      = @new_password,  
  update_by      = @user_id,  
  date_updated     = getdate()  
 where email_id = @email_id and login_id=@login_id

 update institutions_reg
 set login_password=@new_password,
 updated_by      = @user_id
 where id = @reg_id 
   
 if(@@rowcount<0)  
  begin  
   select @error_code='014',@return_status=0  
   rollback transaction  
   return 0  
  end  
    
    
 select @error_code='015',@return_status=1  
 commit transaction  
 return 1  
end  
GO
