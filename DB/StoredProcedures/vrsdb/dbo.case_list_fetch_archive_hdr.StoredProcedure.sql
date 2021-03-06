USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_archive_hdr]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_archive_hdr]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_archive_hdr]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_archive_hdr : fetch case list header
** Created By   : Pavel Guha
** Created On   : 18/09/2020
*******************************************************/
--exec case_list_fetch_archive_hdr '70e5f2c4-343c-4aa5-b6d2-812af6f802e7',76,'11111111-1111-1111-1111-111111111111','6f7b2429-c077-4f4e-8ab0-f07845f4e147','',0
CREATE procedure [dbo].[case_list_fetch_archive_hdr]
    @id uniqueidentifier,	
    @menu_id int,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 declare @species_id int,
			 @institution_id uniqueidentifier,
			 @institution_consult_applicable nchar(1),
	         @user_role_id int,
	         @user_role_code nvarchar(10),
			 @radiologist_id uniqueidentifier,
			 @curr_status_id int,
			 @activity_text nvarchar(max),
			 @menu_text nvarchar(100),
			 @study_uid nvarchar(100),
			 @strSQL varchar(max),
	         @db_name nvarchar(50)

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

    if(@user_role_code = 'RDL')
		begin
			select @radiologist_id = id from radiologists where login_user_id = @user_id
		end

	--controls
	 select pacs_url = (select data_type_string from general_settings where control_code='PACLOGINURL'),
		    image_viewer_url = (select data_type_string from general_settings where control_code='PACIMGVWRURL'),
		    pacs_img_count_url = (select data_type_string from general_settings where control_code='PACIMGCNTURL'),
		    pacs_study_del_url = (select data_type_string from general_settings where control_code='PACSTUDYDELURL'),
			WS8SRVIP     = (select data_type_string from general_settings where control_code='WS8SRVIP'),
	        WS8CLTIP     = (select data_type_string from general_settings where control_code='WS8CLTIP'),
		    WS8SRVUID    = (select data_type_string from general_settings where control_code='WS8SRVUID'),
		    WS8SRVPWD    = (select data_type_string from general_settings where control_code='WS8SRVPWD'),
		    APIVER       = (select data_type_string from general_settings where control_code='APIVER'),
		    WS8SYVWRURL  = (select data_type_string from general_settings where control_code='WS8SYVWRURL'),
		    WS8IMGVWRURL = (select data_type_string from general_settings where control_code='WS8IMGVWRURL'),
		    FTPABSPATH   = (select data_type_string from general_settings where control_code='FTPABSPATH'),
		    DCMMODIFYEXEPATH = (select data_type_string from general_settings where control_code='DCMMODIFYEXEPATH'),
		    PACSARCHIVEFLDR = (select data_type_string from general_settings where control_code='PACSARCHIVEFLDR'),
		    PACSARCHALTFLDR = (select data_type_string from general_settings where control_code='PACSARCHALTFLDR'),
		    VRSAPPLINK      = (select data_type_string from general_settings where control_code='VRSAPPLINK')

	--masters
	select id,name from species where is_active='Y' order by name
	select id,name from modality where is_active='Y' order by name
	select priority_id,priority_desc from sys_priority where is_active='Y'
	select id,name from sys_study_category order by name

	--institutions
	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code = 'TRS')
		begin
		     select id,name from institutions where is_active='Y'
			 order by name
		end
	else if(@user_role_code = 'RDL')
		begin
			if(select count(institution_id) from radiologist_functional_rights_exception_institution where radiologist_id = @radiologist_id)=0
				begin
					select id,name from institutions 
					where is_active='Y' 
					and id not in (select institution_id from radiologist_functional_rights_exception_institution where radiologist_id = @radiologist_id)
					order by name
				end
			else
				begin
					select id,name from institutions where is_active='Y' order by name
					
				end
		end
	else if(@user_role_code = 'IU')
		begin
			select id,name
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_user_link
					   where user_id = @user_id)
			order by name
		end
	else if(@user_role_code = 'SALES')
		begin
			select id,name
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_salesperson_link
					   where salesperson_user_id = @user_id)
			order by name
		end
	else if(@user_role_code = 'AU')
		begin
			 select id=bail.institution_id,i.name
			 from billing_account_institution_link bail
			 inner join institutions i on i.id = bail.institution_id
			 inner join billing_account ba on ba.id = bail.billing_account_id
			 where i.is_active='Y'
			 and ba.login_user_id = @user_id
			 order by i.name
		end
	if(@user_role_code='RDL')
		begin
			select right_code from radiologist_functional_rights_assigned where radiologist_id=@radiologist_id order by right_code
		end
	else
		begin
			select right_code from sys_radiologist_functional_rights order by right_code
		end

	--PACS Credentials
	select pacs_user_id,pacs_password from  users where id=@user_id
	--Report Disclaimer Reasons
	select id,type from report_disclaimer_reasons where is_active='Y' order by type		


	if(select count(id) from study_hdr_archive where id = @id)>0
		begin
			    --details
				select hdr.study_uid,hdr.study_date,
					   study_desc=isnull(hdr.study_desc,''),
					   accession_no_pacs = isnull(hdr.accession_no_pacs,''),accession_no= isnull(hdr.accession_no,''),
					   patient_id_pacs = isnull(hdr.patient_id_pacs,''),patient_id = isnull(hdr.patient_id,''),
					   patient_name_pacs = isnull(hdr.patient_name_pacs,''),patient_name = isnull(hdr.patient_name,''),patient_fname = isnull(hdr.patient_fname,''),patient_lname = isnull(hdr.patient_lname,''),
					   patient_country_id = isnull(hdr.patient_country_id,isnull(ins.country_id,0)), patient_country_name = isnull(c1.name,isnull(c2.name,'')), 
					   patient_state_id = isnull(hdr.patient_state_id,isnull(ins.state_id,0)), patient_state_name = isnull(s1.name,isnull(s2.name,'')), 
					   case when isnull(hdr.patient_city,'')='' then isnull(ins.city,'') else hdr.patient_city end patient_city,
					   patient_sex_pacs = isnull(hdr.patient_sex_pacs,''),patient_sex = isnull(hdr.patient_sex,''),
					   sex_neutered_pacs = isnull(hdr.patient_sex_neutered_pacs,''),sex_neutered_accepted =isnull(hdr.patient_sex_neutered,''),
					   patient_weight_pacs= isnull(hdr.patient_weight_pacs,0),
					   case
							when isnull(wt_uom,'') = 'lbs' then isnull(hdr.patient_weight,0)
							when isnull(wt_uom,'') = 'kgs' then isnull(hdr.patient_weight_kgs,0)
							when isnull(wt_uom,'') = '' then isnull(hdr.patient_weight_pacs,0)
					   end patient_weight,
					   wt_uom = isnull(hdr.wt_uom,''),
					   patient_dob_pacs = isnull(hdr.patient_dob_pacs,'01jan1900'),patient_dob_accepted = isnull(hdr.patient_dob_accepted,'01jan1900'),
					   patient_age_pacs = isnull(hdr.patient_age_pacs,0),patient_age_accepted = isnull(hdr.patient_age_accepted,0),
					   owner_name_pacs = isnull(hdr.owner_name_pacs,''),owner_first_name = isnull(hdr.owner_first_name,''), owner_last_name = isnull(hdr.owner_last_name,''),
					   species_pacs = isnull(hdr.species_pacs,''),species_id = isnull(hdr.species_id,0),species_name=isnull(sp.name,''),
					   breed_pacs = isnull(hdr.breed_pacs,''), breed_id = isnull(hdr.breed_id,'00000000-0000-0000-0000-000000000000'),breed_name = isnull(br.name,''),
					   modality_pacs  = isnull(hdr.modality_pacs,''),  modality_id  = isnull(hdr.modality_id,0),modality_name=isnull(m.name,''),
					   body_part_pacs = isnull(hdr.body_part_pacs,''),body_part_id = isnull(hdr.body_part_id,0),body_part_name = isnull(bp.name,''),
					   institution_name_pacs = isnull(hdr.institution_name_pacs,''),
					   institution_id = isnull(hdr.institution_id,'00000000-0000-0000-0000-000000000000'),
					   institution_name= isnull(ins.name,''),
					   institution_code= isnull(ins.code,''),
					   referring_physician_pacs = isnull(hdr.referring_physician_pacs,''),physician_id = isnull(hdr.physician_id,'00000000-0000-0000-0000-000000000000'),physician_name = isnull(ph.name,''),
					   reason_pacs= isnull(hdr.reason_pacs,''),reason_accepted= isnull(hdr.reason_accepted,''),
					   priority_id = isnull(hdr.priority_id,0),priority_desc = isnull(p.priority_desc,''),
					   object_count = isnull(hdr.object_count,0),
					   img_count_pacs= isnull(hdr.img_count_pacs,0),img_count= isnull(hdr.img_count,0),img_count_accepted=isnull(hdr.img_count_accepted,'N'),
					   track_by = isnull(m.track_by,'I'),
					   invoice_by=isnull(m.invoice_by,''),
					   received_via_dicom_router = isnull(hdr.received_via_dicom_router,'N'),
					   physician_note = isnull(hdr.physician_note,''),
					   consult_applied = isnull(hdr.consult_applied,'N'),
					   category_id = isnull(hdr.category_id,0),category_name = isnull(sc.name,''),
					   service_codes = isnull(hdr.service_codes,''),
					   hdr.study_status_pacs, ssp.status_desc,
					   case 
							when isnull(hdr.prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') ='00000000-0000-0000-0000-000000000000' then hdr.radiologist_id else hdr.prelim_radiologist_id
					   end prelim_radiologist_id,
					   preliminary_radiologist = isnull(r1.name,isnull(r3.name,'None')),
					   final_radiologist_id = isnull(hdr.final_radiologist_id,'00000000-0000-0000-0000-000000000000'),
					   final_radiologist       = isnull(r2.name,'None'),
					   dict_tanscriptionist_id = isnull(hdr.dict_tanscriptionist_id,'00000000-0000-0000-0000-000000000000'),
					   track_by = isnull(m.track_by,'I'),
					   mark_to_teach = isnull(hdr.mark_to_teach,'N'),
					   sync_mode     = isnull(hdr.sync_mode,'PACS'),
					   institution_consult_applicable = isnull
					   (ins.consult_applicable,'N'),
					   custom_report = isnull(ins.custom_report,'N')
				from study_hdr_archive hdr
				left outer join sys_country c1 on c1.id = hdr.patient_country_id
				left outer join sys_states s1 on s1.id = hdr.patient_state_id
				left outer join modality m on m.id= hdr.modality_id
				left outer join body_part bp on bp.id= hdr.body_part_id
				left outer join species sp on sp.id= hdr.species_id
				left outer join breed br on br.id= hdr.breed_id
				left outer join institutions ins on ins.id= hdr.institution_id
				left outer join sys_country c2 on c2.id = ins.country_id
				left outer join sys_states s2 on s2.id = ins.state_id
				left outer join physicians ph on ph.id= hdr.physician_id
				left outer join sys_priority p on p.priority_id = hdr.priority_id
				left outer join sys_study_category sc on sc.id = hdr.category_id
				inner join sys_study_status_pacs ssp on ssp.status_id= hdr.study_status_pacs
				left outer join radiologists r1 on r1.id= hdr.prelim_radiologist_id
				left outer join radiologists r2 on r2.id= hdr.final_radiologist_id
				left outer join radiologists r3 on r3.id= hdr.radiologist_id
				where hdr.id=@id

				select @study_uid  = study_uid
				from study_hdr_archive 
				where id=@id

				--Physicians
				select id,name= rtrim(ltrim(isnull(name,''))) 
				from physicians 
				where is_active='Y' 
				and id in (select physician_id from institution_physician_link where institution_id=(select institution_id from study_hdr_archive where id=@id)) 
				order by name

				select id,name from breed where is_active='Y' and species_id=(select species_id from study_hdr_archive where id=@id) order by name

				--Report Texts
				select dr.report_id,dr.report_text,dr.report_text_html,dr.trans_report_text,
					   dr.trans_report_text_html,translate_report_text= isnull(dr.translate_report_text,''),translate_report_text_html=isnull(dr.translate_report_text_html,''),rating=isnull(dr.rating,''),
					   disclaimer_reason_id = isnull(dr.disclaimer_reason_id,0),
					   disclaimer_reason = isnull(rdr.type,''),
					   disclaimer_desc = isnull(dr.disclaimer_text,'') 
				from study_hdr_dictated_reports_archive dr
				left outer join report_disclaimer_reasons rdr on rdr.id = dr.disclaimer_reason_id
				where dr.study_hdr_id=@id

				select pr.report_id,pr.report_text,pr.report_text_html,rating=isnull(pr.rating,'') ,
					   disclaimer_reason_id = isnull(pr.disclaimer_reason_id,0),
					   disclaimer_reason = isnull(rdr.type,'') ,
					   disclaimer_desc = isnull(pr.disclaimer_text,'') 
				from study_hdr_prelim_reports_archive pr
				left outer join report_disclaimer_reasons rdr on rdr.id = pr.disclaimer_reason_id
				where pr.study_hdr_id=@id

				select fr.report_id,fr.report_text,fr.report_text_html,
					   disclaimer_reason_id = isnull(fr.disclaimer_reason_id,0),
					   disclaimer_reason = isnull(rdr.type,''),
					   disclaimer_desc = isnull(fr.disclaimer_text,'') 
				from study_hdr_final_reports_archive fr
				left outer join report_disclaimer_reasons rdr on rdr.id = fr.disclaimer_reason_id
				where fr.study_hdr_id=@id

				--Modality wise study type
				select st.study_type_id,mst.name 
				from study_hdr_study_types_archive st
				inner join modality_study_types mst on mst.id=st.study_type_id
				where st.study_hdr_id = @id
		end
	else
		begin
			set @db_name=''
			exec common_get_study_database
				@id      = @id,
				@db_name = @db_name output


			   --details
				set @strSQL ='select hdr.study_uid,hdr.study_date,'
				set @strSQL = @strSQL + 'study_desc=isnull(hdr.study_desc,''''),'
				set @strSQL = @strSQL + 'accession_no_pacs = isnull(hdr.accession_no_pacs,''''),accession_no= isnull(hdr.accession_no,''''),'	   
				set @strSQL = @strSQL + 'patient_id_pacs = isnull(hdr.patient_id_pacs,''''),patient_id = isnull(hdr.patient_id,''''),'
				set @strSQL = @strSQL + 'patient_name_pacs = isnull(hdr.patient_name_pacs,''''),patient_name = isnull(hdr.patient_name,''''),patient_fname = isnull(hdr.patient_fname,''''),patient_lname = isnull(hdr.patient_lname,''''),'
				set @strSQL = @strSQL + 'patient_country_id = isnull(hdr.patient_country_id,isnull(ins.country_id,0)), patient_country_name = isnull(c1.name,isnull(c2.name,'''')),' 
				set @strSQL = @strSQL + 'patient_state_id = isnull(hdr.patient_state_id,isnull(ins.state_id,0)), patient_state_name = isnull(s1.name,isnull(s2.name,'''')),'
				set @strSQL = @strSQL + 'case when isnull(hdr.patient_city,'''')='''' then isnull(ins.city,'''') else hdr.patient_city end patient_city,'
				set @strSQL = @strSQL + 'patient_sex_pacs = isnull(hdr.patient_sex_pacs,''''),patient_sex = isnull(hdr.patient_sex,''''),'
				set @strSQL = @strSQL + 'sex_neutered_pacs = isnull(hdr.patient_sex_neutered_pacs,''''),sex_neutered_accepted =isnull(hdr.patient_sex_neutered,''''),'
				set @strSQL = @strSQL + 'patient_weight_pacs= isnull(hdr.patient_weight_pacs,0),'
				set @strSQL = @strSQL + 'case '
				set @strSQL = @strSQL + 'when isnull(wt_uom,'''') = ''lbs'' then isnull(hdr.patient_weight,0)'
				set @strSQL = @strSQL + 'when isnull(wt_uom,'''') = ''kgs'' then isnull(hdr.patient_weight_kgs,0)'
				set @strSQL = @strSQL + 'when isnull(wt_uom,'''') = '''' then isnull(hdr.patient_weight_pacs,0) '
				set @strSQL = @strSQL + 'end patient_weight,'
				set @strSQL = @strSQL + 'wt_uom = isnull(hdr.wt_uom,''''),'
				set @strSQL = @strSQL + 'patient_dob_pacs = isnull(hdr.patient_dob_pacs,''01jan1900''),patient_dob_accepted = isnull(hdr.patient_dob_accepted,''01jan1900''),'
				set @strSQL = @strSQL + 'patient_age_pacs = isnull(hdr.patient_age_pacs,0),patient_age_accepted = isnull(hdr.patient_age_accepted,0),'
				set @strSQL = @strSQL + 'owner_name_pacs = isnull(hdr.owner_name_pacs,''''),owner_first_name = isnull(hdr.owner_first_name,''''), owner_last_name = isnull(hdr.owner_last_name,''''),'
				set @strSQL = @strSQL + 'species_pacs = isnull(hdr.species_pacs,''''),species_id = isnull(hdr.species_id,0),species_name=isnull(sp.name,''''),'
				set @strSQL = @strSQL + 'breed_pacs = isnull(hdr.breed_pacs,''''), breed_id = isnull(hdr.breed_id,''00000000-0000-0000-0000-000000000000''),breed_name = isnull(br.name,''''),'
				set @strSQL = @strSQL + 'modality_pacs  = isnull(hdr.modality_pacs,''''),  modality_id  = isnull(hdr.modality_id,0),modality_name=isnull(m.name,''''),'
				set @strSQL = @strSQL + 'institution_name_pacs = isnull(hdr.institution_name_pacs,''''),'
				set @strSQL = @strSQL + 'institution_id = isnull(hdr.institution_id,''00000000-0000-0000-0000-000000000000''),'
				set @strSQL = @strSQL + 'institution_name= isnull(ins.name,''''),'
				set @strSQL = @strSQL + 'institution_code= isnull(ins.code,''''),'
				set @strSQL = @strSQL + 'referring_physician_pacs = isnull(hdr.referring_physician_pacs,''''),physician_id = isnull(hdr.physician_id,''00000000-0000-0000-0000-000000000000''),physician_name = isnull(ph.name,''''),'
				set @strSQL = @strSQL + 'reason_pacs= isnull(hdr.reason_pacs,''''),reason_accepted= isnull(hdr.reason_accepted,''''),'
				set @strSQL = @strSQL + 'priority_id = isnull(hdr.priority_id,0),priority_desc = isnull(p.priority_desc,''''),'
				set @strSQL = @strSQL + 'object_count = isnull(hdr.object_count,0),'
				set @strSQL = @strSQL + 'img_count_pacs= isnull(hdr.img_count_pacs,0),img_count= isnull(hdr.img_count,0),img_count_accepted=isnull(hdr.img_count_accepted,''N''),'
				set @strSQL = @strSQL + 'track_by = isnull(m.track_by,''I''),'
				set @strSQL = @strSQL + 'invoice_by=isnull(m.invoice_by,''''),'
				set @strSQL = @strSQL + 'physician_note = isnull(hdr.physician_note,''''),'
				set @strSQL = @strSQL + 'consult_applied = isnull(hdr.consult_applied,''N''),'
				set @strSQL = @strSQL + 'category_id = isnull(hdr.category_id,0),category_name = isnull(sc.name,''''),'
				set @strSQL = @strSQL + 'service_codes = isnull(hdr.service_codes,''''),'
				set @strSQL = @strSQL + 'hdr.study_status_pacs, ssp.status_desc,'
				set @strSQL = @strSQL + 'case '
				set @strSQL = @strSQL + 'when isnull(hdr.prelim_radiologist_id,''00000000-0000-0000-0000-000000000000'') =''00000000-0000-0000-0000-000000000000'' then hdr.radiologist_id else hdr.prelim_radiologist_id '
				set @strSQL = @strSQL + 'end prelim_radiologist_id,'
				set @strSQL = @strSQL + 'preliminary_radiologist = isnull(r1.name,isnull(r3.name,''None'')),'
				set @strSQL = @strSQL + 'final_radiologist_id = isnull(hdr.final_radiologist_id,''00000000-0000-0000-0000-000000000000''),'
				set @strSQL = @strSQL + 'final_radiologist= isnull(r2.name,''None''),'
				set @strSQL = @strSQL + 'dict_tanscriptionist_id = isnull(hdr.dict_tanscriptionist_id,''00000000-0000-0000-0000-000000000000''),'
				set @strSQL = @strSQL + 'mark_to_teach = isnull(hdr.mark_to_teach,''N''),'
				set @strSQL = @strSQL + 'sync_mode= isnull(hdr.sync_mode,''PACS''),'
				set @strSQL = @strSQL + 'institution_consult_applicable = isnull(ins.consult_applicable,''N''),'
				set @strSQL = @strSQL + 'custom_report = isnull(ins.custom_report,''N'') '
				set @strSQL = @strSQL + 'from ' + @db_name + '..study_hdr_archive hdr '
				set @strSQL = @strSQL +'left outer join sys_country c1 on c1.id = hdr.patient_country_id '
				set @strSQL = @strSQL +'left outer join sys_states s1 on s1.id = hdr.patient_state_id '
				set @strSQL = @strSQL +'left outer join modality m on m.id= hdr.modality_id '
				--set @strSQL = @strSQL +'left outer join body_part bp on bp.id= hdr.body_part_id '
				set @strSQL = @strSQL +'left outer join species sp on sp.id= hdr.species_id '
				set @strSQL = @strSQL +'left outer join breed br on br.id= hdr.breed_id '
				set @strSQL = @strSQL +'left outer join institutions ins on ins.id= hdr.institution_id '
				set @strSQL = @strSQL +'left outer join sys_country c2 on c2.id = ins.country_id '
				set @strSQL = @strSQL +'left outer join sys_states s2 on s2.id = ins.state_id '
				set @strSQL = @strSQL +'left outer join physicians ph on ph.id= hdr.physician_id '
				set @strSQL = @strSQL +'left outer join sys_priority p on p.priority_id = hdr.priority_id '
				set @strSQL = @strSQL +'left outer join sys_study_category sc on sc.id = hdr.category_id '
				set @strSQL = @strSQL +'inner join sys_study_status_pacs ssp on ssp.status_id= hdr.study_status_pacs '
				set @strSQL = @strSQL +'left outer join radiologists r1 on r1.id= hdr.prelim_radiologist_id '
				set @strSQL = @strSQL +'left outer join radiologists r2 on r2.id= hdr.final_radiologist_id '
				set @strSQL = @strSQL +'left outer join radiologists r3 on r3.id= hdr.radiologist_id '
				set @strSQL = @strSQL +'where hdr.id=''' + convert(varchar(36),@id) + ''' '

				exec(@strSQL)

				create table #tmpUID(study_uid nvarchar(100))
				set @strSQL ='insert into #tmpUID(study_uid)(select study_uid from ' + @db_name + '..study_hdr_archive where id=''' + convert(varchar(36),@id) + ''')'
				exec(@strSQL)
				select @study_uid = study_uid from #tmpUID
				drop table #tmpUID

				--Physicians
				set @strSQL ='select id,name= rtrim(ltrim(isnull(name,''''))) '
				set @strSQL = @strSQL +'from physicians '
				set @strSQL = @strSQL +'where is_active=''Y'' '
				set @strSQL = @strSQL +'and id in (select physician_id from institution_physician_link where institution_id=(select institution_id from ' + @db_name + '..study_hdr_archive where id=''' + convert(varchar(36),@id) + ''')) ' 
				set @strSQL = @strSQL +'order by name '
				exec(@strSQL)

				set @strSQL ='select id,name from breed where is_active=''Y'' and species_id=(select species_id from ' + @db_name + '..study_hdr_archive where id=''' + convert(varchar(36),@id) + ''') order by name'
				exec(@strSQL)

				--Report Texts
				set @strSQL ='select dr.report_id,dr.report_text,dr.report_text_html,dr.trans_report_text,'
				set @strSQL = @strSQL +'dr.trans_report_text_html,translate_report_text= isnull(dr.translate_report_text,''''),translate_report_text_html=isnull(dr.translate_report_text_html,''''),rating=isnull(dr.rating,''''),'
				set @strSQL = @strSQL +'disclaimer_reason_id = isnull(dr.disclaimer_reason_id,0),'
				set @strSQL = @strSQL +'disclaimer_reason = isnull(rdr.type,''''),'
				set @strSQL = @strSQL +'disclaimer_desc = isnull(dr.disclaimer_text,'''') '
				set @strSQL = @strSQL +'from ' + @db_name + '..study_hdr_dictated_reports_archive dr '
				set @strSQL = @strSQL +'left outer join report_disclaimer_reasons rdr on rdr.id = dr.disclaimer_reason_id '
				set @strSQL = @strSQL +'where dr.study_hdr_id=''' + convert(varchar(36),@id) + ''' '
				exec(@strSQL)

				set @strSQL ='select pr.report_id,pr.report_text,pr.report_text_html,rating=isnull(pr.rating,'''') ,'
				set @strSQL = @strSQL +'disclaimer_reason_id = isnull(pr.disclaimer_reason_id,0),'
				set @strSQL = @strSQL +'disclaimer_reason = isnull(rdr.type,'''') ,'
				set @strSQL = @strSQL +'disclaimer_desc = isnull(pr.disclaimer_text,'''') '
				set @strSQL = @strSQL +'from ' + @db_name + '..study_hdr_prelim_reports_archive pr '
				set @strSQL = @strSQL +'left outer join report_disclaimer_reasons rdr on rdr.id = pr.disclaimer_reason_id '
				set @strSQL = @strSQL +'where pr.study_hdr_id=''' + convert(varchar(36),@id) + ''' '
				exec(@strSQL)

				set @strSQL ='select fr.report_id,fr.report_text,fr.report_text_html,'
				set @strSQL = @strSQL +'disclaimer_reason_id = isnull(fr.disclaimer_reason_id,0),'
				set @strSQL = @strSQL +'disclaimer_reason = isnull(rdr.type,''''),'
				set @strSQL = @strSQL +'disclaimer_desc = isnull(fr.disclaimer_text,'''') '
				set @strSQL = @strSQL +'from ' + @db_name + '..study_hdr_final_reports_archive fr '
				set @strSQL = @strSQL +'left outer join report_disclaimer_reasons rdr on rdr.id = fr.disclaimer_reason_id '
				set @strSQL = @strSQL +'where fr.study_hdr_id=''' + convert(varchar(36),@id) + ''' '
				exec(@strSQL)

				--Modality wise study type
				set @strSQL ='select st.study_type_id,mst.name '
				set @strSQL = @strSQL +'from ' + @db_name + '..study_hdr_study_types_archive st '
				set @strSQL = @strSQL +'inner join modality_study_types mst on mst.id=st.study_type_id '
				set @strSQL = @strSQL +'where st.study_hdr_id=''' + convert(varchar(36),@id) + ''' '
				exec(@strSQL)
		end
	
	if(@id<>'00000000-0000-0000-0000-000000000000')
		begin
			if(select count(record_id) from sys_record_lock_ui where record_id=@id and menu_id=@menu_id)=0
				begin
					exec common_lock_record_ui
						@menu_id       = @menu_id,
						@record_id     = @id,
						@user_id       = @user_id,
						@session_id    = @session_id,
						@error_code    = @error_code output,
						@return_status = @return_status output	
						
					if(@return_status=0)
						begin
							return 0
						end
				end
			
		
				set @activity_text =  'Locked'
				exec common_study_user_activity_trail_save
					@study_hdr_id  = @id,
					@study_uid     = '',
					@menu_id       = @menu_id,
					@activity_text = @activity_text,
					@session_id    = @session_id,
					@activity_by   = @user_id,
					@error_code    = @error_code output,
					@return_status = @return_status output

			   if(@return_status=0)
				begin
					return 0
				end

			select @menu_text= menu_desc from sys_menu where menu_id=@menu_id
			set  @activity_text =  isnull(@menu_text,'')  + '==> Opened & Locked => Study UID ' + @study_uid
			exec common_user_activity_log
					@user_id       = @user_id,
					@activity_text = @activity_text,
					@session_id    = @session_id,
					@menu_id       = @menu_id,
					@error_code    = @error_code output,
					@return_status = @return_status output

			if(@return_status=0)
				begin
					return 0
				end
		end
    else
		begin
			if(select count(record_id) from sys_record_lock_ui where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
			    begin
				  delete from sys_record_lock_ui where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
				  delete from sys_record_lock where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
			    end
		end

	set nocount off
end

GO
