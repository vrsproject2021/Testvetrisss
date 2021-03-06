USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[profile_institution_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[profile_institution_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[profile_institution_fetch_brw]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : profile_institution_fetch_brw : fetch institution browser
** Created By   : Pavel Guha
** Created On   : 13/02/2020
*******************************************************/
--exec profile_institution_fetch_brw '','','','',0,0,'Y','570D5DFA-4173-4121-99A5-F4D17EF438B7'
create procedure [dbo].[profile_institution_fetch_brw]
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
	declare @strSQL varchar(max),
			@user_role_id int,
	        @user_role_code nvarchar(10)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	create table #tmpInst
	(
	   id uniqueidentifier
	)


	select @user_role_id = u.user_role_id,
	       @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	if(@user_role_code = 'IU')
		begin
			insert into #tmpInst(id)
			(select id
			from institutions 
			where id in (select institution_id
			           from institution_user_link
					   where user_id = @user_id))
			order by name
		end
	else if(@user_role_code = 'AU')
		begin
			insert into #tmpInst(id)
			(select bail.institution_id
			 from billing_account_institution_link bail
			 inner join institutions i on i.id = bail.institution_id
			 inner join billing_account ba on ba.id = bail.billing_account_id
			 where ba.login_user_id = @user_id)
			 order by i.name
		end

	set @strSQL='select '
	set @strSQL= @strSQL +'inst.id,'
	set @strSQL= @strSQL +'	inst.code,inst.name,'
	set @strSQL= @strSQL +	'inst.city,'
	set @strSQL= @strSQL +	'isnull(stt.name,'''') as state_name,'
	set @strSQL= @strSQL +	'isnull(c.name,'''') as country_name,'
	set @strSQL= @strSQL +	'inst.zip,'
	set @strSQL= @strSQL +	'case when inst.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active,inst.is_new '
	set @strSQL= @strSQL +	'from institutions inst '
	set @strSQL= @strSQL +	'left join sys_states stt on stt.id = inst.state_id '
	set @strSQL= @strSQL +	'left join sys_country c on c.id = inst.country_id '
	set @strSQL= @strSQL +	'where inst.id in (select id from #tmpInst)'

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
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+' and is_active = '''+ @is_active + ''' '
		end
	set @strSQL=@strSQL+' order by code,name'
	--print @strSQL
	exec(@strSQL)

	drop table #tmpInst
	set nocount off
end

GO
