USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_prelim_rpt_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_prelim_rpt_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[case_list_prelim_rpt_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_prelim_rpt_fetch_brw :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
-- exec case_list_prelim_rpt_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',0,'11111111-1111-1111-1111-111111111111'
-- exec case_list_prelim_rpt_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',0,'A2597B22-905E-45D3-AE0C-4561197F0312'
-- exec case_list_prelim_rpt_fetch_brw '',0,1,'f1c7e42a-5640-48dc-9ad4-9a04f951fc35','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','','N','01Jan1900','01Jan1900','11111111-1111-1111-1111-111111111111'
-- exec case_list_prelim_rpt_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','A66FB1FB-E5B0-4D58-981E-0D4B61BF3AA1'
-- exec case_list_prelim_rpt_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','21199081-FDC2-4416-8A1B-A96E217F00C5'
-- exec case_list_prelim_rpt_fetch_brw '',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','N','01Jan1900','01Jan1900',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','11111111-1111-1111-1111-111111111111',22
CREATE PROCEDURE [dbo].[case_list_prelim_rpt_fetch_brw] 
    @patient_name nvarchar(100) ='',
    @modality_id int=0, 
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@physician_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@consider_received_date nchar(1)='N',
	@received_date_from datetime='01Jan1900',
	@received_date_till datetime='01Jan1900',
	@category_id int =0,
	@prelim_radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@final_radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @user_id uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@filter_study_uid nvarchar(250) =''
as
begin
	set nocount on
	declare @strSQL varchar(max),
	        @user_role_id int,
	        @user_role_code nvarchar(10),
			@APIVER nvarchar(200),
			@PACIMGVWRURL nvarchar(200),
			@login_user_id uniqueidentifier,
			@radiologist_id uniqueidentifier,
			@VWINSTINFOCount int,
			@SCHCASVCENBL nchar(1)

	declare @record_id uniqueidentifier,
	        @study_uid nvarchar(100),
		    @menu_text nvarchar(100),
		    @activity_text nvarchar(max),
		    @error_code nvarchar(10),
		    @return_status int

	select @menu_text = menu_desc from sys_menu where menu_id=@menu_id

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	if(select count(record_id) from sys_record_lock_ui where menu_id = @menu_id and user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
		begin
			select @record_id =record_id from sys_record_lock_ui where menu_id = @menu_id and user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id

			 set @activity_text =  'Lock Released'
			 set @error_code=''
			 set @return_status=0

			 exec common_study_user_activity_trail_save
				@study_hdr_id  = @record_id,
				@study_uid     = '',
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


	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN')
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
			select @radiologist_id = id from radiologists where login_user_id = @user_id
			select @VWINSTINFOCount = count(right_code) from radiologist_functional_rights_assigned where right_code='VWINSTINFO' and radiologist_id=@radiologist_id

			insert into #tmpModality(id)
			(select id from modality where is_active='Y' and id in (select modality_id from radiologist_functional_rights_modality  where radiologist_id = @radiologist_id))
			order by name

			if(select count(institution_id) from radiologist_functional_rights_exception_institution where radiologist_id = @radiologist_id)=0
				begin
						insert into #tmpInst(id)
						(select id from institutions where is_active='Y')
						order by name
				end
			else
				begin
						insert into #tmpInst(id)
						(select id from institutions where is_active='Y' and id not in (select institution_id from radiologist_functional_rights_exception_institution  where radiologist_id = @radiologist_id))
						order by name
				end

			insert into #tmpSpecies(id)
			(select id from species where is_active='Y' and id in (select species_id from radiologist_functional_rights_species  where radiologist_id = @radiologist_id))
			 order by name
		end

	set @strSQL= 'select hdr.id,hdr.study_uid,received_date=hdr.synched_on,'
	set @strSQL=  @strSQL + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
	--set @strSQL=  @strSQL + 'time_left  = isnull(replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)/60)))  + convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)/60) + '':''+ replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)%60)))  + convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)%60),''00:00''),' 
	set @strSQL=  @strSQL + 'time_left  = case when hdr.finishing_datetime>getdate() then isnull(replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)/60)))  + convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)/60) + '':''+ replicate(''0'',2- len(convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)%60)))  + convert(varchar,datediff(mi,getdate(),hdr.finishing_datetime)%60),''00:00'') else ''00:00'' end,' 
	set @strSQL=  @strSQL + 'modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),'

	if(@user_role_code = 'RDL' and @VWINSTINFOCount=0)
		begin
			set @strSQL= @strSQL + 'institution_name= ins.code,physician_name = isnull(ph.code,''''),'
		end
	else
		begin
			set @strSQL= @strSQL + 'institution_name= dbo.initcap(ins.name),physician_name = dbo.initcap(isnull(ph.name,'''')),'
		end
	
	set @strSQL= @strSQL + 'inst_code = ins.code,inst_name=ins.name,phys_code = isnull(ph.code,''''),hdr.status_last_updated_on, '
	set @strSQL= @strSQL + 'prelim_radiologist=isnull(r1.name,''None''),'
	set @strSQL= @strSQL + 'final_radiologist=isnull(r2.name,''None''),'
	set @strSQL= @strSQL + 'PACLOGINURL=replace((select data_type_string from general_settings where control_code=''PACLOGINURL''),''#V1'',hdr.study_uid), '
	set @strSQL= @strSQL + 'PACSRPTVWRURL=replace((select data_type_string from general_settings where control_code=''PACSRPTVWRURL''),''#V1'',hdr.accession_no), '
	set @strSQL= @strSQL + 'PACIMGVWRURL=''' + @PACIMGVWRURL + ''', '
	set @strSQL= @strSQL + 'custom_report=isnull(ins.custom_report,''N''), '
	set @strSQL= @strSQL + 'accession_no=isnull(hdr.accession_no,''''), '
	set @strSQL= @strSQL + 'patient_id=isnull(hdr.patient_id,''''),'
	set @strSQL= @strSQL + 'rpt_fmt=isnull(ins.rpt_format,''P''), '
	--set @strSQL= @strSQL + 'case when hdr.object_count_pacs - hdr.object_count<= 3 and hdr.object_count>0  then ''Y'' else ''N'' end show_download,'
	set @strSQL= @strSQL + 'case when (m.track_by = ''O'' and hdr.archive_file_count>0 and hdr.object_count_pacs - hdr.archive_file_count<= 3) then ''Y'' '
	set @strSQL= @strSQL + 'when (m.track_by = ''I'' and hdr.archive_file_count>0 and hdr.img_count - hdr.archive_file_count<= 0) then ''Y'' else ''N'' end show_download,'
	set @strSQL= @strSQL + 'hdr.log_available '

    set @strSQL= @strSQL + 'from study_hdr hdr '
	set @strSQL= @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
	set @strSQL= @strSQL + 'left outer join sys_study_category c on c.id= hdr.category_id '
	set @strSQL= @strSQL + 'left outer join institutions ins on ins.id= hdr.institution_id '
	set @strSQL= @strSQL + 'left outer join physicians ph on ph.id= hdr.physician_id '
	set @strSQL= @strSQL + 'left outer join radiologists r1 on r1.id= hdr.prelim_radiologist_id '
	set @strSQL= @strSQL + 'left outer join radiologists r2 on r2.id= hdr.final_radiologist_id '
	--set @strSQL= @strSQL + 'where hdr.study_status = 3 '
	set @strSQL= @strSQL + 'where hdr.deleted = ''N'' '  

	if(@user_role_code = 'AU' or @user_role_code='IU')
		begin
			set @strSQL= @strSQL + 'and hdr.study_status_pacs  in (80,100) and hdr.final_rpt_released=''N'' ' 
		end
	else
		begin
			set @strSQL= @strSQL + 'and hdr.study_status_pacs =80 ' 
		end

	set @strSQL= @strSQL + 'and hdr.species_id in (select id from #tmpSpecies) '
	

	/******************************************************
	--Radiologist View
	******************************************************/
	if(@user_role_code = 'RDL')
		begin
		    if(@SCHCASVCENBL='Y')
				begin
					if(select count(right_code) from radiologist_functional_rights_assigned where right_code='UPDFINALRPT' and radiologist_id=@radiologist_id)>0
						begin
							if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)=0
								begin
									set @strSQL= @strSQL + 'and hdr.id not in (select id from study_hdr where study_status=3 and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) ' 
								end
							else
								begin
									set @strSQL= @strSQL + 'and hdr.id in (select id from study_hdr where study_status=3 and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) ' 
								end

							if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @radiologist_id)>0
								begin
									set @strSQL= @strSQL + 'and hdr.id not in (select distinct hst.study_hdr_id from study_hdr_study_types hst '
									set @strSQL= @strSQL + 'inner join study_hdr sh on sh.id = hst.study_hdr_id '
									set @strSQL= @strSQL + 'where sh.study_status=3 and hst.study_type_id in (select study_type_id from radiologist_functional_rights_exception_study_type where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''')) '
								end

					
							--if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id)>0
							--	begin
							--		set @strSQL= @strSQL + 'and (isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''' union '
							--		set @strSQL= @strSQL + 'select prelim_radiologist_id from study_hdr  where prelim_radiologist_id in ('''+ convert(varchar(36),@radiologist_id) + ''',''00000000-0000-0000-0000-000000000000'')) '
							--		set @strSQL= @strSQL + 'or (isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''' union '
							--		set @strSQL= @strSQL + 'select final_radiologist_id from study_hdr  where final_radiologist_id in ('''+ convert(varchar(36),@radiologist_id) + ''',''00000000-0000-0000-0000-000000000000'')))) '
							--	end

					
							set @strSQL= @strSQL + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''+ convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id =''00000000-0000-0000-0000-000000000000'')) '
							set @strSQL= @strSQL + 'or isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''+ convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id =''00000000-0000-0000-0000-000000000000'')) '
							set @strSQL= @strSQL + 'or isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''+ convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + '''))) '
						
						end
					else
						begin
							set @strSQL= @strSQL + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') = '''+ convert(varchar(36),@radiologist_id) +''' or isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') = '''+ convert(varchar(36),@radiologist_id) +''') '
						end
                end
			else
				begin
					if(select count(right_code) from radiologist_functional_rights_assigned where right_code='WRKCNSLTCASE' and radiologist_id=@radiologist_id)=0
						begin
							set @strSQL= @strSQL + 'and hdr.id not in (select id from study_hdr where study_status=3 and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) ' 
						end
					else
						begin
							set @strSQL= @strSQL + 'and hdr.id in (select id from study_hdr where study_status=3 and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) ' 
						end

					if(select count(study_type_id) from radiologist_functional_rights_exception_study_type where radiologist_id = @radiologist_id)>0
						begin
							set @strSQL= @strSQL + 'and hdr.id not in (select distinct hst.study_hdr_id from study_hdr_study_types hst '
							set @strSQL= @strSQL + 'inner join study_hdr sh on sh.id = hst.study_hdr_id '
							set @strSQL= @strSQL + 'where sh.study_status=3 and hst.study_type_id in (select study_type_id from radiologist_functional_rights_exception_study_type where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''')) '
						end

					
					--if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id)>0
					--	begin
					--		set @strSQL= @strSQL + 'and (isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''' union '
					--		set @strSQL= @strSQL + 'select prelim_radiologist_id from study_hdr  where prelim_radiologist_id in ('''+ convert(varchar(36),@radiologist_id) + ''',''00000000-0000-0000-0000-000000000000'')) '
					--		set @strSQL= @strSQL + 'or (isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''' union '
					--		set @strSQL= @strSQL + 'select final_radiologist_id from study_hdr  where final_radiologist_id in ('''+ convert(varchar(36),@radiologist_id) + ''',''00000000-0000-0000-0000-000000000000'')))) '
					--	end

					
					set @strSQL= @strSQL + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''+ convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id =''00000000-0000-0000-0000-000000000000'')) '
					set @strSQL= @strSQL + 'or isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''+ convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id =''00000000-0000-0000-0000-000000000000'')) '
					set @strSQL= @strSQL + 'or isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''+ convert(varchar(36),@radiologist_id) + ''') union (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@radiologist_id) + '''))) '
				end
			--set @strSQL= @strSQL + 'and (isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ('''+ convert(varchar(36),@radiologist_id) + ''',''00000000-0000-0000-0000-000000000000'') '
   --         set @strSQL= @strSQL + 'or isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ('''+ convert(varchar(36),@radiologist_id) + ''',''00000000-0000-0000-0000-000000000000'')) '
		end
	/******************************************************
	--Radiologist View
	******************************************************/
	
	 if(isnull(@filter_study_uid,'') <>'')
			begin
				set @strSQL= @strSQL + ' and upper(hdr.study_uid) like ''%'+upper(@filter_study_uid)+'%'' ' 
			end
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
	if (isnull(@physician_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and hdr.physician_id = '''+ convert(varchar(36),@physician_id) + ''' '
		end
	if (isnull(@consider_received_date,'N') ='Y')
		begin
			set @strSQL=@strSQL+'and hdr.synched_on between '''+ convert(varchar(11),@received_date_from,106) + ''' and ''' + convert(varchar(11),@received_date_till,106) + ''' '
		 end
	if (isnull(@prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and upper(hdr.prelim_radiologist_id) ='''+ convert(varchar(36),@prelim_radiologist_id) + ''' '
		end
	if (isnull(@final_radiologist_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and upper(hdr.final_radiologist_id) ='''+ convert(varchar(36),@final_radiologist_id) + ''' '
		end

    set @strSQL= @strSQL + ' order by status_last_updated_on desc,study_uid,patient_name,modality_name'
	--print @strSQL
	exec(@strSQL)
	drop table #tmpInst
	drop table #tmpModality
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
