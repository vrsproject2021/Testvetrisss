USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_finalreport_footer_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_finalreport_footer_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_finalreport_footer_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_finalreport_footer_fetch : fetch 
                  final report footer
** Created By   : Pavel Guha
** Created On   : 04/03/2020
*******************************************************/
--exec rpt_finalreport_footer_fetch '2333d731-5b7a-46ce-a8ed-15be180805c4'

CREATE procedure [dbo].[rpt_finalreport_footer_fetch]
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

	if(@archived='N')
		begin
			select radiologist_name = r.name,sh.rpt_approve_date,sh.patient_name,sh.patient_id,site_code = i.code,
			       signage = convert(varchar(max),isnull(signage,''))
			from study_hdr sh
			inner join radiologists r on r.id= sh.final_radiologist_id
			inner join institutions i on i.id = sh.institution_id
			where sh.id = @id
	
		end
	else if(@archived='Y')
		begin
			select radiologist_name = r.name,sh.rpt_approve_date,sh.patient_name,sh.patient_id,site_code = i.code,
			       signage = convert(varchar(max),isnull(signage,''))
			from study_hdr_archive sh
			inner join radiologists r on r.id= sh.final_radiologist_id
			inner join institutions i on i.id = sh.institution_id
			where sh.id = @id
		end
	else if(@archived='D')
		begin
			set @strSQL ='select radiologist_name = r.name,sh.rpt_approve_date,sh.patient_name,sh.patient_id,site_code = i.code,'
			set @strSQL = @strSQL + 'signage = convert(varchar(max),isnull(signage,'''')) '
			set @strSQL = @strSQL + 'from ' + @db_name + '..study_hdr_archive sh '
			set @strSQL = @strSQL + 'inner join radiologists r on r.id= sh.final_radiologist_id '
			set @strSQL = @strSQL + 'inner join institutions i on i.id = sh.institution_id '
			set @strSQL = @strSQL + 'where sh.id = ''' +  convert(varchar(36),@id) + ''' '

			exec(@strSQL)
		end

	
	

	

	set nocount off
end

GO
