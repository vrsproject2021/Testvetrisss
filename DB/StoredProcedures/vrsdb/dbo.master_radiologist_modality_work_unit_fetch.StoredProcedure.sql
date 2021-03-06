USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_modality_work_unit_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_modality_work_unit_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_modality_work_unit_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_modality_work_unit_fetch : fetch 
                  modalities linked to radiologist
** Created By   : Pavel Guha
** Created On   : 26/09/2019
*******************************************************/
--exec master_radiologist_modality_work_unit_fetch '19AC0EA1-A8B8-49AF-99B7-03B0A24BAC8E'
create procedure [dbo].[master_radiologist_modality_work_unit_fetch]
    @id uniqueidentifier
as
begin
	 set nocount on
	create table #tmp
	(
		srl_no int identity(1,1),
		modality_id int,
		modality_name nvarchar(50),
		work_unit int default 0	
	)

	insert into #tmp(modality_id,modality_name,work_unit)
	(select mwu.modality_id,m.name,mwu.work_unit
	from radiologist_modality_work_units mwu
	inner join modality m on m.id = mwu.modality_id
	where mwu.radiologist_id=@id
	and m.is_active='Y'
	union
	select modality_id=id,modality_name=name, work_unit = 0
	from modality
	where is_active='Y'
	and id not in (select modality_id from radiologist_modality_work_units where radiologist_id=@id))
	order by name

	select * from #tmp order by srl_no
		
	set nocount off
end

GO
