USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_physician_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_physician_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[master_physician_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_physician_fetch_brw : fetch physician browser
** Created By   : Pavel Guha
** Created On   : 24/04/2019
*******************************************************/
create procedure [dbo].[master_physician_fetch_brw]
	@code nvarchar(5) ='',
	@name nvarchar(100) ='',
	@city nvarchar(100)='',
	@mobile_no nvarchar(20)=0,
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
	set @strSQL= @strSQL +'ph.id,'
	set @strSQL= @strSQL +'	ph.code,ph.name,'
	set @strSQL= @strSQL +	'ph.city,'
	set @strSQL= @strSQL +	'stt.name as state_name,'
	set @strSQL= @strSQL +	'c.name as country_name,'
	set @strSQL= @strSQL +	'ph.mobile_no,'
	set @strSQL= @strSQL +	'case when ph.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active '
	set @strSQL= @strSQL +	'from physicians ph '
	set @strSQL= @strSQL +	'   left join sys_states stt on stt.id = ph.state_id'
	set @strSQL= @strSQL +	'	left join sys_country c on c.id = ph.country_id '
	set @strSQL= @strSQL +	'where 1=1'

	if(isnull(@code,'')<>'')
		begin
			set @strSQL= @strSQL + ' and  upper(ph.code) like ''%'+upper(@code)+'%'' ' 
		end
	if(isnull(@name,'')<>'')
		begin
			set @strSQL= @strSQL + ' and upper(ph.name) like ''%'+upper(@name)+'%'' ' 
		end
    if(isnull(@city,'') <>'')
		begin
			set @strSQL= @strSQL + ' and  upper(ph.city) like ''%'+upper(@city)+'%'' ' 
		end
	if (isnull(@mobile_no,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(ph.mobile_no) like ''%'+upper(@mobile_no)+'%'' '
		 end
	if (isnull(@country_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and ph.country_id = '+ convert(varchar,@country_id)
		 end
	if (isnull(@state_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and ph.state_id = '+ convert(varchar,@state_id)
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
