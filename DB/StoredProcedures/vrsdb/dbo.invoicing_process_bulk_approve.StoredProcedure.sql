USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_bulk_approve]    Script Date: 20-08-2021 20:43:16 ******/
DROP PROCEDURE [dbo].[invoicing_process_bulk_approve]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_bulk_approve]    Script Date: 20-08-2021 20:43:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_process_bulk_approve : 
                  approve invoices in bulk
** Created By   : Pavel Guha 
** Created On   : 14/01/2020
*******************************************************/
/*
	exec invoicing_process_bulk_approve 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD',
	'<account><row><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><total_amount>260</total_amount><row_id>1</row_id></row></account>',
	'<institution><row><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><total_amount>260</total_amount><approved><![CDATA[Y]]></approved><row_id>1</row_id></row></institution>',
	'<study><row><study_id><![CDATA[3ccd3da8-53be-4de1-81ba-f01c7d83d431]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Rusty Juarbe]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>1</row_id></row><row><study_id><![CDATA[2f13558a-5bce-4df5-a439-91744d3c3e47]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Cocco Grenwood]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>2</row_id></row><row><study_id><![CDATA[cf8fd9d0-6803-46e5-bb18-995e4294df93]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Sammi Wheat]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>3</row_id></row><row><study_id><![CDATA[8bf55763-c606-480d-91aa-3196f8674212]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Nona Simonenko]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>4</row_id></row><row><study_id><![CDATA[e9d35744-23d6-479e-a1ad-1fe67eddf467]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Angel Walsh]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>5</row_id></row><row><study_id><![CDATA[84662f93-df49-4191-b067-198adfc364d5]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Tinkerbell Puca]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>6</row_id></row></study>'
	,45,'11111111-1111-1111-1111-111111111111','','',0

*/
CREATE procedure [dbo].[invoicing_process_bulk_approve]
	@billing_cycle_id uniqueidentifier,
	@xml_account      ntext,
	@menu_id          int,
    @updated_by       uniqueidentifier,
    @user_name        nvarchar(500) = '' output,
    @error_code       nvarchar(10)='' output,
    @return_status    int =0 output
