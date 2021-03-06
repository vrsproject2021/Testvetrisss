USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_patient_search_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_patient_search_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_patient_search_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_patient_search_fetch :  fetch 
                  patient serach data
** Created By   : Pavel Guha
** Created On   : 12/10/2020
*******************************************************/
--exec case_list_patient_search_fetch 'silvia',0,'00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',0,'11111111-1111-1111-1111-111111111111'
--exec case_list_patient_search_fetch 'ziggy',0,'00000000-0000-0000-0000-000000000000','N','30Mar2021','06Apr2021',0,'570d5dfa-4173-4121-99a5-f4d17ef438b7'
CREATE PROCEDURE [dbo].[case_list_patient_search_fetch] 
    @patient_name nvarchar(100) ='',
    @modality_id int=0,
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@consider_received_date nchar(1)='N',
	@received_date_from datetime='01Jan1900',
	@received_date_till datetime='01Jan1900',
	@category_id int =0,
    @user_id uniqueidentifier
as
begin
	set nocount on
	declare @strSQL varchar(max),
	        @user_role_id int,
	        @user_role_code nvarchar(10),
			@login_user_id uniqueidentifier,
			@rad_id uniqueidentifier,
			@VWINSTINFOCount int,
			@rc int,
			@ctr int,
			@db_name nvarchar(50)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	create table #tmpInst
	(
	   id uniqueidentifier
	)
	create table #tmpModality
	(
	   id int
	)
	create table #tmpArchDB
	(
		rec_id int identity(1,1),
		db_name nvarchar(50)
	)

	create table #tmp
	(
		id uniqueidentifier,
		study_uid nvarchar(100),
		patient_name nvarchar(100),
		received_date datetime,
		modality_name nvarchar(30),
		category_name nvarchar(30),
		priority_id int,
		priority_desc nvarchar(30),
		institution_name nvarchar(100),
		institution_id uniqueidentifier, 
		study_status_pacs int,
		status_desc nvarchar(100),
		menu_id int,
		status_last_updated_on datetime
	)

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

    --print @user_role_code

	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code='TRS')
		begin
		    insert into #tmpInst(id)
			(select id from institutions where is_active='Y')
			 order by name

			insert into #tmpModality(id)
			(select id from modality where is_active='Y')
			 order by name
		end
	else if(@user_role_code = 'IU')
		begin
			insert into #tmpInst(id)
			(select id
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_user_link
					   where user_id = @user_id))
			order by name

			insert into #tmpModality(id)
			(select id from modality where is_active='Y')
			 order by name
		end
	else if(@user_role_code = 'AU')
		begin
			insert into #tmpInst(id)
			(select bail.institution_id
			 from billing_account_institution_link bail
			 inner join institutions i on i.id = bail.institution_id
			 inner join billing_account ba on ba.id = bail.billing_account_id
			 where i.is_active='Y'
			 and ba.login_user_id = @user_id)
			 order by i.name

			 insert into #tmpModality(id)
			(select id from modality where is_active='Y')
			 order by name
		end
	else if(@user_role_code = 'SALES')
		begin
			insert into #tmpInst(id)
			(select id
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_salesperson_link
					   where salesperson_user_id = @user_id))
			order by name

			insert into #tmpModality(id)
			(select id from modality where is_active='Y')
			 order by name
		end
	else if(@user_role_code = 'RDL')
		begin
			select @rad_id = id from radiologists where login_user_id = @user_id

			insert into #tmpInst(id)
			(select id from institutions 
			where is_active='Y' 
			and id not in (select institution_id from radiologist_functional_rights_exception_institution where radiologist_id = @rad_id))
			order by name
			
			insert into #tmpModality(id)
			(select id from modality where is_active='Y' and id in (select modality_id from radiologist_functional_rights_modality  where radiologist_id = @rad_id))
			order by name

			select @VWINSTINFOCount = count(right_code) from radiologist_functional_rights_assigned where right_code='VWINSTINFO' and radiologist_id=@rad_id
		end

    set @strSQL = 'insert into #tmp(id,study_uid,received_date,status_last_updated_on,patient_name,modality_name,category_name,priority_id,priority_desc,institution_id,institution_name,study_status_pacs,status_desc,menu_id)'
	set @strSQL= @strSQL + '(select hdr.id,hdr.study_uid,received_date=hdr.synched_on,hdr.status_last_updated_on,'
	set @strSQL= @strSQL + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
	set @strSQL= @strSQL + 'modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),hdr.priority_id,priority_desc= isnull(p.priority_desc,''''),'
	set @strSQL= @strSQL + 'hdr.institution_id,'
	
	if(@user_role_code = 'RDL' and @VWINSTINFOCount=0)
		begin
			set @strSQL= @strSQL + 'institution_name= ins.code,'
		end
	else
		begin
			set @strSQL= @strSQL + 'institution_name= dbo.initcap(ins.name),'
		end
	
	set @strSQL= @strSQL + 'hdr.study_status_pacs,'
	set @strSQL= @strSQL + 'status_desc = stat.vrs_status_desc,'
	set @strSQL= @strSQL + 'stat.menu_id '
	set @strSQL= @strSQL + 'from study_hdr hdr '
	set @strSQL= @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
	set @strSQL= @strSQL + 'left outer join sys_study_category c on c.id= hdr.category_id '
	set @strSQL= @strSQL + 'inner join institutions ins on ins.id= hdr.institution_id '
	set @strSQL= @strSQL + 'left outer join sys_priority p on p.priority_id= hdr.priority_id '
	set @strSQL= @strSQL + 'inner join sys_study_status_pacs stat on stat.status_id= hdr.study_status_pacs '
	set @strSQL= @strSQL + 'where hdr.deleted = ''N'' ' 
	set @strSQL= @strSQL + 'and hdr.institution_id in (select id from #tmpInst) ' 
	set @strSQL= @strSQL + 'and hdr.modality_id in (select id from #tmpModality) ' 

	if(isnull(@patient_name,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(isnull(hdr.patient_fname,'''') + + '' '' +  upper(isnull(hdr.patient_lname,''''))) like ''%'+upper(@patient_name)+'%'' ' 
		end
	if (isnull(@modality_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and hdr.modality_id = '+ convert(varchar,@modality_id)
		 end
	else
		begin
			set @strSQL=@strSQL+' and hdr.modality_id in (select id from #tmpModality) '
		end
    if (isnull(@category_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and hdr.category_id = '+ convert(varchar,@category_id)
		 end
	if (isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and hdr.institution_id = '''+ convert(varchar(36),@institution_id) + ''' '
		end
	else
		begin
			set @strSQL=@strSQL+' and hdr.institution_id in (select id from #tmpInst) '
		end
	if (isnull(@consider_received_date,'N') ='Y')
		begin
			set @strSQL=@strSQL+'and received_date between '''+ convert(varchar(11),@received_date_from,106) + ''' and ''' + convert(varchar(11),@received_date_till,106) + ''' '
		 end

	set @strSQL= @strSQL + ')'
	exec(@strSQL)

	insert into #tmpArchDB([db_name]) values('vrsdb')
	insert into #tmpArchDB([db_name]) (select [db_name] from sys_archive_db)

	select @rc=count(rec_id),@ctr=1 from  #tmpArchDB

	while(@ctr <= @rc)
		begin
			select @db_name = [db_name] from #tmpArchDB where rec_id = @ctr

			set @strSQL = 'insert into #tmp(id,study_uid,received_date,status_last_updated_on,patient_name,modality_name,category_name,priority_id,priority_desc,institution_id,institution_name,study_status_pacs,status_desc,menu_id)'
			set @strSQL= @strSQL + '(select hdr.id,hdr.study_uid,received_date=hdr.synched_on,hdr.status_last_updated_on,'
			set @strSQL= @strSQL + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
			set @strSQL= @strSQL + 'modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),hdr.priority_id,priority_desc= isnull(p.priority_desc,''''),'
			set @strSQL= @strSQL + 'hdr.institution_id,'

			if(@user_role_code = 'RDL' and @VWINSTINFOCount=0)
				begin
					set @strSQL= @strSQL + 'institution_name= ins.code,'
				end
			else
				begin
					set @strSQL= @strSQL + 'institution_name= dbo.initcap(ins.name),'
				end

			set @strSQL= @strSQL + 'study_status_pacs=11,'
			set @strSQL= @strSQL + 'status_desc = ''Archived'','
			set @strSQL= @strSQL + 'menu_id=(select menu_id from sys_study_status_pacs where status_id=11) '
			set @strSQL= @strSQL + 'from ' + @db_name + '..study_hdr_archive hdr '
			set @strSQL= @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
			set @strSQL= @strSQL + 'left outer join sys_study_category c on c.id= hdr.category_id '
			set @strSQL= @strSQL + 'inner join institutions ins on ins.id= hdr.institution_id '
			set @strSQL= @strSQL + 'left outer join sys_priority p on p.priority_id= hdr.priority_id '
			set @strSQL= @strSQL + 'inner join sys_study_status_pacs stat on stat.status_id= hdr.study_status_pacs '
			set @strSQL= @strSQL + 'where hdr.deleted = ''N'' ' 
			set @strSQL= @strSQL + 'and hdr.institution_id in (select id from #tmpInst) ' 
			set @strSQL= @strSQL + 'and hdr.modality_id in (select id from #tmpModality) ' 

			if(isnull(@patient_name,'') <>'')
				begin
					set @strSQL= @strSQL + ' and upper(isnull(hdr.patient_fname,'''') + + '' '' +  upper(isnull(hdr.patient_lname,''''))) like ''%'+upper(@patient_name)+'%'' ' 
				end
			if (isnull(@modality_id,0) <>0)
				 begin
					set @strSQL=@strSQL+' and hdr.modality_id = '+ convert(varchar,@modality_id)
				 end
			else
				begin
					set @strSQL=@strSQL+' and hdr.modality_id in (select id from #tmpModality) '
				end
			if (isnull(@category_id,0) <>0)
				 begin
					set @strSQL=@strSQL+' and hdr.category_id = '+ convert(varchar,@category_id)
				 end
			if (isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
				begin
					set @strSQL=@strSQL+' and hdr.institution_id = '''+ convert(varchar(36),@institution_id) + ''' '
				end
			else
				begin
					set @strSQL=@strSQL+' and hdr.institution_id in (select id from #tmpInst) '
				end
			if (isnull(@consider_received_date,'N') ='Y')
				begin
					set @strSQL=@strSQL+'and received_date between '''+ convert(varchar(11),@received_date_from,106) + ''' and ''' + convert(varchar(11),@received_date_till,106) + ''' '
				 end

			set @strSQL= @strSQL + ')'
			exec(@strSQL)

			set @ctr = @ctr + 1
		end

	select * from #tmp order by received_date desc,study_uid,patient_name,modality_name

	drop table #tmp
	drop table #tmpInst
	drop table #tmpModality
	drop table #tmpArchDB
	set nocount off
end


GO
