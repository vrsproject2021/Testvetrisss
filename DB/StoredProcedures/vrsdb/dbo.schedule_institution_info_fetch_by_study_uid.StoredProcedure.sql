USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[schedule_institution_info_fetch_by_study_uid]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[schedule_institution_info_fetch_by_study_uid]
GO
/****** Object:  StoredProcedure [dbo].[schedule_institution_info_fetch_by_study_uid]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : schedule_institution_info_fetch_by_study_uid_by_study_uid : fetch
				  institution info from study uid
** Created By   : Pavel Guha
** Created On   : 10/02/2021
*******************************************************/
-- exec schedule_institution_info_fetch_by_study_uid  'Southeast Veterinary Neurology'
CREATE procedure [dbo].[schedule_institution_info_fetch_by_study_uid]
	@study_uid nvarchar(100)
as
begin
	set nocount on
	declare @inst_id uniqueidentifier,
			@inst_code nvarchar(5),
	        @inst_name nvarchar(100),
			@study_exists nchar(1)

	set @inst_id ='00000000-0000-0000-0000-000000000000'
	set @inst_code=''
	set @inst_name=''
	set @study_exists='N'

	if(select count(id) from study_hdr where study_uid = @study_uid)>0
		begin
			set @study_exists='Y'
			select @inst_id= institution_id from study_hdr where study_uid=@study_uid

			if(isnull(@inst_id,'00000000-0000-0000-0000-000000000000')<>'00000000-0000-0000-0000-000000000000')
				begin
					select  @inst_code = code,
							@inst_name = name
					from institutions
					where id = @inst_id
				end
		end
	else if(select count(id) from study_hdr_archive where study_uid = @study_uid)>0
		begin
		   set @study_exists='Y'
		   select @inst_id= institution_id from study_hdr_archive where study_uid=@study_uid

		   if(isnull(@inst_id,'00000000-0000-0000-0000-000000000000')<>'00000000-0000-0000-0000-000000000000')
				begin
					select  @inst_code = code,
							@inst_name = name
					from institutions
					where id = @inst_id
				end
		end

	select institution_id   = @inst_id,
		   institution_code = @inst_code,
	       institution_name = @inst_name,
		   study_exists     = @study_exists

	set nocount off
end

GO
