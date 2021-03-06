USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_final_addendum_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_studyreport_final_addendum_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_final_addendum_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_studyreport_final_addendum_fetch : fetch 
                  final report addendum details
** Created By   : Pavel Guha
** Created On   : 11/02/2020
*******************************************************/
--exec rpt_studyreport_final_addendum_fetch 'cba2d1f6-204f-4123-b2e6-263d671b13a3'

CREATE procedure [dbo].[rpt_studyreport_final_addendum_fetch]
    @id nvarchar(36)
    
as
begin
	 set nocount on
	  declare @db_name nvarchar(50),
			  @strSQL varchar(max)
	

	 if(select count(id) from study_hdr where id= @id)>0
		begin
			select addendum_srl,
				   approved_by = isnull((select name from radiologists where login_user_id=study_report_addendums.created_by),''),
				   date_approved= date_created,
			       addendum_text,
				   addendum_text_html= isnull(cast(addendum_text_html as nvarchar(max)),'')
			from study_report_addendums
			where convert(varchar(36),study_hdr_id) = @id
			order by addendum_srl
		end
	else if(select count(id) from study_hdr_archive where id= @id)>0
		begin
			select addendum_srl,
			       approved_by = isnull((select name from radiologists where login_user_id=study_report_addendums_archive.created_by),''),
				   date_approved= date_created,
			       addendum_text,
				   addendum_text_html= isnull(cast(addendum_text_html as nvarchar(max)),'')
			from study_report_addendums_archive
			where convert(varchar(36),study_hdr_id) = @id
			order by addendum_srl
		end
	else
		begin
			set @db_name=''
			exec common_get_study_database
				 @id      = @id,
				 @db_name = @db_name output

		   set @strSQL ='select  addendum_srl,'
		   set @strSQL = @strSQL + 'approved_by = isnull((select name from radiologists where login_user_id=study_report_addendums_archive.created_by),''''),'
		   set @strSQL = @strSQL + 'date_approved= date_created,'
		   set @strSQL = @strSQL + 'addendum_text,'
		   set @strSQL = @strSQL + 'addendum_text_html= isnull(cast(addendum_text_html as nvarchar(max)),'''') '
		   set @strSQL = @strSQL + 'from ' + @db_name + '..study_report_addendums_archive '
		   set @strSQL = @strSQL + 'where study_hdr_id = ''' + convert(varchar(36),@id) + ''' ' 
		   set @strSQL = @strSQL + 'order by addendum_srl '

		   exec(@strSQL)
		end

	set nocount off
end

GO
