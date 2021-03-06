USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_session_file_missing_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_session_file_missing_check]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_session_file_missing_check]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_session_file_missing_check : 
                  check missing session file
** Created By   : Pavel Guha
** Created On   : 23/01/2020
*******************************************************/
--exec scheduler_session_file_missing_check 'MS1D021121093456RQX','00979_MS1D021121093456RQX_Wholistic_Veterinary_Care_8X45q9_44212-20210210184435178-original.dcm','D','N','','','','N'
CREATE procedure [dbo].[scheduler_session_file_missing_check]
    @session_id nvarchar(20),
	@file_name nvarchar(250),
	@file_type nchar(1),
	@is_missing nchar(1)='N' output,
	@institution_code nvarchar(5)='' output,
	@institution_name nvarchar(100)='' output,
	@study_uid nvarchar(100)='' output,
	@sent_to_pacs nchar(1) ='N' output
as
begin
	
	set nocount on

	declare @id uniqueidentifier,
	        @is_manual nchar(1)

	if(select count(file_name) from scheduler_checked_missing_session_files where file_name=@file_name)>0
		begin
			set @is_missing='N'

			   select @study_uid= study_uid,
					  @institution_code = institution_code,
					  @institution_name = institution_name,
					  @sent_to_pacs = sent_to_pacs
			 from scheduler_checked_missing_session_files 
			 where file_name=@file_name

			 return 1
		end

    if(substring(@session_id,1,4)='MS1D') set @is_manual='Y' else set @is_manual='N'
		

	if(@file_type ='D')
		begin
			
			if(select count(file_name) from scheduler_file_downloads_dtls where file_name=@file_name and isnull(import_session_id,'')=@session_id)=0
				begin
					set @is_missing='Y'

					select @study_uid= '',
							@institution_code = '',
							@institution_name = '',
							@sent_to_pacs = 'N'
				end
			else
				begin
					set @is_missing='N'

					select @study_uid= fdd.study_uid,
							@institution_code = fd.institution_code,
							@institution_name = fd.institution_name,
							@sent_to_pacs = fdd.sent_to_pacs
					from scheduler_file_downloads_dtls fdd
					inner join scheduler_file_downloads fd on fd.id = fdd.id
					where fdd.file_name=@file_name
					and isnull(fdd.import_session_id,'')=@session_id
				end
			
		end
	else if(@file_type ='I')
		begin
			if(@is_manual='N')
				begin
					if(select count(id) from scheduler_img_file_downloads_ungrouped where isnull(import_session_id,'')=@session_id)>0
						begin
							if(select count(file_name) from scheduler_img_file_downloads_ungrouped where file_name=@file_name and isnull(import_session_id,'')=@session_id)=0
								begin
									set @is_missing='Y'

									select  @study_uid= '',
											@institution_code = '',
											@institution_name = '',
											@sent_to_pacs = 'N'
								end
							else
								begin
									set @is_missing='N'
							
									select @study_uid= fdgd.study_uid,
										   @institution_code = fdg.institution_code,
										   @institution_name = fdg.institution_name,
										   @sent_to_pacs = fdgd.sent_to_pacs
									from scheduler_img_file_downloads_grouped_dtls fdgd
									inner join scheduler_img_file_downloads_grouped fdg on fdg.id = fdgd.id
									where fdgd.file_name=@file_name
									and isnull(fdgd.import_session_id,'')=@session_id
								end
						end
					else
						begin
							set @is_missing='Y'

							select  @study_uid= '',
							        @institution_code = '',
							        @institution_name = '',
							        @sent_to_pacs = 'N'
						end
				end
			else
				begin
					if(select count(file_name) from scheduler_img_file_downloads_grouped_dtls where file_name=@file_name and isnull(import_session_id,'')=@session_id)=0
						begin
							set @is_missing='Y'

							select  @study_uid= '',
									@institution_code = '',
									@institution_name = '',
									@sent_to_pacs = 'N'
						end
					else
						begin
							set @is_missing='N'
							
							select  @study_uid= fdgd.study_uid,
									@institution_code = fdg.institution_code,
									@institution_name = fdg.institution_name,
									@sent_to_pacs = fdgd.sent_to_pacs
							from scheduler_img_file_downloads_grouped_dtls fdgd
							inner join scheduler_img_file_downloads_grouped fdg on fdg.id = fdgd.id
							where fdgd.file_name=@file_name
							and isnull(fdgd.import_session_id,'')=@session_id
						end
				end
		end

	 insert into scheduler_checked_missing_session_files(file_name,institution_code,institution_name,study_uid,sent_to_pacs,date_checked)
	                                              values(@file_name,@institution_code,@institution_name,@study_uid,@sent_to_pacs,getdate())


	--print @is_missing
	--print @institution_code
	--print @institution_name
	--print @sent_to_pacs
	--print @study_uid
	set nocount off


end


GO
