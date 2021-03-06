USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_dtls_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ap_radiologist_payment_dtls_save]
GO
/****** Object:  StoredProcedure [dbo].[ap_radiologist_payment_dtls_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ap_radiologist_payment_dtls_save : 
                  save radiologist payment  details
** Created By   : BK
** Created On   : 30/12/2019
*******************************************************/
/*
exec ap_radiologist_payment_dtls_save '586953d4-2b3d-44dd-9349-b1c1b2701246','N',
'<radiologist><row><radiologist_id><![CDATA[218cc963-3ce3-4e76-950d-19f1d5f1852c]]></radiologist_id><study_id><![CDATA[5ef28b94-6a7f-40a4-ae79-5fddce44b2a5]]></study_id><study_uid><![CDATA[2.16.840.1.114440.1.2.5.1866.6295.20200813.153336846.67456395]]></study_uid><adhoc_amount>0</adhoc_amount><row_id>1</row_id></row><row><radiologist_id><![CDATA[218cc963-3ce3-4e76-950d-19f1d5f1852c]]></radiologist_id><study_id><![CDATA[b791ee0d-fdf2-4220-a178-60754b2ab1ac]]></study_id><study_uid><![CDATA[1.2.392.200046.100.14.94762917562545336858155637376256122613]]></study_uid><adhoc_amount>7</adhoc_amount><row_id>2</row_id></row><row><radiologist_id><![CDATA[218cc963-3ce3-4e76-950d-19f1d5f1852c]]></radiologist_id><study_id><![CDATA[f109a5f5-c168-44b9-a258-ef4f69f6f94e]]></study_id><study_uid><![CDATA[1.2.826.0.1.3680043.2.950.284290.1142.20200813171115]]></study_uid><adhoc_amount>0</adhoc_amount><row_id>3</row_id></row><row><radiologist_id><![CDATA[218cc963-3ce3-4e76-950d-19f1d5f1852c]]></radiologist_id><study_id><![CDATA[de2e83e9-ef6e-4d4e-ac99-6faa226ddbe4]]></study_id><study_uid><![CDATA[1.2.840.1136982020081607313050942.443]]></study_uid><adhoc_amount>0</adhoc_amount><row_id>4</row_id></row></radiologist>',
52,'11111111-1111-1111-1111-111111111111','','',0
*/
CREATE procedure [dbo].[ap_radiologist_payment_dtls_save]
	@billing_cycle_id uniqueidentifier,
	@approved nchar(1) ='N',
	@xml_radiologist  ntext,
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

		declare @radiologist_id uniqueidentifier,
		        @study_id uniqueidentifier,
				@study_uid nvarchar(100),
				@adhoc_amount money,
				@payment_srl_no int,
				@payment_no nvarchar(50),
				@rad_pay_no_hdr nvarchar(50),
				@payment_date	 datetime,
		        @radiologist_code nvarchar(5),
				@radiologist_name nvarchar(100)

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
		exec sp_xml_preparedocument @hDoc output,@xml_radiologist 

		
		 --Payment hdr
		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc,'radiologist/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				   select  @radiologist_id  = radiologist_id,
				           @study_id        = study_id,
						   @study_uid       = study_uid,
						   @adhoc_amount    = adhoc_amount
					from openxml(@hDoc,'radiologist/row',2)
					with
					( 
						radiologist_id uniqueidentifier,
						study_id uniqueidentifier,
						study_uid nvarchar(100),
						adhoc_amount money,
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  
					--*************************************************************
					    
					select @radiologist_code = isnull(code,''),
					       @radiologist_name = isnull(name,'')
					from radiologists 
					where id = @radiologist_id

					if(@adhoc_amount =0)
						begin
							if(select count(radiologist_id) from ap_radiologist_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id and study_id=@study_id)>0
								begin
									delete from ap_radiologist_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id and study_id=@study_id

									if(@@rowcount=0)
										begin
											rollback transaction
											exec sp_xml_removedocument @hDoc
											select @user_name = @radiologist_name
											select @error_code='277',@return_status=0
											return 0
										end
								end
						end
					else if(@adhoc_amount <> 0)
						begin
							if(select count(radiologist_id) from ap_radiologist_adhoc_payments where radiologist_id=@radiologist_id and billing_cycle_id=@billing_cycle_id and study_id=@study_id)>0
								begin
									update ap_radiologist_adhoc_payments 
									set adhoc_payment = @adhoc_amount
									where radiologist_id=@radiologist_id 
									and billing_cycle_id=@billing_cycle_id 
									and study_id=@study_id
								end
							else
								begin
									insert into ap_radiologist_adhoc_payments(radiologist_id,billing_cycle_id,study_id,study_uid,adhoc_payment,updated_by,date_updated)
									                                   values(@radiologist_id,@billing_cycle_id,@study_id,@study_uid,@adhoc_amount,@updated_by,getdate())
								end

							if(@@rowcount=0)
								begin
									rollback transaction
									exec sp_xml_removedocument @hDoc
									select @user_name = @radiologist_name
									select @error_code='277',@return_status=0
									return 0
								end
						end

					if(@approved ='Y')
						begin
							select @rad_pay_no_hdr=isnull(payment_no,'') 
							from ap_radiologist_payment_hdr 
							where radiologist_id = @radiologist_id 
							and billing_cycle_id=@billing_cycle_id

							if(isnull(@rad_pay_no_hdr,'') ='')
								begin
									set @rad_pay_no_hdr =''
									set @payment_srl_no = 0
									exec ap_radiologist_payment_no_generate
									    @radiologist_code = @radiologist_code,
										@payment_srl_no   = @payment_srl_no output,
										@payment_no       = @rad_pay_no_hdr output
								end
							
							set @payment_no = @rad_pay_no_hdr + '/' + @radiologist_code
							set @payment_date = convert(datetime,convert(varchar(11),getdate(),106))

							update ap_radiologist_payment_hdr
							set payment_srl_no		= @payment_srl_no,
								payment_srl_year	= year(@payment_date),
								payment_no			= @rad_pay_no_hdr,
								payment_date		= @payment_date,
								approved			= @approved,
								approved_by         = @updated_by,
								date_approved       = getdate()
							where radiologist_id = @radiologist_id
							and billing_cycle_id = @billing_cycle_id

							if(@@rowcount=0)
								begin
									rollback transaction
									exec sp_xml_removedocument @hDoc
									select @user_name = @radiologist_name
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
