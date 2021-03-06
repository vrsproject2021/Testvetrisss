USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_user_save]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_user_save : save
                  user 
** Created By   : Pavel Guha
** Created On   : 23/04/2019
*******************************************************/
CREATE procedure [dbo].[settings_user_save]
(
	@id						 uniqueidentifier='00000000-0000-0000-0000-000000000000' output,
	@code                    nvarchar(10),
	@name		             nvarchar(100)	= '',
	@email_id				 nvarchar(50)	= '',
	@contact_no		     	 nvarchar(20)	= '',
	@user_role_id			 int				= 0,
	@login_id			     nvarchar(50)	= '',
	@password			     nvarchar(200)	= '',
	@pacs_user_id			 nvarchar(20)	= '',
	@pacs_password			 nvarchar(200)	= '',
	@allow_manual_submission nchar(1)       = 'N',
	@allow_dashboard_view    nchar(1)       = 'N',
	@is_active				 nchar(1)		= 'Y',
	@xml_menu                ntext,
	@updated_by              uniqueidentifier,
    @menu_id                 int,
	@session_id              uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @user_name               nvarchar(700) = '' output,
	@error_code				 nvarchar(10)	= '' output,
    @return_status			 int				= 0  output
)
as
begin
	set nocount on 

	declare @hDoc int,
	        @rowcount int,
			@counter int,
			@rec_menu_id int,
			@rec_menu_name nvarchar(30),
			@parent_id1 int,
			@parent_id2 int,
			@parent_id3 int,
			@activity_text nvarchar(max)
	
	if(@is_active='Y')
		begin
			if(select count(id) from users where upper(code) = upper(@code) and id<>@id)>0
				begin
						select @error_code='074',@return_status=0,@user_name=''
						return 0
				end
		end

	if(select count(id) from users where upper(login_id) = upper(@login_id) and id<>@id)>0
		begin
				select @error_code='083',@return_status=0,@user_name=''
				return 0
		end

	begin transaction

	exec sp_xml_preparedocument @hDoc output,@xml_menu 
	set @counter = 1
	
	if(@id = '00000000-0000-0000-0000-000000000000')
		begin
			set @id	=NEWID()
			insert into users
						(
							id,
							code,
							name,
							user_role_id,
							contact_no,
							email_id,
							login_id,
							password,
							pacs_user_id,
							pacs_password,
							allow_manual_submission,
							allow_dashboard_view,
							is_active,
							created_by,
							date_created

						)
					values
						(
							@id,
							@code,
							@name,
							@user_role_id,
							@contact_no,
							@email_id,
							@login_id,
							@password,
							@pacs_user_id,
							@pacs_password,
							@allow_manual_submission,
							@allow_dashboard_view,
							@is_active,
							@updated_by,
							getdate()
						)

			    if(@@rowcount=0)
					begin
						rollback transaction
						select	@return_status=0,
								@error_code='035'
						return 0
					end
			
		end
	else
		begin
			exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @id,
				@user_id       = @updated_by,
				@session_id    = @session_id,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
			if(@return_status=0)
				begin
					rollback transaction
					return 0
				end

			update users
					set
						code                    = @code,
						name					= @name,
						email_id				= @email_id,
						contact_no				= @contact_no,
						user_role_id			= @user_role_id,
						login_id                = @login_id,
						password                = @password,
						pacs_user_id			= @pacs_user_id,
						pacs_password			= @pacs_password,
						allow_manual_submission = @allow_manual_submission,
						allow_dashboard_view    = @allow_dashboard_view,
						is_active				= @is_active,
						update_by				= @updated_by,
						date_updated			= getdate()

					where id = @id

			if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,
							@error_code='035'
					return 0
				end
		
	  end

	delete from user_menu_rights where user_id = @id

	select  @rowcount=count(row_id)  
	from openxml(@hDoc,'menu/row', 2)  
	with( row_id int )

	while(@counter<=@rowcount)
		begin
			select @rec_menu_id = menu_id
			from openxml(@hDoc,'menu/row',2)
			with
			( 
				menu_id int,
				row_id int
			) xmlTemp1 where xmlTemp1.row_id = @counter 
			
			if(select count(user_id) from user_menu_rights where user_id=@id and menu_id=@rec_menu_id)=0
				begin
					insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
										  values(@id,@rec_menu_id,@updated_by,getdate())
					if(@@rowcount = 0)
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @return_status = 0,@error_code ='035'
							return 0
						end
						
					  select @parent_id1=parent_id from sys_menu where menu_id=@rec_menu_id
					  
					  if(@parent_id1>0)
						begin
							 if(select count(menu_id) from user_menu_rights where user_id=@id and menu_id=@parent_id1)=0
								begin
									 insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
																		 values(@id,@parent_id1,@updated_by,getdate())
									 if(@@rowcount = 0)
										begin
											rollback transaction
											exec sp_xml_removedocument @hDoc
											select @return_status = 0,@error_code ='035'
											return 0
										end
										
									select @parent_id2=parent_id from sys_menu where menu_id=@parent_id1
									if(@parent_id2>0)
										begin
											 if(select count(menu_id) from user_menu_rights where user_id=@id and menu_id=@parent_id2)=0
												begin
													 insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
																				 values(@id,@parent_id2,@updated_by,getdate())
													 if(@@rowcount = 0)
														begin
															rollback transaction
															exec sp_xml_removedocument @hDoc
															select @return_status = 0,@error_code ='035'
															return 0
														end
												end
										end
								end
						end
					  
					else
						begin
							select @parent_id1=parent_id from sys_menu where menu_id=@rec_menu_id
							
							
						end
				end
			 
			
			set @counter=@counter + 1
		end

	
	set @activity_text =  'Saved record => ' + @name
	exec common_user_activity_log
		@user_id       = @updated_by,
		@activity_text = @activity_text,
		@menu_id       = @menu_id,
		@session_id    = @session_id,
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
