USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_online_status_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dicom_router_online_status_update]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_online_status_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : dicom_router_online_status_update : update
                  dicom router's online status 
** Created By   : Pavel Guha
** Created On   : 22/01/2020
*******************************************************/
CREATE procedure [dbo].[dicom_router_online_status_update]
	@institution_code nvarchar(5),
	@version_no nvarchar(50),
	@output_msg nvarchar(100)='' output,
	@return_status int = 0 output
as
begin
	set nocount on

	declare @institution_id uniqueidentifier


	if(isnull(rtrim(ltrim(@institution_code)),'')<>'')
		begin
				begin transaction
				select @institution_id = id from institutions where code = @institution_code

				if(select count(institution_id) from sys_dicom_router_online_status where institution_id = @institution_id)=0
					begin
						insert into sys_dicom_router_online_status(institution_id,version_no,last_updated_on)
															values(@institution_id,@version_no,getdate())
					end
				else
					begin
						update sys_dicom_router_online_status
						set version_no = @version_no,
							last_updated_on = getdate()
					    where institution_id = @institution_id

					end

				if(@@rowcount =0)
					begin
						rollback transaction
						select @output_msg='FAILURE', @return_status=0
					end

				if(select dcm_file_xfer_pacs_mode from institutions where id = @institution_id) <> 'M'
					begin
						update institutions
						set dcm_file_xfer_pacs_mode='M'
						where id = @institution_id
					end

			    commit transaction
		end

	
	select @output_msg='SUCCESS', @return_status=1

	set nocount off
end
GO
