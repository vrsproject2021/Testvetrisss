USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_settings_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_settings_fetch]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_settings_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************      
*******************************************************      
** Version  : 1.0.0.0      
** Procedure    : dashboard_settings_fetch : fetch dashboard settings list      
** Created By   : AM      
** Created On   : 14/06/2021     
*******************************************************/      
create Procedure [dbo].[dashboard_settings_fetch]      
	@menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
As      
 Begin      
     delete from sys_record_lock where user_id = @user_id
	 delete from sys_record_lock_ui where user_id = @user_id
      
	  select * from sys_dashboard_settings where parent_id=0
       
	  select * from sys_dashboard_settings where parent_id<>0    
   
	  select a.id,a.dashboard_menu_id,a.[key],a.slot_count,a.slot_1,a.slot_2,a.slot_3,a.slot_4 
	  from   sys_dashboard_settings_aging a   
	  inner join sys_dashboard_settings d on d.id=a.dashboard_menu_id 
	  where d.parent_id<>0  
      

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
