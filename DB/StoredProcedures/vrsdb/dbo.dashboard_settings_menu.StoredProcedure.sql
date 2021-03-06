USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_settings_menu]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_settings_menu]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_settings_menu]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[dashboard_settings_menu]        
    @user_id uniqueidentifier,        
 @id int        
       
As        
 Begin        
         
 set nocount on   
      
 if(select count(id) from sys_dashboard_settings where  parent_id=@id and is_enabled='Y')>0        
  Begin      
   select * from sys_dashboard_settings 
   where parent_id=@id and is_enabled='Y'
   order by display_index
   select top 1 * from sys_dashboard_settings where parent_id=@id and is_enabled='Y' and is_default='Y'      
  End      
    
 set nocount off        
    
 End
GO
