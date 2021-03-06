USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_img_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[study_rec_img_save]
GO
/****** Object:  StoredProcedure [dbo].[study_rec_img_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : study_rec_img_save : update study received 
                  record
** Created By   : Pavel Guha
** Created On   : 06/08/2019
*******************************************************/
-- exec study_rec_img_save '8372ee06-b7bd-4d4b-ab6a-0128f742b108'
CREATE PROCEDURE [dbo].[study_rec_img_save] 
	@id uniqueidentifier='00000000-0000-0000-0000-000000000000' output,
    @study_uid nvarchar(100),
	@study_date datetime,
	@file_count int,
	@institution_id uniqueidentifier,
	@modality_id int,
	@patient_id nvarchar(20),
	@patient_fname nvarchar(80),
	@patient_lname nvarchar(80),
	@series_instance_uid nvarchar(100),
	@series_no nvarchar(100),
	@accession_no nvarchar(20)='',
	@reason nvarchar(2000)='',
	@physician_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@patient_dob datetime=null,
	@patient_age nvarchar(50)='',
	@patient_sex nvarchar(10)='',
	@spayed_neutered nvarchar(30)='',
	@patient_weight decimal(12,3)=0,
	@wt_uom nvarchar(5)='',
	@owner_first_name nvarchar(100)='',
	@owner_last_name nvarchar(100)='',
	@species_id int=0,
	@breed_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@priority_id int = 0,
	@approve_for_pacs nchar(1),
	@physician_note nvarchar(2000)='',
	@consult_applied nchar(1)='N',
	@category_id int,
	@xml_files ntext,
	@TVP_studytypes as case_study_study_type readonly,
	@TVP_docs as case_study_doc_type readonly,
	@updated_by uniqueidentifier,
	@menu_id int,
	@studies_merged nchar(1)='X',
	@sender_time_offset_mins int=0,
	@submit_priority nchar(1)='N',
	@patient_country_id int =0,
	@patient_state_id int =0,
	@patient_city nvarchar(100)='',
	@session_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@delv_time nvarchar(130) = '' output,
	@message_display nvarchar(500) = '' output,
	@user_name nvarchar(500)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
