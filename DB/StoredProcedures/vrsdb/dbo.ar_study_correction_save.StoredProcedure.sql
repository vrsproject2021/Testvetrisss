USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_correction_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_study_correction_save]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_correction_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_study_correction_save : 
                  save study corrections
** Created By   : Pavel Guha 
** Created On   : 31/01/2020
*******************************************************/
/*
	exec ar_study_correction_save 'cd6e2fe4-9ac6-4010-9a9b-08eb687b09dd',
	'<study><row><study_id><![CDATA[60dc0250-a9be-461f-84fa-646fb5f0b030]]></study_id><institution_id><![CDATA[763444b5-7607-4ce6-a9cd-95d433380243]]></institution_id><modality_id>2</modality_id><priority_id>2</priority_id><patient_name><![CDATA[Miller Bailey]]></patient_name><row_id>1</row_id></row></study>',
	56,'11111111-1111-1111-1111-111111111111','','',0
*/
CREATE procedure [dbo].[ar_study_correction_save]
	@billing_cycle_id uniqueidentifier,
	@xml_study        ntext=null,
	@xml_rates        ntext=null,
	@menu_id          int,
    @updated_by       uniqueidentifier,
    @user_name        nvarchar(500) = '' output,
    @error_code       nvarchar(10)='' output,
    @return_status    int =0 output
