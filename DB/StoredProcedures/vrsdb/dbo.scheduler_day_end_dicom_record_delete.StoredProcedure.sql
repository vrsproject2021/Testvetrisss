USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_dicom_record_delete]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_day_end_dicom_record_delete]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_day_end_dicom_record_delete]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_day_end_dicom_record_delete : delete
                  umgrouped image file record
** Created By   : Pavel Guha
** Created On   : 04/05/2020
*******************************************************/
CREATE procedure [dbo].[scheduler_day_end_dicom_record_delete]
    @id uniqueidentifier,
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @study_uid nvarchar(100),
	        @remarks nvarchar(250)
	
	if(select count(id) from study_hdr where id=@id)>0
		begin
			begin transaction

			set @remarks='Synched 3 days before.No action taken by institution'
			select @study_uid=study_uid from study_hdr where id = @id

			insert into study_hdr_deleted(id,study_uid,study_date,received_date,synched_on,patient_name,institution_name,remarks,deleted_on)
			(select h.id,h.study_uid,h.study_date,h.received_date,h.synched_on,h.patient_name,isnull(i.name,isnull(h.institution_name_pacs,'')),@remarks,getdate()
			 from study_hdr h
			 left outer join institutions i on i.id = h.institution_id
			 where h.study_uid =@study_uid)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg ='Error deleting study details of Study UID : ' + @study_uid,
						   @return_type =0
					return 0
				end

			if(select count(study_hdr_id) from study_hdr_documents where study_hdr_id=@id)>0
				begin
					delete from study_hdr_documents
					where study_hdr_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting documents of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
				end

			if(select count(study_id) from sys_case_study_status_log where study_id=@id)>0
			  begin
				 delete from sys_case_study_status_log where study_id = @id

				  if(@@rowcount =0)
						begin
							rollback transaction
							select @error_msg ='Error deleting status log of Study UID : ' + @study_uid,
								   @return_type =0
							return 0
						end
			  end

			 delete from study_hdr where id = @id

			 if(@@rowcount =0)
				begin
					rollback transaction
					select @error_msg ='Error deleting Study UID : ' + @study_uid,
							@return_type =0

							print @error_msg
					return 0
				end

			if(select count(study_uid) from study_synch_dump where study_uid=@study_uid)>0
				begin
					delete from study_synch_dump where study_uid=@study_uid

						if(@@rowcount =0)
							begin
							rollback transaction
							select @error_msg ='Error deleting dump of Study UID : ' + @study_uid+ ' (from dump)',
									@return_type =0
							return 0
							end
				end

			if(select count(id) from scheduler_file_downloads where study_uid = @study_uid)>0
				begin

					if(select count(id) from scheduler_file_downloads_dtls where id=@id)>0
						 begin
							 delete from scheduler_file_downloads_dtls where id = @id

							 if(@@rowcount =0)
									begin
										rollback transaction
										select @error_msg ='Error deleting study files UID : ' + @study_uid + ' (from downloads)',
											   @return_type =0
										return 0
									end
						 end
			
					if(select count(id) from scheduler_file_downloads where id=@id)>0
						 begin
							 delete from scheduler_file_downloads where id = @id

							 if(@@rowcount =0)
									begin
										rollback transaction
										select @error_msg ='Error deleting Study UID : ' + @study_uid+ ' (from downloads)',
											   @return_type =0
										return 0
									end
						 end
				end

			delete from scheduler_study_to_delete where study_id=@id
				
			commit transaction
		end
	
	select @return_type=0,@error_msg=''
	set nocount off
	return 1

end


GO
