USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_save]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_save : save
                  billing account 
** Created By   : Pavel Guha
** Created On   : 22/10/2019
*******************************************************/
CREATE procedure [dbo].[master_billing_account_save]
(
	@id						 uniqueidentifier='00000000-0000-0000-0000-000000000000' output,
	@code                    nvarchar(5)   = '' output,
	@name		             nvarchar(100)	= '',
	@address_Line1			 nvarchar(100)	= '',
	@address_Line2			 nvarchar(100)	= '',
	@city					 nvarchar(100)	= '',
	@state_id				 int			= 0,
	@country_id				 int			= 0,
	@zip			     	 nvarchar(20)	= '',
	@email_id			 	 nvarchar(50)	= '',
	@salesperson_id          uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@commission_1st_yr       decimal(5,2)    = 0,
	@commission_2nd_yr       decimal(5,2)    = 0,
	@login_id                nvarchar(50)    = '',
	@login_pwd               nvarchar(200)   = '',
	@user_email_id           nvarchar(50)    = '',
	@user_mobile_no          nvarchar(20)    = '',
	@notification_pref       nchar(1)        = 'N',
	@discount_per            decimal(5,2)    = 0,
	@accountant_name		 nvarchar(250)   = '',
	@is_active				 nchar(1)		 = 'Y',
	@xml_institution         ntext           = null,
	@xml_contacts            ntext           = null,
	@xml_modality_fees       ntext           = null,
	@xml_service_fees         ntext           = null,
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
			@hDoc3 int,
			@hDoc4 int,
			@hDoc5 int,
		    @counter bigint,
	        @rowcount bigint,
			@rc int,
			@ctr int,
			@inst_del nchar(1),
			@inst_id uniqueidentifier,
			@last_code_id int,
			@institution_user_id uniqueidentifier,
			@institution_user_role_id int,
			@account_user_id uniqueidentifier,
			@account_user_code nvarchar(20),
			@account_user_role_id int,
			@other_user_id uniqueidentifier,
			@user_code nvarchar(10),
			@user_role_id int,
			@update_qb nchar(1),
			@balance money,
			@study_count int

    declare @old_name nvarchar(100),
			@old_address_1 nvarchar(100),
			@old_address_2 nvarchar(100),
			@old_city      nvarchar(100),
			@old_state_id  int,  
			@old_country_id int,
			@old_zip        nvarchar(100),
			@old_user_email_id nvarchar(50) ,
			@old_is_active nchar(1),
			@old_login_id nvarchar(50)

	declare @institution_id uniqueidentifier,
			@institution_name nvarchar(100),
			@consult_applicable nchar(1),
			@storage_applicable nchar(1)

	declare @physician_id uniqueidentifier,
			 @physician_fname nvarchar(80),
			 @physician_lname nvarchar(80),
			 @physician_credentials nvarchar(30),
			 @physician_name nvarchar(200),
			 @physician_email nvarchar(500),
			 @physician_mobile nvarchar(500)


	 declare @rate_id uniqueidentifier,
			 @fee_amount money,
			 @fee_amount_per_unit money,
			 @study_max_amount money,
			 @fee_amount_after_hrs money,
			 @fee_row_id int

	declare @cd int,
			@new_code_var nvarchar(8)

	declare @phone_no	nvarchar(30),
			@fax_no		nvarchar(30),
			@contact_person_name	 nvarchar(100),
			@contact_person_mobile	 nvarchar(100),
			@contact_person_email_id nvarchar(50)
	
	if(isnull(@code,'')= '')--billing account code
		begin
			select @cd = max(convert(int,code)) from billing_account
			set @cd = isnull(@cd,0) + 1
			select @code=replicate('0',5-len(convert(varchar,@cd)))+convert(varchar,@cd)
		end

	if(select count(id) from billing_account where upper(code) = @code and id<>@id)>0
		begin
				select @error_code='074',@return_status=0,@user_name=@name
				return 0
		end

	if(@is_active='Y')
		begin
			if(select count(id) from billing_account where upper(name) = @name and is_active='Y' and id<>@id)>0
				begin
						select @error_code='136',@return_status=0,@user_name=@name
						return 0
				end
		end
--if(@id = '00000000-0000-0000-0000-000000000000') 
	--	begin
	--		set @account_user_id = newid()
	--    end
	--else
	--	begin
	--		select @account_user_code = login_id from billing_account where id=@id
	--		select @account_user_id = id from users where upper(login_id)=upper(@account_user_code)
	--	end

	
	

	--set @account_user_id = isnull(@account_user_id,'00000000-0000-0000-0000-000000000000')

	select @institution_user_role_id = id
	from user_roles
	where code='IU'

	--if(@account_user_id <> '00000000-0000-0000-0000-000000000000')
	--	begin
	--		if(select count(id) from users where upper(login_id) = upper(@login_id) and id<>@account_user_id)>0
	--			begin
	--					select @error_code='108',@return_status=0
	--					return 0
	--			end
	--	end

	begin transaction
	if(@xml_institution is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_institution 
	if(@xml_contacts is not null) exec sp_xml_preparedocument @hDoc3 output,@xml_contacts 
	if(@xml_modality_fees is not null) exec sp_xml_preparedocument @hDoc2 output,@xml_modality_fees 
	if(@xml_service_fees is not null) exec sp_xml_preparedocument @hDoc5 output,@xml_service_fees
	if(@xml_physicians is not null) exec sp_xml_preparedocument @hDoc4 output,@xml_physicians 

	if(@id = '00000000-0000-0000-0000-000000000000') 
		begin
			if(select count(id) 
			   from users 
			   where upper(login_id) = upper(@login_id))>0
				begin
					rollback transaction
					select @error_code='108',@return_status=0
					return 0
				end
	    end

	
	create table #tmpID(id int)

	if(@id = '00000000-0000-0000-0000-000000000000')
		begin
			
			set @id	=newid()
			insert into billing_account
						(
							id,code,name,qb_name,address_1,address_2,city,state_id,country_id,zip,
							salesperson_id,commission_1st_yr,commission_2nd_yr,
							login_id,login_pwd,user_email_id,user_mobile_no,notification_pref,
							discount_per,accountant_name,
							is_active,is_new,update_qb,created_by,date_created

						)
					values
						(
							@id,@code,@name,@name,@address_Line1,@address_Line2,@city,@state_id,@country_id,@zip,
							@salesperson_id,@commission_1st_yr,@commission_2nd_yr,
							@login_id,@login_pwd,@user_email_id,@user_mobile_no,@notification_pref,
							@discount_per,@accountant_name,
							@is_active,'N','Y',@updated_by,getdate()
						)

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
					if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
					select	@return_status=0,@error_code='035'
					return 0
				end

			select @account_user_role_id = user_role_id
			from users
			where id=@account_user_id

			select @user_role_id = id from user_roles where code='AU'

			if(select count(id) from users where login_id=@login_id)>0
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
					if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
					select @error_code='108',@return_status=0
					return 0
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
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='109',@return_status=0
							return 0
						end

					insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
					(select @account_user_id,menu_id,@updated_by,getdate()
					from user_role_menu_rights
					where user_role_id = @user_role_id)

					update billing_account
					set login_user_id = @account_user_id
					where id=@id

					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='217',@return_status=0
							return 0
						end
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
					if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
					return 0
				end

			select @old_name          = name,
			       @old_address_1     = address_1,
				   @old_address_2     = address_2,
				   @old_city          = city,
				   @old_state_id      = state_id,  
				   @old_country_id    =country_id,
				   @old_zip           = zip ,
				   @old_user_email_id = user_email_id  ,
				   @old_is_active    = is_active,
				   @old_login_id     = login_id    
			from billing_account
			where id = @id

			if(@old_login_id <> @login_id)
				begin
					if(select count(id) from users 
					   where upper(login_id) = upper(@login_id)
					   and upper(login_id) not in (select upper(login_id)
					                               from institution_user_link
												   where institution_id in (select id
												                            from institutions 
																			where billing_account_id=@id)))>0
						begin
							rollback transaction
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='108',@return_status=0
							return 0
						end
				end

			set @update_qb='N'

			if(cast(@old_name as varbinary) <> cast(@name as varbinary)) set @update_qb='Y'
			if(@old_address_1 <> @address_Line1) set @update_qb='Y'
			if(@old_address_2 <> @address_Line2) set @update_qb='Y'
			if(@old_city <> @city) set @update_qb='Y'
			if(@old_state_id <> @state_id) set @update_qb='Y'
			if(@old_country_id <> @country_id) set @update_qb='Y'
			if(@old_zip <> @zip) set @update_qb='Y'
			if(@old_user_email_id <> @user_email_id) set @update_qb='Y'
			if(@old_is_active <> @is_active) set @update_qb='Y'

			update billing_account
					set code                    = @code,
						name					= @name,
						address_1				= @address_Line1,
						address_2				= @address_Line2,
						city					= @city,
						state_id				= @state_id,
						country_id				= @country_id,
						zip						= @zip,
						--email_id				= @email_id,
						--phone_no				= @phone_no,
						--fax_no   				= @fax_no,
						--contact_person_name		= @contact_person_name,
						--contact_person_mobile	= @contact_person_mobile,
						--contact_person_email_id = @contact_person_email_id,
						salesperson_id          = @salesperson_id,
						commission_1st_yr       = @commission_1st_yr,
						commission_2nd_yr       = @commission_2nd_yr,
						login_id                = @login_id,
						login_pwd               = @login_pwd,
						user_email_id           = @user_email_id,
						user_mobile_no          = @user_mobile_no,
						notification_pref       = @notification_pref,
						discount_per            = @discount_per,
						accountant_name			= @accountant_name,
						discount_updated_by     = @updated_by,
						discount_updated_on     = getdate(),
						is_active				= @is_active,
						is_new                  = 'N',
						update_qb               = @update_qb,
						updated_by				= @updated_by,
						date_updated			= getdate()
					where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
					select	@return_status=0,@error_code='035'
					return 0
				end

			select @account_user_id = login_user_id
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

					if(isnull(@account_user_role_id,0)>0)
						begin
							if(@account_user_role_id = @user_role_id)
								begin
									update users
									set name              = @login_id,
									    login_id          = @login_id,
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
											if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
											if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
											if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
											if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
											select	@return_status=0,@error_code='217'
											return 0
										end
								end
							else
								begin
									update users
									set code              = @user_code,
										name              = @login_id,
										login_id          = @login_id,
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
											if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
											if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
											if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
											if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
											select	@return_status=0,@error_code='217'
											return 0
										end

									delete from institution_user_link where user_id = @account_user_id
								end
						end
					else
						begin
							select @other_user_id = id from users where login_id=@login_id
							delete from institution_user_link where user_login_id=@login_id
							delete from user_menu_rights where user_id = @other_user_id
							delete from users where login_id=@login_id

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
									if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
									if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
									if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
									select @error_code='109',@return_status=0
									return 0
								end

							if(select count(user_id) from user_menu_rights where user_id=@account_user_id)=0
								begin
									insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
									(select @account_user_id,menu_id,@updated_by,getdate()
									from user_role_menu_rights
									where user_role_id = @user_role_id)

									if(@@rowcount=0)
										begin
											rollback transaction
											if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
											if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
											if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
											if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
											select @error_code='217',@return_status=0
											return 0
										end
								end
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
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='109',@return_status=0
							return 0
						end

					update billing_account
					set login_user_id = @account_user_id
					where id=@id

					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
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

	  end

	create table #tmpOldInst(rec_id int identity(1,1),
	                         institution_id uniqueidentifier,
							 del nchar(1) default 'N')

    insert into #tmpOldInst(institution_id)
	(select institution_id from billing_account_institution_link where billing_account_id=@id)

	select @rc=@@rowcount,@ctr=1
	if(@xml_institution is not null)
		begin
			update #tmpOldInst
			set del='Y'
			where institution_id not in (select  institution_id
					                     from openxml(@hDoc1,'institution/row',2)
										 with( 
												institution_id uniqueidentifier,
												consult_applicable nchar(1),
												storage_applicable nchar(1),
												row_id bigint
											) xmlTemp)
		end

    if(select count(institution_id) from #tmpOldInst where del='Y')>0
		begin
			while(@ctr<=@rc)
				begin
					select @inst_id = institution_id,
					       @inst_del = del
					from  #tmpOldInst
					where rec_id = @ctr

					if(@inst_del='Y')
						begin
							if(select count(iid.study_id)
							   from invoice_institution_dtls iid
							   inner join invoice_hdr ih on ih.id = iid.hdr_id
							   where iid.institution_id=@inst_id
							   and iid.billing_account_id = @id)>0
									begin
										  rollback transaction  
										  if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							              if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							              if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							              if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4 
										  select @user_name = name from institutions where id = @inst_id
										  select @error_code='502',@return_status=0
										  return 0  
									end
						end

					set @ctr=@ctr + 1
				end
		end

	 
    drop table #tmpOldInst
	delete from billing_account_institution_link where billing_account_id=@id
	delete from institution_salesperson_link where salesperson_id = @salesperson_id and  billing_account_id = @id
	if(@xml_institution is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc1,'institution/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @institution_id    = institution_id,
					        @consult_applicable= consult_applicable,
							@storage_applicable= storage_applicable
					from openxml(@hDoc1,'institution/row',2)
					with
					( 
						institution_id uniqueidentifier,
						consult_applicable nchar(1),
						storage_applicable nchar(1),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					select @institution_name = name from institutions where id=@institution_id

					insert into billing_account_institution_link(billing_account_id,institution_id,updated_by,date_updated)
															values(@id,@institution_id,@updated_by,getdate())
					                                              
					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='066',@return_status=0,@user_name=@institution_name 
							return 0
						end

					update institutions
					set link_existing_bill_acct='Y',
					    consult_applicable = @consult_applicable,
						storage_applicable = @storage_applicable,
						billing_account_id = @id,
						updated_by         = @updated_by,
						date_updated       = getdate()
					where id =@institution_id

					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='066',@return_status=0,@user_name=@institution_name 
							return 0
						end


					select @institution_user_id = id
					from users
					where login_id = @login_id

					select @institution_user_role_id = id
					from user_roles
					where code='IU'

					if(isnull(@institution_user_id,'00000000-0000-0000-0000-000000000000')<> '00000000-0000-0000-0000-000000000000')
						begin
							if(select count(id) from users where login_id = @login_id and user_role_id = @institution_user_role_id)>0
								begin
									delete 
									from users
									where id = @institution_user_id

									if(@@rowcount=0)
										begin
											rollback transaction
											if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
											if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
											if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
											if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
											select @error_code='227',@return_status=0,@user_name=@institution_name 
											return 0
										end
								end
						end

					if(isnull(@salesperson_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
						begin
							if(select count(salesperson_id) from institution_salesperson_link where institution_id=@institution_id and salesperson_id=@salesperson_id)=0
								begin
										insert into institution_salesperson_link(salesperson_id,institution_id,billing_account_id,salesperson_fname,salesperson_lname,salesperson_name,
																				 salesperson_login_email,salesperson_email,salesperson_mobile,salesperson_user_id,
																				 salesperson_pacs_user_id,salesperson_pacs_password,
																				 commission_1st_yr,commission_2nd_yr,
																				 created_by,date_created)
																		(select sp.id,@institution_id,@id,sp.fname,sp.lname,rtrim(ltrim(isnull(sp.fname,'') + ' ' + isnull(sp.lname,''))),
																				sp.email_id,sp.email_id,sp.mobile_no,u.id,
																				sp.pacs_user_id,sp.pacs_password,
																				@commission_1st_yr,@commission_2nd_yr,
																				@updated_by,getdate()
																		 from salespersons sp
																		 inner join users u on u.code=sp.code
																		 where sp.id = @salesperson_id)

										
								end
							else
								begin
									update institution_salesperson_link
									set billing_account_id=@id
									where institution_id=@institution_id 
									and salesperson_id=@salesperson_id
								end
					
							if(@@rowcount=0)
								begin
									rollback transaction
									if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
									if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
									if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
									select @error_code='066',@return_status=0,@user_name=@institution_name 
									return 0
								end

						end
			
				
					set @counter = @counter + 1
				end
		end

    delete from billing_account_contacts where billing_account_id=@id
	if(@xml_contacts is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc3,'contact/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @institution_id         = institution_id,
					        @phone_no               = phone_no,
							@fax_no                 = fax_no,
							@contact_person_name    = contact_person_name,
							@contact_person_mobile  = contact_person_mobile,
							@contact_person_email_id=contact_person_email_id
					from openxml(@hDoc3,'contact/row',2)
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
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='066',@return_status=0,@user_name=@institution_name 
							return 0
						end

				
					set @counter = @counter + 1
				end
		end

	delete from billing_account_modality_fee_schedule where billing_account_id=@id
	if(@xml_modality_fees is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc2,'fees/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @rate_id             = rate_id,
							@fee_amount          = fee_amount,
							@fee_amount_per_unit = fee_amount_per_unit,
							@study_max_amount    = study_max_amount,
							@fee_row_id          = fee_row_id
					from openxml(@hDoc2,'fees/row',2)
					with
					( 
						rate_id uniqueidentifier,
						fee_amount money,
						fee_amount_per_unit money,
						study_max_amount money,
						fee_row_id int,
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  


					insert into billing_account_modality_fee_schedule(billing_account_id,rate_id,fee_amount,fee_amount_per_unit,study_max_amount,updated_by,date_updated)
															   values(@id,@rate_id,@fee_amount,@fee_amount_per_unit,@study_max_amount,@updated_by,getdate())
					                                              

					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='135',@return_status=0,@user_name=convert(varchar,@fee_row_id)
							return 0
						end
					

					set @counter = @counter + 1
				end
		end

	delete from billing_account_service_fee_schedule where billing_account_id=@id
	if(@xml_service_fees is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc5,'fees/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @rate_id              = rate_id,
							@fee_amount           = fee_amount,
							@fee_amount_after_hrs = fee_amount_after_hrs,
							@fee_row_id           = fee_row_id
					from openxml(@hDoc5,'fees/row',2)
					with
					( 
						rate_id uniqueidentifier,
						fee_amount money,
						fee_amount_after_hrs money,
						fee_row_id int,
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  


					insert into billing_account_service_fee_schedule(billing_account_id,rate_id,fee_amount,fee_amount_after_hrs,updated_by,date_updated)
															   values(@id,@rate_id,@fee_amount,@fee_amount_after_hrs,@updated_by,getdate())
					                                              

					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='455',@return_status=0,@user_name=convert(varchar,@fee_row_id)
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
			from openxml(@hDoc4,'physician/row', 2)  
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
					from openxml(@hDoc4,'physician/row',2)
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
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
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
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
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
							if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
							if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
							if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
							select @error_code='228',@return_status=0,@user_name=@physician_name + ' of ' + @institution_name
							return 0
						end

					set @counter = @counter + 1
				end
		end

	if(@is_active='N')
		begin
			    --check out standing invoices
				select @balance  =  a.total_amount-sum(a.adjusted) 
				from (
						select 'O' adj_source, hdr.id,hdr.invoice_no,hdr.opbal_date invoice_date,billing_cycle_id='00000000-0000-0000-0000-000000000000',billing_cycle='Opening Balance',hdr.opbal_amount total_amount, isnull(aj.adj_amount,0) adjusted 
						from ar_opening_balance hdr with(nolock) 
						left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id 
						left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
						where ba.id = @id
						UNION ALL
						select 'I' adj_source, hdr.id,hdr.invoice_no,hdr.invoice_date,hdr.billing_cycle_id,billing_cycle=bc.name,hdr.total_amount, isnull(aj.adj_amount,0) adjusted 
						from invoice_hdr hdr with(nolock) 
						left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and ISNULL(aj.adj_source,'I')='I'
						left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
						inner join billing_cycle bc on bc.id = hdr.billing_cycle_id
						where ba.id = @id
						and isnull(aj.adj_amount,0)>=0
						and hdr.approved='Y'	)a
			  group by a.id, a.invoice_no, a.invoice_date,a.billing_cycle_id,a.billing_cycle,a.total_amount
			  having a.total_amount-sum(a.adjusted)>0
			  order by a.invoice_date

			  if(@balance > 0)
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
					if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
					select @error_code='499',@return_status=0,@user_name=convert(varchar(15),round(@balance,2))
					return 0
				end

			select @study_count = count(id) from study_hdr where invoiced='N' and institution_id in (select institution_id from billing_account_institution_link where billing_account_id=@id)
			select @study_count = @study_count + (select count(id) from study_hdr_archive where invoiced='N' and institution_id in (select institution_id from billing_account_institution_link where billing_account_id=@id))

			if(@study_count)>0
				begin
					rollback transaction
					if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
					if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
					select @error_code='500',@return_status=0,@user_name=convert(varchar(10),@study_count)
					return 0
				end

			delete from ar_promotion_modality where billing_account_id = @id	
			delete from ar_promotion_institution where billing_account_id = @id
			delete from ar_promotions where billing_account_id = @id
		end


	commit transaction
	if(@xml_institution is not null) exec sp_xml_removedocument @hDoc1
	if(@xml_modality_fees is not null) exec sp_xml_removedocument @hDoc2
	if(@xml_contacts is not null) exec sp_xml_removedocument @hDoc3
	if(@xml_physicians is not null) exec sp_xml_removedocument @hDoc4
	if(@xml_service_fees is not null) exec sp_xml_removedocument @hDoc5
	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
end



GO
