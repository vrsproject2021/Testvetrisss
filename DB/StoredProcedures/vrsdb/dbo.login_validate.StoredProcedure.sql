USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[login_validate]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[login_validate]
GO
/****** Object:  StoredProcedure [dbo].[login_validate]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : login_validate : validate user login
** Created By   : Pavel Guha
** Created On   : 09/04/2019
*******************************************************/
-- exec login_validate 'support@rad365.com','5bdsT/hQ1eCVQsGC7ekxiA==','00000000-0000-0000-0000-000000000000','','','00000000-0000-0000-0000-000000000000','',0
CREATE procedure [dbo].[login_validate]
    @login_id nvarchar(100),
	@password nvarchar(200),
	@id uniqueidentifier = '00000000-0000-0000-0000-000000000000' output,
	@code varchar(5) ='' output,
	@name nvarchar(100) ='' output,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000' output,
	@error_code nvarchar(10)='' output,
	@return_status int =0 output
as
begin
	set nocount on
	declare @lock_date  datetime,
			@lock_date1  datetime,
			@lock_date2  datetime,
	        @lock_record_count int,
			@first_login nchar(1),
			@user_role_id int,
			@user_role_code nvarchar(5)

		
	if (select count(id) from users where upper(login_id)=upper(@login_id) and is_active='Y')>0
		begin
			select top 1 @id=id from users where upper(login_id)=upper(@login_id) and is_active='Y'
		end
	else
		begin
			select @error_code='000',@return_status=0 /*invalid user login id id*/
			return 0
		end

	if(select isnull(is_active,'N') from users where id=@id)='N'
		begin
			select @error_code='002',@return_status=0 /*user blocked*/
			return 0
		end
		
    select @code = code,
		   @name = name,
		   @first_login=isnull(first_login,'Y'),
		   @user_role_id = user_role_id,
		   @session_id = newid()
	from users 
	where id=@id
    
	begin transaction
	--print @id
	--print @password
	--print @first_login
	select @user_role_code = code from user_roles where id = @user_role_id
	 
	if(@first_login='N')
		begin
			if(select count(id) from users where id=@id and ltrim(rtrim(password))=ltrim(rtrim(@password))) = 0
				begin
					rollback transaction
					delete from tmp_login_failed where id=@id
					insert into  tmp_login_failed(id,code,name,login_id,password,password_entered,email_id,contact_no,date_failed)
					                      (select id,code,name,login_id,password,@password,email_id,contact_no,getdate()
										   from users
										   where id=@id)
 					select @error_code='003',@return_status=0 /*invalid password*/
					return 0
				end
			else
				begin
				   if(@user_role_code<>'RDL')
					begin
						    /*check if an entry exists for this user in the logged_user table*/
							if((select count([user_id]) from sys_user_lock where [user_id] = @id) > 0)
								begin
									select @lock_record_count =  count([user_id]) from sys_record_lock where [user_id] = @id
									select @lock_record_count =  isnull(@lock_record_count,0) + count([user_id]) from sys_record_lock_ui where [user_id] = @id

									if(isnull(@lock_record_count,0)>0)
										begin
											select @lock_date1 = max(locked_on)
											from sys_record_lock 
											where [user_id] = @id
							
											select @lock_date2 = max(locked_on)
											from sys_record_lock_ui
											where [user_id] = @id
							
											if(isnull(@lock_date1,'01Jan1900') > isnull(@lock_date2,'01Jan1900'))
												begin
													set @lock_date = @lock_date1
												end
											else
												begin
													set @lock_date = @lock_date2
												end
								
											if(select datediff(mi,@lock_date,getdate()))>30
												begin
													delete from sys_record_lock where [user_id] = @id
													delete from sys_record_lock_ui where [user_id] = @id
													delete from sys_user_lock where [user_id] = @id
												end
											else
												begin
													/*User already logged in*/
													rollback transaction
													select @error_code='004',@return_status=0
													return 0
												end
								
										end
									else
										begin
											delete from sys_record_lock where [user_id] = @id
											delete from sys_record_lock_ui where [user_id] = @id
											delete from sys_user_lock where [user_id] = @id
										end
									--commit transaction
								end
					end
					
				end
		end
    else
		begin
			update users set first_login = 'N',password=@password where id=@id
			if(@@rowcount = 0)
				begin
					select @error_code='005',@return_status=0
					rollback transaction
					return 0
				end 
				--commit transaction
		end

		--delete from sys_user_lock where user_id = @id
	
	 
	 insert into sys_user_lock(user_id,session_id,last_login)
			           values (@id,@session_id,getdate())

	 if(@@rowcount = 0)
		begin
			/* Unable to insert to sys_logged_user table */
			rollback transaction
			select @error_code='006',@return_status=0
			return 0
		end
		
	exec common_user_activity_log
		 @user_id       = @id,
		 @activity_text = 'Logged In',
		 @session_id    = @session_id,
		 @menu_id       = 0,
		 @error_code    = @error_code output,
		 @return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end


	select @error_code='',@return_status=1
	commit transaction
	return 1
			

	
end

GO