as
	begin
		set nocount on
		declare @hDoc int,
		        @counter bigint,
	            @rowcount bigint,
				@ctr int,
				@rc int

		declare @billing_account_id uniqueidentifier,
				@invoice_srl_no int,
				@invoice_no nvarchar(50),
				@invoice_no_hdr nvarchar(50),
				@invoice_date datetime,
				@DUEDTDAYS int

		declare @institution_id uniqueidentifier,
		        @institution_code nvarchar(5),
				@modality_count int,
				@category_count int,
				@service_id int,
				@gl_code nvarchar(5)


		 exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @billing_cycle_id,
				@user_id       = @updated_by,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
		if(@return_status=0)
			begin
				return 0
			end

		select @DUEDTDAYS= data_value_int
		from invoicing_control_params
		where control_code='DUEDTDAYS'

		create table #tmp
		(
			rec_id int identity(1,1) not null,
			institution_id uniqueidentifier
		)
		begin transaction
		exec sp_xml_preparedocument @hDoc output,@xml_account 

	

		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc,'account/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @billing_account_id  = billing_account_id
					from openxml(@hDoc,'account/row',2)
					with
					( 
						billing_account_id uniqueidentifier,
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  


					select @invoice_no_hdr=isnull(invoice_no,''),
					       @invoice_srl_no = isnull(invoice_srl_no,0) 
					from invoice_hdr 
					where billing_account_id = @billing_account_id 
					and billing_cycle_id=@billing_cycle_id
					

					if(isnull(@invoice_no_hdr,'')<>'')
						begin
							set @invoice_no_hdr =''
							set @invoice_srl_no = 0

							exec invoicing_process_invoice_no_generate
								@invoice_srl_no = @invoice_srl_no output,
								@invoice_no     = @invoice_no_hdr output

							set @invoice_date = convert(datetime,convert(varchar(11),getdate(),106))
						end
					

					insert into #tmp(institution_id)
					(select institution_id
					from invoice_institution_hdr
					where billing_account_id = @billing_account_id
					and billing_cycle_id=@billing_cycle_id)

					set @rc  =@@rowcount
					set @ctr =1

					while(@ctr<= @rc)
						begin
							select @institution_id = institution_id
							from #tmp
							where rec_id= @ctr


						    set @modality_count=0
							set @category_count=0

							select @modality_count = count(iid.study_id) 
							from invoice_institution_dtls  iid 
							where iid.institution_id = @institution_id 
							and iid.billing_account_id = @billing_account_id 
							and iid.billing_cycle_id = @billing_cycle_id
							and iid.modality_id = 0

							if(@modality_count = 0)
								begin
									rollback transaction
									select @user_name = name from institutions where id = @institution_id
									select @error_code='381',@return_status=0
									return 0
								end

							select @category_count = count(iid.study_id) 
							from invoice_institution_dtls  iid 
							inner join study_hdr sh on sh.id = iid.study_id
							where iid.institution_id = @institution_id 
							and iid.billing_account_id = @billing_account_id 
							and iid.billing_cycle_id = @billing_cycle_id
							and sh.category_id = 0

							select @category_count = @category_count + count(iid.study_id) 
							from invoice_institution_dtls  iid 
							inner join study_hdr_archive sh on sh.id = iid.study_id
							where iid.institution_id = @institution_id 
							and iid.billing_account_id = @billing_account_id 
							and iid.billing_cycle_id = @billing_cycle_id
							and sh.category_id = 0

							if(@category_count = 0)
								begin
									rollback transaction
									select @user_name = name from institutions where id = @institution_id
									select @error_code='382',@return_status=0
									return 0
								end
							
							select @institution_code = isnull(code,'') from institutions where id = @institution_id
							set @invoice_no = @invoice_no_hdr + '/' + @institution_code

							update invoice_institution_hdr
							set invoice_no       = @invoice_no,
								invoice_date     = @invoice_date,
								invoice_due_date = dateadd(d,@DUEDTDAYS,@invoice_date),
								approved         = 'Y',
								approved_by      = @updated_by,
								date_approved    = getdate()
							where institution_id   = @institution_id
							and billing_account_id = @billing_account_id
							and billing_cycle_id   = @billing_cycle_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @user_name = name from institutions where id = @institution_id
									select @error_code='233',@return_status=0
									return 0
								end


							if(select count(id) from invoice_institution_dtls where institution_id = @institution_id and billing_account_id = @billing_account_id and billing_cycle_id = @billing_cycle_id)>0
								begin
									update invoice_institution_dtls
									set approved     = 'Y',
									    --gl_code = (select isnull(gl_code,'') from modality where id = invoice_institution_dtls.modality_id),
										approved_by  = @updated_by,
										date_approved= getdate()
									where institution_id = @institution_id
									and billing_account_id = @billing_account_id
									and billing_cycle_id = @billing_cycle_id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @user_name = 'studies of ' + (select name from institutions where id = @institution_id)
											select @error_code='234',@return_status=0
											return 0
										end

									--update VRSMISDB
									update vrsmisdb..studies
									set invoiced = 'Y',
										invoiced_amount = isnull((select total_amount from invoice_institution_dtls where study_id =vrsmisdb..studies.id),0),
								        mis_updated_by  = @updated_by,
								        mis_updated_on  = getdate()
									where id  in  (select study_id
									               from invoice_institution_dtls
												   where institution_id = @institution_id
									               and billing_account_id = @billing_account_id
									               and billing_cycle_id = @billing_cycle_id)

									if(@@rowcount=0)
										begin
											rollback transaction
											select @user_name = 'studies of ' + (select name from institutions where id = @institution_id)
											select @error_code='492',@return_status=0
											return 0
										end
								end

							if(select count(id) from invoice_service_dtls where institution_id = @institution_id and billing_account_id = @billing_account_id and billing_cycle_id = @billing_cycle_id)>0
								begin
									update invoice_service_dtls
									set gl_code = (select isnull(gl_code,'') from services where id = invoice_service_dtls.service_id),
										updated_by  = @updated_by,
										date_updated= getdate()
									where institution_id = @institution_id
									and billing_account_id = @billing_account_id
									and billing_cycle_id = @billing_cycle_id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @user_name = 'service(s) of ' + (select name from institutions where id = @institution_id)
											select @error_code='234',@return_status=0
											return 0
										end
								end

							set @ctr = @ctr + 1
						end

					update invoice_hdr
					set invoice_srl_no   = @invoice_srl_no,
						invoice_srl_year = year(@invoice_date),
						invoice_no       = @invoice_no_hdr,
						invoice_date     = @invoice_date,
						invoice_due_date = dateadd(d,@DUEDTDAYS,@invoice_date),
						pick_for_mail    = 'Y',
						update_qb        = 'Y',
						approved         = 'Y',
						approved_by      = @updated_by,
						date_approved    = getdate()
					where billing_account_id = @billing_account_id
					and billing_cycle_id = @billing_cycle_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @user_name = name from billing_account where id = @billing_account_id
							select @error_code='232',@return_status=0
							return 0
						end

					set @counter = @counter + 1
					truncate table #tmp
			end

		if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
			begin
				exec common_lock_record_ui
					@menu_id       = @menu_id,
					@record_id     = @billing_cycle_id,
					@user_id       = @updated_by,
					@error_code    = @error_code output,
					@return_status = @return_status output	
						
				if(@return_status=0)
					begin
						return 0
					end
			end

		commit transaction
		exec sp_xml_removedocument @hDoc

	    set @return_status=1
	    set @error_code='034'
		set nocount off
		return 1
	end
GO
