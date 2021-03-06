USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_query_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_query_fetch]
GO
/****** Object:  StoredProcedure [dbo].[master_query_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_query_fetch : fetch billing account browser
** Created By   : Pavel Guha
** Created On   : 01/12/2020
*******************************************************/
--exec master_query_fetch '','','Y','','','Y','','Y','11111111-1111-1111-1111-111111111111'
--exec master_query_fetch '','','N','','','Y','','Y','11111111-1111-1111-1111-111111111111'
--exec master_query_fetch '','imaging 4 pets','Y','','','Y','','Y','11111111-1111-1111-1111-111111111111'
--exec master_query_fetch '','','Y','','IMAGING4PETSCHI','Y','','Y','11111111-1111-1111-1111-111111111111'
--exec master_query_fetch '','','Y','','','Y','I4PCHI','Y','11111111-1111-1111-1111-111111111111'
CREATE procedure [dbo].[master_query_fetch]
	@billing_account_code nvarchar(5)='',
	@billing_account_name nvarchar(100)='',
	@billing_account_active nchar(1)='X',
	@institution_code nvarchar(5)='',
	@institution_name nvarchar(100)='',
	@institution_active nchar(1)='X',
	@login_id nvarchar(30)='',
	@user_active nchar(1)='X',
	@user_id uniqueidentifier