begin
	set nocount on
	set datefirst 1

	declare @institution_code nvarchar(5),
	        @institution_name nvarchar(100),
			@modality nvarchar(10),
			@received_date datetime,
			@rc int,
			@ctr int,
			@study_hdr_id uniqueidentifier,
			@merge_status_desc nvarchar(max),
			@salesperson_id  uniqueidentifier,
			@patient_id_srl int,
			@beyond_hour_stat nchar(1),
			@is_stat nchar(1),
			@next_operation_time nvarchar(130),
			@priority_charged nchar(1),
			@error_msg nvarchar(500)

	declare @hDoc int,
		    @counter bigint,
	        @rowcount bigint,
			@ungrouped_id uniqueidentifier,
			@import_session_id nvarchar(30),
			@file_name nvarchar(250),
			@storage_applied nchar(1)

	declare  @document_id uniqueidentifier,
			 @document_link nvarchar(100),
	         @document_name nvarchar(100),
			 @document_srl_no int,
			 @document_file_type nvarchar(5),
			 @document_file varbinary(max)

	declare @study_type_id uniqueidentifier,
	        @srl_no int,
			@field_code nvarchar(5)

	declare	@beyond_operation_time nchar(1),
			@in_exp_list nchar(1)

	exec common_check_record_lock
		@menu_id       = @menu_id,
		@record_id     = @menu_id,
		@user_id       = @updated_by,
		@session_id    = @session_id,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			return 0
		end


    set @beyond_hour_stat='N'
	if(@approve_for_pacs = 'Y' and @submit_priority='N')
		begin
			 select @is_stat = is_stat from sys_priority where priority_id=@priority_id	
			 set @is_stat = isnull(@is_stat,'N')

			 set @in_exp_list='N'
			 set @beyond_operation_time ='N'
		     set @error_code =''
			 set @return_status=0

			exec common_service_availability_check
				@species_id            = @species_id,
				@modality_id           = @modality_id,
				@institution_id        = @institution_id,
				@priority_id           = @priority_id,
				@beyond_operation_time = @beyond_operation_time output,
				@in_exp_list           = @in_exp_list output,
				@error_code            = @error_code output,
				@return_status         = @return_status output

			if(@return_status=0)
				begin
					return 0
				end

		   

			exec common_check_operation_time
				@priority_id             = @priority_id,
				@sender_time_offset_mins = @sender_time_offset_mins,
				@next_operation_time     = @user_name output,
				@delv_time               = @delv_time output,
				@display_message         = @message_display output,
				@error_code              = @error_code output,
				@return_status           = @return_status output
			
					
			if(@return_status=0)
				begin
					if(@is_stat='Y' and @in_exp_list='N')
						begin
							return 0
						end
				    else
						begin
							set @submit_priority='Y'
						end
				end

		end
	--else if(@approve_for_pacs = 'Y' and @submit_priority='Y')
	--	begin
	--		----set @priority_id=@submit_priority
	--		----if(@submit_priority=10) set @beyond_hour_stat='Y'
	--		--if(@submit_priority=10) set @beyond_hour_stat='Y'
	--		--else set @beyond_hour_stat='D'

	--		if(@is_stat='Y') set @beyond_hour_stat='Y'
	--		else set @beyond_hour_stat='D'
	--	end


	select @institution_code = code,
	       @institution_name = name,
		   @patient_id_srl = patient_id_srl
	from institutions
	where id=@institution_id

	select @modality = code
	from modality
	where id=@modality_id

	select @salesperson_id = salesperson_id
	from institution_salesperson_link
	where institution_id = @institution_id
	   

	begin transaction	

	exec sp_xml_preparedocument @hDoc output,@xml_files

	set @id=newid()
	if(rtrim(ltrim(isnull(@accession_no,'')))<>'')
		begin
			if(select count(accession_no) from study_hdr where accession_no = @accession_no and study_uid <> @study_uid)>0
				begin
					set @accession_no = right(@study_uid,15)
					set @accession_no =  REPLACE(@accession_no,'.','-')
				end
		end
	else if(rtrim(ltrim(isnull(@accession_no,'')))='')
		begin
			set @accession_no = right(@study_uid,15)
			set @accession_no =  replace(@accession_no,'.','-')
		end

	set @received_date = getdate()

	 exec common_check_operation_time
		@priority_id             = @priority_id,
		@sender_time_offset_mins = @sender_time_offset_mins,
		@next_operation_time     = @next_operation_time output,
		@delv_time               = @delv_time output,
		@beyond_hour_stat        = @beyond_hour_stat output,
		@error_code              = @error_code output,
		@return_status           = @return_status output

	--if(isnull((select is_active from services where priority_id=@priority_id),'N'))='Y'
	--	begin
			set @priority_charged='Y'
	--	end
	--else
	--	begin
	--		set @priority_charged='N'
	--	end

	insert into scheduler_img_file_downloads_grouped(id,study_uid,study_date,file_count,
				                                        institution_id,institution_code,institution_name,
														patient_id,patient_fname,patient_lname,
														modality_id,category_id,modality,series_instance_uid,series_no,
														accession_no,reason,physician_id,patient_dob,patient_age,patient_sex,
														spayed_neutered,patient_weight,wt_uom,owner_first_name,owner_last_name,
														species_id,breed_id,priority_id,salesperson_id,physician_note,consult_applied,
														beyond_hour_stat,sender_time_offset_mins,priority_charged,
														patient_country_id,patient_state_id,patient_city,
														created_by,date_created)
												values(@id,@study_uid,@study_date,@file_count,
				                                        @institution_id,@institution_code,@institution_name,
														@patient_id,@patient_fname,@patient_lname,
														@modality_id,@category_id,@modality,@series_instance_uid,@series_no,
														@accession_no,@reason,@physician_id,@patient_dob,@patient_age,@patient_sex,
														@spayed_neutered,@patient_weight,@wt_uom,@owner_first_name,@owner_last_name,
														@species_id,@breed_id,@priority_id,isnull(@salesperson_id,'00000000-0000-0000-0000-000000000000'),@physician_note,@consult_applied,
														@beyond_operation_time,@sender_time_offset_mins,@priority_charged,
														@patient_country_id,@patient_state_id,@patient_city,
														@updated_by,@received_date)

	if(@@rowcount = 0)
		begin
			rollback transaction
			exec sp_xml_removedocument @hDoc
			select @return_status = 0,@error_code ='035'
			return 0
		end

	delete from scheduler_img_file_downloads_grouped_dtls where id = @id

	set @counter = 1
	set @storage_applied='N'
	select  @rowcount=count(row_id)  
	from openxml(@hDoc,'file/row', 2)  
	with( row_id bigint )

	while(@counter <= @rowcount)
		begin
			select  @ungrouped_id            = ungrouped_id,
					@file_name               = file_name
			from openxml(@hDoc,'file/row',2)
			with
			( 
				ungrouped_id uniqueidentifier,
				file_name nvarchar(250),
				row_id bigint
			) xmlTemp where xmlTemp.row_id = @counter  

			select @import_session_id = isnull(import_session_id,'')
			from scheduler_img_file_downloads_ungrouped 
			where id=@ungrouped_id

			insert into scheduler_img_file_downloads_grouped_dtls(id,ungrouped_id,study_uid,file_name,series_instance_uid,series_no,import_session_id)
													       values(@id,@ungrouped_id,@study_uid,@file_name,@series_instance_uid,@series_no,@import_session_id)
					                                              

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc
					select @error_code='066',@return_status=0,@user_name=@file_name
					return 0
				end

			if(@storage_applied='N')
				begin
					select @storage_applied= is_stored 
					from scheduler_img_file_downloads_ungrouped
					where id = @ungrouped_id
				end

			update scheduler_img_file_downloads_ungrouped
			set grouped      ='Y',
			    date_grouped =getdate(),
				grouped_id   = @id
			where id = @ungrouped_id

			if(@@rowcount=0)
				begin
					rollback transaction
					exec sp_xml_removedocument @hDoc
					select @error_code='066',@return_status=0,@user_name=@file_name
					return 0
				end

			set @counter = @counter + 1
		end

	update scheduler_img_file_downloads_ungrouped
	set grouped='N'
	where grouped_id = @id
	and id not in (select ungrouped_id
	               from scheduler_img_file_downloads_grouped_dtls
				   where id=@id)

	update scheduler_img_file_downloads_grouped
	set storage_applied = @storage_applied
	where id = @id

	--Save study types
	create table #tmpWBT
	(
		srl_no int identity(1,1),
		field_code nvarchar(5)
	)

	insert into #tmpWBT(field_code) (select field_code from sys_pacs_query_fields where service_id=2 and display_index in (18,19,20,21))

	select @rowcount= @@rowcount

	delete from scheduler_img_file_downloads_grouped_study_types  where study_hdr_id=@id

	if(select count(study_type_id) from @TVP_studytypes)>0
		begin
			
			select @rowcount = count(study_type_id),
				   @counter = 1
			from @TVP_studytypes

			while(@counter <= @rowcount)
				begin
					select @study_type_id  = study_type_id,
						   @srl_no         = srl_no
					from @TVP_studytypes
					where srl_no= @counter 

					insert into scheduler_img_file_downloads_grouped_study_types(study_hdr_id,study_type_id,srl_no,updated_by,date_updated)
												values (@id,@study_type_id,@srl_no,@updated_by,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_code='061',@return_status=0
							return 0
						end
					

					if(@rowcount>0)
						begin
							select @field_code = field_code from #tmpWBT where srl_no = @counter

							if(isnull(@field_code,'')<>'')
								begin
									update scheduler_img_file_downloads_grouped_study_types
									set write_back_tag = @field_code
									where study_hdr_id=@id
									and study_type_id = @study_type_id

									if(@@rowcount=0)
										begin
											rollback transaction
											select @error_code='062',@return_status=0
											return 0
										end
								end
						end

					set @counter = @counter + 1
			end

		end

	drop table #tmpWBT

	--save documents
	create table #tmpDocs
	(id uniqueidentifier not null,
	 document_id uniqueidentifier not null,
	 document_name nvarchar(100) not null,
	 document_srl_no int not null,
	 document_link nvarchar(100) not null,
	 document_file_type nvarchar(5) not null,
	 document_file varbinary(max) null)

	insert into #tmpDocs(id,document_id,document_name,document_srl_no,
							document_link,document_file_type,document_file)
						(select study_hdr_id,document_id,document_name,document_srl_no,
								document_link,document_file_type,document_file
							from scheduler_img_file_downloads_grouped_docs
							where study_hdr_id = @id)

	delete from scheduler_img_file_downloads_grouped_docs where study_hdr_id = @id

	if(select count(document_id) from @TVP_docs)>0
		begin
			
			select @rowcount = count(document_id),
					@counter = 1
			from @TVP_docs


			while(@counter <= @rowcount)
				begin
					select @document_id          = document_id,
							@document_name        = document_name,
							@document_srl_no      = document_srl_no,
							@document_link        = document_link,
							@document_file_type   = document_file_type,
							@document_file        = document_file
					from @TVP_docs
					where document_srl_no= @counter 

					if(@document_id = '00000000-0000-0000-0000-000000000000') set @document_id = newid() 
					
					insert into scheduler_img_file_downloads_grouped_docs(study_hdr_id,document_id,document_name,document_srl_no,
													document_link,document_file_type,document_file,created_by,date_created)
												values (@id,@document_id,@document_name,@document_srl_no,
														@document_link,@document_file_type,@document_file,@updated_by,getdate())

					if(@@rowcount=0)
						begin
							rollback transaction
							select @error_code='036',@return_status=0
							return 0
						end
					

					if(select count(document_link) from #tmpDocs where document_link = @document_link)=0
						begin
							insert into #tmpDocs(id,document_id,document_name,document_srl_no,
													document_link,document_file_type,document_file)
										values (@id,@document_id,@document_name,@document_srl_no,
												@document_link,@document_file_type,@document_file)

							if(@@rowcount=0)
								begin
									rollback transaction
									select @error_code='036',@return_status=0
									return 0
								end
						end

					set @counter = @counter + 1
			end

		end

	create table #tmpDocsDel
	(row_id int identity(1,1) not null,
	 document_id uniqueidentifier not null)

	insert into #tmpDocsDel(document_id)
	(select document_id 
	 from #tmpDocs
	 where document_id not in (select document_id from @TVP_docs where document_id <> '00000000-0000-0000-0000-000000000000'
								union select document_id from #tmpDocs))

	select @rowcount = @@rowcount,
			@counter  = 1

	while(@counter <= @rowcount)
		begin
			select @document_id          = document_id
			from #tmpDocsDel
			where row_id= @counter
					
			select  @document_link = document_link
			from scheduler_img_file_downloads_grouped_docs
			where document_id   = @document_id
			and study_hdr_id    = @id

			delete from scheduler_img_file_downloads_grouped_docs
			where document_id =@document_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_code='036',@return_status=0
					return 0
				end

			set @counter = @counter + 1
		end

	drop table #tmpDocsDel
	drop table #tmpDocs


	if(@approve_for_pacs='Y')
		begin
			update scheduler_img_file_downloads_grouped
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

			insert into study_synch_dump(study_uid,study_date,received_date,accession_no,reason,
										institution_name,manufacturer_name,manufacturer_model_no,device_serial_no,modality_ae_title,referring_physician,
										patient_id,patient_name,patient_sex,patient_dob,patient_age,patient_weight,sex_neutered,
										owner_name,species,breed,modality,body_part,img_count,study_desc,priority_id,synched_on)
									 (select fdg.study_uid,fdg.study_date,fdg.date_created,fdg.accession_no,isnull(fdg.reason,''),
									         fdg.institution_name,'','','','',ipl.physician_name,
											 fdg.patient_id,rtrim(ltrim(fdg.patient_fname + ' ' + fdg.patient_lname)),fdg.patient_sex,fdg.patient_dob,fdg.patient_age,fdg.patient_weight,fdg.spayed_neutered,
											 rtrim(ltrim(fdg.owner_first_name + ' ' + fdg.owner_last_name)),species= s.name,breed = b.name,m.code,'',fdg.file_count,'',fdg.priority_id,getdate()
									  from scheduler_img_file_downloads_grouped  fdg
									  inner join institution_physician_link ipl on ipl.physician_id = fdg.physician_id
									  inner join species s on s.id= fdg.species_id
									  inner join breed b on b.id= fdg.breed_id
									  inner join modality m on m.id=fdg.modality_id
									  where fdg.id=@id)
			
			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_status = 0,@error_code ='035'
					return 0
				end

			--if(select count(id) 
			--   from study_hdr 
			--   where upper(patient_fname) = upper(@patient_fname)
			--   and upper(patient_lname) = upper(@patient_lname)
			--   and patient_sex         = @patient_sex
			--   and institution_id      = @institution_id
			--   and study_status        = 0
			--   and convert(datetime,convert(varchar(11),received_date,106))= convert(datetime,convert(varchar(11),@received_date,106))
			--   and study_uid <> @study_uid)>0
			--	begin
			--		if(@studies_merged ='M' or @studies_merged='C')
			--			begin
			--				create table #tmpIDs
			--				(
			--					rec_id int identity(1,1),
			--					id uniqueidentifier,
			--					suid nvarchar(100),
			--					study_status_pacs int,
			--					study_status int
			--				)

			--				insert into #tmpIDs(id,suid,study_status_pacs,study_status)
			--									(select id,study_uid,study_status_pacs,study_status
			--									 from study_hdr 
			--									 where upper(patient_fname) = upper(@patient_fname)
			--									 and upper(patient_lname)   = upper(@patient_lname)
			--									 and patient_sex            = @patient_sex
			--									 and institution_id         = @institution_id
			--									  and study_status         = 0
			--									 and convert(datetime,convert(varchar(11),received_date,106))= convert(datetime,convert(varchar(11),@received_date,106))
			--									 and study_uid <> @study_uid)

			--				select @rc=max(rec_id) from #tmpIDs

			--				-- merge study types
			--				if(select count(study_hdr_id) from scheduler_img_file_downloads_grouped_study_types where study_hdr_id=@id) > 0
			--					begin
			--						delete 
			--						from study_hdr_study_types
			--						where study_hdr_id in (select id from #tmpIDs)

			--						set @ctr=1

			--						while(@ctr <= @rc)
			--							begin
			--								select @study_hdr_id = id
			--								from #tmpIDs
			--								where rec_id = @ctr

			--								insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,write_back_tag,updated_by,date_updated)
			--								(select @study_hdr_id,study_type_id,srl_no,write_back_tag,@updated_by,getdate()
			--								 from scheduler_img_file_downloads_grouped_study_types
			--								  where study_hdr_id = @id)

			--								if(@@rowcount=0)
			--									begin
			--										rollback transaction
			--										select @error_code='166',@return_status=0
			--										return 0
			--									end

			--								set @ctr = @ctr + 1
			--							end
			--					end

			--				--merge study documents
			--				if(select count(study_hdr_id) from scheduler_img_file_downloads_grouped_docs where study_hdr_id=@id) > 0
			--					begin

			--						delete 
			--						from study_hdr_documents
			--						where study_hdr_id in (select id from #tmpIDs)

			--						set @ctr=1

			--						while(@ctr <= @rc)
			--							begin
			--								select @study_hdr_id = id
			--								from #tmpIDs
			--								where rec_id = @ctr

			--								insert into study_hdr_documents(study_hdr_id,document_id,document_name,document_srl_no,document_link,document_file_type,document_file,
			--																created_by,date_created)
			--								(select @study_hdr_id,newid(),document_name,document_srl_no,document_link,document_file_type,document_file,
			--										@updated_by,getdate()
			--								from scheduler_img_file_downloads_grouped_docs
			--								where study_hdr_id = @id)

			--								if(@@rowcount=0)
			--										begin
			--											rollback transaction
			--											select @error_code='167',@return_status=0
			--											return 0
			--										end

			--								set @ctr = @ctr + 1
			--							end
			--					end

			--				--merge studies
			--				if(@studies_merged = 'M')
			--					begin
			--						set @merge_status_desc ='MERGED with Study UID : ' + @study_uid
			--					end
			--				else if(@studies_merged = 'C')
			--					begin
			--						set @merge_status_desc ='COMPARED with Study UID : ' + @study_uid
			--					end

			--				update study_hdr
			--				set     patient_id           = @patient_id,
			--						patient_name         = @patient_lname + ' ' + @patient_fname,
			--						patient_fname        = @patient_fname,
			--						patient_lname        = @patient_lname,
			--						patient_weight       = @patient_weight,
			--						patient_dob_accepted = @patient_dob,
			--						patient_age_accepted = @patient_age,
			--						patient_sex          = @patient_sex,
			--						patient_sex_neutered = @spayed_neutered,
			--						species_id           = @species_id,
			--						breed_id             = @breed_id,
			--						owner_first_name     = @owner_first_name,
			--						owner_last_name      = @owner_last_name,
			--						accession_no         = @accession_no,
			--						priority_id          = @priority_id,
			--						modality_id          = @modality_id,
			--						reason_accepted      = @reason,
			--						--img_count            = @file_count,
			--						img_count_accepted   = 'Y',
			--						institution_id       = @institution_id,
			--						physician_id         = @physician_id,
			--						salesperson_id       = isnull(@salesperson_id,'00000000-0000-0000-0000-000000000000'),
			--						wt_uom               = @wt_uom,
			--						pacs_wb              ='Y',
			--						study_status         = 2,
			--						study_status_pacs    = 50,
			--						merge_status         = @studies_merged,
			--						merge_status_desc    = @merge_status_desc,
			--						updated_by           = @updated_by,
			--						date_updated         = getdate()
			--				where id in (select id from #tmpIDs)

			--				if(@@rowcount=0)
			--					begin
			--						rollback transaction
			--						select @error_code='164',@return_status=0
			--						return 0
			--					end

			--			    insert into sys_case_study_status_log(study_id,study_uid,status_id_from,status_id_to,date_updated,updated_by)
			--									                     (select id,suid,study_status_pacs,10,getdate(),@updated_by from #tmpIDs)

			--				drop table #tmpIDs

			--			end
			--		else if(@studies_merged ='X')
			--			begin
			--				rollback transaction
			--				select @error_code='165',@return_status=0
			--				return 0
			--			end
			--	end


			if(@patient_id = @institution_code + '-' + convert(varchar,@patient_id_srl + 1))
				begin
					update institutions
					set patient_id_srl = patient_id_srl + 1
					where id = @institution_id
				end

			exec common_study_user_activity_trail_save
				@study_hdr_id  = @id,
				@study_uid     = @study_uid,
				@menu_id       = @menu_id,
				@activity_text = 'Submitted',
				@session_id    = @session_id,
				@activity_by   = @updated_by,
				@error_code    = @error_code output,
				@return_status = @return_status output

		   if(@return_status=0)
			begin
				rollback transaction
				return 0
			end

			exec notification_study_file_sync_pending_create
				@id = @id,
				@is_image ='Y',
				@error_msg = @error_msg output,
				@return_type = @return_status output

			if(@return_status=0)
				begin
					rollback transaction
					select @error_code='487',@return_status=0
					return 0
				end
		   
		    /**********Generate STAT Study submit mail notification**********/
				
			if(@priority_id=10 or @priority_id=30)
				begin
					exec notification_rule_0_create
						@id = @id,
						@error_msg = @error_msg output,
						@return_type = @return_status output
				end

            /**********Generate notification rule notifications**********/
			declare @email_count int,
				    @sms_count int

			set @email_count= 0
			set @sms_count  = 0
			exec scheduler_notification_rule_notification_create
				@email_count = @email_count output,
				@sms_count   = @sms_count output
			
		end
	

	commit transaction
	exec sp_xml_removedocument @hDoc
	select @return_status=1,@error_code='177'			
	set nocount off
	return 1
end

GO
