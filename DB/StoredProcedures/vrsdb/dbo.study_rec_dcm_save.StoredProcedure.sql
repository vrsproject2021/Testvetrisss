USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_dcm_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[study_rec_dcm_save]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_dcm_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : study_rec_dcm_save : update study received 
                  record
** Created By   : Pavel Guha
** Created On   : 06/08/2019
*******************************************************/
-- exec study_rec_dcm_save '8372ee06-b7bd-4d4b-ab6a-0128f742b108'
create PROCEDURE [dbo].[study_rec_dcm_save] 
	@id uniqueidentifier,
    @study_uid nvarchar(100),
	@study_date datetime,
	@patient_id nvarchar(20),
	@patient_fname nvarchar(80),
	@patient_lname nvarchar(80),
	@approve_for_pacs nchar(1),
	@updated_by uniqueidentifier,
	@menu_id int,
	@user_name nvarchar(500)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on


	if(select count(study_uid) from	scheduler_file_downloads where study_uid = @study_uid and id=@id)>0
		begin
				begin transaction	


				exec common_check_record_lock_ui
					@menu_id       = @menu_id,
					@record_id     = @id,
					@user_id       = @updated_by,
					@user_name     = @user_name output,
					@error_code    = @error_code output,
					@return_status = @return_status output

				if(@return_status=0)
					begin
						rollback transaction
						return 0
					end


				update scheduler_file_downloads
				set     patient_id           = @patient_id,
						patient_fname        = @patient_fname,
						patient_lname        = @patient_lname,
						study_date           = @study_date,
						updated_by           = @updated_by,
						date_updated         = getdate()
				where study_uid = @study_uid 
				and id = @id

				if(@@rowcount = 0)
					begin
						rollback transaction
						select @return_status = 0,@error_code ='035'
						return 0
					end


				if(@approve_for_pacs='Y')
					begin
						update scheduler_file_downloads
						set    approve_for_pacs = 'Y',
							   approved_by      = @updated_by,
							  date_approved     = getdate()
						where study_uid = @study_uid 
						and id = @id

						if(@@rowcount = 0)
							begin
								rollback transaction
								select @return_status = 0,@error_code ='035'
								return 0
							end
						else
							select @error_code='177'

					end
				else
					select @error_code='022'

				

				commit transaction
				select @return_status=1
				set nocount off
				return 1
		end
	else
		begin
			select @error_code='169',@return_status=0
			set nocount off
			return 0
		end
	
end

GO