as
begin

	set nocount on
	declare @strSQL1 varchar(max),
	        @strSQL2 varchar(max),
			@strSQL3 varchar(max),
			@intBACount int,
			@intInstCount int,
			@intUserCount int


	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	create table #tmpBA
	(
		id uniqueidentifier,
		code nvarchar(5),
		name nvarchar(100),
		login_id nvarchar(50),
		user_email_id nvarchar(50),
		user_mobile_no nvarchar(20),
		is_active nvarchar(10),
		is_new nchar(1)
	)
	create table #tmpInst
	(
	    billing_account_id uniqueidentifier,
		institution_id uniqueidentifier,
		code nvarchar(5),
		name nvarchar(100),
		is_active nvarchar(10),
		is_new nchar(1)
	)
	create table #tmpUser
	(
	    billing_account_id uniqueidentifier,
		institution_id uniqueidentifier,
		user_id uniqueidentifier,
		user_login_id nvarchar(50),
		user_email nvarchar(50),
		user_contact_no nvarchar(20),
		is_active nvarchar(10)

	)

	--BILLING ACCOUNTS
	set @strSQL1='insert into #tmpBA(id,name,code,login_id,user_email_id,user_mobile_no,is_active,is_new) '
	set @strSQL1= @strSQL1 + '(select id,name,code,login_id,user_email_id,user_mobile_no, '
	set @strSQL1= @strSQL1 + 'case when is_active=''Y'' then ''Active'' else ''Inactive'' end is_active,is_new '
	set @strSQL1= @strSQL1 + 'from billing_account '
	set @strSQL1= @strSQL1 + 'where 1=1'

	if(isnull(@billing_account_code,'')<>'')
		begin
			set @strSQL1= @strSQL1 + ' and  upper(isnull(code,'''')) like ''%'+upper(@billing_account_code)+'%'' ' 
		end
	if(isnull(@billing_account_name,'')<>'')
		begin
			set @strSQL1= @strSQL1 + ' and upper(name) like ''%'+upper(@billing_account_name)+'%'' ' 
		end
	if(isnull(@login_id,'')<>'')
		begin
			set @strSQL1= @strSQL1 + ' and  upper(isnull(login_id,'''')) like ''%'+upper(@login_id)+'%'' ' 
		end
	if (isnull(@billing_account_active,'X') <>'X')
		begin
			set @strSQL1=@strSQL1+' and is_active = '''+ @billing_account_active + ''' '
		end
	set @strSQL1=@strSQL1+') order by code,name'

	--print @strSQL1
	exec(@strSQL1)

	set @intBACount = @@rowcount

	--select * from #tmpBA order by name
	

	--INSTITUTIONS
	set @strSQL2='insert into #tmpInst(billing_account_id,institution_id,code,name,is_active,is_new) '
	set @strSQL2= @strSQL2 + '(select bail.billing_account_id,bail.institution_id,i.code,i.name, '
	set @strSQL2= @strSQL2 + 'case when i.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active,i.is_new '
	set @strSQL2= @strSQL2 + 'from billing_account_institution_link bail '
	set @strSQL2= @strSQL2 + 'inner join billing_account ba on ba.id = bail.billing_account_id  '
	set @strSQL2= @strSQL2 + 'inner join institutions i on i.id = bail.institution_id '

	if(@intBACount>0)
		begin
			set @strSQL2= @strSQL2 + 'where bail.billing_account_id in (select id from #tmpBA) '
		end
	else
		begin
			set @strSQL2= @strSQL2 + 'where 1=1 '
		end

	if(isnull(@billing_account_code,'')<>'')
		begin
			set @strSQL2= @strSQL2 + ' and  upper(isnull(ba.code,'''')) like ''%'+upper(@billing_account_code)+'%'' ' 
		end
	if(isnull(@billing_account_name,'')<>'')
		begin
			set @strSQL2= @strSQL2 + ' and upper(ba.name) like ''%'+upper(@billing_account_name)+'%'' ' 
		end
	if (isnull(@billing_account_active,'X') <>'X')
		begin
			set @strSQL2=@strSQL2+' and ba.is_active = '''+ @billing_account_active + ''' '
		end
	if(isnull(@institution_code,'')<>'')
		begin
			set @strSQL2= @strSQL2 + ' and  upper(isnull(i.code,'''')) like ''%'+upper(@institution_code)+'%'' ' 
		end
	if(isnull(@institution_name,'')<>'')
		begin
			set @strSQL2= @strSQL2 + ' and upper(i.name) like ''%'+upper(@institution_name)+'%'' ' 
		end
	if (isnull(@institution_active,'X') <>'X')
		begin
			set @strSQL2=@strSQL2 +' and i.is_active = '''+ @institution_active + ''' '
		end
	set @strSQL2=@strSQL2+') order by i.code,i.name'

	--print @strSQL2
	exec(@strSQL2)
	set @intInstCount = @@rowcount

	--select * from #tmpInst order by name
	--print @intBACount
	--print @intInstCount
	--select * from #tmpBA 
	if(@intBACount=0 and @intInstCount>0)
		begin
			insert into #tmpBA(id,code,name,login_id,user_email_id,user_mobile_no,is_active,is_new)
			(select distinct t.billing_account_id,ba.code,ba.name,ba.login_id,ba.user_email_id,ba.user_mobile_no,
			        case when ba.is_active='Y' then 'Active' else 'Inactive' end is_active,ba.is_new
			from #tmpInst t
			left outer join billing_account ba on ba.id = t.billing_account_id)
		end
	--else if(@intBACount>@intInstCount and @billing_account_active<>'N')
	--	begin
	--		delete from #tmpBA where id not in (select distinct billing_account_id from #tmpInst)
	--	end

    --select * from #tmpInst order by name

	--USERS
	set @strSQL3='insert into #tmpUser(billing_account_id,institution_id,user_id,user_login_id,user_email,user_contact_no,is_active) '
	set @strSQL3= @strSQL3 + '(select distinct i.billing_account_id,iul.institution_id,iul.user_id,iul.user_login_id,iul.user_email,iul.user_contact_no, '
	set @strSQL3= @strSQL3 + 'case when u.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active '
	set @strSQL3= @strSQL3 + 'from institution_user_link iul '
	set @strSQL3= @strSQL3 + 'inner join users u on u.id = iul.user_id '
	set @strSQL3= @strSQL3 + 'inner join institutions i on i.id = iul.institution_id '
	set @strSQL3= @strSQL3 + 'inner join billing_account_institution_link bail on bail.billing_account_id = i.billing_account_id '
	set @strSQL3= @strSQL3 + 'inner join billing_account ba on ba.id = bail.billing_account_id  '


	if(@intInstCount>0)
		begin
			set @strSQL3= @strSQL3 + 'where iul.institution_id in (select institution_id from #tmpInst)'
		end
	else
		begin
			set @strSQL3= @strSQL3 + 'where 1=1 '
		end
	
	if(isnull(@billing_account_code,'')<>'')
		begin
			set @strSQL3= @strSQL3 + ' and  upper(isnull(ba.code,'''')) like ''%'+upper(@billing_account_code)+'%'' ' 
		end
	if(isnull(@billing_account_name,'')<>'')
		begin
			set @strSQL3= @strSQL3 + ' and upper(ba.name) like ''%'+upper(@billing_account_name)+'%'' ' 
		end
	if (isnull(@billing_account_active,'X') <>'X')
		begin
			set @strSQL3=@strSQL3+' and ba.is_active = '''+ @billing_account_active + ''' '
		end
	if(isnull(@institution_code,'')<>'')
		begin
			set @strSQL3= @strSQL3 + ' and  upper(isnull(i.code,'''')) like ''%'+upper(@institution_code)+'%'' ' 
		end
	if(isnull(@institution_name,'')<>'')
		begin
			set @strSQL3= @strSQL3 + ' and upper(i.name) like ''%'+upper(@institution_name)+'%'' ' 
		end
	if (isnull(@institution_active,'X') <>'X')
		begin
			set @strSQL3=@strSQL3 +' and i.is_active = '''+ @institution_active + ''' '
		end
	if(isnull(@login_id,'')<>'')
		begin
			set @strSQL3= @strSQL3 + ' and  upper(isnull(iul.user_login_id,'''')) like ''%'+upper(@login_id)+'%'' ' 
		end
	if (isnull(@user_active,'X') <>'X')
		begin
			set @strSQL3=@strSQL3 +' and u.is_active = '''+ @user_active + ''' '
		end
	set @strSQL3=@strSQL3+') order by iul.user_login_id'

	exec(@strSQL3)
	set @intUserCount=@@rowcount

	
	
	--select * from #tmpUser order by user_login_id
	

	if(@intInstCount=0 and @intUserCount>0)
		begin
			insert into #tmpInst(billing_account_id,institution_id,code,name,is_active,is_new)
			(select distinct t.billing_account_id,t.institution_id,i.code,i.name,
			        case when i.is_active='Y' then 'Active' else 'Inactive' end is_active,i.is_new
			from #tmpUser t
			inner join institutions i on i.id = t.institution_id)
		end
	--else if(@intInstCount>@intUserCount)
	--	begin
			
	--		delete from #tmpInst where institution_id not in (select distinct institution_id from #tmpUser)
	--		delete from #tmpBA where id not in (select distinct billing_account_id from #tmpInst)
	--	end
	
	--print @strSQL3
	select * from #tmpBA order by name
	select * from #tmpInst order by name
	select * from #tmpUser order by user_login_id
	
	
	drop table #tmpBA
	drop table #tmpInst
	drop table #tmpUser
	set nocount off
end

GO
