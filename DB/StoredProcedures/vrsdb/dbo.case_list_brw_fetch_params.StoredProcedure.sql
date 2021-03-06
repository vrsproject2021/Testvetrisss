USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_brw_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_brw_fetch_params]
GO
/****** Object:  StoredProcedure [dbo].[case_list_brw_fetch_params]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_brw_fetch_params : fetch case list
                  browser parameters 
** Created By   : Pavel Guha
** Created On   : 19/04/2019
*******************************************************/
--exec case_list_brw_fetch_params '97f1dc7a-34ef-45a9-87fa-1d7ddf8882f5'
CREATE PROCEDURE [dbo].[case_list_brw_fetch_params] 
	@user_id uniqueidentifier
as
begin
	set nocount on
	set datefirst 1

	declare @user_role_id int,
	        @user_role_code nvarchar(10),
			@login_user_id uniqueidentifier,
			@radiologist_id uniqueidentifier,
			@day_no int,
			@start_from datetime,
			@end_at datetime,
			@curr_date_time datetime,
			@beyond_operation_time nchar(1)

	create table #tmpInst
	(
		id uniqueidentifier,
		code nvarchar(10),
		name nvarchar(100)
    )

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

   if(@user_role_code = 'RDL')
		begin
			select @radiologist_id = id from radiologists where login_user_id = @user_id
		end

	--print @user_role_code
	
	--Modality & Species
	 if(@user_role_code = 'RDL')
		begin
			select id,name,code,dicom_tag
			from modality 
			where is_active='Y'
			and id in (select modality_id from radiologist_functional_rights_modality where radiologist_id=@radiologist_id) 
			order by name
			
			select id,name 
			from species 
			where is_active='Y' 
			and id in (select species_id from radiologist_functional_rights_species where radiologist_id = @radiologist_id)
			order by name
		end
	 else
		begin
			select id,name,code,dicom_tag from modality where is_active='Y' order by name
			select id,name from species where is_active='Y' order by name
		end

    --Institutions
	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN')
		begin
			insert into #tmpInst(id,code,name)
			(select id,code,name from institutions where is_active='Y')
			 order by name
		end
	else if(@user_role_code = 'RDL')
		begin
			if(select count(institution_id) from radiologist_functional_rights_exception_institution where radiologist_id = @radiologist_id)=0
				begin
					insert into #tmpInst(id,code,name)
					(select id,code,name from institutions where is_active='Y')
					 order by name
				end
			else
				begin
					insert into #tmpInst(id,code,name)
					(select id,code,name from institutions 
					where is_active='Y' 
					and id not in (select institution_id from radiologist_functional_rights_exception_institution where radiologist_id = @radiologist_id))
					order by name
				end
		end
	else if(@user_role_code = 'IU')
		begin
			insert into #tmpInst(id,code,name)
			(select id,code,name 
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_user_link
					   where user_id = @user_id)) 
			order by name
		end
	else if(@user_role_code = 'AU')
		begin
			insert into #tmpInst(id,code,name)
			(select id = bail.institution_id,i.code,i.name
			from billing_account_institution_link bail
			inner join institutions i on i.id = bail.institution_id
			inner join billing_account ba on ba.id = bail.billing_account_id
			where i.is_active='Y'
			and ba.login_user_id = @user_id)
			order by i.name
		end
	else if(@user_role_code = 'SALES')
		begin
			insert into #tmpInst(id,code,name)
			(select id,code,name 
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_salesperson_link
					   where salesperson_user_id = @user_id)) 
			order by name
		end

	select * from #tmpInst order by name

	--In Progress Status
	select status_id,status_desc,vrs_status_desc 
	from sys_study_status_pacs 
	where vrs_status_id=2 
	and status_id>20 order by status_id 

	--Status
	select status_id,status_desc,vrs_status_desc 
	from sys_study_status_pacs 
	order by status_id 

	--Work List Status
	select status_id,status_desc
	from sys_study_status_pacs 
	where status_id>20 
	order by status_id 

	--Category
	select id,name,is_default from sys_study_category  order by name
	--Priority
	select priority_id,priority_desc,is_stat from sys_priority where is_active='Y' order by priority_desc

	--Control Codes
	select control_code,
	       data_type_string
	from general_settings
	where control_code in ('APIVER','WS8SRVIP','WS8CLTIP','WS8SRVUID','WS8SRVPWD','PACSTUDYDELURL','FTPABSPATH','DCMMODIFYEXEPATH','PACSARCHIVEFLDR','PACSARCHALTFLDR','SCHCASVCENBL')

	--Radiologist functional rights
	if(@user_role_code='RDL')
		begin
			select right_code from radiologist_functional_rights_assigned where radiologist_id=@radiologist_id order by right_code
		end
	else
		begin
			select right_code from sys_radiologist_functional_rights order by right_code
		end

	--Physicians
	if(select count(id) from #tmpInst)=1
		begin
			select id,code,name=rtrim(ltrim(isnull(fname,'') + ' ' + isnull(lname,'') + ' ' +  isnull(credentials,''))) 
			from physicians 
			where is_active='Y' 
			and id in (select physician_id from institution_physician_link where institution_id = (select id from #tmpInst))
			order by lname
		end
	else
		begin
			select id,code,name=rtrim(ltrim(isnull(fname,'') + ' ' + isnull(lname,'') + ' ' +  isnull(credentials,''))) 
			from physicians 
			where id='00000000-0000-0000-0000-000000000000'
			order by lname
		end

	--Radiologists
    if(@user_role_code='RDL')
		begin
			select id,
			       name 
			from radiologists 
			where is_active='Y'
			and id in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id=@radiologist_id)
			union
			select id,
			       name 
			from radiologists 
			where id=@radiologist_id 
			order by name
		end
	else
		begin
			select id,name 
			from radiologists
			where is_active='Y'
			order by name
		end

	--Final Radiologists
	if(@user_role_code='RDL')
		begin
			select id,
			       name 
			from radiologists 
			where is_active='Y'
			and id in (select other_radiologist_id 
					   from radiologist_functional_rights_other_radiologist 
					   where radiologist_id=@radiologist_id
					   and other_radiologist_id in (select radiologist_id 
					                                from radiologist_functional_rights_assigned
													where right_code='UPDFINALRPT'))
		   union
		   select id,
			       name 
			from radiologists 
			where is_active='Y'
			and id = @radiologist_id
			and id in (select radiologist_id 
					   from radiologist_functional_rights_assigned
					   where right_code='UPDFINALRPT')
		   order by name
		end
	else
		begin
			select id,name 
			from radiologists
			where is_active='Y'
			and id in (select radiologist_id 
					   from radiologist_functional_rights_assigned
					   where right_code='UPDFINALRPT')
			order by name
		end

	--user role code
	select user_role_code = @user_role_code

	--PACS Credentials
	select pacs_user_id,pacs_password from  users where id=@user_id

	-- Abnormal Report Reasons
	select id,reason from abnormal_rpt_reasons order by reason

	select @day_no = datepart(dw,getdate())
	select  @start_from   = convert(varchar(11),getdate(),106) + ' ' + from_time,
			@end_at       = convert(varchar(11),getdate(),106) + ' ' + till_time
	from settings_operation_time
	where day_no = @day_no

	select @curr_date_time = getdate()

	if((@curr_date_time not between @start_from and @end_at))
		set @beyond_operation_time='Y'
	else
		set @beyond_operation_time='N'

	--beyong Operation time
	select beyond_operation_time=@beyond_operation_time
	--country
	select id,name,is_default from sys_country  order by name

	--state
	select id,name from sys_states where country_id=(select id from sys_country where is_default='Y')

	--users
	select id,name from users where is_active='Y' order by name

	--Modality Service Availability
	select sma.service_id,sma.modality_id,s.priority_id,sma.available 
	from settings_service_modality_available sma
	inner join services s on s.id = sma.service_id
	inner join sys_priority p on p.priority_id = s.priority_id
	and p.is_stat='Y'

	--Modality Service Availability (After Hours)
	select sma.service_id,sma.modality_id,s.priority_id,sma.available 
	from settings_service_modality_available_after_hours sma
	inner join services s on s.id = sma.service_id
	inner join sys_priority p on p.priority_id = s.priority_id
	and p.is_stat='Y'

	--Modality Service Exception Institution (Normal Hours)
	select smei.service_id,smei.modality_id,s.priority_id,smei.institution_id 
	from settings_service_modality_available_exception_institution smei
	inner join services s on s.id = smei.service_id
	where smei.after_hours='N'

	--Modality Service Exception Institution (After Hours)
	select smei.service_id,smei.modality_id,s.priority_id,smei.institution_id 
	from settings_service_modality_available_exception_institution smei
	inner join services s on s.id = smei.service_id
	where smei.after_hours='Y'

	--Species Service Availability (Normal Hours)
	select ssa.service_id,ssa.species_id,s.priority_id,ssa.available 
	from settings_service_species_available ssa
	inner join services s on s.id = ssa.service_id
	inner join sys_priority p on p.priority_id = s.priority_id
	and p.is_stat='Y'

	--Species Service Availability (After Hours)
	select ssa.service_id,ssa.species_id,s.priority_id,ssa.available 
	from settings_service_species_available_after_hours ssa
	inner join services s on s.id = ssa.service_id
	inner join sys_priority p on p.priority_id = s.priority_id
	and p.is_stat='Y'

	--Species Service Exception Institution (Normal Hours)
	select ssei.service_id,ssei.species_id,s.priority_id,ssei.institution_id 
	from settings_service_species_available_exception_institution ssei
	inner join services s on s.id = ssei.service_id
	where ssei.after_hours='N'

	--Species Service Exception Institution (After Hours)
	select ssei.service_id,ssei.species_id,s.priority_id,ssei.institution_id 
	from settings_service_species_available_exception_institution ssei
	inner join services s on s.id = ssei.service_id
	where ssei.after_hours='Y'

	--Get yearly database list
	select * from sys_archive_db order by db_year desc
	

	drop table #tmpInst

	set nocount off
end


GO
