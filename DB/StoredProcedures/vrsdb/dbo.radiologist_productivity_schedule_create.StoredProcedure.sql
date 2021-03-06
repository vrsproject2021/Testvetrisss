USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_productivity_schedule_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_productivity_schedule_create]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_productivity_schedule_create]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_schedule_create : save
                  radiologist schedule details.
** Created By   : Pavel Guha
** Created On   : 17/09/2019
*******************************************************/
/*
exec radiologist_productivity_schedule_create
'<radiologist><row><radiologist_id>0a85e659-ad5c-4ee1-94ce-9fd8cd9a29b6</radiologist_id><row_id>1</row_id></row></radiologist>',
'7/15/2020 12:00:00 AM','7/31/2020 12:00:00 AM','13:00:00','15:00:00','N',0,'sample notes',
'<weekday><row><week_day_no>6</week_day_no><row_id>1</row_id></row></weekday>',
'11111111-1111-1111-1111-111111111111',38,'',0

*/

CREATE procedure [dbo].[radiologist_productivity_schedule_create]
(
	@xml_radiologist ntext,
	@start_date      datetime,
	@end_date        datetime,
	@start_time      nvarchar(8)   ='00:00:00',
	@end_time        nvarchar(8)   ='00:00:00',
	@for_next_days   nchar(1)      = 'N',
	@next_days       int           = 0,
	@xml_weekday    ntext         = null,
	@notes			nvarchar(250) = null,	
	@updated_by      uniqueidentifier,
	@menu_id         int,
	@error_code		 nvarchar(10)  = '' output,
    @return_status   int		   = 0  output
)
as
begin
	set nocount on 
	set datefirst 1  

	declare @from_date datetime,
	     	@till_date datetime,
			@next_start_date datetime,
			@next_end_date datetime,
	        @duration bigint,
			@rowcount int,
			@counter int,
			@hDoc1 int,
			@hDoc2 int,
			@week_day_no int,
			@rc int,
			@ctr int,
			@radiologist_id uniqueidentifier,
			@id uniqueidentifier

	  exec sp_xml_preparedocument @hDoc1 output,@xml_radiologist
	  if(@xml_weekday is not null)  exec sp_xml_preparedocument @hDoc2 output,@xml_weekday

	  set @from_date = convert(datetime, convert(varchar(11),@start_date,106) + ' ' + @start_time)
	  set @till_date   = convert(datetime, convert(varchar(11),@end_date,106) + ' ' + @end_time)

	  if(@from_date > @till_date) set @till_date = dateadd(d,1,@till_date)
	  declare @xfrom_date datetime = convert(datetime, convert(varchar(11),@start_date,106) + ' ' + @start_time)
	  declare @xtill_date datetime = convert(datetime, convert(varchar(11),@start_date,106) + ' ' + @end_time)

	  if(datediff(mi,@xfrom_date,@xtill_date) <=0)
		begin
			select @error_code='195',@return_status=0
			return 0
		end
	 --set @duration = datediff(MILLISECOND,@from_date,@till_date)

	 create table #tmpDates
	 (
		 rec_id int identity(1,1),
		 start_datetime datetime,
		 end_datetime datetime
	 )
	 create table #tmpWeekDays
	 (
		 rec_id int identity(1,1),
		 week_day_no int 
	 )

	-- set @week_day_no = datepart(dw,@from_date)
	-- if(select count(rec_id) from #tmpWeekDays)>0
	--	begin
	--	    set @week_day_no = datepart(dw,@next_start_date)

	--		if(select count(rec_id) from #tmpWeekDays where week_day_no=@week_day_no)>0
	--			begin
	--				insert into #tmpDates(start_datetime,end_datetime) values(@from_date,@till_date)
	--			end
	--	end
	--else
	--	begin
	--		insert into #tmpDates(start_datetime,end_datetime) values(@from_date,@till_date)
	--	end

	 if(@for_next_days='N')
		begin
			set @next_days = datediff(d,@from_date,@till_date)
		end
	 if(@xml_weekday is not null)
		begin
			insert into #tmpWeekDays(week_day_no)
			(select week_day_no
			from openxml(@hDoc2,'weekday/row',2)
			with
			( 
				week_day_no int,
				row_id int
			) xmlTemp)
		end

	set @counter  = 1
	set @rowcount = @next_days
	--set @next_start_date = dateadd(d,1,@from_date)
	--set @next_end_date   = dateadd(d,1,@till_date)
	set @next_start_date =@from_date
	set @next_end_date   =convert(datetime, convert(varchar(11),@start_date,106) + ' ' + @end_time)

	--create date dataset
	while(@counter <= @rowcount)
		begin

			if(select count(rec_id) from #tmpWeekDays)>0
				begin
					 print @next_start_date
					 print @next_end_date
					 set @week_day_no = datepart(dw,@next_start_date)
					 if(select count(rec_id) from #tmpWeekDays where week_day_no=@week_day_no)>0
						begin
							insert into #tmpDates(start_datetime,end_datetime) values(@next_start_date,@next_end_date)
						end
				end
			else
				begin
					insert into #tmpDates(start_datetime,end_datetime) values(@next_start_date,@next_end_date)
				end

			set @next_start_date = dateadd(d,1,@next_start_date)
			set @next_end_date   = dateadd(d,1,@next_end_date)
			set @counter=@counter + 1;
		end
	
	select * from #tmpDates
	begin transaction

	--create records
	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc1,'radiologist/row', 2)  
	with( row_id bigint )

	while(@counter<=@rowcount)
		begin

			select  @radiologist_id = radiologist_id
			from openxml(@hDoc1,'radiologist/row',2)
			with
			( 
				radiologist_id uniqueidentifier,
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter  

			set @ctr  = 1
			select @rc = count(rec_id) from #tmpDates

			while(@ctr <= @rc)
				begin
					select @from_date = start_datetime,
					       @till_date = end_datetime
				    from #tmpDates
					where rec_id = @ctr

					if(select count(id) from radiologist_schedule where start_datetime between @from_date and @till_date and radiologist_id=@radiologist_id)>0
						begin
							delete from radiologist_schedule where start_datetime between @from_date and @till_date and radiologist_id=@radiologist_id
						end 
					if(select count(id) from radiologist_schedule where end_datetime between @from_date and @till_date and radiologist_id=@radiologist_id)>0
						begin
							delete from radiologist_schedule where end_datetime between @from_date and @till_date and radiologist_id=@radiologist_id
						end 

					set @id=newid()
					set @duration = datediff(MILLISECOND,@from_date,@till_date)
				    insert into radiologist_schedule(id,radiologist_id,start_datetime,end_datetime,duration_in_ms,notes,updated_by,date_updated)
					                          values(@id,@radiologist_id,@from_date,@till_date,@duration,@notes,@updated_by,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc1
							if(@xml_weekday is not null)  exec sp_xml_removedocument @hDoc2
							select @error_code='035',@return_status=0
							return 0
						end

					set @ctr =  @ctr + 1
				end


			set @counter= @counter + 1

		end

	commit transaction
	drop table #tmpDates
	drop table #tmpWeekDays
	exec sp_xml_removedocument @hDoc1
	if(@xml_weekday is not null)  exec sp_xml_removedocument @hDoc2
	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
end



GO
