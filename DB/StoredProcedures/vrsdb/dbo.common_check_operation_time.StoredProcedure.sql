USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_check_operation_time]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_check_operation_time]
GO
/****** Object:  StoredProcedure [dbo].[common_check_operation_time]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_check_opeartion_time : check record lock
** Created By   : Pavel Guha 
** Created On   : 11/04/2019
*******************************************************/
--exec common_check_operation_time -330,'','','','','',0
CREATE procedure [dbo].[common_check_operation_time]
	@priority_id int,
	@sender_time_offset_mins int,
	@submission_date datetime='01jan1900',
    @next_operation_time nvarchar(130) = '' output,
	@delv_time nvarchar(130) = '' output,
	@display_message nvarchar(500) = '' output,
	@beyond_hour_stat nchar(1) ='N' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 declare @time_zone_id int,
			 @sender_time_zone_id int,
			 @day_no int,
			 @start_from datetime,
			 @end_at datetime,
			 @gmt_diff decimal(5,2),
			 @sender_gmt_diff decimal(5,2),
			 @hr_diff decimal(5,2),
			 @curr_date_time datetime,
			 @next_op_datetime datetime,
			 @next_op_day int,
			 @finishing_time_hrs int
			 

	set datefirst 1

	if(isnull(@submission_date,'01jan1900')='01jan1900') 
		begin
			select @day_no = datepart(dw,getdate())
			select  @start_from   = convert(varchar(11),getdate(),106) + ' ' + from_time,
					@end_at       = convert(varchar(11),getdate(),106) + ' ' + till_time,
					@time_zone_id = time_zone_id,
					@display_message = isnull(message_display,'')
			from settings_operation_time
			where day_no = @day_no
		end
	else 
		begin
			select @day_no = datepart(dw,@submission_date)
			select  @start_from   = convert(varchar(11),@submission_date,106) + ' ' + from_time,
					@end_at       = convert(varchar(11),@submission_date,106) + ' ' + till_time,
					@time_zone_id = time_zone_id,
					@display_message = isnull(message_display,'')
			from settings_operation_time
			where day_no = @day_no
		end

	

	select top 1 @sender_time_zone_id = id
	from sys_us_time_zones
	where gmt_diff_mins = @sender_time_offset_mins

	--select @sender_time_zone_id =  isnull(@sender_time_zone_id,@time_zone_id)

	set @sender_time_offset_mins = -1 * @sender_time_offset_mins

	if(@sender_time_zone_id <> @time_zone_id)
		begin
			--select @gmt_diff = gmt_diff from sys_us_time_zones where id = @time_zone_id
			--select @sender_gmt_diff = convert(decimal(5,2),@sender_time_offset_mins)/60
			--set @hr_diff = (@gmt_diff - @sender_gmt_diff)
			--set @start_from = dateadd(mi,@hr_diff *60,@start_from)
			--set @end_at = dateadd(mi,@hr_diff * 60,@end_at)
			--if(isnull(@submission_date,'01jan1900') = '01jan1900') select @curr_date_time = dateadd(mi,@hr_diff *60,getdate())
			--else select @curr_date_time = dateadd(mi,@hr_diff *60,@submission_date)

			select @curr_date_time = getdate()

		end
	else
		begin
			if(isnull(@submission_date,'01jan1900') = '01jan1900') select @curr_date_time = getdate()
			else select @curr_date_time = @submission_date
		end
						
	
	--print @curr_date_time
	--print @start_from
	--print @end_at

	--if((@curr_date_time < @start_from) or (@curr_date_time > @end_at))
	--if((@curr_date_time not between @start_from and @end_at))
	--	print '111'
	--else 
	--	print '222'
	select @finishing_time_hrs = finishing_time_hrs from sys_priority where priority_id=@priority_id

	if((@curr_date_time not between @start_from and @end_at))
		begin
			set @beyond_hour_stat='Y'
			if((convert(date,@curr_date_time)=convert(date,@start_from)) and (@curr_date_time < @start_from))
				begin
					select @next_op_day = @day_no
				end
			else
				begin
					if(@day_no=7) select @next_op_day = 1
					else select @next_op_day = @day_no + 1
				end
					
			if(@next_op_day=@day_no)
				begin
					select @next_op_datetime = convert(datetime,convert(varchar(11),getdate(),106) + ' ' + (select from_time from settings_operation_time where day_no = @next_op_day))
				end
			else
				begin
					select @next_op_datetime = convert(datetime,convert(varchar(11),dateadd(DAY,1,getdate()),106) + ' ' + (select from_time from settings_operation_time where day_no = @next_op_day))
				end

			

			
			select @next_operation_time = convert(varchar,@next_op_datetime,100),
			       @delv_time       = convert(varchar,dateadd(hour,@finishing_time_hrs,@next_op_datetime),100)
				   --@stat_delv_time      = convert(varchar,dateadd(hour,(select finishing_time_hrs from sys_priority where priority_id=10),@next_op_datetime),100)
			
			select @return_status = 0,
			       @error_code ='427',
			       @next_operation_time=@next_operation_time,
				   @delv_time=@delv_time,
				   --@stat_delv_time=@stat_delv_time,
				   @display_message=@display_message
			--print @next_operation_time
			--print @std_delv_time
			--print @stat_delv_time
			return 0
		end
	else
		begin
			set @beyond_hour_stat='N'
			select @next_operation_time = convert(varchar,convert(datetime,convert(varchar(11),getdate(),106) + ' ' + (select from_time from settings_operation_time where day_no = @day_no)),100)
			select @delv_time       = convert(varchar,dateadd(hour,@finishing_time_hrs,getdate()),100)
		end

   select @return_status=1,@error_code =''
   return 1

end

GO
