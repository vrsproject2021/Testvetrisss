USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_log_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_log_fetch]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_log_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 5.29.92.2
** Procedure    : scheduler_log_fetch : save scheduler log
** Created By   : Pavel Guha
** Created On   : 26/02/2018
*******************************************************/
--exec scheduler_log_fetch '01Apr2021 00:00','05Jun2021 23:59','','A'
CREATE procedure [dbo].[scheduler_log_fetch]
	@from_date datetime,
	@to_date datetime,
	@service_name nvarchar(50)='',
	@log_type nvarchar(5)='A'
as
begin
	set nocount on
    declare @strSQL varchar(8000)
    
    set @strSQL = 'select log_date = convert(varchar(10),sl.log_date,103),log_time = convert(varchar(8),sl.log_date,108),ss.service_name,'
	set @strSQL = @strSQL + ' case when sl.is_error=''False'' then ''Information'' when sl.is_error=''True'' then ''Error'' end log_type,'
	set @strSQL = @strSQL + ' sl.log_message'
	set @strSQL = @strSQL + ' from vrslogdb..sys_scheduler_log sl'
	set @strSQL = @strSQL + ' inner join scheduler_data_services ss on sl.service_id= ss.service_id'
	set @strSQL = @strSQL + ' where sl.log_date between ''' + convert(varchar(11),@from_date,106) + ' ' + convert(varchar(8),@from_date,108) + ''' and ''' + convert(varchar(11),@to_date,106) + ' ' +  convert(varchar(8),@to_date,108) + ''' '
	
	if(@log_type<>'A')
		begin
			if (@log_type='I') set @log_type='False' else if (@log_type='E') set @log_type='True'
			set @strSQL = @strSQL + ' and sl.is_error=''' + @log_type + ''' '
		end
	if(rtrim(ltrim(@service_name))<>'')
		begin
			set @strSQL = @strSQL + ' and ss.service_name = ''' + rtrim(ltrim(@service_name)) + ''' '
		end
	
	set @strSQL = @strSQL + ' order by sl.log_date desc'
	
	--print @strSQL
	exec(@strSQL)
	set nocount off
end
GO
