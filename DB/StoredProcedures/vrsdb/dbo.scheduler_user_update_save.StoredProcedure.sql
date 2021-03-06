USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_user_update_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[scheduler_user_update_save]
GO
/****** Object:  StoredProcedure [dbo].[scheduler_user_update_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : scheduler_user_update_save : 
                  save transfered file count
** Created By   : Pavel Guha
** Created On   : 01/08/2019
*******************************************************/
CREATE procedure [dbo].[scheduler_user_update_save]
    @user_id uniqueidentifier,
	@user_type nvarchar(5),
	@error_msg nvarchar(500) = '' output,
	@return_type int = 0 output
as
begin
	
	set nocount on

	declare @login_id nvarchar(50),
	        @id uniqueidentifier

	begin transaction

	select @login_id = login_id
	from users
	where id=@user_id

	if(@user_type='IU')
		begin
			update institution_user_link
			set updated_in_pacs= 'Y',
			    date_updated_in_pacs = getdate()
			where user_id=@user_id

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update the institution user ' + @login_id + ' in PACS'
					return 0
				end
		end
	else if(@user_type='RDL')
		begin
			update radiologists
			set updated_in_pacs= 'Y',
			    date_updated_in_pacs = getdate()
			where code=(select code from users where id=@user_id)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update the radiologist ' + @login_id + ' in PACS'
					return 0
				end
		end
	else if(@user_type='TRS')
		begin
			update transciptionists
			set updated_in_pacs= 'Y',
			    date_updated_in_pacs = getdate()
			where code=(select code from users where id=@user_id)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update the transcriptionist ' + @login_id + ' in PACS'
					return 0
				end
		end
    else if(@user_type='TCHN')
		begin
			update technicians
			set updated_in_pacs= 'Y',
			    date_updated_in_pacs = getdate()
			where code=(select code from users where id=@user_id)

			if(@@rowcount=0)
				begin
					rollback transaction
					select @return_type=0,@error_msg='Failed to update the technician ' + @login_id + ' in PACS'
					return 0
				end
		end


    commit transaction
	set @return_type=1
	set @error_msg=''

	set nocount off
	return 1

end


GO
