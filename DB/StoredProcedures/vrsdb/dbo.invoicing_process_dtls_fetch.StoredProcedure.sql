USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_process_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_process_dtls_fetch : 
                  fetch invoicing processing details
** Created By   : Pavel Guha 
** Created On   : 12/11/2019
*******************************************************/
--exec invoicing_process_dtls_fetch 'DC697D90-1AA7-4245-B3B3-DFFA8462CA33','97E069ED-31F3-4554-9F24-69C301718BCF',45,'11111111-1111-1111-1111-111111111111','','',0
--exec invoicing_process_dtls_fetch '397F7134-F742-45F3-924B-9A9F77EC20DC','00000000-0000-0000-0000-000000000000',45,'11111111-1111-1111-1111-111111111111','','',0
--exec invoicing_process_dtls_fetch '51855FD3-5000-4692-A59E-97911AB6592A','98A30426-BA37-4564-98D0-A2CF6A4B9929',45,'11111111-1111-1111-1111-111111111111','','',0
--exec invoicing_process_dtls_fetch 'D2B3965C-73F1-4BB5-AD9A-31003C1A869E','6869F650-A6FA-45E1-A58A-9DCFF86DEC0F',45,'11111111-1111-1111-1111-111111111111','','',0
--exec invoicing_process_dtls_fetch '85BADA40-3C7B-4329-891E-15AEFBC86F66','AB352297-C935-4E57-9CA9-D7C146B15914',45,'11111111-1111-1111-1111-111111111111','','',0
--exec invoicing_process_dtls_fetch '2353a24b-357f-482a-81cc-6a3ef38d7625','AB352297-C935-4E57-9CA9-D7C146B15914',45,'11111111-1111-1111-1111-111111111111','','',0
CREATE procedure [dbo].[invoicing_process_dtls_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
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
				@billing_cycle_name nvarchar(50),
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
				@consult_amount money

		declare @inv_id uniqueidentifier,
				@inv_inst_hdr_id uniqueidentifier,
				@inv_inst_dtls_id uniqueidentifier,
				@study_id uniqueidentifier,
				@study_uid nvarchar(100),
				@modality_id bigint,
				@img_count bigint,
				@rate money,
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
				@svc_id int,
				@svc_code nvarchar(10),
				@service_amount money,
				@disc_amount money,
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
				print '111'
				exec invoicing_process_view_fetch
					@billing_cycle_id   = @billing_cycle_id,
					@billing_account_id = @billing_account_id

			    return 0
			end

		--exec invoicing_process_dtls_fetch '51855FD3-5000-4692-A59E-97911AB6592A','18B3290A-6DB9-43E0-AC8E-28F0A7ECE52F',45,'11111111-1111-1111-1111-111111111111','','',0
		--print '111'

		create table #tmpInvHdr
		(
			rec_id int identity(1,1),
			billing_cycle_id uniqueidentifier,
			billing_cycle_name nvarchar(150),
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
			billing_cycle_name nvarchar(150),
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
			modality_id int,
			modality_name nvarchar(50),
			priority_id int,
			patient_name nvarchar(200),
			image_count int,
			service_codes nvarchar(250),
			rate money default 0,
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
			service_amended_rate nchar(1) null default 'N'
		)
		create table #tmpInstDtls1
		(
			rec_id int identity(1,1),
			study_id uniqueidentifier,
			study_uid nvarchar(100),
			modality_id int,
			modality_name nvarchar(50),
			patient_name nvarchar(200),
			image_count int,
			rate money default 0,
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
			service_id int null default 0,
			priority_id int null default 0,
			amount money null default 0,
			price money null default 0,
			disc_per_applied decimal(5,2) default 0,
			is_free nchar(1) default 'N'
		)
		create table #tmpZAInvHdr
		(
			rec_id int identity(1,1),
			billing_account_id uniqueidentifier,
			billing_acc_name nvarchar(250)
		)
		create table #tmpZAInst
		(
			rec_id int identity(1,1),
			billing_account_id uniqueidentifier,
			institution_id uniqueidentifier,
			institution_code nvarchar(5),
			institution_name nvarchar(100)
		)
		
		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id

		select  @billing_cycle_name = name,
				@date_from= convert(date, date_from,103),
				@date_till=convert(date,date_till,103)
		from billing_cycle
		where id=@billing_cycle_id

	   begin transaction

		if(@billing_account_id = '00000000-0000-0000-0000-000000000000')
			begin
				insert into #tmpInvHdr(billing_cycle_id,billing_cycle_name,billing_account_id,billing_acc_name,
				                       total_study_count,total_study_count_std,total_study_count_stat)
				(select
						@billing_cycle_id,
						@billing_cycle_name,
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
						total_study_count_std = isnull((select count(id)
													from study_hdr
													where study_status_pacs=100
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id = 20
													and institution_id in (select id from institutions where billing_account_id=ba.id)) + 
													(select count(id)
													from study_hdr_archive
													where study_status_pacs in (100,0)
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id = 20
													and institution_id in (select id from institutions where billing_account_id=ba.id)),0),
						total_study_count_stat = isnull((select count(id)
													from study_hdr
													where study_status_pacs=100
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id = 10
													and institution_id in (select id from institutions where billing_account_id=ba.id)) + 
													(select count(id)
													from study_hdr_archive
													where study_status_pacs in (100,0)
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id = 10
													and institution_id in (select id from institutions where billing_account_id=ba.id)),0)
					from billing_account ba
					where is_active='Y'
				)
			end
		else
			begin
				insert into #tmpInvHdr(billing_cycle_id,billing_cycle_name,billing_account_id,billing_acc_name,total_study_count,total_study_count_std,total_study_count_stat)
				(select
						@billing_cycle_id,
						@billing_cycle_name,
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
						total_study_count_std = isnull((select count(id)
													from study_hdr
													where study_status_pacs=100
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id = 20
													and institution_id in (select id from institutions where billing_account_id=ba.id)) + 
													(select count(id)
													from study_hdr_archive
													where study_status_pacs in (100,0)
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id = 20
													and institution_id in (select id from institutions where billing_account_id=ba.id)),0),
						total_study_count_stat = isnull((select count(id)
													from study_hdr
													where study_status_pacs=100
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id = 10
													and institution_id in (select id from institutions where billing_account_id=ba.id)) + 
													(select count(id)
													from study_hdr_archive
													where study_status_pacs in (100,0)
													and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
													and deleted ='N'
													and priority_id = 10
													and institution_id in (select id from institutions where billing_account_id=ba.id)),0)
					from billing_account ba
					where is_active='Y'
					and ba.id = @billing_account_id
				)
			end

        --select * from #tmpInvHdr	
		/**********************************************************/
		delete from #tmpInvHdr where total_study_count=0
		/**********************************************************/
		--select * from #tmpInvHdr

		insert into #tmpInvHdrFinal(billing_cycle_id,billing_cycle_name,billing_account_id,billing_acc_name,total_study_count,total_study_count_std,total_study_count_stat)
		(select billing_cycle_id,billing_cycle_name,billing_account_id,billing_acc_name,total_study_count,total_study_count_std,total_study_count_stat
		from #tmpInvHdr
		where total_study_count > 0)

		--select * from #tmpInvHdrFinal

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
											and priority_id = 20
											and institution_id = ins.id) + 
											(select count(id)
											from study_hdr_archive
											where study_status_pacs in (100,0)
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and priority_id = 20
											and institution_id = ins.id),0),
				total_study_count_stat = isnull((select count(id)
											from study_hdr
											where study_status_pacs=100
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and priority_id = 10
											and institution_id = ins.id) + 
											(select count(id)
											from study_hdr_archive
											where study_status_pacs in (100,0)
											and convert(datetime,convert(varchar(11),received_date,106)) between @date_from and @date_till
											and deleted ='N'
											and priority_id = 10
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
		                         received_date,study_id,study_uid,modality_id,modality_name,priority_id,
								 patient_name,image_count,service_codes,status_id,applied_discount,modality_amended_rate)
		(select @billing_cycle_id,
				ins.billing_account_id,
				institution_id = ins.id,
				sh.received_date,
				sh.id, 
				sh.study_uid,
				sh.modality_id,
				modality_name = dbo.InitCap(isnull(m.name,'Unknown')),
				sh.priority_id,
				patient_name=dbo.InitCap(isnull(sh.patient_name,'')),
				sh.img_count,
				service_codes = isnull(sh.service_codes,''),
				sh.study_status_pacs,
				isnull(sh.discount_per,0),
				case when (select count(study_hdr_id) from ar_amended_rates where study_hdr_id = sh.id and head_id=sh.modality_id and head_type='M')>0 then 'Y' else 'N'  end
		    from study_hdr sh
			inner join institutions ins on sh.institution_id=ins.id
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
				sh.modality_id,
				modality_name = dbo.InitCap(isnull(m.name,'Unknown')),
				sh.priority_id,
				patient_name=dbo.InitCap(isnull(sh.patient_name,'')),
				sh.img_count,
				service_codes = isnull(sh.service_codes,''),
				sh.study_status_pacs,
				isnull(sh.discount_per,0),
				case when (select count(study_hdr_id) from ar_amended_rates where study_hdr_id = sh.id and head_id=sh.modality_id and head_type='M')>0 then 'Y' else 'N'  end
		    from study_hdr_archive sh
			inner join institutions ins on sh.institution_id=ins.id
			left outer join modality m on m.id=sh.modality_id
			where sh.institution_id in (select institution_id from #tmpInst)
			and convert(datetime,convert(varchar(11),sh.received_date,106)) between @date_from and @date_till
			and sh.deleted ='N')

	
		update #tmpInstDtls 
		set rate=(isnull((select rate
			              from ar_amended_rates
						  where billing_account_id = #tmpInstDtls.billing_account_id 
						  and   billing_cycle_id  = @billing_cycle_id
						  and   study_hdr_id   = #tmpInstDtls.study_id
						  and   head_id= #tmpInstDtls.modality_id 
						  and   head_type ='M'),0))
		where status_id=100
		and modality_amended_rate ='Y'
			
		update #tmpInstDtls 
		set rate=(isnull((select bar.fee_amount 
			              from billing_account_rates_fee_schedule bar
						  inner join rates_fee_schedule_template rfs on bar.rate_id=rfs.id
						  where billing_account_id = #tmpInstDtls.billing_account_id 
						  and  rfs.head_id= #tmpInstDtls.modality_id 
						  and  rfs.head_type ='M'
						  and  rfs.img_count_from <=#tmpInstDtls.image_count 
						  and  rfs.img_count_to > = #tmpInstDtls.image_count),0))
		where status_id=100
		and modality_amended_rate ='N'

		update #tmpInstDtls 
		set	amount= rate,
		    study_price = rate
		where status_id=100



		--select * from #tmpInstDtls --where institution_id='761562B3-79CA-40D2-B43B-EEE2A178D58B'

		select  @rowcount=(select count(rec_id) from #tmpInstDtls),
		        @counter =1

	    while(@counter <= @rowcount)
			begin
				create table #tmpServices
				(
					row_id int identity(1,1),
					service_id int null default 0,
					service_code nvarchar(10) null default '',
					amount money null default 0,
					price  money null default 0
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

				select  @bill_cycle_id    = billing_cycle_id,
						@bill_account_id  = billing_account_id,
						@inst_id          = institution_id ,   
						@recived_date     = received_date,
						@study_id         = study_id,
						@study_uid        = study_uid,
						@modality_id      = modality_id,
						@status_id        = status_id,
						@service_code     = service_codes,
						@priority_id      = priority_id,
						@disc_per         = applied_discount
				from #tmpInstDtls
				where rec_id = @counter

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
							
							--print @promotion_id_disc
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
							--print @promotion_id_fc
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
						--and institution_id = @institution_id
						--and modality_id    = @modality_id
						----and billing_cycle_id in (select billing_cycle_id from billing_cycle where date_till < @date_from)
						--and is_free = 'Y'

						
						--print @free_credits_consumed

						select @free_credits_consumed_tmp = count(is_free)
						from #tmpInstDtls
						where promotion_id = @promotion_id
						--and institution_id = @institution_id
						--and modality_id    = @modality_id
						--and is_free = 'Y'

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
						if(charindex('CONSULT',@service_code))>0
							begin
								select @svc_id= id from services where code='CONSULT'

								select @consult_amount=fee_amount 
								from billing_account_rates_fee_schedule
								where billing_account_id = @bill_account_id
								and rate_id = (select id from rates_fee_schedule_template where head_id=@svc_id and head_type='S')

								set @consult_amount= isnull(@consult_amount,0)

								update #tmpInstDtls
								set rate= @consult_amount,
									amount = @consult_amount,
									study_price = @consult_amount
								where rec_id= @counter
							end


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

				 --select * from #tmpServices
				 --print @rc
				 --print @ctr			 

				 while(@ctr <= @rc)
					begin
						
						--set @service_price   = 0
						set @service_amount  = 0

						select @svc_id      = service_id,
						       @svc_code    = service_code
						from #tmpServices
						where row_id = @ctr

						--print @svc_id

						if(@service_code='CONSULT')
							begin
								if(select count(study_hdr_id) from ar_amended_rates where study_hdr_id = @study_id and head_type='S' and head_id=@svc_id and study_hdr_id=@study_id and billing_account_id=@bill_account_id and billing_cycle_id=@bill_cycle_id)=0
									begin
										select @service_amount=bafs.fee_amount 
										from billing_account_rates_fee_schedule bafs
										inner join rates_fee_schedule_template fst on fst.id = bafs.rate_id
										where bafs.billing_account_id = @bill_account_id
										and fst.head_id = @svc_id
										and fst.head_type = 'S'
									end
								else
									begin
										select @service_amount=rate 
										from ar_amended_rates
										where billing_account_id = @bill_account_id
										and billing_cycle_id    = @bill_cycle_id
										and study_hdr_id        = @study_id
										and head_id = @svc_id
										and head_type = 'S'
									end
							end
						else
							begin
								if(select count(study_hdr_id) from ar_amended_rates where study_hdr_id = @study_id and head_type='S' and head_id=@svc_id  and study_hdr_id=@study_id and billing_account_id=@bill_account_id and billing_cycle_id=@bill_cycle_id)=0
									begin
										select  @service_amount=fee_amount 
										from billing_account_rates_fee_schedule
										where billing_account_id = @bill_account_id
										and rate_id = (select id from rates_fee_schedule_template where head_id=@svc_id and head_type='S')
									end
								else
									begin
										select @service_amount=rate 
										from ar_amended_rates
										where billing_account_id = @bill_account_id
										and billing_cycle_id    = @bill_cycle_id
										and study_hdr_id        = @study_id
										and head_id = @svc_id
										and head_type = 'S'
									end
							end

						set @service_amount  = isnull(@service_amount,0)
						
						--print @service_amount

						update #tmpServices
						set amount = isnull(@service_amount,0),
							price = isnull(@service_amount,0)
						where row_id = @ctr

					    --select * from #tmpServices

						insert into #tmpServiceDtls(billing_cycle_id,billing_account_id,institution_id,study_id,study_uid,service_id,priority_id,amount,price)
						                     values(@bill_cycle_id,@bill_account_id,@inst_id,@study_id,@study_uid,@svc_id,@priority_id,@service_amount,@service_amount)

						set @ctr = @ctr + 1
					end

				--select * from #tmpServiceDtls
				set @service_total=0
				set @service_price=0
				select @service_total = isnull((select sum(amount) from #tmpServices),0)
				set @service_price = isnull(@service_total,0)
				
				if(@disc_per>0 and @status_id = 100)
					begin
						
						select @disc_amount = ((@disc_per/100) * (amount + @service_total))
						from #tmpInstDtls
						where rec_id = @counter

						--print @disc_amount

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
				else if(@is_free='Y' and @status_id = 100)
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


		--select * from #tmpServiceDtls
		--select * from #tmpInstDtls where institution_id='C2E366B7-5F6E-4C65-8AC2-8449FCED3E2B' --where billing_account_id='1A1D9DEE-8D88-4ACB-9861-B0A254C30E34'
		-- update #tmpInstFinal


		select  @rowcount=(select count(institution_id) from #tmpInstFinal)

		if(@rowcount > 0)	
			begin
				set @counter = 1
				while(@counter <= @rowcount)
					begin
						--update #tmpInstFinal
						--set total_amount=(select sum(amount) + sum(isnull(service_total,0)) 
						--                  from #tmpInstDtls 
						--				  where institution_id =(select institution_id from #tmpInst where rec_id=@counter))
						--where rec_id=@counter

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
		
		--select * from #tmpInstFinal
		-- update #tmpInvHdr
		--select * from #tmpInvHdr
		--select * from #tmpInvHdrFinal

		select  @rowcount=(select count(billing_account_id) from #tmpInvHdr)

		--print @rowcount

		if(@rowcount > 0)
			begin
				set @counter = 1
				while(@counter <= @rowcount)
					begin
						select @billing_acc_id=billing_account_id 
						from #tmpInvHdrFinal 
						where rec_id=@counter

						select @approved = approved
						from invoice_hdr 
						where billing_cycle_id=@billing_cycle_id 
						and billing_account_id=@billing_acc_id

						set @approved= isnull(@approved,'N')

						--print @billing_acc_id

						if(@approved ='N')
							begin
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

									--print @total_study_count
									-- print       @total_study_count_std
									--	print	@total_study_count_stat
									--print		@total_amount
									--print		@total_disc_amount
									--	print	@total_free_credits

									/****************SAVE INTO invoice_hdr TABLE*********************/
						
									if(select count(id) from invoice_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id)>0
										begin
											--select @inv_id = id,
											--	   @approved = approved
											--from invoice_hdr 
											--where billing_cycle_id=@billing_cycle_id 
											--and billing_account_id=@billing_acc_id
											--print @inv_id

											--update invoice_hdr
											--set total_study_count=@total_study_count,
											--	total_study_count_std   = @total_study_count_std,
											--	total_study_count_stat  = @total_study_count_stat,
											--	total_amount            = @total_amount,
											--	total_disc_amount       = @total_disc_amount,
											--	total_free_credits      = @total_free_credits
											--where id = @inv_id

											delete from invoice_service_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_acc_id
											delete from invoice_institution_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_acc_id
											delete from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_acc_id
											delete from invoice_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_acc_id
								
										end
									
									
									set @inv_id=newid()
									insert into invoice_hdr(id,billing_cycle_id,billing_account_id,total_study_count,total_study_count_std,total_study_count_stat,
															total_amount,total_disc_amount,total_free_credits,created_by,date_created)
														values(@inv_id,@billing_cycle_id,@billing_acc_id,@total_study_count,@total_study_count_std,@total_study_count_stat,
															@total_amount,@total_disc_amount,@total_free_credits,@user_id,getdate())

									if(@@rowcount=0)
										begin
											rollback transaction
											select @user_name = name from billing_account where id = @billing_acc_id
											select @error_code='230',@return_status=0
											return 0
										end
									/****************SAVE INTO invoice_hdr TABLE*********************/

									--select * from invoice_hdr where id = @inv_id
									insert into #tmpInst1(institution_id,institution_code,institution_name,total_study_count,total_study_count_std,total_study_count_stat,
														  total_amount,total_disc_amount,total_free_credits)
									(select institution_id,institution_code,institution_name,total_study_count,total_study_count_std,total_study_count_stat,
											total_amount,total_disc_amount,total_free_credits
									 from #tmpInstFinal
									 where billing_account_id=@billing_acc_id)

									select  @rowcount1=@@rowcount,@counter1=1
									--print @rowcount1

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

													/****************SAVE INTO invoice_institution_hdr TABLE*********************/
													--if((select count(id) from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id and institution_id=@institution_id)>0)
													--	begin
													--		select @inv_inst_hdr_id=id from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id and institution_id=@institution_id

													--		update invoice_institution_hdr
													--		set total_study_count       = @total_study_count,
													--			total_study_count_std   = @total_study_count_std,
													--			total_study_count_stat  = @total_study_count_stat,
													--			total_amount            = @total_amount,
													--			total_disc_amount       = @total_disc_amount,
													--			free_read_count         = @total_free_credits
													--		--where id = @inv_inst_hdr_id
													--		where billing_cycle_id=@billing_cycle_id 
													--		and billing_account_id=@billing_acc_id 
													--		and institution_id=@institution_id
													--	end
													--else
													--	begin
															
														--end

													set @inv_inst_hdr_id=newid()

													insert into invoice_institution_hdr(id,hdr_id,billing_cycle_id,billing_account_id,institution_id,total_study_count,total_study_count_std,total_study_count_stat,
																						total_amount,total_disc_amount,free_read_count,created_by,date_created)
																					values(@inv_inst_hdr_id,@inv_id,@billing_cycle_id,@billing_acc_id,@institution_id,@total_study_count,@total_study_count_std,@total_study_count_stat,
																						@total_amount,@total_disc_amount,@total_free_credits,@user_id,getdate())

													if(@@rowcount=0)
														begin
															rollback transaction
															select @user_name = name from institutions where id = @institution_id
															select @error_code='231',@return_status=0
															return 0
														end

													/****************SAVE INTO invoice_institution_hdr TABLE*********************/

													insert into #tmpInstDtls1(study_id,study_uid,modality_id,modality_name,patient_name,image_count,rate,amount,study_price,service_total,service_price,
																			  applied_discount,disc_amount,is_free,promotion_id)
													(select study_id,study_uid,modality_id,modality_name,patient_name,image_count,rate,amount,study_price,service_total,service_price,
															applied_discount,disc_amount,is_free,promotion_id
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
																			@modality_id=modality_id,
																			@img_count=image_count,
																			@rate=rate,
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

																	/****************SAVE INTO invoice_institution_dtls TABLE*********************/
																	--if(select count(id) from invoice_institution_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id and institution_id=@institution_id and study_id=@study_id)= 0
																	--	begin
																			set @inv_inst_dtls_id = newid()
																			insert into invoice_institution_dtls(id,hdr_id,institution_hdr_id,billing_cycle_id,billing_account_id,institution_id,study_id,study_uid,modality_id,image_count,
																												  rate,amount,service_amount,study_price,service_price,disc_per_applied,disc_amount,is_free,promotion_id,total_amount,
																												  approved,approved_by,date_approved,created_by,date_created)
																										  values(@inv_inst_dtls_id,@inv_id,@inv_inst_hdr_id,@billing_cycle_id,@billing_acc_id,@institution_id,@study_id,@study_uid,@modality_id,@img_count,
																												 @rate,@amount,@service_amount,@study_price,@service_price,@disc_per,@disc_amount,@is_free,@promotion_id,@amount + @service_amount,@approved,@approved_by,@date_approved,
																												 @user_id,getdate())
																		--end
																	--else
																	--	begin
																	--		select @inv_inst_dtls_id = id
																	--		from invoice_institution_dtls
																	--		where billing_cycle_id=@billing_cycle_id 
																	--		and billing_account_id=@billing_acc_id 
																	--		and institution_id=@institution_id 
																	--		and study_id=@study_id

																	--		update invoice_institution_dtls
																	--		set modality_id      = @modality_id,
																	--			image_count      = @img_count,
																	--			rate             = @rate,
																	--			amount           = @amount,
																	--			study_price      = @study_price,
																	--			service_amount   = @service_amount,
																	--			service_price    = @service_price,
																	--			disc_per_applied = @disc_per,
																	--			disc_amount      = @disc_amount,
																	--			is_free          = @is_free,
																	--			promotion_id     = @promotion_id,
																	--			total_amount     = @amount + @service_amount,
																	--			approved         = @approved,
																	--			approved_by      = @approved_by,
																	--			date_approved    = @date_approved
																	--		where billing_cycle_id=@billing_cycle_id 
																	--		and billing_account_id=@billing_acc_id 
																	--		and institution_id=@institution_id 
																	--		and study_id=@study_id

																	--	end
														
																	if(@@rowcount=0)
																		begin
																			rollback transaction
																			select @user_name = name from institutions where id = @institution_id
																			select @error_code='231',@return_status=0
																			return 0
																		end
																	/****************SAVE INTO invoice_institution_dtls TABLE*********************/

																	/****************SAVE INTO invoice_service_dtls TABLE*********************/
																	--delete from invoice_service_dtls
																	--where billing_cycle_id= @billing_cycle_id
																	--and billing_account_id = @billing_acc_id
																	--and institution_id = @institution_id
																	--and study_id=@study_id


																	if(select count(study_id) 
																	   from #tmpServiceDtls
																	   where billing_cycle_id= @billing_cycle_id
																		and billing_account_id = @billing_acc_id
																		and institution_id = @institution_id
																		and study_id=@study_id) >0
																			begin
																				insert into invoice_service_dtls(id,hdr_id,institution_hdr_id,institution_dtls_id,
																												 billing_cycle_id,billing_account_id,institution_id,
																												 study_id,study_uid,service_id,priority_id,amount,service_price,
																												 disc_per_applied,is_free,updated_by,date_updated)
																										  (select newid(),@inv_id,@inv_inst_hdr_id,@inv_inst_dtls_id,
																												 billing_cycle_id,billing_account_id,institution_id,
																												 study_id,study_uid,service_id,priority_id,amount,price,
																												 disc_per_applied,is_free,@user_id,getdate()
																											from #tmpServiceDtls
																											where billing_cycle_id=@billing_cycle_id
																											and billing_account_id=@billing_acc_id 
																											and institution_id=@institution_id
																											and study_id=@study_id)

																				if(@@rowcount=0)
																					begin
																						rollback transaction
																						select @user_name = name from institutions where id = @institution_id
																						select @error_code='253',@return_status=0
																						return 0
																					end
																			end

																	set @counter2 = @counter2 + 1
																	/****************SAVE INTO invoice_service_dtls TABLE*********************/
																end

															truncate table #tmpInstDtls1

															/****************UPDATE invoice_institution_hdr TABLE*********************/
															update invoice_institution_hdr
															set total_amount=(select sum(total_amount) from invoice_institution_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id and institution_id=@institution_id and billed='Y')
															where billing_cycle_id=@billing_cycle_id 
															and billing_account_id=@billing_acc_id 
															and institution_id=@institution_id

															if(@@rowcount=0)
																begin
																	rollback transaction
																	select @user_name = name from institutions where id = @institution_id
																	select @error_code='231',@return_status=0
																	return 0
																end
															/****************UPDATE invoice_institution_hdr TABLE*********************/
														end
													-- End Inst dtls

													/****************UPDATE invoice_hdr TABLE*********************/
													update invoice_hdr
													set total_amount=(select sum(total_amount) from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id=@billing_acc_id)
													where billing_cycle_id=@billing_cycle_id 
													and billing_account_id=@billing_acc_id 

													if(@@rowcount=0)
														begin
															rollback transaction
															select @user_name = name from institutions where id = @institution_id
															select @error_code='231',@return_status=0
															return 0
														end
													/****************UPDATE invoice_hdr TABLE*********************/

													set @counter1 = @counter1 + 1
										
												end
				
										end --End inv_inst hdr
							end

						truncate table #tmpInst1
						set @counter = @counter + 1
					end
				
			end
		else
			begin
				if(@billing_account_id = '00000000-0000-0000-0000-000000000000')
					begin
						delete from invoice_service_dtls where billing_cycle_id=@billing_cycle_id
						delete from invoice_institution_dtls where billing_cycle_id=@billing_cycle_id
						delete from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id
						delete from invoice_hdr where billing_cycle_id=@billing_cycle_id
					end
				else
					begin
						delete from invoice_service_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_account_id
						delete from invoice_institution_dtls where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_account_id
						delete from invoice_institution_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_account_id
						delete from invoice_hdr where billing_cycle_id=@billing_cycle_id and billing_account_id = @billing_account_id
					end
			end

		delete from invoice_service_dtls 
		where billing_cycle_id = @billing_account_id
		and study_id not in (select distinct dtls.study_id
		                     from invoice_institution_dtls dtls
							 inner join invoice_hdr ih on ih.id = dtls.hdr_id
							 where dtls.billing_cycle_id = @billing_cycle_id
							 and ih.approved='N')

		delete from invoice_institution_hdr 
		where billing_cycle_id = @billing_account_id
		and institution_id not in (select distinct dtls.institution_id
		                           from invoice_institution_dtls dtls
								   inner join invoice_hdr ih on ih.id = dtls.hdr_id
								   where dtls.billing_cycle_id = @billing_cycle_id
								   and ih.approved='N')
		
		delete from invoice_hdr 
		where billing_cycle_id = @billing_cycle_id
		and approved = 'N'
		and billing_account_id not in (select distinct billing_account_id
		                               from invoice_institution_hdr
									   where billing_cycle_id = @billing_cycle_id)

		--select * from #tmpZAInvHdr

		 /*****************ZERO AMOUNT INVOICE*************************/
		 if(@billing_account_id = '00000000-0000-0000-0000-000000000000')
			begin
				 /*****************ZERO AMOUNT BILLING ACCOUNTS*************************/
				 insert into #tmpZAInvHdr(billing_account_id,billing_acc_name)
				 (select id,name 
				 from billing_account
				 where is_active='Y'
				 and id not in (select billing_account_id from #tmpInvHdr))

				 insert into #tmpZAInvHdr(billing_account_id,billing_acc_name)
				 (select id,name 
				 from billing_account
				 where is_active='Y'
				 and id in (select billing_account_id 
				            from institutions
							where is_active='Y'
							and id not in (select institution_id from #tmpInstFinal) ))
				/*****************ZERO AMOUNT BILLING ACCOUNTS*************************/
				
			end
		else
			begin
				 /*****************ZERO AMOUNT BILLING ACCOUNTS*************************/
				 --if(select count(billing_account_id) from #tmpInvHdr where billing_account_id=@billing_account_id) =0
					--begin
						 insert into #tmpZAInvHdr(billing_account_id,billing_acc_name)
						 (select id,name 
						 from billing_account
						 where is_active='Y'
						 and id = @billing_account_id)
					--end
				/*****************ZERO AMOUNT BILLING ACCOUNTS*************************/
			
			end

		 set @counter=1
		 select @rowcount=count(rec_id) from #tmpZAInvHdr

		 while(@counter <= @rowcount)
			begin
				select @billing_acc_id = billing_account_id
				from #tmpZAInvHdr
				where rec_id = @counter

				if(select count(id) from invoice_hdr where billing_account_id=@billing_acc_id and billing_cycle_id=@billing_cycle_id)=0
					begin
						set @inv_id=newid()
						insert into invoice_hdr(id,billing_cycle_id,billing_account_id,total_study_count,total_study_count_std,total_study_count_stat,
												total_amount,total_disc_amount,total_free_credits,created_by,date_created)
										 values(@inv_id,@billing_cycle_id,@billing_acc_id,0,0,0,
												0,0,0,@user_id,getdate())

						if(@@rowcount=0)
							begin
								rollback transaction
								select @user_name = name from billing_account where id = @billing_acc_id
								select @error_code='230',@return_status=0
								return 0
							end
					end
				else
					begin
						select @inv_id = id from invoice_hdr where billing_account_id=@billing_acc_id and billing_cycle_id=@billing_cycle_id
					end

				insert into #tmpZAInst(billing_account_id,institution_id,institution_code,institution_name)
				(select billing_account_id,id,code,name 
				 from institutions
				 where is_active='Y'
				 and billing_account_id = @billing_acc_id
				 and id not in (select institution_id 
				                from invoice_institution_hdr 
								where hdr_id = @inv_id
								and billing_account_id = @billing_acc_id
								and billing_cycle_id = @billing_cycle_id))

				set @rowcount1 = @@rowcount

				if(@rowcount1>0)
					begin
						insert into invoice_institution_hdr(id,hdr_id,billing_cycle_id,billing_account_id,institution_id,total_study_count,total_study_count_std,total_study_count_stat,
															total_amount,total_disc_amount,free_read_count,created_by,date_created)
													 (select newid(),@inv_id,@billing_cycle_id,@billing_acc_id,institution_id,0,0,0,
															 0,0,0,@user_id,getdate()
													  from #tmpZAInst)

						if(@@rowcount=0)
							begin
								rollback transaction
								select @user_name = name from billing_account where id = @billing_acc_id
								select @error_code='304',@return_status=0
								return 0
							end
					end

				set @counter = @counter + 1
				truncate table #tmpZAInst
			end

		 /*****************ZERO AMOUNT INVOICE*************************/

		commit transaction
		set @return_status=1
	    set @error_code=''

		if(@billing_account_id ='00000000-0000-0000-0000-000000000000')
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
				where billing_cycle_id = @billing_cycle_id
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
				where billing_cycle_id = @billing_cycle_id
				order by i.name

				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   priority_desc = isnull(p.priority_desc,'Unknown'),
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
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr sh on sh.id = iid.study_id
				left outer join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				union
				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   priority_desc = isnull(p.priority_desc,'Unknown'),
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
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr_archive sh on sh.id = iid.study_id
				left outer join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				order by sh.received_date
				
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
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   priority_desc = isnull(p.priority_desc,'Unknown'),
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
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr sh on sh.id = iid.study_id
				left outer join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.billing_account_id=@billing_account_id
				union
				select iid.billing_account_id,
					   iid.billing_cycle_id,
					   iid.institution_id,
					   iid.study_id,
					   iid.study_uid,
					   sh.received_date,
					   iid.modality_id,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   sh.priority_id,
					   priority_desc = isnull(p.priority_desc,'Unknown'),
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
							when iid.disc_per_applied>0 then 'Discount of ' + convert(varchar(6),disc_per_applied) + '% ($' + convert(varchar(12),disc_amount)	 + ') applied'
							else ''
					   end promo_dtls
				from invoice_institution_dtls iid
				left outer join modality m on m.id= iid.modality_id
				inner join study_hdr_archive sh on sh.id = iid.study_id
				left outer join sys_priority p on p.priority_id = sh.priority_id
				where iid.billing_cycle_id = @billing_cycle_id
				and iid.billing_account_id=@billing_account_id
				order by sh.received_date
			end

	
		drop table #tmpInvHdr
		drop table #tmpInvHdrFinal
		drop table #tmpInst
		drop table #tmpInstFinal
		drop table #tmpInstDtls
		drop table #tmpInst1
		drop table #tmpInstDtls1
		drop table #tmpServiceDtls
		drop table #tmpZAInvHdr
		drop table #tmpZAInst

		if(select count(record_id) from sys_record_lock_ui where record_id=@billing_cycle_id and menu_id=@menu_id)=0
			begin
				exec common_lock_record_ui
					@menu_id       = @menu_id,
					@record_id     = @billing_cycle_id,
					@user_id       = @user_id,
					@error_code    = @error_code output,
					@return_status = @return_status output	
						
				if(@return_status=0)
					begin
						return 0
					end
			end

		
		set nocount off
		return 1
	end
GO
