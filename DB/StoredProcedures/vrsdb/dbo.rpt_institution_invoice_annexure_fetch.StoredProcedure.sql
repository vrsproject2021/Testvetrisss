USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rpt_institution_invoice_annexure_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rpt_institution_invoice_annexure_fetch]
GO
/****** Object:  StoredProcedure [dbo].[rpt_institution_invoice_annexure_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rpt_institution_invoice_annexure_fetch : fetch 
                  institution invoice annexure
** Created By   : Pavel Guha
** Created On   : 13/11/2019
*******************************************************/
--exec rpt_institution_invoice_annexure_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','CB7442AA-F0AE-4BB1-B994-B12F22F8FAA0'
--exec rpt_institution_invoice_annexure_fetch 'D6FC1504-F380-4966-B135-5AEBE40FB683','2B267B98-C7B5-463E-8845-7364283BC133'
CREATE procedure [dbo].[rpt_institution_invoice_annexure_fetch]
	@billing_cycle_id uniqueidentifier,
	@institution_id uniqueidentifier
as
begin
	 set nocount on
	 declare @arch_db_name nvarchar(30),
			 @strSQL varchar(max)

	 create table #tmp
	 (
		rec_id int identity(1,1),
	    study_id uniqueidentifier,
		received_date datetime,
		institution_name nvarchar(100),
		patient_name nvarchar(250),
		modality_name nvarchar(30),
		priority_desc nvarchar(30),
		img_count int null default 0,
		promo_dtls nvarchar(100),
		amount money null default 0,
		service_name nvarchar(100),
		service_amount money null default 0,
		total_amount money null default 0,
		grand_total money null default 0
	 )

	 select @arch_db_name = arch_db_name from billing_cycle where id=@billing_cycle_id

	 if(isnull(@arch_db_name,''))=''
		begin
			 insert into #tmp(study_id,received_date,institution_name,patient_name,modality_name,priority_desc,
							  img_count,promo_dtls,amount,service_name,service_amount)
			 (select iid.study_id,sh.received_date,
				  institution_name = dbo.InitCap(i.name),
				  patient_name = dbo.InitCap(sh.patient_name),
				  modality_name = isnull(m.code,''),
				  priority_desc= isnull(p.priority_desc,''),
				  sh.img_count,
				  case
					when iid.is_free='Y' then 'Free credit applied'
					when iid.disc_per_applied>0 then 'Discount @ ' + convert(varchar(6),iid.disc_per_applied) + '% ($' + convert(varchar(12),iid.disc_amount)	 + ')'
					when iid.disc_amt_applied>0 then 'Discount @ $' + convert(varchar(20),convert(decimal(10,2),iid.disc_amount))+ ' '
					else ''
				  end promo_dtls,
				  iid.amount,
				  service_name = isnull(s.name,''),
				  service_amount = isnull(iid.service_amount,0)
			 from invoice_institution_dtls iid
			 left outer join invoice_service_dtls isd on isd.study_id = iid.study_id
			 left outer join services s on s.id = isd.service_id
			 inner join study_hdr sh on sh.id = iid.study_id
			 inner join institutions i on i.id = sh.institution_id
			 left outer join modality m on m.id = sh.modality_id
			 left outer join sys_priority p on p.priority_id = sh.priority_id
			 where iid.billing_cycle_id=@billing_cycle_id
			 and iid.institution_id=@institution_id
			 union
			 select iid.study_id,sh.received_date,
				  institution_name = dbo.InitCap(i.name),
				  patient_name = dbo.InitCap(sh.patient_name),
				  modality_name = isnull(m.code,''),
				  priority_desc= isnull(p.priority_desc,''),
				  sh.img_count,
				  case
					when iid.is_free='Y' then 'Free credit applied'
					when iid.disc_per_applied>0 then 'Discount @ ' + convert(varchar(6),iid.disc_per_applied) + '% ($' + convert(varchar(12),iid.disc_amount)	 + ')'
					when iid.disc_amt_applied>0 then 'Discount @ $' + convert(varchar(20),convert(decimal(10,2),iid.disc_amount))+ ' '
					else ''
				  end promo_dtls,
				  iid.amount,
				  service_name = isnull(s.name,''),
				  service_amount = isnull(iid.service_amount,0)
			 from invoice_institution_dtls iid
			 left outer join invoice_service_dtls isd on isd.study_id = iid.study_id
			 left outer join services s on s.id = isd.service_id
			 inner join study_hdr_archive sh on sh.id = iid.study_id
			 inner join institutions i on i.id = sh.institution_id
			 left outer join modality m on m.id = sh.modality_id
			 left outer join sys_priority p on p.priority_id = sh.priority_id
			 where iid.billing_cycle_id=@billing_cycle_id
			 and iid.institution_id=@institution_id)
			 order by received_date
		end
	else
		begin
			 set @strSQL = 'insert into #tmp(study_id,received_date,institution_name,patient_name,modality_name,priority_desc,'
			 set @strSQL = @strSQL + 'img_count,promo_dtls,amount,service_name,service_amount)'
			 set @strSQL = @strSQL + '(select iid.study_id,sh.received_date,'
			 set @strSQL = @strSQL + 'institution_name = dbo.InitCap(i.name),'
			 set @strSQL = @strSQL + 'patient_name = dbo.InitCap(sh.patient_name),'
			 set @strSQL = @strSQL + 'modality_name = isnull(m.code,''''),'
			 set @strSQL = @strSQL + 'priority_desc= isnull(p.priority_desc,''''),'
			 set @strSQL = @strSQL + 'sh.img_count,'
			 set @strSQL = @strSQL + 'case '
			 set @strSQL = @strSQL + 'when iid.is_free=''Y'' then ''Free credit applied'' '
			 set @strSQL = @strSQL + 'when iid.disc_per_applied>0 then ''Discount @ '' + '
			 set @strSQL = @strSQL + 'convert(varchar(6),iid.disc_per_applied)' 
			 set @strSQL = @strSQL + '+ ''% ($''+ '
			 set @strSQL = @strSQL + 'convert(varchar(12),iid.disc_amount)'
			 set @strSQL = @strSQL + '+ '')'' '
			 set @strSQL = @strSQL + 'when iid.disc_amt_applied>0 then ''Discount @ '' + ' 
			 set @strSQL = @strSQL + '+ ''$''+ '
			 set @strSQL = @strSQL + 'convert(varchar(20),convert(decimal(10,2),iid.disc_amount))'
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
			 set @strSQL = @strSQL + 'inner join institutions i on i.id = sh.institution_id '
			 set @strSQL = @strSQL + 'left outer join modality m on m.id = sh.modality_id '
			 set @strSQL = @strSQL + 'left outer join sys_priority p on p.priority_id = sh.priority_id '
             set @strSQL = @strSQL + 'where iid.billing_cycle_id= ''' + convert(varchar(36),@billing_cycle_id) + ''' '
			 set @strSQL = @strSQL + 'and iid.institution_id = ''' + convert(varchar(36),@institution_id) + ''') '
			 set @strSQL = @strSQL + 'order by received_date '

			 --print @strSQL
			 exec(@strSQL)
		end

	 if(@@rowcount=0)
		begin
			if(isnull(@arch_db_name,''))=''
				begin
					insert into #tmp(study_id,received_date,institution_name,patient_name,modality_name,priority_desc,
									  img_count,promo_dtls,amount,service_name,service_amount)
					 (select '00000000-0000-0000-0000-000000000000','01jan1900',
						  institution_name = dbo.InitCap(i.name),
						  patient_name = 'N/A',
						  modality_name = 'N/A',
						  priority_desc='N/A',
						  img_count = 0,
						  promo_dtls = '',
						  amount =0,
						  service_name = '',
						  service_amount = 0
					 from invoice_institution_hdr ih
					 inner join institutions i on i.id = ih.institution_id
					 where ih.billing_cycle_id=@billing_cycle_id
					 and ih.institution_id=@institution_id)
			    end
			else
				begin
					set @strSQL = 'insert into #tmp(study_id,received_date,institution_name,patient_name,modality_name,priority_desc,'
					set @strSQL = @strSQL + 'img_count,promo_dtls,amount,service_name,service_amount)'
					set @strSQL = @strSQL + '(select ''00000000-0000-0000-0000-000000000000'',''01jan1900'','
					set @strSQL = @strSQL + 'institution_name = dbo.InitCap(i.name),'
					set @strSQL = @strSQL + 'patient_name = ''N/A'','
					set @strSQL = @strSQL + 'modality_name = ''N/A'','
					set @strSQL = @strSQL + 'priority_desc=''N/A'','
					set @strSQL = @strSQL + 'img_count = 0,'
					set @strSQL = @strSQL + 'promo_dtls = '''','
					set @strSQL = @strSQL + 'amount =0,'
					set @strSQL = @strSQL + 'service_name = '''','
					set @strSQL = @strSQL + 'service_amount = 0 '
					set @strSQL = @strSQL + 'from ' + @arch_db_name + '..invoice_institution_hdr ih '
					set @strSQL = @strSQL + 'inner join institutions i on i.id = ih.institution_id '
					set @strSQL = @strSQL + 'where ih.billing_cycle_id=''' + convert(varchar(36),@billing_cycle_id) + ''' '
					set @strSQL = @strSQL + 'and ih.institution_id=''' + convert(varchar(36),@institution_id) + ''')'

					exec(@strSQL)
				end
		end

	 update #tmp set total_amount = amount
	 
	 update #tmp 
	 set total_amount = total_amount + (select sum(t1.service_amount)
	                                    from #tmp t1
										where t1.study_id = #tmp.study_id)

	update #tmp 
	set grand_total = (select sum(t1.total_amount)
	                  from 
					  (select distinct study_id,total_amount
					   from #tmp) t1)

	 select * from #tmp

	 drop table #tmp

	set nocount off
end

GO
