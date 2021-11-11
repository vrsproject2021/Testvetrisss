USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_save]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments : save
** Created By   : KC
** Created On   : 07/04/2020
*******************************************************/
CREATE procedure [dbo].[ar_payments_save]
(
	@id						uniqueidentifier	= '00000000-0000-0000-0000-000000000000' output,
	@billing_account_id		uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@payment_mode           nvarchar(1) = NULL,
	@payref_no              nvarchar(50) = NULL output,
	@payref_date            datetime = '01jan1900' output,
	@processing_ref_no      nvarchar(100) = '',
	@processing_ref_date    datetime = NULL,
	@processing_pg_name     nvarchar(50) = '',
	@processing_status      nchar(1) = NULL,
	@payment_amount         money = 0,
	@payment_tool           nchar(1),
	@auth_code              nvarchar(50) = '',
	@cvv_response           nvarchar(50) = '',
	@avs_response           nvarchar(50) = '',
	@payment_tool_holder_name nvarchar(100) = '',
	@xml_adjustments        ntext = NULL,
	@remarks				nvarchar(150) = '',
	@vault_id               uniqueidentifier ='00000000-0000-0000-0000-000000000000',
	@user_id				uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(700)		= '' output,
	@error_code				nvarchar(10)		= '' output,
    @return_status			int					= 0  output
)
as
	begin
		declare
				@isupdating int = 0,
		        @count int,
	            @year int,
				@payment_no nvarchar(30);

		declare @hDoc1 int;

		declare @adjustments table (
			invoice_header_id uniqueidentifier,
			invoice_no nvarchar(50), 
			invoice_date datetime,
			adj_amount  money,
			rowid int
		)

		/***********ADDED BY PAVEL ON 08 May 2020******************/
		declare @acct_user_id nvarchar(100),
				@user_email_id nvarchar(100),
				@receipient_name nvarchar(100),
		        @user_email_missing nchar(1),
		        @SENDMAILID nvarchar(200),
				@SENDMAILPWD nvarchar(200),
		        @OPMAILCC nvarchar(200),
				@email_log_id uniqueidentifier,
				@email_text varchar(max),
				@email_subject nvarchar(250),
				@invoice_no nvarchar(50),
				@invoice_date datetime,
				@rowcount int,
				@counter int
		/***********ADDED BY PAVEL ON 08 May 2020******************/
		
		if(ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000')
			begin
				select @billing_account_id=id
				from billing_account where login_user_id = @user_id;
			end

		
		begin transaction

		if(@id = '00000000-0000-0000-0000-000000000000')
			begin
				select @year=Year(GETDATE()) 
				 -- update counter
				if(select count(ar_payments_year) from sys_ar_payments_params where ar_payments_year = @year)>0
					begin
						update sys_ar_payments_params SET ar_payments_count=ar_payments_count+1 where ar_payments_year = @year;
					end
				else
					begin
						insert into sys_ar_payments_params(ar_payments_year,ar_payments_count) values(@year,1)
					end

				 set @id = newid();

				 select @count=ar_payments_count  
				 from sys_ar_payments_params 
				  where ar_payments_year = Year(GETDATE());

					set @payment_no = 'P/'+convert(nvarchar(4),@year) + '/' + convert(nvarchar(10),isnull(@count,0))
					set @payref_date = convert(datetime,convert(varchar(11),getdate(),106));
					if(@payment_mode=0) set @processing_ref_date= getdate()

					

				 INSERT INTO dbo.ar_payments
					(
						id,
						billing_account_id,
						payment_mode,
						payref_no,
						payref_date,
						processing_ref_no,
						processing_ref_date,
						processing_pg_name,
						processing_status,
						payment_amount,
						payment_tool,
						payment_tool_holder_name,
						auth_code,
						cvv_response,
						avs_response,
						remarks,
						created_by,
						date_created,
						updated_by,
						date_updated
					)
					SELECT 
						@id,
						@billing_account_id,
						@payment_mode,
						@payment_no,
						@payref_date,
						@processing_ref_no,
						@processing_ref_date,
						@processing_pg_name,
						@processing_status,
						@payment_amount,
						@payment_tool,
						isnull(@payment_tool_holder_name,''),
						@auth_code,
						@cvv_response,
						@avs_response,
						@remarks,
						@user_id,
						GETDATE(),
						NULL,
						NULL

				if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end
			end
		else
			begin
				set @isupdating=1;
				exec common_check_record_lock_ui
					@menu_id       = @menu_id,
					@record_id     = @id,
					@user_id       = @user_id,
					@user_name     = @user_name output,
					@error_code    = @error_code output,
					@return_status = @return_status output
		
				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end

				/***********ADDED BY PAVEL ON 08 May 2020******************/
				select @payment_no = payref_no
				from ar_payments
				where id=@id
				/***********ADDED BY PAVEL ON 08 May 2020******************/

				UPDATE ar_payments
					   SET 
						   processing_ref_no        = @processing_ref_no,
						   payment_amount		    = @payment_amount,
						   processing_status        = @processing_status,
						   payment_tool             = @payment_tool,
						   payment_tool_holder_name = isnull(@payment_tool_holder_name,''),
						   auth_code                = @auth_code,
						   cvv_response             = @cvv_response,
						   avs_response             = @avs_response,
						   remarks				    = @remarks,
						   updated_by               = @user_id,
						   date_updated             = GETDATE()
					 WHERE id = @id

				if(@@rowcount=0)
					begin
						rollback transaction
						select	@return_status=0,@error_code='035'
						return 0
					end

			end
	
		--adjustments 

	   if(not (@xml_adjustments is null OR datalength(@xml_adjustments)=0))
		   begin
				exec sp_xml_preparedocument @hDoc1 output, @xml_adjustments 
				-- take adjust rows into @adjustments 
				insert into @adjustments (invoice_header_id,invoice_no,invoice_date,adj_amount,rowid)
				select x.invoice_header_id,x.invoice_no,x.invoice_date, x.adj_amount, x.rowid 
				from openxml(@hDoc1,'adjustments/row',2)
				with
				( 
					invoice_header_id uniqueidentifier,
					invoice_no nvarchar(50),
					invoice_date datetime,
					adj_amount  money,
					rowid int
				) x;
				
				-- delete existing entries from adjustments if any with the @id
				delete adj 
				from ar_payments_adj adj
			    inner join @adjustments x on x.invoice_header_id=adj.invoice_header_id
				where adj.ar_payments_id = @id;

				-- insert adjustments
				insert into ar_payments_adj(id, invoice_header_id, ar_payments_id, billing_account_id,adj_amount, 
				                            invoice_no,invoice_date,created_by,date_created, 
											updated_by, date_updated)
				(select newid(), invoice_header_id,@id,@billing_account_id,adj_amount,
				        invoice_no,invoice_date,@user_id,getdate(), 
				        case @isupdating when 1 then @user_id else null end, 
				        case @isupdating when 1 then getdate() else null end
				  from @adjustments)
			end

		/***********ADDED BY PAVEL ON 08 May 2020******************/
		if(@processing_status='1')
			begin
				update ar_payments set post_to_qb = 'Y' where id=@id

				if(@@rowcount=0)
				begin
					rollback transaction
					select	@return_status=0,@error_code='035'
					return 0
				end
			end
		/***********CREATE EMAIL NOTIFICATION FOR SUCCESSFUL ONLINE PAYMENT******************/
		if(@payment_mode='1' and @processing_status='1')
			begin
				select @acct_user_id = isnull(login_user_id,'00000000-0000-0000-0000-000000000000')
				from billing_account
				where id = @billing_account_id

				select @user_email_id   = email_id,
					   @receipient_name = name
				from users 
				where id = @acct_user_id

				if(isnull(@user_email_id,'')<>'')
					begin
						set @user_email_missing='N'
						select @SENDMAILID=data_value_char from invoicing_control_params where control_code='SENDMAILID'
						select @SENDMAILPWD=data_value_char from invoicing_control_params where control_code='SENDMAILPWD'
						select @OPMAILCC=data_value_char from invoicing_control_params where control_code='OPMAILCC'
						select @rowcount = count(rowid) from @adjustments
						

						set @email_subject = 'Your Payment Transaction Receipt #' + @payment_no

						set @email_text = 'Dear Sir/Madam,' + '<br/>'
						set @email_text = @email_text + 'Payment received against the invoice number(s) :<br/>'
						set @counter=1
						while(@counter <= @rowcount)
							begin
								select @invoice_no = invoice_no,
								       @invoice_date = invoice_date
								from @adjustments
								where rowid= @counter

								set @email_text = @email_text + '<b>' + @invoice_no + ' dated ' + convert(varchar,@invoice_date,107) + '</b><br/>'

								set @counter = @counter+1
							end

						set @email_text = @email_text + '<br/>'
						set @email_text = @email_text + 'Below is your receipt with all relevant transaction information for your records.<br/><br/>'
						set @email_text = @email_text + convert(varchar, @processing_ref_date,107) + '<br/>'
						set @email_text = @email_text + format(@processing_ref_date,'hh:mm:ss tt') + ' CDT<br/>'
						set @email_text = @email_text + '<b>' + (select format(@payment_amount, N'c', C.culture) as payment_amount from (values ('en-US')) C (culture)) +'</b><br/><br/>'
						

						if(@payment_tool='C')
							set @email_text = @email_text +  '<b>Type</b>'+ replicate('&nbsp;',(50  - (14 - (14 - len('Type')))) + 7) + 'Card Sale<br/>'
						else if(@payment_tool='A')
							set @email_text = @email_text +  '<b>Type</b>'+ replicate('&nbsp;',(50  - (14 - (14 - len('Type')))) + 7) + 'Check Sale<br/>'
							
						set @email_text = @email_text + '<b>Transaction ID</b>'+ replicate('&nbsp;',50  - (14 - (14 - len('Transaction ID')))) + @processing_ref_no + '<br/>'  
						set @email_text = @email_text + '<b>Auth Code</b>'+ replicate('&nbsp;',(50  - (14 - (14 - len('Auth Code')))) + 2) + @auth_code + '<br/>'  
						set @email_text = @email_text + '<b>Response Text</b>'+ replicate('&nbsp;',50  - (14 - (14 - len('Response Text')))) + @remarks + '<br/>'
						set @email_text = @email_text + '<b>AVS Response</b>'+ replicate('&nbsp;',50  - (14 - (14 - len('AVS Response')))) + @avs_response + '<br/>'
						set @email_text = @email_text + '<b>CVV Response</b>'+ replicate('&nbsp;',50  - (14 - (14 - len('CVV Response')))) + @cvv_response + '<br/>'
						set @email_text = @email_text + '<br/>'
						set @email_text = @email_text + '<b>Billing Details</b>'+ '<br/>'
						
						--if(@payment_tool='C')
						--	set @email_text = @email_text +  '<b>Type</b>'+ space(50  - (14 - (14 - len('Type')))) + 'Card Sale<br/>'
						--else if(@payment_tool='A')
						--	set @email_text = @email_text +  '<b>Type</b>'+ space(50  - (14 - (14 - len('Type')))) + 'Check Sale<br/>'
							
						--set @email_text = @email_text + '<b>Transaction ID</b>'+ space(50 - (14 - (14 - len('Transaction ID')))) + @processing_ref_no + '<br/>'  
						--set @email_text = @email_text + '<b>Auth Code</b>'+ space(50 - (14 - (14 - len('Auth COde')))) + @auth_code + '<br/>'  
						--set @email_text = @email_text + '<b>Response Text</b>'+ space(50 - (14 - (14 - len('Response Text')))) + @remarks + '<br/>'
						--set @email_text = @email_text + '<b>AVS Response</b>'+ space(50 - (14 - (14 - len('AVS Response')))) + @avs_response + '<br/>'
						--set @email_text = @email_text + '<b>CVV Response</b>'+ space(50 - (14 - (14 - len('CVV Response')))) + @cvv_response + '<br/>'
						--set @email_text = @email_text + '<br/>'
						--set @email_text = @email_text + '<b>Billing Details</b>'+ '<br/>'

						if(rtrim(ltrim(isnull(@payment_tool_holder_name,'')))<>'')
							begin
								if(isnull(@vault_id,'00000000-0000-0000-0000-000000000000')<>'00000000-0000-0000-0000-000000000000')
									begin
										select @payment_tool_holder_name= holder_name
										from billing_account_vault
										where billing_account_id=@billing_account_id
										and vault_id =@vault_id
									end
							end

						set @email_text = @email_text + isnull(@payment_tool_holder_name,'') + '<br/>'
						set @email_text = @email_text + (select name from billing_account where id=@billing_account_id)+ '<br/>'
						if(select rtrim(ltrim(isnull(address_1,''))) from billing_account where id=@billing_account_id)<>''
							begin
								set @email_text = @email_text + (select rtrim(ltrim(address_1)) from billing_account where id=@billing_account_id)+ '<br/>'
							end	
						if(select rtrim(ltrim(isnull(address_2,''))) from billing_account where id=@billing_account_id)<>''
							begin
								set @email_text = @email_text + (select rtrim(ltrim(address_2)) from billing_account where id=@billing_account_id)+ '<br/>'
							end
						if(select rtrim(ltrim(isnull(city,'')))	from billing_account where id=@billing_account_id)<>''
							begin
								set @email_text = @email_text + (select rtrim(ltrim(city)) from billing_account where id=@billing_account_id)+ '<br/>'
							end
						if(select isnull(state_id,0) from billing_account where id=@billing_account_id)<>0
							begin
								set @email_text = @email_text + (select rtrim(ltrim(name)) from sys_states where id=(select state_id from billing_account where id=@billing_account_id))+ '<br/>'
							end
						if(select isnull(country_id,0) from billing_account where id=@billing_account_id)<>0
							begin
								set @email_text = @email_text + (select rtrim(ltrim(name)) from sys_country where id=(select country_id from billing_account where id=@billing_account_id))+ '<br/>'
							end
						if(select rtrim(ltrim(isnull(zip,'')))	from billing_account where id=@billing_account_id)<>''
							begin
								set @email_text = @email_text + (select rtrim(ltrim(zip)) from billing_account where id=@billing_account_id)+ '<br/>'
							end

						set @email_text = @email_text + '<br/>'
						set @email_text = @email_text + 'THANK YOU FOR YOUR BUSINESS !'
						set @email_text = @email_text + '<br/>'
						set @email_text = @email_text + 'Regards,<br/>'
						set @email_text = @email_text + 'VETS CHOICE RADIOLOGY'

						set @email_log_id= newid()

						insert into vrslogdb..email_log(email_log_id,email_log_datetime,recipient_address,recipient_name,cc_address,
								                email_subject,email_text,email_type,sender_email_address,sender_email_password)
										values(@email_log_id,getdate(),@user_email_id,@receipient_name,@OPMAILCC,
								                @email_subject,@email_text,'OPSCSS',@SENDMAILID,@SENDMAILPWD)

						if(@@rowcount=0)
							begin
								set @user_email_missing='X'
							end

								
					end
				else
					begin
						set @user_email_missing='Y'
					end
					
			end
		else
			begin
				set @user_email_missing='N'
			end

		/***********ADDED BY PAVEL ON 08 May 2020******************/
		/***********CREATE EMAIL NOTIFICATION FOR SUCCESSFUL ONLINE PAYMENT******************/

		exec common_lock_record_ui
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @user_id,
		@error_code    = @error_code output,
		@return_status = @return_status output

		if(@return_status=0)
			begin
				rollback transaction
				return 0
			end

		commit transaction

		set @return_status=1
		if(@user_email_missing='N') set @error_code='306'
		else if(@user_email_missing='Y') set @error_code='305'
		else if(@user_email_missing='X') set @error_code='307'
		set nocount off

		return 1
	end
GO
