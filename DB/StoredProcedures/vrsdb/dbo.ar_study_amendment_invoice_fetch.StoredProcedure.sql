USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_amendment_invoice_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_study_amendment_invoice_fetch]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_amendment_invoice_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_study_amendment_invoice_fetch : 
                  fetch study details for correction
** Created By   : Pavel Guha 
** Created On   : 05/03/2021
*******************************************************/
--exec ar_study_amendment_invoice_fetch 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD','763444b5-7607-4ce6-a9cd-95d433380243','',56,'11111111-1111-1111-1111-111111111111','',0
--exec ar_study_amendment_invoice_fetch '2353A24B-357F-482A-81CC-6A3EF38D7625','CB7442AA-F0AE-4BB1-B994-B12F22F8FAA0','',0,0,45,'11111111-1111-1111-1111-111111111111','',0
--exec ar_study_amendment_invoice_fetch '8C5E3E24-B7A8-495D-83E0-D1C706589912','EF38282A-02A7-4A32-B3D8-4CDAFF485323','',0,0,45,'11111111-1111-1111-1111-111111111111','',0
--exec ar_study_amendment_invoice_fetch '5438D25A-B616-45D1-8126-3B0C1BD011E8','D929FA87-5E95-40DD-BD06-1E338A05DE38','',0,0,45,'11111111-1111-1111-1111-111111111111','',0

