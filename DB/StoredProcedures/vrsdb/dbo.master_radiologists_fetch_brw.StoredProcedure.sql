USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologists_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologists_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologists_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_radiologists_fetch_brw : fetch 
                  Radiologists browser
** Created By   : BK
** Created On   : 24/07/2019
*******************************************************/
CREATE procedure [dbo].[master_radiologists_fetch_brw]
	@code nvarchar(5) ='',
	@name nvarchar(100) ='',
	@acct_group_id int=0,
	@mobile_no nvarchar(20)=0,
	@country_id int=0,
	@state_id int=0,
	@is_active nchar(1)='X',
	@user_id uniqueidentifier,
	@timezone_id int=0

as
begin

	set nocount on
	declare @strSQL varchar(max)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	set @strSQL='select '
	set @strSQL= @strSQL +'rd.id,'
	set @strSQL= @strSQL +'	rd.code,rd.name,'
	set @strSQL= @strSQL +	'isnull(rg.name,'''') as accounts_group,'
	set @strSQL= @strSQL +	'stt.name as state_name,'
	set @strSQL= @strSQL +	'c.name as country_name,'
	set @strSQL= @strSQL +	'd.name as timezone_name,'
	set @strSQL= @strSQL +	'rd.mobile_no,'
	set @strSQL= @strSQL +	'case when rd.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active,action='''' '
	set @strSQL= @strSQL +	'from radiologists rd '
	set @strSQL= @strSQL +	'   left join sys_states stt on stt.id = rd.state_id'
	set @strSQL= @strSQL +	'	left join sys_country c on c.id = rd.country_id '
	set @strSQL= @strSQL +	'	left join sys_us_time_zones d on d.id = rd.timezone_id '
	set @strSQL= @strSQL +	'	left join sys_radiologist_group rg on rg.id = rd.acct_group_id '
	set @strSQL= @strSQL +	'where 1=1'

	if(isnull(@code,'')<>'')
		begin
			set @strSQL= @strSQL + ' and  upper(rd.code) like ''%'+upper(@code)+'%'' ' 
		end
	if(isnull(@name,'')<>'')
		begin
			set @strSQL= @strSQL + ' and upper(rd.name) like ''%'+upper(@name)+'%'' ' 
		end
    if(isnull(@acct_group_id,0) <>0)
		begin
			set @strSQL= @strSQL + ' and  rd.acct_group_id = '+ convert(varchar,@acct_group_id) 
		end
	if (isnull(@mobile_no,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(rd.mobile_no) like ''%'+upper(@mobile_no)+'%'' '
		 end
	if (isnull(@country_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and rd.country_id = '+ convert(varchar,@country_id)
		 end
	if (isnull(@timezone_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and rd.timezone_id = '+ convert(varchar,@timezone_id)
		 end
	if (isnull(@state_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and rd.state_id = '+ convert(varchar,@state_id)
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
