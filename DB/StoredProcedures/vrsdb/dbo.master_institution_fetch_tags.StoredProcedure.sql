USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_tags]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_tags]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_tags]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_tags : fetch dicom tags
** Created By   : Pavel Guha
** Created On   : 11/12/2019
*******************************************************/
--exec master_institution_fetch_tags '9305A40D-706B-47B9-8E2F-A4422E462053'
create procedure [dbo].[master_institution_fetch_tags]
    @id uniqueidentifier
as
begin
	 set nocount on

    create table #tmp
	(
		rec_id int identity(1,1),
		group_id nvarchar(5),
		element_id nvarchar(5),
		tag_desc nvarchar(500),
		default_value nvarchar(250),
		junk_characters nvarchar(100),
		sel nvarchar(1) default 'N'
	)


	insert into #tmp(group_id,element_id,tag_desc,default_value,junk_characters,sel)
	(select ddt.group_id,ddt.element_id,dt.tag_desc,ddt.default_value,
	        ddt.junk_characters,sel ='Y'
	from institution_dispute_dicom_tags ddt 
	inner join sys_dicom_tags dt on dt.group_id = ddt.group_id and dt.element_id = ddt.element_id
	where ddt.institution_id=@id
	union
	select group_id,element_id,tag_desc,'',
	        '',sel ='N'
	from sys_dicom_tags
	where group_id not in (select group_id from institution_dispute_dicom_tags where institution_id=@id)
	and element_id not in (select element_id from institution_dispute_dicom_tags where institution_id=@id))
	order by sel desc,group_id,element_id



	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
