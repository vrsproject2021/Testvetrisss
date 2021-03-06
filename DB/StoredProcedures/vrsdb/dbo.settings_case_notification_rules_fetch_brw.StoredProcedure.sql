USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rules_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_case_notification_rules_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[settings_case_notification_rules_fetch_brw]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_case_notification_rules_fetch_brw :  fetch
				  notification rules
** Created By   : Pavel Guha
** Created On   : 26/09/2019
*******************************************************/
-- exec settings_case_notification_rules_fetch_brw -999,0,'X','11111111-1111-1111-1111-111111111111'
CREATE PROCEDURE [dbo].[settings_case_notification_rules_fetch_brw]
	@status_id int=-999,
	@prority_id int=0, 
	@is_active nchar(1)='X',
    @user_id uniqueidentifier
	
as
begin
	set nocount on
	declare @strSQL varchar(max)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	set @strSQL='select cnr.rule_no,cnr.rule_desc,cnr.pacs_status_id,stat.status_desc,cnr.priority_id,p.priority_desc,'
	set @strSQL= @strSQL +	'case when cnr.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active '
	set @strSQL= @strSQL +	'from case_notification_rule_hdr cnr '
	set @strSQL= @strSQL +	'inner join sys_study_status_pacs stat on stat.status_id = cnr.pacs_status_id '
	set @strSQL= @strSQL +	'inner join sys_priority p on p.priority_id = cnr.priority_id '
	set @strSQL= @strSQL +	'where 1=1'

	if (isnull(@status_id,-999) <>-999)
		begin
			set @strSQL=@strSQL+' and cnr.pacs_status_id = ' + convert(varchar,@status_id)
		end
	if (isnull(@prority_id,0) <>0)
		begin
			set @strSQL=@strSQL+' and cnr.priority_id = ' + convert(varchar,@prority_id)
		end
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+' and cnr.is_active = '''+ @is_active + ''' '
		end
	set @strSQL=@strSQL+' order by cnr.rule_no '

	exec(@strSQL)
	
	set nocount off
end

GO
