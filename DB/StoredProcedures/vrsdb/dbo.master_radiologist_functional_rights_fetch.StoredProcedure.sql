USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_functional_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_functional_rights_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_functional_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_functional_rights_fetch : fetch 
                  modalities linked to radiologist
** Created By   : Pavel Guha
** Created On   : 04/03/2020
*******************************************************/
--exec master_radiologist_functional_rights_fetch '08182B91-289E-4862-B4F4-8DD6F3BA807E'
CREATE procedure [dbo].[master_radiologist_functional_rights_fetch]
    @id uniqueidentifier
as
begin
	set nocount on
	
	select rfra.right_code,rfr.right_desc,sel='Y'
	from radiologist_functional_rights_assigned rfra
	inner join sys_radiologist_functional_rights rfr on rfr.right_code = rfra.right_code
	where rfra.radiologist_id = @id
	union
	select right_code,right_desc,sel='N'
	from sys_radiologist_functional_rights
	where right_code not in (select right_code 
	                         from radiologist_functional_rights_assigned
							 where radiologist_id = @id)
	order by sel desc,right_desc 
		
	set nocount off
end

GO
