USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_transciptionists_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_transciptionists_save]
GO
/****** Object:  StoredProcedure [dbo].[master_transciptionists_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_transciptionists_save : save
                  transciptionists details.
** Created By   : BK
** Created On   : 16/09/2020
*******************************************************/
CREATE procedure [dbo].[master_transciptionists_save]
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
	@login_id               nvarchar(20)		= '',
	@login_pwd				nvarchar(200)		= '',
	@is_active				nchar(1)			= 'Y',
	@notification_pref      char(1)             = 'B',
	@notes                  nvarchar(500)       = '',
	@xml_modality           ntext               = null,
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
				@transciptionists_user_id uniqueidentifier,
				@last_code_id int,
				@user_role_id int,
				@hDoc int,
				@counter int,
	            @rowcount int,
				@modality_id int,
				@modality_name nvarchar(30),
				@default_fee money,
				@addl_STAT_fee money,
				@updated_in_pacs nchar(1),-- Added on 10th SEP 2019 @BK
				@old_user_login_id nvarchar(20),-- Added on 10th SEP 2019 @BK
				@old_user_login_password nvarchar(200)-- Added on 10th SEP 2019 @BK
	
	if(@id = '00000000-0000-0000-0000-000000000000') 
		begin
			set @transciptionists_user_id = newid()
	    end
	else
		begin
			--select @code = code from transciptionists where id=@id
			select @transciptionists_user_id = login_user_id from transciptionists where id=@id
		end

	if(select count(id) from users where upper(login_id) = upper(@login_id) and id<>@transciptionists_user_id)>0
		begin
				select @error_code='108',@return_status=0
				return 0
		end

	select @user_role_id = id
	from user_roles
	where code='TRS'

	begin transaction
	exec sp_xml_preparedocument @hDoc output,@xml_modality 
	if(@id = '00000000-0000-0000-0000-000000000000')
		begin
			
			create table #tmpID(id int)

			insert into #tmpID(id) 
			(select convert(int,substring(code,4,len(code)-3))
			from users
			where user_role_id=@user_role_id)
								
			select @last_code_id =max(id) 
			from #tmpID

			set @last_code_id = isnull(@last_code_id,0) + 1
			set @code = 'TRS' + convert(varchar,@last_code_id)

			drop table #tmpID

            set @id = newid()
			insert into transciptionists
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
				notification_pref,	
				notes,
				is_active,
				updated_in_pacs	,	
				granted_rights_pacs,
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
				@transciptionists_user_id,		
				@login_id,           
				@login_pwd,
				@notification_pref,
				@notes,	
				@is_active,
				'N',
				'gT',			
				@user_id,
				getdate()
			)

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc
					select	@return_status=0,@error_code='035'
					return 0
				end


			insert into users(id,code,name,
							  login_id,password,
							  email_id,contact_no,user_role_id,
							  pacs_user_id,pacs_password,notification_pref,
							  is_active,is_visible,
							  created_by,date_created)
					   values(@transciptionists_user_id,@code,rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
							  @login_id,@login_pwd,
							  @email_id,@mobile,@user_role_id,
							  @login_id,@login_pwd,@notification_pref,--pacs_user_id,pacs_password
							  'Y','Y',
							  @user_id,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc
					select @error_code='109',@return_status=0
					return 0
				end

			insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
			(select @transciptionists_user_id,menu_id,@user_id,getdate()
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
					exec sp_xml_removedocument @hDoc
					return 0
				end

			-- Added on 10th SEP 2019 @BK
			select @old_user_login_id			= login_id,
				   @old_user_login_password		= login_pwd
			from transciptionists
			where id = @id

			if(UPPER( RTRIM(LTRIM(@old_user_login_id))) <> UPPER( RTRIM(LTRIM(@login_id))))
				begin
					set @updated_in_pacs ='N'
				end
			if(@old_user_login_password <> @login_pwd)
				begin
					set @updated_in_pacs ='N'
				end
			--~~ Added on 10th SEP 2019 @BK
			update transciptionists
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
						notification_pref       = @notification_pref,
						notes                   = @notes,
						is_active				= @is_active,
						updated_by				= @user_id,
						date_updated			= getdate(),
						updated_in_pacs			= @updated_in_pacs,-- Added on 10th SEP 2019 @BK
						granted_rights_pacs		= 'gT'-- Added on 10th SEP 2019 @BK
					
					where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc
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
			where id = @transciptionists_user_id

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc
					select @error_code='109',@return_status=0
					return 0
				end
		end

	delete from transcriptionist_modality_link where transcriptionist_id=@id
    set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc,'modality/row', 2)  
	with( row_id bigint )

	while(@counter <= @rowcount)
		begin
			select  @modality_id        = modality_id,
					@default_fee        = default_fee,
					@addl_STAT_fee      = addl_STAT_fee
			from openxml(@hDoc,'modality/row',2)
			with
			( 
				modality_id int,
				default_fee money,
				addl_STAT_fee money,
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter
			
		    select @modality_name= name from modality where id=@modality_id 
			
			insert into transcriptionist_modality_link(transcriptionist_id,modality_id,default_fee,addl_STAT_fee,updated_by,date_updated)
			                                    values(@id,@modality_id,@default_fee,@addl_STAT_fee,@user_id,getdate()) 

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc
					select @error_code='066',@return_status=0,@user_name= @modality_name
					return 0
				end
			
			
			set @counter = @counter + 1
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
