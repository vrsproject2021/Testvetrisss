USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_institution_fetch_brw : fetch institution browser
** Created By   : Pavel Guha
** Created On   : 24/04/2019
*******************************************************/
--exec master_institution_fetch_brw '','','','',0,0,'Y','11111111-1111-1111-1111-111111111111'
CREATE procedure [dbo].[master_institution_fetch_brw]
	@code nvarchar(5) ='',
	@name nvarchar(100) ='',
	@city nvarchar(100)='',
	@zip nvarchar(20)='',
	@country_id int=0,
	@state_id int=0,
	@dcm_file_xfer_pacs_mode nchar(1)='X',
	@fax_rpt nchar(1)='A',
	@is_active nchar(1)='X',
	@user_id uniqueidentifier

as
begin

	set nocount on
	declare @strSQL varchar(max)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	set @strSQL='select '
	set @strSQL= @strSQL +'inst.id,'
	set @strSQL= @strSQL +'	inst.code,inst.name,'
	set @strSQL= @strSQL +	'inst.city,'
	set @strSQL= @strSQL +	'stt.name as state_name,'
	set @strSQL= @strSQL +	'c.name as country_name,'
	set @strSQL= @strSQL +	'inst.zip,'
	set @strSQL= @strSQL +	'case when inst.dcm_file_xfer_pacs_mode =''M'' then ''Yes'' else ''No'' end dcm_file_xfer_pacs_mode, '
	set @strSQL= @strSQL +	'case when inst.fax_rpt =''Y'' then ''Yes'' else ''No'' end fax_rpt, '
	set @strSQL= @strSQL +	'case when inst.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active,inst.is_new '
	set @strSQL= @strSQL +	'from institutions inst '
	set @strSQL= @strSQL +	'left join sys_states stt on stt.id = inst.state_id '
	set @strSQL= @strSQL +	'left join sys_country c on c.id = inst.country_id '
	set @strSQL= @strSQL +	'where 1=1'

	if(isnull(@code,'')<>'')
		begin
			set @strSQL= @strSQL + ' and  upper(isnull(inst.code,'''')) like ''%'+upper(@code)+'%'' ' 
		end
	if(isnull(@name,'')<>'')
		begin
			set @strSQL= @strSQL + ' and upper(inst.name) like ''%'+upper(@name)+'%'' ' 
		end
    if(isnull(@city,'') <>'')
		begin
			set @strSQL= @strSQL + ' and  upper(isnull(inst.city,'''')) like ''%'+upper(@city)+'%'' ' 
		end
	if (isnull(@zip,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(isnull(inst.zip,'''')) like ''%'+upper(@zip)+'%'' '
		 end
	if (isnull(@country_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and isnull(inst.country_id,0) = '+ convert(varchar,@country_id)
		 end
	if (isnull(@state_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and isnull(inst.state_id,0) = '+ convert(varchar,@state_id)
		 end
	if (isnull(@dcm_file_xfer_pacs_mode,'X') <>'X')
		begin
			set @strSQL=@strSQL+' and dcm_file_xfer_pacs_mode = '''+ @dcm_file_xfer_pacs_mode + ''' '
		end
	if (isnull(@fax_rpt,'A') <>'A')
		begin
			set @strSQL=@strSQL+' and fax_rpt = '''+ @fax_rpt + ''' '
		end
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+' and is_active = '''+ @is_active + ''' '
		end
	set @strSQL=@strSQL+' order by is_new desc,code,name'
	--print @strSQL
	exec(@strSQL)
	set nocount off
end

GO
