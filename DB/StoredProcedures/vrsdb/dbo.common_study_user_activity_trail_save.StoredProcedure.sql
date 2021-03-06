USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[common_study_user_activity_trail_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[common_study_user_activity_trail_save]
GO
/****** Object:  StoredProcedure [dbo].[common_study_user_activity_trail_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[common_study_user_activity_trail_save]
	@study_hdr_id uniqueidentifier,
	@study_uid nvarchar(100)=null,
	@menu_id int=0,
	@activity_text nvarchar(max)='',
	@activity_by uniqueidentifier,
	@session_id uniqueidentifier = '00000000-0000-0000-0000-000000000000',
	@error_code nvarchar(10)='' output,
    @return_status int =0 output
As
	set nocount on 
	Begin
		
		if(isnull(@study_uid,'') ='')
			begin
				if(select count(id) from study_hdr where id=@study_hdr_id)>0
					begin
						select @study_uid=study_uid from study_hdr where id=@study_hdr_id
					end
				else if(select count(id) from study_hdr_archive where id=@study_hdr_id)>0
					begin
						select @study_uid=study_uid from study_hdr_archive where id=@study_hdr_id
					end
				else
					begin
						select @study_uid=''
					end
			end


		Insert Into vrslogdb..sys_study_user_activity_trail(study_hdr_id,study_uid,menu_id,activity_text,
		                                          activity_by,activity_datetime,session_id)
											values(@study_hdr_id,@study_uid,@menu_id,@activity_text,
													@activity_by,GETDATE(),@session_id)
		
		if(@@rowcount=0)
			begin
				select @error_code='390',@return_status=0
				return 0
			end


		select @return_status=1
		set nocount off
		return 1
	End

GO
