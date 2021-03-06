USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_charges_discount_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_charges_discount_save]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_charges_discount_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_charges_discount_save : save 
                  discount percentage billing account wise
** Created By   : BK
** Created On   : 15/11/2019
*******************************************************/
create procedure [dbo].[invoicing_charges_discount_save]
(
	@xml_data		ntext,
    @updated_by		uniqueidentifier,
    @menu_id		int,
    @user_name		nvarchar(500)		= '' output,
    @error_code		nvarchar(10)		= '' output,
    @return_status	int					= 0 output
)
as
	begin
		set nocount on 
	
		declare @hDoc int,
				@counter bigint,
				@rowcount bigint,
				@billing_account_id uniqueidentifier,
				@charges_discount decimal(5,2),
				@old_charges_discount decimal(5,2)

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

		begin transaction
		exec sp_xml_preparedocument @hDoc output,@xml_data 

		set @counter = 1
		select  @rowcount=count(row_id)  
		from openxml(@hDoc,'chargesdiscount/row', 2)  
		with( row_id bigint )

		while(@counter <= @rowcount)
			begin
				select  @billing_account_id = billing_account_id,
						@charges_discount   = charges_discount
				from openxml(@hDoc,'chargesdiscount/row',2)
				with
				( 
					billing_account_id uniqueidentifier,
					charges_discount decimal(5,2),
					row_id bigint
				) xmlTemp where xmlTemp.row_id = @counter 
				
				-----
				select @old_charges_discount = discount_perc from invoicing_charges_discount where billing_account_id=@billing_account_id
				if(@billing_account_id <> '00000000-0000-0000-0000-000000000000' and @old_charges_discount <> @charges_discount)
					begin
						update invoicing_charges_discount
						set    discount_perc	= @charges_discount,
							   updated_by		= @updated_by,
							   date_updated		= getdate()
						where billing_account_id=@billing_account_id
				    
						if(@@rowcount=0)
							begin
								rollback transaction
								exec sp_xml_removedocument @hDoc
								select @error_code='066',@return_status=0,@user_name=''
								return 0
							end
						
					end
				
				
				----- 
			
		
				set @counter = @counter + 1
			end

		
		commit transaction
		exec sp_xml_removedocument @hDoc
		select @error_code='034',@return_status=1
		set nocount off
		return 1
	
	end
GO
