USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_study_type_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_study_type_save]
GO
/****** Object:  StoredProcedure [dbo].[master_study_type_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_study_type_save : save 
                  study type records
** Created By   : Pavel Guha
** Created On   : 10/05/2019
*******************************************************/
CREATE procedure [dbo].[master_study_type_save]
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
	        @id uniqueidentifier,
			@name nvarchar(50),
			@modality_id int,
			@category_id int,
			@validate_study_count nchar(1),
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
			select  @id                   = id,
					@name                 = name,
					@modality_id          = modality_id,
					@category_id          = category_id,
					@validate_study_count = validate_study_count,
					@is_active = is_active
			from openxml(@hDoc,'data/row',2)
			with
			( 
				id uniqueidentifier,
				name nvarchar(50),
				modality_id int,
				category_id int,
				validate_study_count nchar(1),
				is_active nchar(1),
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter  
					
			if(@id = '00000000-0000-0000-0000-000000000000')
				begin

					set @id=newid()

					insert into modality_study_types(id,name,modality_id,category_id,validate_study_count,is_active,created_by,date_created)
					                          values(@id,@name,@modality_id,@category_id,@validate_study_count,@is_active,@updated_by,getdate())
					                                              
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
				    update modality_study_types
					set    name                 = @name,
						   modality_id          = @modality_id,
						   category_id          = @category_id,
						   validate_study_count = @validate_study_count,
						   is_active            = @is_active,
						   updated_by           = @updated_by,
						   date_updated         = getdate()
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
