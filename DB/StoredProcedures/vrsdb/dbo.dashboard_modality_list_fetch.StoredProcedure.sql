USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_modality_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_modality_list_fetch]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_modality_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************    
*******************************************************    
** Version  : 1.0.0.0    
** Procedure    : dashboard_modality_list_fetch : fetch modality list    
                      
** Created By   : AM   
** Created On   : 21/06/2021    
*******************************************************/    
--exec dashboard_modality_list_fetch    
CREATE PROCEDURE [dbo].[dashboard_modality_list_fetch]     
as    
begin    
 set nocount on    
   select id,name,code,dicom_tag    
   from modality     
   where is_active='Y'      
   order by name    
 set nocount off    
end    
GO
