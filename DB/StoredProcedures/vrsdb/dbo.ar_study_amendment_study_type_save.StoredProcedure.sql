USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_amendment_study_type_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[ar_study_amendment_study_type_save]
GO
/****** Object:  StoredProcedure [dbo].[ar_study_amendment_study_type_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : ar_study_amendment_study_type_save : 
                  save selected study types
** Created By   : Pavel Guha 
** Created On   : 16-Mar-2021
*******************************************************/

--exec ar_study_amendment_study_type_fetch 'cf84156f-8f53-4906-9a0d-0fcb65393225',3
create procedure [dbo].[ar_study_amendment_study_type_save]
    @billing_cycle_id uniqueidentifier,
	@id uniqueidentifier,
	@TVP_studytypes as case_study_study_type readonly,
	@menu_id          int,
    @updated_by       uniqueidentifier,
	@user_name nvarchar(130)='' output,
    @error_code nvarchar(10)='' output,
    @return_status int =0 output
as
	begin
		set nocount on

		declare @study_type_id uniqueidentifier,
	            @srl_no int,
			    @field_code nvarchar(5),
			    @file_count int,
				@rowcount int,
				@counter int,
				@rc int
		
		 exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @billing_cycle_id,
				@user_id       = @updated_by,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
		if(@return_status=0)
			begin
				return 0
			end

		begin transaction

		create table #tmpWBT
		(
			srl_no int identity(1,1),
			field_code nvarchar(5)
		)

		insert into #tmpWBT(field_code) (select field_code from sys_pacs_query_fields where service_id=2 and display_index in (18,19,20,21))
		select @rc= @@rowcount

		if(Select count(id) from study_hdr where id=@id)>0
			begin
				delete from study_hdr_study_types  where study_hdr_id=@id
			end
		else
			begin
				delete from study_hdr_study_types_archive  where study_hdr_id=@id
			end

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

						if(Select count(id) from study_hdr where id=@id)>0
							begin
								insert into study_hdr_study_types(study_hdr_id,study_type_id,srl_no,updated_by,date_updated)
															values (@id,@study_type_id,@srl_no,@updated_by,getdate())
							end
						else
							begin
								insert into study_hdr_study_types_archive(study_hdr_id,study_type_id,srl_no,updated_by,date_updated)
																    values (@id,@study_type_id,@srl_no,@updated_by,getdate())
							end

						if(@@rowcount=0)
							begin
								rollback transaction
								select @error_code='061',@return_status=0
								return 0
							end
					

						if(@rc>0)
							begin
								select @field_code = field_code from #tmpWBT where srl_no = @counter

								if(isnull(@field_code,'')<>'')
									begin
										if(Select count(id) from study_hdr where id=@id)>0
											begin
												update study_hdr_study_types
												set write_back_tag = @field_code
												where study_hdr_id=@id
												and study_type_id = @study_type_id
											end
										else
											begin
												update study_hdr_study_types_archive
												set write_back_tag = @field_code
												where study_hdr_id=@id
												and study_type_id = @study_type_id
											end

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

				if(Select count(id) from study_hdr where id=@id)>0
					begin
						update study_hdr set pacs_wb='Y' where id=@id
					end
				else
					begin
						update study_hdr_archive set pacs_wb='Y' where id=@id
					end
			end

		drop table #tmpWBT

		commit transaction
	    set @return_status=1
	    set @error_code='034'

		set nocount off
	end

	
GO
