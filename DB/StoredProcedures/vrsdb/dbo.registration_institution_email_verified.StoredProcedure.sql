USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[registration_institution_email_verified]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[registration_institution_email_verified]
GO
/****** Object:  StoredProcedure [dbo].[registration_institution_email_verified]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[registration_institution_email_verified]
(
	@id						uniqueidentifier='00000000-0000-0000-0000-000000000000',
	@error_code				nvarchar(10)	= '' output,
    @return_status			int				= 0  output
)
as
begin
	declare @rc int 
	begin transaction
	if(@id <> '00000000-0000-0000-0000-000000000000')
		begin
			set @rc = (select count(*) from institutions where id=@id)

			if (@rc = 1)
				begin
					update institutions
								set is_email_verified ='Y'
					where id=@id

					if(@@rowcount=1)
						begin
							rollback transaction
							select	@return_status=0
							return 0
						end	

					update users
					set is_active='Y'
					where id in (select user_id from institution_user_link where institution_id=@id)

					if(@@rowcount=1)
						begin
							rollback transaction
							select	@return_status=0
							return 0
						end	

				end
				
		end

	 commit transaction
	 select @error_code='',@return_status=1
	 return 1
end
GO
