USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_promotion_save]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_promotion_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_promotion_save : save
                  promotion details 
** Created By   : BK
** Created On   : 27/11/2019
*******************************************************/
/*
	exec invoicing_promotion_save '00000000-0000-0000-0000-000000000000','D','45b9952d-6ced-4676-bbcc-334b21e3ad70','16Dec2019','20Dec2019',
	                              'e4102102-b190-4a9b-8ba8-9dbd6b208b73','Y',
								   '<promo><row><line_no>1</line_no><id><![CDATA[00000000-0000-0000-0000-000000000000]]></id><institution_id><![CDATA[340dc26a-a9ff-4796-9bfc-7854c506f3cc]]></institution_id><modality_id>1</modality_id><free_credits>0</free_credits><discount_percent>100</discount_percent><row_id>1</row_id></row></promo>',
								   '11111111-1111-1111-1111-111111111111',47,'','',0
*/
CREATE procedure [dbo].[invoicing_promotion_save]
(
	@id uniqueidentifier = '00000000-0000-0000-0000-000000000000' output,
	@promotion_type nchar(1),
	@billing_account_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@valid_from datetime='01jan1900',
	@valid_till datetime,
	@reason_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@is_active nchar(1)='Y',
	@xml_details ntext,
	@user_id uniqueidentifier,
	@menu_id int,
	@user_name nvarchar(150)	='' output,
    @error_code nvarchar(10)	='' output,
    @return_status int			= 0 output
)
as
	begin
		set nocount on
		declare @hDoc int,
				@counter bigint,
				@rowcount bigint

		declare @line_no int,
		        @line_id uniqueidentifier,
				@institution_id uniqueidentifier,
				@modality_id int,
				@free_credits int,
				@discount_percent decimal(5,2)

		

		begin transaction
		if(@xml_details is not null) exec sp_xml_preparedocument @hDoc output,@xml_details 

		if(@id = '00000000-0000-0000-0000-000000000000')
			begin
				set @id	=newid()
				insert into ar_promotions(id,promotion_type,billing_account_id,valid_from,valid_till,reason_id,is_active,created_by,date_created)
						           values(@id,@promotion_type,@billing_account_id,@valid_from,@valid_till,@reason_id,@is_active,@user_id,getdate())

				if(@@rowcount=0)
					begin
						rollback transaction
						if(@xml_details is not null) exec sp_xml_removedocument @hDoc
						select	@return_status=0,@error_code='035'
						return 0
					end
			end
		else
			begin
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
						if(@xml_details is not null) exec sp_xml_removedocument @hDoc
						return 0
					end

				update ar_promotions
						set is_active       = @is_active,
							valid_from      = @valid_from,
							valid_till      = @valid_till,
							reason_id       = @reason_id,
							updated_by		= @user_id,
							date_updated	= getdate()
						where id = @id

				if(@@rowcount=0)
					begin
						rollback transaction
						if(@xml_details is not null) exec sp_xml_removedocument @hDoc
						select	@return_status=0,@error_code='035'
						return 0
					end

			end

		if(@xml_details is not null)
			begin
				set @counter = 1
				select  @rowcount=count(row_id)  
				from openxml(@hDoc,'promo/row', 2)  
				with( row_id bigint )

				while(@counter <= @rowcount)
					begin
						select	@line_no           = line_no,
								@line_id           = id,
								@institution_id    = institution_id,
						        @modality_id       = modality_id,
								@free_credits      = free_credits,
								@discount_percent  = discount_percent
						from openxml(@hDoc,'promo/row',2)
						with
						( 
							line_no int,
							id uniqueidentifier,
							institution_id uniqueidentifier,
							modality_id int,
							free_credits int,
							discount_percent decimal(5,2),
							row_id int
						) xmlTemp where xmlTemp.row_id = @counter  


						if(select count(arpi.id) 
						   from ar_promotion_institution arpi
						   inner join ar_promotions arp on arp.id = arpi.hdr_id
						   where arpi.institution_id = @institution_id
						   and arpi.modality_id = @modality_id
						   and arp.promotion_type ='D'
						   and arpi.id <> @line_id
						   and ((arp.valid_from between @valid_from and @valid_till)
				               or (arp.valid_till between @valid_from and @valid_till)))>0
							   begin
									rollback transaction
									if(@xml_details is not null) exec sp_xml_removedocument @hDoc
									select @error_code='262',@return_status=0,@user_name='Row #' + convert(varchar,@line_id) 
									return 0
							   end

						if(select count(arpi.id) 
						   from ar_promotion_institution arpi
						   inner join ar_promotions arp on arp.id = arpi.hdr_id
						   where arpi.institution_id = @institution_id
						   and arpi.modality_id = @modality_id
						   and arp.promotion_type ='F'
						   and arpi.id <> @line_id
						   and arp.valid_till >=@valid_till)>0
							   begin
									rollback transaction
									if(@xml_details is not null) exec sp_xml_removedocument @hDoc
									select @error_code='263',@return_status=0,@user_name=convert(varchar,@line_no) 
									return 0
							   end
							   
						if(@line_id = '00000000-0000-0000-0000-000000000000')
							begin
								set @line_id = newid()
								insert into ar_promotion_institution(id,hdr_id,line_no,billing_account_id,institution_id,modality_id,
								                                     free_credits,discount_percent,updated_by,date_updated)
															  values(@line_id,@id,@line_no,@billing_account_id,@institution_id,@modality_id,
															         @free_credits,@discount_percent,@user_id,getdate())
					                                              
								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_details is not null) exec sp_xml_removedocument @hDoc
										select @error_code='066',@return_status=0,@user_name= convert(varchar,@line_no) 
										return 0
									end

								

							end
						else
							begin
								
								update ar_promotion_institution
								set institution_id   = @institution_id,
								    modality_id      = @modality_id,
									free_credits     = @free_credits,
									discount_percent = @discount_percent
								where id =@line_id
								and hdr_id = @id

								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_details is not null) exec sp_xml_removedocument @hDoc
										select @error_code='066',@return_status=0,@user_name= convert(varchar,@line_no) 
										return 0
									end
							end
						
						set @counter = @counter + 1
					end
			        
			end
					
		commit transaction
		if(@xml_details is not null) exec sp_xml_removedocument @hDoc
		set @return_status=1
		set @error_code='034'
		set nocount off
		return 1
	end
GO
