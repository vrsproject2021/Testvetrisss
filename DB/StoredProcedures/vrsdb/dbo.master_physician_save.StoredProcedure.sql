USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_physician_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_physician_save]
GO
/****** Object:  StoredProcedure [dbo].[master_physician_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : master_physician_save : save
                  physician 
** Created By   : Pavel Guha
** Created On   : 23/04/2019
*******************************************************/
CREATE procedure [dbo].[master_physician_save]
(
	@id						uniqueidentifier='00000000-0000-0000-0000-000000000000' output,
	@code                   nvarchar(5),
	@name		            nvarchar(100)	= '',
	@email_id				nvarchar(50)	= '',
	@address_Line1			nvarchar(100)	= '',
	@address_Line2			nvarchar(100)	= '',
	@city					nvarchar(100)	= '',
	@zip			     	nvarchar(20)	= '',
	@state_id				int				= 0,
	@country_id				int				= 0,
	@phone					nvarchar(30)	= '',
	@mobile					nvarchar(20)	= '',
	@is_active				char(1)			= 'Y',
	@xml_institution        ntext           = null,
	@updated_by             uniqueidentifier,
    @menu_id                int,
    @user_name              nvarchar(700) = '' output,
	@error_code				nvarchar(10)	= '' output,
    @return_status			int				= 0  output
)
as
begin
	set nocount on 
	
	declare @hDoc int,
		    @counter bigint,
	        @rowcount bigint 


	 declare @institution_id uniqueidentifier,
	         @institution_name nvarchar(200),
			 @physician_user_id uniqueidentifier
			 
	--if(select count(id) from physicians where upper(code) = @code and id<>@id)>0
	--	begin
	--			select @error_code='074',@return_status=0,@user_name=@name
	--			return 0
	--	end

	if(select count(id) from physicians where upper(email_id) = @email_id and id<>@id)>0
		begin
				select @error_code='092',@return_status=0
				return 0
		end

	begin transaction
	if(@xml_institution is not null) exec sp_xml_preparedocument @hDoc output,@xml_institution 

	if(@id = '00000000-0000-0000-0000-000000000000')
		begin
			set @id	=NEWID()
			insert into physicians
						(
							id,
							code,
							name,
							address_1,
							address_2,
							city,
							state_id,
							country_id,
							zip,
							email_id,
							phone_no,
							mobile_no,
							is_active,
							created_by,
							date_created

						)
					values
						(
							@id,
							@code,
							@name,
							@email_id,
							@address_Line1,
							@address_Line2,
							@city,
							@state_id,
							@country_id,
							@zip,
							@phone,
							@mobile,
							@is_active,
							@updated_by,
							getdate()
						)

			    if(@@rowcount=0)
					begin
						rollback transaction
						if(@xml_institution is not null) exec sp_xml_removedocument @hDoc
						select	@return_status=0,
								@error_code='035'
						return 0
					end
			
		end
	else
		begin
			exec common_check_record_lock_ui
				@menu_id       = @menu_id,
				@record_id     = @id,
				@user_id       = @updated_by,
				@user_name     = @user_name output,
				@error_code    = @error_code output,
				@return_status = @return_status output
		
			if(@return_status=0)
				begin
					return 0
				end

			update physicians
					set
						code                    = @code,
						name					= @name,
						email_id				= @email_id,
						address_1				= @address_Line1,
						address_2				= @address_Line2,
						city					= @city,
						state_id				= @state_id,
						country_id				= @country_id,
						zip						= @zip,
						phone_no				= @phone,
						mobile_no				= @mobile,
						is_active				= @is_active,
						updated_by				= @updated_by,
						date_updated			= getdate()

					where id = @id

		
	  end



	if(@xml_institution is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc,'institution/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @institution_id    = institution_id
					from openxml(@hDoc,'institution/row',2)
					with
					( 
						institution_id uniqueidentifier,
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  
			
					if(@institution_id <> '00000000-0000-0000-0000-000000000000')
						begin
							select @institution_name = name from physicians where id=@institution_id
						
							if(select count(institution_id) from institution_physician_link  where institution_id=@institution_id and physician_id=@id)=0
								begin
									insert into institution_physician_link(institution_id,physician_id,created_by,date_created)
																	values(@institution_id,@id,@updated_by,getdate())
					                                              
									if(@@rowcount=0)
										begin
											rollback transaction
											if(@xml_institution is not null) exec sp_xml_removedocument @hDoc
											select @error_code='066',@return_status=0,@user_name=@institution_name
											return 0
										end
						
				
								end
							
						end

					set @counter = @counter + 1
				end
		end

	delete from institution_physician_link
	where physician_id = @id
	and institution_id not in (select  institution_id  
								from openxml(@hDoc,'institution/row', 2)  
							   with( institution_id uniqueidentifier,
									 row_id bigint ))

	exec common_lock_record_ui
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @updated_by,
		@error_code    = @error_code output,
		@return_status = @return_status output

	if(@return_status=0)
		begin
			rollback transaction
			return 0
		end

	commit transaction
	if(@xml_institution is not null) exec sp_xml_removedocument @hDoc
	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
end



GO
