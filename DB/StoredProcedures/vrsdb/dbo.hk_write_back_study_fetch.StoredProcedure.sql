USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_write_back_study_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_write_back_study_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_write_back_study_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_write_back_study_fetch : fetch studies for write back
** Created By   : Pavel Guha
** Created On   : 16/10/2019
*******************************************************/
--exec hk_write_back_study_fetch '02Oct2019 00:00:00','17Oct2019 23:59:59'
CREATE PROCEDURE [dbo].[hk_write_back_study_fetch] 
	@from_date datetime,
	@till_date datetime
as
begin
	set nocount on

	select hdr.id,hdr.study_uid,patient_name=dbo.InitCap(hdr.patient_name),received_date=hdr.synched_on,
	       institution_name = isnull(i.name,''),stat.status_desc,
		   priority_desc = isnull(p.priority_desc,''),hdr.pacs_wb
	from study_hdr hdr
	left outer join institutions i on i.id = hdr.institution_id
	inner join sys_study_status_pacs stat on stat.status_id = hdr.study_status_pacs
	left outer join sys_priority p on p.priority_id = hdr.priority_id
	where hdr.synched_on between @from_date and @till_date
	--and study_status_pacs in (20,50,60)
	union
	select hdr.id,hdr.study_uid,patient_name=dbo.InitCap(hdr.patient_name),received_date=hdr.synched_on,
	       institution_name = isnull(i.name,''),stat.status_desc,
		   priority_desc = isnull(p.priority_desc,''),hdr.pacs_wb
	from study_hdr_archive hdr
	left outer join institutions i on i.id = hdr.institution_id
	inner join sys_study_status_pacs stat on stat.status_id = hdr.study_status_pacs
	left outer join sys_priority p on p.priority_id = hdr.priority_id
	where hdr.synched_on between @from_date and @till_date
	--and study_status_pacs in (20,50,60)
	order by hdr.synched_on desc

	set nocount off
end


GO
