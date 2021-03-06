USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rule_receipient_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_case_notification_rule_receipient_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rule_receipient_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_case_notification_rule_receipient_fetch : fetch institution physicians
** Created By   : Pavel Guha
** Created On   : 01/10/2019
*******************************************************/
--exec settings_case_notification_rule_receipient_fetch  0
create procedure [dbo].[settings_case_notification_rule_receipient_fetch]
    @id int
as
begin
	 set nocount on

	 create table #tmpUsers
	 (
		rec_id int identity(1,1),
		user_id uniqueidentifier,
		user_name nvarchar(100),
		user_role_id int,
		sel nchar(1)
	 )

	select distinct cnrd.user_role_id,ur.code,ur.name,scheduled= isnull(cnrd.scheduled,'N'),notify_all= isnull(cnrd.notify_all,'N'),
	                recepient_count = (select count(user_id) from case_notification_rule_dtls where user_id<>'00000000-0000-0000-0000-000000000000' and rule_no=@id and user_role_id=cnrd.user_role_id)
	from case_notification_rule_dtls cnrd
	inner join user_roles ur on ur.id= cnrd.user_role_id
	where cnrd.rule_no=@id
	and ur.is_active='Y'
	union
	select user_role_id = ur.id,ur.code,ur.name,scheduled= 'N',notify_all= 'N',
	       recepient_count=0
	from user_roles ur
	where ur.is_active='Y'
	and ur.code <>'SUPP'
	and id not in (select distinct user_role_id from case_notification_rule_dtls where rule_no=@id)
	order by user_role_id

	insert into #tmpUsers(user_id,user_name,user_role_id,sel)
	(select cnrd.user_id,user_name = u.name,u.user_role_id,sel='Y'
	from case_notification_rule_dtls cnrd
	inner join users u on u.id=cnrd.user_id
	where u.is_active='Y'
	and cnrd.rule_no=@id
	union
	select user_id = u.id,user_name = u.name,u.user_role_id,sel='N'
	from users u 
	inner join user_roles ur on ur.id = u.user_role_id
	where u.is_active='Y'
	and ur.code not in ('SUPP','IU')
	and u.id not in (select user_id from case_notification_rule_dtls where rule_no=@id))
	order by user_role_id,sel desc,u.name

	select * from #tmpUsers
	drop table #tmpUsers
		
	set nocount off
end

GO
