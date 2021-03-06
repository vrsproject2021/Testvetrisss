USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_productivity_schedule_details_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_productivity_schedule_details_fetch]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_productivity_schedule_details_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_productivity_schedule_details_fetch : 
                  fetch file download records to finalise
** Created By   : Pavel Guha
** Created On   : 31/12/2020
*******************************************************/
--exec radiologist_productivity_schedule_details_fetch '01Mar2021','01Mar2021','5B098448-4F0F-444D-AAB4-19A4066553CB'
--exec radiologist_productivity_schedule_details_fetch '01Mar2021','03Mar2021','00000000-0000-0000-0000-000000000000'
CREATE procedure [dbo].[radiologist_productivity_schedule_details_fetch]
	@from_date datetime,
	@till_date datetime,
	@radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@group_id int =0
as
begin
	
	set nocount on

	declare @rowcount int,
	        @counter int,
			@id uniqueidentifier,
		    @name nvarchar(100),
		    @id_color nvarchar(10),
			@login_user_id uniqueidentifier,
		    @rpt_datetime datetime,
		    @study_uid nvarchar(100),
			@fdt datetime,
			@tdt datetime,
			@next_date datetime,
			@RADTHSTUDYCNT int,
			@reccount int,
			@ctr int

	create table #tmpRad
	(
		rec_id int identity(1,1),
		id uniqueidentifier,
		name nvarchar(100),
		id_color nvarchar(10),
		login_user_id  uniqueidentifier
	)

	create table #tmpRadProd
	(
		id uniqueidentifier,
		name nvarchar(100),
		id_color nvarchar(10),
		rpt_datetime datetime,
		study_uid nvarchar(100)
	)

	create table #tmpRadProdFinal
	(
		id uniqueidentifier,
		name nvarchar(100) default'',
		study_count int,
		hrs_scheduled decimal(12,2) default 0,
		study_count_per_hr int default 0
	)

	create table #tmpSubmitDate
	(
		id uniqueidentifier,
		submit_datetime datetime
	)
	
	set @tdt = dateadd(d,-1,@from_date);
	set @fdt = dateadd(d,-30,@tdt);

    --select @fdt, @tdt,@from_date,@till_date;
	select @RADTHSTUDYCNT = data_type_number
	from general_settings
	where control_code='RADTHSTUDYCNT'
	-- collect radiologists 
	if(isnull(@radiologist_id,'00000000-0000-0000-0000-000000000000')<>'00000000-0000-0000-0000-000000000000')
		begin
			insert into #tmpRad(id,name,id_color,login_user_id)(select id,name,identity_color,login_user_id from radiologists where is_active='Y' and id=@radiologist_id) order by name
		end
	else if(isnull(@group_id,0)>0)
		begin
			insert into #tmpRad(id,name,id_color,login_user_id)(select id,name,identity_color,login_user_id from radiologists where is_active='Y' and acct_group_id=@group_id) order by name
		end
	else
		begin
			insert into #tmpRad(id,name,id_color,login_user_id)(select id,name,identity_color,login_user_id from radiologists where is_active='Y') order by name
		end

	select @rowcount=@@rowcount,
	       @counter =1
	-- for each radiologist	collect reports
	while(@counter <= @rowcount)
		begin
			select  @id            = id,
					@name          = name,
					@id_color      = id_color,
					@login_user_id = login_user_id
			from #tmpRad
			where rec_id = @counter
			-- collect studies
			insert into #tmpRadProd(id,name,id_color,rpt_datetime,study_uid)
			(
			 select @id,@name,@id_color,t.date_created, t.study_uid
			 from
			 -- final reports archive
			 (select date_created= convert(datetime,date_created),study_uid
			 from study_hdr_final_reports_archive
			 where created_by = @login_user_id
			 and date_created between convert(datetime,convert(varchar(11),@fdt,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@tdt,106) + ' 23:59:59')
			 union
			 -- final reports
			 select date_created= convert(datetime,date_created),study_uid
			 from study_hdr_final_reports
			 where created_by = @login_user_id
			 and date_created between convert(datetime,convert(varchar(11),@fdt,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@tdt,106) + ' 23:59:59')) t
			 where t.study_uid not in (select study_uid from #tmpRadProd where id=@id))
			 order by date_created
			 
			 select @reccount=@@rowcount

			 insert into #tmpRadProd(id,name,id_color,rpt_datetime,study_uid)
			(
			 select @id,@name,@id_color,t.date_created, t.study_uid
			 from
			 -- dictated reports archive 
			 (select date_created= convert(datetime,date_created),study_uid
			 from study_hdr_dictated_reports_archive
			 where created_by = @login_user_id
			 and date_created between convert(datetime,convert(varchar(11),@fdt,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@tdt,106) + ' 00:00:00')
			 union
			 -- dictated reports  
			 select date_created= convert(datetime,date_created),study_uid
			 from study_hdr_dictated_reports
			 where created_by = @login_user_id
			 and date_created between convert(datetime,convert(varchar(11),@fdt,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@tdt,106) + ' 00:00:00')) t
			 where t.study_uid not in (select study_uid from #tmpRadProd where id=@id))
			 order by date_created

			 select @reccount=@reccount + @@rowcount

			 if(@reccount=0)
				begin
					set @next_date =@fdt

					while(@next_date <= @tdt)
						begin
							set @ctr=1
							

							while(@ctr<=@RADTHSTUDYCNT)
								begin
									
									insert into #tmpRadProd(id,name,id_color,rpt_datetime,study_uid)
					                                 values(@id,@name,@id_color,@fdt,'')

									set @ctr = @ctr + 1
								end

							set @next_date = dateadd(d,1,@next_date)
						end

					
				end

			set @counter = @counter + 1
		end
	
	insert into #tmpRadProdFinal(id,study_count)
	(select id,study_count=count(study_uid)
	from #tmpRadProd 
	group by id)

	update #tmpRadProdFinal
	set name =(select name from radiologists where id=#tmpRadProdFinal.id)

	update #tmpRadProdFinal
	set hrs_scheduled = isnull(((select sum(((duration_in_ms/1000)/60.0)/60.0)
								  from radiologist_schedule
								  where radiologist_id=#tmpRadProdFinal.id
								  and start_datetime>= convert(datetime,convert(varchar(11),@fdt + ' 00:00:00'))
								  and end_datetime<=convert(datetime,convert(varchar(11),@tdt + ' 23:59:59')))),0)

	update #tmpRadProdFinal
	set study_count_per_hr = convert(int,study_count/hrs_scheduled)
	where hrs_scheduled>0

	update #tmpRadProdFinal
	set study_count_per_hr = @RADTHSTUDYCNT
	where study_count_per_hr=0

	select id, name, hrs_scheduled hrsScheduled, study_count_per_hr studyCountPerHr from #tmpRadProdFinal order by name

	-- stats
	declare @dw table ( [date] date, dw int);
	declare @d date = @from_date;
	declare @td date;
	while @d<=@till_date
	begin
		declare @day int=DATEPART(dw, @d);
		set @td = dateadd(d,-7*5,@d); -- 5 week old data

		while @td<=@d
		begin
			if(not exists(select top 1 [date] from @dw where [date]=@td))
				insert into @dw(date,dw) values (@td,@day);
			set @td = dateadd(d,7,@td);
		end
	
		set @d = dateadd(d,1,@d)
	end

	select @fdt = min(date), @tdt=max(date) from @dw

	insert into #tmpSubmitDate(id,submit_datetime)
	(select study_id,max(date_updated) 
	 from sys_case_study_status_log 
	 where status_id_to=50 
	 and date_updated between convert(datetime,convert(varchar(11),@fdt,106) + ' 00:00:00') and convert(datetime,convert(varchar(11),@tdt,106) + ' 23:59:59')
	 group by study_id)

	select stat= count(h.id),t.submit_datetime [date], h.priority_id [type]
	from study_hdr h
	inner join #tmpSubmitDate t on t.id = h.id
	where h.priority_id in(10,20)
	group by t.submit_datetime, h.priority_id
	union
	select stat= count(h.id),t.submit_datetime [date], h.priority_id [type]
	from study_hdr_archive h
	inner join #tmpSubmitDate t on t.id = h.id
	where h.priority_id in(10,20)
	group by t.submit_datetime, h.priority_id
	
    drop table #tmpRad
	drop table #tmpRadProd
	drop table #tmpRadProdFinal
	drop table #tmpSubmitDate
	set nocount off


end


GO
