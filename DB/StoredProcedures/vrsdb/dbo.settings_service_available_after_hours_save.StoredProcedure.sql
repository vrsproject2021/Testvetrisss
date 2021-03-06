USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_service_available_after_hours_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_service_available_after_hours_save]
GO
/****** Object:  StoredProcedure [dbo].[settings_service_available_after_hours_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_service_available_after_hours_save : 
                  save modality wise service availability after hours
** Created By   : Pavel Guha 
** Created On   : 09/07/2021
*******************************************************/
create Procedure [dbo].[settings_service_available_after_hours_save]
	@xml_modality  ntext,
	@xml_species  ntext,
	@menu_id       int,
	@updated_by    uniqueidentifier,
	@user_name     nvarchar(500) = '' output,
	@error_code    nvarchar(10)='' output,
	@return_status int =0 output
As
	Begin
	set nocount on
	set datefirst 1

	declare @hDoc1 int,
	        @hDoc2 int,
			@counter int,
	        @rowcount int

	declare @service_id int,
			@service_name nvarchar(50),
			@modality_id int,
		    @modality_name nvarchar(30),
			@species_id int,
			@species_name nvarchar(30),
			@available nchar(1),
			@message_display nvarchar(500)

	exec common_check_record_lock
			@menu_id       = @menu_id,
			@record_id     = @menu_id,
			@user_id       = @updated_by,
			@user_name     = @user_name output,
			@error_code    = @error_code output,
			@return_status = @return_status output
		
	if(@return_status=0)
		begin
			return 0
		end

		begin transaction  
		exec sp_xml_preparedocument @hDoc1 output,@xml_modality
		exec sp_xml_preparedocument @hDoc2 output,@xml_species

		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc1,'service/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @service_id			= service_id,
						   @modality_id         = modality_id,
						   @available           = available,
						   @message_display     = message_display
					from openxml(@hDoc1,'service/row',2)
					with
					( 
						service_id int,
						modality_id int,
						available nchar(1),
						message_display nvarchar(500),
						row_id int
					) xmlTemp where xmlTemp.row_id = @counter  

					select @service_name = name from services where id=@service_id
					select @modality_name = name  from modality where id=@modality_id

					if(isnull(@message_display,'')<> '')
						begin
							if(LEN(@message_display) > 500)
								begin
									rollback transaction
									select @user_name = @modality_name
									select @user_name = @user_name + ' of ' + @service_name
									select @error_code='428',@return_status=0
									return 0
								end
						end


					if(select count(modality_id) from settings_service_modality_available_after_hours where modality_id=@modality_id and service_id=@service_id)=0
						begin
							insert into settings_service_modality_available_after_hours(service_id,modality_id,available,message_display,created_by,date_created)
							                                         values(@service_id,@modality_id,@available,@message_display,@updated_by,getdate())
						end
					else
						begin
							update settings_service_modality_available_after_hours
							set available       = @available,
								message_display = @message_display,
								updated_by      = @updated_by,
								date_updated    = getdate()
							where service_id = @service_id
							and modality_id = @modality_id
						end

					if(@@rowcount=0)
						begin
							rollback transaction
							select @user_name = @modality_name
							select @user_name = @user_name + ' of ' + @service_name
							select @error_code='465',@return_status=0
							return 0
						end

					set @counter = @counter + 1
			end

		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc2,'species/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @service_id			= service_id,
						   @species_id         = species_id,
						   @available           = available,
						   @message_display     = message_display
					from openxml(@hDoc2,'species/row',2)
					with
					( 
						service_id int,
						species_id int,
						available nchar(1),
						message_display nvarchar(500),
						row_id int
					) xmlTemp where xmlTemp.row_id = @counter  

					select @service_name = name from services where id=@service_id
					select @species_name = name  from species where id=@species_id

					if(isnull(@message_display,'')<> '')
						begin
							if(LEN(@message_display) > 500)
								begin
									rollback transaction
									select @user_name = @species_name
									select @user_name = @user_name + ' of ' + @service_name
									select @error_code='428',@return_status=0
									return 0
								end
						end


					if(select count(species_id) from settings_service_species_available_after_hours where species_id=@species_id and service_id=@service_id)=0
						begin
							insert into settings_service_species_available_after_hours(service_id,species_id,available,message_display,created_by,date_created)
							                                         values(@service_id,@species_id,@available,@message_display,@updated_by,getdate())
						end
					else
						begin
							update settings_service_species_available_after_hours
							set available       = @available,
								message_display = @message_display,
								updated_by      = @updated_by,
								date_updated    = getdate()
							where service_id = @service_id
							and species_id = @species_id
						end

					if(@@rowcount=0)
						begin
							rollback transaction
							select @user_name = @species_name
							select @user_name = @user_name + ' of ' + @service_name
							select @error_code='465',@return_status=0
							return 0
						end

					set @counter = @counter + 1
			end

		if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
			begin
				exec common_lock_record
					@menu_id       = @menu_id,
					@record_id     = @menu_id,
					@user_id       = @updated_by,
					@error_code    = @error_code output,
					@return_status = @return_status output	
						
				if(@return_status=0)
					begin
						return 0
					end
			end

		commit transaction  
		exec sp_xml_removedocument @hDoc1

	    set @return_status=1
	    set @error_code='034'
		set nocount off
		return 1
	End
GO
