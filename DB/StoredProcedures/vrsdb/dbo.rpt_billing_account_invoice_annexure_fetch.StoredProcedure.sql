USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_billing_account_invoice_annexure_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_billing_account_invoice_annexure_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_billing_account_invoice_annexure_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_billing_account_invoice_annexure_fetch : fetch 
                  institution invoice annexure
** Created By   : Pavel Guha
** Created On   : 13/11/2019
*******************************************************/
--exec rpt_billing_account_invoice_annexure_fetch '3ACFB756-45AF-424C-80E5-5B66406E08A6','8CBBD0EF-14AB-42AB-AE21-8992A4AFA6E6'
--exec rpt_billing_account_invoice_annexure_fetch '3225FD47-3FF5-4863-8E89-22E960263EB3','8797227B-E316-4FC1-B2CF-3FA18DE8C3AB'
--exec rpt_billing_account_invoice_annexure_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','CF1659F2-C8B0-427E-A397-1BE40BD61CE1'
--exec rpt_billing_account_invoice_annexure_fetch '0A4B7FC1-D6D5-41D3-918B-66C6DA2F6D78','4FCB12A4-248E-4E95-AEC6-CFB9360F15C8'
CREATE procedure [dbo].[rpt_billing_account_invoice_annexure_fetch]
	@billing_cycle_id uniqueidentifier,
	@billing_account_id uniqueidentifier
