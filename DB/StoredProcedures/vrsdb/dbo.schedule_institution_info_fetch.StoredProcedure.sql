USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[schedule_institution_info_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[schedule_institution_info_fetch]
GO
/****** Object:  StoredProcedure [dbo].[schedule_institution_info_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : schedule_institution_info_fetch : fetch
				  institution info
** Created By   : Pavel Guha
** Created On   : 30/09/2020
*******************************************************/
-- exec schedule_institution_info_fetch  'Southeast Veterinary Neurology'
CREATE procedure [dbo].[schedule_institution_info_fetch]
	@institution_name nvarchar(100)
as
begin
	set nocount on
	declare @inst_id uniqueidentifier,
			@inst_code nvarchar(5),
	        @inst_name nvarchar(100)

	set @inst_id ='00000000-0000-0000-0000-000000000000'
	set @inst_code=''
	set @inst_name=''

	if(select count(i.id)
	from institutions i
	where (upper(i.name) = upper(rtrim(ltrim(@institution_name))) or upper(i.code) = upper(rtrim(ltrim(@institution_name))))
	and is_active='Y'
	) =0
		begin
			if(select count(inl.institution_id)
				from institution_alt_name_link inl
				inner join institutions i on i.id= inl.institution_id
				where upper(inl.alternate_name) =upper(rtrim(ltrim(@institution_name)))
				and i.is_active='Y'
				) =0
					begin
						select @inst_id='00000000-0000-0000-0000-000000000000',
						       @inst_code='',
							   @inst_name=upper(rtrim(ltrim(@institution_name)))
					end
				else
					begin
						select @inst_id   = i.id,
							   @inst_code = i.code,
						       @inst_name = i.name
						from institution_alt_name_link inl
						inner join institutions i on i.id= inl.institution_id
						where upper(inl.alternate_name) = upper(rtrim(ltrim(@institution_name)))
						and i.is_active='Y'
					end
		end
	else
		begin
		    if(select count(id)
			   from institutions
			   where (upper(name) = upper(rtrim(ltrim(@institution_name))) or upper(code) = upper(rtrim(ltrim(@institution_name)))))=1
				begin
					select  @inst_id = id,
							@inst_code = code,
							@inst_name = name
					from institutions
					where (upper(name) = upper(rtrim(ltrim(@institution_name))) or upper(code) = upper(rtrim(ltrim(@institution_name))))
					and is_active='Y'
				end
			else
				begin
					select top 1 @inst_id = id,
								 @inst_code = code,
								 @inst_name = name
						from institutions
						where (upper(name) = upper(rtrim(ltrim(@institution_name))) or upper(code) = upper(rtrim(ltrim(@institution_name))))
						and is_active='Y'
						order by code
				end
			
		end

	select institution_id   = @inst_id,
		   institution_code = @inst_code,
	       institution_name = @inst_name
	set nocount off
end

GO
