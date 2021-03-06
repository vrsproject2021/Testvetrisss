USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_service_modality_exception_institution_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_service_modality_exception_institution_save]
GO
/****** Object:  StoredProcedure [dbo].[settings_service_modality_exception_institution_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_service_modality_exception_institution_save : 
                  save modality wise service availability
** Created By   : Pavel Guha 
** Created On   : 25/03/2021
*******************************************************/
CREATE Procedure [dbo].[settings_service_modality_exception_institution_save]
    @service_id int,
	@record_id int,
	@after_hours nchar(1),
	@available nchar(1),
	@message_display nvarchar(500),
	@xml_data      ntext= null,
	@menu_id       int,
	@updated_by    uniqueidentifier,
	@user_name     nvarchar(500) = '' output,
	@error_code    nvarchar(10)='' output,
	@return_status int =0 output
As
	Begin
	set nocount on
	set datefirst 1

	declare @hDoc int,
			@counter int,
	        @rowcount int

	declare @institution_id uniqueidentifier,
	        @institution_name nvarchar(100)

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

		if(@after_hours='N')
			begin
				if(select count(modality_id) from settings_service_modality_available where modality_id=@record_id and service_id=@service_id)=0
					begin
						insert into settings_service_modality_available(service_id,modality_id,available,message_display,created_by,date_created)
																	values(@service_id,@record_id,@available,@message_display,@updated_by,getdate())
					end
				else
					begin
						update settings_service_modality_available
						set available       = @available,
							message_display = @message_display,
							updated_by      = @updated_by,
							date_updated    = getdate()
						where service_id = @service_id
						and modality_id = @record_id
					end
			end
		else if(@after_hours='Y')
			begin
				if(select count(modality_id) from settings_service_modality_available_after_hours where modality_id=@record_id and service_id=@service_id)=0
					begin
						insert into settings_service_modality_available_after_hours(service_id,modality_id,available,message_display,created_by,date_created)
																	         values(@service_id,@record_id,@available,@message_display,@updated_by,getdate())
					end
				else
					begin
						update settings_service_modality_available_after_hours
						set available       = @available,
							message_display = @message_display,
							updated_by      = @updated_by,
							date_updated    = getdate()
						where service_id = @service_id
						and modality_id = @record_id
					end
			end

		if(@@rowcount=0)
			begin
				rollback transaction
				select @error_code='035',@return_status=0
				return 0
			end


        delete from settings_service_modality_available_exception_institution where service_id=@service_id and modality_id=@record_id and after_hours=@after_hours

		if(@xml_data is not null)
			begin
				exec sp_xml_preparedocument @hDoc output,@xml_data
				set @counter = 1
				select  @rowcount=count(row_id)  
				from openxml(@hDoc,'institution/row', 2)  
				with( row_id int )

				while(@counter <= @rowcount)
					begin
						   select  @institution_id		= id
							from openxml(@hDoc,'institution/row',2)
							with
							( 
								id uniqueidentifier,
								row_id int
							) xmlTemp where xmlTemp.row_id = @counter  

							select @institution_name = name from institutions where id=@institution_id
							

							insert into settings_service_modality_available_exception_institution(service_id,modality_id,institution_id,after_hours,created_by,date_created)
							                                                               values(@service_id,@record_id,@institution_id,@after_hours,@updated_by,getdate())

							if(@@rowcount=0)
								begin
									rollback transaction
									select @user_name = @institution_name
									select @error_code='467',@return_status=0
									return 0
						end

							set @counter = @counter + 1
					end
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
		exec sp_xml_removedocument @hDoc

	    set @return_status=1
	    set @error_code='034'
		set nocount off
		return 1
	End
GO
