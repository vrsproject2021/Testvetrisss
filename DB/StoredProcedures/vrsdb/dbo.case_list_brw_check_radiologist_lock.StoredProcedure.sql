USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[case_list_brw_check_radiologist_lock]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[case_list_brw_check_radiologist_lock]
GO
/****** Object:  StoredProcedure [dbo].[case_list_brw_check_radiologist_lock]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : case_list_brw_check_radiologist_lock : fetch case list
                  browser parameters 
** Created By   : Pavel Guha
** Created On   : 24/04/2020
*******************************************************/
--exec case_list_brw_check_radiologist_lock 'cdc32f30-af38-4ada-b047-f8182f4f584d',60,'ee8643fd-58ce-4ec8-8192-97011ac55420','N','','','',0
CREATE PROCEDURE [dbo].[case_list_brw_check_radiologist_lock]
	@id  uniqueidentifier,
	@status_id int,
	@user_id uniqueidentifier,
	@menu_id int,
	@session_id uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@refresh nchar(1) ='N' output,
	@status_desc nvarchar(30)='' output,
	@user_name nvarchar(100)='' output,
	@error_code nvarchar(10)='' output,
	@return_status int=0 output
as
begin
	set nocount on

	declare @radiologist_id uniqueidentifier,
			@curr_radiologist_id uniqueidentifier,
	        @curr_status_id int,
			@user_role_code nvarchar(10),
			@activity_text nvarchar(max),
			@menu_text nvarchar(100),
			@study_uid nvarchar(100)
			
	
	select @radiologist_id = id from radiologists where login_user_id=@user_id
	select @curr_status_id = study_status_pacs from study_hdr where id=@id
	select @status_desc = status_desc from sys_study_status_pacs where status_id = @curr_status_id

	--print @status_id
	--print @curr_status_id

	select @user_role_code = ur.code
	from users u
	inner join user_roles ur on ur.id =u.user_role_id
	where u.id = @user_id

	

	if(@status_id <> @curr_status_id)
		begin
			set @refresh ='Y'
			select @error_code = '296',@return_status=0
			return 0
		end

	if(select count(record_id) from sys_record_lock_ui where menu_id=@menu_id and record_id=@id and isnull(session_id,'00000000-0000-0000-0000-000000000000')<> @session_id)>0
		begin
			set @refresh ='Y'
			--select @user_name = name from users where id =(select user_id from sys_record_lock_ui where menu_id=@menu_id and record_id=@id)
			select @error_code = '303',@return_status=0
			return 0
		end


    if(@user_role_code='RDL')
		begin
			if(@status_id = 50)
				begin
					if(select count(id) from study_hdr where id=@id)>0
						begin
							select @curr_radiologist_id= radiologist_id from study_hdr where id=@id
						end
				    else if(select count(id) from study_hdr_archive where id=@id)>0
						begin
							select @curr_radiologist_id= radiologist_id from study_hdr_archive where id=@id
						end

				   if(@curr_radiologist_id<>'00000000-0000-0000-0000-000000000000' and @curr_radiologist_id<> @radiologist_id)
						begin
							if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id and other_radiologist_id=@curr_radiologist_id)=0
								begin
									set @refresh ='Y'
									select @error_code = '493',@return_status=0
									return 0
								end

							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='ACCLOCKSTUDY')=0
								begin
									if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='VWLOCKSTUDY')=0
										begin
											set @refresh ='Y'
											select @error_code = '297',@return_status=1
											return 1
										end
									else
										begin
											set @refresh ='N'
											select @user_name = name from radiologists where id = @curr_radiologist_id
											select @error_code = '303',@return_status=1
											return 1
										end
								end
							else
								begin
									set @refresh ='N'
									select @user_name = name from radiologists where id = @curr_radiologist_id
									select @error_code = '303',@return_status=1
									return 1
								end
						end
					else
						begin
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and (right_code='DICTRPT' or right_code='UPDFINALRPT' or right_code='UPDPRELIMRPT' or right_code='UPDFINALRPT'))<=0
								begin
									set @refresh ='Y'
									select @error_code = '296',@return_status=0
									return 0
								end
						end
				end
			else if(@status_id = 60)
				begin
				   if(select count(id) from study_hdr where id=@id)>0
						begin
							select @curr_radiologist_id= radiologist_id from study_hdr where id=@id
						end
				    else if(select count(id) from study_hdr_archive where id=@id)>0
						begin
							select @curr_radiologist_id= radiologist_id from study_hdr_archive where id=@id
						end

				   if(@curr_radiologist_id<>'00000000-0000-0000-0000-000000000000' and @curr_radiologist_id<> @radiologist_id)
						begin
						    if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id and other_radiologist_id=@curr_radiologist_id)=0
								begin
									set @refresh ='Y'
									select @error_code = '493',@return_status=0
									return 0
								end
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='ACCLOCKSTUDY')=0
								begin
									if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='VWLOCKSTUDY')=0
										begin
											set @refresh ='Y'
											select @error_code = '297',@return_status=1
											return 1
										end
									else
										begin
											set @refresh ='N'
											select @user_name = name from radiologists where id = @curr_radiologist_id
											select @error_code = '303',@return_status=1
											return 1
										end
								end
							else
								begin
									set @refresh ='N'
									select @user_name = name from radiologists where id = @curr_radiologist_id
									select @error_code = '303',@return_status=1
									return 1
								end
						end
					else
						begin
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and (right_code='DICTRPT' or right_code='UPDFINALRPT' or right_code='UPDPRELIMRPT' or right_code='UPDFINALRPT'))<=0
								begin
									set @refresh ='Y'
									select @error_code = '296',@return_status=0
									return 0
								end
						end
				end
			else if(@status_id = 80)
				begin
					if(select count(id) from study_hdr where id=@id)>0
						begin
							select @curr_radiologist_id= final_radiologist_id from study_hdr where id=@id
						end
					else if(select count(id) from study_hdr_archive where id=@id)>0
						begin
							select @curr_radiologist_id= final_radiologist_id from study_hdr_archive where id=@id
						end

				   if(@curr_radiologist_id<>'00000000-0000-0000-0000-000000000000' and @curr_radiologist_id<> @radiologist_id)
						begin
						    if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id and other_radiologist_id=@curr_radiologist_id)=0
								begin
									set @refresh ='Y'
									select @error_code = '493',@return_status=0
									return 0
								end
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='ACCLOCKSTUDY')=0
								begin
									if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='VWLOCKSTUDY')=0
										begin
											set @refresh ='Y'
											select @error_code = '297',@return_status=1
											return 1
										end
									else
										begin
											set @refresh ='N'
											select @user_name = name from radiologists where id = @curr_radiologist_id
											select @error_code = '303',@return_status=1
											return 1
										end
								end
							else
								begin
									set @refresh ='N'
									select @user_name = name from radiologists where id = @curr_radiologist_id
									select @error_code = '303',@return_status=1
									return 1
								end
						end
					else
						begin
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code='UPDFINALRPT')<=0
								begin
									set @refresh ='Y'
									select @error_code = '296',@return_status=0
									return 0
								end
						end
				end
			else if(@status_id = 100)
				begin

				   if(select count(id) from study_hdr where id=@id)>0
						begin
							select @curr_radiologist_id= final_radiologist_id from study_hdr where id=@id
						end
					else if(select count(id) from study_hdr_archive where id=@id)>0
						begin
							select @curr_radiologist_id= final_radiologist_id from study_hdr_archive where id=@id
						end

				   if(@curr_radiologist_id<>'00000000-0000-0000-0000-000000000000' and @curr_radiologist_id<> @radiologist_id)
						begin
						    if(select count(other_radiologist_id) from radiologist_functional_rights_other_radiologist where radiologist_id = @radiologist_id and other_radiologist_id=@curr_radiologist_id)=0
								begin
									set @refresh ='Y'
									select @error_code = '493',@return_status=0
									return 0
								end
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='ACCLOCKSTUDY')=0
								begin
									if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code ='VWLOCKSTUDY')=0
										begin
											set @refresh ='Y'
											select @error_code = '297',@return_status=1
											return 1
										end
									else
										begin
											set @refresh ='N'
											select @user_name = name from radiologists where id = @curr_radiologist_id
											select @error_code = '303',@return_status=1
											return 1
										end
								end
							else
								begin
									set @refresh ='N'
									select @user_name = name from radiologists where id = @curr_radiologist_id
									select @error_code = '303',@return_status=1
									return 1
								end
						end
					else
						begin
							if(select count(right_code) from radiologist_functional_rights_assigned where radiologist_id = @radiologist_id and right_code='UPDFINALRPT')<=0
								begin
									set @refresh ='Y'
									select @error_code = '296',@return_status=0
									return 0
								end
						end
				end

			if(select count(record_id) from sys_record_lock_ui where record_id=@id and menu_id=@menu_id)=0
				begin
				    select @study_uid  = study_uid
					from study_hdr 
					where id=@id

					exec common_lock_record_ui
						@menu_id       = @menu_id,
						@record_id     = @id,
						@user_id       = @user_id,
						@session_id    = @session_id,
						@error_code    = @error_code output,
						@return_status = @return_status output	
						
					if(@return_status=0)
						begin
							return 0
						end

					set @activity_text =  'Locked'
					exec common_study_user_activity_trail_save
						@study_hdr_id  = @id,
						@study_uid     = '',
						@menu_id       = @menu_id,
						@activity_text = @activity_text,
						@session_id    = @session_id,
						@activity_by   = @user_id,
						@error_code    = @error_code output,
						@return_status = @return_status output

					if(@return_status=0)
						begin
							return 0
						end

					select @menu_text= menu_desc from sys_menu where menu_id=@menu_id
					set  @activity_text =  isnull(@menu_text,'')  + '==>Locked => Study UID ' + @study_uid
					exec common_user_activity_log
							@user_id       = @user_id,
							@activity_text = @activity_text,
							@session_id    = @session_id,
							@menu_id       = @menu_id,
							@error_code    = @error_code output,
							@return_status = @return_status output

					if(@return_status=0)
						begin
							return 0
						end
				end
		end
	
	


	select @error_code = '',@return_status=1,@refresh='N'
	return 1
	set nocount off
end


GO
