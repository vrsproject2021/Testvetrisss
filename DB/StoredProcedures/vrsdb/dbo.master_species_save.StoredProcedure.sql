USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_species_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_species_save]
GO
/****** Object:  StoredProcedure [dbo].[master_species_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_species_save : save 
                  species records
** Created By   : Pavel Guha
** Created On   : 23/04/2019
*******************************************************/
CREATE procedure [dbo].[master_species_save]
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
			@name nvarchar(50),
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
			select  @id        = id,
			        @code      = code,
					@name      = name,
					@is_active = is_active
			from openxml(@hDoc,'data/row',2)
			with
			( 
				id int,
				code nvarchar(10),
				name nvarchar(50),
				is_active nchar(1),
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter  
			
			
			
						
			if(@id = 0)
				begin

					if(select count(code) from species where code=@code and id<>@id)>0
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='074',@return_status=0,@user_name=@name
							return 0
						end

					insert into species(code,name,is_active,created_by,date_created)
					              values(@code,@name,@is_active,@updated_by,getdate())
					                                              
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
					
				    
				    update species
					set    name          = @name,
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
