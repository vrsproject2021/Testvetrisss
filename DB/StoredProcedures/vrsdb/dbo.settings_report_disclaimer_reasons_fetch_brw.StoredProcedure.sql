USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_report_disclaimer_reasons_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_report_disclaimer_reasons_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[settings_report_disclaimer_reasons_fetch_brw]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_report_disclaimer_reasons_fetch_brw :  fetch report
                  disclaimer reasons
** Created By   : Pavel Guha
** Created On   : 16/11/2020
*******************************************************/
-- exec report_disclaimer_reasons_fetch_brw '','X','11111111-1111-1111-1111-111111111111',10,'',0
CREATE PROCEDURE [dbo].[settings_report_disclaimer_reasons_fetch_brw] 
	@type nvarchar(30),
	@is_active nchar(1)='X',
    @user_id uniqueidentifier
as
begin
	set nocount on
	declare @strSQL varchar(max)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	
	
	
	set @strSQL ='select id,type,'
	set @strSQL= @strSQL +	'case when is_active=''Y'' then ''Active'' else ''Inactive'' end is_active '
    set @strSQL= @strSQL + 'from report_disclaimer_reasons '
	set @strSQL= @strSQL + 'where 1 = 1 ' 
	
 

	if (isnull(@type,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(type) like ''%'+upper(@type)+'%'' '
		 end
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+'and is_active = '''+ @is_active + ''' '
		 end

    set @strSQL= @strSQL + ' order by type'
	--print @strSQL
	exec(@strSQL)


	set nocount off
end




GO
