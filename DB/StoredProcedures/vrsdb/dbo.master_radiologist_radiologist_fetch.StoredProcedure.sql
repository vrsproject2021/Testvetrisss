USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_radiologist_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_radiologist_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_radiologist_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_radiologist_fetch : fetch 
                  radiologists(s)
** Created By   : Pavel Guha
** Created On   : 17/04/2020
*******************************************************/
--exec master_radiologist_radiologist_fetch '6CEFF867-BF6F-49A5-9A36-B3D8C5099F5F'
CREATE procedure [dbo].[master_radiologist_radiologist_fetch]
    @id uniqueidentifier
as
begin
	
	set nocount on
	
	select radiologist_id = orad.other_radiologist_id,radiologist_code =r.code,radiologist_name=dbo.InitCap(r.name),sel='Y'
	from radiologist_functional_rights_other_radiologist orad
	inner join radiologists r on r.id = orad.other_radiologist_id
	where orad.radiologist_id = @id
	and r.is_active='Y'
	union
	select radiologist_id=id,radiologist_code=code,radiologist_name=dbo.InitCap(name),sel='N'
	from radiologists
	where id not in (select other_radiologist_id 
	                 from radiologist_functional_rights_other_radiologist
					 where radiologist_id = @id)
	and is_active='Y'
	and id <> @id
	order by sel desc,radiologist_name
		

		
	set nocount off
end

GO
