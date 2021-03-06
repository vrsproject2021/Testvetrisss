USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_institution_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_institution_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_institution_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_institution_fetch : fetch 
                  exception institution(s) 
** Created By   : Pavel Guha
** Created On   : 11/04/2020
*******************************************************/
--exec master_radiologist_institution_fetch '08182B91-289E-4862-B4F4-8DD6F3BA807E'
create procedure [dbo].[master_radiologist_institution_fetch]
    @id uniqueidentifier
as
begin
	set nocount on
	
	select ei.institution_id,institution_code=i.code,institution_name = i.name,sel='Y'
	from radiologist_functional_rights_exception_institution ei
	inner join institutions i on i.id = ei.institution_id
	where ei.radiologist_id = @id
	and i.is_active='Y'
	union
	select institution_id=id,institution_code=code,institution_name = name,sel='N'
	from institutions
	where id not in (select institution_id 
	                 from radiologist_functional_rights_exception_institution
					 where radiologist_id = @id)
	and is_active='Y'
	order by sel desc,institution_name 
		
	set nocount off
end
GO
