USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_other_radiologist_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_other_radiologist_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_other_radiologist_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_other_radiologist_fetch : fetch 
                  other radiologist(s)
** Created By   : Pavel Guha
** Created On   : 17/04/2020
*******************************************************/
--exec master_radiologist_other_radiologist_fetch '08182B91-289E-4862-B4F4-8DD6F3BA807E'
create procedure [dbo].[master_radiologist_other_radiologist_fetch]
    @id uniqueidentifier
as
begin
	set nocount on
	
	select radiologist_id = orad.other_radiologist_id,radiologist_code =r.code,radiologist_name=dbo.InitCap(r.name),sel='Y'
	from radiologist_functional_rights_other_radiologist orad
	inner join radiologists r on r.id = orad.other_radiologist_id
	where orad.radiologist_id = @id
	and r.is_active='Y'
	order by sel desc,radiologist_name
		
	set nocount off
end
GO
