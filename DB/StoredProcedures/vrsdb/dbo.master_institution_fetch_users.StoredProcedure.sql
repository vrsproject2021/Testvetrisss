USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_users]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_users]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_users]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_users : fetch institution users
** Created By   : Pavel Guha
** Created On   : 27/05/2019
*******************************************************/
--exec master_institution_fetch_users 'd70e6ed7-843c-4c63-ad98-35a19464b4f0'
CREATE procedure [dbo].[master_institution_fetch_users]
    @id uniqueidentifier
as
begin
	 set nocount on

	  create table #tmp
	(
		rec_id int identity(1,1),
		id uniqueidentifier,
		login_id nvarchar(50),
		password nvarchar(200),
		pacs_user_id nvarchar(50),
		pacs_password nvarchar(200),
		email_id nvarchar(50),
		contact_no nvarchar(20),
		is_active nvarchar(1)
	)


	insert into #tmp(id,login_id,password,pacs_user_id,pacs_password,email_id,contact_no,is_active)
	(select iul.user_id,iul.user_login_id,iul.user_pwd,iul.user_pacs_user_id,iul.user_pacs_password,iul.user_email,isnull(iul.user_contact_no,''),is_active
	from institution_user_link iul
	inner join users u on u.id = iul.user_id
	where iul.institution_id=@id)
	order by iul.user_login_id

	
	select * from #tmp

	drop table #tmp
		
	set nocount off
end

GO
