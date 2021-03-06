USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_unlock_user_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_unlock_user_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_unlock_user_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_unlock_user_fetch : fetch locked/unlocked users
** Created By   : BK
** Created On   : 29/07/2019
*******************************************************/
--exec hk_unlockuser_fetch '',''
CREATE procedure [dbo].[hk_unlock_user_fetch]
(
    @user_id uniqueidentifier,
    @code nvarchar(5)=null,
    @name nvarchar(50)=null
)
as
begin
	declare @strSQL varchar(max)
	
	if(select count(record_id) from sys_record_lock where user_id=@user_id)>0
		 begin
			  delete from sys_record_lock where user_id=@user_id
		 end
	if(select count(record_id) from sys_record_lock_ui where user_id=@user_id)>0
		 begin
			  delete from sys_record_lock_ui where user_id=@user_id
		 end
		
	set @strSQL = 'select id=ul.user_id,u.code,u.name,ul.session_id,last_login=convert(varchar(11),ul.last_login,106) + '' '' + convert(varchar(8),ul.last_login,108),unlock='''' '
	set @strSQL = @strSQL + 'from sys_user_lock ul '
	set @strSQL = @strSQL + 'inner join users u on u.id=ul.user_id '
	set @strSQL = @strSQL + 'where ul.user_id <>''' + convert(varchar(36),@user_id) + ''''
	
	
	
	if(RTRIM(LTRIM(ISNULL(@code,'')))<>'') 
		begin
			set @strSQL = @strSQL + ' and upper(u.code) like ''%' + UPPER(RTRIM(LTRIM(@code))) + '%'' '
		end
	if(RTRIM(LTRIM(ISNULL(@name,'')))<>'') 
		begin
			set @strSQL = @strSQL + ' and upper(u.name) like ''%' + UPPER(RTRIM(LTRIM(@name))) + '%'' '
		end
	
	
	set @strSQL = @strSQL + ' order by ul.last_login desc,u.name'
	--print @strSQL
	exec(@strSQL)
	
	
end

GO
