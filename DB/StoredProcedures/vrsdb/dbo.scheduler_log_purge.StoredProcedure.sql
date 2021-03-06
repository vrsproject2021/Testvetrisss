USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_log_purge]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_log_purge]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_log_purge]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 
** Procedure    : scheduler_log_purge : save scheduler log
** Created By   : Rajdeep
** Created On   : 21/08/2018
*******************************************************/
--exec scheduler_log_purge '26Feb2018 00:00','26Feb2018 23:59','A','A'
CREATE procedure [dbo].[scheduler_log_purge]
	@from_date datetime,
	@to_date datetime,
	@service_name nvarchar(30)='',
	@log_type nvarchar(5)='A',
	@error_msg nvarchar(100)='' output,
	@return_type int =0 output
as
begin
	set nocount off
    declare @strSQL varchar(8000)
    
    set @strSQL = 'delete '
	set @strSQL = @strSQL + 'from vrslgdb..sys_scheduler_log '
	set @strSQL = @strSQL + 'where log_date between ''' + convert(varchar(11),@from_date,106) + ' ' + convert(varchar(8),@from_date,108) + ''' and ''' + convert(varchar(11),@to_date,106) + ' ' +  convert(varchar(8),@to_date,108) + ''' '
	
	if(@log_type<>'A')
		begin
			if (@log_type='I') set @log_type='False' else if (@log_type='E') set @log_type='True'
			set @strSQL = @strSQL + 'and is_error=''' + @log_type + ''' '
		end
	if(rtrim(ltrim(@service_name))<>'')
		begin
			set @strSQL = @strSQL + 'and service_id = (select service_id from scheduler_data_services where service_name= ''' + rtrim(ltrim(@service_name)) + ''') '
		end
	

	
	--print @strSQL
	exec(@strSQL)
	
	if(@@rowcount=0)
		begin
			select	@error_msg='No log data purged for the selected criteria',@return_type=0
			return 0
		end
	else
		begin
			select	@error_msg='Log purged for the selected criteria',@return_type=1
			return 1
		end
	set nocount off
end
GO