as
begin
	 set nocount on
	 create table #tmp
	 (
		rec_id int identity(1,1),
	    study_id uniqueidentifier,
		received_date datetime,
		institution_id uniqueidentifier,
		institution_name nvarchar(100),
		patient_name nvarchar(250),
		item_type nchar(1),
		item_name nvarchar(30),
		priority_desc nvarchar(30),
		img_count int null default 0,
		promo_dtls nvarchar(100),
		amount money null default 0,
		service_name nvarchar(100),
		service_amount money null default 0,
		total_amount money null default 0,
		study_total money null default 0,
		inst_total  money null default 0,
		grand_total money null default 0,
		institution_count int null default 0,
		institution_item_count int null default 0,
		study_item_count int null default 0
	 )

	 declare  @inst_total money,
	          @grand_total money,
			  @institution_id uniqueidentifier,
			  @study_id uniqueidentifier,
			  @rowcount int,
			  @counter int,
			  @rc int,
	          @arch_db_name nvarchar(30),
			  @strSQL varchar(max)

	 select @arch_db_name = arch_db_name from billing_cycle where id=@billing_cycle_id

	 if(isnull(@arch_db_name,''))=''
		begin
			insert into #tmp(study_id,received_date,institution_id,institution_name,patient_name,item_type,item_name,priority_desc,
							  img_count,promo_dtls,amount,service_name,service_amount)
			 (select iid.study_id,sh.received_date,iid.institution_id,
				  institution_name = dbo.InitCap(i.name),
				  patient_name = dbo.InitCap(sh.patient_name),
				  item_type ='M',
				  modality_name = dbo.InitCap(isnull(m.name,'')),
				  priority_desc = isnull(p.priority_desc,''),
				  sh.img_count,
				  case
					when iid.is_free='Y' then 'Credit applied ($' + convert(varchar(20),iid.study_price) + ')'
					when iid.disc_per_applied>0 then 'Discount @ ' + convert(varchar(6),iid.disc_per_applied) + '% ($' + convert(varchar(20),convert(decimal(10,2),(iid.disc_per_applied * iid.study_price)/100))+ ')'
					when iid.disc_amt_applied>0 then 'Discount @ $' + convert(varchar(20),convert(decimal(10,2),iid.disc_amt_applied))+ ' '
					else ''
				  end promo_dtls,
				  iid.amount,
				  service_name = isnull(s.name,''),
				  service_amount = isnull(iid.service_amount,0)
			 from invoice_institution_dtls iid
			 left outer join invoice_service_dtls isd on isd.study_id = iid.study_id
			 left outer join services s on s.id = isd.service_id
			 inner join study_hdr sh on sh.id = iid.study_id
			 inner join institutions i on i.id = iid.institution_id
			 left outer join modality m on m.id = iid.modality_id
			 left outer join sys_priority p on p.priority_id = sh.priority_id
			 where iid.billing_cycle_id=@billing_cycle_id
			 and iid.billing_account_id=@billing_account_id
			 union
			 select iid.study_id,sh.received_date,iid.institution_id,
				  institution_name = dbo.InitCap(i.name),
				  patient_name = dbo.InitCap(sh.patient_name),
				  item_type ='M',
				  modality_name = dbo.InitCap(isnull(m.name,'')),
				  priority_desc = isnull(p.priority_desc,''),
				  sh.img_count,
				  case
					when iid.is_free='Y' then 'Credit applied ($' + convert(varchar(20),iid.study_price) + ')'
					when iid.disc_per_applied>0 then 'Discount @ ' + convert(varchar(6),iid.disc_per_applied) + '% ($' + convert(varchar(20),convert(decimal(10,2),(iid.disc_per_applied * iid.study_price)/100))+ ')'
					when iid.disc_amt_applied>0 then 'Discount @ $' + convert(varchar(20),convert(decimal(10,2),iid.disc_amt_applied))+ ' '
					else ''
				  end promo_dtls,
				  iid.amount,
				  service_name = isnull(s.name,''),
				  service_amount = isnull(iid.service_amount,0)
			 from invoice_institution_dtls iid
			 left outer join invoice_service_dtls isd on isd.study_id = iid.study_id
			 left outer join services s on s.id = isd.service_id
			 inner join study_hdr_archive sh on sh.id = iid.study_id
			 inner join institutions i on i.id = iid.institution_id
			 left outer join modality m on m.id = iid.modality_id
			 left outer join sys_priority p on p.priority_id = sh.priority_id
			 where iid.billing_cycle_id=@billing_cycle_id
			 and iid.billing_account_id=@billing_account_id)
			 order by institution_name,received_date
		end
	 else
		begin
			 set @strSQL = 'insert into #tmp(study_id,received_date,institution_id,institution_name,patient_name,item_type,item_name,priority_desc,'
			 set @strSQL = @strSQL + 'img_count,promo_dtls,amount,service_name,service_amount)'
			 set @strSQL = @strSQL + '(select iid.study_id,sh.received_date,iid.institution_id,'
			 set @strSQL = @strSQL + 'institution_name = dbo.InitCap(i.name),'
			 set @strSQL = @strSQL + 'patient_name = dbo.InitCap(sh.patient_name),'
			 set @strSQL = @strSQL + 'item_type =''M'','
			 set @strSQL = @strSQL + 'modality_name = dbo.InitCap(isnull(m.name,'''')),'
			 set @strSQL = @strSQL + 'priority_desc = isnull(p.priority_desc,''''),'
			 set @strSQL = @strSQL + 'sh.img_count,'
			 set @strSQL = @strSQL + 'case '
			 set @strSQL = @strSQL + 'when iid.is_free=''Y'' then ''Credit applied ($'' + '
			 set @strSQL = @strSQL + 'convert(varchar(20),iid.study_price)'
			 set @strSQL = @strSQL + ' + '')'' '
			 set @strSQL = @strSQL + 'when iid.disc_per_applied>0 then ''Discount @ '' + '
			 set @strSQL = @strSQL + 'convert(varchar(6),iid.disc_per_applied)' 
			 set @strSQL = @strSQL + '+ ''% ($''+ '
			 set @strSQL = @strSQL + 'convert(varchar(20),convert(decimal(10,2),(iid.disc_per_applied * iid.study_price)/100))'
			 set @strSQL = @strSQL + '+ '')'' '
			 set @strSQL = @strSQL + 'when iid.disc_amt_applied>0 then ''Discount @ '' + ' 
			 set @strSQL = @strSQL + '+ ''$''+ '
			 set @strSQL = @strSQL + 'convert(varchar(20),convert(decimal(10,2),iid.disc_amt_applied))'
			 set @strSQL = @strSQL + '+ '' '' '
			 set @strSQL = @strSQL + 'else '''' '
			 set @strSQL = @strSQL + 'end promo_dtls,'
			 set @strSQL = @strSQL + 'iid.amount,'
			 set @strSQL = @strSQL + 'service_name = isnull(s.name,''''),'
			 set @strSQL = @strSQL + 'service_amount = isnull(iid.service_amount,0) '
			 set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls iid '
			 set @strSQL = @strSQL + 'left outer join ' + @arch_db_name + '..invoice_service_dtls isd on isd.study_id = iid.study_id '
			 set @strSQL = @strSQL + 'left outer join services s on s.id = isd.service_id '
			 set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..study_hdr_archive sh on sh.id = iid.study_id '
			 set @strSQL = @strSQL + 'inner join institutions i on i.id = iid.institution_id '
			 set @strSQL = @strSQL + 'left outer join modality m on m.id = iid.modality_id '
			 set @strSQL = @strSQL + 'left outer join sys_priority p on p.priority_id = sh.priority_id '
			 set @strSQL = @strSQL + 'where iid.billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
			 set @strSQL = @strSQL + 'and iid.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
			 set @strSQL = @strSQL + 'order by institution_name,received_date'

			 exec(@strSQL)
		end
	 

	 set @rc =@@rowcount

	  if(isnull(@arch_db_name,''))=''
		    begin
				 insert into #tmp(study_id,received_date,institution_id,institution_name,patient_name,item_type,item_name,priority_desc,
								  img_count,promo_dtls,amount,service_name,service_amount)
				 (select iid.study_id,sh.received_date,iid.institution_id,
					  institution_name = dbo.InitCap(i.name),
					  patient_name = dbo.InitCap(sh.patient_name),
					  item_type ='S',
					  item_name = dbo.InitCap(isnull(s.name,'')),
					  priority_desc = isnull(p.priority_desc,''),
					  sh.img_count,
					  case
						when iid.is_free='Y' then 'Credit applied ($' + convert(varchar(20),iid.service_price) + ')'
						when iid.disc_per_applied>0 then 'Discount @ ' + convert(varchar(6),iid.disc_per_applied) + '% ($' + convert(varchar(20),convert(decimal(10,2),(iid.disc_per_applied * iid.service_price)/100))+ ')'
						when iid.disc_amt_applied>0 then 'Discount @ $' + convert(varchar(20),convert(decimal(10,2),isd.disc_amt_applied))+ ' '
						else ''
					  end promo_dtls,
					  isd.amount,
					  service_name = isnull(s.name,''),
					  service_amount = isnull(iid.service_amount,0)
				 from invoice_institution_dtls iid
				 inner join invoice_service_dtls isd on isd.study_id = iid.study_id
				 inner join services s on s.id = isd.service_id
				 inner join study_hdr sh on sh.id = iid.study_id
				 inner join institutions i on i.id = iid.institution_id
				 inner join sys_priority p on p.priority_id = sh.priority_id
				 where iid.billing_cycle_id=@billing_cycle_id
				 and iid.billing_account_id=@billing_account_id
				 union
				 select iid.study_id,sh.received_date,iid.institution_id,
					  institution_name = dbo.InitCap(i.name),
					  patient_name = dbo.InitCap(sh.patient_name),
					  item_type ='S',
					  item_name = dbo.InitCap(isnull(s.name,'')),
					  priority_desc = isnull(p.priority_desc,''),
					  sh.img_count,
					  case
						when iid.is_free='Y' then 'Credit applied ($' + convert(varchar(20),iid.service_price) + ')'
						when iid.disc_per_applied>0 then 'Discount @ ' + convert(varchar(6),iid.disc_per_applied) + '% ($' + convert(varchar(20),convert(decimal(10,2),(iid.disc_per_applied * iid.service_price)/100))+ ')'
						when iid.disc_amt_applied>0 then 'Discount @ $' + convert(varchar(20),convert(decimal(10,2),isd.disc_amt_applied))+ ' '
						else ''
					  end promo_dtls,
					  isd.amount,
					  service_name = isnull(s.name,''),
					  service_amount = isnull(iid.service_amount,0)
				 from invoice_institution_dtls iid
				 inner join invoice_service_dtls isd on isd.study_id = iid.study_id
				 inner join services s on s.id = isd.service_id
				 inner join study_hdr_archive sh on sh.id = iid.study_id
				 inner join institutions i on i.id = iid.institution_id
				 inner join sys_priority p on p.priority_id = sh.priority_id
				 where iid.billing_cycle_id=@billing_cycle_id
				 and iid.billing_account_id=@billing_account_id)
				 order by institution_name,received_date
			end
	  else
			begin
				 set @strSQL = 'insert into #tmp(study_id,received_date,institution_id,institution_name,patient_name,item_type,item_name,priority_desc,'
				 set @strSQL = @strSQL + 'img_count,promo_dtls,amount,service_name,service_amount)'
				 set @strSQL = @strSQL + '(select iid.study_id,sh.received_date,iid.institution_id,'
				 set @strSQL = @strSQL + 'institution_name = dbo.InitCap(i.name),'
				 set @strSQL = @strSQL + 'patient_name = dbo.InitCap(sh.patient_name),'
				 set @strSQL = @strSQL + 'item_type =''S'','
				 set @strSQL = @strSQL + 'item_name = dbo.InitCap(isnull(s.name,'')),'
				 set @strSQL = @strSQL + 'priority_desc = isnull(p.priority_desc,''),'
				 set @strSQL = @strSQL + 'sh.img_count,'
				 set @strSQL = @strSQL + 'case '
			     set @strSQL = @strSQL + 'when iid.is_free=''Y'' then ''Credit applied ($'' + '
				 set @strSQL = @strSQL + 'convert(varchar(20),iid.service_price)'
				 set @strSQL = @strSQL + ' + '')'' '
				 set @strSQL = @strSQL + 'when iid.disc_per_applied>0 then ''Discount @ '' + '
			     set @strSQL = @strSQL + 'convert(varchar(6),iid.disc_per_applied)' 
				 set @strSQL = @strSQL + '+ ''% ($''+ '
				 set @strSQL = @strSQL + 'convert(varchar(20),convert(decimal(10,2),(iid.disc_per_applied * iid.service_price)/100))'
			     set @strSQL = @strSQL + '+ '')'' '
				 set @strSQL = @strSQL + 'when iid.disc_amt_applied>0 then ''Discount @ '' + ' 
				 set @strSQL = @strSQL + '+ ''$''+ '
				 set @strSQL = @strSQL + 'convert(varchar(20),convert(decimal(10,2),isd.disc_amt_applied))'
				 set @strSQL = @strSQL + '+ '' '' '
				 set @strSQL = @strSQL + 'else '''' '
				 set @strSQL = @strSQL + 'end promo_dtls,'
				 set @strSQL = @strSQL + 'isd.amount,'
				 set @strSQL = @strSQL + 'service_name = isnull(s.name,''''),'
				 set @strSQL = @strSQL + 'service_amount = isnull(iid.service_amount,0)'
				 set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_dtls iid '
			     set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..invoice_service_dtls isd on isd.study_id = iid.study_id '
			     set @strSQL = @strSQL + 'inner join services s on s.id = isd.service_id '
				 set @strSQL = @strSQL + 'inner join ' + @arch_db_name + '..study_hdr_archive sh on sh.id = iid.study_id '
			     set @strSQL = @strSQL + 'inner join institutions i on i.id = iid.institution_id '
				 set @strSQL = @strSQL + 'inner join sys_priority p on p.priority_id = sh.priority_id '
				 set @strSQL = @strSQL + 'where iid.billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
			     set @strSQL = @strSQL + 'and iid.billing_account_id = ''' + convert(varchar(36),@billing_account_id) + ''' '
			     set @strSQL = @strSQL + 'order by institution_name,received_date'

				 exec(@strSQL)
			end

	 set @rc = @rc+ @@rowcount

	 if(@@rowcount=0)
		begin
			if(isnull(@arch_db_name,''))=''
				begin
					insert into #tmp(study_id,received_date,institution_id,institution_name,patient_name,item_name,priority_desc,
									img_count,promo_dtls,amount,service_name,service_amount)
						(select '00000000-0000-0000-0000-000000000000','01jan1900',ih.institution_id,
							institution_name = dbo.InitCap(i.name),
							patient_name = 'N/A',
							modality_name = 'N/A',
							priority_desc = 'N/A',
							img_count = 0,
							promo_dtls = '-',
							amount = 0,
							service_name = '',
							service_amount = 0
						from invoice_institution_hdr ih
						inner join institutions i on i.id = ih.institution_id
						where ih.billing_cycle_id=@billing_cycle_id
						and ih.billing_account_id=@billing_account_id)
						order by institution_name
				end
			else
				begin
					set @strSQL = 'insert into #tmp(study_id,received_date,institution_id,institution_name,patient_name,item_name,priority_desc,'
					set @strSQL = @strSQL + 'img_count,promo_dtls,amount,service_name,service_amount)'
					set @strSQL = @strSQL + '(select ''00000000-0000-0000-0000-000000000000'',''01jan1900'',ih.institution_id,'
					set @strSQL = @strSQL + 'institution_name = dbo.InitCap(i.name),'
					set @strSQL = @strSQL + 'patient_name = ''N/A'','
					set @strSQL = @strSQL + 'modality_name = ''N/A'','
					set @strSQL = @strSQL + 'priority_desc = ''N/A'','
					set @strSQL = @strSQL + 'img_count = 0,'
					set @strSQL = @strSQL + 'promo_dtls = ''-'','
					set @strSQL = @strSQL + 'amount = 0,'
					set @strSQL = @strSQL + 'service_name = '''','
					set @strSQL = @strSQL + 'service_amount = 0'
					set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_hdr ih '
					set @strSQL = @strSQL + 'inner join institutions i on i.id = ih.institution_id '
					set @strSQL = @strSQL + 'where ih.billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + ''' '
					set @strSQL = @strSQL + 'and ih.billing_account_id=''' + convert(varchar(36),@billing_account_id) + ''') '
					set @strSQL = @strSQL + 'order by institution_name '

					exec(@strSQL)
				end
		end

	 update #tmp set total_amount = amount

	 --update #tmp 
	 --set total_amount = total_amount + (select sum(t1.service_amount)
	 --                                   from #tmp t1
		--								where t1.study_id = #tmp.study_id)

	 update #tmp 
	 set study_total = (select sum(t1.amount)
	                    from #tmp t1
						where t1.study_id = #tmp.study_id)

	 update #tmp set inst_total = (select sum(total_amount)
	                               from #tmp t
								   where t.institution_id = #tmp.institution_id)

	update #tmp 
	set grand_total = (select sum(t1.inst_total)
	                  from 
					  (select distinct institution_id,inst_total
					   from #tmp) t1)

	update #tmp
	set institution_count = (select count(distinct institution_id) from #tmp)

    update #tmp
	set institution_item_count = isnull((select count(institution_id)
										 from #tmp t1
										 where t1.institution_id = #tmp.institution_id),0)

	update #tmp
	set study_item_count  = isnull((select count(study_id)
									from #tmp t1
									where t1.institution_id = #tmp.institution_id
									and t1.study_id = #tmp.study_id),0)


	 select * from #tmp order by institution_name,received_date,item_type,patient_name

	 drop table #tmp

	set nocount off
end

GO
