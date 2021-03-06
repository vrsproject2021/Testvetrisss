USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_salesperson_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_salesperson_save]
GO
/****** Object:  StoredProcedure [dbo].[master_salesperson_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_salesperson_save : save
                  salesperson 
** Created By   : Pavel Guha
** Created On   : 23/04/2019
*******************************************************/
CREATE procedure [dbo].[master_salesperson_save]
(
	@id						uniqueidentifier ='00000000-0000-0000-0000-000000000000' output,
	@fname                  nvarchar(80)='',
	@lname                  nvarchar(80)='',
	@address_Line1			nvarchar(100)	= '',
	@address_Line2			nvarchar(100)	= '',
	@city					nvarchar(100)	= '',
	@zip			     	nvarchar(20)	= '',
	@state_id				int				= 0,
	@country_id				int				= 0,
	@email_id   			nvarchar(50)	= '',
	@phone					nvarchar(30)	= '',
	@mobile					nvarchar(20)	= '',
	@login_id               nvarchar(50)    = '',
	@login_password         nvarchar(200)    = '',
	@pacs_user_id           nvarchar(20)    = '',
	@pacs_password          nvarchar(200)    = '',
	@is_active				char(1)			= 'Y',
	@notification_pref      nchar(1)        ='B',
	@updated_by             uniqueidentifier,
    @menu_id                int,
	@xml_institution		ntext           = null,
    @user_name              nvarchar(700) = '' output,
	@error_code				nvarchar(10)	= '' output,
    @return_status			int				= 0  output
)
as
begin
	set nocount on 
	
	 declare @institution_id uniqueidentifier,
	         @institution_name nvarchar(200),
			 @commission_1st_yr decimal(5,2),
			 @commission_2nd_yr decimal(5,2),
			 @code nvarchar(10),
			 @salesperson_user_id uniqueidentifier,
			 @last_code_id int,
			 @user_role_id int,
			 @counter bigint,
			 @rowcount bigint,
			 @hDoc1 int
			 


	--if(select count(id) from salespersons where upper(email_id) = @email_id and id<>@id)>0
	--	begin
	--			select @error_code='092',@return_status=0
	--			return 0
	--	end

    if(@id = '00000000-0000-0000-0000-000000000000') 
		begin
			set @salesperson_user_id = newid()
	    end
	else
		begin
			select @code = code from salespersons where id=@id
			select @salesperson_user_id = id from users where code=@code
		end

	if(select count(id) from users where upper(login_id) = upper(@login_id) and id<>@salesperson_user_id)>0
		begin
				select @error_code='108',@return_status=0
				return 0
		end

	begin transaction
	if(@xml_institution is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_institution 

	if(@id = '00000000-0000-0000-0000-000000000000')
		begin
			set @id	=newid()
			

			create table #tmpID(id int)

			select @user_role_id = id
			from user_roles
			where code='SALES'

			insert into #tmpID(id) 
			(select convert(int,substring(code,3,len(code)-2))
			 from users
			 where user_role_id=@user_role_id)
								
			select @last_code_id =max(id) 
			from #tmpID

			set @last_code_id = isnull(@last_code_id,0) + 1
			set @code = 'SP' + convert(varchar,@last_code_id)

			

			
			drop table #tmpID

			insert into salespersons(id,code,fname,lname,name,
			                         address_1,address_2,city,state_id,country_id,zip,
									 email_id,phone_no,mobile_no,notification_pref,
									 login_id,login_pwd,pacs_user_id,pacs_password,
									 is_active,created_by,date_created

			)
			values
			(
				@id,@code,@fname,@lname,rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
				@address_Line1,@address_Line2,@city,@state_id,@country_id,@zip,
				@email_id,@phone,@mobile,@notification_pref,
				@login_id,@login_password,@pacs_user_id,@pacs_password,
				@is_active,@updated_by,getdate()
			)

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					select	@return_status=0,@error_code='035'
					return 0
				end

			insert into users(id,code,name,login_id,password,
							  email_id,contact_no,user_role_id,
							  pacs_user_id,pacs_password,notification_pref,
							  is_active,is_visible,created_by,date_created)
					   values(@salesperson_user_id,@code,rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),@login_id,@login_password,
							  @email_id,@mobile,@user_role_id,
							  @pacs_user_id,@pacs_password,@notification_pref,
							  'Y','N',@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					select @error_code='109',@return_status=0
					return 0
				end

			insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
			(select @salesperson_user_id,menu_id,@updated_by,getdate()
			from user_role_menu_rights
			where user_role_id = @user_role_id)

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					select @error_code='125',@return_status=0
					return 0
				end
			
		end
	else
		begin
			exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @id,
				@user_id       = @updated_by,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
			if(@return_status=0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					return 0
				end

			update salespersons
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
						login_pwd               = @login_password,
						pacs_user_id            = @pacs_user_id,
						pacs_password           = @pacs_password,
						notification_pref       = @notification_pref,
						is_active				= @is_active,
						updated_by				= @updated_by,
						date_updated			= getdate()
					where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					select	@return_status=0,@error_code='035'
					return 0
				end
			
			--select @code = code from salespersons where id=@id
			--select @salesperson_user_id = id from users where code=@code

			update users
			set name              = rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
				email_id          = @email_id,
				contact_no        = @mobile,
				notification_pref = @notification_pref,
				login_id          = @login_id,
				password          = @login_password,
				pacs_user_id      = @pacs_user_id,
				pacs_password     = @pacs_password,
				update_by         = @updated_by,
				date_updated      = getdate()
			where id = @salesperson_user_id

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					select @error_code='109',@return_status=0
					return 0
				end
		end

	exec common_lock_record_ui
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @updated_by,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end

	-- ************Institution Salesperson Link****************

	if(@xml_institution is not null)-- Added on 4th SEP 2019 @BK
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc1,'institution/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @institution_id         = institution_id,
							@commission_1st_yr      = commission1Year,
							@commission_2nd_yr      = commission2Year
					from openxml(@hDoc1,'institution/row',2)
					with
					( 
						institution_id uniqueidentifier,
						commission1Year decimal(5,2),
						commission2Year decimal(5,2),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  
			
					if(select count(institution_id) from institution_salesperson_link where salesperson_id=@id and institution_id= @institution_id) = 0
						begin
							insert into institution_salesperson_link(salesperson_id,institution_id,salesperson_fname,salesperson_lname,salesperson_name,
																	 salesperson_login_email,salesperson_email,salesperson_mobile,salesperson_user_id,
																	 salesperson_pacs_user_id,salesperson_pacs_password,
																	 commission_1st_yr,commission_2nd_yr,
																	 created_by,date_created)
														(select @id,@institution_id,sp.fname,sp.lname,rtrim(ltrim(isnull(sp.fname,'') + ' ' + isnull(sp.lname,''))),
																sp.email_id,sp.email_id,sp.mobile_no,u.id,
																sp.pacs_user_id,sp.pacs_password,
																@commission_1st_yr,@commission_2nd_yr,
																@updated_by,getdate()
														 from salespersons sp
														 inner join users u on u.code=sp.code
														 where sp.id = @id)
					                                              
							if(@@rowcount=0)
								begin
									rollback transaction
									if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
													
									select @error_code='066',@return_status=0
									return 0
								end
						end
					else
						begin
							update institution_salesperson_link
							set salesperson_fname			= isnull((select fname from salespersons where id = @id),''),
								salesperson_lname			= isnull((select lname from salespersons where id = @id),''),
								salesperson_name			= isnull((select rtrim(ltrim(isnull(fname,'') + ' ' + isnull(lname,''))) from salespersons where id = @id),''),
								salesperson_login_email		= isnull((select email_id from salespersons where id = @id),''),
								salesperson_email			= isnull((select email_id from salespersons where id = @id),''),
								salesperson_mobile			= isnull((select mobile_no from salespersons where id = @id),''),
								salesperson_user_id			= (select id from users where code= ( select code from salespersons where id = @id)),
								salesperson_pacs_user_id	= isnull((select pacs_user_id from salespersons where id = @id),''),
								salesperson_pacs_password	= isnull((select pacs_password from salespersons where id = @id),''),
								commission_1st_yr			= @commission_1st_yr,-- Added on 4th SEP 2019 @BK
								commission_2nd_yr			= @commission_2nd_yr,-- Added on 4th SEP 2019 @BK
								updated_by					= @updated_by,
								date_updated				= getdate()
							where institution_id = @institution_id
							and salesperson_id   = @id

							if(@@rowcount=0)
								begin
									rollback transaction
									if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
								    
									select @error_code='066',@return_status=0
									return 0
								end
						end

					set @counter = @counter + 1
				end
		end

	-- ***********~~*Institution Salesperson Link****************


	commit transaction
	if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
end



GO
