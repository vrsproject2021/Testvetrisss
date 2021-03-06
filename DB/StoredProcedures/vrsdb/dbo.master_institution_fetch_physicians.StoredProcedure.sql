USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_physicians]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_physicians]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_physicians]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_physicians : fetch institution physicians
** Created By   : Pavel Guha
** Created On   : 24/04/2019
*******************************************************/
--exec master_institution_fetch_physicians '841903a1-c151-4d91-b3b4-22d9580b4720'
CREATE procedure [dbo].[master_institution_fetch_physicians]
    @id uniqueidentifier
as
begin
	 set nocount on

	  create table #tmp
	(
		rec_id int identity(1,1),
		physician_id uniqueidentifier,
		physician_fname nvarchar(80),
		physician_lname nvarchar(80),
		physician_credentials nvarchar(30),
		physician_email nvarchar(500),
		physician_mobile nvarchar(500),
		del nvarchar(1) default ''
	)


	insert into #tmp(physician_id,physician_fname,physician_lname,physician_credentials,
	                 physician_email,physician_mobile)
	(select physician_id,physician_fname,physician_lname,physician_credentials,
	        physician_email,physician_mobile
	from institution_physician_link 
	where institution_id=@id)
	order by physician_fname,physician_lname

	-- create table #tmp
	--(
	--	rec_id int identity(1,1),
	--	physician_id uniqueidentifier,
	--	physician_name nvarchar(200),
	--	physician_email nvarchar(50),
	--	physician_mobile nvarchar(20),
	--	del nvarchar(1) default ''
	--)
	
	--insert into #tmp(physician_id,physician_name,physician_email,physician_mobile)
	--(select pl.physician_id,pl.physician_name,pl.physician_email,pl.physician_mobile
	--from institution_physician_link pl
	--inner join physicians ph on ph.id = pl.physician_id
	--where pl.institution_id=@id)
	--order by ph.name

	--update #tmp
	--set physician_name = isnull((select name from physicians where id=#tmp.physician_id),'')
	--where isnull(physician_name,'') = ''
	--and physician_id <>'00000000-0000-0000-0000-000000000000'

	--update #tmp
	--set physician_email = isnull((select email_id from physicians where id=#tmp.physician_id),'')
	--where isnull(physician_email,'') = ''
	--and physician_id <>'00000000-0000-0000-0000-000000000000'

	--update #tmp
	--set physician_mobile = isnull((select mobile_no from physicians where id=#tmp.physician_id),'')
	--where isnull(physician_mobile,'') = ''
	--and physician_id <>'00000000-0000-0000-0000-000000000000'

	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
