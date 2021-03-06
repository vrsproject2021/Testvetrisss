USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[general_settings_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[general_settings_fetch]
GO
/****** Object:  StoredProcedure [dbo].[general_settings_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[general_settings_fetch]
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
As
	Begin
		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id

		select * from sys_general_settings_group order by group_id
		select control_code, data_type_number, RTRIM(data_type_string) as data_type_string, data_type_decimal, updated_by, 
				date_updated, group_id, data_type, control_desc,is_password,ui_control,ui_value_list
		from general_settings where isnull(control_desc,'')<>''  
		order by group_id,group_display_index


		if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
			begin
				exec common_lock_record
					@menu_id       = @menu_id,
					@record_id     = @menu_id,
					@user_id       = @user_id,
					@error_code    = @error_code output,
					@return_status = @return_status output	
						
				if(@return_status=0)
					begin
						return 0
					end
			end
	End
GO
