USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_dtls_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_transcriptionist_payment_dtls_save]
GO
/****** Object:  StoredProcedure [dbo].[ap_transcriptionist_payment_dtls_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_transcriptionist_payment_dtls_save : 
                  save transcriptionist payment  details
** Created By   : Pavel Guha
** Created On   : 26/10/2020
*******************************************************/
create procedure [dbo].[ap_transcriptionist_payment_dtls_save]
	@billing_cycle_id uniqueidentifier,
	@approved nchar(1) ='N',
	@xml_transcriptionist  ntext,
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
	            @rowcount bigint

		declare @transcriptionist_id uniqueidentifier,
		        @study_id uniqueidentifier,
				@study_uid nvarchar(100),
				@adhoc_amount money,
				@payment_srl_no int,
				@payment_no nvarchar(50),
				@trans_pay_no_hdr nvarchar(50),
				@payment_date	 datetime,
		        @transcriptionist_code nvarchar(5),
				@transcriptionist_name nvarchar(100)

		declare @patient_name nvarchar(250)
		
		 exec common_check_record_lock
				@menu_id       = @menu_id,
				@record_id     = @menu_id,
				@user_id       = @updated_by,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
		if(@return_status=0)
			begin
				return 0
			end

		----********************---

		--*********************----

		begin transaction
		exec sp_xml_preparedocument @hDoc output,@xml_transcriptionist 

		
		 --Payment hdr
		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc,'transcriptionist/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @transcriptionist_id  = transcriptionist_id,
				           @study_id        = study_id,
						   @study_uid       = study_uid,
						   @adhoc_amount    = adhoc_amount
					from openxml(@hDoc,'transcriptionist/row',2)
					with
					( 
						transcriptionist_id uniqueidentifier,
						study_id uniqueidentifier,
						study_uid nvarchar(100),
						adhoc_amount money,
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  
					--*************************************************************
					    
					select @transcriptionist_code = isnull(code,''),
					       @transcriptionist_name = isnull(name,'')
					from transciptionists 
					where id = @transcriptionist_id

					if(@adhoc_amount =0)
						begin
							if(select count(transcriptionist_id) from ap_transcriptionist_adhoc_payments where transcriptionist_id=@transcriptionist_id and billing_cycle_id=@billing_cycle_id and study_id=@study_id)>0
								begin
									delete from ap_transcriptionist_adhoc_payments where transcriptionist_id=@transcriptionist_id and billing_cycle_id=@billing_cycle_id and study_id=@study_id

									if(@@rowcount=0)
										begin
											rollback transaction
											exec sp_xml_removedocument @hDoc
											select @user_name = @transcriptionist_name
											select @error_code='277',@return_status=0
											return 0
										end
								end
						end
					else if(@adhoc_amount <> 0)
						begin
							if(select count(transcriptionist_id) from ap_transcriptionist_adhoc_payments where transcriptionist_id=@transcriptionist_id and billing_cycle_id=@billing_cycle_id and study_id=@study_id)>0
								begin
									update ap_transcriptionist_adhoc_payments 
									set adhoc_payment = @adhoc_amount
									where transcriptionist_id=@transcriptionist_id 
									and billing_cycle_id=@billing_cycle_id 
									and study_id=@study_id
								end
							else
								begin
									insert into ap_transcriptionist_adhoc_payments(transcriptionist_id,billing_cycle_id,study_id,study_uid,adhoc_payment,updated_by,date_updated)
									                                   values(@transcriptionist_id,@billing_cycle_id,@study_id,@study_uid,@adhoc_amount,@updated_by,getdate())
								end

							if(@@rowcount=0)
								begin
									rollback transaction
									exec sp_xml_removedocument @hDoc
									select @user_name = @transcriptionist_name
									select @error_code='277',@return_status=0
									return 0
								end
						end

					if(@approved ='Y')
						begin
							select @trans_pay_no_hdr=isnull(payment_no,'') 
							from ap_transcriptionist_payment_hdr 
							where transcriptionist_id = @transcriptionist_id 
							and billing_cycle_id=@billing_cycle_id

							if(isnull(@trans_pay_no_hdr,'') ='')
								begin
									set @trans_pay_no_hdr =''
									set @payment_srl_no = 0
									exec ap_transcriptionist_payment_no_generate
									    @transcriptionist_code = @transcriptionist_code,
										@payment_srl_no        = @payment_srl_no output,
										@payment_no            = @trans_pay_no_hdr output
								end
							
							set @payment_no = @trans_pay_no_hdr + '/' + @transcriptionist_code
							set @payment_date = convert(datetime,convert(varchar(11),getdate(),106))

							update ap_transcriptionist_payment_hdr
							set payment_srl_no		= @payment_srl_no,
								payment_srl_year	= year(@payment_date),
								payment_no			= @trans_pay_no_hdr,
								payment_date		= @payment_date,
								approved			= @approved,
								approved_by         = @updated_by,
								date_approved       = getdate()
							where transcriptionist_id = @transcriptionist_id
							and billing_cycle_id = @billing_cycle_id

							if(@@rowcount=0)
								begin
									rollback transaction
									exec sp_xml_removedocument @hDoc
									select @user_name = @transcriptionist_name
									select @error_code='277',@return_status=0
									return 0
								end
					   end

					set @counter = @counter + 1
			end

		if(select count(record_id) from sys_record_lock where record_id=@menu_id and menu_id=@menu_id)=0
			begin
				exec common_lock_record
					@menu_id       = @menu_id,
					@record_id     = @menu_id,
					@user_id       = @updated_by,
					@error_code    = @error_code output,
					@return_status = @return_status output	
						
				if(@return_status=0)
					begin
						rollback transaction 
						exec sp_xml_removedocument @hDoc
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
