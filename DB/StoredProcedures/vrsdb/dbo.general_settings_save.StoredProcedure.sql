USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[general_settings_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[general_settings_save]
GO
/****** Object:  StoredProcedure [dbo].[general_settings_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[general_settings_save]
@xml_general_settings      ntext,
@menu_id          int,
@updated_by       uniqueidentifier,
@user_name        nvarchar(500) = '' output,
@error_code       nvarchar(10)='' output,
@return_status    int =0 output
As
	Begin
	set nocount on
		declare @hDoc1 int,
			    @counter bigint,
	            @rowcount bigint,
				@ctr int,
	            @rc int

		declare @control_code nvarchar(20),
			    @data_type_number int,
		        @data_type_string nvarchar(200),
				@data_type_decimal decimal(12,2)
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
		exec sp_xml_preparedocument @hDoc1 output,@xml_general_settings
		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc1,'general_settings/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @control_code			= control_code,
						   @data_type_number        = data_type_number,
						   @data_type_string		= data_type_string,
						   @data_type_decimal       = data_type_decimal

					from openxml(@hDoc1,'general_settings/row',2)
					with
					( 
						control_code nvarchar(20),
						data_type_number  int,
						data_type_string  nvarchar(200),
						data_type_decimal decimal(12,2),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					update general_settings
					set data_type_string = @data_type_string,
						data_type_number = @data_type_number,
						data_type_decimal     = @data_type_decimal,
						updated_by  = @updated_by,
						date_updated= getdate()
					where control_code = @control_code

					if(@@rowcount=0)
						begin
							rollback transaction
							select @user_name = control_code from general_settings where control_code = @control_code
							select @error_code='388',@return_status=0
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
	    set @error_code='387'
		set nocount off
		return 1
	End
GO
