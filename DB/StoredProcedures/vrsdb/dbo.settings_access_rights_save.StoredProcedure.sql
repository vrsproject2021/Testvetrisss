USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_access_rights_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_access_rights_save]
GO
/****** Object:  StoredProcedure [dbo].[settings_access_rights_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : access_rights_save : save access rights
** Created By   : Pavel Guha
** Created On   : 02/05/2019
*******************************************************/
create procedure [dbo].[settings_access_rights_save]
    @id int=0 output,
    @xml_menu ntext,
    @updated_by uniqueidentifier,
    @menu_id int,
    @user_name nvarchar(30) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	declare @hDoc int,
	        @rowcount int,
			@active nchar(1),
			@rec_menu_id int,
			@rec_menu_name nvarchar(30),
			@counter int,
			@parent_id1 int,
			@parent_id2 int,
			@parent_id3 int,
			@new_value nvarchar(max)
			
	exec common_check_record_lock
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @updated_by,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output
		
	if(@return_status=0)
		begin
			return 0
		end
	
	begin transaction
	
	EXEC sp_xml_preparedocument @hDoc OUTPUT,@xml_menu 
	set @counter = 1
	
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
			
			if(select count(user_role_id) from user_role_menu_rights where user_role_id=@id and menu_id=@rec_menu_id)=0
				begin
					insert into user_role_menu_rights(user_role_id,menu_id,update_by,date_updated)
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
							 if(select count(menu_id) from user_role_menu_rights where user_role_id=@id and menu_id=@parent_id1)=0
								begin
									 insert into user_role_menu_rights(user_role_id,menu_id,update_by,date_updated)
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
											 if(select count(menu_id) from user_role_menu_rights where user_role_id=@id and menu_id=@parent_id2)=0
												begin
													 insert into user_role_menu_rights(user_role_id,menu_id,update_by,date_updated)
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
		
	
	exec sp_xml_removedocument @hDoc
	select @error_code='034',@return_status=1
	commit transaction
	return 1
	
end

GO
