USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[rate_fee_schedule_template_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[rate_fee_schedule_template_save]
GO
/****** Object:  StoredProcedure [dbo].[rate_fee_schedule_template_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : rate_fee_schedule_template_save : save 
                  rate fee schedule template
** Created By   : Pavel Guha
** Created On   : 24/06/2019
*******************************************************/
CREATE procedure [dbo].[rate_fee_schedule_template_save]
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
			@head_id int,
			@head_type nchar(1),
			@modality nvarchar(50),
			@img_count_from int,
			@img_count_to int,
			@fee_amount money,
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
					select  @id             = id,
							@head_id        = head_id,
							@head_type      = head_type,
							@img_count_from = img_count_from,
							@img_count_to   = img_count_to,
							@fee_amount     = fee_amount
					from openxml(@hDoc,'fees/row',2)
					with
					( 
						id uniqueidentifier,
						head_id int,
						head_type nchar(1),
						img_count_from int,
						fee_amount money,
						img_count_to int,
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  
			
					if(@id = '00000000-0000-0000-0000-000000000000')
						begin
								set @id=newid()
								insert into rates_fee_schedule_template(id,head_id,head_type,img_count_from,img_count_to,fee_amount,created_by,date_created)
								                                  values(@id,@head_id,@head_type,@img_count_from,@img_count_to,@fee_amount,@updated_by,getdate())
						end
					else
						begin
							update rates_fee_schedule_template
							set    head_id          = @head_id,
								   head_type        = @head_type,
								   img_count_from   = @img_count_from,
								   img_count_to     = @img_count_to,
								   fee_amount       = @fee_amount,
								   updated_by       = @updated_by,
								   date_updated      = getdate()
							where id=@id 
				    
							if(@@rowcount=0)
								begin
									rollback transaction
									exec sp_xml_removedocument @hDoc
									select @error_code='128',@return_status=0,@user_name=convert(varchar,@row_id)
									return 0
								end
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
