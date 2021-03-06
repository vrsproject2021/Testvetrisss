USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rule_radiologist_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_case_notification_rule_radiologist_fetch]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rule_radiologist_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_case_notification_rule_radiologist_fetch : fetch radiologists
** Created By   : Pavel Guha
** Created On   : 09/10/2019
*******************************************************/
--exec settings_case_notification_rule_radiologist_fetch  0
create procedure [dbo].[settings_case_notification_rule_radiologist_fetch]
    @id int
as
begin
	 set nocount on

	

	select cnrd.radiologist_id,r.name,notify_if_scheduled= isnull(cnrd.notify_if_scheduled,'N'),notify_always= isnull(cnrd.notify_always,'N'),
	                user_id = u.id
	from case_notification_rule_radiologist_dtls cnrd
	inner join radiologists r on r.id=cnrd.radiologist_id
	inner join users u on u.code = r.code
	where cnrd.rule_no=@id
	and r.is_active='Y'
	union
	select radiologist_id = r.id,r.name,notify_if_scheduled= 'N',notify_always= 'N',
	       user_id = u.id
	from radiologists r
	inner join users u on u.code = r.code
	where r.is_active='Y'
	and r.id not in (select distinct radiologist_id from case_notification_rule_radiologist_dtls where rule_no=@id)
	order by r.name

	
		
	set nocount off
end

GO
