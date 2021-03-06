USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_final_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_studyreport_final_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_final_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_studyreport_final_fetch : fetch 
                  final report details
** Created By   : Pavel Guha
** Created On   : 11/02/2020
*******************************************************/
--exec rpt_studyreport_final_fetch 'cba2d1f6-204f-4123-b2e6-263d671b13a3','11111111-1111-1111-1111-111111111111'

CREATE procedure [dbo].[rpt_studyreport_final_fetch]
    @id nvarchar(36),
	@user_id nvarchar(36)
    
as
begin
	 set nocount on

	 declare @study_types varchar(max),
	         @user_role_id int,
	         @user_role_code nvarchar(10),
			 @rad_id uniqueidentifier,
			 @VWINSTINFOCount int,
			 @db_name nvarchar(50),
			 @strSQL varchar(max)

	 set @study_types=''

	 select @user_role_id = u.user_role_id,
	        @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	if(@user_role_code='RDL')
		begin
			select @rad_id = id from radiologists where login_user_id = @user_id
			select @VWINSTINFOCount = count(right_code) from radiologist_functional_rights_assigned where right_code='VWINSTINFO' and radiologist_id=@rad_id
		end
	

	 if(select count(id) from study_hdr where id= @id)>0
		begin
			if(select count(study_type_id) from study_hdr_study_types where study_hdr_id=@id)=1
				begin
					select @study_types= name
					from modality_study_types
					where id in (select study_type_id from study_hdr_study_types where study_hdr_id=@id) 

					
				end
			else
				begin
					select @study_types = isnull(@study_types,'') + name +','
					from modality_study_types
					where id in (select study_type_id from study_hdr_study_types where study_hdr_id=@id)

					set @study_types = substring(@study_types,0,len(@study_types))
				end
				
			
			select hdr.study_uid,
				   hdr.study_date,
				   hdr.received_date,
				   patient_id = isnull(hdr.patient_id,''),
				   patient_name = isnull(hdr.patient_name,''),
				   patient_sex = isnull(hdr.patient_sex,''),
				   patient_spayed_neutered = isnull(hdr.patient_sex_neutered,''),
				   patient_age_accepted = isnull(hdr.patient_age_accepted,0),
				   case
						when isnull(wt_uom,'') = 'lbs' then isnull(hdr.patient_weight,0)
						when isnull(wt_uom,'') = 'kgs' then isnull(hdr.patient_weight_kgs,0)
						when isnull(wt_uom,'') = '' then isnull(hdr.patient_weight_pacs,0)
				   end patient_weight,
				   wt_uom = isnull(hdr.wt_uom,''),
				   owner_name = rtrim(ltrim(isnull(hdr.owner_first_name,'') + ' ' +  isnull(hdr.owner_last_name,''))),
				   species_name=isnull(sp.name,''),
				   breed_name = isnull(br.name,''),
				   modality_name=isnull(m.name,''),
				   institution_code = isnull(ins.code,''),
				   case
						when (@user_role_code='RDL' and @VWINSTINFOCount=0) then isnull(ins.code,'') else isnull(ins.name,'')
				   end institution_name,
				   case
						when (@user_role_code='RDL' and @VWINSTINFOCount=0) then isnull(ph.code,'') else isnull(ph.name,'')
				   end physician_name,
				   reason_accepted= isnull(hdr.reason_accepted,''),
				   accession_no = isnull(hdr.accession_no,''),
				   image_count = isnull(hdr.img_count,0),
				   study_types = isnull(@study_types,''),
				   report_text=isnull(fr.report_text,''),
				   report_text_html = isnull(cast(fr.report_text_html as nvarchar(max)),''),
				   final_radiologist = isnull(r.name,''),
				   rpt_approve_date = isnull(hdr.rpt_approve_date,'01jan1900'),
				   disclaimer_reason = isnull(convert(nvarchar(max),fr.disclaimer_text),'')
			from study_hdr hdr
			left outer join modality m on m.id= hdr.modality_id
			left outer join body_part bp on bp.id= hdr.body_part_id
			left outer join species sp on sp.id= hdr.species_id
			left outer join breed br on br.id= hdr.breed_id
			left outer join institutions ins on ins.id= hdr.institution_id
			left outer join physicians ph on ph.id= hdr.physician_id
			left outer join study_hdr_final_reports fr on fr.study_hdr_id = hdr.id
			--left outer join report_disclaimer_reasons rdr on rdr.id = fr.disclaimer_reason_id
			left outer join radiologists r on r.id = hdr.final_radiologist_id
			where convert(varchar(36),hdr.id)=@id
		end
	else if(select count(id) from study_hdr_archive where id= @id)>0
		begin
			if(select count(study_type_id) from study_hdr_study_types_archive where study_hdr_id=@id)=1
				begin
					select @study_types= name
					from modality_study_types
					where id in (select study_type_id from study_hdr_study_types_archive where study_hdr_id=@id) 
				end
			else
				begin
					select @study_types = isnull(@study_types,'') + name +','
					from modality_study_types
					where id in (select study_type_id from study_hdr_study_types_archive where study_hdr_id=@id)

					set @study_types = substring(@study_types,0,len(@study_types))
				end
			
			

			select hdr.study_uid,
				   hdr.study_date,
				   hdr.received_date,
				   patient_id = isnull(hdr.patient_id,''),
				   patient_name = isnull(hdr.patient_name,''),
				   patient_sex = isnull(hdr.patient_sex,''),
				   patient_spayed_neutered = isnull(hdr.patient_sex_neutered,''),
				   patient_age_accepted = isnull(hdr.patient_age_accepted,0),
				   case
						when isnull(wt_uom,'') = 'lbs' then isnull(hdr.patient_weight,0)
						when isnull(wt_uom,'') = 'kgs' then isnull(hdr.patient_weight_kgs,0)
						when isnull(wt_uom,'') = '' then isnull(hdr.patient_weight_pacs,0)
				   end patient_weight,
				   wt_uom = isnull(hdr.wt_uom,''),
				   owner_name = rtrim(ltrim(isnull(hdr.owner_first_name,'') + ' ' +  isnull(hdr.owner_last_name,''))),
				   species_name=isnull(sp.name,''),
				   breed_name = isnull(br.name,''),
				   modality_name=isnull(m.name,''),
				   institution_code = isnull(ins.code,''),
				   institution_name= isnull(ins.name,''),
				   physician_name = isnull(ph.name,''),
				   reason_accepted= isnull(hdr.reason_accepted,''),
				   accession_no = isnull(hdr.accession_no,''),
				   image_count = isnull(hdr.img_count,0),
				   study_types = isnull(@study_types,''),
				   report_text=isnull(fr.report_text,''),
				   report_text_html = isnull(cast(fr.report_text_html as nvarchar(max)),''),
				   final_radiologist = isnull(r.name,''),
				   rpt_approve_date = isnull(hdr.rpt_approve_date,'01jan1900'),
				   disclaimer_reason = isnull(convert(nvarchar(max),fr.disclaimer_text),'')
			from study_hdr_archive hdr
			left outer join modality m on m.id= hdr.modality_id
			left outer join body_part bp on bp.id= hdr.body_part_id
			left outer join species sp on sp.id= hdr.species_id
			left outer join breed br on br.id= hdr.breed_id
			left outer join institutions ins on ins.id= hdr.institution_id
			left outer join physicians ph on ph.id= hdr.physician_id
			left outer join study_hdr_final_reports_archive fr on fr.study_hdr_id = hdr.id
			--left outer join report_disclaimer_reasons rdr on rdr.id = fr.disclaimer_reason_id
			left outer join radiologists r on r.id = hdr.final_radiologist_id
			where convert(varchar(36),hdr.id)=@id
		end
	else
		begin
			set @db_name=''
			exec common_get_study_database
				@id      = @id,
				@db_name = @db_name output

			create table #tmpST(id uniqueidentifier,name nvarchar(100))
			set @strSQL = 'insert into #tmpST(id,name)'
			set @strSQL = @strSQL + '(select id,name from modality_study_types where id in (select study_type_id from ' + @db_name + '..study_hdr_study_types_archive where study_hdr_id=''' + convert(varchar(36),@id) + '''))'
			exec(@strSQL)

			select @study_types = isnull(@study_types,'') + name +',' from #tmpST
			set @study_types = substring(@study_types,0,len(@study_types))

			set @strSQL='select hdr.study_uid,'
			set @strSQL= @strSQL + 'hdr.study_date,'
			set @strSQL= @strSQL + 'hdr.received_date,'
			set @strSQL= @strSQL + 'patient_id = isnull(hdr.patient_id,''''),'
			set @strSQL= @strSQL + 'patient_name = isnull(hdr.patient_name,''''),'
			set @strSQL= @strSQL + 'patient_sex = isnull(hdr.patient_sex,''''),'
			set @strSQL= @strSQL + 'patient_spayed_neutered = isnull(hdr.patient_sex_neutered,''''),'
			set @strSQL= @strSQL + 'patient_age_accepted = isnull(hdr.patient_age_accepted,0),'
			set @strSQL= @strSQL + 'case '
			set @strSQL= @strSQL + 'when isnull(wt_uom,'''') = ''lbs'' then isnull(hdr.patient_weight,0) '
			set @strSQL= @strSQL + 'when isnull(wt_uom,'''') = ''kgs'' then isnull(hdr.patient_weight_kgs,0) '
			set @strSQL= @strSQL + 'when isnull(wt_uom,'''') = '''' then isnull(hdr.patient_weight_pacs,0) '
			set @strSQL= @strSQL + 'end patient_weight,'
			set @strSQL= @strSQL + 'wt_uom = isnull(hdr.wt_uom,''''),'
			set @strSQL= @strSQL + 'owner_name = rtrim(ltrim(isnull(hdr.owner_first_name,'''') + '' '' +  isnull(hdr.owner_last_name,''''))),'
			set @strSQL= @strSQL + 'species_name=isnull(sp.name,''''),'
			set @strSQL= @strSQL + 'breed_name = isnull(br.name,''''),'
			set @strSQL= @strSQL + 'modality_name=isnull(m.name,''''),'
			set @strSQL= @strSQL + 'institution_code = isnull(ins.code,''''),'
			set @strSQL= @strSQL + 'institution_name= isnull(ins.name,''''),'
			set @strSQL= @strSQL + 'physician_name = isnull(ph.name,''''),'
			set @strSQL= @strSQL + 'reason_accepted= isnull(hdr.reason_accepted,''''),'
			set @strSQL= @strSQL + 'accession_no = isnull(hdr.accession_no,''''),'
			set @strSQL= @strSQL + 'image_count = isnull(hdr.img_count,0),'
			set @strSQL= @strSQL + 'study_types = ''' +  isnull(@study_types,'') + ''','
			set @strSQL= @strSQL + 'report_text=isnull(fr.report_text,''''),'
			set @strSQL= @strSQL + 'report_text_html = isnull(cast(fr.report_text_html as nvarchar(max)),''''),'
			set @strSQL= @strSQL + 'final_radiologist = isnull(r.name,''''),'
			set @strSQL= @strSQL + 'rpt_approve_date = isnull(hdr.rpt_approve_date,''01jan1900''),'
			set @strSQL= @strSQL + 'disclaimer_reason = isnull(convert(nvarchar(max),fr.disclaimer_text),'''') '
			set @strSQL= @strSQL + 'from ' + @db_name + '..study_hdr_archive hdr '
			set @strSQL= @strSQL + 'left outer join modality m on m.id= hdr.modality_id '
			set @strSQL= @strSQL + 'left outer join body_part bp on bp.id= hdr.body_part_id '
			set @strSQL= @strSQL + 'left outer join species sp on sp.id= hdr.species_id '
			set @strSQL= @strSQL + 'left outer join breed br on br.id= hdr.breed_id '
			set @strSQL= @strSQL + 'left outer join institutions ins on ins.id= hdr.institution_id '
			set @strSQL= @strSQL + 'left outer join physicians ph on ph.id= hdr.physician_id '
			set @strSQL= @strSQL + 'left outer join ' + @db_name + '..study_hdr_final_reports_archive fr on fr.study_hdr_id = hdr.id '
			set @strSQL= @strSQL + 'left outer join radiologists r on r.id = hdr.final_radiologist_id '
			set @strSQL= @strSQL + 'where convert(varchar(36),hdr.id)=''' + convert(varchar(36),@id) + ''''

			exec(@strSQL)
			drop table #tmpST
		end

	set nocount off
end

GO
