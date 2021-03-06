USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_radiologists_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_schedule_radiologists_fetch]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_radiologists_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_approved_grouped_img_files_to_transfer_fetch : 
                  fetch file download records to finalise
** Created By   : Pavel Guha
** Created On   : 16/09/2019
*******************************************************/
--exec radiologist_schedule_radiologists_fetch
create procedure [dbo].[radiologist_schedule_radiologists_fetch]
as
begin
	
	set nocount on

	create table #tmp
	(
		srl_no int identity(1,1),
		id uniqueidentifier,
		name nvarchar(250),
		sel nvarchar(1) null default 'N'
	)

	insert into #tmp(id,name)
	(select id,name 
	from radiologists 
	where is_active='Y')
	order by name
	

	select * from #tmp

	drop table #tmp

	set nocount off


end


GO
