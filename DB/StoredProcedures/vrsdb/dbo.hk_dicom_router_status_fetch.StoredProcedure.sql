USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_dicom_router_status_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_dicom_router_status_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_dicom_router_status_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_dicom_router_status_fetch :  fetch case(s)
				  requiring action
** Created By   : Pavel Guha
** Created On   : 29/01/2020
*******************************************************/
-- exec hk_dicom_router_status_fetch '11111111-1111-1111-1111-111111111111'
create procedure [dbo].[hk_dicom_router_status_fetch]
	@user_id uniqueidentifier 
as
begin
	set nocount on
	declare @OLSTATTIME int

	create table #tmp
	(
		rec_id bigint identity(1,1),
		is_online nchar(1),
		institution_id uniqueidentifier,
		institution_code nvarchar(5),
		institution_name nvarchar(100),
		version_no nvarchar(50),
		last_updated_on datetime
	)

	delete from sys_record_lock where user_id = @user_id
	delete from sys_record_lock_ui where user_id = @user_id

	select @OLSTATTIME= data_type_number
	from general_settings
	where control_code = 'OLSTATTIME'
	 

    insert into #tmp(is_online,institution_id,institution_code,institution_name,version_no,last_updated_on)
	(select case 
				when datediff(mi,dros.last_updated_on,getdate())> @OLSTATTIME then 'N' else 'Y'
			end is_online,
	dros.institution_id,i.code,dbo.InitCap(i.name),dros.version_no,dros.last_updated_on
	from sys_dicom_router_online_status dros
	inner join institutions i on i.id = dros.institution_id
	where 1=1)
	order by i.code

	

	select * from #tmp

	drop table #tmp

	set nocount off
end


GO