CREATE procedure [dbo].[ar_study_amendment_invoice_fetch]
	@billing_cycle_id uniqueidentifier,
	@institution_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@patient_name nvarchar(100) ='', 
	@modality_id int =0,
	@category_id int =0,
	@menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
	begin
		set nocount on
		declare @date_from datetime,
				@date_till datetime,
				@counter bigint,
				@sql1 nvarchar(max),
				@sql2 nvarchar(max),
				@sql3 nvarchar(max)

		delete from sys_record_lock where user_id=@user_id
		delete from sys_record_lock_ui where user_id= @user_id

		select  @date_from= convert(date, date_from,103),
				@date_till=convert(date,date_till,103)
		from billing_cycle
		where id=@billing_cycle_id

	    create table #tmp
		(
			id uniqueidentifier,
			study_uid nvarchar(100),
			received_date datetime,
			patient_name nvarchar(100),
			institution_id uniqueidentifier,
			modality_id int,
			category_id int,
			priority_id int,
			invoiced nchar(1),
			service_codes nvarchar(250),
			promotion_id uniqueidentifier,
			changed nchar(1),
			del nchar(1)
		)
		create table #tmpRates
		(
			rec_id int identity(1,1),
			study_id uniqueidentifier,
			study_uid nvarchar(100),
			head_id int,
			head_name nvarchar(100),
			head_type nchar(1),
			image_count int,
			amount   money,
			addon_rate_per_unit money,
			study_max_amount money,
			rate_after_hrs money,
			received_date datetime null,
			invoice_by nchar(1),
			changed nchar(1) null default 'N',
			sel nchar(1)
		)

		set @sql1='select sh.id, sh.study_uid,sh.received_date,'
		set @sql1=@sql1 + 'patient_name=dbo.InitCap(isnull(sh.patient_name,'''')),'
		set @sql1=@sql1 + 'institution_id = isnull(sh.institution_id,''00000000-0000-0000-0000-000000000000''),'
		set @sql1=@sql1 + 'modality_id = isnull(sh.modality_id,0),category_id = isnull(sh.category_id,0),sh.priority_id,sh.invoiced,service_codes=isnull(sh.service_codes,''''),'
		set @sql1=@sql1 + 'promotion_id=isnull(iid.promotion_id,''00000000-0000-0000-0000-000000000000''),changed = ''N'',del=''N'' '
		set @sql1=@sql1 + 'from study_hdr sh '
		set @sql1=@sql1 + 'inner join invoice_institution_dtls iid on iid.institution_id = sh.institution_id and iid.study_id = sh.id ' 
		set @sql1=@sql1 + 'where convert(datetime,convert(varchar(11),sh.received_date,106)) between ''' + convert(varchar(11),@date_from,106)  + ' 00:00:00'' and ''' + convert(varchar(11),@date_till,106)  + ' 23:59:59'' '
		set @sql1=@sql1 + 'and sh.deleted =''N'' '
		set @sql1=@sql1 + 'and sh.invoiced =''N'' '
		set @sql1=@sql1 + 'and sh.study_status_pacs=100 '
		
		if(isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
			begin
				set @sql1=@sql1 + ' and sh.institution_id= ''' + convert(varchar(36),@institution_id) + ''' '
			end
		if(rtrim(ltrim(isnull(@patient_name,''))) <> '')
			begin
				set @sql1=@sql1 + ' and upper(sh.patient_name) like  ''%' + upper(rtrim(ltrim(@patient_name))) + '%'' '
			end
		if(isnull(@modality_id,0) <> 0)
			begin
				set @sql1=@sql1 + ' and sh.modality_id= ''' + convert(varchar,@modality_id) + ''' '
			end
		if(isnull(@category_id,0) <> 0)
			begin
				set @sql1=@sql1 + ' and sh.category_id= ''' + convert(varchar,@category_id) + ''' '
			end

		
			
		set @sql2='select sh.id, sh.study_uid,sh.received_date,'
		set @sql2=@sql2 + 'patient_name=dbo.InitCap(isnull(sh.patient_name,'''')),'
		set @sql2=@sql2 + 'institution_id = isnull(sh.institution_id,''00000000-0000-0000-0000-000000000000''),'
		set @sql2=@sql2 + 'modality_id = isnull(sh.modality_id,0),category_id = isnull(sh.category_id,0),sh.priority_id,sh.invoiced,service_codes=isnull(sh.service_codes,''''),'
		set @sql2=@sql2 + 'promotion_id=isnull(iid.promotion_id,''00000000-0000-0000-0000-000000000000''),changed = ''N'',del=''N'' '
		set @sql2=@sql2 + 'from study_hdr_archive sh '
		set @sql2=@sql2 + 'inner join invoice_institution_dtls iid on iid.institution_id = sh.institution_id and iid.study_id = sh.id ' 
		set @sql2=@sql2 + 'where convert(datetime,convert(varchar(11),sh.received_date,106)) between ''' + convert(varchar(11),@date_from,106)  + ' 00:00:00'' and ''' + convert(varchar(11),@date_till,106)  + ' 23:59:59'' '
		set @sql2=@sql2 + 'and sh.deleted =''N'' '
		set @sql2=@sql2 + 'and sh.invoiced =''N'' '
		set @sql2=@sql2 + 'and sh.study_status_pacs in (100,0) '

		if(isnull(@institution_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
			begin
				set @sql2=@sql2 + ' and sh.institution_id= ''' + convert(varchar(36),@institution_id) + ''' '
			end
		if(rtrim(ltrim(isnull(@patient_name,''))) <> '')
			begin
				set @sql2=@sql2 + ' and upper(sh.patient_name) like  ''%' + upper(rtrim(ltrim(@patient_name))) + '%'' '
			end
		if(isnull(@modality_id,0) <> 0)
			begin
				set @sql2=@sql2 + ' and sh.modality_id= ''' + convert(varchar,@modality_id) + ''' '
			end
		if(isnull(@category_id,0) <> 0)
			begin
				set @sql2=@sql2 + ' and sh.category_id= ''' + convert(varchar,@category_id) + ''' '
			end

		--exec(@sql1 + ' union ' + @sql2 + ') order by sh.received_date')
		--print @sql1 + ' union ' + @sql2 + ' order by sh.received_date'

		set @sql3 = 'insert into #tmp(id,study_uid,received_date,patient_name,institution_id,modality_id,category_id,priority_id,invoiced,service_codes,promotion_id,changed,del)'
		--print @sql1
		--print @sql2
		--print @sql3
		--print @sql3 + '(' + @sql1 + ' union ' + @sql2 + ') order by sh.received_date'
		exec(@sql3 + '(' + @sql1 + ' union ' + @sql2 + ') order by sh.received_date')

		insert into #tmpRates(study_id,study_uid,head_id,head_name,head_type,image_count,amount,addon_rate_per_unit,study_max_amount,rate_after_hrs,invoice_by,sel)
		(select study_id= iid.study_id,
               iid.study_uid,
			   aar.head_id,
			   head_name = dbo.InitCap(m.name),
			   head_type ='M',
			   iid.image_count,
			   amount = aar.rate,
			   aar.addon_rate_per_unit,
			   aar.study_max_amount,
			   aar.rate_after_hrs,
			   m.invoice_by,
			   'Y'
		from invoice_institution_dtls iid
		inner join ar_amended_rates aar on aar.study_hdr_id = iid.study_id
		inner join modality m on m.id= aar.head_id
		where aar.head_type = 'M'
		and iid.institution_id = @institution_id
		and iid.billing_cycle_id  = @billing_cycle_id
		union
		select study_id=iid.study_id,
               iid.study_uid,
			   head_id = iid.modality_id,
			   head_name = dbo.InitCap(m.name),
			   head_type ='M',
			   iid.image_count,
			   iid.amount,
			   iid.rate_per_unit,
			   iid.study_max_amount,
			   rate_after_hrs = 0,
			   m.invoice_by,
			   'Y'
		from invoice_institution_dtls iid
		inner join modality m on m.id= iid.modality_id
		and iid.institution_id = @institution_id
		and iid.billing_cycle_id  = @billing_cycle_id
		--and iid.study_uid='1.2.840.113619.2.224.963334213084.1604310698.2'
		union
		select study_id=isd.study_id,
               isd.study_uid,
			   aar.head_id,
			   head_name = dbo.InitCap(s.name),
			   head_type ='S',
			   image_count = 0,
			   amount = aar.rate,
			   aar.addon_rate_per_unit,
			   aar.study_max_amount,
			   rate_after_hrs = isd.service_price_after_hrs,
			   '',
			  'Y'
		from invoice_service_dtls isd
		inner join ar_amended_rates aar on aar.study_hdr_id = isd.study_id
		inner join services s on s.id= aar.head_id
		where aar.head_type='S'
		and isd.institution_id = @institution_id
		and isd.billing_cycle_id  = @billing_cycle_id
		union
		select study_id=isd.study_id,
               isd.study_uid,
			   head_id = isd.service_id,
			   head_name = dbo.InitCap(s.name),
			   head_type ='S',
			   image_count = 0,
			   isd.amount,
			   rate_per_unit=0,
			   study_max_amount=0,
			   rate_after_hrs = isd.service_price_after_hrs,
			   '',
			   'Y'
		from invoice_service_dtls isd
		inner join services s on s.id= isd.service_id
		and isd.institution_id = @institution_id
		and isd.billing_cycle_id  = @billing_cycle_id
		and isd.study_id not in (select study_hdr_id from ar_amended_rates where billing_cycle_id  = @billing_cycle_id and institution_id = @institution_id and head_type='S'))
		
		--and isd.study_uid='1.2.840.113619.2.224.963334213084.1604310698.2'
		

		select * from #tmp order by received_date

		update #tmpRates
		set received_date = isnull((select received_date from study_hdr where id=#tmpRates.study_id),'01jan1900')
		where isnull(received_date,'01jan1900') ='01jan1900'

		update #tmpRates
		set received_date = isnull((select received_date from study_hdr_archive where id=#tmpRates.study_id),'01jan1900')
		where isnull(received_date,'01jan1900') ='01jan1900'

		delete from #tmpRates where study_id not in (select id from #tmp)

		select * from #tmpRates order by received_date,head_type


		drop table #tmp
		drop table #tmpRates
				
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
