USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_study_category]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_study_category]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_study_category]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************  
*******************************************************  
** Version  : 1.0.0.0  
** Procedure    : master_institution_fetch_institution_category : fetch   
                  institution category   
** Created By   : Amritesh Maity  
** Created On   : 04/06/2021  
*******************************************************/  
--exec master_institution_fetch_institution_category '669AB247-0FC8-4518-A802-03828A1384F6'  
CREATE procedure [dbo].[master_institution_fetch_study_category]  
    @id uniqueidentifier  
as  
begin  
 set nocount on  
   
 select insc.category_id,category_name=dbo.InitCap(c.name),c.gl_code as category_code,sel='Y'  
 from institution_category_link insc  
 inner join sys_study_category c on c.id = insc.category_id  
 where insc.institution_id = @id
 union  
 select category_id=id,category_name=dbo.InitCap(name),gl_code as category_code,sel='N'  
 from sys_study_category  
 where id not in (select category_id   
                      from institution_category_link  
          where institution_id = @id)
 order by sel desc,category_name  
    
 set nocount off  
end
GO
