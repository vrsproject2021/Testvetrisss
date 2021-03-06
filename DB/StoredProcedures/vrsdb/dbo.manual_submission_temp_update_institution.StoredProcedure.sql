USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[manual_submission_temp_update_institution]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[manual_submission_temp_update_institution]
GO
/****** Object:  StoredProcedure [dbo].[manual_submission_temp_update_institution]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec manual_submission_temp_update_institution '31A5913E-9C99-4B01-AAFF-581F4A380893','','',0

CREATE Proc [dbo].[manual_submission_temp_update_institution]
@id uniqueidentifier ,
@user_name nvarchar(700)= '' output,
@error_code nvarchar(10) = '' output,
@return_status int= 0  output
	As
	Begin
	set nocount on 

		declare @cd int,
				@new_code_var nvarchar(8),
				@code nvarchar(5)= '',
				@name nvarchar(100)= '',
				@counter bigint,
				@rowcount bigint,
				@last_code_id int,
				@physician_code nvarchar(10),
				@user_role_id int

		declare @physician_id uniqueidentifier,
			 @physician_fname nvarchar(80),
			 @physician_lname nvarchar(80),
			 @physician_credentials nvarchar(30),
			 @physician_name nvarchar(200),
			 @physician_email nvarchar(500),
			 @physician_mobile nvarchar(500)

		declare  @user_code nvarchar(10),
			 @user_login_id nvarchar(50),
	         @user_pwd nvarchar(50),
	         @user_pacs_user_id nvarchar(20),
			 @user_pacs_password nvarchar(200),
			 @user_user_id uniqueidentifier,
			 @user_email_id nvarchar(50),
			 @user_contact_no nvarchar(20),
			 @is_user_active nchar(1)='Y',
			 @updated_in_pacs nchar(1),
			 @old_user_pacs_user_id nvarchar(20),
			 @old_user_pacs_password nvarchar(200)

		select @name=[name],@code=code,@user_login_id=login_id,@user_pwd=login_password,@user_email_id=login_email_id,
				@user_contact_no=login_mobile_no,@user_pacs_user_id=login_id,@user_pacs_password=login_password
		from institutions_reg where id=@id

		if(isnull(@code,'')= '')
		begin
			select @cd = max(convert(int,code)) from institutions
			set @cd = isnull(@cd,0) + 1
			select @code=replicate('0',5-len(convert(varchar,@cd)))+convert(varchar,@cd)
		end

		if(select count(id) from institutions where upper(code) = @code and id<>@id)>0
		begin
				select @error_code='074',@return_status=0,@user_name=@name
				return 0
		end

	if(select count(id) from institutions where upper(name) = @name and is_active='Y' and id<>@id)>0
		begin
				select @error_code='136',@return_status=0,@user_name=@name
				return 0
		end
	
	begin transaction
	/* Insert Into Institution from  institutions_reg */

	insert into institutions
						(
							id,code,temp_code,name,address_1,address_2,city,state_id,country_id,zip,
							email_id,phone_no,mobile_no,contact_person_name,contact_person_mobile,notes,
							discount_per,accountant_name,-- Added on 3rd SEP 2019 @BK
							business_source_id,format_dcm_files,dcm_file_xfer_pacs_mode,study_img_manual_receive_path,
							consult_applicable,storage_applicable,custom_report,logo_img,image_content_type,
							is_active,is_new,created_by,date_created

						)
						SELECT id, @code,@code, name,address_1,address_2,city,state_id,country_id,zip,
								email_id,phone_no,mobile_no,contact_person_name,contact_person_mobile,'',
								0,'',
								0,'N','N','',
								'N','N','N',null,null,
								'Y','Y','00000000-0000-0000-0000-000000000000',GETDATE()
						FROM institutions_reg
						WHERE id=@id;

			if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end

	if(select count(physician_id) from institution_reg_physician_link where institution_id=@id)>0
					begin
					set @counter = 1
					select  @rowcount=(select count(*) from institution_reg_physician_link where institution_id=@id) 
					Select *
					Into   #Temp
					From   institution_reg_physician_link where institution_id=@id
					while(@counter <= @rowcount)
						Begin
							select  @physician_id            = physician_id,
							@physician_name=physician_name,
					        @physician_fname         = physician_fname,
							@physician_lname         = physician_lname,
							@physician_credentials   = physician_credentials,
							@physician_email         = physician_email,
							@physician_mobile        = physician_mobile
					from #Temp
					
						if(select count(physician_id) from institution_physician_link where upper(physician_name) = upper(@physician_name) and institution_id=@id)>0
									begin
										rollback transaction
										select @error_code='261',@return_status=0,@user_name=@physician_name
										return 0
									end
							
								insert into institution_physician_link(physician_id,institution_id,physician_fname,physician_lname,physician_credentials,
																		physician_name,physician_email,physician_mobile,created_by,date_created)
																values(@physician_id,@id,@physician_fname,@physician_lname,@physician_credentials,@physician_name,
																	   @physician_email,@physician_mobile,'00000000-0000-0000-0000-000000000000',getdate())
					                                              
								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='066',@return_status=0,@user_name=@physician_name
										return 0
									end

								select @last_code_id =max(convert(int,substring(code,5,len(code)-4))) 
								from physicians

								set @last_code_id = isnull(@last_code_id,0) + 1
								set @physician_code = 'PHYS' + convert(varchar,@last_code_id)
									
								insert into physicians(id,code,fname,lname,[credentials],name,institution_id,
								                       email_id,mobile_no,created_by,date_created) 
									            values (@physician_id,@physician_code,@physician_fname,@physician_lname,@physician_credentials,
												@physician_name,@id,@physician_email,@physician_mobile,'00000000-0000-0000-0000-000000000000',getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='066',@return_status=0,@user_name=@physician_name
										return 0
									end
						--Delete #temp table
						Delete #Temp Where physician_id = @physician_id
						set @counter = @counter + 1
					End
					
	End

	if(select isnull(login_id,'') from institutions_reg where id=@id)<>''
		Begin
			if(select count(id) from users where upper(login_id) = upper(@user_login_id) and is_active='Y')>0
				begin
						rollback transaction
						select @error_code='118',@return_status=0,@user_name=Convert(varchar,@counter)
						return 0
				end

			if(select count(user_login_id) 
				from institution_user_link 
				where upper(user_login_id) = upper(@user_login_id) 
				and institution_id=@id)>0
				begin
						rollback transaction
						select @error_code='114',@return_status=0,@user_name=Convert(varchar,@counter)
						return 0
				end

			set @user_user_id= newid()
			insert into institution_user_link(user_id,institution_id,user_login_id,user_pwd,user_pacs_user_id,user_pacs_password,
												user_email,user_contact_no,granted_rights_pacs,created_by,date_created)
											select @user_user_id,id,login_id,login_password,login_id,login_password,
													email_id,login_mobile_no,'EOWIN',@user_user_id,GETDATE()
											from institutions_reg where id=@id
					                                              
			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='113',@return_status=0,@user_name=@user_login_id
					return 0
				end

			select @user_role_id = id from user_roles where code='IU'
			create table #tmpID(id int)

			insert into #tmpID(id) 
			(select convert(int,substring(code,3,len(code)-2))
				from users 
				where user_role_id = @user_role_id)
								
			select @last_code_id =max(id) 
			from #tmpID

			set @last_code_id = isnull(@last_code_id,0) + 1
				set @user_code = 'IU' + convert(varchar,@last_code_id)

				set @last_code_id = isnull(@last_code_id,0) + 1
				set @user_code = 'IU' + convert(varchar,@last_code_id)

				drop table #tmpID

			insert into users(id,code,name,login_id,password,email_id,contact_no,user_role_id,
								pacs_user_id,pacs_password,is_active,is_visible,created_by,date_created) 
								values (@user_user_id,@user_code,@user_login_id,@user_login_id,@user_pwd,@user_email_id,@user_contact_no,@user_role_id,
								 @user_pacs_user_id,@user_pacs_password,@is_user_active,'Y',@user_user_id,getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='113',@return_status=0,@user_name=@user_login_id
										return 0
									end

								insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
								(select @user_user_id,menu_id,null,getdate()
								from user_role_menu_rights
								where user_role_id = @user_role_id)

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code='124',@return_status=0,@user_name=@user_login_id
										return 0
									end

				--Update CreatedBy field
				update institutions set created_by=@user_user_id where id=@id
				update institution_physician_link set created_by=@user_user_id where institution_id=@id
				update physicians set created_by=@user_user_id where institution_id=@id
		End

		commit transaction
	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
	End




GO
