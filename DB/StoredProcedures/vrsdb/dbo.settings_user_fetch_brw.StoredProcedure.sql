USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[settings_user_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[settings_user_fetch_brw]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : settings_user_fetch_brw : fetch institution devices
** Created By   : Pavel Guha
** Created On   : 24/04/2019
*******************************************************/
--exec settings_user_fetch_brw '','waller','','','','A',0,'Y','11111111-1111-1111-1111-111111111111',12,'7851f4fd-721f-4c46-9dd7-92d4ed052790'
CREATE procedure [dbo].[settings_user_fetch_brw]
	@code nvarchar(5) ='',
	@name nvarchar(100) ='',
	@login_id nvarchar(100)='',
	@institution_name nvarchar(100)='',
	@billing_account_name nvarchar(100)='',
	@allow_manual_submission nchar(1)='A',
	@user_role_id int=0,
	@is_active nchar(1)='X',
	@user_id uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000'

as
begin

	set nocount on
	declare @strSQL varchar(max)

	declare @record_id uniqueidentifier,
	        @menu_text nvarchar(100),
		    @activity_text nvarchar(max),
			@error_code nvarchar(10),
		    @return_status int

	if(select count(record_id) from sys_record_lock_ui where menu_id =@menu_id and user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id)>0
		begin
			select @record_id =record_id from sys_record_lock_ui where menu_id =@menu_id and user_id=@user_id and isnull(session_id,'00000000-0000-0000-0000-000000000000')=@session_id
			

			set @activity_text =  @menu_text  + '==> Lock released =>  ' + (select name from users where id=@record_id)
			set @error_code=''
			set @return_status=0	
				
			exec common_user_activity_log
				@user_id       = @user_id,
				@activity_text = @activity_text,
				@menu_id       = @menu_id,
				@session_id    = @session_id,
				@error_code    = @error_code output,
				@return_status = @return_status output
		end

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	select @menu_text= menu_desc from sys_menu where menu_id=@menu_id

	set @strSQL='select '
	set @strSQL= @strSQL +'u.id,'
	set @strSQL= @strSQL + 'u.code,u.name,'
	set @strSQL= @strSQL +	'u.login_id,'
	set @strSQL= @strSQL +	'ur.name as user_role_name,'
	set @strSQL= @strSQL +	'case '
	set @strSQL= @strSQL +  'when ur.code=''IU'' then isnull(i.name,'''') else '''' '
	set @strSQL= @strSQL +	'end institution_name,'
	set @strSQL= @strSQL +	'case '
	set @strSQL= @strSQL +  'when ur.code=''AU'' then isnull(ba.name,'''') '
	set @strSQL= @strSQL +  'else '''' '
	set @strSQL= @strSQL +	'end billing_account_name,'
	set @strSQL= @strSQL +	'case when u.allow_manual_submission=''Y'' then ''Yes'' else ''No'' end allow_manual_submission, '
	set @strSQL= @strSQL +	'case when u.is_active=''Y'' then ''Active'' else ''Inactive'' end is_active '
	set @strSQL= @strSQL +	'from users u '
	set @strSQL= @strSQL +	'inner join user_roles ur on ur.id = u.user_role_id '
	set @strSQL= @strSQL +	'left outer join institution_user_link iul on iul.user_id = u.id '
	set @strSQL= @strSQL +	'left outer join institutions i on i.id = iul.institution_id '
	set @strSQL= @strSQL +	'left outer join billing_account ba on ba.login_user_id = u.id '
	set @strSQL= @strSQL +	'where u.is_visible=''Y'''

	if(isnull(@code,'')<>'')
		begin
			set @strSQL= @strSQL + ' and upper(u.code) like ''%'+upper(@code)+'%'' ' 
		end
	if(isnull(@name,'')<>'')
		begin
			set @strSQL= @strSQL + ' and  upper(u.name) like ''%'+upper(@name)+'%'' ' 
		end
    if(isnull(@login_id,'') <>'')
		begin
			set @strSQL= @strSQL + ' and  upper(u.login_id) like ''%'+upper(@login_id)+'%'' ' 
		end
	if (isnull(@user_role_id,0) <>0)
		 begin
			set @strSQL=@strSQL+' and u.user_role_id = '+ convert(varchar,@user_role_id)
		 end
	if (isnull(@institution_name,'') <>'')
		begin
			set @strSQL=@strSQL+'and i.name like ''%'+ @institution_name + '%'' '
		end
	if (isnull(@billing_account_name,'') <>'')
		begin
			set @strSQL=@strSQL+'and ba.name like ''%'+ @billing_account_name + '%'' '
		end
	if (isnull(@allow_manual_submission,'A') <>'A')
		begin
			set @strSQL=@strSQL+'and u.allow_manual_submission = '''+ @allow_manual_submission + ''' '
		end
	if (isnull(@is_active,'X') <>'X')
		begin
			set @strSQL=@strSQL+'and u.is_active = '''+ @is_active + ''' '
		end

	set @strSQL=@strSQL+' order by name'
	
	--print @strSQL
	exec(@strSQL)

	set @activity_text =  @menu_text  + '==> User list loaded'
	set @error_code=''
	set @return_status=0

	exec common_user_activity_log
		@user_id       = @user_id,
		@activity_text = @activity_text,
		@menu_id       = @menu_id,
		@session_id    = @session_id,
		@error_code    = @error_code output,
		@return_status = @return_status output

	set nocount off
end

GO
