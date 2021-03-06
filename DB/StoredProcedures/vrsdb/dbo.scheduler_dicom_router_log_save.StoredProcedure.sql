USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_dicom_router_log_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_dicom_router_log_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_dicom_router_log_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_dicom_router_log_save : save dicom router
                  log
** Created By   : Pavel Guha
** Created On   : 25/09/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_dicom_router_log_save]
    @institution_code nvarchar(5),
	@service_id int=0,
	@service_name nvarchar(100),
	@log_date datetime,
	@log_message varchar(8000)='',
    @is_error bit=0,
	@error_msg nvarchar(100) = '' output,
	@return_type int = 0 output
as
begin
    set nocount on

	declare @institution_id uniqueidentifier,
	        @institution_name nvarchar(100)

	select @institution_id= id,
	       @institution_name = name
	from institutions
	where code = @institution_code
	
	--if(@institution_id <>'B939CBBE-B3A2-4C65-885F-F449A27A4EA9')
	--	begin
		if(@log_message <>'doUpdateOnlineStatus() - Exception: Object reference not set to an instance of an object.')
			begin
				insert into vrslogdb..sys_dicom_router_log(institution_id,institution_code,institution_name,
												 service_id,service_name,log_date,log_message,
												 is_error,date_synched)
									   values(@institution_id,@institution_code,@institution_name,
											  @service_id,@service_name,@log_date,@log_message,
											  @is_error,getdate())

				if(@@rowcount>0)
					begin
						select  @error_msg='',@return_type=1
						set nocount off	
						return 1				
					end
				else
					begin
						select  @error_msg='Failed to save the dicom router log',@return_type=0
						set nocount off
						return 0
					end
			end
		
	else
		begin
			
					    select  @error_msg='',@return_type=1
						set nocount off	
						return 1				

		end


	
end


GO
