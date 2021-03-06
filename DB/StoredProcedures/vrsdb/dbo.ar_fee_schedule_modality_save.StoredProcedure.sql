USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_fee_schedule_modality_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_fee_schedule_modality_save]
GO
/****** Object:  StoredProcedure [dbo].[ar_fee_schedule_modality_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_fee_schedule_modality_save : save 
                  rate fee schedule template
** Created By   : Pavel Guha
** Created On   : 24/02/2021
*******************************************************/
CREATE procedure [dbo].[ar_fee_schedule_modality_save]
(
	@xml_fees               ntext,
	@updated_by             uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(700) = '' output,
	@error_code				nvarchar(10)	= '' output,
    @return_status			int				= 0  output
)
as
begin
	set nocount on 
	
	declare @hDoc int,
		    @counter bigint,
	        @rowcount bigint

	declare @id uniqueidentifier,
			@category_id int,
			@modality_id int,
			@invoice_by nchar(1),
			@default_count_from int,
			@default_count_to int,
			@fee_amount money,
			@fee_amount_per_unit money,
			@study_max_amount money,
			@gl_code nvarchar(5),
			@row_id int

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
	exec sp_xml_preparedocument @hDoc output,@xml_fees 
	


	if(@xml_fees is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc,'fees/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @id                  = id,
					        @category_id         = category_id,
							@modality_id         = modality_id,
							@invoice_by          = invoice_by,
							@default_count_from  = default_count_from,
							@default_count_to    = default_count_to,
							@fee_amount          = fee_amount,
							@fee_amount_per_unit = fee_amount_per_unit,
							@study_max_amount    = study_max_amount,
							@gl_code             = gl_code,
							@row_id              = row_id
					from openxml(@hDoc,'fees/row',2)
					with
					( 
						id uniqueidentifier,
						category_id int,
						modality_id int,
						invoice_by nchar(1),
						default_count_from int,
						default_count_to int,
						fee_amount money,
						fee_amount_per_unit money,
						study_max_amount money,
						gl_code nvarchar(5),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					if(@default_count_to < @default_count_from)
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='133',@return_status=0,@user_name=convert(varchar,@row_id)
							return 0
						end

					if(select count(id) 
					   from ar_modality_fee_schedule_template 
					   where category_id =@category_id 
					   and modality_id=@modality_id 
					   and (default_count_from=@default_count_from or default_count_to = @default_count_from)
					   and deleted ='N'
					   and id<>@id)>0
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='459',@return_status=0,@user_name=convert(varchar,@row_id)
							return 0
						end

					if(select count(id) 
					   from ar_modality_fee_schedule_template 
					   where category_id =@category_id 
					   and modality_id=@modality_id 
					   and (default_count_from=@default_count_to or default_count_to = @default_count_to)
					   and deleted ='N'
					   and id<>@id)>0
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='460',@return_status=0,@user_name=convert(varchar,@row_id)
							return 0
						end
			
					if(@id = '00000000-0000-0000-0000-000000000000')
						begin
								set @id=newid()
								insert into ar_modality_fee_schedule_template(id,category_id,modality_id,invoice_by,default_count_from,default_count_to,
								                                              fee_amount,fee_amount_per_unit,study_max_amount,gl_code,created_by,date_created)
								                                       values(@id,@category_id,@modality_id,@invoice_by,@default_count_from,@default_count_to,
																	          @fee_amount,@fee_amount_per_unit,@study_max_amount,@gl_code,@updated_by,getdate())
						end
					else
						begin
							update ar_modality_fee_schedule_template
							set    category_id          = @category_id,
							       modality_id          = @modality_id,
								   invoice_by           = @invoice_by,
								   default_count_from   = @default_count_from,
								   default_count_to     = @default_count_to,
								   fee_amount           = @fee_amount,
								   fee_amount_per_unit  = @fee_amount_per_unit,
								   study_max_amount     = @study_max_amount,
								   gl_code              = @gl_code,
								   updated_by           = @updated_by,
								   date_updated         = getdate()
							where id=@id 
						end

					if(@@rowcount=0)
						begin
							rollback transaction
							exec sp_xml_removedocument @hDoc
							select @error_code='128',@return_status=0,@user_name=convert(varchar,@row_id)
							return 0
						end

					set @counter = @counter + 1
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
