USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_salespersons]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_salespersons]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_salespersons]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_salespersons : fetch institution 
                  sales persons
** Created By   : Pavel Guha
** Created On   : 21/05/2019
*******************************************************/
--exec master_institution_fetch_salespersons '841903a1-c151-4d91-b3b4-22d9580b4720'
create procedure [dbo].[master_institution_fetch_salespersons]
    @id uniqueidentifier
as
begin
	 set nocount on

	  create table #tmp
	(
		rec_id int identity(1,1),
		salesperson_id uniqueidentifier,
		salesperson_fname nvarchar(80),
		salesperson_lname nvarchar(80),
		salesperson_login_email nvarchar(50),
		salesperson_email nvarchar(50),
		salesperson_mobile nvarchar(20),
		salesperson_pacs_user_id nvarchar(10),
		salesperson_pacs_password nvarchar(200),
		del nvarchar(1) default ''
	)


	insert into #tmp(salesperson_id,salesperson_fname,salesperson_lname,
	       salesperson_login_email,salesperson_email,salesperson_mobile,
		   salesperson_pacs_user_id,salesperson_pacs_password)
	(select salesperson_id,salesperson_fname,salesperson_lname,
	       salesperson_login_email,salesperson_email,salesperson_mobile,
		   salesperson_pacs_user_id=isnull(salesperson_pacs_user_id,''),salesperson_pacs_password= isnull(salesperson_pacs_password,'')
	from institution_salesperson_link 
	where institution_id=@id)
	order by salesperson_fname,salesperson_lname

	if(@@rowcount=0)
		begin
			insert into #tmp(salesperson_id,salesperson_fname,salesperson_lname,
							 salesperson_login_email,salesperson_email,salesperson_mobile,
							 salesperson_pacs_user_id,salesperson_pacs_password)
					   values('00000000-0000-0000-0000-000000000000','','',
					           '','','',
							   '','')

		end

	
	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
