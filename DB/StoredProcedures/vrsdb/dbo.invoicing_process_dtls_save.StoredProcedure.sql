USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_dtls_save]    Script Date: 20-08-2021 20:43:16 ******/
DROP PROCEDURE [dbo].[invoicing_process_dtls_save]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_dtls_save]    Script Date: 20-08-2021 20:43:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_process_dtls_save : 
                  save invoicing processing details
** Created By   : Pavel Guha 
** Created On   : 12/11/2019
*******************************************************/
/*
	exec invoicing_process_dtls_save 'CD6E2FE4-9AC6-4010-9A9B-08EB687B09DD',
	'<account><row><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><total_amount>260</total_amount><row_id>1</row_id></row></account>',
	'<institution><row><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><total_amount>260</total_amount><approved><![CDATA[Y]]></approved><row_id>1</row_id></row></institution>',
	'<study><row><study_id><![CDATA[3ccd3da8-53be-4de1-81ba-f01c7d83d431]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Rusty Juarbe]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>1</row_id></row><row><study_id><![CDATA[2f13558a-5bce-4df5-a439-91744d3c3e47]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Cocco Grenwood]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>2</row_id></row><row><study_id><![CDATA[cf8fd9d0-6803-46e5-bb18-995e4294df93]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Sammi Wheat]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>3</row_id></row><row><study_id><![CDATA[8bf55763-c606-480d-91aa-3196f8674212]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Nona Simonenko]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>4</row_id></row><row><study_id><![CDATA[e9d35744-23d6-479e-a1ad-1fe67eddf467]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Angel Walsh]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>5</row_id></row><row><study_id><![CDATA[84662f93-df49-4191-b067-198adfc364d5]]></study_id><institution_id><![CDATA[6e864585-531f-4ec1-a41c-a8af4dd8f092]]></institution_id><billing_account_id><![CDATA[4b391e57-8529-4f47-bc4e-e2a222a48d72]]></billing_account_id><patient_name><![CDATA[Tinkerbell Puca]]></patient_name><approved><![CDATA[Y]]></approved><billed><![CDATA[0]]></billed><row_id>6</row_id></row></study>'
	,45,'11111111-1111-1111-1111-111111111111','','',0

*/
CREATE procedure [dbo].[invoicing_process_dtls_save]
	@billing_cycle_id uniqueidentifier,
	@xml_account      ntext,
	@xml_institution  ntext,
	@xml_study	      ntext,
	@menu_id          int,
    @updated_by       uniqueidentifier,
    @user_name        nvarchar(500) = '' output,
    @error_code       nvarchar(10)='' output,
    @return_status    int =0 output
