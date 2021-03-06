USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_bulk_disapprove]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_process_bulk_disapprove]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_process_bulk_disapprove]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_process_bulk_disapprove : 
                  disapprove invoices in bulk
** Created By   : Pavel Guha 
** Created On   : 19/05/2020
*******************************************************/
CREATE procedure [dbo].[invoicing_process_bulk_disapprove]
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
		        @institution_code nvarchar(5)


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

					select @invoice_no = invoice_no
					from invoice_hdr
					where billing_account_id = @billing_account_id
					and billing_cycle_id     = @billing_cycle_id

					if(select count(id) from ar_payments_adj where invoice_no=@invoice_no and billing_account_id =@billing_account_id)>0
						begin
							rollback transaction
							select @user_name= @invoice_no + ' of ' + (select name from billing_account where id = @billing_account_id)
							select @error_code='476',@return_status=0
							return 0
						end 
	
					insert into #tmp(institution_id)
					(select institution_id
					from invoice_institution_hdr
					where billing_account_id = @billing_account_id
					and billing_cycle_id = @billing_cycle_id)

					set @rc  =@@rowcount
					set @ctr =1

					while(@ctr<= @rc)
						begin
							select @institution_id = institution_id
							from #tmp
							where rec_id= @ctr

							update invoice_institution_hdr
							set approved            ='N',
								disapproved_by      = @updated_by,
								date_disapproved    = getdate()
							where institution_id    = @institution_id
							and billing_account_id  = @billing_account_id
							and billing_cycle_id    = @billing_cycle_id

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
									set approved        = 'N',
									    gl_code         = '',
										disapproved_by  = @updated_by,
										date_disapproved= getdate()
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

									if(select count(id) from invoice_service_dtls where institution_id = @institution_id and billing_account_id = @billing_account_id and billing_cycle_id = @billing_cycle_id)>0
										begin
											update invoice_service_dtls
											set gl_code      = '',
											    updated_by   = @updated_by,
												date_updated = getdate()
											where institution_id     = @institution_id
											and billing_account_id = @billing_account_id
											and billing_cycle_id    = @billing_cycle_id

											if(@@rowcount=0)
												begin
													rollback transaction
													select @user_name = 'service(s) of ' + (select name from institutions where id = @institution_id)
													select @error_code='234',@return_status=0
													return 0
												end
										end

									update study_hdr
									set invoiced='N'
									where id in (select study_id from invoice_institution_dtls where institution_id=@institution_id and billing_account_id = @billing_account_id and billing_cycle_id = @billing_cycle_id)

									update study_hdr_archive
									set invoiced='N'
									where id in (select study_id from invoice_institution_dtls where institution_id=@institution_id and billing_account_id = @billing_account_id and billing_cycle_id = @billing_cycle_id)

									--update VRSMISDB
									update vrsmisdb..studies
									set invoiced = 'N',
										invoiced_amount = 0,
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

							set @ctr = @ctr + 1
						end

					update invoice_hdr
					set approved            = 'N',
						pick_for_mail       = 'N',
						update_qb           = 'R',
						disapproved_by      = @updated_by,
						date_disapproved    = getdate()
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
