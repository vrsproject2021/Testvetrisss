USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_custom_hdr_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_studyreport_custom_hdr_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_studyreport_custom_hdr_fetch]    Script Date: 28-09-2021 19:36:35 ******/
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
--exec rpt_studyreport_custom_hdr_fetch '7B250217-8826-4C84-B86C-0624C240C2DB'

create procedure [dbo].[rpt_studyreport_custom_hdr_fetch]
    @id nvarchar(36)
as
begin
	 set nocount on

	 declare @address varchar(max),
	         @institution_id uniqueidentifier


	 set @address=''
	 if(select count(id) from study_hdr where id = @id)>0
		begin
			 select @institution_id = institution_id
			 from study_hdr
			 where convert(varchar(36),id) = @id
		end
	else
		begin
			select @institution_id = institution_id
			 from study_hdr_archive
			 where convert(varchar(36),id) = @id
		end


	if(select isnull(address_1,'') from institutions where id = @institution_id)<>''
		begin
			select @address =  isnull(address_1,'') from institutions where id = @institution_id
		end
	if(select isnull(address_2,'') from institutions where id = @institution_id)<>''
		begin
			select @address =  @address + isnull(address_2,'') from institutions where id = @institution_id
		end 
	if(select isnull(city,'') from institutions where id = @institution_id)<>''
		begin
			select @address =  @address + '<br/>' +  isnull(city,'') from institutions where id = @institution_id
		end 
	if(select isnull(state_id,0) from institutions where id = @institution_id)<>0
		begin
			select @address =  @address + ',' +  name
			from sys_states
			where id = (select state_id from institutions where id = @institution_id)
		end 
	if(select isnull(zip,'') from institutions where id = @institution_id)<>0
		begin
			select @address =  @address + '<br/>Zip:' +  zip from institutions where id = @institution_id
		end
	if(select isnull(country_id,0) from institutions where id = @institution_id)<>0
		begin
			select @address =  @address + '<br/>' +  name
			from sys_country
			where id = (select country_id from institutions where id = @institution_id)
		end 


	select logo_img,
	       addr = @address
	from institutions where id= @institution_id
	

	set nocount off
end

GO
