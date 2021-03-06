USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_productivity_schedule_parameters_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_productivity_schedule_parameters_fetch]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_productivity_schedule_parameters_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_productivity_schedule_parameters_fetch : 
                  getch radiologist schedule parameters
** Created By   : Pavel Guha
** Created On   : 16/09/2019
*******************************************************/
--exec radiologist_productivity_schedule_parameters_fetch '72388AAC-986C-4285-83FD-6E4C9A3BABE5'
--exec radiologist_productivity_schedule_parameters_fetch '11111111-1111-1111-1111-111111111111'
CREATE procedure [dbo].[radiologist_productivity_schedule_parameters_fetch]
	@user_id uniqueidentifier
as
begin
	
	set nocount on

	declare @user_role_id int,
			@user_timezone_id int,
	        @user_role_code nvarchar(10),
			@login_user_id uniqueidentifier,
			@radiologist_id uniqueidentifier=null

	declare @threshold int=0;

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	if(@user_role_code = 'RDL')
		begin
			select @radiologist_id = id, @user_timezone_id=timezone_id from radiologists where login_user_id = @user_id
		end
	else
		begin
			select @user_timezone_id=id from sys_us_time_zones where is_default='Y'
		end

	select @threshold = data_type_number
		from general_settings
		where control_code=N'RADTHSTUDYCNT'
	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id


	--select id,name,identity_color from radiologists where is_active='Y' order by name
	select a.id,a.name,a.identity_color color,a.acct_group_id groupId, b.name groupName, b.group_color groupColor, a.timezone_id timeZoneId, @threshold thresholdPerHr,
			case Replace(Stuff((SELECT ','+case c.right_code when 'UPDFINALRPT' then 'F' when 'UPDPRELIMRPT' then 'P' else '' end 
				FROM   radiologist_functional_rights_assigned c 
				WHERE  c.radiologist_id = a.id 
				FOR XML PATH('')), 1, 2, ''),',','')
			when 'FP' then 2
			when 'PF' then 2
			when 'P' then 1
			else 0 
			end [Rights]
	       
	from radiologists a  
	inner join sys_radiologist_group b on a.acct_group_id=b.id
	where a.is_active='Y' 
	and a.id=case when @radiologist_id is null then a.id else @radiologist_id end
	order by b.display_order, a.name;

	select id,name, group_color color from sys_radiologist_group order by display_order;

	select id, standard_name name, gmt_diff offset, is_default isDefault 
	from sys_us_time_zones 
	where id in (select distinct timezone_id from radiologists where is_active='Y' )
	union
	select id, standard_name name, gmt_diff offset, is_default isDefault 
	from sys_us_time_zones 
	where is_default='Y'
	order by standard_name;

	
	-- default radiologist and rolecode
	if(@user_role_code='RDL')
		begin
			select @radiologist_id defaultReaderId, @user_timezone_id timezoneId ,@user_role_code roleCode 
		end
	else
		begin
			select convert(uniqueidentifier,'00000000-0000-0000-0000-000000000000') defaultReaderId,@user_timezone_id timezoneId, @user_role_code roleCode
		end

	set nocount off

end


GO
