USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_broadcast_log_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_broadcast_log_save]
GO
/****** Object:  StoredProcedure [dbo].[hk_broadcast_log_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_broadcast_log_save : save email log
                  \sms log
** Created By   :BK
** Created On   : 02/08/2019
*******************************************************/
CREATE procedure [dbo].[hk_broadcast_log_save]
    @xml_data ntext,
	@email_subject nvarchar(100),
	@email_body ntext,
	@sms_text nvarchar(160),
	@broadcast_flag char(1) = 'E',
	
	@user_id uniqueidentifier,
    @menu_id int,
    @user_name nvarchar(500) = '' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on 
	
	declare @hDoc int,
		    @counter bigint,
	        @rowcount bigint,
	        @id uniqueidentifier,
			@name nvarchar(250),
			@email_id nvarchar(500),
			@mobile nvarchar(100),
			@sender_no nvarchar(20)

	declare @MAILSVRUSRCODE nvarchar(100),
            @MAILSVRUSRPWD nvarchar(100)

    exec common_check_record_lock
		@menu_id       = @menu_id,
		@record_id     = @menu_id,
		@user_id       = @user_id,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output
		
	if(@return_status=0)
		begin
			return 0
		end
	
   select @MAILSVRUSRCODE = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRCODE'

   select @MAILSVRUSRPWD = data_type_string
   from general_settings
   where control_code ='MAILSVRUSRPWD'
	
	begin transaction
	exec sp_xml_preparedocument @hDoc output,@xml_data 
	
	select @sender_no = data_type_string from general_settings where control_code='SMSSENDERNO'
	
	set @counter = 1
	select  @rowcount=count(row_id)  
	from openxml(@hDoc,'data/row', 2)  
	with( row_id bigint )
	
	while(@counter <= @rowcount)
		begin
			select  @id			= id,
					@name		= name,
					@email_id	= email_id,
					@mobile		= mobile

			from openxml(@hDoc,'data/row',2)
			with
			( 
				id uniqueidentifier,
				name nvarchar(250),
				email_id nvarchar(500),
				mobile nvarchar(100),
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter  
			
			
			
						
			if(@broadcast_flag = 'E')
				begin

					--if(select count(name) from business_sources where name=@name and id<>@id)>0
					--	begin
					--		rollback transaction
					--		exec sp_xml_removedocument @hDoc
					--		select @error_code='136',@return_status=0,@user_name=@name
					--		return 0
					--	end

					insert into vrslogdb..email_log(
											email_log_id,
											email_log_datetime,
											recipient_address,
											recipient_name,
											email_subject,
											email_text,
											email_type,
											sender_email_address,
											sender_email_password,
											updated_by,
											date_updated
										)
					              values(
											newid(),
											getdate(),
											@email_id,
											@name,
											@email_subject,
											@email_body,
											'BRDCST',
											@MAILSVRUSRCODE,
											@MAILSVRUSRPWD,
											@user_id,
											getdate()
										)
					                                              
					--if(@@rowcount=0)
					--	begin
					--		rollback transaction
					--		exec sp_xml_removedocument @hDoc
					--		select @error_code='066',@return_status=0,@user_name=@name
					--		return 0
					--	end
						
				
				end
			else if(@broadcast_flag = 'S')
				begin
					
				    
				    insert into vrslogdb..sms_log(
											sms_log_id,
											sms_log_datetime,
											recipient_no,
											recipient_name,
											sender_no,
											sequence_no,
											sms_text,
											updated_by,
											date_updated
										)
								  values(
											newid(),
											getdate(),
											@mobile,
											@name,
											@sender_no,
											1,
											@sms_text,
											@user_id,
											getdate()
										)
						  
				    
				  --  if(@@rowcount=0)
						--begin
						--	rollback transaction
						--	exec sp_xml_removedocument @hDoc
						--	select @error_code='066',@return_status=0,@user_name=@name
						--	return 0
						--end
						
				
				end
			
			

			set @counter = @counter + 1
		end

		
	commit transaction
	exec sp_xml_removedocument @hDoc
	select @error_code='438',@return_status=1
	set nocount off
	return 1
	
end

GO
