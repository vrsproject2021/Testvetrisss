USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[reg_institution_wise_physician_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[reg_institution_wise_physician_fetch]
GO
/****** Object:  StoredProcedure [dbo].[reg_institution_wise_physician_fetch]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************    
*******************************************************    
** Version  : 1.0.0.0    
** Procedure    :  reg_institution_wise_physician_fetch:fetch '    
                   institution wise physicians    
** Created By   : AM    
** Created On   : 29/07/2020    
*******************************************************    
*******************************************************/    
-- exec institution_wise_physician_fetch 2    
CREATE procedure [dbo].[reg_institution_wise_physician_fetch]    
 @reg_institution_id uniqueidentifier    
as    
begin    
 set nocount on    
     
 select physician_id,physician_name from institution_reg_physician_link     
 where institution_id=@reg_institution_id    
 order by physician_name    
    
 select code,[name],login_id,login_password,
        case when login_email_id='' then email_id else login_email_id end as email_id,
		contact_person_name
 from institutions_reg 
 where id=@reg_institution_id    
    
 set nocount off    
     
end 
GO
