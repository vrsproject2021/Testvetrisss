USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_billing_account_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[master_billing_account_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_billing_account_fetch_brw : fetch billing account browser
** Created By   : Pavel Guha
** Created On   : 22/10/2019
*******************************************************/
--exec master_billing_account_fetch_brw '','','','',0,0,'Y','11111111-1111-1111-1111-111111111111'
CREATE procedure [dbo].[master_billing_account_fetch_brw]
	@code nvarchar(5) ='',
	@name nvarchar(100) ='',
	@city nvarchar(100)='',
	@zip nvarchar(20)='',
	@country_id int=0,
	@state_id int=0,
	@is_active nchar(1)='X',
	@user_id uniqueidentifier

as
begin

	set nocount on
	declare @strSQL varchar(max)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	set @strSQL='select '
	set @strSQL= @strSQL +'ba.id,'
	set @strSQL= @strSQL +'	ba.code,ba.name,'
	set @strSQL= @strSQL +	'ba.city,'
	set @strSQL= @strSQL +	'stt.name as state_name,'
	set @strSQL= @strSQL +	'c.name as country_name,'
	set @strSQL= @strSQL +	'ba.zip,'
	set @strSQL= @strSQL +	'case when ba.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active,ba.is_new '
	set @strSQL= @strSQL +	'from billing_account ba '
	set @strSQL= @strSQL +	'   left join sys_states stt on stt.id = ba.state_id'
	set @strSQL= @strSQL +	'	left join sys_country c on c.id = ba.country_id '
	set @strSQL= @strSQL +	'where 1=1'

	if(isnull(@code,'')<>'')
		begin
			set @strSQL= @strSQL + ' and  upper(isnull(ba.code,'''')) like ''%'+upper(@code)+'%'' ' 
		end
	if(isnull(@name,'')<>'')
		begin
			set @strSQL= @strSQL + ' and upper(ba.name) like ''%'+upper(@name)+'%'' ' 
		end
    if(isnull(@city,'') <>'')
		begin
			set @strSQL= @strSQL + ' and  upper(isnull(ba.city,'''')) like ''%'+upper(@city)+'%'' ' 
		end
	if (isnull(@zip,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(isnull(ba.zip,'''')) like ''%'+upper(@zip)+'%'' '
		 end
	if (isnull(@country_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and isnull(ba.country_id,0) = '+ convert(varchar,@country_id)
		 end
	if (isnull(@state_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and isnull(ba.state_id,0) = '+ convert(varchar,@state_id)
		 end
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+' and is_active = '''+ @is_active + ''' '
		end
	set @strSQL=@strSQL+' order by code,name'
	--print @strSQL
	exec(@strSQL)
	set nocount off
end

GO
