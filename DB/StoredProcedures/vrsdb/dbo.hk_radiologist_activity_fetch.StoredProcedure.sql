USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_radiologist_activity_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_radiologist_activity_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_radiologist_activity_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_radiologist_activity_fetch :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
-- exec hk_radiologist_activity_fetch '02Sep2019','09Sep2019','00000000-0000-0000-0000-000000000000',2,'X','11111111-1111-1111-1111-111111111111'
CREATE procedure [dbo].[hk_radiologist_activity_fetch] 
	@study_uid nvarchar(100),
    @user_id uniqueidentifier,
	@session_id uniqueidentifier= '00000000-0000-0000-0000-000000000000'
as
begin
	set nocount on
	declare  @user_role_code nvarchar(10)

	select @user_role_code = code from user_roles where id =(select user_role_id from users where id=@user_id)


	if(@user_role_code<> 'RDL')
		begin
			delete from sys_record_lock where user_id = @user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000') = @session_id
			delete from sys_record_lock_ui where user_id = @user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')= @session_id
		end

	
	select id = row_number() over(order by activity_datetime),
		   functionality = isnull(m2.menu_desc + '-->','') + isnull(m1.menu_desc,'System'),
	       suat.activity_text,
		   activity_by  = isnull(u.name,'System'),
		   suat.activity_datetime,
		   session_id = isnull(session_id,'00000000-0000-0000-0000-000000000000')
	from vrslogdb..sys_study_user_activity_trail suat
	left outer join sys_menu m1 on m1.menu_id = suat.menu_id
	left outer join sys_menu m2 on m2.menu_id = m1.parent_id
	left outer join users u on u.id=suat.activity_by
	where suat.study_uid =  @study_uid

	if(select count(id) from study_hdr where study_uid=@study_uid)>0
		begin
			select patient_name from study_hdr where study_uid=@study_uid
		end
	else
		begin
			select patient_name from study_hdr_archive where study_uid=@study_uid
		end


	set nocount off
end


GO
