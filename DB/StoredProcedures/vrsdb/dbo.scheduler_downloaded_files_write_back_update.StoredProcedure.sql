USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_files_write_back_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_downloaded_files_write_back_update]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_downloaded_files_write_back_update]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_downloaded_files_write_back_update : update
                  case study status
** Created By   : Pavel Guha
** Created On   : 15/04/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_downloaded_files_write_back_update]
    @study_id uniqueidentifier,
	@study_uid nvarchar(100),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	declare @study_date datetime,
			@received_date datetime,
			@accession_no nvarchar(20),
			@reason nvarchar(500),
			@institution_name nvarchar(100),
			@manufacturer_name nvarchar(100),
			@device_serial_no nvarchar(20),
			@referring_physician nvarchar(200),
			@patient_id nvarchar(20),
			@patient_name nvarchar(100),
			@patient_sex nvarchar(10),
			@patient_dob datetime,
			@patient_age varchar(50),
			@modality nvarchar(50),
			@manufacturer_model_no nvarchar(100),
			@img_count int,
			@modality_ae_title nvarchar(50),
			@priority_id int

	set nocount on

	begin transaction

	update scheduler_file_downloads
	set write_back_status  = 'Y'
	where id      = @study_id
	and study_uid = @study_uid

	
	if(@@rowcount=0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='Failed to update write back status of Study UID ' + @study_uid
			return 0
		end

	select  @study_date           = study_date,
			@received_date        = received_date,
			@accession_no         = accession_no,
			@reason               = reason,
			@institution_name     = institution_name,
			@manufacturer_name    = manufacturer_name,
			@device_serial_no     = device_serial_no,
			@referring_physician  = referring_physician,
			@patient_id           = patient_id,
			@patient_name         = rtrim(ltrim(isnull(patient_lname,'') + ' ' + isnull(patient_fname,''))),
			@patient_sex          = patient_sex,
			@patient_dob          = patient_dob,
			@patient_age          = patient_age,
			@modality             = modality,
			@manufacturer_model_no= manufacturer_model_no,
			@img_count            = file_count,
			@modality_ae_title    = modality_ae_title,
			@priority_id          = priority_id
	from scheduler_file_downloads
	where id      = @study_id
	and study_uid = @study_uid


	exec scheduler_new_data_synch_save
		@study_uid             = @study_uid,
		@study_date            = @study_date,
		@received_date         = @received_date,
		@accession_no          = @accession_no,
		@reason                = @reason,
		@institution_name      = @institution_name,
		@manufacturer_name     = @manufacturer_name,
		@device_serial_no      = @device_serial_no,
		@referring_physician   = @referring_physician,
		@patient_id            = @patient_id,
		@patient_name          = @patient_name,
		@patient_sex           = @patient_sex,
		@patient_dob           = @patient_dob,
		@patient_age           = @patient_age,
		@patient_weight        = 0,
		@owner_name            = '',
		@species               = '',
		@breed                 = '',
		@modality              = @modality,
		@body_part             = '',
		@manufacturer_model_no = @manufacturer_model_no,
		@sex_neutered          = '',
		@img_count             = @img_count,
		@study_desc            = '',
		@modality_ae_title     = @modality_ae_title,
		@priority_id           = @priority_id,
		@error_msg             = @error_msg output,
		@return_type           = @return_type output

	if(@return_type = 0)
		begin
			rollback transaction
			select @return_type=0,@error_msg='Failed to create Study UID ' + @study_uid
			return 0
		end
	else
		begin
			update scheduler_file_downloads
			set req_action_created='Y'
			where id      = @study_id
			and study_uid = @study_uid

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update require action creation status of Study UID ' + @study_uid
					return 0
				end
		end


	commit transaction
	select @return_type=1,@error_msg='Write back status of Study UID ' + @study_uid + ' updated'

	set nocount off
	return 1

end


GO
