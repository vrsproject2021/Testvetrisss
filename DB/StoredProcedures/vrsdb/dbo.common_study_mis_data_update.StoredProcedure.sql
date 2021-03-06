USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_study_mis_data_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_study_mis_data_update]
GO
/****** Object:  StoredProcedure [dbo].[common_study_mis_data_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_study_mis_data_update : update mis data
** Created By   : Pavel Guha
** Created On   : 28/06/2021
*******************************************************/
CREATE procedure [dbo].[common_study_mis_data_update]
    @id uniqueidentifier,
	@updated_by uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin

	  if(select count(id) from  vrsmisdb..studies where id=@id)=0
		begin
			  insert into vrsmisdb..studies(id,study_uid,modality_id,category_id,institution_id,priority_id,synched_on,study_date,
                                            patient_name,species_id,breed_id,patient_dob,patient_age,patient_sex,image_count,object_count,file_archive_count,
					                        assigned_radiologist_id,assigned_radiologist_on,dict_radiologist_id,prelim_radiologist_id,final_radiologist_id,invoiced,
					                        mis_updated_by,mis_updated_on)
									(select id,study_uid,modality_id,category_id,institution_id,priority_id,synched_on,study_date,
											patient_name,species_id,breed_id,patient_dob_accepted,patient_age_accepted,patient_sex,img_count,object_count,archive_file_count,
											radiologist_id,rad_assigned_on,dict_radiologist_id,prelim_radiologist_id,final_radiologist_id,invoiced,
											@updated_by,getdate()
									 from study_hdr
									 where id= @id)

			  if(@@rowcount=0)
				begin
					select @error_code='492',@return_status=0
					return 0
				end
							  
			  update vrsmisdb..studies set submitted_on = isnull((select max(date_updated) from sys_case_study_status_log where study_id = @id and status_id_to=10),synched_on) where synched_on between '01jan2021 00:00:00' and '31dec2021 23:59:59:59'

			  if(@@rowcount=0)
				begin
					select @error_code='492',@return_status=0
					return 0
				end

			  update vrsmisdb..studies set report_dictated_on = isnull((select max(date_created) from vrsdb..study_hdr_dictated_reports where study_hdr_id = @id),'01jan1900') where id = @id

			  if(@@rowcount=0)
				begin
					select @error_code='492',@return_status=0
					return 0
				end

			  update vrsmisdb..studies set report_prelim_on = isnull((select max(date_created) from vrsdb..study_hdr_prelim_reports where study_hdr_id = @id),'01jan1900') where id = @id

			  if(@@rowcount=0)
				begin
					select @error_code='492',@return_status=0
					return 0
				end

              update vrsmisdb..studies set report_final_on = isnull((select max(date_created) from vrsdb..study_hdr_final_reports where study_hdr_id = @id),'01jan1900') where id = @id

			  if(@@rowcount=0)
				begin
					select @error_code='492',@return_status=0
					return 0
				end

				update vrsmisdb..studies set status_last_updated_on = isnull((select status_last_updated_on from study_hdr where id = @id),'01jan1900') where id = @id

				if(@@rowcount=0)
					begin
						select @error_code='492',@return_status=0
						return 0
					end

		end
	else
		begin
			if(select count(id) from study_hdr where id = @id)>0
				begin
					update vrsmisdb..studies set report_addendum_count = isnull((select count(addendum_srl) from study_report_addendums where study_hdr_id = @id),0) where id = @id

					if(@@rowcount=0)
						begin
							select @error_code='492',@return_status=0
							return 0
						end

					update vrsmisdb..studies set report_addendum_last_added_on = isnull((select max(date_created) from study_report_addendums where study_hdr_id = @id),'01jan1900') where id = @id

					if(@@rowcount=0)
						begin
							select @error_code='492',@return_status=0
							return 0
						end

					update vrsmisdb..studies set status_last_updated_on = isnull((select status_last_updated_on from study_hdr where id = @id),'01jan1900') where id = @id

					if(@@rowcount=0)
						begin
							select @error_code='492',@return_status=0
							return 0
						end
				end
			else
				begin
					update vrsmisdb..studies set report_addendum_count = isnull((select count(addendum_srl) from study_report_addendums_archive where study_hdr_id = @id),0) where id = @id

					if(@@rowcount=0)
						begin
							select @error_code='492',@return_status=0
							return 0
						end

					update vrsmisdb..studies set report_addendum_last_added_on = isnull((select max(date_created) from study_report_addendums_archive where study_hdr_id = @id),'01jan1900') where id = @id

					if(@@rowcount=0)
						begin
							select @error_code='492',@return_status=0
							return 0
						end

					update vrsmisdb..studies set status_last_updated_on = isnull((select status_last_updated_on from study_hdr_archive where id = @id),'01jan1900') where id = @id

					if(@@rowcount=0)
						begin
							select @error_code='492',@return_status=0
							return 0
						end
				end
			
		end

		

	select @return_status=1
	return 1
end

GO
