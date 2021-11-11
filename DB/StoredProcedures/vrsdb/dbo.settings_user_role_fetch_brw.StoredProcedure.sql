USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_role_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_user_role_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_role_fetch_brw]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_user_role_fetch_brw :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
-- exec settings_user_role_fetch_brw '','','X','11111111-1111-1111-1111-111111111111',10,'',0
CREATE PROCEDURE [dbo].[settings_user_role_fetch_brw] 
    @code nvarchar(10) ='',
    @name nvarchar(50),
	@is_active nchar(1)='X',
    @user_id uniqueidentifier,
	@menu_id int,
	@error_code nvarchar(10)='' output,
	@return_status int =0 output 
as
begin
	set nocount on
	declare @strSQL varchar(max)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	create table #tmp
	(
		rec_id int identity(1,1),
		id int,
		code nvarchar(10),
		name nvarchar(30),
		is_active nchar(1),
		changed nchar(1) null default 'N',
		action nvarchar(1) null default '',
	)
	
	set @strSQL='insert into #tmp(id,code,name,is_active)'
	set @strSQL= @strSQL + '(select id,code,name,is_active '
    set @strSQL= @strSQL + 'from user_roles '
	set @strSQL= @strSQL + 'where is_visible = ''Y''' 
	
 
	if(isnull(@code,'') <>'')
		begin
			set @strSQL= @strSQL + ' and upper(code) like ''%'+upper(@code)+'%'' ' 
		end
	if (isnull(@name,'') <>'')
		 begin
			set @strSQL=@strSQL+' and upper(name) like ''%'+upper(@name)+'%'' '
		 end
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+'and is_active = '''+ @is_active + ''' '
		 end

    set @strSQL= @strSQL + ') order by name'
	--print @strSQL
	exec(@strSQL)
	select * from #tmp

	drop table #tmp

	if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
		begin
			exec common_lock_record
				@menu_id       = @menu_id,
				@record_id     = @menu_id,
				@user_id       = @user_id,
				@error_code    = @error_code output,
				@return_status = @return_status output	
						
			if(@return_status=0)
				begin
					return 0
				end
		end
	select @error_code='',@return_status=1
	set nocount off
end

GO
