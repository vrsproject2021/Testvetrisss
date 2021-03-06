USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[profile_billing_account_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[profile_billing_account_save]
GO
/****** Object:  StoredProcedure [dbo].[profile_billing_account_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : profile_billing_account_save : save
                  billing account 
** Created By   : Pavel Guha
** Created On   : 17/02/2020
*******************************************************/
create procedure [dbo].[profile_billing_account_save]
(
	@id						uniqueidentifier='00000000-0000-0000-0000-000000000000' output,
	@login_id                nvarchar(50)    = '',
	@login_pwd               nvarchar(200)   = '',
	@user_email_id           nvarchar(50)    = '',
	@user_mobile_no          nvarchar(20)    = '',
	@notification_pref       nchar(1)        = 'N',
	@xml_contacts            ntext           = null,
	@xml_physicians          ntext           = null,
	@updated_by              uniqueidentifier,
    @menu_id                 int,
    @user_name               nvarchar(500)	= '' output,
	@error_code				 nvarchar(10)	= '' output,
    @return_status			 int				= 0  output
)
as
begin
	set nocount on 
	
	declare @hDoc1 int,
			@hDoc2 int,
		    @counter bigint,
	        @rowcount bigint,
			@last_code_id int,
			@institution_user_id uniqueidentifier,
			@institution_user_role_id int,
			@account_user_id uniqueidentifier,
			@account_user_role_id int,
			@user_code nvarchar(10),
			@user_role_id int,
			@is_active nchar(1)

	declare @institution_id uniqueidentifier,
			@institution_name nvarchar(100)

	 declare @physician_id uniqueidentifier,
			 @physician_fname nvarchar(80),
			 @physician_lname nvarchar(80),
			 @physician_credentials nvarchar(30),
			 @physician_name nvarchar(200),
			 @physician_email nvarchar(500),
			 @physician_mobile nvarchar(500)



	declare @cd int,
			@new_code_var nvarchar(8)

	declare @phone_no	nvarchar(30),
			@fax_no		nvarchar(30),
			@contact_person_name	 nvarchar(100),
			@contact_person_mobile	 nvarchar(100),
			@contact_person_email_id nvarchar(50)
	
	
	

	select @institution_user_role_id = id
	from user_roles
	where code='IU'

	begin transaction
	if(@xml_contacts is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_contacts 
	if(@xml_physicians is not null) exec sp_xml_preparedocument @hDoc2 output,@xml_physicians 

	create table #tmpID(id int)

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
			return 0
		end

	select @is_active= is_active
	from billing_account
	where id = @id

	update billing_account
	set login_id                = @login_id,
		login_pwd               = @login_pwd,
		user_email_id           = @user_email_id,
		user_mobile_no          = @user_mobile_no,
		notification_pref       = @notification_pref,
		discount_updated_by     = @updated_by,
		discount_updated_on     = getdate(),
		is_new                  = 'N',
		updated_by				= @updated_by,
		date_updated			= getdate()
	where id = @id

	if(@@rowcount=0)
		begin
			rollback transaction
			if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
			if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
			select	@return_status=0,@error_code='035'
			return 0
		end

	select @account_user_id = isnull(login_user_id,'00000000-0000-0000-0000-000000000000')
	from billing_account
	where id=@id

	select @user_role_id = id
	from user_roles
	where code='AU'


	insert into #tmpID(id) 
	(select convert(int,substring(code,3,len(code)-2))
	from users 
	where user_role_id=@user_role_id)

	select @last_code_id =max(id) 
	from #tmpID

	set @last_code_id = isnull(@last_code_id,0) + 1
	set @user_code = 'AU' + convert(varchar,@last_code_id)

	drop table #tmpID

	if(isnull(@account_user_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
		begin
			select @account_user_role_id = user_role_id
			from users
			where id=@account_user_id

			if(@account_user_role_id = @user_role_id)
				begin
					update users
					set name              = @login_id,
						password          = @login_pwd,
						email_id          = @user_email_id,
						contact_no        = @user_mobile_no,
						pacs_user_id      = @login_id,
						pacs_password     = @login_pwd,
						notification_pref = @notification_pref,
						is_active         = @is_active
					where id = @account_user_id

					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
							select	@return_status=0,@error_code='217'
							return 0
						end
				end
			else
				begin
					update users
					set code              = @user_code,
						name              = @login_id,
						password          = @login_pwd,
						user_role_id      = @user_role_id,
						email_id          = @user_email_id,
						contact_no        = @user_mobile_no,
						pacs_user_id      = @login_id,
						pacs_password     = @login_pwd,
						notification_pref = @notification_pref,
						is_active         = @is_active
					where id = @account_user_id

					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
							select	@return_status=0,@error_code='217'
							return 0
						end

					delete from institution_user_link where user_id = @account_user_id
				end
		
			
		end
	else
		begin
			set @account_user_id = newid()
			insert into users(id,code,name,
								login_id,password,
								email_id,contact_no,user_role_id,
								pacs_user_id,pacs_password,notification_pref,
								is_active,is_visible,
								created_by,date_created)
						values(@account_user_id,@user_code,@login_id,
								@login_id,@login_pwd,
								@user_email_id,@user_mobile_no,@user_role_id,
								@login_id,@login_pwd,@notification_pref,
								@is_active,'Y',@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='109',@return_status=0
					return 0
				end

			update billing_account
			set login_user_id = @account_user_id
			where id=@id

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='217',@return_status=0
					return 0
				end
		end

	delete 
	from user_menu_rights
	where user_id = @account_user_id
	and menu_id in (select menu_id from user_role_menu_rights where user_role_id=@user_role_id)

	insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
	(select @account_user_id,menu_id,@updated_by,getdate()
	from user_role_menu_rights
	where user_role_id = @user_role_id)


    delete from billing_account_contacts where billing_account_id=@id
	if(@xml_contacts is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc1,'contact/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @institution_id         = institution_id,
					        @phone_no               = phone_no,
							@fax_no                 = fax_no,
							@contact_person_name    = contact_person_name,
							@contact_person_mobile  = contact_person_mobile,
							@contact_person_email_id=contact_person_email_id
					from openxml(@hDoc1,'contact/row',2)
					with
					( 
						institution_id uniqueidentifier,
						phone_no nvarchar(30),
						fax_no nvarchar(20),
						contact_person_name nvarchar(100),
						contact_person_mobile nvarchar(20),
						contact_person_email_id  nvarchar(50),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					select @institution_name = name from institutions where id=@institution_id

					insert into billing_account_contacts(billing_account_id,institution_id,phone_no,fax_no,contact_person_name,contact_person_mobile,contact_person_email_id,updated_by,date_updated)
												 values(@id,@institution_id,@phone_no,@fax_no,@contact_person_name,@contact_person_mobile,@contact_person_email_id,@updated_by,getdate())
					                                              
					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='066',@return_status=0,@user_name=@institution_name 
							return 0
						end

					update institutions
					set phone_no				      = @phone_no,
						mobile_no				      = @fax_no,
						contact_person_name		      = @contact_person_name,
						contact_person_mobile         = @contact_person_mobile
					where id = @institution_id

					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='066',@return_status=0,@user_name=@institution_name 
							return 0
						end
				
					set @counter = @counter + 1
				end
		end

	
	delete from billing_account_physicians where billing_account_id=@id
	if(@xml_physicians is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc2,'physician/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @institution_id          = institution_id,
							@physician_id            = physician_id,
					        @physician_fname         = physician_fname,
							@physician_lname         = physician_lname,
							@physician_credentials   = physician_credentials,
							@physician_email         = physician_email,
							@physician_mobile        = physician_mobile
					from openxml(@hDoc2,'physician/row',2)
					with
					( 
						institution_id uniqueidentifier,
						physician_id uniqueidentifier,
						physician_fname nvarchar(80),
						physician_lname nvarchar(80),
						physician_credentials nvarchar(30),
						physician_email nvarchar(500),
						physician_mobile nvarchar(500),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					select @physician_name =  rtrim(ltrim(rtrim(ltrim(@physician_fname)) + ' ' + rtrim(ltrim(@physician_lname)) + ' ' + rtrim(ltrim(@physician_credentials)))) 
					select @institution_name = rtrim(ltrim(name)) from institutions where id = @institution_id

					insert into billing_account_physicians(billing_account_id,institution_id,physician_id,updated_by,date_updated)
													values(@id,@institution_id,@physician_id,@updated_by,getdate())
					                                              
					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='228',@return_status=0,@user_name=@physician_name + ' of ' + @institution_name
							return 0
						end

					update institution_physician_link
					set physician_fname         = @physician_fname,
						physician_lname         = @physician_lname,
						physician_credentials   = @physician_credentials,   
						physician_name          = @physician_name,
						physician_email         = isnull(@physician_email,''),
						physician_mobile        = isnull(@physician_mobile,''),
						updated_by    = @updated_by,
						date_updated  = getdate()
					where physician_id = @physician_id 
					and institution_id = @institution_id
					
					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='228',@return_status=0,@user_name=@physician_name + ' of ' + @institution_name
							return 0
						end

					update physicians
					set fname         = @physician_fname,
						lname         = @physician_lname,
						credentials   = @physician_credentials,   
						name          = @physician_name,
						email_id      = isnull(@physician_email,''),
						updated_by    = @updated_by,
						date_updated  = getdate()
					where id = @physician_id 
					
					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='228',@return_status=0,@user_name=@physician_name + ' of ' + @institution_name
							return 0
						end

					set @counter = @counter + 1
				end
		end
	commit transaction
	if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc1
	if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc2
	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
end



GO
