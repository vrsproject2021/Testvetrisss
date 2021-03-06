USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_invoice_process_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_invoice_process_view_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_invoice_process_view_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_invoice_process_view_fetch : 
                  fetch invoicing processing details 
				  if locked by other user
** Created By   : Pavel Guha 
** Created On   : 12/11/2019
*******************************************************/
--exec ar_invoice_process_view_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','1A1D9DEE-8D88-4ACB-9861-B0A254C30E34',45,'11111111-1111-1111-1111-111111111111','','',0
--exec ar_invoice_process_view_fetch 'D2B3965C-73F1-4BB5-AD9A-31003C1A869E','00000000-0000-0000-0000-000000000000'
--exec ar_invoice_process_view_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','98A30426-BA37-4564-98D0-A2CF6A4B9929',45,'11111111-1111-1111-1111-111111111111','','',0
CREATE procedure [dbo].[ar_invoice_process_view_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@menu_id int,
	@user_id uniqueidentifier,
	@session_id uniqueidentifier
as
	begin
		set nocount on

     	declare @arch_db_name nvarchar(30),
		        @return_status int,
				@error_code nvarchar(10),
				@activity_text nvarchar(max)

		select @arch_db_name = arch_db_name from billing_cycle where id =@billing_cycle_id
		
		if(isnull(@arch_db_name,'')<>'')
			begin
				exec ar_invoice_archive_dtls_fetch
					@billing_cycle_id   = @billing_cycle_id,
					@billing_account_id = @billing_account_id,
					@arch_db_name       = @arch_db_name
			end
		else
			begin
				select ih.billing_account_id,
					   ih.billing_cycle_id,
					   billing_account_name = dbo.InitCap(replace(ba.name,char(39),'')),
					   ih.total_study_count,
					   ih.total_study_count_std,
					   ih.total_study_count_stat,
					   ih.total_amount,
					   ih.approved,
					   ih.total_disc_amount,
					   ih.total_free_credits,
					   action=''
				from invoice_hdr ih
				inner join billing_account ba on ba.id = ih.billing_account_id
				where ih.billing_cycle_id = @billing_cycle_id
				and ih.billing_account_id=@billing_account_id
				order by ba.name

				select iih.billing_account_id,
					   iih.billing_cycle_id,
					   iih.institution_id,
					   institution_code = i.code,
					   institution_name = dbo.InitCap(replace(i.name,char(39),'')),
					   iih.total_study_count,
					   iih.total_study_count_std,
					   iih.total_study_count_stat,
					   iih.total_disc_amount,
					   iih.free_read_count,
					   iih.total_amount,
					   iih.approved,
					   action='' 
				from invoice_institution_hdr iih
				inner join institutions i on i.id = iih.institution_id
				where iih.billing_cycle_id = @billing_cycle_id
				and iih.billing_account_id=@billing_account_id
				order by i.name

				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.category_id,
					   category_name =  dbo.InitCap(isnull(c.name,'Unkown')),
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   p.priority_desc,
					   iid.image_count,
					   object_count = isnull(sh.object_count,iid.image_count),
					   iid.rate,
					   iid.amount,
					   service_amount = isnull(iid.service_amount,0),
					   total_amount   = isnull(iid.total_amount,0),
					   iid.billed,
					   iid.approved,
					    case
							when iid.is_free='Y' then 'Free credit applied'
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),iid.disc_per_applied) + '% ($' + convert(varchar(12),iid.disc_amount)	 + ') applied'
							when iid.disc_amt_applied>0 then 'Discount of $' + convert(varchar(12),iid.disc_amount)	 + ' applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join sys_study_category c on c.id= iid.category_id
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.billing_account_id=@billing_account_id
				union
				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.category_id,
					   category_name =  dbo.InitCap(isnull(c.name,'Unkown')),
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   p.priority_desc,
					   iid.image_count,
					   object_count = isnull(sh.object_count,iid.image_count),
					   iid.rate,
					   iid.amount,
					   service_amount = isnull(iid.service_amount,0),
					   total_amount   = isnull(iid.total_amount,0),
					   iid.billed,
					   iid.approved,
					    case
							when iid.is_free='Y' then 'Free credit applied'
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),iid.disc_per_applied) + '% ($' + convert(varchar(12),iid.disc_amount)	 + ') applied'
							when iid.disc_amt_applied>0 then 'Discount of $' + convert(varchar(12),iid.disc_amount)	 + ' applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join sys_study_category c on c.id= iid.category_id
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr_archive sh on sh.id = iid.study_id
				inner join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.billing_account_id=@billing_account_id
				order by sh.received_date	
				
				if(select count(record_id) 
				   from sys_record_lock_ui 
				   where record_id = @billing_cycle_id 
				   and addl_record_id_ui = @billing_account_id 
				   and session_id = @session_id  
				   and menu_id=@menu_id)=0
					begin
						exec common_lock_record_ui
							  @menu_id           = @menu_id,
							  @record_id         = @billing_cycle_id,
							  @addl_record_id_ui = @billing_account_id,
							  @session_id        = @session_id,
							  @user_id           = @user_id,
							  @error_code        = @error_code output,
						      @return_status     = @return_status output	
						
						if(@return_status=0)
							begin
								return 0
							end
						
						select @activity_text = 'Processed and locked invoice of ' + (select name from billing_account where id = @billing_account_id) 
						select @activity_text = @activity_text + ' for ' + (select name from billing_cycle where id = @billing_cycle_id) 

						exec common_user_activity_log
							@user_id       = @user_id,
							@activity_text = @activity_text,
							@menu_id       = @menu_id,
							@session_id    = @session_id,
							@error_code    = @error_code output,
							@return_status = @return_status output


					end	
	        end
		

		
		
		set nocount off

	end
GO
