USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rate_fee_schedule_template_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rate_fee_schedule_template_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rate_fee_schedule_template_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rate_fee_schedule_template_fetch : fetch fee schedule template
** Created By   : Pavel Guha
** Created On   : 24/06/2019
*******************************************************/
--exec rate_fee_schedule_template_fetch 27,'11111111-1111-1111-1111-111111111111','',0
CREATE procedure [dbo].[rate_fee_schedule_template_fetch]
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	 delete from sys_record_lock where user_id = @user_id
	 delete from sys_record_lock_ui where user_id = @user_id

	 create table #tmp
	 (
		row_id int identity(1,1),
		id uniqueidentifier,
		head_id int,
		head_type nchar(1),
		head_name nvarchar(50),
		img_count_from int ,
		img_count_to int ,
		fee_amount money,
		del nvarchar(1) default ''
	 )

	insert into #tmp(id,head_id,head_name,head_type,img_count_from,img_count_to,fee_amount)
	(select id,head_id,
		   case
				when head_type='M' then (select name from modality where id=rates_fee_schedule_template.head_id)
				when head_type='S' then (select name from services where id=rates_fee_schedule_template.head_id)
		   end head_name,
	       head_type,
		   img_count_from = isnull(img_count_from,0),
		   img_count_to = isnull(img_count_to,0),
		   fee_amount = isnull(fee_amount,0)
	from rates_fee_schedule_template
	where deleted='N')
	 order by head_type,head_name
	
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

	select * from #tmp

	

	drop table #tmp
		
	set nocount off
end

GO
