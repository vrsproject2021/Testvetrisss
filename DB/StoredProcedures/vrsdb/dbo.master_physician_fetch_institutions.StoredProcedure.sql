USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_physician_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_physician_fetch_institutions]
GO
/****** Object:  StoredProcedure [dbo].[master_physician_fetch_institutions]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_physician_fetch_institutions : fetch institutions 
				  linked to physicians
** Created By   : Pavel Guha
** Created On   : 24/04/2019
*******************************************************/
--exec master_physician_fetch_institutions 'faded955-300e-4517-8f6c-e6513b1889f6'
CREATE procedure [dbo].[master_physician_fetch_institutions]
    @id uniqueidentifier
as
begin
	 set nocount on

	 create table #tmp
	(
		rec_id int identity(1,1),
		institution_id uniqueidentifier,
		del nvarchar(1) default ''
	)
	
	insert into #tmp(institution_id)
	(select pl.institution_id
	from institution_physician_link pl
	inner join institutions i on i.id = pl.institution_id
	where pl.physician_id=@id
	and i.is_active='Y')
	order by i.name 

	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
