USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_user_activity_log_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_user_activity_log_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_user_activity_log_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_user_activity_log_fetch :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 17/05/2021
*******************************************************/
-- exec hk_user_activity_log_fetch '17May2021','17May2021','11111111-1111-1111-1111-111111111111',''
CREATE procedure [dbo].[hk_user_activity_log_fetch] 
	@date_from datetime='01Jan1900',
	@date_till datetime='01Jan1900',
	@user_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@activity_text nvarchar(max)=''
as
begin
	set nocount on
	declare @strSQL varchar(max)

	create table #tmp
	(
		rec_id bigint identity(1,1),
		log_date datetime,
		user_name nvarchar(100),
		log_message varchar(max),
		session_id uniqueidentifier
	)

    set @strSQL= 'insert into #tmp(log_date,user_name,log_message,session_id)'
	set @strSQL= @strSQL + '(select ual.activity_datetime,user_name= name,ual.activity_text,session_id = isnull(ual.session_id,''00000000-0000-0000-0000-000000000000'') '
    set @strSQL= @strSQL + 'from vrslogdb..sys_user_activity_log ual '
	set @strSQL= @strSQL + 'inner join users u on u.id= ual.user_id '
	set @strSQL= @strSQL + 'where ual.activity_datetime between '''+ convert(varchar(11),@date_from,106) + ' 00:00:00' + ''' and ''' + convert(varchar(11),@date_till,106) + ' 23:59:59' + ''' '

	if (isnull(@user_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and ual.user_id = '''+ convert(varchar(36),@user_id) + ''' '
		end	
    if (rtrim(ltrim(isnull(@activity_text,''))) <>'')
		begin
			set @strSQL=@strSQL+' and ual.activity_text like ''%'+ @activity_text + '%'' '
		end	
    set @strSQL= @strSQL + ') order by ual.activity_datetime '
	--print @strSQL
	exec(@strSQL)

	select * from #tmp order by log_date

	drop table #tmp

	set nocount off
end


GO
