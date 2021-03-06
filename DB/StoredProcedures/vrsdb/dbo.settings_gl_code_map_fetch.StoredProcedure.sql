USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_gl_code_map_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_gl_code_map_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_gl_code_map_fetch : fetch records on load
** Created By   : Pavel Guha
** Created On   : 17/06/2020
*******************************************************/
--exec settings_gl_code_map_fetch 67,'11111111-1111-1111-1111-111111111111','',''
CREATE procedure [dbo].[settings_gl_code_map_fetch]
	@menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as 
begin
	--unlock records
	if(select count(record_id) from sys_record_lock where user_id=@user_id)>0
		 begin
			  delete from sys_record_lock where user_id=@user_id
		 end
	if(select count(record_id) from sys_record_lock_ui where user_id=@user_id)>0
		 begin
			  delete from sys_record_lock_ui where user_id=@user_id
		 end

	select id,name from sys_study_category  order by name 
	select id,name from modality where is_active='Y'  order by name 
	select id,name from services where is_active='Y'  order by name 
	
	if(select count(record_id) from sys_record_lock where menu_id=@menu_id)=0
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
	
end

GO
