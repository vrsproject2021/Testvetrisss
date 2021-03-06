USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_billing_cycle_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_billing_cycle_fetch_brw]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_billing_cycle_fetch_brw]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_billing_cycle_fetch_brw : fetch 
                  bill cycle browser
** Created By   : BK
** Created On   : 07/11/2019
*******************************************************/
create procedure [dbo].[invoicing_billing_cycle_fetch_brw]
(
	
	@name nvarchar(150) ='',
	@menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
)
as
	begin
		set nocount on
		declare @strSQL varchar(max)

		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id

		set @strSQL='select '
		set @strSQL= @strSQL +'bc.id,'
		set @strSQL= @strSQL +'	bc.name,'
		set @strSQL= @strSQL +	'bc.date_from,'
		set @strSQL= @strSQL +	'bc.date_till,'
		set @strSQL= @strSQL +'	bc.locked  '
		set @strSQL= @strSQL +	'from billing_cycle bc '
		set @strSQL= @strSQL +	'where 1=1'

		if(isnull(@name,'')<>'')
			begin
				set @strSQL= @strSQL + ' and upper(bc.name) like ''%'+upper(@name)+'%'' ' 
			end
	   set @strSQL=@strSQL+' order by bc.date_from desc'

		--print @strSQL
		exec(@strSQL)
		set nocount off
	end
GO
