USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_modality_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_modality_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_modality_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_modality_fetch : fetch 
                  modalities linked to radiologist
** Created By   : Pavel Guha
** Created On   : 26/09/2019
*******************************************************/
--exec master_radiologist_modality_fetch '19AC0EA1-A8B8-49AF-99B7-03B0A24BAC8E'
CREATE procedure [dbo].[master_radiologist_modality_fetch]
    @id uniqueidentifier
as
begin
	 set nocount on
	create table #tmp
	(
		srl_no int identity(1,1),
		modality_id int,
		modality_name nvarchar(50),
		prelim_fee money default 0,
		final_fee money default 0,
		addl_STAT_fee money default 0,
		work_unit int default 0,
		sel nchar(1)
	)

	insert into #tmp(modality_id,modality_name,prelim_fee,final_fee,addl_STAT_fee,work_unit,sel)
	(select rml.modality_id,m.name,prelim_fee,final_fee,addl_STAT_fee,work_unit,'Y'
	from radiologist_modality_link rml
	inner join modality m on m.id = rml.modality_id
	where rml.radiologist_id=@id
	and m.is_active='Y')
	order by m.name

	insert into #tmp(modality_id,modality_name,sel)
	(select id,name,'N'
	from modality 
	where id not in (select modality_id from radiologist_modality_link where radiologist_id=@id)
	and is_active='Y')
	order by name

	select * from #tmp order by srl_no
		
	set nocount off
end

GO
