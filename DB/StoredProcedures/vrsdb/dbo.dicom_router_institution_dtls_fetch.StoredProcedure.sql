USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_institution_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dicom_router_institution_dtls_fetch]
GO
/****** Object:  StoredProcedure [dbo].[dicom_router_institution_dtls_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : dicom_router_institution_dtls_fetch : fetch
                  institution details
** Created By   : Pavel Guha
** Created On   : 10/11/2019
*******************************************************/
--EXEC dicom_router_institution_dtls_fetch '00616','','','','','','','',0
CREATE procedure [dbo].[dicom_router_institution_dtls_fetch]
	@code nvarchar(5),
	@name nvarchar(100) = '' output,
	@address_1 nvarchar(100)='' output,
	@address_2 nvarchar(100)='' output,
	@zip nvarchar(10)='' output,
	@login_id nvarchar(30)='' output,
	@study_img_manual_receive_path  nvarchar(250) = '' output,
	@xfer_files_compress nchar(1)='Y' output,
	@output_msg nvarchar(100)='' output,
	@return_status int = 0 output
as
begin
	set nocount on
	declare @id uniqueidentifier,
	        @billing_account_id uniqueidentifier

	if(select count(id) from institutions where code=@code)>0
		begin
			if(select is_active from institutions where code=@code)='Y'
				begin
					select @id = id,
					       @billing_account_id = billing_account_id
					from institutions 
					where code= @code
					print @id
					if(select count(user_login_id) from institution_user_link where institution_id = @id)>0
						begin
							select top 1 @login_id = iul.user_login_id
							from institution_user_link iul
							inner join users u on u.id = iul.user_id
							where iul.institution_id = @id
							and u.is_active='Y'

							if(isnull(@login_id,''))=''
								begin
									if(isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
										begin
								
											select @login_id = login_id
											from billing_account
											where id = @billing_account_id
										end
								end
						end
					else
						begin
							if(isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000') <> '00000000-0000-0000-0000-000000000000')
								begin
								
									select @login_id = login_id
									from billing_account
									where id = @billing_account_id
								end 
							
						end
					 --PRINT @billing_account_id
					 --print  @login_id
					select @name                          = i.name,
					       @address_1                     = isnull(i.address_1,''),
						   @address_2                     = rtrim(ltrim(isnull(i.address_2,'') + ' ' + isnull(s.name,'') + ' ' + c.name)),
						   @zip                           = isnull(i.zip,''),
						   @login_id                      = isnull(@login_id,''),
						   @study_img_manual_receive_path = rtrim(ltrim(isnull(i.study_img_manual_receive_path ,''))),
						   @xfer_files_compress           = isnull(i.xfer_files_compress,'Y') 
					from institutions i
					left outer join sys_states s on s.id = i.state_id
					inner join sys_country c on c.id = i.country_id
					where i.id=@id

					--print @name                     
					--print       @address_1                 
					--print	   @address_2                    
					--print	   @zip                         
					--print	   @login_id                     
					--print	   @study_img_manual_receive_path

					select @output_msg='SUCCESS', @return_status=1
				end
			else
				begin
					select @output_msg='FAIL : The institution has been deactivated',@return_status=0
				end
		end 
	else 
		begin
			select @output_msg='FAIL : The site code does not exist',@return_status=0
		end
	set nocount off
	if(@output_msg='001' or @output_msg='002') return 0
	else return 1
	
end
GO
