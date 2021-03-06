USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_institutions_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologist_institutions_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologist_institutions_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologist_institutions_fetch : fetch 
                  modalities linked to radiologist
** Created By   : Pavel Guha
** Created On   : 04/03/2020
*******************************************************/
--exec master_radiologist_institutions_fetch '08182B91-289E-4862-B4F4-8DD6F3BA807E'
create procedure [dbo].[master_radiologist_institutions_fetch]
    @id uniqueidentifier
as
begin
	 set nocount on
	create table #tmp
	(
		id uniqueidentifier,
		code nvarchar(5),
		name nvarchar(100),
		sel nchar(1)
	)

	insert into #tmp(id,code,name,sel)
	(select sil.institution_id,i.code,i.name,'Y'
	from radiologist_signage_institution_link sil
	inner join institutions i on i.id = sil.institution_id
	where sil.radiologist_id=@id
	and i.is_active='Y')
	order by i.name

	insert into #tmp(id,code,name,sel)
	(select id,code,name,'N'
	from institutions 
	where id not in (select institution_id from radiologist_signage_institution_link where radiologist_id=@id)
	and is_active='Y')
	order by name

	select * from #tmp order by sel desc, name
		
	set nocount off
end

GO
