USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_patient_previous_studies_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_patient_previous_studies_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_patient_previous_studies_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_patient_previous_studies_fetch :  fetch 
                  patient's previous study list
** Created By   : Pavel Guha
** Created On   : 18/11/2020
*******************************************************/
--exec case_list_patient_previous_studies_fetch '1bc57ba6-4c58-42cb-99e0-eb31dd473a82','Demicchi Frankie','58d33f5e-09c9-4a47-ba16-eeec924bcb5b','11111111-1111-1111-1111-111111111111'
CREATE PROCEDURE [dbo].[case_list_patient_previous_studies_fetch] 
    @id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @patient_name nvarchar(100),
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@user_id uniqueidentifier
as
begin
	set nocount on

	declare @user_role_id int,
	        @user_role_code nvarchar(10),
	        @rad_id uniqueidentifier,
			@species_id int,
	        @VWINSTINFOCount int,
			@strSQL varchar(max),
	        @db_name nvarchar(50),
			@rc int,
			@ctr int

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	--print @user_role_code

	if(@user_role_code='RDL')
		begin
			select @rad_id = id from radiologists where login_user_id = @user_id
			select @VWINSTINFOCount = count(right_code) from radiologist_functional_rights_assigned where right_code='VWINSTINFO' and radiologist_id=@rad_id
		end

	select @species_id = species_id from study_hdr where id=@id

	--print @VWINSTINFOCount

	create table #tmp
	(
		id uniqueidentifier,
		study_uid nvarchar(100),
		received_date datetime,
		status_last_updated_on datetime,
		study_date datetime,
		patient_name nvarchar(200),
		modality_name nvarchar(30),
		category_name nvarchar(30),
		priority_id int,
		priority_desc nvarchar(30),
		study_status_pacs int,
		status_desc nvarchar(30),
		physician_name nvarchar(200),
		radiologist nvarchar(200),
		final_radiologist nvarchar(200)
	)

	create table #tmpArchDB
	(
		rec_id int identity(1,1),
		db_name nvarchar(50)
	)
	
	insert into #tmp(id,study_uid,received_date,status_last_updated_on,study_date,patient_name,modality_name,category_name,
	                 priority_id,priority_desc,study_status_pacs,status_desc,physician_name,radiologist,final_radiologist)
	 (select hdr.id,hdr.study_uid,received_date=hdr.synched_on,hdr.status_last_updated_on,study_date= isnull(hdr.study_date,'01jan1900'),
			case when hdr.patient_name like '%[^a-zA-Z0-9.&( )_-]%' then '' else dbo.InitCap(isnull(hdr.patient_fname,'') + ' ' +  isnull(hdr.patient_lname,'')) end patient_name,
	       modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),hdr.priority_id,priority_desc= isnull(p.priority_desc,''),
	       hdr.study_status_pacs,
	       stat.status_desc,
		   case
				when (@user_role_code = 'RDL' and @VWINSTINFOCount=0) then isnull(ph.code,'') else isnull(ph.name,'')
		   end physician_name,
		   radiologist = isnull(r1.name,''),
		   final_radiologist= isnull(r2.name,'')
	from study_hdr hdr 
	left outer join modality m on m.id= hdr.modality_id 
	left outer join sys_study_category c on c.id= hdr.category_id
	left outer join sys_priority p on p.priority_id= hdr.priority_id
	inner join sys_study_status_pacs stat on stat.status_id= hdr.study_status_pacs
	left outer join physicians ph on ph.id = hdr.physician_id
	left outer join radiologists r1 on r1.id = hdr.radiologist_id
	left outer join radiologists r2 on r2.id = hdr.final_radiologist_id
	where hdr.deleted = 'N'
	and hdr.id <> @id
	and hdr.institution_id = @institution_id
	and hdr.species_id=@species_id
	and ((upper(isnull(hdr.patient_fname,'') + + ' ' +  upper(isnull(hdr.patient_lname,''))) = upper(@patient_name))
	     or (upper(isnull(hdr.patient_lname,'') + + ' ' +  upper(isnull(hdr.patient_fname,''))) = upper(@patient_name)))
	and isnull(hdr.radiologist_id,'00000000-0000-0000-0000-000000000000') <>(select id from radiologists where is_active='Y' and assign_merged_study ='Y')
	union
	select hdr.id,hdr.study_uid,received_date=hdr.synched_on,hdr.status_last_updated_on,study_date= isnull(hdr.study_date,'01jan1900'),
	       case when hdr.patient_name like '%[^a-zA-Z0-9.&( )_-]%' then '' else dbo.InitCap(isnull(hdr.patient_fname,'') + ' ' +  isnull(hdr.patient_lname,'')) end patient_name,
	       modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),hdr.priority_id,priority_desc= isnull(p.priority_desc,''),
	       hdr.study_status_pacs,
	       stat.status_desc,
		   case
				when (@user_role_code = 'RDL' and @VWINSTINFOCount=0) then isnull(ph.code,'') else isnull(ph.name,'')
		   end physician_name,
		   radiologist = isnull(r1.name,''),
		   final_radiologist= isnull(r2.name,'')
	from study_hdr_archive hdr 
	left outer join modality m on m.id= hdr.modality_id 
	left outer join sys_study_category c on c.id= hdr.category_id
	left outer join sys_priority p on p.priority_id= hdr.priority_id
	inner join sys_study_status_pacs stat on stat.status_id= hdr.study_status_pacs
	left outer join physicians ph on ph.id = hdr.physician_id
	left outer join radiologists r1 on r1.id = hdr.radiologist_id
	left outer join radiologists r2 on r2.id = hdr.final_radiologist_id
	where hdr.deleted = 'N'
	and hdr.id <> @id
	and hdr.institution_id = @institution_id
	and hdr.species_id=@species_id
	and ((upper(isnull(hdr.patient_fname,'') + + ' ' +  upper(isnull(hdr.patient_lname,''))) = upper(@patient_name))
	     or (upper(isnull(hdr.patient_lname,'') + + ' ' +  upper(isnull(hdr.patient_fname,''))) = upper(@patient_name)))
	and isnull(hdr.radiologist_id,'00000000-0000-0000-0000-000000000000') <>(select id from radiologists where is_active='Y' and assign_merged_study ='Y')
    )
	
	insert into #tmpArchDB([db_name]) (select [db_name] from sys_archive_db)

	select @rc=@@rowcount,@ctr=1 from  #tmpArchDB	

	while(@ctr <= @rc)
		begin
			select @db_name = [db_name] from #tmpArchDB where rec_id = @ctr

			set @strSQL= 'insert into #tmp(id,study_uid,received_date,status_last_updated_on,study_date,patient_name,modality_name,category_name,'
	        set @strSQL= @strSQL + 'priority_id,priority_desc,study_status_pacs,status_desc,physician_name,radiologist,final_radiologist)'
			set @strSQL= @strSQL + '(select hdr.id,hdr.study_uid,received_date=hdr.synched_on,hdr.status_last_updated_on,study_date= isnull(hdr.study_date,''01jan1900''),'
	        set @strSQL= @strSQL + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
	        set @strSQL= @strSQL + 'modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),hdr.priority_id,priority_desc= isnull(p.priority_desc,''''),'
	        set @strSQL= @strSQL + 'hdr.study_status_pacs,'
	        set @strSQL= @strSQL + 'stat.status_desc,'

			if (@user_role_code = 'RDL' and @VWINSTINFOCount=0)
				set @strSQL= @strSQL + 'physician_name = isnull(ph.code,''''),'
			else
				set @strSQL= @strSQL + 'physician_name = isnull(ph.name,''''),'

		   set @strSQL= @strSQL + 'radiologist = isnull(r1.name,''''),'
		   set @strSQL= @strSQL + 'final_radiologist= isnull(r2.name,'''') '
		   set @strSQL= @strSQL + 'from ' + @db_name + '..study_hdr_archive hdr '
		   set @strSQL= @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
		   set @strSQL= @strSQL + 'left outer join sys_study_category c on c.id= hdr.category_id '
		   set @strSQL= @strSQL + 'left outer join sys_priority p on p.priority_id= hdr.priority_id '
		   set @strSQL= @strSQL + 'inner join sys_study_status_pacs stat on stat.status_id= hdr.study_status_pacs '
		   set @strSQL= @strSQL + 'left outer join physicians ph on ph.id = hdr.physician_id '
		   set @strSQL= @strSQL + 'left outer join radiologists r1 on r1.id = hdr.radiologist_id '
		   set @strSQL= @strSQL + 'left outer join radiologists r2 on r2.id = hdr.final_radiologist_id '
		   set @strSQL= @strSQL + 'where hdr.deleted = ''N'' '
		   set @strSQL= @strSQL + 'and hdr.id <> ''' + convert(varchar(36),@id) + ''' '
		   set @strSQL= @strSQL + 'and hdr.institution_id =''' +   convert(varchar(36),@institution_id) + ''' '
		   set @strSQL= @strSQL + 'and hdr.species_id= ' + convert(varchar,@species_id) + ' '
		   set @strSQL= @strSQL + 'and ((upper(isnull(hdr.patient_fname,'''') + + '' '' +  upper(isnull(hdr.patient_lname,''''))) = upper(''' + @patient_name + ''')) '
		   set @strSQL= @strSQL + 'or (upper(isnull(hdr.patient_lname,'''') + + '' '' +  upper(isnull(hdr.patient_fname,''''))) = upper(''' + @patient_name + '''))) '
		   set @strSQL= @strSQL + 'and isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') <>(select id from radiologists where is_active=''Y'' and assign_merged_study =''Y''))'

			exec(@strSQL)
			set @ctr=@ctr + 1
		end 

	 select * from #tmp order by received_date desc
	 drop table #tmpArchDB
	 drop table #tmp

	
	set nocount off
end


GO
