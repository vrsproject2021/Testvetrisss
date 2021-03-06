USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_services_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_services_save]
GO
/****** Object:  StoredProcedure [dbo].[master_services_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_services_save : save 
                  \services records
** Created By   : Pavel Guha
** Created On   : 08/07/2019
*******************************************************/
CREATE procedure [dbo].[master_services_save]
    @xml_data ntext,
    @updated_by uniqueidentifier,
    @menu_id int,
    @user_name nvarchar(500) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on 
	
	declare @hDoc int,
		    @counter bigint,
	        @rowcount bigint,
	        @id int,
			@code nvarchar(10),
			@priority_id int,
			@name nvarchar(50),
			@gl_code nvarchar(5),
			@is_active nchar(1)

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
	exec sp_xml_preparedocument @hDoc output,@xml_data 
	
	
	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc,'data/row', 2)  
	with( row_id bigint )
	
	while(@counter <= @rowcount)
		begin
			select  @id          = id,
					@name        = name,
					@code        = code,
					@priority_id = priority_id,
					@gl_code     = gl_code,
					@is_active   = is_active
			from openxml(@hDoc,'data/row',2)
			with
			( 
				id int,
				name nvarchar(50),
				code nvarchar(10),
				priority_id int,
				gl_code  nvarchar(5),
				is_active nchar(1),
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter  
			
			if(rtrim(ltrim(@code))<> '')
				begin
					if(select count(code) from [services] where code=@code and id<>@id)>0
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='245',@return_status=0,@user_name=@code
							return 0
						end
				end
			else
				begin
					if(@priority_id=0)
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='246',@return_status=0,@user_name=@name
							return 0
						end
				end

			if(select count(name) from [services] where name=@name and id<>@id)>0
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc
					select @error_code='136',@return_status=0,@user_name=@name
					return 0
				end
			
			if(@priority_id>0)
				begin
					if(select count(id) from services where priority_id=@priority_id and id<>@id)>0
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='457',@return_status=0,@user_name=@name
							return 0
						end
				end
						
			if(@id = 0)
				begin
					insert into [services](name,code,priority_id,gl_code,is_active,created_by,date_created)
					              values(@name,@code,@priority_id,@gl_code,@is_active,@updated_by,getdate())
					                                              
					if(@@rowcount=0)
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='066',@return_status=0,@user_name=@name
							return 0
						end
						
				
				end
			else
				begin
				    update [services]
					set    name          = @name,
					       code          = @code,
						   priority_id   = @priority_id,
						   gl_code       = @gl_code,
						   is_active     = @is_active,
						   updated_by     = @updated_by,
						   date_updated  = getdate()
				    where id=@id 
				    
				    if(@@rowcount=0)
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='066',@return_status=0,@user_name=@name
							return 0
						end
						
				
				end
			
			

			set @counter = @counter + 1
		end

		
	commit transaction
	exec sp_xml_removedocument @hDoc
	select @error_code='034',@return_status=1
	set nocount off
	return 1
	
end

GO
