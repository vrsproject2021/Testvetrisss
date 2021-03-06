USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[operation_time_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[operation_time_save]
GO
/****** Object:  StoredProcedure [dbo].[operation_time_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : operation_time_save : save operation time
** Created By   : Pavel Guha 
** Created On   : 19/11/2020
*******************************************************/
CREATE Procedure [dbo].[operation_time_save]
	@xml_data      ntext,
	@menu_id       int,
	@updated_by    uniqueidentifier,
	@user_name     nvarchar(500) = '' output,
	@error_code    nvarchar(10)='' output,
	@return_status int =0 output
As
	Begin
	set nocount on
	set datefirst 1

	declare @hDoc int,
			@counter int,
	        @rowcount int

	declare @day_no int,
			@from_time nvarchar(5),
		    @till_time nvarchar(5),
			@from_datetime datetime,
		    @till_datetime datetime,
			@time_zone_id int,
			@message_display nvarchar(500)

	exec common_check_record_lock
			@menu_id       = @menu_id,
			@record_id     = @menu_id,
			@user_id       = @updated_by,
			@user_name     = @user_name output,
			@error_code    = @error_code output,
			@return_status = @return_status output
		
	if(@return_status=0)
		begin
			return 0
		end

		begin transaction  
		exec sp_xml_preparedocument @hDoc output,@xml_data
		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc,'OT/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @day_no			= day_no,
						   @from_time       = from_time,
						   @till_time		= till_time,
						   @time_zone_id    = time_zone_id,
						   @message_display = message_display
					from openxml(@hDoc,'OT/row',2)
					with
					( 
						day_no int,
						from_time  nvarchar(5),
						till_time  nvarchar(5),
						time_zone_id int,
						message_display nvarchar(500),
						row_id int
					) xmlTemp where xmlTemp.row_id = @counter  

					select @from_datetime = convert(datetime,convert(varchar(11),getdate(),106) + ' ' + @from_time)
					select @till_datetime = convert(datetime,convert(varchar(11),getdate(),106) + ' ' + @till_time)

					if(@from_datetime >= @till_datetime)
						begin
							rollback transaction
							select @user_name = day_name from settings_operation_time where day_no = @day_no
							select @error_code='426',@return_status=0
							return 0
						end

					if(isnull(@message_display,'')<> '')
						begin
							if(LEN(@message_display) > 500)
								begin
									rollback transaction
									select @user_name = day_name from settings_operation_time where day_no = @day_no
									select @error_code='428',@return_status=0
									return 0
								end
						end

					update settings_operation_time
					set from_time       = @from_time,
						till_time       = @till_time,
						time_zone_id    = @time_zone_id,
						message_display = @message_display,
						updated_by      = @updated_by,
						date_updated    = getdate()
					where day_no = @day_no

					if(@@rowcount=0)
						begin
							rollback transaction
							select @user_name = day_name from settings_operation_time where day_no = @day_no
							select @error_code='425',@return_status=0
							return 0
						end

					set @counter = @counter + 1
			end

			if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
				begin
					exec common_lock_record
						@menu_id       = @menu_id,
						@record_id     = @menu_id,
						@user_id       = @updated_by,
						@error_code    = @error_code output,
						@return_status = @return_status output	
						
					if(@return_status=0)
						begin
							return 0
						end
				end

		commit transaction  
		exec sp_xml_removedocument @hDoc

	    set @return_status=1
	    set @error_code='034'
		set nocount off
		return 1
	End
GO
