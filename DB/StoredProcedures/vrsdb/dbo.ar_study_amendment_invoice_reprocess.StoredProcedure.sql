USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_amendment_invoice_reprocess]    Script Date: 27-08-2021 15:29:25 ******/
DROP PROCEDURE [dbo].[ar_study_amendment_invoice_reprocess]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_amendment_invoice_reprocess]    Script Date: 27-08-2021 15:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_study_amendment_invoice_reprocess : 
                  fetch invoicing processing details
** Created By   : Pavel Guha 
** Created On   : 04/03/2021
*******************************************************/
--exec ar_study_amendment_invoice_reprocess '2353a24b-357f-482a-81cc-6a3ef38d7625','AB352297-C935-4E57-9CA9-D7C146B15914',45,'11111111-1111-1111-1111-111111111111','','',0
CREATE procedure [dbo].[ar_study_amendment_invoice_reprocess]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier,
	@menu_id int,
    @user_id uniqueidentifier,
    @user_name nvarchar(500) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
	begin
		set nocount on
		declare @date_from datetime,
				@date_till datetime,
				@counter bigint,
				@counter1 bigint,
				@counter2 bigint,
				@rowcount bigint,
				@rowcount1 bigint,
				@rowcount2 bigint,
				@billing_acc_id uniqueidentifier,
				@institution_id uniqueidentifier,
				@total_study_count int,
				@total_study_count_std int,
				@total_study_count_stat int,
				@total_amount money,
				@total_disc_amount money,
				@total_free_credits int,
				@recived_date datetime,
				@disc_per decimal(5,2),
				@free_credits int,
				@free_credits_consumed int,
				@free_credits_consumed_tmp int,
				@is_free nchar(1),
				@promotion_id_disc uniqueidentifier,
				@promotion_id_fc uniqueidentifier,
				@promotion_id uniqueidentifier,
				@service_total money,
				@consult_amount money,
				@amend_hdr_id uniqueidentifier

		declare @inv_id uniqueidentifier,
				@inv_inst_hdr_id uniqueidentifier,
				@inv_inst_dtls_id uniqueidentifier,
				@study_id uniqueidentifier,
				@study_uid nvarchar(100),
				@category_id int,
				@modality_id bigint,
				@charge_by nchar(1),
				@img_count bigint,
				@max_count int,
				@rate money,
				@rate_per_unit money,
				@study_max_amount money,
				@amount money,
				@study_price money,
				@service_price money,
				@bill_cycle_id uniqueidentifier,
				@bill_account_id uniqueidentifier,
				@inst_id uniqueidentifier,
				@approved nchar(1),
				@approved_by uniqueidentifier,
				@date_approved datetime,
				@service_code nvarchar(250),
				@status_id int,
				@priority_id int,
				@priority_charged nchar(1),
				@beyond_hour_stat nchar(1),
				@svc_id int,
				@svc_code nvarchar(10),
				@service_amount money,
				@service_amount_after_hrs money,
				@disc_amount money,
				@CALCMINUTEFACT int,
				@study_seconds int,
				@modality_amended_rate nchar(1),
				@rc int,
				@ctr int

		exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @billing_cycle_id,
				@user_id       = @user_id,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
		if(@return_status=0)
			begin
				exec invoicing_process_view_fetch
					@billing_cycle_id   = @billing_cycle_id,
					@billing_account_id = @billing_account_id

			    return 0
			end

		create table #tmpInvHdr
		(
			rec_id int identity(1,1),
			billing_cycle_id uniqueidentifier,
			billing_account_id uniqueidentifier,
			billing_acc_name nvarchar(250),
			total_study_count int,
			total_study_count_std int,
			total_study_count_stat int,
			total_amount money default 0,
			action nchar(1)	 default ''
			
		)
		create table #tmpInvHdrFinal
		(
			rec_id int identity(1,1),
			billing_cycle_id uniqueidentifier,
			billing_account_id uniqueidentifier,
			billing_acc_name nvarchar(250),
			total_study_count int,
			total_study_count_std int,
			total_study_count_stat int,
			total_amount money default 0,
			total_disc_amount money default 0,
			total_free_credits int default 0,
			action nchar(1)	 default ''
			
		)
		create table #tmpInst
		(
			rec_id int identity(1,1),
			billing_cycle_id uniqueidentifier,
			billing_account_id uniqueidentifier,
			institution_id uniqueidentifier,
			institution_code nvarchar(5),
			institution_name nvarchar(100),
			total_study_count int default 0,
			total_study_count_std int,
			total_study_count_stat int,
			total_amount money default 0
		)
		create table #tmpInstFinal
		(
			rec_id int identity(1,1),
			billing_cycle_id uniqueidentifier,
			billing_account_id uniqueidentifier,
			institution_id uniqueidentifier,
			institution_code nvarchar(5),
			institution_name nvarchar(100),
			total_study_count int default 0,
			total_study_count_std int,
			total_study_count_stat int,
			total_amount money default 0,
			total_disc_amount money default 0,
			total_free_credits int default 0
		)
		create table #tmpInst1
		(
			rec_id int identity(1,1),
			institution_id uniqueidentifier,
			institution_code nvarchar(5),
			institution_name nvarchar(100),
			total_study_count int default 0,
			total_study_count_std int,
			total_study_count_stat int,
			total_amount money default 0,
			total_disc_amount money default 0,
			total_free_credits int default 0
		)
		create table #tmpInstDtls
		(
			rec_id int identity(1,1),
			billing_cycle_id uniqueidentifier,
			billing_account_id uniqueidentifier,
			institution_id uniqueidentifier,
			received_date datetime,
			study_id uniqueidentifier,
			study_uid nvarchar(100),
			category_id int,
			category_name nvarchar(30),
			modality_id int,
			modality_name nvarchar(50),
			charge_by nchar(1),
			priority_id int,
			patient_name nvarchar(200),
			beyond_hour_stat nchar(1) default 'N',
			image_count int,
			service_codes nvarchar(250),
			rate money default 0,
			addon_rate_per_unit money default 0,
			study_max_amount money default 0,
			service_total money default 0,
			is_free nchar(1) default 'N',
			applied_discount decimal(5,2) default 0,
			disc_amount money default 0,
			status_id int default 0,
			promotion_id uniqueidentifier default '00000000-0000-0000-0000-000000000000',
			amount money default 0,
			study_price money default 0,
			service_price money default 0,
			modality_amended_rate nchar(1) null default 'N',
			service_amended_rate nchar(1) null default 'N',
			priority_charged nchar(1) null default 'Y'
		)
		create table #tmpInstDtls1
		(
			rec_id int identity(1,1),
			study_id uniqueidentifier,
			study_uid nvarchar(100),
			category_id int,
			category_name nvarchar(30),
			modality_id int,
			modality_name nvarchar(50),
			patient_name nvarchar(200),
			image_count int,
			rate money default 0,
			addon_rate_per_unit money default 0,
			study_max_amount money default 0,
			amount money default 0,
			study_price money default 0,
			service_price money default 0,
			service_total money default 0,
			is_free nchar(1) default 'N',
			status_id int default 0,
			applied_discount decimal(5,2) default 0,
			disc_amount money default 0,
			promotion_id uniqueidentifier default '00000000-0000-0000-0000-000000000000'
		)
		create table #tmpServiceDtls
		(
			rec_id int identity(1,1),
			billing_cycle_id uniqueidentifier,
			billing_account_id uniqueidentifier,
			institution_id uniqueidentifier,
			study_id uniqueidentifier,
			study_uid nvarchar(100),
			modality_id int null default 0,
			service_id int null default 0,
			priority_id int null default 0,
			amount money null default 0,
			price money null default 0,
			price_after_hrs money null default 0,
			disc_per_applied decimal(5,2) default 0,
			is_free nchar(1) default 'N',
			is_after_hrs nchar(1) default 'N'
		)

		select  @date_from= convert(date, date_from,103),
				@date_till=convert(date,date_till,103)
		from billing_cycle
		where id=@billing_cycle_id

	    select @CALCMINUTEFACT = data_value_int
		from invoicing_control_params
		where control_code='CALCMINUTEFACT'

	    delete from invoice_service_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_account_id
	    delete from invoice_institution_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_account_id
		delete from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_account_id
		delete from invoice_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_account_id

		insert into #tmpInvHdr(billing_cycle_id,billing_account_id,billing_acc_name,total_study_count,total_study_count_std,total_study_count_stat)
		(select
				@billing_cycle_id,
				biiling_account_id =id,
				billing_account_name= name,
				total_study_count = isnull((select count(id)
											from study_hdr
											where study_status_pacs=100
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and institution_id in (select id from institutions where billing_account_id=ba.id)) + 
											(select count(id)
											from study_hdr_archive
											where study_status_pacs in (100,0)
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and institution_id in (select id from institutions where billing_account_id=ba.id)),0),
				total_study_count_std     = isnull((select count(id)
													from study_hdr
													where study_status_pacs=100
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id in (select priority_id from sys_priority where is_stat ='N')
													and institution_id in (select id from institutions where billing_account_id=ba.id)) + 
													(select count(id)
													from study_hdr_archive
													where study_status_pacs in (100,0)
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id in (select priority_id from sys_priority where is_stat ='N')
													and institution_id in (select id from institutions where billing_account_id=ba.id)),0),
					total_study_count_stat = isnull((select count(id)
												from study_hdr
												where study_status_pacs=100
												and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
												and deleted ='N'
												and priority_id  in (select priority_id from sys_priority where is_stat ='Y')
												and institution_id in (select id from institutions where billing_account_id=ba.id)) + 
												(select count(id)
												from study_hdr_archive
												where study_status_pacs in (100,0)
												and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
												and deleted ='N'
												and priority_id in (select priority_id from sys_priority where is_stat ='Y')
												and institution_id in (select id from institutions where billing_account_id=ba.id)),0)
			from billing_account ba
			where is_active='Y'
			and ba.id = @billing_account_id
		)
		
		delete from #tmpInvHdr where total_study_count=0

		insert into #tmpInvHdrFinal(billing_cycle_id,billing_account_id,billing_acc_name,total_study_count,total_study_count_std,total_study_count_stat)
		(select billing_cycle_id,billing_account_id,billing_acc_name,total_study_count,total_study_count_std,total_study_count_stat
		from #tmpInvHdr
		where total_study_count > 0)

		insert into #tmpInst(billing_cycle_id,billing_account_id,institution_id,institution_name,institution_code,
		                      total_study_count,total_study_count_std,total_study_count_stat)
		(select @billing_cycle_id,
				ins.billing_account_id, 
				ins.id,dbo.InitCap(ins.name),ins.code,
				total_study_count=isnull((select count(id)
											from study_hdr
											where study_status_pacs=100
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and institution_id = ins.id) + 
											(select count(id)
											from study_hdr_archive
											where study_status_pacs in (100,0)
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and institution_id = ins.id),0),
				total_study_count_std = isnull((select count(id)
											from study_hdr
											where study_status_pacs=100
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and priority_id in (select priority_id from sys_priority where is_stat ='N')
											and institution_id = ins.id) + 
											(select count(id)
											from study_hdr_archive
											where study_status_pacs in (100,0)
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and priority_id in (select priority_id from sys_priority where is_stat ='N')
											and institution_id = ins.id),0),
				total_study_count_stat = isnull((select count(id)
											from study_hdr
											where study_status_pacs=100
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and priority_id in (select priority_id from sys_priority where is_stat ='Y')
											and institution_id = ins.id) + 
											(select count(id)
											from study_hdr_archive
											where study_status_pacs in (100,0)
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and priority_id in (select priority_id from sys_priority where is_stat ='Y')
											and institution_id = ins.id),0)
			from institutions ins
			where ins.billing_account_id in (select billing_account_id from #tmpInvHdr)
		)
		
		delete from #tmpInst where total_study_count=0
		--select * from #tmpInst

		insert into #tmpInstFinal(billing_cycle_id,billing_account_id,institution_id,institution_name,institution_code,total_study_count,total_study_count_std,total_study_count_stat)
		(select billing_cycle_id,billing_account_id,institution_id,institution_name,institution_code,total_study_count,total_study_count_std,total_study_count_stat
		 from #tmpInst
		 where total_study_count>0)

		insert into #tmpInstDtls(billing_cycle_id,billing_account_id,institution_id,
		                         received_date,study_id,study_uid,category_id,category_name,modality_id,modality_name,charge_by,priority_id,priority_charged,
								 beyond_hour_stat,patient_name,image_count,service_codes,status_id,applied_discount,modality_amended_rate)
		(select @billing_cycle_id,
				ins.billing_account_id,
				institution_id = ins.id,
				sh.received_date,
				sh.id, 
				sh.study_uid,
				sh.category_id,
				category_name= isnull(c.name,'Unknown'),
				sh.modality_id,
				modality_name = dbo.InitCap(isnull(m.name,'Unknown')),
				charge_by = isnull(m.invoice_by,'X'),
				sh.priority_id,
				sh.priority_charged,
				sh.beyond_hour_stat,
				patient_name=dbo.InitCap(isnull(sh.patient_name,'')),
				case 
				   when isnull(m.invoice_by,'') = 'B' then (select count(study_type_id) from study_hdr_study_types where study_hdr_id = sh.id) 
				   else sh.img_count
				 end img_count,
				service_codes = isnull(sh.service_codes,''),
				sh.study_status_pacs,
				isnull(sh.discount_per,0),
				case when (select count(study_hdr_id) from ar_amended_rates where study_hdr_id = sh.id and head_id=sh.modality_id and category_id=sh.category_id and head_type='M')>0 then 'Y' else 'N'  end
		    from study_hdr sh
			inner join institutions ins on sh.institution_id=ins.id
			left outer join sys_study_category c on c.id=sh.category_id
			left outer join modality m on m.id=sh.modality_id
			where sh.institution_id in (select institution_id from #tmpInst)
			and convert(datetime,convert(varchar(11),sh.received_date,106)) between @date_from and @date_till
			and sh.deleted ='N'
			union
			select @billing_cycle_id,
				ins.billing_account_id,
				institution_id = ins.id,
				sh.received_date,
				sh.id, 
				sh.study_uid,
				sh.category_id,
				category_name= isnull(c.name,'Unknown'),
				sh.modality_id,
				modality_name = dbo.InitCap(isnull(m.name,'Unknown')),
				charge_by = isnull(m.invoice_by,'X'),
				sh.priority_id,
				sh.priority_charged,
				sh.beyond_hour_stat,
				patient_name=dbo.InitCap(isnull(sh.patient_name,'')),
				case 
				   when isnull(m.invoice_by,'') = 'B' then (select count(study_type_id) from study_hdr_study_types_archive where study_hdr_id = sh.id) 
				   else sh.img_count
				 end img_count,
				service_codes = isnull(sh.service_codes,''),
				sh.study_status_pacs,
				isnull(sh.discount_per,0),
				case when (select count(study_hdr_id) from ar_amended_rates where study_hdr_id = sh.id and head_id=sh.modality_id and category_id=sh.category_id and head_type='M')>0 then 'Y' else 'N'  end
		    from study_hdr_archive sh
			inner join institutions ins on sh.institution_id=ins.id
			left outer join sys_study_category c on c.id=sh.category_id
			left outer join modality m on m.id=sh.modality_id
			where sh.institution_id in (select institution_id from #tmpInst)
			and convert(datetime,convert(varchar(11),sh.received_date,106)) between @date_from and @date_till
			and sh.deleted ='N')

		--select * from #tmpInstDtls-- where institution_id='761562B3-79CA-40D2-B43B-EEE2A178D58B'
		select  @rowcount=(select count(rec_id) from #tmpInstDtls),
		        @counter =1

	    while(@counter <= @rowcount)
			begin
				create table #tmpServices
				(
					row_id int identity(1,1),
					modality_id int null default 0,
					service_id int null default 0,
					service_code nvarchar(10) null default '',
					amount money null default 0,
					price  money null default 0,
					price_after_hrs money null default 0,
					is_after_hrs nchar(1) default 'N'
				)

				set @disc_per=0
				set @promotion_id_disc = '00000000-0000-0000-0000-000000000000'
				set @promotion_id_fc = '00000000-0000-0000-0000-000000000000'
				set @is_free='N'
				set @free_credits_consumed=0
				set @free_credits_consumed_tmp=0
				set @service_code =''
				set @status_id = 0
				set @consult_amount=0
				set @svc_id=0
				set @rate=0
				set @amount=0
				set @priority_charged='Y'
				set @beyond_hour_stat = 'N'
				set @modality_amended_rate='N'
				set @rate_per_unit =0
				set @study_price=0
				set @img_count=0
				set @max_count=0

				select  @bill_cycle_id    = billing_cycle_id,
						@bill_account_id  = billing_account_id,
						@inst_id          = institution_id ,   
						@recived_date     = received_date,
						@study_id         = study_id,
						@study_uid        = study_uid,
						@img_count        = image_count,
						@category_id      = category_id,
						@modality_id      = modality_id,
						@charge_by        = charge_by,
						@status_id        = status_id,
						@service_code     = service_codes,
						@priority_id      = priority_id,
						@priority_charged = priority_charged,
						@beyond_hour_stat = beyond_hour_stat,
						@disc_per         = applied_discount,
						@modality_amended_rate = modality_amended_rate
				from #tmpInstDtls
				where rec_id = @counter

				if(@charge_by='I' or @charge_by='B')
					begin
						select @max_count = max(default_count_to)
						from ar_modality_fee_schedule_template
						where modality_id=@modality_id
						and category_id =@category_id
						and deleted='N'

						if(@img_count <= @max_count)
							begin
								select @rate = bamfs.fee_amount
								from billing_account_modality_fee_schedule bamfs
								inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
								where bamfs.billing_account_id = @bill_account_id
								and  mfst.category_id= @category_id
								and  mfst.modality_id= @modality_id
								and  (mfst.default_count_from<=@img_count and mfst.default_count_to >=@img_count)

								select @rate_per_unit = bamfs.fee_amount_per_unit
								from billing_account_modality_fee_schedule bamfs
								inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
								where bamfs.billing_account_id = @bill_account_id
								and  mfst.category_id= @category_id
								and  mfst.modality_id= @modality_id
								and  (mfst.default_count_from<=@img_count and mfst.default_count_to >=@img_count)

								select @study_max_amount = bamfs.study_max_amount
								from billing_account_modality_fee_schedule bamfs
								inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
								where bamfs.billing_account_id = @bill_account_id
								and  mfst.category_id= @category_id
								and  mfst.modality_id= @modality_id
								and  (mfst.default_count_from<=@img_count and mfst.default_count_to >=@img_count)
							end
						else
							begin
								select @rate = bamfs.fee_amount
								from billing_account_modality_fee_schedule bamfs
								inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
								where bamfs.billing_account_id = @bill_account_id
								and  mfst.category_id= @category_id
								and  mfst.modality_id= @modality_id
								and  mfst.default_count_to = @max_count

								select @rate_per_unit = bamfs.fee_amount_per_unit
								from billing_account_modality_fee_schedule bamfs
								inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
								where bamfs.billing_account_id = @bill_account_id
								and  mfst.category_id= @category_id
								and  mfst.modality_id= @modality_id
								and  mfst.default_count_to = @max_count

								select @study_max_amount = bamfs.study_max_amount
								from billing_account_modality_fee_schedule bamfs
								inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
								where bamfs.billing_account_id = @bill_account_id
								and  mfst.category_id= @category_id
								and  mfst.modality_id= @modality_id
								and  mfst.default_count_to = @max_count
							end
					end
				else if(@charge_by ='M')
					begin
						select @rate = bamfs.fee_amount
						from billing_account_modality_fee_schedule bamfs
						inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
						where bamfs.billing_account_id = @bill_account_id
						and  mfst.category_id= @category_id
						and  mfst.modality_id= @modality_id

						select @rate_per_unit = bamfs.fee_amount_per_unit
						from billing_account_modality_fee_schedule bamfs
						inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
						where bamfs.billing_account_id = @bill_account_id
						and  mfst.category_id= @category_id
						and  mfst.modality_id= @modality_id

						select @study_max_amount = bamfs.study_max_amount
						from billing_account_modality_fee_schedule bamfs
						inner join ar_modality_fee_schedule_template mfst on bamfs.rate_id=mfst.id
						where bamfs.billing_account_id = @bill_account_id
						and  mfst.category_id= @category_id
						and  mfst.modality_id= @modality_id
					end

				set @rate             = isnull(@rate,0)
				set @rate_per_unit    = isnull(@rate_per_unit,0)
				set @study_max_amount = isnull(@study_max_amount,0)

				update #tmpInstDtls 
				set rate                = @rate,
				    addon_rate_per_unit = @rate_per_unit,
					study_max_amount    = @study_max_amount
				where rec_id = @counter

				/**********************************************************************/
				/***********************Modality Rate Calculations*********************/
				/**********************************************************************/
				set @amount=0
				set @max_count=0
				if(@charge_by='I' or @charge_by='B')
					begin
						if(@modality_amended_rate='N')
							begin
								set @amount = @rate

								if(@max_count < @img_count)
									begin
									   set @amount = @amount + ((@img_count - @max_count) * @rate_per_unit)
									end
								set @study_price = @amount
							end
						else
							begin
								set @amount = isnull((select rate
										              from ar_amended_rates
													  where billing_account_id = @bill_account_id
													  and   billing_cycle_id   = @bill_cycle_id
													  and   study_hdr_id       = @study_id
													  and   category_id        = @category_id
													  and   head_id            = @modality_id
													  and   head_type = 'M'),0)
								set @study_price = @amount
							end
					end
				else if(@charge_by='M')
					begin
						if(@modality_amended_rate='N')
							begin
								set @amount = @rate
								

								select @max_count = default_count_to 
								from ar_modality_fee_schedule_template 
								where modality_id=@modality_id 
								and category_id=@category_id
								and deleted='N'

								select @max_count = isnull(@max_count,0) * 60
								set @study_seconds = round(convert(decimal(8,2),@img_count)/convert(decimal(8,2),@CALCMINUTEFACT),0)
						
								if(@study_seconds - convert(decimal(8,2),@max_count))>0
									begin
										set  @rate_per_unit = @rate_per_unit/60
										set  @amount = @amount + (@rate_per_unit * (@study_seconds - convert(decimal(8,2),@max_count)))
									end

								set @study_price = @amount
							end
						else
							begin
								set @amount = isnull((select rate
													  from ar_amended_rates
													  where billing_account_id = @bill_account_id
													  and   billing_cycle_id   = @bill_cycle_id
													  and   study_hdr_id       = @study_id
													  and   category_id        = @category_id
													  and   head_id            = @modality_id
													  and   head_type = 'M'),0)
								set @study_price = @amount
							end
					end

				if(@study_max_amount>0 and @modality_amended_rate='N')
					begin
						if(@study_max_amount < @amount)
							begin
								set @amount = @study_max_amount
								set @study_price = @amount
							end
					end

				update #tmpInstDtls set amount =@amount,study_price=@study_price where rec_id = @counter

				/****************************************************************************/
				/***********************Apply Discount & Promotion***************************/
				/****************************************************************************/
				if(@disc_per=0)
					begin
							/**********************************************************************/
							/*************************Discount Applied*****************************/
							/**********************************************************************/
							if(@status_id = 100)
								begin
									select  @disc_per          =  arpi.discount_percent,
											@promotion_id_disc = arpi.id
									from ar_promotion_institution arpi
									inner join ar_promotions arp on arp.id= arpi.hdr_id
									where arpi.institution_id=@inst_id
									and arpi.modality_id = @modality_id
									and arp.promotion_type='D'
									and arp.valid_from <= convert(datetime,convert(varchar(11),@recived_date,106))
									and arp.valid_till >=convert(datetime,convert(varchar(11),@recived_date,106))
								end

							--print @modality_id
							--print @recived_date
							set @disc_per = isnull(@disc_per,0)
							set @promotion_id_disc = isnull(@promotion_id_disc,'00000000-0000-0000-0000-000000000000')
							--print @disc_per

							/**********************************************************************/
							/***************************Free Credits*******************************/
							/**********************************************************************/
							if(@status_id = 100)
								begin
									select  @free_credits    =  arpi.free_credits,
											@promotion_id_fc = arpi.id
									from ar_promotion_institution arpi
									inner join ar_promotions arp on arp.id = arpi.hdr_id
									where arpi.institution_id=@inst_id
									and arpi.modality_id = @modality_id
									and arp.promotion_type='F'
									and arp.valid_till >=convert(datetime,convert(varchar(11),@recived_date,106))
								end

							set @free_credits = isnull(@free_credits,0)
							set @promotion_id_fc = isnull(@promotion_id_fc,'00000000-0000-0000-0000-000000000000')
							--print @free_credits
					end
						
				if(@promotion_id_disc<>'00000000-0000-0000-0000-000000000000')
					begin
						set @promotion_id = @promotion_id_disc
					end
				else if(@promotion_id_fc<>'00000000-0000-0000-0000-000000000000')
					begin
						set @promotion_id = @promotion_id_fc

						select @free_credits_consumed = count(is_free)
						from invoice_institution_dtls
						where promotion_id = @promotion_id
						and billing_cycle_id <> @billing_cycle_id

						select @free_credits_consumed_tmp = count(is_free)
						from #tmpInstDtls
						where promotion_id = @promotion_id

						set @free_credits_consumed = @free_credits_consumed + @free_credits_consumed_tmp
						
						
						--print '-----------------------------------'

						if(@free_credits > @free_credits_consumed)
							begin
								set @is_free='Y'
							end

						--print @is_free

					end
				else
					begin
						set @promotion_id = '00000000-0000-0000-0000-000000000000'
					end

				--select @service_code
				/**********************************************************************/
				/***********************Service Calculations***************************/
				/**********************************************************************/
				set @svc_code= ''
				set @rc = 0
				set @ctr= 0

				if(rtrim(ltrim(@service_code)) <> '')
					begin
						set @svc_id=0
						if(charindex(',',@service_code))>0
							begin
								insert into #tmpServices(service_code)
								(select data from dbo.split(@service_code,','))

								set @rc=@@rowcount
								
								update #tmpServices
								set service_id=isnull((select id from services where code=#tmpServices.service_code),0)
							end
						else
							begin
								set @svc_code = @service_code

								insert into #tmpServices(service_code) values(@svc_code)

								set @rc=@@rowcount

								update #tmpServices
								set service_id=isnull((select id from services where code=@svc_code),0)
							end
					end

				if(@priority_id > 0 and @status_id = 100)
					begin
						insert into #tmpServices(service_id)
						(select id from services
						 where priority_id = @priority_id)
					end

				 set @rc = @rc + @@rowcount
				 set @ctr=1
				 

				 while(@ctr <= @rc)
					begin
						set @service_amount  = 0
						select @svc_id      = service_id,
						       @svc_code    = service_code
						from #tmpServices
						where row_id = @ctr

					

								if(select count(study_hdr_id) from ar_amended_rates where study_hdr_id = @study_id and head_type='S' and head_id=@svc_id and billing_account_id=@bill_account_id and billing_cycle_id=@bill_cycle_id)=0
									begin
										if(@charge_by <>'B')
											begin
												select  @service_amount=fee_amount ,
														@service_amount_after_hrs = fee_amount_after_hrs
												from billing_account_service_fee_schedule
												where billing_account_id = @bill_account_id
												and rate_id = (select id 
															   from ar_service_fee_schedule_template 
															   where service_id=@svc_id 
															   and modality_id=@modality_id
															   and deleted='N')

												--print @service_amount
												--print @service_amount_after_hrs
											end
										 else if(@charge_by ='B')
											begin
												select  @service_amount=fee_amount ,
														@service_amount_after_hrs = fee_amount_after_hrs
												from billing_account_service_fee_schedule
												where billing_account_id = @bill_account_id
												and rate_id = (select id 
															   from ar_service_fee_schedule_template 
															   where service_id=@svc_id 
															   and modality_id=@modality_id
															   and (default_count_from<=@img_count and default_count_to>=@img_count)
															   and deleted='N')
											end
									end
								else
									begin
										select @service_amount=rate,
											   @service_amount_after_hrs = rate
										from ar_amended_rates
										where billing_account_id = @bill_account_id
										and billing_cycle_id     = @bill_cycle_id
										and study_hdr_id         = @study_id
										and head_id              = @svc_id
										and head_type            = 'S'
									end
							

						set @service_amount  = isnull(@service_amount,0)
						set @service_amount_after_hrs = isnull(@service_amount_after_hrs,0)

						if(@priority_charged ='N') 
							begin
								set @service_amount  = 0
							end

						if(@beyond_hour_stat ='N')
							begin
								set @service_amount_after_hrs = 0
							end
						else
							begin
								set @service_amount  = 0
								if(@priority_charged ='N') 
									begin
										set @service_amount_after_hrs  = 0
									end
							end

							--print @service_amount
						set @service_amount = isnull(@service_amount,0) + isnull(@service_amount_after_hrs,0)
						update #tmpServices
						set modality_id     = @modality_id,
						    amount          = @service_amount,
							price           = isnull(@service_amount,0),
							price_after_hrs = isnull(@service_amount_after_hrs,0),
							is_after_hrs    = @beyond_hour_stat
						where row_id = @ctr

						insert into #tmpServiceDtls(billing_cycle_id,billing_account_id,institution_id,study_id,study_uid,modality_id,service_id,priority_id,amount,price,price_after_hrs,is_after_hrs)
						                     values(@bill_cycle_id,@bill_account_id,@inst_id,@study_id,@study_uid,@modality_id,@svc_id,@priority_id,@service_amount,@service_amount,@service_amount_after_hrs,@beyond_hour_stat)

						set @ctr = @ctr + 1
					end

				set @service_total=0
				set @service_price=0
				select @service_total = isnull((select sum(amount) from #tmpServices),0)
				set @service_price = @service_total

				
				
				if(@disc_per>0 and @status_id = 100)
					begin
						select @disc_amount = ((@disc_per/100) * (amount + @service_total))
						from #tmpInstDtls
						where rec_id = @counter

						update #tmpInstDtls
						set amount           = (amount - ((@disc_per/100) * amount)),
							service_total    = (@service_total - ((@disc_per/100) * @service_total)),
							service_price    = @service_price,
							applied_discount = @disc_per,
							disc_amount      = @disc_amount,
							promotion_id     = @promotion_id
						where rec_id = @counter

						update #tmpServiceDtls
						set amount = (amount - ((@disc_per/100) * amount)),
						    disc_per_applied = @disc_per
						where study_id = @study_id
					end
				else if(@is_free='Y')
					begin
						update #tmpInstDtls
						set amount           = 0,
							rate             = 0,
							service_total    = 0,
							service_price    = @service_price,
							is_free          = 'Y',
							promotion_id     = @promotion_id
						where rec_id = @counter

						update #tmpServiceDtls
						set amount = 0,
						    is_free = 'Y'
						where study_id = @study_id
					end
				else
					begin
						update #tmpInstDtls
						set service_total    = @service_total,
						    service_price    = @service_price
						where rec_id = @counter
					end

				drop table #tmpServices

				set @counter=@counter + 1

			end

		select  @rowcount=(select count(institution_id) from #tmpInstFinal)

		if(@rowcount > 0)	
			begin
				set @counter = 1
				while(@counter <= @rowcount)
					begin
						
						update #tmpInstFinal
						set total_amount      =(select sum(amount) + sum(service_total) 
						                        from #tmpInstDtls 
										        where institution_id =#tmpInstFinal.institution_id),
							total_disc_amount = (select sum(disc_amount) 
												 from #tmpInstDtls 
												 where institution_id =#tmpInstFinal.institution_id),
							total_free_credits = (select count(is_free) 
												 from #tmpInstDtls 
												 where institution_id =#tmpInstFinal.institution_id
												 and is_free='Y')
						where rec_id=@counter
						
						set @counter = @counter + 1
					end
				
			end

		select  @rowcount=(select count(billing_account_id) from #tmpInvHdr)
		if(@rowcount > 0)
			begin
				set @counter = 1
				while(@counter <= @rowcount)
					begin
						select @billing_acc_id=billing_account_id 
						from #tmpInvHdrFinal 
						where rec_id=@counter

						update #tmpInvHdrFinal
						set total_amount    =(select sum(total_amount) 
						                      from #tmpInstFinal 
										      where billing_account_id =@billing_acc_id),
							total_disc_amount = (select sum(total_disc_amount) 
						                         from #tmpInstFinal 
										         where billing_account_id =@billing_acc_id),
							total_free_credits = (select sum(total_free_credits) 
						                         from #tmpInstFinal 
										         where billing_account_id =@billing_acc_id)
						where billing_account_id=@billing_acc_id

						select 	@total_study_count      = total_study_count,
						        @total_study_count_std  = total_study_count_std,
								@total_study_count_stat = total_study_count_stat,
								@total_amount           = total_amount,
								@total_disc_amount      = total_disc_amount,
								@total_free_credits     = total_free_credits
						from #tmpInvHdrFinal 
						where billing_account_id=@billing_acc_id

						
						if(select count(id) from invoice_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id)>0
							begin
								select @inv_id = id from invoice_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id

								update invoice_hdr
								set total_study_count=@total_study_count,
								    total_study_count_std   = @total_study_count_std,
								    total_study_count_stat  = @total_study_count_stat,
									total_amount            = @total_amount,
									total_disc_amount       = @total_disc_amount,
								    total_free_credits      = @total_free_credits
								where id = @inv_id

								
							end
						else
							begin
								set @inv_id=newid()
								insert into invoice_hdr(id,billing_cycle_id,billing_account_id,total_study_count,total_study_count_std,total_study_count_stat,
								                        total_amount,total_disc_amount,total_free_credits,created_by,date_created)
												 values(@inv_id,@billing_cycle_id,@billing_acc_id,@total_study_count,@total_study_count_std,@total_study_count_stat,
												        @total_amount,@total_disc_amount,@total_free_credits,@user_id,getdate())
							end 

						if(@@rowcount=0)
							begin
								
								select @user_name = name from billing_account where id = @billing_acc_id
								select @error_code='230',@return_status=0
								return 0
							end

						insert into #tmpInst1(institution_id,institution_code,institution_name,total_study_count,total_study_count_std,total_study_count_stat,
						                      total_amount,total_disc_amount,total_free_credits)
						(select institution_id,institution_code,institution_name,total_study_count,total_study_count_std,total_study_count_stat,
						        total_amount,total_disc_amount,total_free_credits
						 from #tmpInstFinal
						 where billing_account_id=@billing_acc_id)

						select  @rowcount1=@@rowcount,@counter1=1

						if(@rowcount1 > 0)
							begin 
								set @counter1 = 1

								while(@counter1 <= @rowcount1)
									begin
										
										select  @institution_id         = institution_id,
												@total_study_count      = total_study_count,
												@total_study_count_std  = total_study_count_std,
												@total_study_count_stat = total_study_count_stat,
												@total_amount           = total_amount,
												@total_disc_amount      = total_disc_amount,
												@total_free_credits     = total_free_credits
										from #tmpInst1 
										where rec_id=@counter1

										if((select count(id) from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id and institution_id=@institution_id)>0)
											begin
												select @inv_inst_hdr_id=id from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id and institution_id=@institution_id

												update invoice_institution_hdr
												set total_study_count       = @total_study_count,
													total_study_count_std   = @total_study_count_std,
												    total_study_count_stat  = @total_study_count_stat,
													total_amount            = @total_amount,
													total_disc_amount       = @total_disc_amount,
													free_read_count         = @total_free_credits
												--where id = @inv_inst_hdr_id
												where billing_cycle_id=@billing_cycle_id 
												and billing_account_id=@billing_acc_id 
												and institution_id=@institution_id
											end
										else
											begin
												set @inv_inst_hdr_id=newid()

												insert into invoice_institution_hdr(id,hdr_id,billing_cycle_id,billing_account_id,institution_id,total_study_count,total_study_count_std,total_study_count_stat,
												                                    total_amount,total_disc_amount,free_read_count,created_by,date_created)
																	         values(@inv_inst_hdr_id,@inv_id,@billing_cycle_id,@billing_acc_id,@institution_id,@total_study_count,@total_study_count_std,@total_study_count_stat,
																			        @total_amount,@total_disc_amount,@total_free_credits,@user_id,getdate())
											end

										if(@@rowcount=0)
											begin
												
												select @user_name = name from institutions where id = @institution_id
												select @error_code='231',@return_status=0
												return 0
											end

										insert into #tmpInstDtls1(study_id,study_uid,category_id,category_name,modality_id,modality_name,patient_name,image_count,
													                          rate,amount,addon_rate_per_unit,study_max_amount,
													                          study_price,service_total,service_price,applied_discount,disc_amount,is_free,promotion_id)
													(select study_id,study_uid,category_id,category_name,modality_id,modality_name,patient_name,image_count,
													        rate,amount,addon_rate_per_unit,study_max_amount,
													        study_price,service_total,service_price,applied_discount,disc_amount,is_free,promotion_id
													 from #tmpInstDtls
													 where billing_cycle_id=@billing_cycle_id 
													 and billing_account_id=@billing_acc_id 
													 and institution_id=@institution_id)

										select  @rowcount2=@@rowcount,@counter2=1

										-- Inst dtls
												
										if(@rowcount2 > 0)
											begin
												set @counter2 = 1
												while(@counter2 <= @rowcount2)
													begin

														select 	@study_id=study_id,
																@study_uid=study_uid,
																@category_id = category_id,
																@modality_id=modality_id,
																@img_count=image_count,
																@rate=rate,
																@rate_per_unit = addon_rate_per_unit,
																@study_max_amount = study_max_amount,
																@amount=amount,
																@study_price = study_price,
																@service_amount = service_total,
																@service_price  = service_price,
																@disc_per       = applied_discount,
																@disc_amount= disc_amount,
																@is_free        = is_free,
																@promotion_id   = promotion_id
														from #tmpInstDtls1 
														where rec_id=@counter2

														select @approved      = isnull(approved,'N'),
														       @approved_by   = @user_id,
															   @date_approved = getdate()
														from invoice_institution_hdr
														where billing_cycle_id=@billing_cycle_id 
														and billing_account_id=@billing_acc_id 
														and institution_id=@institution_id

														if(@approved='N')
															begin
																set  @approved_by   = '00000000-0000-0000-0000-000000000000'
																set  @date_approved = null
															end

														if(select count(id) from invoice_institution_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id and institution_id=@institution_id and study_id=@study_id)= 0
															begin
																set @inv_inst_dtls_id = newid()
																insert into invoice_institution_dtls(id,hdr_id,institution_hdr_id,billing_cycle_id,billing_account_id,institution_id,study_id,study_uid,category_id,modality_id,image_count,
																												  rate,amount,rate_per_unit,study_max_amount,service_amount,study_price,service_price,disc_per_applied,disc_amount,is_free,promotion_id,total_amount,
																												  approved,approved_by,date_approved,created_by,date_created)
																										  values(@inv_inst_dtls_id,@inv_id,@inv_inst_hdr_id,@billing_cycle_id,@billing_acc_id,@institution_id,@study_id,@study_uid,@category_id,@modality_id,@img_count,
																												 @rate,@amount,@rate_per_unit,@study_max_amount,@service_amount,@study_price,@service_price,@disc_per,@disc_amount,@is_free,@promotion_id,@amount + @service_amount,@approved,@approved_by,@date_approved,
																												 @user_id,getdate())
															end
														else
															begin
																select @inv_inst_dtls_id = id
																from invoice_institution_dtls
																where billing_cycle_id=@billing_cycle_id 
																and billing_account_id=@billing_acc_id 
																and institution_id=@institution_id 
																and study_id=@study_id

																update invoice_institution_dtls
																set modality_id      = @modality_id,
																    image_count      = @img_count,
																	rate             = @rate,
																	amount           = @amount,
																	study_price      = @study_price,
																	service_amount   = @service_amount,
																	service_price    = @service_price,
																	disc_per_applied = @disc_per,
																	disc_amount      = @disc_amount,
																	is_free          = @is_free,
																	promotion_id     = @promotion_id,
																	total_amount     = @amount + @service_amount,
																	approved         = @approved,
																	approved_by      = @approved_by,
																	date_approved    = @date_approved
																where billing_cycle_id=@billing_cycle_id 
																and billing_account_id=@billing_acc_id 
																and institution_id=@institution_id 
																and study_id=@study_id

															end
														
														if(@@rowcount=0)
															begin
																
																select @user_name = name from institutions where id = @institution_id
																select @error_code='231',@return_status=0
																return 0
															end


														delete from invoice_service_dtls
														where billing_cycle_id= @billing_cycle_id
														and billing_account_id = @billing_acc_id
														and institution_id = @institution_id
														and study_id=@study_id


														if(select count(study_id) 
														   from #tmpServiceDtls
														   where billing_cycle_id= @billing_cycle_id
															and billing_account_id = @billing_acc_id
															and institution_id = @institution_id
															and study_id=@study_id) >0
																begin
																	insert into invoice_service_dtls(id,hdr_id,institution_hdr_id,institution_dtls_id,
																												 billing_cycle_id,billing_account_id,institution_id,
																												 study_id,study_uid,modality_id,service_id,priority_id,amount,service_price,service_price_after_hrs,is_after_hrs,
																												 disc_per_applied,is_free,updated_by,date_updated)
																										  (select newid(),@inv_id,@inv_inst_hdr_id,@inv_inst_dtls_id,
																												 billing_cycle_id,billing_account_id,institution_id,
																												 study_id,study_uid,modality_id,service_id,priority_id,amount,price,price_after_hrs,is_after_hrs,
																												 disc_per_applied,is_free,@user_id,getdate()
																											from #tmpServiceDtls
																											where billing_cycle_id=@billing_cycle_id
																											and billing_account_id=@billing_acc_id 
																											and institution_id=@institution_id
																											and study_id=@study_id)

																	if(@@rowcount=0)
																		begin
																			
																			select @user_name = name from institutions where id = @institution_id
																			select @error_code='253',@return_status=0
																			return 0
																		end
															    end

														set @counter2 = @counter2 + 1
														
													end
												truncate table #tmpInstDtls1

												update invoice_institution_hdr
												set total_amount=(select sum(total_amount) from invoice_institution_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id and institution_id=@institution_id and billed='Y')
												where billing_cycle_id=@billing_cycle_id 
												and billing_account_id=@billing_acc_id 
												and institution_id=@institution_id

												if(@@rowcount=0)
													begin
														
														select @user_name = name from institutions where id = @institution_id
														select @error_code='231',@return_status=0
														return 0
													end
											end
										-- End Inst dtls

										update invoice_hdr
										set total_amount=(select sum(total_amount) from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id)
										where billing_cycle_id=@billing_cycle_id 
										and billing_account_id=@billing_acc_id 

										if(@@rowcount=0)
											begin
												
												select @user_name = name from institutions where id = @institution_id
												select @error_code='231',@return_status=0
												return 0
											end
										

										set @counter1 = @counter1 + 1
										
									end
				
							end --End inv_inst hdr

						truncate table #tmpInst1
						set @counter = @counter + 1
					end
				
			end


		delete from invoice_service_dtls 
		where billing_cycle_id = @billing_account_id
		and study_id not in (select distinct study_id
		                    from invoice_institution_dtls
							where billing_cycle_id = @billing_cycle_id)

		delete from invoice_institution_hdr 
		where billing_cycle_id = @billing_account_id
		and institution_id not in (select distinct institution_id
		                               from invoice_institution_dtls
									   where billing_cycle_id = @billing_cycle_id)
		
		delete from invoice_hdr 
		where billing_cycle_id = @billing_cycle_id
		and billing_account_id not in (select distinct billing_account_id
		                               from invoice_institution_hdr
									   where billing_cycle_id = @billing_cycle_id)

		set @return_status=1
	    set @error_code=''

	
		drop table #tmpInvHdr
		drop table #tmpInvHdrFinal
		drop table #tmpInst
		drop table #tmpInstFinal
		drop table #tmpInstDtls
		drop table #tmpInst1
		drop table #tmpInstDtls1
		drop table #tmpServiceDtls

		
		set nocount off
		return 1
	end
GO
