USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_species_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_species_rights_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_species_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************  
*******************************************************  
** Version  : 1.0.0.0  
** Procedure    : master_radiologist_species_rights_fetch : fetch   
                  species right   
** Created By   : Amritesh Maity  
** Created On   : 01/06/2021  
*******************************************************/  
--exec master_radiologist_species_rights_fetch '08182B91-289E-4862-B4F4-8DD6F3BA807E'  
Create procedure [dbo].[master_radiologist_species_rights_fetch]  
    @id uniqueidentifier  
as  
begin  
 set nocount on  
   
 select frs.species_id,species_name=dbo.InitCap(s.name),s.code as species_code,sel='Y'  
 from radiologist_functional_rights_species frs  
 inner join species s on s.id = frs.species_id  
 where frs.radiologist_id = @id  
 and s.is_active='Y'  
 union  
 select species_id=id,species_name=dbo.InitCap(name),code as species_code,sel='N'  
 from species  
 where id not in (select species_id   
                      from radiologist_functional_rights_species  
          where radiologist_id = @id)  
 and is_active='Y'  
 order by sel desc,species_name  
    
 set nocount off  
end
GO
