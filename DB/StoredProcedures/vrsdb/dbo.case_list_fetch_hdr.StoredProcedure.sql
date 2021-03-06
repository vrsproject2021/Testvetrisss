USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_hdr]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_hdr]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_hdr]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_hdr : fetch case list header
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
--exec case_list_fetch_hdr '68f5d868-16a5-47c9-a6b0-9cd07a50cd4e',21,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[case_list_fetch_hdr]
    @id uniqueidentifier,	
    @menu_id int,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on
	 set datefirst 1

	 declare @species_id int,
	         @modality_id int,
			 @institution_id uniqueidentifier,
			 @institution_consult_applicable nchar(1),
	         @user_role_id int,
	         @user_role_code nvarchar(10),
			 @radiologist_id uniqueidentifier,
			 @curr_status_id int,
			 @activity_text nvarchar(max),
			 @day_no int,
			 @start_from datetime,
			 @end_at datetime,
			 @curr_date_time datetime,
			 @beyond_operation_time nchar(1),
			 @menu_text nvarchar(100),
			 @study_uid nvarchar(100)

	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	--print @user_role_code

    if(@user_role_code = 'RDL')
		begin
			select @radiologist_id = id from radiologists where login_user_id = @user_id
		end

    set datefirst 1
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

	select @species_id = species_id,
	       @modality_id = modality_id,
		   @institution_id = institution_id,
		   @study_uid  = study_uid
	from study_hdr 
	where id=@id

   select @institution_consult_applicable = consult_applicable 
   from institutions 
   where id = @institution_id

	

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
		   referring_physician_pacs = isnull(hdr.referring_physician_pacs,''),physician_id = isnull(hdr.physician_id,'00000000-0000-0000-0000-000000000000'),physician_code=isnull(ph.code,''),physician_name = isnull(ph.name,''),
		   reason_pacs= isnull(hdr.reason_pacs,''),reason_accepted= isnull(hdr.reason_accepted,''),
		   img_count_pacs= isnull(hdr.img_count_pacs,0),img_count= isnull(hdr.img_count,0),img_count_accepted=isnull(hdr.img_count_accepted,'N'),
		   pacs_url = (select data_type_string from general_settings where control_code='PACLOGINURL'),
		   image_viewer_url = (select data_type_string from general_settings where control_code='PACIMGVWRURL'),
		   pacs_img_count_url = (select data_type_string from general_settings where control_code='PACIMGCNTURL'),
		   pacs_study_del_url = (select data_type_string from general_settings where control_code='PACSTUDYDELURL'),
		   priority_id = isnull(hdr.priority_id,0),priority_desc = isnull(p.priority_desc,''),
		   object_count = isnull(hdr.object_count,0),
		   track_by = isnull(m.track_by,'I'),
		   invoice_by=isnull(m.invoice_by,''),
		   received_via_dicom_router = isnull(hdr.received_via_dicom_router,'N'),
		   physician_note = isnull(hdr.physician_note,''),
		   consult_applied = isnull(hdr.consult_applied,'N'),
		   category_id = isnull(hdr.category_id,0),category_name = isnull(sc.name,''),
		   service_codes = isnull(hdr.service_codes,''),
		   hdr.study_status_pacs, ssp.status_desc,
		   reading_radiologist_id =isnull(hdr.radiologist_id,'00000000-0000-0000-0000-000000000000'),
		   reading_radiologist_name =isnull(r3.name,''),
		   case 
				when isnull(hdr.prelim_radiologist_id,'00000000-0000-0000-0000-000000000000') ='00000000-0000-0000-0000-000000000000' then hdr.radiologist_id else hdr.prelim_radiologist_id
		   end prelim_radiologist_id,
		   preliminary_radiologist = isnull(r1.name,isnull(r3.name,'None')),
		   final_radiologist_id = isnull(hdr.final_radiologist_id,'00000000-0000-0000-0000-000000000000'),
		   final_radiologist       = isnull(r2.name,'None'),
		   dict_tanscriptionist_id = isnull(hdr.dict_tanscriptionist_id,'00000000-0000-0000-0000-000000000000'),
		   mark_to_teach = isnull(hdr.mark_to_teach,'N'),
		   sync_mode     = isnull(hdr.sync_mode,'PACS'),
		   WS8SRVIP      = (select data_type_string from general_settings where control_code='WS8SRVIP'),
	       WS8CLTIP      = (select data_type_string from general_settings where control_code='WS8CLTIP'),
		   WS8SRVUID     = (select data_type_string from general_settings where control_code='WS8SRVUID'),
		   WS8SRVPWD     = (select data_type_string from general_settings where control_code='WS8SRVPWD'),
		   APIVER        = (select data_type_string from general_settings where control_code='APIVER'),
		   WS8SYVWRURL   = (select data_type_string from general_settings where control_code='WS8SYVWRURL'),
		   WS8IMGVWRURL  = (select data_type_string from general_settings where control_code='WS8IMGVWRURL'),
		   FTPABSPATH    = (select data_type_string from general_settings where control_code='FTPABSPATH'),
		   DCMMODIFYEXEPATH = (select data_type_string from general_settings where control_code='DCMMODIFYEXEPATH'),
		   PACSARCHIVEFLDR = (select data_type_string from general_settings where control_code='PACSARCHIVEFLDR'),
		   PACSARCHALTFLDR = (select data_type_string from general_settings where control_code='PACSARCHALTFLDR'),
		   VRSAPPLINK      = (select data_type_string from general_settings where control_code='VRSAPPLINK'),
		   GOOGLETRANSAPILINK = (select data_type_string from general_settings where control_code='GOOGLETRANSAPILINK'),
		   GOOGLETRANSAPIKEY  = (select data_type_string from general_settings where control_code='GOOGLETRANSAPIKEY'),
		   custom_report = isnull(ins.custom_report,'N'),
		   case
				when hdr.study_status_pacs>0 then 0
				                             else (select count(id) 
											       from study_hdr hdr1
												   where isnull(hdr1.patient_name_pacs,'')= isnull(hdr.patient_name_pacs,'')
												   and isnull(hdr1.patient_sex_pacs,'')= isnull(hdr.patient_sex_pacs,'')
												   and hdr1.institution_id             = hdr.institution_id
												   and hdr1.study_status_pacs          = 0
												   and isnull(hdr1.merge_status,'N')   ='N'
												   and hdr1.study_uid <> hdr.study_uid) 
		   end study_to_merge,
		   beyond_operation_time=@beyond_operation_time
	from study_hdr hdr
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
			else if(select count(record_id) from sys_record_lock_ui where record_id=@id and menu_id=@menu_id and user_id<>@user_id)=0
				begin
					set @activity_text =  'Opened'
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
					set  @activity_text =  isnull(@menu_text,'')  + '==> Opened => Study UID ' + @study_uid
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
		end
    else
		begin
			if(select count(record_id) from sys_record_lock_ui where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
			    begin
				  delete from sys_record_lock_ui where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
				  delete from sys_record_lock where user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
			    end
		end

	--species
	select id,name from species where is_active='Y' order by name
	--breed
	select id,name from breed where is_active='Y' and species_id=@species_id order by name
	--modality
	select id,name from modality where is_active='Y' order by name

	--institutions
	if(@user_role_code = 'SUPP' or @user_role_code = 'SYSADMIN' or @user_role_code = 'TRS')
		begin
		     select id,code,name from institutions where is_active='Y'
			 order by name
		end
	else if(@user_role_code = 'RDL')
		begin
			if(select count(institution_id) from radiologist_functional_rights_exception_institution where radiologist_id = @radiologist_id)=0
				begin
					select id,code,name from institutions 
					where is_active='Y' 
					and id not in (select institution_id from radiologist_functional_rights_exception_institution where radiologist_id = @radiologist_id)
					order by name
				end
			else
				begin
					select id,code,name from institutions where is_active='Y' order by name
					
				end
		end
	else if(@user_role_code = 'IU')
		begin
			select id,code,name
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_user_link
					   where user_id = @user_id)
			order by name
		end
	else if(@user_role_code = 'SALES')
		begin
			select id,code,name
			from institutions 
			where is_active='Y'
			and id in (select institution_id
			           from institution_salesperson_link
					   where salesperson_user_id = @user_id)
			order by name
		end
	else if(@user_role_code = 'AU')
		begin
			 select id=bail.institution_id,i.code,i.name
			 from billing_account_institution_link bail
			 inner join institutions i on i.id = bail.institution_id
			 inner join billing_account ba on ba.id = bail.billing_account_id
			 where i.is_active='Y'
			 and ba.login_user_id = @user_id
			 order by i.name
		end

	--select id,name from institutions where is_active='Y' order by name
	--print @institution_id
	--select id,name= rtrim(ltrim(isnull(lname,'') + ' ' + isnull(fname,'') + ' ' + isnull(credentials,''))) from physicians where is_active='Y' and id in (select physician_id from institution_physician_link where institution_id=@institution_id) 
	--order by lname

	--Physicians
	select id,
	       code= isnull(code,''),
	       name=  rtrim(ltrim(isnull(fname,'') + ' ' + isnull(lname,'') + ' ' +  isnull(credentials,''))) 
	from physicians 
	where is_active='Y' 
	and id in (select physician_id from institution_physician_link where institution_id=@institution_id) 
	order by lname

	--Report Texts
	select dr.report_id,dr.report_text,dr.report_text_html,dr.trans_report_text,
	       dr.trans_report_text_html,translate_report_text= isnull(dr.translate_report_text,''),translate_report_text_html=isnull(dr.translate_report_text_html,''),
		   rating=isnull(dr.rating,''),rating_reason_id = isnull(dr.rating_reason_id,'00000000-0000-0000-0000-000000000000'),
	       disclaimer_reason_id = isnull(dr.disclaimer_reason_id,0),
		   disclaimer_reason = isnull(rdr.type,''),
		   disclaimer_desc = isnull(dr.disclaimer_text,'') 
	from study_hdr_dictated_reports dr
	left outer join report_disclaimer_reasons rdr on rdr.id = dr.disclaimer_reason_id
	where dr.study_hdr_id=@id
	
	select pr.report_id,pr.report_text,pr.report_text_html,
	       rating=isnull(pr.rating,'') ,rating_reason_id = isnull(pr.rating_reason_id,'00000000-0000-0000-0000-000000000000'),
	       disclaimer_reason_id = isnull(pr.disclaimer_reason_id,0),
		   disclaimer_reason = isnull(rdr.type,'') ,
		   disclaimer_desc = isnull(pr.disclaimer_text,'') 
	from study_hdr_prelim_reports pr
	left outer join report_disclaimer_reasons rdr on rdr.id = pr.disclaimer_reason_id
	where pr.study_hdr_id=@id

	select fr.report_id,fr.report_text,fr.report_text_html,
	       disclaimer_reason_id = isnull(fr.disclaimer_reason_id,0),rating_reason_id = isnull(fr.rating_reason_id,'00000000-0000-0000-0000-000000000000'),
		   disclaimer_reason = isnull(rdr.type,''),
		   disclaimer_desc = isnull(fr.disclaimer_text,'') 
	from study_hdr_final_reports fr
	left outer join report_disclaimer_reasons rdr on rdr.id = fr.disclaimer_reason_id
	where fr.study_hdr_id=@id

	--Modality wise study type
	select st.study_type_id,mst.name 
	from study_hdr_study_types st
	inner join modality_study_types mst on mst.id=st.study_type_id
	where st.study_hdr_id = @id

	select priority_id,priority_desc,is_stat from sys_priority where is_active='Y' order by priority_desc
	select institution_consult_applicable = isnull(@institution_consult_applicable,'N')
	select id,name from sys_study_category where id in (select category_id from institution_category_link where institution_id=@institution_id) order by is_default desc,name
	
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
	
	--Studies Merged
	select study_id,study_uid,image_count,merge_compare_none
	from study_hdr_merged_studies 
	where study_hdr_id=@id
	
	--Abnormal Report Reasons
	select id,reason from abnormal_rpt_reasons where is_active='Y' order by reason	

	--country
	select id,name from sys_country  order by name

	--state
	select id,name from sys_states where country_id=isnull((select isnull(hdr.patient_country_id,isnull(ins.country_id,0))
	                                                        from study_hdr hdr
															left outer join institutions ins on ins.id = hdr.institution_id
															where hdr.id = @id),0)
	order by name

	--Modality Service Availability (Normal Hours)
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

	

	set nocount off
end

GO
