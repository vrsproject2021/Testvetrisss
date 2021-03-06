USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_correction_service_code_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_study_correction_service_code_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_correction_service_code_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_study_correction_service_code_fetch : 
                  fetch service codes
** Created By   : Pavel Guha 
** Created On   : 22-May-2020
*******************************************************/

--exec ar_study_correction_service_code_fetch '5B1DB430-339F-4BF4-8BC4-E24F2B09E241'
create procedure [dbo].[ar_study_correction_service_code_fetch]
	@id uniqueidentifier='00000000-0000-0000-0000-000000000000'
as
	begin
		set nocount on

		declare @service_codes nvarchar(250),
		        @rowcount int,
				@counter int,
				@code nvarchar(10)

		create table #tmp
		(
			id int,
			code nvarchar(10),
			name nvarchar(50),
			sel nchar(1) null default 'N'
		)
		

		insert into #tmp(id,code,name)
		(select id,code,name from services where is_active='Y' and isnull(code,'')<>'')

		--if(select count(id) from study_hdr where id=@id)>0
		--	begin
		--		select @service_codes= isnull(service_codes,'') from study_hdr where id=@id
		--	end
		--else if(select count(id) from study_hdr_archive where id=@id)>0
		--	begin
		--		select @service_codes= isnull(service_codes,'') from study_hdr_archive where id=@id
		--	end

		--if(rtrim(ltrim(@service_codes))<>'')
		--	begin
		--		create table #tmpCodes
		--		(
		--			row_id int,
		--			code nvarchar(10)
		--		)

		--		if(charindex(',',rtrim(ltrim(@service_codes))))>0
		--			begin
		--				insert into #tmpCodes(row_id,code)
		--				(select id,data from dbo.Split(@service_codes,','))
		--			end
		--		else
		--			begin
		--				insert into #tmpCodes(row_id,code) values(1,@service_codes)
		--			end

		--		select @rowcount=@@rowcount,@counter=1

		--		while(@counter<=@rowcount)
		--			begin
		--				select @code = code
		--				from #tmpCodes
		--				where row_id= @counter

		--				update #tmp
		--				set sel='Y'
		--				where code=@code

		--				set @counter = @counter + 1
		--			end

		--		drop table #tmpCodes
		--	end

		select id,code,name,sel
		from #tmp
		order by code

		drop table #tmp
		
		set nocount off
	end

	
GO
