USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_technicians_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_technicians_save]
GO
/****** Object:  StoredProcedure [dbo].[master_technicians_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_technicians_save : save
                  technicians details.
** Created By   : BK
** Created On   : 25/07/2019
*******************************************************/
CREATE procedure [dbo].[master_technicians_save]
(
	@id						uniqueidentifier	= '00000000-0000-0000-0000-000000000000' output,
	@fname                  nvarchar(80)		= '',
	@lname                  nvarchar(80)		= '',
	@address_Line1			nvarchar(100)		= '',
	@address_Line2			nvarchar(100)		= '',
	@city					nvarchar(100)		= '',
	@zip			     	nvarchar(20)		= '',
	@state_id				int					= 0,
	@country_id				int					= 0,
	@email_id   			nvarchar(50)		= '',
	@phone					nvarchar(30)		= '',
	@mobile					nvarchar(20)		= '',
	@login_id               nvarchar(50)		= '',
	@login_pwd				nvarchar(200)		= '',
	@default_fee			money				= 0,
	@is_active				char(1)				= 'Y',
	@notification_pref      nchar(1)            ='B',
	@user_id				uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(700)		= '' output,
	
	@error_code				nvarchar(10)		= '' output,
    @return_status			int					= 0  output
)
as
begin
	set nocount on 

	 declare	@code nvarchar(10),
				@technicians_user_id uniqueidentifier,
				@last_code_id int,
				@user_role_id int,
				@updated_in_pacs nchar(1),-- Added on 12th SEP 2019 @BK
				@old_user_login_id nvarchar(20),-- Added on 12th SEP 2019 @BK
				@old_user_login_password nvarchar(200)-- Added on 12th SEP 2019 @BK

	
	if(@id = '00000000-0000-0000-0000-000000000000') 
		begin
			set @technicians_user_id = newid()
	    end
	else
		begin
			select @code = code from technicians where id=@id
			select @technicians_user_id = id from users where code=@code
		end

	if(select count(id) from users where upper(login_id) = upper(@login_id) and id<>@technicians_user_id)>0
		begin
				select @error_code='108',@return_status=0
				return 0
		end

	begin transaction

	if(@id = '00000000-0000-0000-0000-000000000000')
		begin
			
			create table #tmpID(id int)

			select @user_role_id = id
			from user_roles
			where code='TCHN'

			insert into #tmpID(id) 
			(select convert(int,substring(code,5,len(code)-4))
			from users
			where user_role_id=@user_role_id)

			select @last_code_id =max(id) 
			from #tmpID

			set @last_code_id = isnull(@last_code_id,0) + 1
			set @code = 'TCHN' + convert(varchar,@last_code_id)

			

			drop table #tmpID

            set @id = newid()

			insert into technicians
			(
				id,
				code,
				fname,
				lname,
				name,
				address_1,
				address_2,
				city,
				state_id,
				country_id,
				zip,
				email_id,
				phone_no,
				mobile_no,
				login_user_id,
				login_id,
				login_pwd,
				default_fee,
				notification_pref,
				is_active,
				updated_in_pacs	,-- Added on 12th SEP 2019 @BK	
				granted_rights_pacs,-- Added on 12th SEP 2019 @BK
				
				created_by,
				date_created

			)
			values
			(
				@id,
				@code,
				@fname,
				@lname,
				rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
				
				@address_Line1,
				@address_Line2,
				@city,
				@state_id,
				@country_id,
				@zip,
				@email_id,
				@phone,
				@mobile,
				@technicians_user_id,		
				@login_id,           
				@login_pwd,
				@default_fee,	
				@notification_pref,	
				@is_active,
				'N',-- Added on 12th SEP 2019 @BK
				'OI',-- Added on 12th SEP 2019 @BK			

				@user_id,
				getdate()
			)

			if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end

			insert into users(id,code,name,
							  login_id,password,
							  email_id,contact_no,user_role_id,
							  pacs_user_id,pacs_password,notification_pref,
							  is_active,is_visible,
							  created_by,date_created)
					   values(@technicians_user_id,@code,rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
							  @login_id,@login_pwd,
							  @email_id,@mobile,@user_role_id,
							  @login_id,@login_pwd,@notification_pref,--pacs_user_id,pacs_password
							  'Y','N',@user_id,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='109',@return_status=0
					return 0
				end

			insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
			(select @technicians_user_id,menu_id,@user_id,getdate()
			from user_role_menu_rights
			where user_role_id = @user_role_id)

			/*if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='125',@return_status=0
					return 0
				end*/
			
		end
	else
		begin
			exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @id,
				@user_id       = @user_id,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
			if(@return_status=0)
				begin
					rollback transaction
					return 0
				end

			-- Added on 12th SEP 2019 @BK
			select @old_user_login_id			= login_id,
				   @old_user_login_password		= login_pwd
			from radiologists
				where id = @id
			if(UPPER( RTRIM(LTRIM(@old_user_login_id))) <> UPPER( RTRIM(LTRIM(@login_id))))
				begin
					set @updated_in_pacs ='N'
				end
			if(@old_user_login_password <> @login_pwd)
				begin
					set @updated_in_pacs ='N'
				end
			--~~ Added on 12th SEP 2019 @BK

			update technicians
					set	fname                   = @fname,
					    lname                   = @lname,
						name                    = rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
						address_1				= @address_Line1,
						address_2				= @address_Line2,
						city					= @city,
						state_id				= @state_id,
						country_id				= @country_id,
						zip						= @zip,
						email_id                = @email_id,
						phone_no				= @phone,
						mobile_no				= @mobile,
						login_id                = @login_id,
						login_pwd               = @login_pwd,
						default_fee				= @default_fee,
						notification_pref       = @notification_pref,		
						is_active				= @is_active,
						updated_in_pacs			= @updated_in_pacs,-- Added on 12th SEP 2019 @BK
						granted_rights_pacs		= 'OI',-- Added on 12th SEP 2019 @BK

						updated_by				= @user_id,
						date_updated			= getdate()
					
					where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end
			

			update users
			set name			  = rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
				email_id		  = @email_id,
				contact_no		  = @mobile,
				notification_pref = @notification_pref,
				login_id		  = @login_id,
				password		  = @login_pwd,
				update_by		  = @user_id,
				date_updated	  = getdate()
			where id = @technicians_user_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='109',@return_status=0
					return 0
				end
		end

	exec common_lock_record_ui
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @user_id,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end

	commit transaction

	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
end



GO
