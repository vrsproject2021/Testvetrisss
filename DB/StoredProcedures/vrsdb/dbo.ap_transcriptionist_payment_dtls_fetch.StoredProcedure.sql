USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_transcriptionist_payment_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_transcriptionist_payment_dtls_fetch : 
                  fetch transcriptionist payment details
** Created By   : Pavel Guha
** Created On   : 26/10/2020
*******************************************************/
create procedure [dbo].[ap_transcriptionist_payment_dtls_fetch]
	@billing_cycle_id uniqueidentifier,
	@transcriptionist_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@menu_id int,
    @user_id uniqueidentifier,
    @user_name nvarchar(500) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
	begin
		set nocount on
		declare @date_from				datetime,
				@date_till				datetime,
				@billing_cycle_name		nvarchar(50),
				@counter				bigint,
				@rowcount				bigint,
				@trans_id					uniqueidentifier,
				@rad_pay_id				uniqueidentifier


		 exec common_check_record_lock
				@menu_id       = @menu_id,
				@record_id     = @menu_id,
				@user_id       = @user_id,
				@user_name     = @user_name		output,
				@error_code    = @error_code	output,
				@return_status = @return_status output
		
		if(@return_status=0)
			begin
				return 0
			end


		create table #tmpTransPayHdr
		(
			rec_id					       int identity(1,1),
			billing_cycle_id		       uniqueidentifier,
			billing_cycle_name		       nvarchar(150),
			transcriptionist_id			   uniqueidentifier,
			transcriptionist_name		   nvarchar(250),
			total_study_count		      int null default 0,
			total_study_count_std         int null default 0,
			total_study_count_stat        int null default 0,
			total_std_amount              money	 default 0,
			total_stat_amount             money	 default 0,
			total_adhoc_amount			  money	 default 0,
			total_amount			      money	 default 0
		)
		
		create table #tmpTransPayDtls
		(
			rec_id				int identity(1,1),
			billing_cycle_id	uniqueidentifier,
			transcriptionist_id		uniqueidentifier,
			study_id			uniqueidentifier,
			study_uid			nvarchar(100),
			modality_id			int,
			modality_name		nvarchar(50),
			priority_id			int,
			patient_name		nvarchar(200),
			rate			    money default 0,
			amount		        money default 0,
			addl_stat_rate      money default 0,
			adhoc_payment       money default 0,
			total_amount        money default 0,
		)
		

		--**************************

		delete from sys_record_lock where user_id = @user_id
	    delete from sys_record_lock_ui where user_id = @user_id

		select  @billing_cycle_name = name,
				@date_from			= convert(date, date_from,103),
				@date_till			= convert(date,date_till,103)
		from billing_cycle
		where id = @billing_cycle_id

		--print @date_from
		--print @date_till

		

		if(@transcriptionist_id = '00000000-0000-0000-0000-000000000000')
			begin
				insert into #tmpTransPayHdr(billing_cycle_id,billing_cycle_name,transcriptionist_id,transcriptionist_name)
				(select
						@billing_cycle_id,
						@billing_cycle_name,
						id,
						name
					from transciptionists
					where is_active='Y'
					and id not in (select transcriptionist_id
					               from ap_transcriptionist_payment_hdr
								   where billing_cycle_id = @billing_cycle_id
								   and isnull(approved,'N')='Y')
				)
				order by name
			end
		else
			begin
				insert into #tmpTransPayHdr(billing_cycle_id,billing_cycle_name,transcriptionist_id,transcriptionist_name)
				(select
						@billing_cycle_id,
						@billing_cycle_name,
						id,
						name
					from transciptionists
					where is_active='Y'
					and id = @transcriptionist_id
					and id not in (select transcriptionist_id
					               from ap_transcriptionist_payment_hdr
								   where billing_cycle_id = @billing_cycle_id
								   and isnull(approved,'N')='Y')
				)
			end

		select @rowcount = @@rowcount,@counter=1

		while(@counter<=@rowcount)
			begin
				select @trans_id = transcriptionist_id
				from #tmpTransPayHdr
				where rec_id=@counter

				insert into #tmpTransPayDtls(billing_cycle_id,transcriptionist_id,study_id,study_uid,modality_id,modality_name,
								           priority_id,patient_name)
						(select @billing_cycle_id,
						        @trans_id,
								sh.id, 
								sh.study_uid,
								sh.modality_id,
								modality_name = dbo.InitCap(isnull(m.name,'Unknown')),
								sh.priority_id,
								patient_name=dbo.InitCap(isnull(sh.patient_name,''))
							from study_hdr sh
							left outer join transciptionists t on sh.dict_tanscriptionist_id=t.id
							left outer join modality m on m.id=sh.modality_id
							where sh.dict_tanscriptionist_id = @trans_id
							and convert(datetime,convert(varchar(11),sh.received_date,106)) between @date_from and @date_till
							and sh.study_status_pacs=100
							and sh.deleted ='N'
							union
							select @billing_cycle_id,
								@trans_id,
								sh.id, 
								sh.study_uid,
								sh.modality_id,
								modality_name = dbo.InitCap(isnull(m.name,'Unknown')),
								sh.priority_id,
								patient_name=dbo.InitCap(isnull(sh.patient_name,''))
							from study_hdr_archive sh
							left outer join transciptionists t on sh.dict_tanscriptionist_id=t.id
							left outer join modality m on m.id=sh.modality_id
							where sh.dict_tanscriptionist_id = @trans_id
							and convert(datetime,convert(varchar(11),sh.received_date,106)) between @date_from and @date_till
							and sh.study_status_pacs=100
							and sh.deleted ='N')
					
					--print @@rowcount
					--print @trans_id
					

				set @counter= @counter + 1
			end


		--select * from #tmpTransPayDtls
		update #tmpTransPayDtls
		set adhoc_payment = isnull((select adhoc_payment
		                            from ap_transcriptionist_adhoc_payments
									where billing_cycle_id = @billing_cycle_id
									and transcriptionist_id = #tmpTransPayDtls.transcriptionist_id
									and study_id = #tmpTransPayDtls.study_id),0)
		


		

		update #tmpTransPayDtls
		set rate = isnull((select default_fee
		                   from transcriptionist_modality_link
						   where modality_id = #tmpTransPayDtls.modality_id
						   and transcriptionist_id = #tmpTransPayDtls.transcriptionist_id),0)

		

		update #tmpTransPayDtls
		set addl_stat_rate = isnull((select addl_stat_fee
		                          from transcriptionist_modality_link
								  where modality_id = #tmpTransPayDtls.modality_id
								  and transcriptionist_id = #tmpTransPayDtls.transcriptionist_id
								  and priority_id = 10),0)

		update #tmpTransPayDtls set amount = rate 
		update #tmpTransPayDtls set total_amount = amount + adhoc_payment 

		update #tmpTransPayHdr
		set total_study_count_std = isnull((select count(study_id)
											from #tmpTransPayDtls
											where priority_id = 20
											and transcriptionist_id = #tmpTransPayHdr.transcriptionist_id),0)

		update #tmpTransPayHdr
		set total_study_count_stat = isnull((select count(study_id)
											 from #tmpTransPayDtls
											 where priority_id = 10
											 and transcriptionist_id = #tmpTransPayHdr.transcriptionist_id),0)

		update #tmpTransPayHdr
		set total_study_count	= total_study_count_std + total_study_count_stat



		update #tmpTransPayHdr
		set total_std_amount = isnull((select sum(amount)
		                               from #tmpTransPayDtls
									   where transcriptionist_id = #tmpTransPayHdr.transcriptionist_id
									   and priority_id=20),0)

	    update #tmpTransPayHdr
		set total_stat_amount = isnull((select sum(amount)
		                               from #tmpTransPayDtls
									   where transcriptionist_id = #tmpTransPayHdr.transcriptionist_id
									   and priority_id=10),0)

		update #tmpTransPayHdr
		set total_adhoc_amount = isnull((select sum(adhoc_payment)
		                               from #tmpTransPayDtls
									   where transcriptionist_id = #tmpTransPayHdr.transcriptionist_id),0)

		update #tmpTransPayHdr
		set total_amount = total_std_amount + total_stat_amount + total_adhoc_amount

		select @rowcount = count(rec_id),
		       @counter  =1
		from #tmpTransPayHdr

		begin transaction
		
		while(@counter <= @rowcount)
			begin

				select @trans_id = transcriptionist_id
				from #tmpTransPayHdr
				where rec_id=@counter

				delete from ap_transcriptionist_payment_hdr
				where billing_cycle_id=@billing_cycle_id
				and transcriptionist_id = @trans_id

				delete from ap_transcriptionist_payment_dtls 
				where billing_cycle_id=@billing_cycle_id
				and transcriptionist_id = @trans_id

				set @rad_pay_id= newid()

				insert into ap_transcriptionist_payment_hdr(id,billing_cycle_id,transcriptionist_id,
				                                           total_study_count,total_study_count_std,total_study_count_stat,
													       total_std_amount,total_stat_amount,total_adhoc_amount,total_amount,created_by,date_created)
											   (select @rad_pay_id,billing_cycle_id,transcriptionist_id,
				                                       total_study_count,total_study_count_std,total_study_count_stat,
													   total_std_amount,total_stat_amount,total_adhoc_amount,total_amount,@user_id,getdate()
											    from #tmpTransPayHdr
												where transcriptionist_id = @trans_id)

				if(@@rowcount=0)
					begin
						rollback transaction
						select @user_name = name from transciptionists where id = @trans_id
						select @error_code='404',@return_status=0
						return 0
					end

				if(select count(study_id) from #tmpTransPayDtls where transcriptionist_id = @trans_id)>0
					begin
						insert into ap_transcriptionist_payment_dtls(id,hdr_id,billing_cycle_id,transcriptionist_id,study_id,study_uid,
																	 modality_id,priority_id,rate,amount,addl_stat_rate,adhoc_amount,total_amount,
																	 created_by,date_created)
													   (select newid(),@rad_pay_id,billing_cycle_id,transcriptionist_id,study_id,study_uid,
															   modality_id,priority_id,rate,amount,addl_stat_rate,adhoc_payment,total_amount,
															   @user_id,getdate()
														from #tmpTransPayDtls
														where transcriptionist_id = @trans_id)

						if(@@rowcount=0)
							begin
								rollback transaction
								select @user_name = name from transcriptionists where id = @trans_id
								select @error_code='404',@return_status=0
								return 0
							end
					end

				set @counter = @counter + 1
			end


		commit transaction
		set @return_status=1
	    set @error_code=''

		if(@transcriptionist_id ='00000000-0000-0000-0000-000000000000')
			begin
				select tph.transcriptionist_id,
					   tph.billing_cycle_id,
					   transcriptionist_name = dbo.InitCap(r.name),
					   tph.total_study_count,
					   tph.total_study_count_std,
					   tph.total_study_count_stat,
					   tph.total_amount,
					   tph.approved
				from ap_transcriptionist_payment_hdr tph
				inner join transciptionists r on r.id = tph.transcriptionist_id
				where billing_cycle_id = @billing_cycle_id
				order by r.name

				---*********************
				select row_number() over(order by transcriptionist_id,received_date) as row_id,
					   transcriptionist_id,
					   billing_cycle_id,
					   study_id,
					   study_uid,
					   received_date,
					   modality_name,
					   priority_desc,
					   institution_name,
					   patient_name,
					   amount,
					   adhoc_amount,
					   total_amount,
					   custom_report
				from
				(select tpd.transcriptionist_id,
					   tpd.billing_cycle_id,
					   tpd.study_id,
					   tpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   tpd.amount,
					   tpd.adhoc_amount,
					   tpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_transcriptionist_payment_dtls tpd
				left outer join modality m on m.id= tpd.modality_id
				left outer join sys_priority p on p.priority_id = tpd.priority_id
				inner join study_hdr sh on sh.id = tpd.study_id
				inner join institutions i on i.id = sh.institution_id
				where tpd.billing_cycle_id = @billing_cycle_id
				union
				select tpd.transcriptionist_id,
					   tpd.billing_cycle_id,
					   tpd.study_id,
					   tpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   tpd.amount,
					   tpd.adhoc_amount,
					   tpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_transcriptionist_payment_dtls tpd
				left outer join modality m on m.id= tpd.modality_id
				left outer join sys_priority p on p.priority_id = tpd.priority_id
				inner join study_hdr_archive sh on sh.id = tpd.study_id
				inner join institutions i on sh.institution_id=i.id
			    where tpd.billing_cycle_id = @billing_cycle_id) t
				---**************
				
			end
		else
			begin
				select tph.transcriptionist_id,
					   tph.billing_cycle_id,
					   transcriptionist_name = dbo.InitCap(r.name),
					   tph.total_study_count,
					   tph.total_study_count_std,
					   tph.total_study_count_stat,
					   tph.total_amount,
					   tph.approved
				from ap_transcriptionist_payment_hdr tph
				inner join transciptionists r on r.id = tph.transcriptionist_id
				where billing_cycle_id = @billing_cycle_id
				and tph.transcriptionist_id	= @transcriptionist_id
				order by r.name

				select row_number() over(order by transcriptionist_id,received_date) as row_id,
					   transcriptionist_id,
					   billing_cycle_id,
					   study_id,
					   study_uid,
					   received_date,
					   modality_name,
					   priority_desc,
					   institution_name,
					   patient_name,
					   amount,
					   adhoc_amount,
					   total_amount,
					   custom_report
				from
				(select tpd.transcriptionist_id,
					   tpd.billing_cycle_id,
					   tpd.study_id,
					   tpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   tpd.amount,
					   tpd.adhoc_amount,
					   tpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_transcriptionist_payment_dtls tpd
				left outer join modality m on m.id= tpd.modality_id
				left outer join sys_priority p on p.priority_id = tpd.priority_id
				inner join study_hdr sh on sh.id = tpd.study_id
				inner join institutions i on i.id = sh.institution_id
				where tpd.billing_cycle_id = @billing_cycle_id
				and tpd.transcriptionist_id = @transcriptionist_id
				union
				select tpd.transcriptionist_id,
					   tpd.billing_cycle_id,
					   tpd.study_id,
					   tpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   tpd.amount,
					   tpd.adhoc_amount,
					   tpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_transcriptionist_payment_dtls tpd
				left outer join modality m on m.id= tpd.modality_id
				left outer join sys_priority p on p.priority_id = tpd.priority_id
				inner join study_hdr_archive sh on sh.id = tpd.study_id
				inner join institutions i on sh.institution_id=i.id
			    where tpd.billing_cycle_id = @billing_cycle_id
				and tpd.transcriptionist_id = @transcriptionist_id) t
				

			end


	
		drop table #tmpTransPayHdr
		drop table #tmpTransPayDtls

		if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
			begin
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
			end



		set nocount off
		return 1
	end
GO
