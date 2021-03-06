USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_write_back_report_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_write_back_report_details_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_write_back_report_details_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_write_back_report_records_fetch :  fetch 
				  report records for write back to pacs
** Created By   : Pavel Guha
** Created On   : 31/07/2020
*******************************************************/
-- exec scheduler_write_back_report_records_fetch
CREATE PROCEDURE [dbo].[scheduler_write_back_report_details_fetch] 
	@study_hdr_id uniqueidentifier,
	@is_addendum nchar(1)='N'
as
begin
	set nocount on

	declare @status_id int

	if(select count(id) from study_hdr where id=@study_hdr_id)>0
		begin
			if(@is_addendum='N')
				begin
					select dict_radiologist_pacs    = rtrim(ltrim((isnull(r1.lname,'') + ' ' + isnull(r1.fname,'') + ' ' + isnull(r1.credentials,'')))),
						   prelim_radiologist_pacs  = rtrim(ltrim((isnull(r2.lname,'') + ' ' + isnull(r2.fname,'') + ' ' + isnull(r2.credentials,'')))),
						   final_radiologist_pacs   = rtrim(ltrim((isnull(r3.lname,'') + ' ' + isnull(r3.fname,'') + ' ' + isnull(r3.credentials,'')))),
						   sh.study_status_pacs,
						   dict_date = isnull(dr.date_created,'01jan1900'),
						   prelim_date = isnull(pr.date_created,'01jan1900'),
						   final_date = isnull(fr.date_created,'01jan1900')
					from study_hdr sh
					left outer join study_hdr_dictated_reports dr on dr.study_hdr_id = sh.id
					left outer join study_hdr_prelim_reports pr on pr.study_hdr_id = sh.id
					left outer join study_hdr_final_reports fr on fr.study_hdr_id = sh.id
					left outer join radiologists r1 on r1.login_user_id = dr.created_by
					left outer join radiologists r2 on r2.login_user_id = pr.created_by
					left outer join radiologists r3 on r3.login_user_id = fr.created_by
					where sh.id=@study_hdr_id
				end
			else if(@is_addendum='Y')
				begin
					select dict_radiologist_pacs    = isnull(r1.lname,'') + ' ' + isnull(r1.fname,'') + ' ' + isnull(r1.credentials,''),
						   prelim_radiologist_pacs  = isnull(r2.lname,'') + ' ' + isnull(r2.fname,'') + ' ' + isnull(r2.credentials,''),
						   final_radiologist_pacs   = isnull(r3.lname,'') + ' ' + isnull(r3.fname,'') + ' ' + isnull(r2.credentials,''),
						   sh.study_status_pacs,
						   dict_date = isnull(dr.date_created,'01jan1900'),
						   prelim_date = isnull(pr.date_created,'01jan1900'),
						   final_date = isnull(ra.date_created,'01jan1900')
					from study_hdr sh
					left outer join study_report_addendums ra on ra.study_hdr_id = sh.id
					left outer join study_hdr_dictated_reports dr on dr.study_hdr_id = sh.id
					left outer join study_hdr_prelim_reports pr on pr.study_hdr_id = sh.id
					left outer join study_hdr_final_reports fr on fr.study_hdr_id = sh.id
					left outer join radiologists r1 on r1.login_user_id = dr.created_by
					left outer join radiologists r2 on r2.login_user_id = pr.created_by
					left outer join radiologists r3 on r3.login_user_id = fr.created_by
					where sh.id=@study_hdr_id
				end

			select @status_id = study_status_pacs from study_hdr where id=@study_hdr_id

			if(@status_id=60)
				begin
					select record_id   = 0,
					       report_text = cast(report_text as nvarchar(max)),
						   is_addendum='N'
				    from study_hdr_dictated_reports 
					where pacs_wb='Y'
				end
			else if(@status_id=80)
				begin
					select record_id   = 0,
					       report_text = cast(report_text as nvarchar(max)),
						   is_addendum='N'
				    from study_hdr_prelim_reports 
					where pacs_wb='Y'
				end
			else if(@status_id=100)
				begin
					select record_id   = 0,
					       report_text = cast(report_text as nvarchar(max)),
						   is_addendum='N'
				    from study_hdr_final_reports 
					where pacs_wb='Y'
					union
					select record_id=ra.addendum_srl,
						   report_text= cast(ra.addendum_text as nvarchar(max)),
						   is_addendum='Y'
					from study_report_addendums ra 
					where ra.pacs_wb='Y'
					order by record_id
				end
		end
	else if(select count(id) from study_hdr_archive where id=@study_hdr_id)>0
		begin
			if(@is_addendum='N')
				begin
					select dict_radiologist_pacs    = rtrim(ltrim((isnull(r1.lname,'') + ' ' + isnull(r1.fname,'') + ' ' + isnull(r1.credentials,'')))),
						   prelim_radiologist_pacs  = rtrim(ltrim((isnull(r2.lname,'') + ' ' + isnull(r2.fname,'') + ' ' + isnull(r2.credentials,'')))),
						   final_radiologist_pacs   = rtrim(ltrim((isnull(r3.lname,'') + ' ' + isnull(r3.fname,'') + ' ' + isnull(r3.credentials,'')))),
						   sh.study_status_pacs,
						   dict_date = isnull(dr.date_created,'01jan1900'),
						   prelim_date = isnull(pr.date_created,'01jan1900'),
						   final_date = isnull(fr.date_created,'01jan1900')
					from study_hdr_archive sh
					left outer join study_hdr_dictated_reports_archive dr on dr.study_hdr_id = sh.id
					left outer join study_hdr_prelim_reports_archive pr on pr.study_hdr_id = sh.id
					left outer join study_hdr_final_reports_archive fr on fr.study_hdr_id = sh.id
					left outer join radiologists r1 on r1.login_user_id = dr.created_by
					left outer join radiologists r2 on r2.login_user_id = pr.created_by
					left outer join radiologists r3 on r3.login_user_id = fr.created_by
					where sh.id=@study_hdr_id
				end
			else if(@is_addendum='Y')
				begin
					select dict_radiologist_pacs    = isnull(r1.lname,'') + ' ' + isnull(r1.fname,'') + ' ' + isnull(r1.credentials,''),
						   prelim_radiologist_pacs  = isnull(r2.lname,'') + ' ' + isnull(r2.fname,'') + ' ' + isnull(r2.credentials,''),
						   final_radiologist_pacs   = rtrim(ltrim((isnull(r3.lname,'') + ' ' + isnull(r3.fname,'') + ' ' + isnull(r3.credentials,'')))),
						   sh.study_status_pacs,
						   dict_date = isnull(dr.date_created,'01jan1900'),
						   prelim_date = isnull(pr.date_created,'01jan1900'),
						   final_date = isnull(ra.date_created,'01jan1900')
					from study_hdr_archive sh
					left outer join study_report_addendums_archive ra on ra.study_hdr_id = sh.id
					left outer join study_hdr_dictated_reports_archive dr on dr.study_hdr_id = sh.id
					left outer join study_hdr_prelim_reports_archive pr on pr.study_hdr_id = sh.id
					left outer join study_hdr_final_reports_archive fr on fr.study_hdr_id = sh.id
					left outer join radiologists r1 on r1.login_user_id = dr.created_by
					left outer join radiologists r2 on r2.login_user_id = pr.created_by
					left outer join radiologists r3 on r3.login_user_id = fr.created_by
					where sh.id=@study_hdr_id
				end

			select @status_id = study_status_pacs from study_hdr_archive where id=@study_hdr_id

			if(@status_id=60)
				begin
					select record_id   = 0,
					       report_text = cast(report_text as nvarchar(max)),
						   is_addendum='N'
				    from study_hdr_dictated_reports_archive
					where pacs_wb='Y'
				end
			else if(@status_id=80)
				begin
					select record_id   = 0,
					       report_text = cast(report_text as nvarchar(max)),
						   is_addendum='N'
				    from study_hdr_prelim_reports_archive
					where pacs_wb='Y'
				end
			else if(@status_id=100)
				begin
					select record_id   = 0,
					       report_text = cast(report_text as nvarchar(max)),
						   is_addendum='N'
				    from study_hdr_final_reports_archive
					where pacs_wb='Y'
					union
					select record_id=ra.addendum_srl,
						   report_text= cast(ra.addendum_text as nvarchar(max)),
						   is_addendum='Y'
					from study_report_addendums_archive ra 
					where ra.pacs_wb='Y'
					order by record_id
				end
		end
	
	set nocount off
end

GO
