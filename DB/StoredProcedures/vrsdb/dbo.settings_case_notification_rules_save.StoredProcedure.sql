USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rules_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_case_notification_rules_save]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rules_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_case_notification_rules_save : save 
                  case notification rules
** Created By   : Pavel Guha
** Created On   : 10/05/2019
*******************************************************/
CREATE procedure [dbo].[settings_case_notification_rules_save]
	@rule_no int =0 output,
	@rule_desc nvarchar(500)='',
	@pacs_status_id int,
	@priority_id int,
	@time_ellapsed_mins int,
	@time_left_mins int,
	@notify_by_time nchar(1),
	@is_active nchar(1),
	@xml_radiologists ntext = null,
    @xml_others ntext = null,
    @updated_by uniqueidentifier,
    @menu_id int,
    @user_name nvarchar(500) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on 
	
	declare @hDoc1 int,
	        @hDoc2 int,
		    @counter bigint,
	        @rowcount bigint,
			@last_rule_no int

	declare @user_role_id int,
			@recepient_type_name nvarchar(30),
	        @scheduled nchar(1),
			@notify_all nchar(1),
			@user_id uniqueidentifier,
			@recepient_name nvarchar(50),
			@radiologist_id uniqueidentifier,
			@notify_always nchar(1)

	begin transaction
	if(@xml_radiologists is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_radiologists 
	if(@xml_others is not null) exec sp_xml_preparedocument @hDoc2 output,@xml_others 

	if(@rule_no = 0)
		begin
			select @last_rule_no = max(rule_no) from case_notification_rule_hdr
			set @rule_no = isnull(@last_rule_no,0) + 1

			insert into case_notification_rule_hdr(rule_no,rule_desc,pacs_status_id,priority_id,time_ellapsed_mins,time_left_mins,notify_by_time,
			                                       is_active,created_by,date_created)
											values(@rule_no,@rule_desc,@pacs_status_id,@priority_id,@time_ellapsed_mins,@time_left_mins,@notify_by_time,
			                                       @is_active,@updated_by,getdate())

			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_radiologists is not null)  exec sp_xml_removedocument @hDoc1
					if(@xml_others is not null)  exec sp_xml_removedocument @hDoc2
					select @error_code='035',@return_status=0
					return 0
				end

		end
	else
		begin
			 exec common_check_record_lock
				@menu_id       = @menu_id,
				@record_id     = @menu_id,
				@user_id       = @updated_by,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
			if(@return_status=0)
				begin
					rollback transaction
					if(@xml_radiologists is not null)  exec sp_xml_removedocument @hDoc1
					if(@xml_others is not null)  exec sp_xml_removedocument @hDoc2
					return 0
				end

			update case_notification_rule_hdr
			set rule_desc          = @rule_desc,
			    pacs_status_id     = @pacs_status_id,
				priority_id        = @priority_id,
				time_ellapsed_mins = @time_ellapsed_mins,
				time_left_mins     = @time_left_mins,
				notify_by_time     = @notify_by_time,
				is_active          = @is_active,
				updated_by         = @updated_by,
				date_updated       = getdate()
			where rule_no = @rule_no  
			
			if(@@rowcount=0)
				begin
					rollback transaction
					if(@xml_radiologists is not null)  exec sp_xml_removedocument @hDoc1
					if(@xml_others is not null)  exec sp_xml_removedocument @hDoc2
					select @error_code='035',@return_status=0
					return 0
				end 

		end
	
	-- Save Radiologists
	delete from case_notification_rule_radiologist_dtls where rule_no = @rule_no

	if(@xml_radiologists is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc1,'data/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @radiologist_id  = radiologist_id,
							@user_id         = user_id,
							@scheduled       = notify_if_scheduled,
							@notify_always   = notify_always		
					from openxml(@hDoc1,'data/row',2)
					with
					( 
						radiologist_id uniqueidentifier,
						user_id uniqueidentifier,
						notify_if_scheduled nchar(1),
						notify_always nchar(1),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					select @recepient_name = name from radiologists where id=@radiologist_id
					
					insert into case_notification_rule_radiologist_dtls(rule_no,radiologist_id,user_id,notify_if_scheduled,notify_always)
																 values(@rule_no,@radiologist_id,@user_id,@scheduled,@notify_always)

				    
					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_radiologists is not null)  exec sp_xml_removedocument @hDoc1
							if(@xml_others is not null)  exec sp_xml_removedocument @hDoc2
							select @error_code='066',@return_status=0,@user_name='Radiologist : ' + @recepient_name
							return 0
						end
			

					set @counter = @counter + 1
				end
		end
	
	-- Save Other Recipients
	delete from case_notification_rule_dtls where rule_no = @rule_no

	if(@xml_others is not null)
		begin
				set @counter = 1
				select  @rowcount=count(row_id)  
				from openxml(@hDoc2,'data/row', 2)  
				with( row_id bigint )
	
				while(@counter <= @rowcount)
					begin
						select  @user_role_id  = user_role_id,
								@scheduled     = scheduled,
								@notify_all    = notify_all,
								@user_id       = user_id
						from openxml(@hDoc2,'data/row',2)
						with
						( 
							user_role_id int,
							scheduled nchar(1),
							notify_all nchar(1),
							user_id uniqueidentifier,
							row_id bigint
						) xmlTemp where xmlTemp.row_id = @counter  

						select @recepient_type_name = name from user_roles where id=@user_role_id
						select @recepient_name = name from users where id=@user_id
					
							insert into case_notification_rule_dtls(rule_no,user_role_id,scheduled,notify_all,user_id)
															values(@rule_no,@user_role_id,@scheduled,@notify_all,@user_id)

				    
						if(@@rowcount=0)
							begin
								rollback transaction
								if(@xml_radiologists is not null)  exec sp_xml_removedocument @hDoc1
								if(@xml_others is not null)  exec sp_xml_removedocument @hDoc2
								select @error_code='066',@return_status=0,@user_name= 'Recepient Type :' + @recepient_type_name + ':: Recepient : ' + @recepient_name
								return 0
							end
			

						set @counter = @counter + 1
					end
		end
		
	commit transaction
	if(@xml_radiologists is not null)  exec sp_xml_removedocument @hDoc1
	if(@xml_others is not null)  exec sp_xml_removedocument @hDoc2
	select @error_code='034',@return_status=1
	set nocount off
	return 1
	
end

GO
