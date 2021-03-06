USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_radiologist_roaster_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_radiologist_roaster_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_radiologist_roaster_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_radiologist_
l
** Created By   : Pavel Guha
** Created On   : 30/04/2021
*******************************************************/
--exec scheduler_radiologist_roaster_update '',0
create procedure [dbo].[scheduler_radiologist_roaster_update]
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	set nocount on 

	declare @rowcount int,
	        @counter int,
			@rc int,
			@ctr int,
			@radiologist_id uniqueidentifier,
			@name nvarchar(100),
			@max_end_date datetime,
			@mins_scheduled_on_date int,
			@work_unit_on_date int,
			@modality_id int,
			@modality_work_units_per_hour int,
			@modality_work_units_on_date int,
			@log_message varchar(8000)
			

	
	create table #tmpRad
	(
		rec_id int identity(1,1),
		id uniqueidentifier,
		name nvarchar(250) null
	)

	insert into #tmpRad(id,name)
	(select id,name
	 from radiologists
	 where is_active='Y'
	 and id in (select radiologist_id from radiologist_schedule where convert(date,start_datetime)= convert(date,getdate())))

	 select @rowcount = @@rowcount,@counter=1

	 while(@counter<=@rowcount)
		begin
			select @radiologist_id = id,
			       @name           = name
			from #tmpRad
			where rec_id = @counter

			select @max_end_date = max(end_datetime)
			from radiologist_schedule
			where radiologist_id=@radiologist_id
			and convert(date,start_datetime)= convert(date,getdate())

			set @mins_scheduled_on_date=0
			set @work_unit_on_date=0

			if(convert(date,@max_end_date) = convert(date,getdate()))
				begin
					select @mins_scheduled_on_date = sum(datediff(mi,start_datetime,end_datetime))
					from radiologist_schedule
					where radiologist_id=@radiologist_id
					and convert(date,start_datetime)= convert(date,getdate())
				end
			else
				begin
					select @mins_scheduled_on_date = sum(datediff(mi,start_datetime,end_datetime))
					from radiologist_schedule
					where radiologist_id=@radiologist_id
					and convert(date,start_datetime)= convert(date,getdate())
					and convert(date,end_datetime)= convert(date,getdate())

					select @mins_scheduled_on_date = @mins_scheduled_on_date + datediff(mi,start_datetime,convert(datetime,convert(varchar(11),getdate()) + ' 23:59'))
					from radiologist_schedule
					where radiologist_id=@radiologist_id
					and convert(date,start_datetime)= convert(date,getdate())
					and convert(date,end_datetime)> convert(date,getdate())
				end

			select @work_unit_on_date = (max_wu_per_hr) * (@mins_scheduled_on_date/60) from radiologists where id=@radiologist_id

			if((@mins_scheduled_on_date%60)>0)
				begin
					select @work_unit_on_date = @work_unit_on_date + convert(int,(select round((convert(decimal(10,2),max_wu_per_hr)/60) * (@mins_scheduled_on_date%60),0) from radiologists where id=@radiologist_id))
				end

			--print @name
			--print @mins_scheduled_on_date
			--print @work_unit_on_date
			--print '====================================='

			if(@mins_scheduled_on_date>0 and @work_unit_on_date>0)
				begin
					

					begin transaction

					if(select count(radiologist_id) from radiologist_work_unit_balance where scheduled_date=convert(date,getdate()) and radiologist_id=@radiologist_id)=0
						begin
							insert into radiologist_work_unit_balance(scheduled_date,radiologist_id,mins_scheduled_on_date,work_unit_on_date,work_unit_consumed_on_date,work_unit_balance_on_date,date_updated)
																   values(convert(date,getdate()),@radiologist_id,@mins_scheduled_on_date,@work_unit_on_date,0,@work_unit_on_date,getdate())
						end
					else
						begin
							update radiologist_work_unit_balance
							set mins_scheduled_on_date    = @mins_scheduled_on_date,
								work_unit_on_date         = @work_unit_on_date,
								work_unit_balance_on_date = @work_unit_on_date - work_unit_consumed_on_date
							where radiologist_id = @radiologist_id
							and scheduled_date    = convert(date,getdate())
						end

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg   ='',
								   @return_type =1,
								   @log_message = 'Failed to create roaster for ' + @name

							exec scheduler_log_save
								@is_error = 1,
								@service_id = 9,
								@log_message = @log_message,
								@error_msg   = @error_msg output,
								@return_type = @return_type output
						end
					else
						begin
							commit transaction
						end
				end

			set @counter = @counter + 1
		end

	
	 --select * from #tmpRad
	 --select * from radiologist_work_unit_balance
	select @error_msg   ='',@return_type =1
	set nocount off
	drop table #tmpRad

	return 1
end



GO
