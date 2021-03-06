USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[registration_institution_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[registration_institution_save]
GO
/****** Object:  StoredProcedure [dbo].[registration_institution_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : registration_institution_save : register
                  institution and verified email and mobile.
** Created By   : BK
** Created On   : 19/09/2019
*******************************************************/
/*

	exec registration_institution_save
	'55eeda7b-a02a-4297-8fa3-c9177ff87c78','VIMG','VETTECH IMAGING INC','kdoyle@vcradiology.com','','','Florida','',3930,231,'','','Kevin Doyle','','asdsd','Y',
	'<physician><row><physician_id>00000000-0000-0000-0000-000000000000</physician_id><physician_fname><![CDATA[Kevin]]></physician_fname><physician_lname><![CDATA[Doyle]]></physician_lname><physician_credentials><![CDATA[MD]]></physician_credentials><user_login_id><![CDATA[kdoyle@vcradiology.com]]></user_login_id><physician_email><![CDATA[amiguy05@yahoo.com]]></physician_email><physician_mobile><![CDATA[+16065719566]]></physician_mobile><user_pacs_user_id><![CDATA[Doylek]]></user_pacs_user_id><user_pacs_password><![CDATA[9UnEauERhsE=]]></user_pacs_password><row_id>1</row_id></row><row><physician_id>1e8c2dad-6e93-489e-b89a-77605e7c2a38</physician_id><physician_fname><![CDATA[Pavel]]></physician_fname><physician_lname><![CDATA[Guha]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><user_login_id><![CDATA[pavelguha@gmail.com]]></user_login_id><physician_email><![CDATA[pavelguha@gmail.com]]></physician_email><physician_mobile><![CDATA[+919831352184]]></physician_mobile><user_pacs_user_id><![CDATA[Doylek]]></user_pacs_user_id><user_pacs_password><![CDATA[9UnEauERhsE=]]></user_pacs_password><row_id>2</row_id></row></physician>',
	'11111111-1111-1111-1111-111111111111',18,'','',0
*/
CREATE procedure [dbo].[registration_institution_save]
(
	@id						uniqueidentifier='00000000-0000-0000-0000-000000000000' output,
	@code                   nvarchar(5)     = '' output,
	@name		            nvarchar(100)	= '',
	@email_id				nvarchar(50)	= '',
	@address_Line1			nvarchar(100)	= '',
	@address_Line2			nvarchar(100)	= '',
	@city					nvarchar(100)	= '',
	@zip			     	nvarchar(20)	= '',
	@state_id				int				= 0,
	@country_id				int				= 0,
	@phone					nvarchar(30)	= '',
	@mobile					nvarchar(30)	= '',
	@contact_person_name	nvarchar(100)	= '',
	@contact_person_mob		nvarchar(100)	= '',
	@discount_per           decimal(5,2)    = 0,
	@is_active				char(1)			= 'Y',
	@user_login_id			nvarchar(50)	= '',
	@user_pwd				nvarchar(200)	= '',
	@user_email_id			nvarchar(50)	= '',
	@xml_physician          ntext           = null,
	@updated_by             uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(700)	= '' output,
	@error_code				nvarchar(10)	= '' output,
    @return_status			int				= 0  output
)
as
begin
	set nocount on 
	
	declare @hDoc2 int,
		    @counter bigint,
	        @rowcount bigint,
			@last_code_id int,
			@physician_code nvarchar(10),
			@user_role_id int,
			@is_user_active nchar(1),
			@gen_verify_msg nchar(1),
			@is_new nchar(1),
			@VRSAPPLINK nvarchar(200),
			@SMSSENDERNO nvarchar(200)
		
	 declare @physician_id uniqueidentifier,
			 @physician_fname nvarchar(80),
			 @physician_lname nvarchar(80),
			 @physician_credentials nvarchar(30),
			 @physician_name nvarchar(200),
			 @physician_email nvarchar(500),
			 @physician_mobile nvarchar(500)
			
	declare @cd int,
			@user_code nvarchar(10),
			@new_code_var nvarchar(8),
			@user_user_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
			@email_log_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
			@sms_log_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
			@email_subject varchar(250) = 'VETCHOICE || EMAIL VERIFICATION',
			@email_text nvarchar(max) ,
			@sms_text nvarchar(max) 
	
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
	if(@xml_physician is not null) exec sp_xml_preparedocument @hDoc2 output,@xml_physician
	set @gen_verify_msg='N'
	set @is_new = 'N'

	if(@id = '00000000-0000-0000-0000-000000000000')
		begin
			set @id	=newid()
			set @is_new='Y'

			insert into institutions
						(
							id,code,name,address_1,address_2,city,state_id,country_id,zip,
							email_id,phone_no,mobile_no,contact_person_name,contact_person_mobile,discount_per,is_active,is_new,
							is_online,is_email_verified,is_mobile_verified,created_by,date_created

						)
					values
						(
							@id,@code,@name,
							@address_Line1,@address_Line2,@city,@state_id,@country_id,@zip,
							@email_id,@phone,@mobile,@contact_person_name,@contact_person_mob,@discount_per,@is_active,'Y',
							'Y','N','N',@updated_by,getdate()
						)

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
					select	@return_status=0,@error_code='035'
					return 0
				end
			else
				begin
					set @gen_verify_msg='Y'
				end
			
		end
	--else
	--	begin
	--		exec common_check_record_lock_ui
	--			@menu_id       = @menu_id,
	--			@record_id     = @id,
	--			@user_id       = @updated_by,
	--			@user_name     = @user_name output,
	--			@error_code    = @error_code output,
	--			@return_status = @return_status output
		
	--		if(@return_status=0)
	--			begin
	--				rollback transaction
	--				if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
	--				return 0
	--			end

	--		update institutions
	--				set
	--					code                    = @code,
	--					name					= @name,
	--					email_id				= @email_id,
	--					address_1				= @address_Line1,
	--					address_2				= @address_Line2,
	--					city					= @city,
	--					state_id				= @state_id,
	--					country_id				= @country_id,
	--					zip						= @zip,
	--					phone_no				= @phone,
	--					mobile_no				= @mobile,
	--					contact_person_name		= @contact_person_name,
	--					contact_person_mobile	= @contact_person_mob,
	--					discount_updated_by     = @updated_by,
	--					discount_updated_on     = getdate(),
	--					is_active				= @is_active,
	--					is_new                  = 'N',
	--					updated_by				= @updated_by,
	--					date_updated			= getdate()
	--				where id = @id

	--		if(@@rowcount=0)
	--			begin
	--				rollback transaction
	--				if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
	--				select	@return_status=0,@error_code='035'
	--				return 0
	--			end
		
	--  end


	delete from institution_physician_link where institution_id = @id

	if(@xml_physician is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc2,'physician/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @physician_id            = physician_id,
					        @physician_fname         = physician_fname,
							@physician_lname         = physician_lname,
							@physician_credentials   = physician_credentials,
							@physician_email         = physician_email,
							@physician_mobile        = physician_mobile
					from openxml(@hDoc2,'physician/row',2)
					with
					( 
						physician_id uniqueidentifier,
						physician_fname nvarchar(80),
						physician_lname nvarchar(80),
						physician_credentials nvarchar(30),
						physician_email nvarchar(500),
						physician_mobile nvarchar(500),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					select @physician_name =  rtrim(ltrim(rtrim(ltrim(@physician_fname)) + ' ' + rtrim(ltrim(@physician_lname)) + ' ' + rtrim(ltrim(@physician_credentials)))) 

					if(@physician_id <> '00000000-0000-0000-0000-000000000000')
						begin
							select @physician_code = code
							from physicians
							where id = @physician_id

							insert into institution_physician_link(physician_id,institution_id,physician_fname,physician_lname,physician_credentials,physician_name,
																	physician_email,physician_mobile,created_by,date_created)
															values(@physician_id,@id,@physician_fname,@physician_lname,@physician_credentials,@physician_name,
																    @physician_email,@physician_mobile,@updated_by,getdate())
					                                              

							if(@@rowcount=0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='066',@return_status=0,@user_name=@physician_name
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
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='066',@return_status=0,@user_name=@physician_name
									return 0
								end

							
						end
					else
						begin
							   set @physician_id= newid()

							
							
								insert into institution_physician_link(physician_id,institution_id,physician_fname,physician_lname,physician_credentials,physician_name,
									                                    physician_email,physician_mobile,created_by,date_created)
																values(@physician_id,@id,@physician_fname,@physician_lname,@physician_credentials,@physician_name,
																	   @physician_email,@physician_mobile,@updated_by,getdate())
					                                              
								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='066',@return_status=0,@user_name=@physician_name
										return 0
									end

								select @last_code_id =max(convert(int,substring(code,5,len(code)-4))) 
								from physicians

								set @last_code_id = isnull(@last_code_id,0) + 1
								set @physician_code = 'PHYS' + convert(varchar,@last_code_id)
									
								insert into physicians(id,code,fname,lname,credentials,name,
								                       email_id,mobile_no,created_by,date_created) 
									            values (@physician_id,@physician_code,@physician_fname,@physician_lname,@physician_credentials,@physician_name,
												        @physician_email,@physician_mobile,@updated_by,getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='066',@return_status=0,@user_name=@physician_name
										return 0
									end

								
						end

					set @counter = @counter + 1
				end
		end

	if(@is_active ='Y')
		begin
			if(select count(id) from users where upper(login_id) = upper(@user_login_id) and id<>@user_user_id and is_active='Y')>0
				begin
						rollback transaction
						if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
						select @error_code='118',@return_status=0,@user_name=Convert(varchar,@counter)
						return 0
				end

			if(select count(user_login_id) 
				from institution_user_link 
				where upper(user_login_id) = upper(@user_login_id) 
				and user_id <> @user_user_id 
				and institution_id=@id)>0
				begin
						rollback transaction
						if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
						select @error_code='114',@return_status=0,@user_name=Convert(varchar,@counter)
						return 0
				end
		end
	else
		begin
			set @is_user_active='N'
		end

	if(@user_user_id = '00000000-0000-0000-0000-000000000000')
		begin
			if(@is_active ='Y')
				begin
					if(select count(id) from users where upper(login_id) = upper(@user_login_id) and id<>@user_user_id and is_active='Y')>0
						begin
								rollback transaction
								if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
								select @error_code='118',@return_status=0,@user_name=Convert(varchar,@counter)
								return 0
						end

					if(select count(user_login_id) 
						from institution_user_link 
						where upper(user_login_id) = upper(@user_login_id) 
						and user_id <> @user_user_id 
						and institution_id=@id)>0
						begin
								rollback transaction
								if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
								select @error_code='114',@return_status=0,@user_name=Convert(varchar,@counter)
								return 0
						end

					set @is_user_active='N'
				end
			else
				begin
					set @is_user_active='N'
				end
			set @user_user_id= newid()
			insert into institution_user_link(user_id,      institution_id,user_login_id,user_pwd,     user_email,created_by,date_created)
									   values(@user_user_id,@id,           @user_login_id, @user_pwd,@user_email_id,@updated_by,getdate())
		
		--=========..?================================
			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='113',@return_status=0,@user_name=@physician_name
					return 0
				end

			create table #tmpID(id int)

			insert into #tmpID(id) 
			(select convert(int,substring(u.code,3,len(u.code)-2))
				from users u
				inner join user_roles ur on ur.id = u.user_role_id
				where ur.code='IU')
								
			select @last_code_id =max(id) 
			from #tmpID
								

			select @user_role_id = id
			from user_roles
			where code='IU'

			set @last_code_id = isnull(@last_code_id,0) + 1
			set @user_code = 'IU' + convert(varchar,@last_code_id)

			set @last_code_id = isnull(@last_code_id,0) + 1
			set @user_code = 'IU' + convert(varchar,@last_code_id)

			drop table #tmpID
									
			insert into users(id,code,name,login_id,password,email_id,user_role_id,
								pacs_user_id,pacs_password,is_active,is_visible,created_by,date_created) 
					values (@user_user_id,@user_code,@user_login_id,@user_login_id,@user_pwd,@user_email_id,@user_role_id,
							@user_login_id,@user_pwd,@is_user_active,'Y',@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='113',@return_status=0,@user_name=@user_login_id
					return 0
				end

			insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
			(select @user_user_id,menu_id,@updated_by,getdate()
			from user_role_menu_rights
			where user_role_id = @user_role_id)

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='124',@return_status=0,@user_name=@user_login_id
					return 0
				end
		--=========..?================================
		end

	if(@gen_verify_msg='Y')
		begin
			select @VRSAPPLINK = data_type_string from general_settings where control_code='VRSAPPLINK'
			select @SMSSENDERNO = data_type_string from general_settings where control_code='SMSSENDERNO'

			--email Log
			set @email_text= concat('Hello [USER_NAME], \n\n Please click the following link to activate your account \n\n <a href=''',@VRSAPPLINK + '/Registration/VRSVerification.aspx?activationcode=1' ,'''>Click here to verify your Email ID</a>\n\n Regards \n  Support \n\n VETCHOICE')
			set @email_log_id = newid()
			insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,email_subject,email_text,study_hdr_id,email_processed,for_email_verification) 
						             values(@email_log_id,getdate(),@email_id,@contact_person_name,@email_subject,@email_text,'00000000-0000-0000-0000-000000000000','N','Y')


			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='207',@return_status=0
					return 0
				end

			--Sms Log
			set @sms_text = 'Click ' + @VRSAPPLINK+ '/Registration/VRSVerification.aspx?activationcode=1 to verify your mobile no.'
			set @sms_log_id = newid()
			insert into vrslogdb..sms_log(sms_log_id,sms_log_datetime,recipient_no,recipient_name,sender_no,sms_text,study_hdr_id,sms_processed,for_sms_verification) 
							       values(@sms_log_id,getdate(),@contact_person_mob,@contact_person_name,@SMSSENDERNO,@sms_text,'00000000-0000-0000-0000-000000000000','N','Y')
 
			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='208',@return_status=0
					return 0
				end
		end

	
	commit transaction
	if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2
	set @return_status=1
	set @error_code='206'
	set nocount off

	return 1
end



GO
