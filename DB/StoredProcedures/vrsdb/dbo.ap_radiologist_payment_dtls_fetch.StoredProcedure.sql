USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_radiologist_payment_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_radiologist_payment_dtls_fetch : 
                  fetch radiologist payment details
** Created By   : BK
** Created On   : 26/12/2019
*******************************************************/
--exec ap_radiologist_payment_dtls_fetch '586953D4-2B3D-44DD-9349-B1C1B2701246','00000000-0000-0000-0000-000000000000',52,'11111111-1111-1111-1111-111111111111','','',0
--exec ap_radiologist_payment_dtls_fetch '3ACFB756-45AF-424C-80E5-5B66406E08A6','6CEFF867-BF6F-49A5-9A36-B3D8C5099F5F',52,'11111111-1111-1111-1111-111111111111','','',0


CREATE procedure [dbo].[ap_radiologist_payment_dtls_fetch]
	@billing_cycle_id uniqueidentifier,
	@radiologist_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
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
				@rad_id					uniqueidentifier,
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


		create table #tmpRadPayHdr
		(
			rec_id					      int identity(1,1),
			billing_cycle_id		      uniqueidentifier,
			billing_cycle_name		      nvarchar(150),
			radiologist_id			      uniqueidentifier,
			radiologist_name		      nvarchar(250),
			total_study_count		      int null default 0,
			total_study_count_prelim      int null default 0,
			total_study_count_prelim_std  int null default 0,
			total_study_count_prelim_stat int null default 0,
			total_study_count_final       int null default 0, 
			total_study_count_final_std   int null default 0,
			total_study_count_final_stat  int null default 0,
			total_study_count_std	      int null default 0,
			total_study_count_stat	      int null default 0,
			total_std_amount              money	 default 0,
			total_stat_amount             money	 default 0,
			total_adhoc_amount			  money	 default 0,
			total_amount			      money	 default 0
		)
		
		create table #tmpRadPayDtls
		(
			rec_id				int identity(1,1),
			billing_cycle_id	uniqueidentifier,
			radiologist_id		uniqueidentifier,
			study_id			uniqueidentifier,
			study_uid			nvarchar(100),
			modality_id			int,
			modality_name		nvarchar(50),
			priority_id			int,
			patient_name		nvarchar(200),
			image_count			int,
			is_reading_prelim   nchar(1) default 'N',
			is_reading_final    nchar(1) default 'N',
			rate_prelim			money default 0,
			amount_prelim		money default 0,
			rate_final			money default 0, 
			amount_final		money default 0,
			addl_stat_rate      money default 0,
			adhoc_payment       money default 0,
			total_amount        money default 0,
			read_radiologist_id    uniqueidentifier default '00000000-0000-0000-0000-000000000000',
			--prelim_radiologist_id  uniqueidentifier default '00000000-0000-0000-0000-000000000000',
			final_radiologist_id   uniqueidentifier default '00000000-0000-0000-0000-000000000000'
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

		

		if(@radiologist_id = '00000000-0000-0000-0000-000000000000')
			begin
				insert into #tmpRadPayHdr(billing_cycle_id,billing_cycle_name,radiologist_id,radiologist_name)
				(select
						@billing_cycle_id,
						@billing_cycle_name,
						id,
						name
					from radiologists
					where is_active='Y'
					and id not in (select radiologist_id
					               from ap_radiologist_payment_hdr
								   where billing_cycle_id = @billing_cycle_id
								   and isnull(approved,'N')='Y')
				)
				order by name
			end
		else
			begin
				insert into #tmpRadPayHdr(billing_cycle_id,billing_cycle_name,radiologist_id,radiologist_name)
				(select
						@billing_cycle_id,
						@billing_cycle_name,
						id,
						name
					from radiologists
					where is_active='Y'
					and id = @radiologist_id
					and id not in (select radiologist_id
					               from ap_radiologist_payment_hdr
								   where billing_cycle_id = @billing_cycle_id
								   and isnull(approved,'N')='Y')
				)
			end

		select @rowcount = @@rowcount,@counter=1

		while(@counter<=@rowcount)
			begin
				select @rad_id = radiologist_id
				from #tmpRadPayHdr
				where rec_id=@counter

				insert into #tmpRadPayDtls(billing_cycle_id,radiologist_id,study_id,study_uid,modality_id,modality_name,
								           priority_id,patient_name,image_count,
										   read_radiologist_id,--prelim_radiologist_id,
										   final_radiologist_id)
						(select @billing_cycle_id,
						        @rad_id,
								sh.id, 
								sh.study_uid,
								sh.modality_id,
								modality_name = dbo.InitCap(isnull(m.name,'Unknown')),
								sh.priority_id,
								patient_name=dbo.InitCap(isnull(sh.patient_name,'')),
								sh.img_count,
								isnull(sh.radiologist_id,'00000000-0000-0000-0000-000000000000'),
								--isnull(sh.prelim_radiologist_id,'00000000-0000-0000-0000-000000000000'),
								isnull(sh.final_radiologist_id,'00000000-0000-0000-0000-000000000000')
							from study_hdr sh
							left outer join radiologists rad on sh.radiologist_id=rad.id
							left outer join modality m on m.id=sh.modality_id
							where (sh.radiologist_id = @rad_id 
							      --or sh.prelim_radiologist_id = @rad_id 
								  or sh.final_radiologist_id = @rad_id)
							and convert(datetime,convert(varchar(11),sh.received_date,106)) between @date_from and @date_till
							and sh.study_status_pacs=100
							and sh.deleted ='N'
							union
							select @billing_cycle_id,
								@rad_id,
								sh.id, 
								sh.study_uid,
								sh.modality_id,
								modality_name = dbo.InitCap(isnull(m.name,'Unknown')),
								sh.priority_id,
								patient_name=dbo.InitCap(isnull(sh.patient_name,'')),
								sh.img_count,
								isnull(sh.radiologist_id,'00000000-0000-0000-0000-000000000000'),
								--isnull(sh.prelim_radiologist_id,'00000000-0000-0000-0000-000000000000'),
								isnull(sh.final_radiologist_id,'00000000-0000-0000-0000-000000000000')
							from study_hdr_archive sh
							left outer join radiologists rad on sh.radiologist_id=rad.id
							left outer join modality m on m.id=sh.modality_id
							where (sh.radiologist_id = @rad_id 
							      --or sh.prelim_radiologist_id = @rad_id 
								  or sh.final_radiologist_id = @rad_id)
							and convert(datetime,convert(varchar(11),sh.received_date,106)) between @date_from and @date_till
							and sh.study_status_pacs=100
							and sh.deleted ='N')
					
					--print @@rowcount
					--print @rad_id
					

				set @counter= @counter + 1
			end


		--select * from #tmpRadPayDtls
		update #tmpRadPayDtls
		set adhoc_payment = isnull((select adhoc_payment
		                            from ap_radiologist_adhoc_payments
									where billing_cycle_id = @billing_cycle_id
									and radiologist_id = #tmpRadPayDtls.radiologist_id
									and study_id = #tmpRadPayDtls.study_id),0)
		

		update #tmpRadPayDtls
		set final_radiologist_id = #tmpRadPayDtls.radiologist_id
		where isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000'
		--and (read_radiologist_id <> '00000000-0000-0000-0000-000000000000' or prelim_radiologist_id <> '00000000-0000-0000-0000-000000000000')
		and read_radiologist_id <> '00000000-0000-0000-0000-000000000000' 

		update #tmpRadPayDtls
		set read_radiologist_id = #tmpRadPayDtls.radiologist_id
		where isnull(final_radiologist_id,'00000000-0000-0000-0000-000000000000') = #tmpRadPayDtls.radiologist_id
		and read_radiologist_id = '00000000-0000-0000-0000-000000000000' 
		--and prelim_radiologist_id = '00000000-0000-0000-0000-000000000000'

		--update #tmpRadPayDtls
		--set read_radiologist_id = prelim_radiologist_id
		--where read_radiologist_id = '00000000-0000-0000-0000-000000000000' 
		--and prelim_radiologist_id <> '00000000-0000-0000-0000-000000000000'

		update #tmpRadPayDtls
		set read_radiologist_id = read_radiologist_id
		where read_radiologist_id <> '00000000-0000-0000-0000-000000000000' 
		--and prelim_radiologist_id = '00000000-0000-0000-0000-000000000000'

		update #tmpRadPayDtls
		set is_reading_prelim = 'Y'
		--where (read_radiologist_id = #tmpRadPayDtls.radiologist_id or prelim_radiologist_id=#tmpRadPayDtls.radiologist_id) 
		where read_radiologist_id = #tmpRadPayDtls.radiologist_id 

		update #tmpRadPayDtls
		set is_reading_final = 'Y'
		where final_radiologist_id = #tmpRadPayDtls.radiologist_id

		update #tmpRadPayDtls
		set rate_prelim = isnull((select prelim_fee
		                          from radiologist_modality_link
								  where modality_id = #tmpRadPayDtls.modality_id
								  and radiologist_id = #tmpRadPayDtls.radiologist_id),0)

		update #tmpRadPayDtls
		set rate_final = isnull((select final_fee
		                          from radiologist_modality_link
								  where modality_id = #tmpRadPayDtls.modality_id
								  and radiologist_id = #tmpRadPayDtls.radiologist_id),0)

		update #tmpRadPayDtls
		set addl_stat_rate = isnull((select addl_stat_fee
		                          from radiologist_modality_link
								  where modality_id = #tmpRadPayDtls.modality_id
								  and radiologist_id = #tmpRadPayDtls.radiologist_id),0)

		update #tmpRadPayDtls set amount_prelim = rate_prelim where is_reading_prelim='Y' and priority_id=20
		update #tmpRadPayDtls set amount_prelim = rate_prelim + addl_stat_rate where is_reading_prelim='Y' and priority_id=10
		update #tmpRadPayDtls set amount_final  = rate_final where is_reading_final='Y' and priority_id=20
		update #tmpRadPayDtls set amount_final  = rate_final + addl_stat_rate where is_reading_final='Y' and priority_id=10

		update #tmpRadPayDtls set total_amount = amount_prelim + amount_final + adhoc_payment 

		update #tmpRadPayHdr
		set total_study_count_prelim_std = isnull((select count(study_id)
												 from #tmpRadPayDtls
												 where is_reading_prelim='Y'
												 and priority_id = 20
												 and radiologist_id = #tmpRadPayHdr.radiologist_id),0)

		update #tmpRadPayHdr
		set total_study_count_prelim_stat = isnull((select count(study_id)
												   from #tmpRadPayDtls
												   where is_reading_prelim='Y'
												   and priority_id = 10
												   and radiologist_id = #tmpRadPayHdr.radiologist_id),0)

		update #tmpRadPayHdr
		set total_study_count_prelim 	= total_study_count_prelim_std + total_study_count_prelim_stat

		update #tmpRadPayHdr
		set total_study_count_final_std = isnull((select count(study_id)
												 from #tmpRadPayDtls
												 where is_reading_final='Y'
												 and priority_id = 20
												 and radiologist_id = #tmpRadPayHdr.radiologist_id),0)

		update #tmpRadPayHdr
		set total_study_count_final_stat = isnull((select count(study_id)
												   from #tmpRadPayDtls
												   where is_reading_final='Y'
												   and priority_id = 10
												   and radiologist_id = #tmpRadPayHdr.radiologist_id),0)

		update #tmpRadPayHdr
		set total_study_count_final 	= total_study_count_final_std + total_study_count_final_stat

		update #tmpRadPayHdr
		set total_study_count_std  = total_study_count_prelim_std + total_study_count_final_std,
		    total_study_count_stat = total_study_count_prelim_stat + total_study_count_final_stat

		update #tmpRadPayHdr
		set total_study_count	= total_study_count_prelim + total_study_count_final

		update #tmpRadPayHdr
		set total_std_amount = isnull((select sum(amount_prelim + amount_final)
		                               from #tmpRadPayDtls
									   where radiologist_id = #tmpRadPayHdr.radiologist_id
									   and priority_id=20),0)

	    update #tmpRadPayHdr
		set total_stat_amount = isnull((select sum(amount_prelim + amount_final)
		                               from #tmpRadPayDtls
									   where radiologist_id = #tmpRadPayHdr.radiologist_id
									   and priority_id=10),0)

		update #tmpRadPayHdr
		set total_adhoc_amount = isnull((select sum(adhoc_payment)
		                                 from #tmpRadPayDtls
									     where radiologist_id = #tmpRadPayHdr.radiologist_id),0)

		update #tmpRadPayHdr
		set total_adhoc_amount = total_adhoc_amount + isnull((select sum(adhoc_payment)
															  from ap_radiologist_other_adhoc_payments
															  where radiologist_id = #tmpRadPayHdr.radiologist_id
															  and billing_cycle_id = #tmpRadPayHdr.billing_cycle_id),0)


		update #tmpRadPayHdr
		set total_amount = total_std_amount + total_stat_amount + total_adhoc_amount

		select @rowcount = count(rec_id),
		       @counter  =1
		from #tmpRadPayHdr

		begin transaction
		
		while(@counter <= @rowcount)
			begin

				select @rad_id = radiologist_id
				from #tmpRadPayHdr
				where rec_id=@counter

				delete from ap_radiologist_payment_hdr
				where billing_cycle_id=@billing_cycle_id
				and radiologist_id = @rad_id

				delete from ap_radiologist_payment_dtls 
				where billing_cycle_id=@billing_cycle_id
				and radiologist_id = @rad_id

				set @rad_pay_id= newid()

				insert into ap_radiologist_payment_hdr(id,billing_cycle_id,radiologist_id,
				                                       total_study_count,total_study_count_prelim,total_study_count_final,total_study_count_prelim_std,total_study_count_prelim_stat,total_study_count_final_std,total_study_count_final_stat,
													   total_std_amount,total_stat_amount,total_adhoc_amount,total_amount,created_by,date_created)
											   (select @rad_pay_id,billing_cycle_id,radiologist_id,
				                                       total_study_count,total_study_count_prelim,total_study_count_final,total_study_count_prelim_std,total_study_count_prelim_stat,total_study_count_final_std,total_study_count_final_stat,
													   total_std_amount,total_stat_amount,total_adhoc_amount,total_amount,@user_id,getdate()
											    from #tmpRadPayHdr
												where radiologist_id = @rad_id)

				if(@@rowcount=0)
					begin
						rollback transaction
						select @user_name = name from radiologists where id = @rad_id
						select @error_code='395',@return_status=0
						return 0
					end

				if(select count(study_id) from #tmpRadPayDtls where radiologist_id = @rad_id)>0
					begin
						insert into ap_radiologist_payment_dtls(id,hdr_id,billing_cycle_id,radiologist_id,study_id,study_uid,
																modality_id,priority_id,is_reading_prelim,is_reading_final,
																prelim_rate,prelim_amount,final_rate,final_amount,addl_stat_rate,adhoc_amount,total_amount,
																created_by,date_created)
													   (select newid(),@rad_pay_id,billing_cycle_id,radiologist_id,study_id,study_uid,
															   modality_id,priority_id,is_reading_prelim,is_reading_final,
															   rate_prelim,amount_prelim,rate_final,amount_final,addl_stat_rate,adhoc_payment,total_amount,
															   @user_id,getdate()
														from #tmpRadPayDtls
														where radiologist_id = @rad_id)

						if(@@rowcount=0)
							begin
								rollback transaction
								select @user_name = name from radiologists where id = @rad_id
								select @error_code='395',@return_status=0
								return 0
							end
					end

				set @counter = @counter + 1
			end


		commit transaction
		set @return_status=1
	    set @error_code=''

		if(@radiologist_id ='00000000-0000-0000-0000-000000000000')
			begin
				select rph.radiologist_id,
					   rph.billing_cycle_id,
					   radiologist_name = dbo.InitCap(r.name),
					   rph.total_study_count_prelim,
					   rph.total_study_count_final,
					   rph.total_amount,
					   rph.approved
				from ap_radiologist_payment_hdr rph
				inner join radiologists r on r.id = rph.radiologist_id
				where billing_cycle_id = @billing_cycle_id
				order by r.name

				---*********************
				select row_number() over(order by radiologist_id,received_date) as row_id,
					   radiologist_id,
					   billing_cycle_id,
					   study_id,
					   study_uid,
					   received_date,
					   modality_name,
					   priority_desc,
					   institution_name,
					   patient_name,
					   is_reading_prelim,
					   is_reading_final,
					   prelim_amount,
					   final_amount,
					   adhoc_amount,
					   total_amount,
					   custom_report
				from
				(select rpd.radiologist_id,
					   rpd.billing_cycle_id,
					   rpd.study_id,
					   rpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   rpd.is_reading_prelim,
					   rpd.is_reading_final,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_radiologist_payment_dtls rpd
				left outer join modality m on m.id= rpd.modality_id
				left outer join sys_priority p on p.priority_id = rpd.priority_id
				inner join study_hdr sh on sh.id = rpd.study_id
				inner join institutions i on i.id = sh.institution_id
				where rpd.billing_cycle_id = @billing_cycle_id
				union
				select rpd.radiologist_id,
					   rpd.billing_cycle_id,
					   rpd.study_id,
					   rpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   rpd.is_reading_prelim,
					   rpd.is_reading_final,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_radiologist_payment_dtls rpd
				left outer join modality m on m.id= rpd.modality_id
				left outer join sys_priority p on p.priority_id = rpd.priority_id
				inner join study_hdr_archive sh on sh.id = rpd.study_id
				inner join institutions i on sh.institution_id=i.id
			    where rpd.billing_cycle_id = @billing_cycle_id) t
				---**************
				
			end
		else
			begin
				select rph.radiologist_id,
					   rph.billing_cycle_id,
					   radiologist_name = dbo.InitCap(r.name),
					   rph.total_study_count_prelim,
					   rph.total_study_count_final,
					   rph.total_amount,
					   rph.approved
				from ap_radiologist_payment_hdr rph
				inner join radiologists r on r.id = rph.radiologist_id
				where billing_cycle_id		= @billing_cycle_id
				  and rph.radiologist_id	= @radiologist_id
				order by r.name

				select row_number() over(order by radiologist_id,received_date) as row_id,
					   radiologist_id,
					   billing_cycle_id,
					   study_id,
					   study_uid,
					   received_date,
					   modality_name,
					   priority_desc,
					   institution_name,
					   patient_name,
					   is_reading_prelim,
					   is_reading_final,
					   prelim_amount,
					   final_amount,
					   adhoc_amount,
					   total_amount,
					   custom_report
				from
				(select rpd.radiologist_id,
					   rpd.billing_cycle_id,
					   rpd.study_id,
					   rpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   rpd.is_reading_prelim,
					   rpd.is_reading_final,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_radiologist_payment_dtls rpd
				left outer join modality m on m.id= rpd.modality_id
				left outer join sys_priority p on p.priority_id = rpd.priority_id
				inner join study_hdr sh on sh.id = rpd.study_id
				inner join institutions i on i.id =  sh.institution_id
				where rpd.billing_cycle_id	= @billing_cycle_id
				and rpd.radiologist_id	= @radiologist_id
				union
				select rpd.radiologist_id,
					   rpd.billing_cycle_id,
					   rpd.study_id,
					   rpd.study_uid,
					   sh.received_date,
					   modality_name = dbo.InitCap(isnull(m.name,'Unkown')),
					   priority_desc = dbo.InitCap(isnull(p.priority_desc,'Unkown')),
					   institution_name=i.name,
					   patient_name = dbo.InitCap(isnull(sh.patient_name,'')),
					   rpd.is_reading_prelim,
					   rpd.is_reading_final,
					   rpd.prelim_amount,
					   rpd.final_amount,
					   rpd.adhoc_amount,
					   rpd.total_amount,
					   custom_report = isnull(i.custom_report,'N')
				from ap_radiologist_payment_dtls rpd
				left outer join modality m on m.id= rpd.modality_id
				left outer join sys_priority p on p.priority_id = rpd.priority_id
				inner join study_hdr_archive sh on sh.id = rpd.study_id
				inner join institutions i on i.id = sh.institution_id
				where rpd.billing_cycle_id	= @billing_cycle_id
				and rpd.radiologist_id	= @radiologist_id) t
				

			end


	
		drop table #tmpRadPayHdr
		drop table #tmpRadPayDtls

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
