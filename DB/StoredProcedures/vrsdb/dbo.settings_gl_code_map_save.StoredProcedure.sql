USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_gl_code_map_save]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_gl_code_map_save : save gl code mapping
** Created By   : Pavel Guha
** Created On   : 17/06/2020
*******************************************************/
CREATE procedure [dbo].[settings_gl_code_map_save]
    @xml_modality ntext = null,
	@xml_service ntext = null,
	@xml_nonrevenue_head ntext = null,
	@xml_rad_charge ntext = null,
    @updated_by uniqueidentifier,
    @menu_id int,
    @user_name nvarchar(30) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	declare @hDoc1 int,
			@hDoc2 int,
			@hDoc3 int,
			@hDoc4 int,
	        @rowcount int,
			@counter int,
			@modality_id int,
			@modality_name nvarchar(30),
			@category_id int,
			@category_name nvarchar(30),
			@service_id int,
			@service_name nvarchar(50),
			@control_code nvarchar(10),
			@control_desc nvarchar(50),
			@group_id int,
			@group_name nvarchar(100),
			@gl_code nvarchar(5),
			@gl_code_after_hrs nvarchar(5)
			
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
	
	if(@xml_modality is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_modality 
	if(@xml_service is not null) exec sp_xml_preparedocument @hDoc2 output,@xml_service 
	if(@xml_nonrevenue_head is not null) exec sp_xml_preparedocument @hDoc3 output,@xml_nonrevenue_head 
	if(@xml_rad_charge is not null) exec sp_xml_preparedocument @hDoc4 output,@xml_rad_charge 

	set @counter = 1
	
	--modality
	delete from modality_gl_code_link

	select  @rowcount=count(row_id)  
	from openxml(@hDoc1,'modality/row', 2)  
	with( row_id int )
	
	while(@counter<=@rowcount)
		begin
			select  @category_id = category_id,
					@modality_id = modality_id,
					@gl_code     = gl_code
			from openxml(@hDoc1,'modality/row',2)
			with
			( 
				category_id int,
				modality_id int,
				gl_code nvarchar(5),
				row_id int
			) xmlTemp1 where xmlTemp1.row_id = @counter

			select @category_name = name from sys_study_category where id=@category_id
			select @modality_name = name from modality where id=@modality_id

			if(select count(modality_id) from modality_gl_code_link where modality_id=@modality_id and category_id=@category_id)>0
				begin
					rollback transaction
					if(@xml_modality is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_service is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_nonrevenue_head is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_rad_charge is not null) exec sp_xml_removedocument @hDoc4
					select @return_status = 0,@error_code ='329',@user_name=@modality_name + ' under ' + @category_name
					return 0
				end
			
			
			insert into modality_gl_code_link(category_id,modality_id,gl_code,created_by,date_created)
									   values(@category_id,@modality_id,@gl_code,@updated_by,getdate())
			if(@@rowcount = 0)
				begin
					rollback transaction
					if(@xml_modality is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_service is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_nonrevenue_head is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_rad_charge is not null) exec sp_xml_removedocument @hDoc4
					select @return_status = 0,@error_code ='326',@user_name=@modality_name + ' under ' + @category_name
					return 0
				end 

			set @counter=@counter + 1
		end

	--service
	delete from service_gl_code_link

	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc2,'service/row', 2)  
	with( row_id int )
	
	while(@counter<=@rowcount)
		begin
			select  @service_id        = service_id,
			        @modality_id       = modality_id,
					@gl_code           = gl_code,
					@gl_code_after_hrs = gl_code_after_hrs
			from openxml(@hDoc2,'service/row',2)
			with
			( 
				service_id int,
				modality_id int,
				gl_code nvarchar(5),
				gl_code_after_hrs nvarchar(5),
				row_id int
			) xmlTemp2 where xmlTemp2.row_id = @counter

			select @service_name = name from services where id=@service_id
			select @modality_name = name from modality where id=@modality_id

			if(select count(service_id) from service_gl_code_link where modality_id=@modality_id and service_id=@service_id)>0
				begin
					rollback transaction
					if(@xml_modality is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_service is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_nonrevenue_head is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_rad_charge is not null) exec sp_xml_removedocument @hDoc4
					select @return_status = 0,@error_code ='458',@user_name=@service_name + ' for ' + @modality_name
					return 0
				end
			
			insert into service_gl_code_link(service_id,modality_id,gl_code_default,gl_code_after_hrs,created_by,date_created)
									  values(@service_id,@modality_id,@gl_code,@gl_code_after_hrs,@updated_by,getdate())

			if(@@rowcount = 0)
				begin
					rollback transaction
					if(@xml_modality is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_service is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_nonrevenue_head is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_rad_charge is not null) exec sp_xml_removedocument @hDoc4
					select @return_status = 0,@error_code ='327',@user_name = @service_name + ' for ' + @modality_name
					return 0
				end 

			set @counter=@counter + 1
		end

	--non revenue head
	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc3,'nrh/row', 2)  
	with( row_id int )

	while(@counter<=@rowcount)
		begin
			select  @control_code = control_code,
					@gl_code     = gl_code
			from openxml(@hDoc3,'nrh/row',2)
			with
			( 
				control_code nvarchar(10),
				gl_code nvarchar(5),
				row_id int
			) xmlTemp3 where xmlTemp3.row_id = @counter

			select @control_desc = control_desc from ar_non_revenue_acct_control where control_code=@control_code
			
			update ar_non_revenue_acct_control
			set gl_code =@gl_code
			where control_code = @control_code

			if(@@rowcount = 0)
				begin
					rollback transaction
					if(@xml_modality is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_service is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_nonrevenue_head is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_rad_charge is not null) exec sp_xml_removedocument @hDoc4
					select @return_status = 0,@error_code ='328',@user_name=@control_desc
					return 0
				end 

			set @counter=@counter + 1
		end

	--radiologist charge
	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc4,'rc/row', 2)  
	with( row_id int )

	while(@counter<=@rowcount)
		begin
			select  @group_id = group_id,
					@gl_code  = gl_code
			from openxml(@hDoc4,'rc/row',2)
			with
			( 
				group_id int,
				gl_code nvarchar(5),
				row_id int
			) xmlTemp4 where xmlTemp4.row_id = @counter

			select @group_name = name from sys_radiologist_group where id=@group_id
			
			update sys_radiologist_group
			set gl_code =@gl_code
			where id = @group_id

			if(@@rowcount = 0)
				begin
					rollback transaction
					if(@xml_modality is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_service is not null) exec sp_xml_removedocument @hDoc2
					if(@xml_nonrevenue_head is not null) exec sp_xml_removedocument @hDoc3
					if(@xml_rad_charge is not null) exec sp_xml_removedocument @hDoc4
					select @return_status = 0,@error_code ='410',@user_name=@group_name
					return 0
				end 

			set @counter=@counter + 1
		end
		
	
	if(@xml_modality is not null) exec sp_xml_removedocument @hDoc1
	if(@xml_service is not null) exec sp_xml_removedocument @hDoc2
	if(@xml_nonrevenue_head is not null) exec sp_xml_removedocument @hDoc3
	if(@xml_rad_charge is not null) exec sp_xml_removedocument @hDoc4

	select @error_code='034',@return_status=1
	commit transaction
	return 1
	
end

GO
