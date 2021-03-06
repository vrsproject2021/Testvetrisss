USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_modality_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_modality_rights_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_modality_rights_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_modality_rights_fetch : fetch 
                  exception wtudy type(s) 
** Created By   : Pavel Guha
** Created On   : 13/04/2020
*******************************************************/
--exec master_radiologist_modality_rights_fetch '08182B91-289E-4862-B4F4-8DD6F3BA807E'
create procedure [dbo].[master_radiologist_modality_rights_fetch]
    @id uniqueidentifier
as
begin
	set nocount on
	
	select frm.modality_id,modality=dbo.InitCap(m.name),sel='Y'
	from radiologist_functional_rights_modality frm
	inner join modality m on m.id = frm.modality_id
	where frm.radiologist_id = @id
	and m.is_active='Y'
	union
	select modality_id=id,modality=dbo.InitCap(name),sel='N'
	from modality
	where id not in (select modality_id 
	                     from radiologist_functional_rights_modality
					     where radiologist_id = @id)
	and is_active='Y'
	order by sel desc,modality
		
	set nocount off
end
GO
