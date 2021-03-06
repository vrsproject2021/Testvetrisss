USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_custom_footer_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_studyreport_custom_footer_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_custom_footer_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_studyreport_custom_hdr_fetch : fetch 
                  final report details
** Created By   : Pavel Guha
** Created On   : 04/03/2020
*******************************************************/
--exec rpt_studyreport_custom_footer_fetch '2333d731-5b7a-46ce-a8ed-15be180805c4'

CREATE procedure [dbo].[rpt_studyreport_custom_footer_fetch]
    @id nvarchar(36)
as
begin
	 set nocount on

	 declare @radiologist_id uniqueidentifier,
	         @signage varchar(max),
			 @archived nchar(1),
			 @db_name nvarchar(50),
			 @strSQL varchar(max)
	
	if(select count(id) from study_hdr where id = @id)>0
		begin
			select @radiologist_id = final_radiologist_id
			from study_hdr
			where convert(varchar(36),id) = @id

			set @archived='N'
		end
	else if(select count(id) from study_hdr_archive where id = @id)>0
		begin
			select @radiologist_id = final_radiologist_id
			from study_hdr_archive
			where convert(varchar(36),id) = @id

			set @archived='Y'
		end
	else
		begin
			set @db_name=''
			exec common_get_study_database
				 @id      = @id,
				 @db_name = @db_name output

            set @archived='D'
		end

	set @signage='<p>'
	if(@archived='N')
		begin
			set @signage=@signage + '<b>Electronically Signed By</b> : ' + isnull((select name from radiologists where id = @radiologist_id),'') + '<br/>'
			set @signage=@signage + '<b>Approval Date/Time (CST)</b> : ' + isnull((select convert(varchar(10),rpt_approve_date,101)+ ' ' + convert(varchar(5),rpt_approve_date,114)  from study_hdr where id = @id),'01/01/1900 00:00') + '<br/>'
			set @signage=@signage + '<b>Patient Name</b>             : ' + isnull((select patient_name from study_hdr where id = @id),'') + '<br/>'
			set @signage=@signage + '<b>Patient ID</b>               : ' + isnull((select patient_id from study_hdr where id = @id),'') + '<br/>'
			set @signage=@signage + '<b>Site Code</b>                : ' + isnull((select code from institutions where id = (select isnull(institution_id,'00000000-0000-0000-0000-000000000000')  from study_hdr where id = @id)),'') + '<br/>'
		end
	else if(@archived='Y')
		begin
			set @signage=@signage + '<b>Electronically Signed By</b> : ' + isnull((select name from radiologists where id = @radiologist_id),'') + '<br/>'
			set @signage=@signage + '<b>Approval Date/Time (CST)</b> : ' + isnull((select convert(varchar(10),rpt_approve_date,101)+ ' ' + convert(varchar(5),rpt_approve_date,114)  from study_hdr_archive where id = @id),'01/01/1900 00:00') + '<br/>'
			set @signage=@signage + '<b>Patient Name</b>             : ' + isnull((select patient_name from study_hdr_archive where id = @id),'') + '<br/>'
			set @signage=@signage + '<b>Patient ID</b>               : ' + isnull((select patient_id from study_hdr_archive where id = @id),'') + '<br/>'
			set @signage=@signage + '<b>Site Code</b>                : ' + isnull((select code from institutions where id = (select isnull(institution_id,'00000000-0000-0000-0000-000000000000')  from study_hdr_archive where id = @id)),'') + '<br/>'
		end
	else if(@archived='D')
		begin
			create table #tmpArch
			(
				final_radiologist_id uniqueidentifier null,
				rpt_approve_date datetime null,
				patient_name nvarchar(100) null,
				patient_id nvarchar(36) null,
				institution_id uniqueidentifier null
			)
			set @strSQL='insert into #tmpArch(final_radiologist_id,rpt_approve_date,patient_name,patient_id,institution_id)'
			set @strSQL=@strSQL + '(select final_radiologist_id,rpt_approve_date,patient_name,patient_id,institution_id from ' +  @db_name + '..study_hdr_archive where id =''' + convert(varchar(36),@id) + ''')'
			exec(@strSQL)

			select @radiologist_id= isnull((select final_radiologist_id from #tmpArch),'00000000-0000-0000-0000-000000000000')

			set @signage=@signage + '<b>Electronically Signed By</b> : ' + isnull((select name from radiologists where id = @radiologist_id),'') + '<br/>'
			set @signage=@signage + '<b>Approval Date/Time (CST)</b> : ' + isnull((select convert(varchar(10),rpt_approve_date,101)+ ' ' + convert(varchar(5),rpt_approve_date,114)  from #tmpArch),'01/01/1900 00:00') + '<br/>'
			set @signage=@signage + '<b>Patient Name</b>             : ' + isnull((select patient_name from #tmpArch),'') + '<br/>'
			set @signage=@signage + '<b>Patient ID</b>               : ' + isnull((select patient_id from #tmpArch),'') + '<br/>'
			set @signage=@signage + '<b>Site Code</b>                : ' + isnull((select code from institutions where id = (select isnull(institution_id,'00000000-0000-0000-0000-000000000000')  from #tmpArch where id = @id)),'') + '<br/>'
		
		    drop table #tmpArch
		end

	
	set @signage=@signage + '</p>'

	if rtrim(ltrim((isnull((select convert(varchar(max),signage) from radiologists where id = @radiologist_id),''))))<>''
		begin
			select @signage =  @signage + convert(varchar(max),isnull(signage,''))
			from radiologists
			where id = @radiologist_id
		end

	
	
	select signage = @signage

	

	set nocount off
end

GO
