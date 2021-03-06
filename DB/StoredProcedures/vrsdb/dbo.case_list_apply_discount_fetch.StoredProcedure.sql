USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_apply_discount_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_apply_discount_fetch]
GO
/****** Object:  StoredProcedure [dbo].[case_list_apply_discount_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_apply_discount : 
                  apply discount on a case
** Created By   : Pavel Guha 
** Created On   : 02/01/2020
*******************************************************/

CREATE procedure [dbo].[case_list_apply_discount_fetch]
	@study_id uniqueidentifier,
	@menu_id int,
    @user_id uniqueidentifier,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
	begin
		set nocount on
		declare @activity_text nvarchar(max),
		        @menu_text nvarchar(100),
				@study_uid nvarchar(100),
				@study_cost money

		select id,reason from promo_reasons order by reason

		set @study_cost=0
		exec case_list_study_price_calculate
		     @study_id = @study_id,
		     @study_cost = @study_cost output

		if(select count(id) from study_hdr where id=@study_id)>0
			begin
				select hdr.study_uid,
				       institution_name = isnull(i.name,''),
					   patient_name     = isnull(hdr.patient_name,''),
					   discount_type    = isnull(hdr.discount_type,'N'),
					   discount_per     = isnull(hdr.discount_per,0),
					   discount_amount  = isnull(hdr.discount_amount,0),
					   promo_reason_id  = isnull(hdr.promo_reason_id,'00000000-0000-0000-0000-000000000000'),
					   invoiced         = isnull(hdr.invoiced,'N'),
					   study_cost       = isnull(@study_cost,0)
				from study_hdr hdr
				left outer join institutions i on i.id = hdr.institution_id
				where hdr.id= @study_id

				select @study_uid = study_uid from study_hdr where id=@study_id
			end
		if(select count(id) from study_hdr_archive where id=@study_id)>0
			begin
				select hdr.study_uid,
				       institution_name = isnull(i.name,''),
					   patient_name     = isnull(hdr.patient_name,''),
					   discount_type    = isnull(hdr.discount_type,'N'),
					   discount_per     = isnull(hdr.discount_per,0),
					   discount_amount  = isnull(hdr.discount_amount,0),
					   promo_reason_id  = isnull(hdr.promo_reason_id,'00000000-0000-0000-0000-000000000000'),
					   invoiced         = isnull(hdr.invoiced,'N'),
					   study_cost       = isnull(@study_cost,0)
				from study_hdr_archive hdr
				left outer join institutions i on i.id = hdr.institution_id
				where hdr.id= @study_id

				select @study_uid = study_uid from study_hdr_archive where id=@study_id
			end

		

		if(select count(record_id) from sys_record_lock_ui where record_id=@study_id and menu_id=@menu_id)=0
			begin
				exec common_lock_record_ui
					@menu_id       = @menu_id,
					@record_id     = @study_id,
					@user_id       = @user_id,
					@session_id    = @session_id,
					@error_code    = @error_code output,
					@return_status = @return_status output	
						
				if(@return_status=0)
					begin
						return 0
					end

				set @activity_text =  'Locked for applying/reverting discount'
				exec common_study_user_activity_trail_save
					@study_hdr_id  = @study_id,
					@study_uid     = '',
					@menu_id       = @menu_id,
					@activity_text = @activity_text,
					@session_id    = @session_id,
					@activity_by   = @user_id,
					@error_code    = @error_code output,
					@return_status = @return_status output

				if(@return_status=0)
					begin
						return 0
					end

				select @menu_text= menu_desc from sys_menu where menu_id=@menu_id
				set  @activity_text =  isnull(@menu_text,'')  + '==> Opened & Locked => Study UID ' + @study_uid + ' for applying/reverting discount'
				exec common_user_activity_log
						@user_id       = @user_id,
						@activity_text = @activity_text,
						@session_id    = @session_id,
						@menu_id       = @menu_id,
						@error_code    = @error_code output,
						@return_status = @return_status output

				if(@return_status=0)
					begin
						return 0
					end
			end
		set nocount off

	end
GO
