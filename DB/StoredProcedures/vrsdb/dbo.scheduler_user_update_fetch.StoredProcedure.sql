USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_user_update_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_user_update_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_user_update_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_settings_fetch : fetch scheduler settings
** Created By   : Pavel Guha
** Created On   : 01/08/2019
*******************************************************/
-- exec scheduler_user_update_fetch 
CREATE procedure [dbo].[scheduler_user_update_fetch]

as
begin
	select  ul.user_id , ul.user_pacs_user_id , ul.user_pacs_password ,
			case
				when charindex(' ',u.name)>0 then left(u.name,charindex(' ', u.name)-1) + '^' + right(u.name, charindex(' ', reverse(u.name))-1)
				else u.name
			end user_name,
	        user_group='Physician', ul.granted_rights_pacs,user_type = 'IU',
			user_email = isnull(ul.user_email,''),
			user_contact_no=  isnull(ul.user_contact_no,'')
	from institution_user_link ul
	inner join users u on u.id=ul.user_id 
	where ul.updated_in_pacs='N' 
	and u.is_active='Y'
	union
	select  user_id=u.id , r.login_id , r.login_pwd ,
			user_name= isnull(r.lname,'') + '^' + isnull(r.fname,''),
	        user_group='Radiologist', r.granted_rights_pacs,user_type = 'RDL',
			user_email = isnull(r.email_id,''),
			user_contact_no=isnull(r.mobile_no,'')
	from radiologists r
	inner join users u on u.id=r.login_user_id
	where r.updated_in_pacs='N' 
	and u.is_active='Y'
	union
	select  user_id=u.id , trn.login_id , trn.login_pwd ,
			user_name= isnull(trn.lname,'') + '^' + isnull(trn.fname,''),
	        user_group='Transcriptionist', trn.granted_rights_pacs,user_type = 'TRS',
			user_email = isnull(trn.email_id,''),
			user_contact_no=isnull(trn.mobile_no,'')
	from transciptionists trn
	inner join users u on u.id=trn.login_user_id
	where trn.updated_in_pacs='N' 
	and u.is_active='Y'
	union
	select  user_id=u.id , tech.login_id , tech.login_pwd ,
			user_name= isnull(tech.lname,'') + '^' + isnull(tech.fname,''),
	        user_group='Technologist', tech.granted_rights_pacs,user_type = 'TCHN',
			user_email = isnull(tech.email_id,''),
			user_contact_no=isnull(tech.mobile_no,'')
	from technicians tech
	inner join users u on u.id=tech.login_user_id
	where tech.updated_in_pacs='N' 
	and u.is_active='Y'
end

GO