as
	begin
		set nocount on

		declare @recorded_institution_id uniqueidentifier,
		        @recorded_billing_account_id uniqueidentifier,
				@billing_account_id uniqueidentifier,
				@study_id uniqueidentifier,
				@study_uid nvarchar(100),
				@institution_id uniqueidentifier,
				@institution_code nvarchar(5),
				@institution_name nvarchar(100),
				@modality_id int,
				@priority_id int,
				@category_id int,
				@patient_name nvarchar(250),
				@modality_code nvarchar(5),
				@service_codes nvarchar(250),
				@rate money,
				@head_type nchar(1),
				@head_id int

		declare @hDoc int,
		        @hDoc1 int,
		        @counter bigint,
	            @rowcount bigint

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

		create table #tmpBA
		(
			rec_id int identity(1,1),
			billing_account_id uniqueidentifier
		)

	 --    exec common_check_record_lock_ui
		--		@menu_id       = @menu_id,
		--		@record_id     = @billing_cycle_id,
		--		@user_id       = @updated_by,
		--		@user_name     = @user_name output,
		--		@error_code    = @error_code output,
		--		@return_status = @return_status output
		
		--if(@return_status=0)
		--	begin
		--		return 0
		--	end

		
		begin transaction

		if(@xml_study is not null) exec sp_xml_preparedocument @hDoc output,@xml_study 
		if(@xml_rates is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_rates 

		if(@xml_study is not null)
			begin
				set @counter = 1
				select  @rowcount=count(row_id)  
				from openxml(@hDoc,'study/row', 2)  
				with( row_id bigint)

				while(@counter <= @rowcount)
					begin
						select  @study_id            = study_id,
								@institution_id      = institution_id,
								@modality_id         = modality_id,
								@priority_id         = priority_id,
								@category_id         = category_id,
								@patient_name        = patient_name,
								@service_codes       = service_codes
						from openxml(@hDoc,'study/row',2)
						with
						( 
							study_id uniqueidentifier,
							institution_id uniqueidentifier,
							modality_id int,
							priority_id  int,
							category_id int,
							patient_name nvarchar(250),
							service_codes nvarchar(250),
							row_id bigint
						) xmlTemp where xmlTemp.row_id = @counter  

						select @institution_code = code,
							   @institution_name = name
						from institutions
						where id = @institution_id

						if(select count(id) from study_hdr where id = @study_id)>0
							begin
								select @study_uid = study_uid,
									   @recorded_institution_id = institution_id
								from study_hdr
								where id = @study_id
							 end
						else if(select count(id) from study_hdr_archive where id = @study_id)>0
							begin
								select @study_uid = study_uid,
									   @recorded_institution_id = institution_id
								from study_hdr_archive archive
								where id = @study_id
							end

						select @recorded_billing_account_id = billing_account_id from institutions where id = @recorded_institution_id
						select @billing_account_id = billing_account_id from institutions where id = @institution_id
						select @modality_code = code from modality where id = @modality_id


						if(select count(id) from study_hdr where id = @study_id)>0
							begin
								update study_hdr
								set  institution_id = @institution_id,
									 modality_id    = @modality_id,
									 category_id    = @category_id,
									 priority_id    = @priority_id,
									 service_codes  = @service_codes
								where id = @study_id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code ='066',@return_status =0,@user_name = @patient_name
										return 0
									end

							end
						else if(select count(id) from study_hdr_archive where id = @study_id)>0
							begin
								update study_hdr_archive
								set  institution_id = @institution_id,
									 modality_id    = @modality_id,
									 category_id    = @category_id,
									 priority_id    = @priority_id,
									 service_codes  = @service_codes
								where id = @study_id

								if(@@rowcount=0)
									begin
										rollback transaction
										select @error_code ='066',@return_status =0,@user_name = @patient_name
										return 0
									end
							end

						if(select count(id) from scheduler_file_downloads where id = @study_id)>0
							begin
								update scheduler_file_downloads
								set  institution_id   = @institution_id,
									 institution_code = @institution_code,
									 institution_name = @institution_name,
									 modality         = @modality_code,
									 priority_id      = @priority_id
								where id = @study_id


								if(@@rowcount =0)
									begin
										rollback transaction
										select @error_code ='066',@return_status =0,@user_name= @patient_name
										return 0
									end

							end

						if(select count(id) from scheduler_img_file_downloads_grouped where id = @study_id)>0
							begin
								update scheduler_img_file_downloads_grouped
								set  institution_id   = @institution_id,
									 institution_code = @institution_code,
									 institution_name = @institution_name,
									 modality_id      = @modality_id,
									 modality         = @modality_code,
									 priority_id      = @priority_id,
									 category_id      = @category_id
								where id = @study_id

								if(@@rowcount =0)
									begin
										rollback transaction
										select @error_code ='066',@return_status =0,@user_name= @patient_name
										return 0
									end

								if(select count(id) from scheduler_img_file_downloads_ungrouped where grouped_id=@study_id)>0
									 begin
										 update scheduler_img_file_downloads_ungrouped
										 set  institution_id   = @institution_id,
											  institution_code = @institution_code,
											  institution_name = @institution_name
										 where grouped_id = @study_id

										 if(@@rowcount =0)
												begin
													rollback transaction
													select @error_code ='066',@return_status =0,@user_name= @patient_name
													return 0
												end
									 end
							end

						if(select count(id) from invoice_institution_dtls where study_id = @study_id and billing_cycle_id=@billing_cycle_id)>0
						   begin
								if(select count(rec_id) from #tmpBA where billing_account_id = @recorded_billing_account_id)=0
									begin
										insert into #tmpBA(billing_account_id) values (@recorded_billing_account_id)
									end
								if(select count(rec_id) from #tmpBA where billing_account_id = @billing_account_id)=0
									begin
										insert into #tmpBA(billing_account_id) values (@billing_account_id)
									end

								--set @user_name=''
								--set @error_code =''
								--set @return_status = 0

								--exec ar_study_correction_invoice_reprocess
								--	@billing_cycle_id   = @billing_cycle_id,
								--	@billing_account_id = @recorded_billing_account_id,
								--	@menu_id            = 45,
								--	@user_id            = @updated_by,
								--	@user_name          = @user_name output,
								--	@error_code         = @error_code output,
								--	@return_status      = @return_status output

								--if(@return_status=0)
								--	begin
								--		rollback transaction
								--		return 0
								--	end

					       end
	 
						if(@recorded_billing_account_id <> @billing_account_id)
							begin
								if(select count(rec_id) from #tmpBA where billing_account_id = @recorded_billing_account_id)=0
									begin
										insert into #tmpBA(billing_account_id) values (@recorded_billing_account_id)
									end
								if(select count(rec_id) from #tmpBA where billing_account_id = @billing_account_id)=0
									begin
										insert into #tmpBA(billing_account_id) values (@billing_account_id)
									end
								--set @user_name=''
								--set @error_code =''
								--set @return_status = 0

								--exec ar_study_correction_invoice_reprocess
								--	@billing_cycle_id   = @billing_cycle_id,
								--	@billing_account_id = @recorded_billing_account_id,
								--	@menu_id            = 45,
								--	@user_id            = @updated_by,
								--	@user_name          = @user_name output,
								--	@error_code         = @error_code output,
								--	@return_status      = @return_status output

								--if(@return_status=0)
								--	begin
								--		rollback transaction
								--		return 0
								--	end
							end

						set @counter = @counter + 1

					end
		   end

		if(@xml_rates is not null)
			begin
				set @counter = 1
				select  @rowcount=count(row_id)  
				from openxml(@hDoc1,'rate/row', 2)  
				with( row_id bigint)

				  while(@counter <= @rowcount)
					begin
						select  @study_id            = study_id,
								@study_uid           = study_uid,
								@institution_id      = institution_id,
								@rate                = rate,
								@head_type           = head_type,
								@head_id             = head_id
						from openxml(@hDoc1,'rate/row',2)
						with
						( 
							study_id uniqueidentifier,
							study_uid nvarchar(100),
							institution_id uniqueidentifier,
							rate money,
							head_type  nchar(1),
							head_id int,
							row_id bigint
						) xmlTemp where xmlTemp.row_id = @counter  

						if(select count(id) from study_hdr where id = @study_id)>0
							begin
								select @study_uid = study_uid,
									   @recorded_institution_id = institution_id,
									   @patient_name  = patient_name,
									   @category_id = category_id
								from study_hdr
								where id = @study_id
							 end
						else if(select count(id) from study_hdr_archive where id = @study_id)>0
							begin
								select @study_uid = study_uid,
									   @recorded_institution_id = institution_id,
									   @patient_name  = patient_name,
									   @category_id = category_id
								from study_hdr_archive archive
								where id = @study_id
							end
						
						select @recorded_billing_account_id = billing_account_id from institutions where id = @recorded_institution_id
				        select @billing_account_id = billing_account_id from institutions where id = @institution_id
						
						if(select count(study_hdr_id) from ar_amended_rates where study_hdr_id = @study_id and head_type = @head_type and head_id = @head_id and billing_cycle_id = billing_cycle_id)>0
							begin
								update ar_amended_rates
								set  billing_account_id = @billing_account_id,
								     institution_id     = @institution_id,
									 study_hdr_id       = @study_id,
									 study_uid          = @study_uid,
									 category_id        = @category_id,
									 rate               = @rate,
									 updated_by         = @updated_by,
									 date_updated       = getdate()
								where study_hdr_id = @study_id
								and head_type = @head_type
								and head_id = @head_id
								and billing_cycle_id = billing_cycle_id
							end
						else
							begin
								insert into ar_amended_rates(billing_cycle_id,billing_account_id,institution_id,study_hdr_id,study_uid,rate,head_type,head_id,category_id,updated_by,date_updated)
								                      values(@billing_cycle_id,@billing_account_id,@institution_id,@study_id,@study_uid,@rate,@head_type,@head_id,@category_id,@updated_by,getdate())
							end

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_code ='440',@return_status =0,@user_name = @patient_name
								return 0
							end


						if(select count(id) from invoice_institution_dtls where  billing_cycle_id=@billing_cycle_id and study_id=@study_id)>0
							begin
								if(select count(rec_id) from #tmpBA where billing_account_id = @recorded_billing_account_id)=0
									begin
										insert into #tmpBA(billing_account_id) values (@recorded_billing_account_id)
									end
								if(select count(rec_id) from #tmpBA where billing_account_id = @billing_account_id)=0
									begin
										insert into #tmpBA(billing_account_id) values (@billing_account_id)
									end
								--set @user_name=''
								--set @error_code =''
								--set @return_status = 0

								--exec ar_study_correction_invoice_reprocess
								--	@billing_cycle_id   = @billing_cycle_id,
								--	@billing_account_id = @recorded_billing_account_id,
								--	@menu_id            = 45,
								--	@user_id            = @updated_by,
								--	@user_name          = @user_name output,
								--	@error_code         = @error_code output,
								--	@return_status      = @return_status output

								--if(@return_status=0)
								--	begin
								--		rollback transaction
								--		return 0
								--	end

							end

						set @counter = @counter + 1

					end
		
			end

		if(select count(rec_id) from #tmpBA)>0
			begin
				select @rowcount = count(rec_id) from #tmpBA
				select @counter =1

				while(@counter<=@rowcount)
					begin
						select @billing_account_id = billing_account_id
						from #tmpBA
						where rec_id = @counter

						set @user_name=''
						set @error_code =''
						set @return_status = 0

						exec ar_study_correction_invoice_reprocess
							@billing_cycle_id   = @billing_cycle_id,
							@billing_account_id = @billing_account_id,
							@with_tran          = 'N',
							@menu_id            = 45,
							@user_id            = @updated_by,
							@user_name          = @user_name output,
							@error_code         = @error_code output,
							@return_status      = @return_status output

						if(@return_status=0)
							begin
								rollback transaction
								return 0
							end

						set @counter = @counter + 1
					end
			end

		commit transaction
		if(@xml_study is not null) exec sp_xml_removedocument @hDoc
		if(@xml_rates is not null) exec sp_xml_removedocument @hDoc1

	    set @return_status=1
	    set @error_code='034'
		set nocount off
		return 1
	end
GO