as
	begin
		set nocount on
		declare @hDoc1 int,
			    @hDoc2 int,
			    @hDoc3 int,
		        @counter bigint,
	            @rowcount bigint,
				@ctr int,
	            @rc int

		declare @billing_account_id uniqueidentifier,
		        @study_invoice_amount money,
		        @total_amount money,
				@approved nchar(1),
				@inst_count int,
				@approve_count int,
				@invoice_srl_no int,
				@invoice_no nvarchar(50),
				@invoice_no_hdr nvarchar(50),
				@invoice_date datetime,
				@pick_for_mail nchar(1),
				@DUEDTDAYS int,
				@update_qb nchar(1)

		declare @institution_id uniqueidentifier,
		        @institution_code nvarchar(5)

		declare @study_id uniqueidentifier,
			    @patient_name nvarchar(250),
		        @billed nchar(1),
				@modality_id int,
				@category_id int,
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

		begin transaction
		exec sp_xml_preparedocument @hDoc1 output,@xml_account 
		exec sp_xml_preparedocument @hDoc2 output,@xml_institution 
		exec sp_xml_preparedocument @hDoc3 output,@xml_study 

		--institution dtls 
		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc3,'study/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @study_id            = study_id,
						   @institution_id      = institution_id,
						   @billing_account_id  = billing_account_id,
						   @patient_name        = patient_name,
						   @billed              = billed,
						   @approved            = approved
					from openxml(@hDoc3,'study/row',2)
					with
					( 
						study_id uniqueidentifier,
						institution_id uniqueidentifier,
						billing_account_id uniqueidentifier,
						billed nchar(1),
						patient_name nvarchar(250),
						approved  nchar(1),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					if(@approved='N')
						begin
							update invoice_institution_dtls
							set billed       = @billed,
							    gl_code      = '',
								approved     = @approved
							where study_id   = @study_id
							and institution_id = @institution_id
							and billing_account_id = @billing_account_id
							and billing_cycle_id = @billing_cycle_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @user_name = @patient_name + ' of ' + (select name from institutions where id = @institution_id)
									select @error_code='234',@return_status=0
									return 0
								end
						end
					else
						begin
							set @modality_id = 0
							set @category_id = 0

							select @modality_id = modality_id
							from invoice_institution_dtls
							where study_id   = @study_id
							and institution_id = @institution_id
							and billing_account_id = @billing_account_id
							and billing_cycle_id = @billing_cycle_id

							set @modality_id = isnull(@modality_id,0)

							if(@modality_id = 0)
								begin
									rollback transaction
									select @user_name = @patient_name + ' of ' + (select name from institutions where id = @institution_id)
									select @error_code='379',@return_status=0
									return 0
								end

							if(select count(id) from study_hdr where id=@study_id)>0
								begin
									select @category_id= category_id from study_hdr where id = @study_id
								end
							else if(select count(id) from study_hdr_archive where id=@study_id)>0
								begin
									select @category_id= category_id from study_hdr_archive where id = @study_id
								end

							set @category_id = isnull(@category_id,0)

							if(@category_id = 0)
								begin
									rollback transaction
									select @user_name = @patient_name + ' of ' + (select name from institutions where id = @institution_id)
									select @error_code='380',@return_status=0
									return 0
								end

							--select @gl_code = gl_code
							--from modality
							--where id=@modality_id

							--set @gl_code=isnull(@gl_code,'')

							--update invoice_institution_dtls
							--set billed       = @billed,
							--	gl_code      = @gl_code,
							--	approved     = @approved,
							--	approved_by  = @updated_by,
							--	date_approved= getdate()
							--where study_id   = @study_id
							--and institution_id = @institution_id
							--and billing_account_id = @billing_account_id
							--and billing_cycle_id = @billing_cycle_id

							--if(@@rowcount=0)
							--	begin
							--		rollback transaction
							--		select @user_name = @patient_name + ' of ' + (select name from institutions where id = @institution_id)
							--		select @error_code='234',@return_status=0
							--		return 0
							--	end

							if(select count(id) from invoice_service_dtls where study_id   = @study_id and institution_id = @institution_id and billing_account_id = @billing_account_id and billing_cycle_id = @billing_cycle_id)>0
								begin

									create table #tmpSvc(
										rec_id int identity(1,1),
										service_id int 
									)

									insert into #tmpSvc(service_id)
									(select service_id
									 from invoice_service_dtls
									 where study_id   = @study_id
									 and institution_id = @institution_id
									 and billing_account_id = @billing_account_id
									 and billing_cycle_id = @billing_cycle_id)

									 select @rc=@@rowcount,
									        @ctr=1

									while(@ctr <= @rc)
										begin
											select @service_id = service_id from #tmpSvc where rec_id=@ctr

											select @gl_code = gl_code from services where id=@service_id

											set @gl_code=isnull(@gl_code,'')

											update invoice_service_dtls
											set gl_code      = @gl_code,
											    updated_by   = @updated_by,
												date_updated = getdate()
											where study_id   = @study_id
											and institution_id     = @institution_id
											and service_id         = @service_id
											and billing_account_id = @billing_account_id
											and billing_cycle_id    = @billing_cycle_id

											if(@@rowcount=0)
												begin
													rollback transaction
													select @user_name = @patient_name + ' of ' + (select name from institutions where id = @institution_id) + ' - Service ' + (select name from services where id = @service_id)
													select @error_code='234',@return_status=0
													return 0
												end

											set @ctr = @ctr + 1
										end

									drop table #tmpSvc
								end

							if(select count(id) from study_hdr where id=@study_id)>0
								begin
									update study_hdr
									set invoiced='Y'
									where id  = @study_id
								end
						    else
								begin
									update study_hdr_archive
									set invoiced='Y'
									where id  = @study_id
								end

							if(@@rowcount=0)
								begin
									rollback transaction
									select @user_name = @patient_name + ' of ' + (select name from institutions where id = @institution_id)
									select @error_code='234',@return_status=0
									return 0
								end

							select @study_invoice_amount = total_amount  from invoice_institution_dtls where study_id = @study_id

							--update VRSMISDB
							update vrsmisdb..studies
							set invoiced = 'Y',
							    invoiced_amount = isnull(@study_invoice_amount,0),
								mis_updated_by  = @updated_by,
								mis_updated_on  = getdate()
							where id = @study_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @user_name = @patient_name + ' of ' + (select name from institutions where id = @institution_id)
									select @error_code='492',@return_status=0
									return 0
								end

						end
						 
					set @counter = @counter + 1
			end

		-- institution hdr
	    set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc2,'institution/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @institution_id      = institution_id,
						   @billing_account_id  = billing_account_id,
						   @total_amount        = total_amount,
						   @approved            = approved
					from openxml(@hDoc2,'institution/row',2)
					with
					( 
						institution_id uniqueidentifier,
						billing_account_id uniqueidentifier,
						total_amount money,
						approved  nchar(1),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  
					
					if(@approved='N')
						begin
							update invoice_institution_hdr
							set total_amount = @total_amount,
								approved     = @approved
							where institution_id = @institution_id
							and billing_account_id = @billing_account_id
							and billing_cycle_id = @billing_cycle_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @user_name = name from institutions where id = @institution_id
									select @error_code='233',@return_status=0
									return 0
								end

							
						end
					else
						begin
							select @institution_code = isnull(code,'') from institutions where id = @institution_id

							select @invoice_no_hdr=isnull(invoice_no,''),
							       @invoice_srl_no = isnull(invoice_srl_no,0) 
							from invoice_hdr 
							where billing_account_id = @billing_account_id 
							and billing_cycle_id=@billing_cycle_id

							if(isnull(@invoice_no_hdr,'') ='')
								begin
									set @invoice_no_hdr =''
									set @invoice_srl_no = 0
									exec invoicing_process_invoice_no_generate
											@invoice_srl_no = @invoice_srl_no output,
											@invoice_no     = @invoice_no_hdr output
								end

							set @invoice_no = @invoice_no_hdr + '/' + @institution_code
							set @invoice_date = convert(datetime,convert(varchar(11),getdate(),106))

							update invoice_institution_hdr
							set total_amount = @total_amount,
								invoice_no       = @invoice_no,
								invoice_date     = @invoice_date,
								invoice_due_date = dateadd(d,@DUEDTDAYS,@invoice_date),
								approved     = @approved,
								approved_by  = @updated_by,
								date_approved= getdate()
							where institution_id = @institution_id
							and billing_account_id = @billing_account_id
							and billing_cycle_id = @billing_cycle_id

							if(@@rowcount=0)
								begin
									rollback transaction
									select @user_name = name from institutions where id = @institution_id
									select @error_code='233',@return_status=0
									return 0
								end

							set @pick_for_mail='Y'
						end

					update invoice_hdr
					set invoice_srl_no   = @invoice_srl_no,
						invoice_srl_year = year(@invoice_date),
						invoice_no       = @invoice_no_hdr,
						invoice_date     = @invoice_date
					where billing_account_id = @billing_account_id
					and billing_cycle_id = @billing_cycle_id

					if(@@rowcount=0)
						begin
							rollback transaction
							select @user_name = name from institutions where id = @institution_id
							select @error_code='233',@return_status=0
							return 0
						end

						
					set @counter = @counter + 1
			end

		-- Invoice hdr
		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc1,'account/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @billing_account_id  = billing_account_id,
						   @total_amount        = total_amount
					from openxml(@hDoc1,'account/row',2)
					with
					( 
						billing_account_id uniqueidentifier,
						total_amount money,
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					select @inst_count = count(institution_id)
					from invoice_institution_hdr
					where billing_account_id=@billing_account_id
					and billing_cycle_id = @billing_cycle_id
					and total_study_count>0
					
					select @approve_count = count(institution_id)
					from invoice_institution_hdr
					where billing_account_id=@billing_account_id
					and billing_cycle_id = @billing_cycle_id
					and approved ='Y'
					and total_study_count>0

					if(@inst_count = @approve_count)
						begin
							set @approved='Y'
							set @pick_for_mail='Y'
							set @update_qb ='Y'
						end
					else
						begin
							set @approved='N'
							set @pick_for_mail='N'
							set @update_qb ='N'
						end

				  -- if(@approved ='Y')
						--begin
							
						--end
					--set @pick_for_mail ='N'
					--set @update_qb='N'

					update invoice_hdr
					set total_amount = @total_amount,
						invoice_due_date = dateadd(d,@DUEDTDAYS,@invoice_date),
						approved     = @approved,
						approved_by  = @updated_by,
						date_approved= getdate(),
						pick_for_mail= @pick_for_mail,
						update_qb    = @update_qb
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
		exec sp_xml_removedocument @hDoc1
		exec sp_xml_removedocument @hDoc2
		exec sp_xml_removedocument @hDoc3

	    set @return_status=1
	    set @error_code='034'
		set nocount off
		return 1
	end
GO
