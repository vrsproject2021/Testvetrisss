USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_control_params_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[invoicing_control_params_save]
GO
/****** Object:  StoredProcedure [dbo].[invoicing_control_params_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : invoicing_control_params_save : save 
                  invoicing control parameters
** Created By   : BK
** Created On   : 11/11/2019
*******************************************************/
CREATE procedure [dbo].[invoicing_control_params_save]
(
	@xml_params				ntext,
	@user_id				uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(500)		= '' output,
	@error_code				nvarchar(10)		= '' output,
    @return_status			int					= 0  output
)
as
	begin
		declare @hDoc int,
				@counter int,
	            @rowcount int

		declare @contro_code		nvarchar(20)	= '',
				@data_value_char	nvarchar(2000)	= '',
				@data_value_int		int				= 0,
				@data_value_desc	decimal(12,2)	= 0,
				@value_type			nvarchar(5)		= '',
				@ui_prefix			nvarchar(5)
		begin transaction
		if(@xml_params is not null) exec sp_xml_preparedocument @hDoc output,@xml_params 
		

		
	if(@xml_params is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc,'params/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select	@contro_code		= control_code,
							@data_value_char	= data_value_char,
							@data_value_int		= data_value_int,
							@data_value_desc	= data_value_dec,
							@value_type			= value_type,
							@ui_prefix			= ui_prefix

					from openxml(@hDoc,'params/row',2)
					with
					(
						control_code	nvarchar(20),
						data_value_char	nvarchar(2000),
						data_value_int	int,
						data_value_dec	decimal(12,2),
						value_type		nvarchar(5),
						ui_prefix		nvarchar(5),
						row_id			bigint

					) xmlTemp where xmlTemp.row_id = @counter  
					
					if((select count(*) from invoicing_control_params where control_code=@contro_code)>0)
						begin
							
							exec common_check_record_lock
								@menu_id       = @menu_id,
								@record_id     = @menu_id,
								@user_id       = @user_id,
								@user_name     = @user_name output,
								@error_code    = @error_code output,
								@return_status = @return_status output
		
								if(@return_status=0)
									begin
										rollback transaction
										return 0
									end	

							update invoicing_control_params
									set 
										data_value_char	= @data_value_char,	
										data_value_int	= @data_value_int,
										data_value_dec	= @data_value_desc,
										value_type		= @value_type,
										ui_prefix		= @ui_prefix
							where control_code	= @contro_code

						end
					else
						begin
							insert into invoicing_control_params(control_code,data_value_char,data_value_int,data_value_dec,value_type,ui_prefix)
											 values(@contro_code,@data_value_char,@data_value_int,@data_value_desc,@value_type,@ui_prefix)  
						end

					
				
					
					if(@@rowcount=0)
						begin
							rollback transaction
							if(@xml_params is not null) exec sp_xml_removedocument @hDoc
							select @error_code='066',@return_status=0,@user_name=''
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
