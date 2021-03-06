USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[registration_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[registration_save]
GO
/****** Object:  StoredProcedure [dbo].[registration_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************  
*******************************************************  
** Version  : 1.0.0.0  
** Procedure    : registration_save : save  
                  registration details.  
** Created By   : AM  
** Created On   : 22/07/2020  
*******************************************************/  
CREATE procedure [dbo].[registration_save]  
(  
 @id      uniqueidentifier = '00000000-0000-0000-0000-000000000000' output,  
 @name                  nvarchar(100)  = '',  
 @address_1                  nvarchar(100)  = '',  
 @address_2   nvarchar(100)  = '',  
 @city     nvarchar(100)  = '',  
 @state_id    int     = 0,  
 @country_id    int     = 0,  
 @zip         nvarchar(20)  = '',  
 @email_id      nvarchar(50)  = '',  
 @phone_no     nvarchar(30)  = '',  
 @mobile_no     nvarchar(20)  = '',  
 @contact_person_name     nvarchar(100)  = '',  
 @contact_person_mobile     nvarchar(100)  = '',  
 @login_id               nvarchar(20)  = '',  
 @login_password    nvarchar(200)  = '',  
 @login_email_id    nvarchar(100)  = '',  
 @login_mobile_no    nvarchar(20)  = '',  
 --@preferred_pmt_method    nvarchar(5)  = '',  
 @is_email_verified   char(1)    = 'N',  
 @is_mobile_verified    char(1)    = 'Y',  
 @submitted_by nvarchar(100),
 @img_software_name nvarchar(100) = '',
 @code                     nvarchar(5)     = '' output,  
 @xml_modality_link        ntext           = null,  
 @xml_physician_link           ntext           = null,  
 @country_name     nvarchar(100)  = '',  
 @state_name     nvarchar(100)  = '',  
 @user_name              nvarchar(700)  = '' output,  
 @error_code    nvarchar(10)  = '' output,  
 @return_status   int     = 0  output  
)  
as  
begin  
	set nocount on   
  declare	@hDoc1 int,  
			@hDoc2 int,  
			@counter bigint,  
			@rowcount bigint,  
			@physician_code nvarchar(10),
			@user_user_id uniqueidentifier,
			@user_role_id int,
			@user_code nvarchar(10)
  
  declare	@cd nvarchar(5),  
			@registration_id uniqueidentifier,  
			@last_code_id int  
  declare	@modality_id int,  
			@institution_id uniqueidentifier,
			@created_by_default uniqueidentifier='00000000-0000-0000-0000-000000000000'
  
  declare	@physician_id uniqueidentifier,  
			@physician_fname nvarchar(80),  
			@physician_lname nvarchar(80),  
			@physician_credentials nvarchar(30),  
			@physician_name nvarchar(200),  
			@physician_email nvarchar(500),  
			@physician_mobile nvarchar(500)  
   
 /***************VALIDATIONS***************/  
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
 /***************VALIDATIONS***************/  
  
 begin transaction  
 if(@xml_modality_link is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_modality_link 
 if(@xml_physician_link is not null) exec sp_xml_preparedocument @hDoc2 output,@xml_physician_link   

 if(@xml_physician_link is null)
	begin
		rollback transaction
		if(@xml_modality_link is not null) exec sp_xml_removedocument @hDoc1  
		if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2 
		select	@return_status=0,@error_code='364'
		return 0
	end
  
  if(@id = '00000000-0000-0000-0000-000000000000')
		begin
			set @id	=newid()
			insert into institutions
						(
							id,code,[name],address_1,address_2,city,state_id,country_id,zip,
							email_id,phone_no,mobile_no,contact_person_name,contact_person_mobile,submitted_by,img_software_name,
							notes,discount_per,accountant_name,business_source_id,format_dcm_files,dcm_file_xfer_pacs_mode,study_img_manual_receive_path,
							consult_applicable,storage_applicable,custom_report,logo_img,image_content_type,is_online,
							is_active,is_new,created_by,date_created
						)
					values
						(
							@id,@code,@name,@address_1,@address_2,@city,@state_id,@country_id,@zip,
							@email_id,@phone_no,@mobile_no,@contact_person_name,@contact_person_mobile,@submitted_by,@img_software_name,
							'',0,'',0,'N','N','',
							'N','N','N',null,null,'Y',
							'Y','Y',@created_by_default,getdate()
						)

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_modality_link is not null) exec sp_xml_removedocument @hDoc1  
					if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2 
					select	@return_status=0,@error_code='035'
					return 0
				end
			
		end

  /* Save Physician */

 if(@xml_physician_link is not null)  
  begin  
   set @counter = 1  
   select  @rowcount=count(row_id)    
   from openxml(@hDoc2,'physician/row', 2)    
   with( row_id bigint )  
  
   while(@counter <= @rowcount)  
    begin  
     select	@physician_id            = physician_id,  
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
  
      set @physician_id= newid()  
  
      insert into institution_physician_link (physician_id,institution_id,physician_fname,physician_lname,physician_credentials, physician_name, 
					                          physician_email,physician_mobile,created_by,date_created)  
                                        values(@physician_id,@id,@physician_fname,@physician_lname,@physician_credentials, @physician_name, 
					                          @physician_email,@physician_mobile,@created_by_default,GetDATE())  
                                                     
  
      if(@@rowcount=0)  
       begin  
        rollback transaction  
		if(@xml_modality_link is not null) exec sp_xml_removedocument @hDoc1  
        if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2  
        select @error_code='363',@return_status=0,@user_name=@physician_name  
        return 0  
       end 
	   
	   select @last_code_id =max(convert(int,substring(code,5,len(code)-4))) 
								from physicians

		set @last_code_id = isnull(@last_code_id,0) + 1
		set @physician_code = 'PHYS' + convert(varchar,@last_code_id)
									
		insert into physicians(id,code,fname,lname,credentials,name,institution_id,
								email_id,mobile_no,created_by,date_created) 
						values (@physician_id,@physician_code,@physician_fname,@physician_lname,@physician_credentials,@physician_name,@id,
								@physician_email,@physician_mobile,@created_by_default,getdate())

		if(@@rowcount=0)
			begin
				rollback transaction
				if(@xml_modality_link is not null) exec sp_xml_removedocument @hDoc1  
				if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2
				select @error_code='066',@return_status=0,@user_name=@physician_name
				return 0
			end
     set @counter = @counter + 1  
    end  
  end  
  
  
/* Save User */
  

	if(select count(id) from users where upper(login_id) = upper(@login_id) and is_active='Y')>0
		begin
				rollback transaction
				if(@xml_modality_link is not null) exec sp_xml_removedocument @hDoc1  
				if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2  
				select @error_code='118',@return_status=0,@user_name=Convert(varchar,@counter)
				return 0
		end

	if(select count(user_login_id) 
		from institution_user_link 
		where upper(user_login_id) = upper(@login_id) 
		and institution_id=@id)>0
		begin
				rollback transaction
				if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2  
				select @error_code='114',@return_status=0,@user_name=Convert(varchar,@counter)
				return 0
		end

	set @user_user_id= newid()
	insert into institution_user_link(user_id,institution_id,user_login_id,user_pwd,user_pacs_user_id,user_pacs_password,
										user_email,user_contact_no,granted_rights_pacs,created_by,date_created)
								values( @user_user_id,@id,@login_id,@login_password,@login_id,@login_password,
										@login_email_id,'','EOWIN',@created_by_default,GETDATE())
					                                              
	if(@@rowcount=0)
		begin
			rollback transaction
			if(@xml_modality_link is not null) exec sp_xml_removedocument @hDoc1  
			if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2  
			select @error_code='113',@return_status=0,@user_name=@login_id
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

	insert into users(id,code,[name],login_id,password,email_id,contact_no,user_role_id,
						pacs_user_id,pacs_password,is_active,is_visible,created_by,date_created) 
				values (@user_user_id,@user_code,@login_id,@login_id,@login_password,@login_email_id,'',@user_role_id,
						@login_id,@login_password,'Y','Y',@user_user_id,getdate())

	if(@@rowcount=0)
		begin
			rollback transaction
			select @error_code='113',@return_status=0,@user_name=@login_id
			return 0
		end

	insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
	(select @user_user_id,menu_id,null,getdate()
	from user_role_menu_rights
	where user_role_id = @user_role_id)

	if(@@rowcount=0)
		begin
			rollback transaction
			select @error_code='124',@return_status=0,@user_name=@login_id
			return 0
		end

	--Update CreatedBy field
	
			

  --Generate Mail
	declare @MAILSVRUSRCODE nvarchar(200),
	        @MAILSVRUSRPWD 	nvarchar(200),
			@NEWREGRECEMAILID nvarchar(200),
			@NEWREGRECEMAILCC nvarchar(200),
			@mail_subject nvarchar(250),
			@mail_text nvarchar(max),
			@modalities nvarchar(100)	
			
	select @MAILSVRUSRCODE    = data_type_string from general_settings where control_code = 'MAILSVRUSRCODE'
	select @MAILSVRUSRPWD     = data_type_string from general_settings where control_code = 'MAILSVRUSRPWD'		
	select @NEWREGRECEMAILID  = data_type_string from general_settings where control_code = 'NEWREGRECEMAILID'
	select @NEWREGRECEMAILCC  = data_type_string from general_settings where control_code = 'NEWREGRECEMAILCC'	

	set @modalities=''
	if(@xml_modality_link is not null)  
		begin
			set @counter = 1  
			select  @rowcount=count(row_id)    
			from openxml(@hDoc1,'modality_link/row', 2)    
			with( row_id int )  

			while(@counter <= @rowcount)
				begin
					select	@modality_id = modality_id
					 from openxml(@hDoc1,'modality_link/row',2)  
					 with  
					 (   
					  modality_id int,  
					  row_id int  
					 ) xmlTemp where xmlTemp.row_id = @counter   
					 
					 if(isnull(@modalities,'')<>'') set @modalities = @modalities + ','
					 set @modalities = @modalities + (isnull((select name from modality where id=@modality_id),''))
					  
					set @counter = @counter + 1  
				end
		end
	
	set @mail_subject= 'New institution ' + upper(@name) + ' (' + @code + ') registered' 
	set @mail_text = '<table style=''width: 100%; border-collapse: separate; border-spacing: 2px;''>'
	set @mail_text = @mail_text + '<tr><td colspan=''2'' style=''padding:2px;text-align:center;border:solid 2px #000;background-color:#eee;''><b>REGISTRATION DETAILS</b></td></tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>INSTITUTION NAME</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + @name + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>STREET NAME</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@address_1,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>CITY</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@city,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>STATE</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull((select name from sys_states where id=@state_id),'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>COUNTRY</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull((select name from sys_country where id=@country_id),'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>ZIP CODE</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@zip,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>MODALITIES</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@modalities,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	--set @mail_text = @mail_text + '<tr>'
	--set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>PREFFERRED METHOD OF PAYMENT</td>'
	--set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' 
	--if(@preferred_pmt_method='0') set @mail_text = @mail_text + 'None'
	--else if(@preferred_pmt_method='CK') set @mail_text = @mail_text + 'Check'
	--else if(@preferred_pmt_method='CC') set @mail_text = @mail_text + 'Credit Card'
	--else if(@preferred_pmt_method='OP') set @mail_text = @mail_text + 'Online Payment'
	--else if(@preferred_pmt_method='MI') set @mail_text = @mail_text + 'Mail Invoice'
	--set @mail_text = @mail_text + '</td>'
	--set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>IMAGING SOFTWARE NAME</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' 
	set @mail_text = @mail_text + isnull(@img_software_name,'')
	set @mail_text = @mail_text + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr><td colspan=''2'' style=''padding:2px;text-align:center;border:solid 2px #000;background-color:#eee;''><b>CONTACT DETAILS</b></td></tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>CONTACT PERSON NAME</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@contact_person_name,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>MOBILE NUMBER</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@contact_person_mobile,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>EMAIL ID</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@email_id,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>OFFICE NUMBER</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@phone_no,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr><td colspan=''2'' style=''padding:2px;text-align:center;border:solid 2px #000;background-color:#eee;''><b>LOGIN CREDENTIALS</b></td></tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>LOGIN ID</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@login_id,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>PASSWORD</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>(Please check it from with in VETRIS)</td>'
	set @mail_text = @mail_text + '</tr>'
	--set @mail_text = @mail_text + '<tr>'
	--set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>EMAIL ID</td>'
	--set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@login_email_id,'') + '</td>'
	--set @mail_text = @mail_text + '</tr>'
	--set @mail_text = @mail_text + '<tr>'
	--set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>MOBILE NUMBER</td>'
	--set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;''>' + isnull(@login_mobile_no,'') + '</td>'
	--set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '<tr><td colspan=''2'' style=''padding:2px;text-align:center;border:solid 2px #000;background-color:#eee;''><b>Veterinarians on Staff</b></td></tr>'
	set @mail_text = @mail_text + '<tr><td colspan=2>'
	set @mail_text = @mail_text + '<table style=''width: 100%; border-collapse: separate; border-spacing: 2px;''>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;font-weight:bold;border:solid 2px #000;''>Sl.</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;font-weight:bold;border:solid 2px #000;''>First Name</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;font-weight:bold;border:solid 2px #000;''>Last Name</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;font-weight:bold;border:solid 2px #000;''>Credentials</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;font-weight:bold;border:solid 2px #000;''>Email ID</td>'
	set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;font-weight:bold;border:solid 2px #000;''>Mobile #</td>'
	set @mail_text = @mail_text + '</tr>'

	if(@xml_physician_link is not null)  
	  begin  
		   set @counter = 1  
		   select  @rowcount=count(row_id)    
		   from openxml(@hDoc2,'physician/row', 2)    
		   with( row_id bigint )  
  
		   while(@counter <= @rowcount)  
			begin  
					 select	@physician_fname         = physician_fname,  
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

					set @mail_text = @mail_text + '<tr>'
					set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;border:solid 1px #bbb;''>' + convert(varchar,@counter) +'</td>'
					set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;border:solid 1px #bbb;''>' + @physician_fname + '</td>'
					set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;border:solid 1px #bbb;''>' + @physician_lname + '</td>'
					set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;border:solid 1px #bbb;''>' + @physician_credentials + '</td>'
					set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;border:solid 1px #bbb;''>' + @physician_email + '</td>'
					set @mail_text = @mail_text + '<td style=''padding:2px;text-align:left;border:solid 1px #bbb;''>' + @physician_mobile + '</td>'
					set @mail_text = @mail_text + '</tr>'

					set @counter=@counter + 1
			 end
		 end

	set @mail_text = @mail_text + '</table>'
	set @mail_text = @mail_text + '</td></tr>'
	set @mail_text = @mail_text + '<tr>'
	set @mail_text = @mail_text + '<td style=''padding:10px;text-align:right;'' colspan=''2''>SUBMITTED BY : ' + isnull(@submitted_by,'') + '</td>'
	set @mail_text = @mail_text + '</tr>'
	set @mail_text = @mail_text + '</table>'
	
	insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,cc_address,
	                      email_subject,email_text,email_type,sender_email_address,sender_email_password)	
				   values(newid(),getdate(),@NEWREGRECEMAILID,'',@NEWREGRECEMAILCC,
				          @mail_subject,@mail_text,'NEWREG',@MAILSVRUSRCODE,@MAILSVRUSRPWD)			
  
    if(@@rowcount=0)
		begin
			rollback transaction
			if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2  
			select @error_code='371',@return_status=0,@user_name=@login_id
			return 0
		end
  
 commit transaction  
 if(@xml_physician_link is not null) exec sp_xml_removedocument @hDoc2  
 set @user_name=@name  
 set @return_status=1  
 set @error_code='034'  
 set nocount off  
  
 return 1  
end  
  
  
GO
