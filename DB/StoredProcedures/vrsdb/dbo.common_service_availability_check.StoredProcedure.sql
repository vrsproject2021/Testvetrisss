USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_service_availability_check]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_service_availability_check]
GO
/****** Object:  StoredProcedure [dbo].[common_service_availability_check]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : common_service_availability_check : check
                  service availability
** Created By   : Pavel Guha
** Created On   : 11/04/2019
*******************************************************/
--exec common_service_availability_check 1,2,'C3250CFB-EA6A-4809-8034-17DEE232EB48',10,'N','N','',0
CREATE PROCEDURE [dbo].[common_service_availability_check] 
	@species_id int,
	@modality_id int,
	@institution_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@priority_id int,
	@beyond_operation_time nchar(1) ='N' output,
	@in_exp_list nchar(1)='N' output,
    @error_code nvarchar(500)='' output,
    @return_status int =0 output
as
begin

	set nocount on
	set datefirst 1

	declare @is_stat nchar(1),
	        @day_no int,
			@start_from datetime,
			@end_at datetime,
			@curr_date_time datetime
			

	select @is_stat = is_stat from sys_priority where priority_id=@priority_id
	set @is_stat = isnull(@is_stat,'N')

	select @day_no = datepart(dw,getdate())
	select  @start_from   = convert(varchar(11),getdate(),106) + ' ' + from_time,
			@end_at       = convert(varchar(11),getdate(),106) + ' ' + till_time
	from settings_operation_time
	where day_no = @day_no

	select @curr_date_time = getdate()

	if((@curr_date_time not between @start_from and @end_at))
		set @beyond_operation_time='Y'
	else
		set @beyond_operation_time='N'

	set @in_exp_list='N'

	if(@beyond_operation_time ='N' and @is_stat='Y')--Not beyond Operation Time
		begin
			
			--check for service for species availability
			if(isnull((select available 
						from settings_service_species_available
						where species_id = @species_id
						and service_id = (select id from services where priority_id = @priority_id)),'N'))='N'
			  begin
					--check of species exception institution
					if(select count(institution_id)
						from settings_service_species_available_exception_institution
						where after_hours='N'
						and institution_id     = @institution_id
						and species_id         = @species_id
						and service_id         = (select id from services where priority_id = @priority_id))= 0
							begin
									select @return_status = 0,
										   @error_code    = isnull((select message_display 
																	from settings_service_species_available
																	where species_id = @species_id
																	and service_id = (select id from services where priority_id = @priority_id)),'')

									if(rtrim(ltrim(@error_code)) ='') select @error_code='495'
									return 0
						   end
					else --check for service for modality availability
							begin
								if(isnull((select available 
										   from settings_service_modality_available
										   where modality_id = @modality_id
										   and service_id = (select id from services where priority_id = @priority_id)),'N'))='N'
								begin
									--check of modality exception institution
									if(select count(institution_id)
										from settings_service_modality_available_exception_institution
										where after_hours='N'
										and institution_id     = @institution_id
										and modality_id        = @modality_id
										and service_id         = (select id from services where priority_id = @priority_id))= 0
											begin
												select @return_status=0,
													   @error_code = isnull((select message_display 
																			 from settings_service_modality_available
																			 where modality_id = @modality_id
																			 and service_id = (select id from services where priority_id = @priority_id)),'')

												if(rtrim(ltrim(@error_code)) ='') select @error_code='466'
												return 0
										   end
								end
							end
			  end
			else if(isnull((select available 
							from settings_service_species_available
							where species_id = @species_id
							and service_id = (select id from services where priority_id = @priority_id)),'N'))='Y'
				begin
					--check of species exception institution
					if(select count(institution_id)
					   from settings_service_species_available_exception_institution
					   where after_hours      ='N'
					   and institution_id     = @institution_id
					   and species_id         = @species_id
					   and service_id         = (select id from services where priority_id = @priority_id))> 0
						 begin
								select @return_status=0,
									   @error_code = isnull((select message_display 
															 from settings_service_modality_available
															 where modality_id = @modality_id
														 	 and service_id = (select id from services where priority_id = @priority_id)),'')
								if(rtrim(ltrim(@error_code)) ='') select @error_code='495'
								return 0
						 end
					else--check for service for modality availability
						 begin
						    --check of modality exception institution
							if(isnull((select available 
									   from settings_service_modality_available
										where modality_id = @modality_id
										and service_id = (select id from services where priority_id = @priority_id)),'N'))='N'
								begin
									if(select count(institution_id)
										from settings_service_modality_available_exception_institution
										where after_hours      ='N'
										and institution_id    = @institution_id
										and modality_id        = @modality_id
										and service_id         = (select id from services where priority_id = @priority_id))= 0
											begin
												select @return_status=0,
													   @error_code = isnull((select message_display 
																			 from settings_service_modality_available
																			 where modality_id = @modality_id
																			 and service_id = (select id from services where priority_id = @priority_id)),'')

												if(rtrim(ltrim(@error_code)) ='') select @error_code='466'
												return 0
										   end
								end
						 end
				end
		end
	else if(@beyond_operation_time ='Y' and @is_stat='Y')--Beyond operation time
		begin
			--check for service for species availability
			if(isnull((select available 
						from settings_service_species_available_after_hours
						where species_id = @species_id
						and service_id = (select id from services where priority_id = @priority_id)),'N'))='N'
				begin
					if(select count(institution_id)
					   from settings_service_species_available_exception_institution
					   where after_hours='Y'
					   and institution_id     = @institution_id
					   and species_id         = @species_id
					   and service_id         = (select id from services where priority_id = @priority_id))= 0
							begin
								set @in_exp_list='N'
							end
					--check for service for modality availability
					else if(isnull((select available 
							from settings_service_modality_available_after_hours
							where modality_id = @modality_id
							and service_id = (select id from services where priority_id = @priority_id)),'N'))='N'
							begin
								if(select count(institution_id)
									from settings_service_modality_available_exception_institution
									where after_hours='Y'
									and institution_id     = @institution_id
									and modality_id        = @modality_id
									and service_id         = (select id from services where priority_id = @priority_id))= 0
										begin
										   set @in_exp_list='N'
										end
								else
										begin
											set @in_exp_list='Y'
										end
							end
					else if(isnull((select available 
									from settings_service_modality_available_after_hours
									where modality_id = @modality_id
									and service_id = (select id from services where priority_id = @priority_id)),'N'))='Y'
							begin
								if(select count(institution_id)
									from settings_service_modality_available_exception_institution
									where after_hours='Y'
									and institution_id     = @institution_id
									and modality_id        = @modality_id
									and service_id         = (select id from services where priority_id = @priority_id))> 0
										begin
											set @in_exp_list='Y'
										end
								else
										begin
											set @in_exp_list='N'
										end
							end

				end
			else if(isnull((select available 
							from settings_service_species_available_after_hours
							where species_id = @species_id
							and service_id = (select id from services where priority_id = @priority_id)),'N'))='Y'
				begin
					if(select count(institution_id)
						from settings_service_modality_available_exception_institution
						where after_hours='Y'
						and institution_id     = @institution_id
						and modality_id        = @modality_id
						and service_id         = (select id from services where priority_id = @priority_id))> 0
							begin
								set @in_exp_list='Y'
							end
					--check for service for modality availability
					else if(isnull((select available 
							from settings_service_modality_available_after_hours
							where modality_id = @modality_id
							and service_id = (select id from services where priority_id = @priority_id)),'N'))='N'
								begin
									if(select count(institution_id)
										from settings_service_modality_available_exception_institution
										where after_hours='Y'
										and institution_id     = @institution_id
										and modality_id        = @modality_id
										and service_id         = (select id from services where priority_id = @priority_id))= 0
											begin
											   set @in_exp_list='N'
											end
									else
											begin
												set @in_exp_list='Y'
											end
								end
					else if(isnull((select available 
									from settings_service_modality_available_after_hours
									where modality_id = @modality_id
									and service_id = (select id from services where priority_id = @priority_id)),'N'))='Y'
								begin
									if(select count(institution_id)
										from settings_service_modality_available_exception_institution
										where after_hours='Y'
										and institution_id     = @institution_id
										and modality_id        = @modality_id
										and service_id         = (select id from services where priority_id = @priority_id))> 0
											begin
												set @in_exp_list='Y'
											end
									else
											begin
												set @in_exp_list='N'
											end
								end
				end
		end

	select @error_code='',@return_status=1
	set nocount off
end
GO
