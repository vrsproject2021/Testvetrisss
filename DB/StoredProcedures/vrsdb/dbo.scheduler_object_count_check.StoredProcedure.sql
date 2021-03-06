USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_object_count_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_object_count_check]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_object_count_check]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_object_count_check : 
                  check and rectify the 
** Created By   : Pavel Guha
** Created On   : 14/10/2020
*******************************************************/
--exec scheduler_object_count_check
CREATE procedure [dbo].[scheduler_object_count_check]
   
as
begin
	
	set nocount on

	declare @rowcount int,
	        @counter int,
			@study_uid nvarchar(100),
			@id uniqueidentifier,
			@archived nchar(1),
			@object_count int,
			@object_count_pacs int,
			@file_count int


	create table #tmp
	(
	  rec_id int identity(1,1),
	  id uniqueidentifier,
	  study_uid nvarchar(100),
	  modality_id int,
	  track_by nchar(1),
	  img_count int,
	  object_count int,
	  object_count_pacs int,
	  study_status_pacs int,
	  archive nchar(1)
	)


	insert into #tmp(id,study_uid,modality_id,track_by,img_count,object_count,object_count_pacs,study_status_pacs,archive)
	              (select sh.id,sh.study_uid,sh.modality_id,m.track_by,sh.img_count,sh.object_count,sh.object_count_pacs,sh.study_status_pacs,'N'
				   from study_hdr sh
				   inner join modality m on m.id = sh.modality_id
				   where sh.study_status_pacs>0
				   and sh.object_count <= sh.object_count_pacs
				   and (sh.object_count_pacs - sh.object_count)>3
				   and m.track_by='O')
	insert into #tmp(id,study_uid,modality_id,track_by,img_count,object_count,object_count_pacs,study_status_pacs,archive)
	              (select sh.id,sh.study_uid,sh.modality_id,m.track_by,sh.img_count,sh.object_count,sh.object_count_pacs,sh.study_status_pacs,'N'
				   from study_hdr sh
				   inner join modality m on m.id = sh.modality_id
				   where sh.study_status_pacs>0
				   and sh.object_count < sh.img_count
				   and m.track_by='I')

	select @rowcount = count(rec_id),@counter=1 from #tmp

	--select * from #tmp

	while(@counter <= @rowcount)
		begin
			select @id                = id,
				   @study_uid         = study_uid,
			       @object_count      = object_count,
				   @object_count_pacs = object_count_pacs,
				   @archived          = archive
			from #tmp
			where rec_id=@counter

			select @file_count = count(file_name) from scheduler_file_downloads_dtls where study_uid=@study_uid and id=@id

			select @file_count = @file_count + count(file_name) from scheduler_file_downloads_dtls where study_uid=@study_uid and id=@id

			--print 'STUDY UID ' +  convert(nvarchar(100),@study_uid)
			--print 'OBJECT COUNT ' +  convert(nvarchar,@object_count)
			--print 'OBJECT COUNT PACS ' +  convert(nvarchar,@object_count_pacs)
			--print 'FILE COUNT ' +  convert(nvarchar,@file_count)
			--print '------------------------------------------------------------------------------------------------'

			if(@file_count>0 and (@object_count_pacs - @file_count)<=3)
				begin
					begin transaction
					if(@archived='N')
						begin
							update study_hdr
							set object_count= @file_count
							where study_uid=@study_uid
							and id = @id
						end
					else if(@archived='Y')
						begin
							update study_hdr_archive
							set object_count= @file_count
							where study_uid=@study_uid
							and id = @id
						end

					if(@@rowcount = 0)
						begin
							rollback transaction
						end
					else
						begin
							commit transaction
						end

					
				end

			set @counter = @counter + 1
		end

		drop table #tmp

	set nocount off
	return 1

end


GO
