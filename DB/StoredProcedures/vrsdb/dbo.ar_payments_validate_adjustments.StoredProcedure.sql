USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_validate_adjustments]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_payments_validate_adjustments]
GO
/****** Object:  StoredProcedure [dbo].[ar_payments_validate_adjustments]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_payments_validate_adjustments : check
** Created By   : KC
** Created On   : 14/05/2021
*******************************************************/
CREATE procedure [dbo].[ar_payments_validate_adjustments]
(
	@billing_account_id		uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@xml_adjustments        ntext = NULL,
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
		
		if(ISNULL(@billing_account_id,'00000000-0000-0000-0000-000000000000') = '00000000-0000-0000-0000-000000000000')
			begin
				select @billing_account_id=id
				from billing_account where login_user_id = @user_id;
			end



	
		--adjustments 

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
		
		select @rowcount=count(1) from @adjustments;

		select @counter=count(1) from [invoice_hdr] h with (nolock)  
			inner join @adjustments a on h.id=a.invoice_header_id and h.invoice_no=a.invoice_no
			where h.billing_account_id=@billing_account_id

		if(@rowcount=@counter and @rowcount>0)
		begin
			set @return_status=1
			return 0;
		end

		set @return_status=0
		set @error_code='480';
		return 1
	end
GO
