USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_radiologist_assignment_get]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_radiologist_assignment_get]
GO
/****** Object:  StoredProcedure [dbo].[case_list_radiologist_assignment_get]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_radiologist_assignment_get : schedule
                  time slot to get cases
** Created By   : Pavel Guha
** Created On   : 24/05/2021
*******************************************************/
CREATE procedure [dbo].[case_list_radiologist_assignment_get]
	@updated_by uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	
	set nocount on
	declare @radiologist_id uniqueidentifier,
	        @MINTMRADSCHGCASE int,
			@id uniqueidentifier,
			@start_datetime datetime,
			@end_datetime  datetime,
			@duration bigint

  select @radiologist_id = id
  from radiologists 
  where login_user_id=@updated_by

   if(select count(id) 
      from radiologist_schedule 
	  where radiologist_id=@radiologist_id 
	  and start_datetime <= getdate()
	  and end_datetime >= getdate())>0
	begin
		select @return_status=0,@error_code='483'
		return 0
	end

   select @MINTMRADSCHGCASE = data_type_number from general_settings where control_code ='MINTMRADSCHGCASE'

	begin transaction
	set @start_datetime = convert(datetime,convert(varchar(11),getdate(),106) + ' ' + convert(varchar(5),getdate(),114))
	set @end_datetime = dateadd(mi,@MINTMRADSCHGCASE,@start_datetime)
	set @duration = datediff(ms,@start_datetime,@end_datetime)
	set @id=newid()
	insert into radiologist_schedule(id,radiologist_id,start_datetime,end_datetime,duration_in_ms,notes,updated_by,date_updated)
					          values(@id,@radiologist_id,@start_datetime,@end_datetime,@duration,'',@updated_by,getdate())

	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_status=0,@error_code='484'
			return 0
		end

 
	
	exec common_user_activity_log
		 @user_id       = @updated_by,
		 @activity_text = 'Created schedule to get case(s)',
		 @menu_id       = @menu_id,
		 @session_id    = @session_id,
		 @error_code    = @error_code output,
		 @return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end

	commit transaction
	set @return_status=1
	set @error_code='485'
	set nocount off
	return 1

end


GO
