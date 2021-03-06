USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_final_rpt_excel_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_final_rpt_excel_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_final_rpt_excel_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************  
*******************************************************  
** Version  : 1.0.0.0  
** Procedure    : case_list_final_rpt_excel_fetch :  fetch case(s)  
      finalised & archived  
** Created By   : Pavel Guha  
** Created On   : 31/03/2021  
*******************************************************/
-- exec case_list_final_rpt_excel_fetch '',0,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','N','23jan2021','25jan2021',0,-1,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','N','00000000-0000-0000-0000-000000000000','X','11111111-1111-1111-1111-111111111111'  
CREATE PROCEDURE [dbo].[case_list_final_rpt_excel_fetch]
    @patient_name nvarchar(100) = '',
    @modality_id int = 0,
    @institution_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @physician_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @consider_received_date nchar(1) = 'N',
    @received_date_from datetime = '01Jan1900',
    @received_date_till datetime = '01Jan1900',
    @category_id int = 0,
    @status_id int = -1,
    @approved_by uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @radiologist_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @show_abnormal_rpt nchar(1) = 'N',
    @abnormal_report_reason_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
    @rpt_rel_pending nchar(1) = 'X',
    @user_id uniqueidentifier,
	@db_name nvarchar(50)='vrsdb'
as
begin
    set nocount on
    declare @strSQL varchar(max),
            @user_role_id int,
            @user_role_code nvarchar(10),
            @PACIMGVWRURL nvarchar(200),
            @login_user_id uniqueidentifier,
            @rad_id uniqueidentifier,
            @VWINSTINFOCount int

    delete from sys_record_lock
    where user_id = @user_id
    delete from sys_record_lock_ui
    where user_id = @user_id

    create table #tmp
    (
        id uniqueidentifier,
        study_uid nvarchar(100),
        patient_name nvarchar(100),
        received_date datetime,
        modality_name nvarchar(30),
        category_name nvarchar(30),
        priority_desc nvarchar(30),
        institution_name nvarchar(100),
        physician_name nvarchar(200),
        status_last_updated_on datetime,
        radiologist_pacs nvarchar(250),
        date_dictated datetime,
        final_radiologist nvarchar(250),
        study_status_pacs int,
        status_desc nvarchar(50),
        PACIMGVWRURL nvarchar(200),
        custom_report nchar(1),
        accession_no nvarchar(20),
        patient_id nvarchar(20),
        received_via_dicom_router nchar(1) null
            default 'N',
        rating nchar(1),
        rating_reason nvarchar(30),
        final_rpt_released nchar(1) null
            default 'Y',
        final_rpt_released_by uniqueidentifier,
        is_archive nchar(1) null
            default 'N',
        species_name nvarchar(50),
        submitted_date datetime
            default '01Jan1900',
        image_object_count nvarchar(30)
    )

    create table #tmpInst (id uniqueidentifier)
    create table #tmpModality (id int)


    select @PACIMGVWRURL = data_type_string
    from general_settings
    where control_code = 'WS8IMGVWRURL'

    select @user_role_id = u.user_role_id,
           @user_role_code = ur.code
    from users u
        inner join user_roles ur
            on ur.id = u.user_role_id
    where u.id = @user_id

    if (@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN')
    begin
        insert into #tmpInst
        (
            id
        )
        (select id
         from institutions
         where is_active = 'Y')
        order by name

        insert into #tmpModality
        (
            id
        )
        (select id
         from modality
         where is_active = 'Y')
        order by name
    end
    else if (@user_role_code = 'IU')
    begin
        insert into #tmpInst
        (
            id
        )
        (select id
         from institutions
         where is_active = 'Y'
               and id in (
                             select institution_id from institution_user_link where user_id = @user_id
                         ))
        order by name

        insert into #tmpModality
        (
            id
        )
        (select id
         from modality
         where is_active = 'Y')
        order by name
    end
    else if (@user_role_code = 'SALES')
    begin
        insert into #tmpInst
        (
            id
        )
        (select id
         from institutions
         where is_active = 'Y'
               and id in (
                             select institution_id
                             from institution_salesperson_link
                             where salesperson_user_id = @user_id
                         ))
        order by name

        insert into #tmpModality
        (
            id
        )
        (select id
         from modality
         where is_active = 'Y')
        order by name
    end
    else if (@user_role_code = 'AU')
    begin
        insert into #tmpInst
        (
            id
        )
        (select bail.institution_id
         from billing_account_institution_link bail
             inner join institutions i
                 on i.id = bail.institution_id
             inner join billing_account ba
                 on ba.id = bail.billing_account_id
         where i.is_active = 'Y'
               and ba.login_user_id = @user_id)
        order by i.name

        insert into #tmpModality
        (
            id
        )
        (select id
         from modality
         where is_active = 'Y')
        order by name
    end
    else if (@user_role_code = 'RDL')
    begin
        select @rad_id = id
        from radiologists
        where login_user_id = @user_id
        select @VWINSTINFOCount = count(right_code)
        from radiologist_functional_rights_assigned
        where right_code = 'VWINSTINFO'
              and radiologist_id = @rad_id
    end


    /******************************************************  
 --BEGIN study_hdr  
 ******************************************************/
 if @db_name='vrsdb'
	 Begin
		set @strSQL
			= 'insert into #tmp(id,study_uid,received_date,patient_name,modality_name,category_name,priority_desc,institution_name,physician_name,status_last_updated_on,radiologist_pacs,date_dictated,final_radiologist,'
		set @strSQL
			= @strSQL
			  + 'study_status_pacs,status_desc,PACIMGVWRURL,custom_report,accession_no,patient_id,rating,rating_reason,final_rpt_released,final_rpt_released_by,species_name,submitted_date,image_object_count)'
		set @strSQL = @strSQL + '(select hdr.id,hdr.study_uid,received_date=hdr.synched_on,'
		set @strSQL
			= @strSQL
			  + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
		set @strSQL
			= @strSQL
			  + 'modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),priority_desc=isnull(p.priority_desc,''''),'

		if (@user_role_code = 'RDL' and @VWINSTINFOCount = 0)
		begin
			set @strSQL = @strSQL + 'institution_name= ins.code,physician_name = isnull(ph.code,''''),'
		end
		else
		begin
			set @strSQL
				= @strSQL + 'institution_name= dbo.initcap(ins.name),physician_name = dbo.initcap(isnull(ph.name,'''')),'
		end

		set @strSQL = @strSQL + 'hdr.status_last_updated_on, '
		set @strSQL = @strSQL + 'radiologist_pacs=isnull(r1.name,''None''),'
		set @strSQL = @strSQL + 'date_dictated=isnull(dr.date_created,isnull(pr.date_created,fr.date_created)),'
		set @strSQL = @strSQL + 'final_radiologist=isnull(r2.name,''None''),'
		set @strSQL = @strSQL + 'hdr.study_status_pacs,ssp.status_desc,'
		set @strSQL = @strSQL + 'PACIMGVWRURL=''' + @PACIMGVWRURL + ''', '
		set @strSQL = @strSQL + 'custom_report=isnull(ins.custom_report,''N''), '
		set @strSQL = @strSQL + 'accession_no=isnull(hdr.accession_no,''''), '
		set @strSQL = @strSQL + 'patient_id=isnull(hdr.patient_id,''''),'
		set @strSQL = @strSQL + 'case '
		set @strSQL = @strSQL + 'when isnull(dr.rating,''N'') =''A'' then ''A'' '
		set @strSQL = @strSQL + 'when isnull(pr.rating,''N'') =''A'' then ''A'' else ''N'' end rating, '
		set @strSQL = @strSQL + 'case '
		set @strSQL
			= @strSQL
			  + 'when isnull(dr.rating,''N'') =''A'' then isnull((select reason from abnormal_rpt_reasons where id=dr.rating_reason_id),'''') '
		set @strSQL
			= @strSQL
			  + 'when isnull(pr.rating,''N'') =''A'' then isnull((select reason from abnormal_rpt_reasons where id=pr.rating_reason_id),'''') else ''NORMAL'' end rating_reason, '
		set @strSQL
			= @strSQL
			  + 'hdr.final_rpt_released,hdr.final_rpt_released_by,spe.name species_name,isnull(slog.date_updated,hdr.synched_on)  submitted_date,convert(nvarchar(30),hdr.img_count)+''/''+convert(nvarchar(30),hdr.object_count) image_object_count '
		set @strSQL = @strSQL + 'from study_hdr hdr '
		set @strSQL = @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
		set @strSQL = @strSQL + 'left outer join sys_study_category c on c.id= hdr.category_id '
		set @strSQL = @strSQL + 'left outer join sys_priority p on p.priority_id= hdr.priority_id '
		set @strSQL = @strSQL + 'inner join institutions ins on ins.id= hdr.institution_id '
		set @strSQL = @strSQL + 'left outer join physicians ph on ph.id= hdr.physician_id '
		set @strSQL = @strSQL + 'left outer join radiologists r1 on r1.id= hdr.radiologist_id '
		set @strSQL = @strSQL + 'left outer join radiologists r2 on r2.id= hdr.final_radiologist_id '
		set @strSQL = @strSQL + 'left outer join study_hdr_dictated_reports dr on dr.study_hdr_id= hdr.id '
		set @strSQL = @strSQL + 'left outer join study_hdr_prelim_reports pr on pr.study_hdr_id= hdr.id '
		set @strSQL = @strSQL + 'left outer join study_hdr_final_reports fr on fr.study_hdr_id= hdr.id '
		set @strSQL = @strSQL + 'inner join sys_study_status_pacs ssp on ssp.status_id= hdr.study_status_pacs '
		set @strSQL = @strSQL + 'left outer join species spe on spe.id= hdr.species_id '
		set @strSQL
			= @strSQL + 'left outer join sys_case_study_status_log slog on slog.study_id= hdr.id and slog.status_id_to=10 '
		set @strSQL = @strSQL + 'where hdr.study_status = 4 and spe.is_active=''Y'' '
		set @strSQL = @strSQL + 'and hdr.study_status_pacs =100 '

		if (@user_role_code = 'AU' or @user_role_code = 'IU')
		begin
			set @strSQL = @strSQL + 'and hdr.final_rpt_released =''Y'' '
		end

		set @strSQL = @strSQL + 'and hdr.deleted = ''N'' '

		/******************************************************  
	 --BEGIN Radiologist View  
	 ******************************************************/
		if (@user_role_code = 'RDL')
		begin
			insert into #tmpModality
			(
				id
			)
			(select id
			 from modality
			 where is_active = 'Y'
				   and id in (
								 select modality_id
								 from radiologist_functional_rights_modality
								 where radiologist_id = @rad_id
							 ))
			order by name

			if
			(
				select count(institution_id)
				from radiologist_functional_rights_exception_institution
				where radiologist_id = @rad_id
			) = 0
			begin
				insert into #tmpInst
				(
					id
				)
				(select id
				 from institutions
				 where is_active = 'Y')
				order by name
			end
			else
			begin
				insert into #tmpInst
				(
					id
				)
				(select id
				 from institutions
				 where is_active = 'Y'
					   and id not in (
										 select institution_id
										 from radiologist_functional_rights_exception_institution
										 where radiologist_id = @rad_id
									 ))
				order by name
			end

			if
			(
				select count(right_code)
				from radiologist_functional_rights_assigned
				where right_code = 'WRKCNSLTCASE'
					  and radiologist_id = @rad_id
			) = 0
			begin
				set @strSQL
					= @strSQL
					  + 'and hdr.id not in (select id from study_hdr where study_status=4 and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) '
			end
			else
			begin
				set @strSQL
					= @strSQL
					  + 'and hdr.id in (select id from study_hdr where study_status=4 and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) '
			end

			if
			(
				select count(study_type_id)
				from radiologist_functional_rights_exception_study_type
				where radiologist_id = @rad_id
			) > 0
			begin
				set @strSQL
					= @strSQL + 'and hdr.id not in (select distinct hst.study_hdr_id from study_hdr_study_types hst '
				set @strSQL = @strSQL + 'inner join study_hdr sh on sh.id = hst.study_hdr_id '
				set @strSQL
					= @strSQL
					  + 'where sh.study_status=4 and hst.study_type_id in (select study_type_id from radiologist_functional_rights_exception_study_type where radiologist_id = '''
					  + convert(varchar(36), @rad_id) + ''')) '
			end

			--  if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @rad_id)>0  
			--begin  
			-- set @strSQL= @strSQL + 'and (isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@rad_id) + ''' union '  
			-- set @strSQL= @strSQL + 'select prelim_radiologist_id from study_hdr  where prelim_radiologist_id in ('''+ convert(varchar(36),@rad_id) + ''')) '  
			-- set @strSQL= @strSQL + 'or (isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@rad_id) + ''' union '  
			-- set @strSQL= @strSQL + 'select final_radiologist_id from study_hdr  where final_radiologist_id in ('''+ convert(varchar(36),@rad_id) + ''')))) '  
			--end  

			if
			(
				select count(other_radiologist_id)
				from radiologist_functional_rights_other_radiologist
				where radiologist_id = @rad_id
			) > 0
			begin
				set @strSQL
					= @strSQL
					  + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = '''
					  + convert(varchar(36), @rad_id) + ''' unio
	n '
				set @strSQL
					= @strSQL + 'select radiologist_id from study_hdr  where prelim_radiologist_id  ='''
					  + convert(varchar(36), @rad_id)
					  + ''' union select radiologist_id from study_hdr  where radiologist_id  ='''
					  + convert(varchar(36), @rad_id) + ''')) '

			end


			if
			(
				select count(right_code)
				from radiologist_functional_rights_assigned
				where right_code = 'UPDFINALRPT'
					  and radiologist_id = @rad_id
			) = 0
			begin
				set @strSQL
					= @strSQL
					  + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''
					  + convert(varchar(36), @rad_id)
					  + ''') union (select other_radiologist_id from radiologist_functional_rights_other_ra
	diologist where radiologist_id = ''' + convert(varchar(36), @rad_id) + ''')) '
				set @strSQL
					= @strSQL
					  + 'and isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''
					  + convert(varchar(36), @rad_id)
					  + ''') union (select other_radiologist_id from radiologist_functional_rights_oth
	er_radiologist where radiologist_id = ''' + convert(varchar(36), @rad_id) + '''))) '
			end
			else
			begin
				set @strSQL
					= @strSQL
					  + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''
					  + convert(varchar(36), @rad_id)
					  + ''') union (select other_radiologist_id from radiologist_functional_rights_other_ra
	diologist where radiologist_id = ''' + convert(varchar(36), @rad_id) + ''')) '
				set @strSQL
					= @strSQL
					  + 'or isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''
					  + convert(varchar(36), @rad_id)
					  + ''') union (select other_radiologist_id from radiologist_functional_rights_othe
	r_radiologist where radiologist_id = ''' + convert(varchar(36), @rad_id) + '''))) '
			end
		end
		/******************************************************  
	 --END Radiologist View  
	 ******************************************************/

		if (isnull(@patient_name, '') <> '')
		begin
			set @strSQL
				= @strSQL
				  + ' and upper(isnull(hdr.patient_fname,'''') + + '' '' +  upper(isnull(hdr.patient_lname,''''))) like ''%'
				  + upper(@patient_name) + '%'' '
		end
		if (isnull(@modality_id, 0) <> 0)
		begin
			set @strSQL = @strSQL + ' and hdr.modality_id = ' + convert(varchar, @modality_id)
		end
		if (isnull(@category_id, 0) <> 0)
		begin
			set @strSQL = @strSQL + ' and hdr.category_id = ' + convert(varchar, @category_id)
		end
		if (isnull(@status_id, -1) <> -1)
		begin
			set @strSQL = @strSQL + ' and hdr.study_status_pacs = ' + convert(varchar, @status_id)
		end
		if (isnull(@institution_id, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL = @strSQL + ' and hdr.institution_id = ''' + convert(varchar(36), @institution_id) + ''' '
		end
		else
		begin
			set @strSQL = @strSQL + ' and hdr.institution_id in (select id from #tmpInst) '
		end
		if (isnull(@physician_id, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL = @strSQL + ' and hdr.physician_id = ''' + convert(varchar(36), @physician_id) + ''' '
		end
		if (isnull(@consider_received_date, 'N') = 'Y')
		begin
			set @strSQL
				= @strSQL + 'and hdr.synched_on between ''' + convert(varchar(11), @received_date_from, 106)
				  + ' 00:00:00'' and ''' + convert(varchar(11), @received_date_till, 106) + ' 23:59:59'' '
		end
		if (isnull(@approved_by, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL = @strSQL + ' and upper(hdr.final_radiologist_id) =''' + convert(varchar(36), @approved_by) + ''' '
		end
		if (isnull(@radiologist_id, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL = @strSQL + ' and upper(hdr.radiologist_id) =''' + convert(varchar(36), @radiologist_id) + ''' '
		end
		if (isnull(@abnormal_report_reason_id, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL
				= @strSQL + ' and hdr.id in (select study_hdr_id from study_hdr_final_reports where rating_reason_id ='''
				  + convert(varchar(36), @abnormal_report_reason_id) + ''') '
		end
		if (isnull(@rpt_rel_pending, 'X') = 'Y')
		begin
			set @strSQL = @strSQL + 'and hdr.final_rpt_released = ''N'''
		end

		set @strSQL = @strSQL + ')'

		--print @strSQL  
		exec (@strSQL)
	End
    /******************************************************  
 --END study_hdr  
 ******************************************************/

    /******************************************************  
 --BEGIN study_hdr_archive  
 ******************************************************/
    set @strSQL
        = 'insert into #tmp(id,study_uid,received_date,patient_name,modality_name,category_name,priority_desc,institution_name,physician_name,status_last_updated_on,radiologist_pacs,date_dictated,final_radiologist,'
    set @strSQL
        = @strSQL
          + 'study_status_pacs,status_desc,PACIMGVWRURL,custom_report,accession_no,patient_id,received_via_dicom_router,rating,rating_reason,final_rpt_released,final_rpt_released_by,is_archive,species_name,submitted_date,image_object_count)'
    set @strSQL = @strSQL + '(select hdr.id,hdr.study_uid,received_date=hdr.synched_on,'
    set @strSQL
        = @strSQL
          + 'case when hdr.patient_name like ''%[^a-zA-Z0-9.&( )_-]%'' then '''' else dbo.InitCap(isnull(hdr.patient_fname,'''') + '' '' +  isnull(hdr.patient_lname,'''')) end patient_name,'
    set @strSQL
        = @strSQL
          + 'modality_name=dbo.initcap(isnull(m.name,'''')),category_name=dbo.initcap(isnull(c.name,'''')),priority_desc=isnull(p.priority_desc,''''),'

    if (@user_role_code = 'RDL' and @VWINSTINFOCount = 0)
    begin
        set @strSQL = @strSQL + 'institution_name= ins.code,physician_name = isnull(ph.code,''''),'
    end
    else
    begin
        set @strSQL
            = @strSQL + 'institution_name= dbo.initcap(ins.name),physician_name = dbo.initcap(isnull(ph.name,'''')),'
    end

    set @strSQL = @strSQL + 'hdr.status_last_updated_on,'
    set @strSQL = @strSQL + 'radiologist_pacs=isnull(r1.name,''None''),'
    set @strSQL = @strSQL + 'date_dictated=isnull(dr.date_created,isnull(pr.date_created,fr.date_created)),'
    set @strSQL = @strSQL + 'final_radiologist=isnull(r2.name,''None''),'
    set @strSQL = @strSQL + 'hdr.study_status_pacs,status_desc = ssp.status_desc + ''(Archived)'','
    set @strSQL = @strSQL + 'PACIMGVWRURL=''' + @PACIMGVWRURL + ''', '
    set @strSQL = @strSQL + 'custom_report=isnull(ins.custom_report,''N''), '
    set @strSQL = @strSQL + 'accession_no=isnull(hdr.accession_no,''''), '
    set @strSQL = @strSQL + 'patient_id=isnull(hdr.patient_id,''''), '
    set @strSQL = @strSQL + 'hdr.received_via_dicom_router,'
    set @strSQL = @strSQL + 'case '
    set @strSQL = @strSQL + 'when isnull(dr.rating,''N'') =''A'' then ''A'' '
    set @strSQL = @strSQL + 'when isnull(pr.rating,''N'') =''A'' then ''A'' else ''N'' end rating, '
    set @strSQL = @strSQL + 'case '
    set @strSQL
        = @strSQL
          + 'when isnull(dr.rating,''N'') =''A'' then isnull((select reason from abnormal_rpt_reasons where id=fr.rating_reason_id),'''') '
    set @strSQL
        = @strSQL
          + 'when isnull(pr.rating,''N'') =''A'' then isnull((select reason from abnormal_rpt_reasons where id=fr.rating_reason_id),'''') else ''NORMAL'' end rating_reason, '
    set @strSQL = @strSQL + 'hdr.final_rpt_released,hdr.final_rpt_released_by, '
    set @strSQL
        = @strSQL
          + 'is_archive=''Y'',spe.name species_name,isnull(slog.date_updated,hdr.synched_on) submitted_date,convert(nvarchar(30),hdr.img_count)+''/''+convert(nvarchar(30),hdr.object_count) image_object_count '
    set @strSQL = @strSQL + 'from ' + @db_name + '..study_hdr_archive hdr '
    set @strSQL = @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
    set @strSQL = @strSQL + 'left outer join sys_study_category c on c.id= hdr.category_id '
    set @strSQL = @strSQL + 'left outer join sys_priority p on p.priority_id= hdr.priority_id '
    set @strSQL = @strSQL + 'inner join sys_study_status_pacs ssp on ssp.status_id= hdr.study_status_pacs '
    set @strSQL = @strSQL + 'left outer join institutions ins on ins.id= hdr.institution_id '
    set @strSQL = @strSQL + 'left outer join physicians ph on ph.id= hdr.physician_id '
    set @strSQL = @strSQL + 'left outer join radiologists r1 on r1.id= hdr.radiologist_id '
    set @strSQL = @strSQL + 'left outer join radiologists r2 on r2.id= hdr.final_radiologist_id '
    set @strSQL = @strSQL + 'left outer join ' + @db_name + '..study_hdr_dictated_reports_archive dr on dr.study_hdr_id= hdr.id '
    set @strSQL = @strSQL + 'left outer join ' + @db_name + '..study_hdr_prelim_reports_archive pr on pr.study_hdr_id= hdr.id '
    set @strSQL = @strSQL + 'left outer join ' + @db_name + '..study_hdr_final_reports_archive fr on fr.study_hdr_id= hdr.id '
    set @strSQL = @strSQL + 'left outer join species spe on spe.id= hdr.species_id '
    set @strSQL
        = @strSQL + 'left outer join sys_case_study_status_log slog on slog.study_id= hdr.id and slog.status_id_to=10 '
    set @strSQL = @strSQL + 'where hdr.deleted = ''N'' and spe.is_active=''Y'' '


    /******************************************************  
 --BEGIN Radiologist View  
 ******************************************************/
    if (@user_role_code = 'RDL')
    begin
        set @strSQL = @strSQL + 'and hdr.study_status_pacs = 100 '


        insert into #tmpModality
        (
            id
        )
        (select id
         from modality
         where is_active = 'Y'
               and id in (
                             select modality_id
                             from radiologist_functional_rights_modality
                             where radiologist_id = @rad_id
                         ))
        order by name

        if
        (
            select count(institution_id)
            from radiologist_functional_rights_exception_institution
            where radiologist_id = @rad_id
        ) = 0
        begin
            insert into #tmpInst
            (
                id
            )
            (select id
             from institutions
             where is_active = 'Y')
            order by name
        end
        else
        begin
            insert into #tmpInst
            (
                id
            )
            (select id
             from institutions
             where is_active = 'Y'
                   and id not in (
                                     select institution_id
                                     from radiologist_functional_rights_exception_institution
                                     where radiologist_id = @rad_id
                                 ))
            order by name
        end

        if
        (
            select count(right_code)
            from radiologist_functional_rights_assigned
            where right_code = 'WRKCNSLTCASE'
                  and radiologist_id = @rad_id
        ) = 0
        begin
            set @strSQL
                = @strSQL
                  + 'and hdr.id not in (select id from study_hdr_archive where study_status in (4,5) and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) '
        end
        else
        begin
            set @strSQL
                = @strSQL
                  + 'and hdr.id in (select id from study_hdr_archive where study_status in (4,5) and charindex(''CONSULT'',upper(isnull(service_codes,''''))) > 0) '
        end

        if
        (
            select count(study_type_id)
            from radiologist_functional_rights_exception_study_type
            where radiologist_id = @rad_id
        ) > 0
        begin
            set @strSQL
                = @strSQL + 'and hdr.id not in (select distinct hst.study_hdr_id from study_hdr_study_types hst '
            set @strSQL = @strSQL + 'inner join study_hdr sh on sh.id = hst.study_hdr_id '
            set @strSQL
                = @strSQL
                  + 'where sh.study_status=4 and hst.study_type_id in (select study_type_id from radiologist_functional_rights_exception_study_type where radiologist_id = '''
                  + convert(varchar(36), @rad_id) + ''')) '
        end

        --  if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @rad_id)>0  
        --begin  
        -- set @strSQL= @strSQL + 'and (isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@rad_id) +''' union '  
        -- set @strSQL= @strSQL + 'select prelim_radiologist_id from study_hdr  where prelim_radiologist_id in ('''+ convert(varchar(36),@rad_id) + ''')) '  
        -- set @strSQL= @strSQL + 'or (isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in (select other_radiologist_id from radiologist_functional_rights_other_radiologist where radiologist_id = ''' + convert(varchar(36),@rad_id) + ''' union '  
        -- set @strSQL= @strSQL + 'select final_radiologist_id from study_hdr  where final_radiologist_id in ('''+ convert(varchar(36),@rad_id) + ''')))) '  
        --end  

        set @strSQL
            = @strSQL
              + 'and (isnull(hdr.radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''
              + convert(varchar(36), @rad_id)
              + ''') union (select other_radiologist_id from radiologist_functional_rights_other_radi
ologist where radiologist_id = ''' + convert(varchar(36), @rad_id) + ''')) '
        set @strSQL
            = @strSQL
              + 'and isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000'') in ((select other_radiologist_id ='''
              + convert(varchar(36), @rad_id)
              + ''') union (select other_radiologist_id from radiologist_functional_rig
hts_other_radiologist where radiologist_id = ''' + convert(varchar(36), @rad_id) + '''))) '
    end
    /******************************************************  
 --END Radiologist View  
 ******************************************************/

    if (isnull(@patient_name, '') <> '')
    begin
        set @strSQL
            = @strSQL
              + ' and ((upper(isnull(hdr.patient_fname,'''') + '' '' +  upper(isnull(hdr.patient_lname,''''))) like ''%'
              + upper(@patient_name) + '%'') '
        set @strSQL
            = @strSQL
              + ' or (upper(isnull(hdr.patient_lname,'''') + '' '' +  upper(isnull(hdr.patient_fname,''''))) like ''%'
              + upper(@patient_name) + '%'')) '
    end
    if (isnull(@modality_id, 0) <> 0)
    begin
        set @strSQL = @strSQL + ' and hdr.modality_id = ' + convert(varchar, @modality_id)
    end
    if (isnull(@category_id, 0) <> 0)
    begin
        set @strSQL = @strSQL + ' and hdr.category_id = ' + convert(varchar, @category_id)
    end
    if (isnull(@status_id, -1) <> -1 and isnull(@status_id, -1) <> 11)
    begin
        set @strSQL = @strSQL + ' and hdr.study_status_pacs = ' + convert(varchar, @status_id)
    end
    if (isnull(@institution_id, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
    begin
        set @strSQL = @strSQL + ' and hdr.institution_id = ''' + convert(varchar(36), @institution_id) + ''' '
    end
    else
    begin
        set @strSQL = @strSQL + ' and hdr.institution_id in (select id from #tmpInst) '
    end
    if (isnull(@physician_id, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
    begin
        set @strSQL = @strSQL + ' and hdr.physician_id = ''' + convert(varchar(36), @physician_id) + ''' '
    end
    if (isnull(@consider_received_date, 'N') = 'Y')
    begin
        set @strSQL
            = @strSQL + 'and hdr.synched_on between ''' + convert(varchar(11), @received_date_from, 106) + ' 00:00:00'
              + ''' and ''' + convert(varchar(11), @received_date_till, 106) + ' 23:59:59' + ''' '
    end
    if (isnull(@approved_by, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
    begin
        set @strSQL = @strSQL + ' and upper(hdr.final_radiologist_id) =''' + convert(varchar(36), @approved_by) + ''' '
    end
    if (isnull(@radiologist_id, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
    begin
        set @strSQL = @strSQL + ' and upper(hdr.radiologist_id) =''' + convert(varchar(36), @radiologist_id) + ''' '
    end
    if (isnull(@abnormal_report_reason_id, '00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
    begin
        set @strSQL
            = @strSQL
              + ' and hdr.id in (select study_hdr_id from study_hdr_final_reports_archive where rating_reason_id ='''
              + convert(varchar(36), @abnormal_report_reason_id) + ''')'
    end

    set @strSQL = @strSQL + ')'

    --print @strSQL  
    exec (@strSQL)

    /******************************************************  
 --END study_hdr_archive  
 ******************************************************/

    select *
    from #tmp
    order by is_archive,
             status_last_updated_on desc,
             study_uid,
             patient_name,
             modality_name

    drop table #tmpInst
    drop table #tmp
    set nocount off
end

GO
