USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[radiologist_schedule_save]
GO
/****** Object:  StoredProcedure [dbo].[radiologist_schedule_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : radiologist_schedule_save : save
                  radiologist schedule details fo single instance.
** Created By   : Pavel Guha
** Created On   : 19/09/2019
*******************************************************/
CREATE procedure [dbo].[radiologist_schedule_save]
(
	@id              uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@radiologist_id  uniqueidentifier ='00000000-0000-0000-0000-000000000000',
	@start_date      datetime,
	@start_time      nvarchar(8)   ='00:00:00',
	@end_time        nvarchar(8)   ='00:00:00',
	@notes           nvarchar(250)      = '',
	@updated_by      uniqueidentifier,
	@menu_id         int,
	@error_code		 nvarchar(10)  = '' output,
    @return_status   int		   = 0  output
)
as
begin
	set nocount on 

	declare @from_date datetime,
			@till_date datetime,
	        @duration bigint

	

	  set @from_date = convert(datetime, convert(varchar(11),@start_date,106) + ' ' + @start_time)
	  set @till_date   = convert(datetime, convert(varchar(11),@start_date,106) + ' ' + @end_time)

	  if(datediff(mi,@from_date,@till_date) <=0)
		begin
			select @error_code='195',@return_status=0
			return 0
		end

	 set @duration = datediff(MILLISECOND,@from_date,@till_date)

	

	begin transaction

	if(@id='00000000-0000-0000-0000-000000000000')
		begin
			set @id = newid()
			insert into radiologist_schedule(id,radiologist_id,start_datetime,end_datetime,duration_in_ms,notes,updated_by,date_updated)
			                          values(@id,@radiologist_id,@from_date,@till_date,@duration,@notes,@updated_by,getdate())
		end
	else
		begin
			update radiologist_schedule
			set    radiologist_id = @radiologist_id,
			       start_datetime = @from_date,
				   end_datetime   = @till_date,
				   duration_in_ms = @duration,
				   notes          = @notes,
				   updated_by     = @updated_by,
				   date_updated   = getdate()
			where id = @id
		end

	if(@@rowcount=0)
		begin
			rollback transaction
			select @error_code='196',@return_status=0
			return 0
		end

	commit transaction
	set @return_status=1
	set @error_code='197'
	set nocount off

	return 1
end



GO
