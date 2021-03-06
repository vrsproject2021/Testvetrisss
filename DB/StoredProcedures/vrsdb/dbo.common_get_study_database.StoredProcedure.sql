USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_get_study_database]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_get_study_database]
GO
/****** Object:  StoredProcedure [dbo].[common_get_study_database]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_get_study_database : fetch 
                  stiudy's database
** Created By   : Pavel Guha
** Created On   : 11-06-2021
*******************************************************/
--exec rpt_finalreport_fetch 'DE1401A2-AC74-4FA7-A4FC-E3E868D24C93','11111111-1111-1111-1111-111111111111'

create procedure [dbo].[common_get_study_database]
    @id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@study_uid nvarchar(100)=null,
	@db_name nvarchar(50)='' output
as
begin
	declare @rc int,
			@ctr int,
			@strQL nvarchar(1000)

	create table #tmpArchDB
	(
		rec_id int identity(1,1),
		db_name nvarchar(50)
	)
	create table #tmpID
	(
		id uniqueidentifier,
		study_uid nvarchar(100)
	)

	insert into #tmpArchDB([db_name]) (select [db_name] from sys_archive_db)order by db_year desc

	select @rc=count(rec_id),@ctr=1 from  #tmpArchDB

	while(@ctr <= @rc)
		begin
			select @db_name = [db_name] from #tmpArchDB where rec_id = @ctr
			if(isnull(@id,'00000000-0000-0000-0000-000000000000')<> '00000000-0000-0000-0000-000000000000')
				begin
					set @strQL ='insert into #tmpID(id,study_uid)(select id,study_uid from ' + @db_name+ '..study_hdr_archive where id=''' + convert(varchar(36),@id) + ''')'
				end
			else if(isnull(@study_uid,'')<> '')
				begin
					set @strQL ='insert into #tmpID(id,study_uid)(select id,study_uid from ' + @db_name+ '..study_hdr_archive where study_uid=''' + @study_uid + ''')'
				end
			
			exec(@strQL)
			if(select count(id) from #tmpID)>0
				begin
					return 1
				end
			set @ctr = @ctr + 1
	    end
	select @db_name='vrsdb'
	return 0
end

GO
