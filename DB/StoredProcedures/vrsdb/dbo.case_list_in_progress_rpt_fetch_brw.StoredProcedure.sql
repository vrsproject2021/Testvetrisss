USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_in_progress_rpt_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_in_progress_rpt_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[case_list_in_progress_rpt_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_in_progress_rpt_fetch_brw :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 22/04/2019
*******************************************************/
-- exec case_list_in_progress_rpt_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',-1,0,'00000000-0000-0000-0000-000000000000','21199081-FDC2-4416-8A1B-A96E217F00C5',21
-- exec case_list_in_progress_rpt_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',-1,0,'00000000-0000-0000-0000-000000000000','11111111-1111-1111-1111-111111111111',21
-- exec case_list_in_progress_rpt_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',-1,0,'00000000-0000-0000-0000-000000000000','B555D7E2-ADFE-47A1-94DF-C9371AE80C2B',21

CREATE PROCEDURE [dbo].[case_list_in_progress_rpt_fetch_brw] 
    @patient_name nvarchar(100) ='',
    @modality_id int=0,
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@consider_received_date nchar(1)='N',
	@received_date_from datetime='01Jan1900',
	@received_date_till datetime='01Jan1900',
	@status_id int = -1,
	@category_id int =0,
	@radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@species_id int,
    @user_id uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@filter_study_uid nvarchar(250) ='',
	@priority_id int=0
as
begin
	set nocount on
	declare @strSQL varchar(max),
	        @user_role_id int,
	        @user_role_code nvarchar(10),
			@login_user_id uniqueidentifier,
			@rad_id uniqueidentifier,
			@ACCLOCKSTUDY nchar(1),
			@APIVER nvarchar(200),
			@PACIMGVWRURL nvarchar(200),
			@VWINSTINFOCount int,
			@SCHCASVCENBL nchar(1)

    declare @record_id uniqueidentifier,
            @study_uid nvarchar(100),
		    @menu_text nvarchar(100),
		    @activity_text nvarchar(max),
		    @error_code nvarchar(10),
		    @return_status int

	select @menu_text= menu_desc from sys_menu where menu_id=@menu_id

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id
	

	if(select count(record_id) from sys_record_lock_ui where menu_id = @menu_id and user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
		begin
			select @record_id =record_id from sys_record_lock_ui where menu_id = @menu_id and user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
			select @study_uid = study_uid from study_hdr where id= @record_id 

			 set @activity_text =  'Lock Released'
			 set @error_code=''
			 set @return_status=0

			 exec common_study_user_activity_trail_save
				@study_hdr_id  = @record_id,
				@study_uid     = @study_uid,
				@menu_id       = @menu_id,
				@activity_text = @activity_text,
				@session_id    = @session_id,
				@activity_by   = @user_id,
				@error_code    = @error_code output,
				@return_status = @return_status output

			set @activity_text =  @menu_text  + '==> Lock released => Study UID ' + @study_uid
			set @error_code=''
			set @return_status=0	
				
			exec common_user_activity_log
				@user_id       = @user_id,
				@activity_text = @activity_text,
				@menu_id       = @menu_id,
				@session_id    = @session_id,
				@error_code    = @error_code output,
				@return_status = @return_status output
		end

	delete from sys_record_lock where user_id = @user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
	delete from sys_record_lock_ui where user_id = @user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id

	create table #tmpInst
	(
	   id uniqueidentifier
	)
	create table #tmpModality
	(
	   id int
	)
	create table #tmpPriority
	(
	   priority_id int
	)
	create table #tmpSpecies
	(
	   id int
	)

	select @APIVER = data_type_string
	from general_settings
	where control_code ='APIVER'

	if(@APIVER ='7.2')
		begin
			select @PACIMGVWRURL = data_type_string
			from general_settings
			where control_code ='PACIMGVWRURL'
		end
	else if(@APIVER ='8')
		begin
			select @PACIMGVWRURL = data_type_string
			from general_settings
			where control_code ='WS8IMGVWRURL'
		end

	select @SCHCASVCENBL = data_type_string
	from general_settings
	where control_code ='SCHCASVCENBL'

    --print @user_role_code
	insert into #tmpPriority(priority_id)
			(select priority_id from sys_priority where is_active='Y')
			 order by priority_desc

	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code='TRS')
		begin
		    insert into #tmpInst(id)
			(select id from institutions where is_active='Y')
			 order by name

			insert into #tmpModality(id)
			(select id from modality where is_active='Y')
			 order by name

			insert into #tmpSpecies(id)
			(select id from species where is_active='Y')
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

			 insert into #tmpSpecies(id)
			(select id from species where is_active='Y')
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

			 insert into #tmpSpecies(id)
			(select id from species where is_active='Y')
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

			 insert into #tmpSpecies(id)
			(select id from species where is_active='Y')
			 order by name
		end
	else if(@user_role_code='RDL')
		begin
			select @rad_id = id from radiologists where login_user_id = @user_id
			if(select count(right_code) from radiologist_functional_rights_assigned where right_code='ACCLOCKSTUDY' and radiologist_id=@rad_id)>0
				begin
					set @ACCLOCKSTUDY='Y'
				end
			else
				begin
					set @ACCLOCKSTUDY='N'
				end
			select @VWINSTINFOCount = count(right_code) from radiologist_functional_rights_assigned where right_code='VWINSTINFO' and radiologist_id=@rad_id

			insert into #tmpModality(id)
			(select id from modality where is_active='Y' and id in (select modality_id from radiologist_functional_rights_modality  where radiologist_id = @rad_id))
			order by name

			if(select count(institution_id) from radiologist_functional_rights_exception_institution where radiologist_id = @rad_id)=0
				begin
						insert into #tmpInst(id)
						(select id from institutions where is_active='Y')
						order by name
				end
			else
				begin
						insert into #tmpInst(id)
						(select id from institutions where is_active='Y' 
						and id not in (select institution_id from radiologist_functional_rights_exception_institution  where radiologist_id = @rad_id))
						order by name
				end

			insert into #tmpSpecies(id)
			(select id from species where is_active='Y' and id in (select species_id from radiologist_functional_rights_species  where radiologist_id = @rad_id))
			 order by name
		end

	set @strSQL= 'select hdr.id,hdr.study_uid,received_date=hdr.synched_on,hdr.status_last_updated_on,'
	set @strSQL=  @strSQL + 'time_left  = case when hdr.finishing_datetime>getdate() then isnull(replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)/60)))  + convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)/60) + '':''+ replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)%60)))  + convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)%60),''00:00'') else ''00:00'' end,' 
	set @strSQL=  @strSQL + 'time_left_trans  = case when hdr.transcription_finishing_datetime>getdate() then isnull(replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.transcription_finishing_datetime)/60)))  + convert(varchar,datediff(mi,getdate(),hdr.transcription_finishing_datetime)/60) + '':''+ replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.transcription_finishing_datetime)%60)))  + convert(varchar,datediff(mi,getdate(),hdr.transcription_finishing_datetime)%60),''00:00'') else ''00:00'' end,' 
	set @strSQL=  @strSQL + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
	set @strSQL= @strSQL + 'modality_id = isnull(hdr.modality_id,0),modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),hdr.priority_id,priority_desc= isnull(p.priority_desc,''''),species_name= isnull(s.name,''''),'
	
	if(@user_role_code = 'RDL' and @VWINSTINFOCount=0)
		begin
			set @strSQL= @strSQL + 'institution_name= ins.code,physician_name = isnull(ph.code,''''),'
		end
	else
		begin
			set @strSQL= @strSQL + 'institution_name= dbo.initcap(ins.name),physician_name = dbo.initcap(isnull(ph.name,'''')),'
		end
	
	set @strSQL= @strSQL + 'hdr.institution_id,inst_code = ins.code,inst_name=ins.name,phys_code = isnull(ph.code,''''), hdr.study_status_pacs,'

	if(@user_role_code='RDL')
		begin
			set @strSQL= @strSQL + 'stat.status_desc,'
		end
	else if(@user_role_code = 'AU' or @user_role_code='IU')
		begin
			set @strSQL= @strSQL + 'case when hdr.study_status_pacs=100 then (select vrs_status_desc from sys_study_status_pacs where status_id=60) else stat.vrs_status_desc end status_desc,'
		end
	else
		begin
			set @strSQL= @strSQL + 'status_desc = stat.vrs_status_desc,'
		end
	
	
	set @strSQL= @strSQL + 'hdr.status_last_updated_on, '
	set @strSQL= @strSQL + 'assigned_radiologist=isnull(r.name,''None''),assigned_rad_id = isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000''),'
	set @strSQL= @strSQL + 'PACLOGINURL=replace((select data_type_string from general_settings where control_code=''PACLOGINURL''),''#V1'',hdr.study_uid), '
	set @strSQL= @strSQL + 'PACMAILRPTURL=(select data_type_string from general_settings where control_code=''PACMAILRPTURL'') + hdr.study_uid, '
	set @strSQL= @strSQL + 'PACIMGVWRURL=''' + @PACIMGVWRURL + ''', '
	set @strSQL= @strSQL + 'case when (select count(record_id) from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and record_id=hdr.id)>0 then ''Y'' else ''N'' end locked,'
	
	if(@user_role_code='RDL' and @ACCLOCKSTUDY='Y')
		begin
			set @strSQL= @strSQL + 'case when (select count(record_id) from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and record_id=hdr.id and user_id not in (select login_user_id from transciptionists where is_active=''Y''))>0 then (select name from users where id=(select user_id from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and record_id=hdr.id)) else (select name from radiologists where id = hdr.radiologist_id) end locked_user,'
		end
	else
		begin
			set @strSQL= @strSQL + 'case when (select count(record_id) from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and record_id=hdr.id)>0 then (select name from users where id=(select user_id from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and record_id=hdr.id)) else '''' end locked_user,'
		end
    
	set @strSQL= @strSQL + 'case when isnull((select code from user_roles where id = (select user_role_id from users where id = (select user_id from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and record_id=hdr.id))),'''') = ''TRS'' then ''Y'' else ''N'' end under_trans,'
	set @strSQL= @strSQL + 'case when isnull(r.transcription_required,''N'')=''Y'' and isnull(hdr.dict_tanscriptionist_id,''00000000-0000-0000-0000-000000000000'')<>''00000000-0000-0000-0000-000000000000'' then ''Y'' else ''N'' end req_trans, '

	set @strSQL=  @strSQL + 'patient_id=isnull(hdr.patient_id,''''),accession_no=isnull(hdr.accession_no,''''), '
	--set @strSQL=  @strSQL + 'case when hdr.object_count_pacs - hdr.object_count<= 3 and hdr.object_count>0  then ''Y'' else ''N'' end show_download,'
	set @strSQL =  @strSQL + 'case when (m.track_by = ''O'' and hdr.archive_file_count>0 and hdr.object_count_pacs - hdr.archive_file_count<= 3) then ''Y'' '
	set @strSQL =  @strSQL + 'when (m.track_by = ''I'' and hdr.archive_file_count>0 and hdr.img_count - hdr.archive_file_count<= 0) then ''Y'' else ''N'' end show_download,'
	set @strSQL =  @strSQL + 'hdr.log_available,hdr.species_id '

	set @strSQL= @strSQL + 'from study_hdr hdr '
	set @strSQL= @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
	set @strSQL= @strSQL + 'left outer join species s on s.id= hdr.species_id '
	set @strSQL= @strSQL + 'left outer join sys_study_category c on c.id= hdr.category_id '
	set @strSQL= @strSQL + 'inner join institutions ins on ins.id= hdr.institution_id '
	set @strSQL= @strSQL + 'left outer join sys_priority p on p.priority_id= hdr.priority_id '
	set @strSQL= @strSQL + 'left outer join physicians ph on ph.id= hdr.physician_id '
	set @strSQL= @strSQL + 'inner join sys_study_status_pacs stat on stat.status_id= hdr.study_status_pacs '
	set @strSQL= @strSQL + 'left outer join radiologists r on r.id= hdr.radiologist_id '
	--set @strSQL= @strSQL + 'where hdr.study_status = 2 ' 
	set @strSQL= @strSQL + 'where hdr.deleted = ''N'' ' 

	if(@user_role_code = 'TRS')
		begin
			set @strSQL= @strSQL + 'and hdr.study_status_pacs=60 '
		end
	else if(@user_role_code = 'AU' or @user_role_code='IU')
		begin
			set @strSQL= @strSQL + 'and hdr.study_status_pacs in (10,20,50,60,100) and hdr.final_rpt_released=''N'' '
		end
	else
		begin
			set @strSQL= @strSQL + 'and hdr.study_status_pacs in (10,20,50,60) '
		end

	/******************************************************
	--Radiologist View
	******************************************************/
	if(@user_role_code = 'RDL')
		begin
			if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@rad_id)=0
				begin
					set @strSQL= @strSQL + 'and hdr.id not in (select id from study_hdr where study_status=2 and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) ' 
				end
			else
				begin
					set @strSQL= @strSQL + 'and hdr.id in (select id from study_hdr where study_status=2 and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) ' 
				end

			if(@SCHCASVCENBL='Y')
				begin
					if(select count(right_code) from radiologist_functional_rights_assigned where right_code='UPDFINALRPT' and radiologist_id=@rad_id)>0
						begin
							if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @rad_id)>0
								begin
									set @strSQL= @strSQL + 'and hdr.id not in (select distinct hst.study_hdr_id from study_hdr_study_types hst '
									set @strSQL= @strSQL + 'inner join study_hdr sh on sh.id = hst.study_hdr_id '
									set @strSQL= @strSQL + 'where sh.study_status=2 and hst.study_type_id in (select study_type_id from radiologist_functional_rights_exception_study_type where radiologist_id = ''' + convert(varchar(36),@rad_id) + ''')) '
								end

							if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @rad_id)>0
								begin
									set @strSQL= @strSQL + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@rad_id) + ''' union select other_radiologist_id =''' + convert(varchar(36),@rad_id) + ''' union '
									set @strSQL= @strSQL + 'select prelim_radiologist_id from study_hdr  where prelim_radiologist_id in ('''+ convert(varchar(36),@rad_id) + ''',''00000000-0000-0000-0000-000000000000''))) '
								end
							else
								begin
									set @strSQL= @strSQL + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select radiologist_id = ''' + convert(varchar(36),@rad_id) + ''' union select radiologist_id = ''00000000-0000-0000-0000-000000000000'' union '
									set @strSQL= @strSQL + 'select radiologist_id =prelim_radiologist_id from study_hdr  where prelim_radiologist_id in ('''+ convert(varchar(36),@rad_id) + ''',''00000000-0000-0000-0000-000000000000''))) '
								end
						end
					else
						begin
							set @strSQL= @strSQL + 'and (hdr.radiologist_id =''' + convert(varchar(36),@rad_id) + ''') or '
							set @strSQL= @strSQL + '(isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'')=''00000000-0000-0000-0000-000000000000'' and isnull(p.is_stat,''N'')=''Y'') '
						end
				end
			else
				begin
					
					if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @rad_id)>0
						begin
							set @strSQL= @strSQL + 'and hdr.id not in (select distinct hst.study_hdr_id from study_hdr_study_types hst '
							set @strSQL= @strSQL + 'inner join study_hdr sh on sh.id = hst.study_hdr_id '
							set @strSQL= @strSQL + 'where sh.study_status=2 and hst.study_type_id in (select study_type_id from radiologist_functional_rights_exception_study_type where radiologist_id = ''' + convert(varchar(36),@rad_id) + ''')) '
						end

					if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @rad_id)>0
						begin
							set @strSQL= @strSQL + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@rad_id) + ''' union select other_radiologist_id =''' + convert(varchar(36),@rad_id) + ''' union '
							set @strSQL= @strSQL + 'select prelim_radiologist_id from study_hdr  where prelim_radiologist_id in ('''+ convert(varchar(36),@rad_id) + ''',''00000000-0000-0000-0000-000000000000''))) '
						end
					else
						begin
							set @strSQL= @strSQL + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select radiologist_id = ''' + convert(varchar(36),@rad_id) + ''' union select radiologist_id = ''00000000-0000-0000-0000-000000000000'' union '
							set @strSQL= @strSQL + 'select radiologist_id =prelim_radiologist_id from study_hdr  where prelim_radiologist_id in ('''+ convert(varchar(36),@rad_id) + ''',''00000000-0000-0000-0000-000000000000''))) '
						end
				end
			
			if(select count(right_code) from radiologist_functional_rights_assigned where right_code='ACCLOCKSTUDY' and radiologist_id=@rad_id)=0
				begin
					set @strSQL= @strSQL + 'and hdr.id not in (select record_id from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and user_id<>''' + convert(varchar(36),@user_id) + ''') ' 
				end
			else
				begin
					set @strSQL= @strSQL + 'and hdr.id in (select record_id from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and user_id in (select login_user_id from radiologists where id in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id=''' + convert(varchar(36),@rad_id) + '''))) ' 
				end


			--set @strSQL= @strSQL + 'and (isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ('''+ convert(varchar(36),@rad_id) + ''',''00000000-0000-0000-0000-000000000000'') '
   --         set @strSQL= @strSQL + 'or isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ('''+ convert(varchar(36),@rad_id) + ''',''00000000-0000-0000-0000-000000000000'')) '
		end
	/******************************************************
	--Radiologist View
	******************************************************/

	/******************************************************
	--Transcriptionist View
	******************************************************/
	if(@user_role_code = 'TRS')
		begin
			set @strSQL= @strSQL + 'and hdr.id in ((select id from study_hdr where isnull(dict_tanscriptionist_id,''00000000-0000-0000-0000-000000000000'') = ''00000000-0000-0000-0000-000000000000'' and (isnull(radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select id from radiologists where is_active=''Y'' and transcription_required=''Y'') '
			set @strSQL= @strSQL + ' or isnull(prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select id from radiologists where is_active=''Y'' and transcription_required=''Y'')))) '
			set @strSQL= @strSQL + 'and hdr.id not in (select record_id from sys_record_lock_ui where menu_id='+ convert(varchar,@menu_id) + ' and user_id<>''' + convert(varchar(36),@user_id) + ''') ' 
		end
	/******************************************************
	--Transcriptionist View
	******************************************************/
	if(isnull(@filter_study_uid,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(hdr.study_uid) like ''%'+upper(@filter_study_uid)+'%'' ' 
		end
	if(isnull(@patient_name,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(isnull(hdr.patient_fname,'''') + + '' '' +  upper(isnull(hdr.patient_lname,''''))) like ''%'+upper(@patient_name)+'%'' ' 
		end
	if (isnull(@priority_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and hdr.priority_id = '+ convert(varchar,@priority_id)
		 end
	else
		begin
			set @strSQL=@strSQL+' and hdr.priority_id in (select priority_id from #tmpPriority) '
		end
	if (isnull(@modality_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and hdr.modality_id = '+ convert(varchar,@modality_id)
		 end
	else
		begin
			set @strSQL=@strSQL+' and hdr.modality_id in (select id from #tmpModality) '
		end

	if (isnull(@species_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and hdr.species_id = '+ convert(varchar,@species_id)
		 end
	else
		begin
			set @strSQL=@strSQL+' and hdr.species_id in (select id from #tmpSpecies) '
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
	if (isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			if(@radiologist_id='11111111-1111-1111-1111-111111111111')
				set @strSQL=@strSQL+' and upper(hdr.radiologist_id) =''00000000-0000-0000-0000-000000000000'' '
			else
				set @strSQL=@strSQL+' and upper(hdr.radiologist_id) ='''+ convert(varchar(36),@radiologist_id) + ''' '
		end
	if (isnull(@consider_received_date,'N') ='Y')
		begin
			set @strSQL=@strSQL+'and hdr.synched_on between '''+ convert(varchar(11),@received_date_from,106) + ''' and ''' + convert(varchar(11),@received_date_till,106) + ''' '
		 end
	if (isnull(@status_id,-1) <>-1)
		 begin
			if(@user_role_code = 'AU' or @user_role_code='IU')
				begin
					if(@status_id=60)
						begin
							set @strSQL=@strSQL+' and hdr.study_status_pacs in (60,100) and hdr.final_rpt_released=''N'' '
						end
					else
						begin
							set @strSQL=@strSQL+' and hdr.study_status_pacs = '+ convert(varchar,@status_id)
						end
				end
			else
				begin
					set @strSQL=@strSQL+' and hdr.study_status_pacs = '+ convert(varchar,@status_id)
				end
		 end
	
    set @strSQL= @strSQL + ' order by hdr.finishing_datetime,hdr.study_uid,hdr.patient_name,modality_name'
	--print @strSQL
	exec(@strSQL)
	drop table #tmpInst
	drop table #tmpModality
	drop table #tmpPriority
	drop table #tmpSpecies

	set @activity_text =  @menu_text  + '==> Study list loaded'
	set @error_code=''
	set @return_status=0

	exec common_user_activity_log
		@user_id       = @user_id,
		@activity_text = @activity_text,
		@menu_id       = @menu_id,
		@session_id    = @session_id,
		@error_code    = @error_code output,
		@return_status = @return_status output

	set nocount off
end


GO
