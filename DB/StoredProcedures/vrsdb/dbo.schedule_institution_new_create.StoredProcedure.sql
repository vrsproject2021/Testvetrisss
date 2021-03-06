USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[schedule_institution_new_create]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[schedule_institution_new_create]
GO
/****** Object:  StoredProcedure [dbo].[schedule_institution_new_create]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : schedule_institution_new_create : create
				  new institution 
** Created By   : Pavel Guha
** Created On   : 30/09/2020
*******************************************************/
-- exec schedule_institution_new_create 
create procedure [dbo].[schedule_institution_new_create]
	@institution_name nvarchar(100),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	set nocount on
	declare @inst_id uniqueidentifier,
			@inst_code nvarchar(5),
	        @inst_name nvarchar(100),
			@default_country_id int

	set @inst_id ='00000000-0000-0000-0000-000000000000'
	set @inst_code=''
	set @inst_name=''

	begin transaction
	if(select count(id)
	from institutions
	where upper(name) = upper(@institution_name)
	and is_active='Y') =0
		begin
			if(select count(inl.institution_id)
				from institution_alt_name_link inl
				inner join institutions i on i.id= inl.institution_id
				where upper(inl.alternate_name) = upper(@institution_name)
				and i.is_active='Y') =0
					begin
						set @inst_id = newid()
						select @default_country_id =  id from sys_country where is_default='Y'

						insert into institutions(id,code,name,country_id,created_by,date_created) 
										 values (@inst_id,'',@institution_name,@default_country_id,'00000000-0000-0000-0000-000000000000',getdate())

						if(@@rowcount=0)
							begin
								rollback transaction
								select @return_type=0,@error_msg='Failed to create institution ' + @institution_name
								return 0
							end
					end
				
		end

	commit transaction
	select @error_msg='',@return_type=1
	set nocount off
	return 1
end

GO
