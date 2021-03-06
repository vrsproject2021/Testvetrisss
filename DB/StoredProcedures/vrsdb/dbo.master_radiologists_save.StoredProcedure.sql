USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologists_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologists_save]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologists_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologists_save : save
                  radiologist details.
** Created By   : BK
** Created On   : 24/07/2019
*******************************************************/
CREATE procedure [dbo].[master_radiologists_save]
(
	@id						uniqueidentifier	= '00000000-0000-0000-0000-000000000000' output,
	@fname                  nvarchar(80)		= '',
	@lname                  nvarchar(80)		= '',
	@credentials			nvarchar(100)		= '',-- Added on 12th SEP 2019 @BK
	@address_Line1			nvarchar(100)		= '',
	@address_Line2			nvarchar(100)		= '',
	@city					nvarchar(100)		= '',
	@zip			     	nvarchar(20)		= '',
	@state_id				int					= 0,
	@country_id				int					= 0,
	@timezone_id			int					= 0,
	@email_id   			nvarchar(50)		= '',
	@phone					nvarchar(30)		= '',
	@mobile					nvarchar(20)		= '',
	@login_id               nvarchar(50)		= '',
	@login_pwd				nvarchar(200)		= '',
	@pacs_user_id           nvarchar(50)		= '',
	@pacs_user_pwd   		nvarchar(200)		= '',
	@is_active				char(1)				= 'Y',
	@identity_color         nvarchar(10)        = '#FFFFFF',
	@notification_pref      nchar(1)            ='B',
	@signage                ntext,
	@schedule_view          nchar(1)            ='O',
	@notes                  ntext               = null,
	@transcription_required nchar(1)            = 'N',
	@acct_group_id          int                 = 0,
	@assign_merged_study    nchar(1)            = 'N',
	@max_wu_per_hr       int                 = 0,
	@xml_modality           ntext,
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
				@radiologists_user_id uniqueidentifier,
				@last_code_id int,
				@user_role_id int,
				@updated_in_pacs nchar(1),-- Added on 9th SEP 2019 @BK
				@old_user_login_id nvarchar(50),-- Added on 9th SEP 2019 @BK
				@old_user_login_password nvarchar(200),-- Added on 9th SEP 2019 @BK
				@old_pacs_user_id nvarchar(50),
				@old_pacs_user_pwd nvarchar(200),
				@old_fname nvarchar(80),
				@old_lname nvarchar(80),
				@old_credentials nvarchar(30),
				@hDoc1 int,
				@hDoc2 int,
				@counter int,
	            @rowcount int,
				@modality_id int,
				@modality_name nvarchar(30),
				@inst_id uniqueidentifier,
				@inst_name nvarchar(100),
				@prelim_fee money,
				@final_fee money,
				@addl_STAT_fee money,
				@work_unit int
				
	
	if(@id = '00000000-0000-0000-0000-000000000000') 
		begin
			set @radiologists_user_id = newid()
	    end
	else
		begin
			select @code = code from radiologists where id=@id
			select @radiologists_user_id = id from users where code=@code
		end

	if(select count(id) from users where upper(login_id) = upper(@login_id) and id<>@radiologists_user_id)>0
		begin
				select @error_code='108',@return_status=0
				return 0
		end

	if(@assign_merged_study='Y')
		begin
			if(select count(id) from radiologists where assign_merged_study = 'Y' and id<>@id)>0
			   begin
						select @error_code='431',@return_status=0
						return 0
				end
		end
   

	select @user_role_id = id
	from user_roles
	where code='RDL'

	begin transaction
	exec sp_xml_preparedocument @hDoc1 output,@xml_modality 

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
			set @code = 'RDL' + convert(varchar,@last_code_id)

            drop table #tmpID

            set @id = newid()

			insert into radiologists
			(
				id,
				code,
				fname,
				lname,
				name,
				credentials,-- Added on 9th SEP 2019 @BK
				address_1,
				address_2,
				city,
				state_id,
				country_id,
				timezone_id,
				zip,
				email_id,
				phone_no,
				mobile_no,
				login_user_id,
				login_id,
				login_pwd,
				login_name,
				pacs_user_id,
				pacs_user_pwd,
				identity_color,
				notification_pref,
				signage,
				schedule_view,
				notes,
				transcription_required,
				acct_group_id,
				assign_merged_study,
				max_wu_per_hr,
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
				rtrim(ltrim(rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))) + ' ' + upper(rtrim(ltrim(@credentials))))),
				upper(rtrim(ltrim(@credentials))),-- Added on 9th SEP 2019 @BK
				@address_Line1,
				@address_Line2,
				@city,
				@state_id,
				@country_id,
				@timezone_id,
				@zip,
				@email_id,
				@phone,
				@mobile,
				@radiologists_user_id,		
				@login_id,           
				@login_pwd,	
				rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
				@pacs_user_id,
				@pacs_user_pwd,		
				@identity_color,
				@notification_pref,
				@signage,
				@schedule_view,
				@notes,
				@transcription_required,
				@acct_group_id,
				@assign_merged_study,
				@max_wu_per_hr,
				@is_active,
				'N',
				'JEFKORTPWI134N',				
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
					   values(@radiologists_user_id,@code, rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
							  @login_id,@login_pwd,
							  @email_id,@mobile,@user_role_id,
							  @pacs_user_id,@pacs_user_pwd,@notification_pref,--pacs_user_id,pacs_password
							  @is_active,'Y',@user_id,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc1
					select @error_code='109',@return_status=0
					return 0
				end

			insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
			(select @radiologists_user_id,menu_id,@user_id,getdate()
			from user_role_menu_rights
			where user_role_id = @user_role_id)

			/*if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='125',@return_status=0
					return 0
				end*/

			--insert into case_notification_rule_radiologist_dtls(rule_no,radiologist_id,user_id,notify_if_scheduled,notify_always)
			--(select rule_no,@id,@radiologists_user_id,'N','N'
			-- from case_notification_rule_hdr)
			
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
					exec sp_xml_removedocument @hDoc1
					return 0
				end

			set @updated_in_pacs ='Y'

			-- Added on 9th SEP 2019 @BK
			select @old_fname                   = fname,
			       @old_lname                   = lname,
				   @old_credentials             = credentials,
				   @old_user_login_id			= login_id,
				   @old_user_login_password		= login_pwd,
				   @old_pacs_user_id			= pacs_user_id,
				   @old_pacs_user_pwd		    = pacs_user_pwd
			from radiologists
			where id = @id
			
			if(upper(rtrim(ltrim(@old_fname))) <> upper(rtrim(ltrim(@fname))))
				begin
					set @updated_in_pacs ='N'
				end
			if(upper(rtrim(ltrim(@old_lname))) <> upper(rtrim(ltrim(@lname))))
				begin
					set @updated_in_pacs ='N'
				end
			if(upper(rtrim(ltrim(@old_credentials))) <> upper(rtrim(ltrim(@credentials))))
				begin
					set @updated_in_pacs ='N'
				end
			if(upper(rtrim(ltrim(@old_user_login_id))) <> upper(rtrim(ltrim(@login_id))))
				begin
					set @updated_in_pacs ='N'
				end
			if(@old_user_login_password <> @login_pwd)
				begin
					set @updated_in_pacs ='N'
				end
			if(upper(rtrim(ltrim(@old_pacs_user_id))) <> upper(rtrim(ltrim(@pacs_user_id))))
				begin
					set @updated_in_pacs ='N'
				end
			if(@old_pacs_user_pwd <> @pacs_user_pwd)
				begin
					set @updated_in_pacs ='N'
				end

			--~~ Added on 9th SEP 2019 @BK
			update radiologists
					set	fname                   = @fname,
					    lname                   = @lname,
						name                    = rtrim(ltrim(rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))) + ' ' + upper(rtrim(ltrim(@credentials))))),
						credentials				= upper(rtrim(ltrim(@credentials))),-- Added on 9th SEP 2019 @BK
						address_1				= @address_Line1,
						address_2				= @address_Line2,
						city					= @city,
						state_id				= @state_id,
						country_id				= @country_id,
						timezone_id				= @timezone_id,
						zip						= @zip,
						email_id                = @email_id,
						phone_no				= @phone,
						mobile_no				= @mobile,
						login_id                = @login_id,
						login_pwd               = @login_pwd,
						login_name              = rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
						pacs_user_id            = @pacs_user_id,
				        pacs_user_pwd           = @pacs_user_pwd,
						identity_color          = @identity_color,
						notification_pref       = @notification_pref,
						signage                 = @signage,
						schedule_view           = @schedule_view,
						notes                   = @notes,
						transcription_required  = @transcription_required,
						acct_group_id           = @acct_group_id,
						assign_merged_study     = @assign_merged_study,
						max_wu_per_hr           = @max_wu_per_hr,
						is_active				= @is_active,
						updated_by				= @user_id,
						date_updated			= getdate(),
						updated_in_pacs			= @updated_in_pacs,-- Added on 9th SEP 2019 @BK
						granted_rights_pacs		= 'JEFKORTPWI134N'-- Added on 9th SEP 2019 @BK
					where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc1
					select	@return_status=0,@error_code='035'
					return 0
				end
			

			update users
			set name			  =  rtrim(ltrim(isnull(@fname,'') + ' ' + isnull(@lname,''))),
				contact_no		  = @mobile,
				notification_pref = @notification_pref,
				login_id		  = @login_id,
				password		  = @login_pwd,
				pacs_user_id      = @pacs_user_id,
				pacs_password     = @pacs_user_pwd,
				user_role_id      = @user_role_id,
				is_active         = @is_active,
				update_by		  = @user_id,
				date_updated	  = getdate()
			where id = @radiologists_user_id

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc1
					select @error_code='109',@return_status=0
					return 0
				end
		end


	delete from radiologist_modality_link where radiologist_id=@id
    set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc1,'modality/row', 2)  
	with( row_id bigint )

	while(@counter <= @rowcount)
		begin
			select  @modality_id        = modality_id,
			        @prelim_fee         = prelim_fee,
					@final_fee          = final_fee,
					@addl_STAT_fee      = addl_STAT_fee,
					@work_unit          = work_unit
			from openxml(@hDoc1,'modality/row',2)
			with
			( 
				modality_id int,
				prelim_fee money,
				final_fee money,
				addl_STAT_fee money,
				work_unit int,
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter
			
		    select @modality_name= name from modality where id=@modality_id 
			
			insert into radiologist_modality_link(radiologist_id,modality_id,prelim_fee,final_fee,addl_STAT_fee,work_unit,updated_by,date_updated)
			                               values(@id,@modality_id,@prelim_fee,@final_fee,@addl_STAT_fee,@work_unit,@user_id,getdate()) 

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc1
					select @error_code='066',@return_status=0,@user_name= @modality_name
					return 0
				end

			if(select count(modality_id) from radiologist_functional_rights_modality where radiologist_id = @id and modality_id=@modality_id)=0
				begin
					insert into radiologist_functional_rights_modality(radiologist_id,modality_id,created_by,date_created)
			                                                    values(@id,@modality_id,@user_id,getdate()) 

					if(@@rowcount=0)
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc1
							select @error_code='066',@return_status=0,@user_name= @modality_name
							return 0
						end
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
	exec sp_xml_removedocument @hDoc1
	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
end



GO
