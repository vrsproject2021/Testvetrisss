USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_exception_study_type_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_exception_study_type_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_exception_study_type_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_exception_study_type_fetch : fetch 
                  exception study type(s) 
** Created By   : Pavel Guha
** Created On   : 11/04/2020
*******************************************************/
--exec master_radiologist_exception_study_type_fetch '08182B91-289E-4862-B4F4-8DD6F3BA807E'
create procedure [dbo].[master_radiologist_exception_study_type_fetch]
    @id uniqueidentifier
as
begin
	set nocount on
	
	select est.study_type_id,modality=dbo.InitCap(m.name),study_type = mst.name,sel='Y'
	from radiologist_functional_rights_exception_study_type est
	inner join modality_study_types mst on mst.id = est.study_type_id
	inner join modality m on m.id = mst.modality_id
	where est.radiologist_id = @id
	and mst.is_active='Y'
	order by sel desc,modality,study_type 
		
	set nocount off
end
GO
