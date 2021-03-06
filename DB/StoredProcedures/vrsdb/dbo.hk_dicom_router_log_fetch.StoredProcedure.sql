USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_dicom_router_log_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_dicom_router_log_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_dicom_router_log_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_dicom_router_log_fetch :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 10/04/2019
*******************************************************/
-- exec hk_dicom_router_log_fetch '02Sep2019','09Sep2019','00000000-0000-0000-0000-000000000000',2,'X','11111111-1111-1111-1111-111111111111'
CREATE procedure [dbo].[hk_dicom_router_log_fetch] 
	@date_from datetime='01Jan1900',
	@date_till datetime='01Jan1900',
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@service_id int=0,
	@type nchar(1)='X',
    @user_id uniqueidentifier
as
begin
	set nocount on
	declare @strSQL varchar(max)

	create table #tmp
	(
		rec_id bigint identity(1,1),
		is_error bit,
		log_date datetime,
		institution_name nvarchar(100),
		service_name nvarchar(100),
		log_message varchar(max)
	)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id
	 

    set @strSQL= 'insert into #tmp(is_error,log_date,institution_name,service_name,log_message)'
	set @strSQL= @strSQL + '(select is_error,log_date,institution_name,service_name,log_message '
    set @strSQL= @strSQL + 'from vrslogdb..sys_dicom_router_log hdr '
	set @strSQL= @strSQL + 'where log_date between '''+ convert(varchar(11),@date_from,106) + ' 00:00:00' + ''' and ''' + convert(varchar(11),@date_till,106) + ' 23:59:59' + ''' '

	if (isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <>'00000000-0000-0000-0000-000000000000')
		begin
			set @strSQL=@strSQL+' and institution_id = '''+ convert(varchar(36),@institution_id) + ''' '
		end	
	if (isnull(@service_id,0) > 0)
		begin
			set @strSQL=@strSQL+' and service_id = '+ convert(varchar,@service_id) 
		end
	if (isnull(@type,'X') <>'X')
		begin
			if(@type= 'Y')
				begin
					set @strSQL=@strSQL+'and is_error = ''true'' '
				end
			else if(@type= 'Y')
				begin
					set @strSQL=@strSQL+'and is_error = ''false'' '
				end
		 end

    set @strSQL= @strSQL + ') order by log_date desc'
	--print @strSQL
	exec(@strSQL)

	select * from #tmp

	drop table #tmp

	set nocount off
end


GO
