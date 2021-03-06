USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_dcm_fetch_hdr]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[study_rec_dcm_fetch_hdr]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_dcm_fetch_hdr]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : study_rec_dcm_fetch_hdr : fetch case list header
** Created By   : Pavel Guha
** Created On   : 05/08/2019
*******************************************************/
--exec study_rec_dcm_fetch_hdr '8372EE06-B7BD-4D4B-AB6A-0128F742B108',1,'11111111-1111-1111-1111-111111111111','',0
create procedure [dbo].[study_rec_dcm_fetch_hdr]
    @id uniqueidentifier,
    @menu_id int,
    @user_id uniqueidentifier,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	 set nocount on

	select hdr.id,hdr.study_uid,hdr.study_date,hdr.date_downloaded,
	       patient_id    = isnull(hdr.patient_id,''),
		   patient_fname = isnull(hdr.patient_fname,''),
		   patient_lname = isnull(hdr.patient_lname,''),
		   institution_id = isnull(hdr.institution_id,'00000000-0000-0000-0000-000000000000'),
		   institution_code = isnull(ins.code,''),
		   institution_name= isnull(ins.name,''),
		   file_count,
		   file_xfer_count,
		   approve_for_pacs
	from scheduler_file_downloads hdr
	left outer join institutions ins on ins.id= hdr.institution_id
	where hdr.id=@id

	
	if(@id<>'00000000-0000-0000-0000-000000000000')
		begin
			
				if(select count(record_id) from sys_record_lock_ui where record_id=@id and menu_id=@menu_id)=0
					begin
						exec common_lock_record_ui
							@menu_id       = @menu_id,
							@record_id     = @id,
							@user_id       = @user_id,
							@error_code    = @error_code output,
							@return_status = @return_status output	
						
						if(@return_status=0)
							begin
								return 0
							end
					end
				
		end
    else
		begin
			if(select count(record_id) from sys_record_lock_ui where user_id=@user_id)>0
			    begin
				  delete from sys_record_lock_ui where user_id=@user_id
				  delete from sys_record_lock where user_id=@user_id
			    end
		end

		
	set nocount off
end

GO
