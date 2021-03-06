USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_other_adhoc_pmt_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_radiologist_payment_other_adhoc_pmt_save]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_other_adhoc_pmt_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_radiologist_payment_other_adhoc_pmt_save : 
                  save radiologist payment  details
** Created By   : Pavel Guha
** Created On   : 05/11/2020
*******************************************************/
create procedure [dbo].[ap_radiologist_payment_other_adhoc_pmt_save]
    @radiologist_id uniqueidentifier,
	@billing_cycle_id uniqueidentifier,
	@xml_payments  ntext,
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

		declare @adhoc_amount money,
				@head_id int,
				@remarks nvarchar(250),
				@head_name nvarchar(50)
		
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
		exec sp_xml_preparedocument @hDoc output,@xml_payments 

		
		 --Payment hdr
		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc,'payment/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @head_id         = head_id,
						   @adhoc_amount    = adhoc_amount,
						   @remarks         = remarks
					from openxml(@hDoc,'payment/row',2)
					with
					( 
						head_id int,
						adhoc_amount money,
						remarks nvarchar(250),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  
					--*************************************************************
					    
					select @head_name = isnull(name,'')
					from ap_adhoc_payment_heads 
					where id = @head_id

					if(@adhoc_amount =0)
						begin
							if(select count(radiologist_id) from ap_radiologist_other_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id and head_id=@head_id)>0
								begin
									delete from ap_radiologist_other_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id

									if(@@rowcount=0)
										begin
											rollback transaction
											exec sp_xml_removedocument @hDoc
											select @user_name = @head_name
											select @error_code='415',@return_status=0
											return 0
										end
								end
						end
					else if(@adhoc_amount <> 0)
						begin
							if(select count(radiologist_id) from ap_radiologist_other_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id and head_id=@head_id)>0
								begin
									update ap_radiologist_other_adhoc_payments 
									set adhoc_payment = @adhoc_amount,
									    remarks       = @remarks
									where radiologist_id = @radiologist_id 
									and billing_cycle_id = @billing_cycle_id 
									and head_id          = @head_id
								end
							else
								begin
									insert into ap_radiologist_other_adhoc_payments(radiologist_id,billing_cycle_id,head_id,adhoc_payment,remarks,updated_by,date_updated)
									                                         values(@radiologist_id,@billing_cycle_id,@head_id,@adhoc_amount,@remarks,@updated_by,getdate())
								end

							if(@@rowcount=0)
								begin
									rollback transaction
									exec sp_xml_removedocument @hDoc
									select @user_name = @head_name
									select @error_code='415',@return_status=0
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
