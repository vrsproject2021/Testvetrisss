USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[hk_message_recipient_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[hk_message_recipient_list_fetch]
GO
/****** Object:  StoredProcedure [dbo].[hk_message_recipient_list_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : hk_message_recipient_list_fetch : fetch Recipient List from Institution/SalesPerson/
				  Radiologist/Technician/Transcriptionist.
** Created By   : BK
** Created On   : 01/08/2019
*******************************************************/

-- exec hk_message_recipient_list_fetch
CREATE PROCEDURE [dbo].[hk_message_recipient_list_fetch]
(
	@user_id uniqueidentifier
)

AS
begin 

	-- select Institution List---------
	select institution_id = inst.id
		   ,id=ipl.physician_id
		   ,name=ipl.physician_name
		   ,email_id=ipl.physician_email
		   ,mobile=ipl.physician_mobile
		   ,checkbox=''''
	from institution_physician_link ipl
		left join institutions inst 
			on ipl.institution_id=inst.id
	where is_active='Y'
	order by institution_id


	-- select Radiologists List
	select  id
		   ,name 
		   ,email_id
		   ,mobile = mobile_no
		   ,checkbox=''''
	from radiologists 
	where is_active='Y'

	-- select Sales Persons List
	select  id
		   ,name 
		   ,email_id
		   ,mobile = mobile_no
		   ,checkbox=''''
	from salespersons 
	where is_active='Y'

	-- select Technicians List
	select  id
		   ,name 
		   ,email_id
		   ,mobile = mobile_no
		   ,checkbox=''''
	from technicians
	where is_active='Y' 

	-- select Transciptionists List
	select  id
		   ,name 
		   ,email_id
		   ,mobile = mobile_no
		   ,checkbox=''''
	from transciptionists 
		where is_active='Y'

end



GO
