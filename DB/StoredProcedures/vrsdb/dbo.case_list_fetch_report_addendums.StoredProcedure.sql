USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_report_addendums]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_fetch_report_addendums]
GO
/****** Object:  StoredProcedure [dbo].[case_list_fetch_report_addendums]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_fetch_report_addendums : fetch 
				  selected study types
** Created By   : Pavel Guha
** Created On   : 28/07/2020
*******************************************************/
--exec case_list_fetch_report_addendums '02C6DE5E-381D-4B89-8505-7B191E59BD88'
CREATE procedure [dbo].[case_list_fetch_report_addendums]
    @id uniqueidentifier
as
begin
	 set nocount on

	 declare @strSQL varchar(max),
	          @db_name nvarchar(50)
	
	 if(select count(study_hdr_id) from study_report_addendums  where study_hdr_id=@id)>0
		begin
			select addendum_srl,addendum_text from study_report_addendums  where study_hdr_id=@id order by addendum_srl
		end
	else if(select count(study_hdr_id) from study_report_addendums_archive  where study_hdr_id=@id)>0
		begin
			select addendum_srl,addendum_text from study_report_addendums_archive  where study_hdr_id=@id order by addendum_srl
		end
	else
		begin
			set @db_name=''
			exec common_get_study_database
				 @id      = @id,
				 @db_name = @db_name output

		    set @strSQL ='select addendum_srl,addendum_text from ' + @db_name + '..study_report_addendums_archive  where study_hdr_id=''' + convert(varchar(36),@id)  + ''' order by addendum_srl'
			exec(@strSQL)
		end
		
	set nocount off
end

GO
