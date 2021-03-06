USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_addendum_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_addendum_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_addendum_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_addendum_fetch : 
                  fetch addendum text
** Created By   : Pavel Guha 
** Created On   : 29/07/2020
*******************************************************/

CREATE procedure [dbo].[case_list_addendum_fetch]
	@study_id uniqueidentifier,
	@addendum_srl int
as
	begin
		set nocount on

		declare @strSQL varchar(max),
	            @db_name nvarchar(50)
		
		if(select count(study_hdr_id) from study_report_addendums where study_hdr_id=@study_id)>0
			begin
				select addendum_text
				from study_report_addendums
				where study_hdr_id=@study_id
				and addendum_srl = @addendum_srl
			end
		else if(select count(study_hdr_id) from study_report_addendums_archive where study_hdr_id=@study_id)>0
			begin
				select addendum_text
				from study_report_addendums_archive
				where study_hdr_id=@study_id
				and addendum_srl = @addendum_srl
			end
		else
			begin
				set @db_name=''
				exec common_get_study_database
						@id      = @study_id,
						@db_name = @db_name output

				set @strSQL ='select addendum_text from ' + @db_name + '..study_report_addendums_archive where study_hdr_id=''' + convert(varchar(36),@study_id) + ''' and addendum_srl = ' + convert(varchar,@addendum_srl)
				exec(@strSQL)
			end

		set nocount off

	end
GO
