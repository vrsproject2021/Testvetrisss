USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologists_rights_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_radiologists_rights_save]
GO
/****** Object:  StoredProcedure [dbo].[master_radiologists_rights_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************    
*******************************************************    
** Version  : 1.0.0.0    
** Procedure    : master_radiologists_rights_save : save    
                  radiologist details.    
** Created By   : Pavel Guha    
** Created On   : 13/04/2020    
*******************************************************/    
CREATE procedure [dbo].[master_radiologists_rights_save]    
(    
 @id                     uniqueidentifier = '00000000-0000-0000-0000-000000000000',    
 @xml_fn_rights          ntext               = null,    
 @xml_modality           ntext               = null,    
 @xml_species            ntext               = null,    
 @xml_institution        ntext               = null,    
 @xml_study_types        ntext               = null,    
 @xml_radiologist        ntext               = null,    
 @user_id                uniqueidentifier,    
 @menu_id                int,    
 @user_name              nvarchar(700)       = '' output,    
 @error_code             nvarchar(10)        = '' output,    
 @return_status          int                 = 0  output    
)    
as    
begin    
 set nocount on     
    
  declare   @hDoc1 int,    
			@hDoc2 int,    
			@hDoc3 int,    
			@hDoc4 int,    
			@hDoc5 int,    
			@hDoc6 int,   
			@counter int,    
			@rowcount int,    
			@right_code nvarchar(20),    
			@right_desc nvarchar(100),    
			@modality_id int,    
			@modality_name nvarchar(30),    
			@species_id int,  
			@species_name nvarchar(30),  
			@inst_id uniqueidentifier,    
			@inst_name nvarchar(100),    
			@study_type_id uniqueidentifier,    
			@study_type_desc nvarchar(100),    
			@radiologist_id uniqueidentifier,    
			@radiologist_name nvarchar(100)    
    
 begin transaction    
 if(@xml_fn_rights is not null)exec sp_xml_preparedocument @hDoc1 output,@xml_fn_rights     
 if(@xml_modality is not null)exec sp_xml_preparedocument @hDoc2 output,@xml_modality     
 if(@xml_institution is not null) exec sp_xml_preparedocument @hDoc3 output,@xml_institution     
 if(@xml_study_types is not null) exec sp_xml_preparedocument @hDoc4 output,@xml_study_types     
 if(@xml_radiologist is not null) exec sp_xml_preparedocument @hDoc5 output,@xml_radiologist     
 if(@xml_species is not null) exec sp_xml_preparedocument @hDoc6 output,@xml_species    
    
 exec common_check_record_lock_ui    
	  @menu_id       = @menu_id,    
	  @record_id     = @id,    
	  @user_id       = @user_id,    
	  @user_name     = @user_name output,    
	  @error_code    = @error_code output,    
	  @return_status = @return_status output    
      
 if(@return_status=0)    
  begin    
	   rollback transaction    
	   if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
	   if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
	   if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
	   if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
	   if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
	   if(@xml_species is not null) exec sp_xml_removedocument @hDoc6   
	   return 0    
  end    
    
     
 delete from radiologist_functional_rights_assigned where radiologist_id=@id    
    set @counter = 1    
 select  @rowcount=count(row_id)      
 from openxml(@hDoc1,'rights/row', 2)      
 with( row_id bigint )    
    
 while(@counter <= @rowcount)    
  begin    
   select  @right_code        = right_code    
   from openxml(@hDoc1,'rights/row',2)    
   with    
   (     
    right_code nvarchar(20),    
    row_id bigint    
   ) xmlTemp where xmlTemp.row_id = @counter    
       
      select @right_desc= right_desc from sys_radiologist_functional_rights where right_code=@right_code     
       
   insert into radiologist_functional_rights_assigned(radiologist_id,right_code,created_by,date_created)    
                                               values(@id,@right_code,@user_id,getdate())     
    
   if(@@rowcount=0)    
    begin    
     rollback transaction    
     if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
     if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
     if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
     if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
     if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
	 if(@xml_species is not null) exec sp_xml_removedocument @hDoc6   
     select @error_code='066',@return_status=0,@user_name= @right_desc    
     return 0    
    end    
       
       
   set @counter = @counter + 1    
  end    
  --Modality  
 delete from radiologist_functional_rights_modality where radiologist_id=@id    
    set @counter = 1    
 select  @rowcount=count(row_id)      
 from openxml(@hDoc2,'modality/row', 2)      
 with( row_id bigint )    
    
 while(@counter <= @rowcount)    
  begin    
   select  @modality_id        = modality_id    
   from openxml(@hDoc2,'modality/row',2)    
   with    
   (     
    modality_id int,    
    row_id bigint    
   ) xmlTemp where xmlTemp.row_id = @counter    
       
      select @modality_name= name from modality where id=@modality_id     
       
   insert into radiologist_functional_rights_modality(radiologist_id,modality_id,created_by,date_created)    
                                               values(@id,@modality_id,@user_id,getdate())     
    
   if(@@rowcount=0)    
    begin    
     rollback transaction    
     if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
     if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
     if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
     if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
     if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
     select @error_code='066',@return_status=0,@user_name= @modality_name    
     return 0    
    end    
    
   if(select count(modality_id) from radiologist_modality_link where radiologist_id = @id and modality_id=@modality_id)=0    
    begin    
     insert into radiologist_modality_link(radiologist_id,modality_id,prelim_fee,final_fee,addl_STAT_fee,work_unit,updated_by,date_updated)    
                                  values(@id,@modality_id,0,0,0,0,@user_id,getdate())    
    
     if(@@rowcount=0)    
      begin    
       rollback transaction    
       if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
             if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
             if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
             if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
             if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
       select @error_code='066',@return_status=0,@user_name= @modality_name    
       return 0    
      end    
    end    
       
       
   set @counter = @counter + 1    
  end    
    
  --Species  
 delete from radiologist_functional_rights_species where radiologist_id=@id    
    set @counter = 1    
 select  @rowcount=count(row_id)      
 from openxml(@hDoc6,'species/row', 2)      
 with( row_id bigint )    
    
 while(@counter <= @rowcount)    
  begin    
   select  @species_id        = species_id    
   from openxml(@hDoc6,'species/row',2)    
   with    
   (     
    species_id int,    
    row_id bigint    
   ) xmlTemp where xmlTemp.row_id = @counter    
       
      select @species_name= name from species where id=@species_id     
       
   insert into radiologist_functional_rights_species(radiologist_id,species_id,created_by,date_created)    
                                               values(@id,@species_id,@user_id,getdate())     
    
   if(@@rowcount=0)    
    begin    
     rollback transaction    
     if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
     if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
     if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
     if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
     if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
  if(@xml_species is not null) exec sp_xml_removedocument @hDoc6   
     select @error_code='066',@return_status=0,@user_name= @species_name    
     return 0    
    end    
       
   set @counter = @counter + 1    
  end   
  
 delete from radiologist_functional_rights_exception_institution where radiologist_id=@id    
    
 if(@xml_institution is not null)    
  begin    
   set @counter = 1    
   select  @rowcount=count(row_id)      
   from openxml(@hDoc3,'institution/row', 2)      
   with( row_id bigint )    
    
   while(@counter <= @rowcount)    
    begin    
     select  @inst_id        = institution_id    
     from openxml(@hDoc3,'institution/row',2)    
     with    
     (     
      institution_id uniqueidentifier,    
      row_id bigint    
     ) xmlTemp where xmlTemp.row_id = @counter    
       
     select @inst_name= name from institutions where id=@inst_id     
       
     insert into radiologist_functional_rights_exception_institution(radiologist_id,institution_id,created_by,date_created)    
                                         values(@id,@inst_id,@user_id,getdate())     
    
     if(@@rowcount=0)    
      begin    
       rollback transaction    
       if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
       if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
       if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
       if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
       if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
       select @error_code='066',@return_status=0,@user_name= @inst_name    
       return 0    
      end    
       
       
     set @counter = @counter + 1    
    end    
  end    
    
    
 delete from radiologist_functional_rights_exception_study_type where radiologist_id=@id    
    
 if(@xml_study_types is not null)    
  begin    
   set @counter = 1    
   select  @rowcount=count(row_id)      
   from openxml(@hDoc4,'study/row', 2)      
   with( row_id bigint )    
    
   while(@counter <= @rowcount)    
    begin    
     select  @study_type_id        = study_type_id    
     from openxml(@hDoc4,'study/row',2)    
     with    
     (     
      study_type_id uniqueidentifier,    
      row_id bigint    
     ) xmlTemp where xmlTemp.row_id = @counter    
       
     select @study_type_desc= name from modality_study_types where id=@study_type_id     
       
     insert into radiologist_functional_rights_exception_study_type(radiologist_id,study_type_id,created_by,date_created)    
                                         values(@id,@study_type_id,@user_id,getdate())     
    
     if(@@rowcount=0)    
      begin    
       rollback transaction    
       if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
       if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
       if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
       if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
       if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
       select @error_code='066',@return_status=0,@user_name= @inst_name    
       return 0    
      end    
       
       
     set @counter = @counter + 1    
    end    
  end    
    
 delete from radiologist_functional_rights_other_radiologist where radiologist_id=@id    
    
 if(@xml_radiologist is not null)    
  begin    
   set @counter = 1    
   select  @rowcount=count(row_id)      
   from openxml(@hDoc5,'radiologist/row', 2)      
   with( row_id bigint )    
    
   while(@counter <= @rowcount)    
    begin    
     select  @radiologist_id        = radiologist_id    
     from openxml(@hDoc5,'radiologist/row',2)    
     with    
     (     
      radiologist_id uniqueidentifier,    
      row_id bigint    
     ) xmlTemp where xmlTemp.row_id = @counter    
       
     select @radiologist_name= name from radiologists where id=@radiologist_id     
       
     insert into radiologist_functional_rights_other_radiologist(radiologist_id,other_radiologist_id,created_by,date_created)    
                                         values(@id,@radiologist_id,@user_id,getdate())     
    
     if(@@rowcount=0)    
      begin    
       rollback transaction    
       if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
       if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
       if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
       if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
       if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
       select @error_code='066',@return_status=0,@user_name= @inst_name    
       return 0    
      end    
       
       
     set @counter = @counter + 1    
    end    
  end    
        
 exec common_lock_record_ui    
  @menu_id       = @menu_id,    
  @record_id     = @id,    
  @user_id       = @user_id,    
  @error_code    = @error_code output,    
  @return_status = @return_status output    
    
 if(@return_status=0)    
  begin    
   rollback transaction    
   return 0    
  end    
    
 commit transaction    
 if(@xml_fn_rights is not null) exec sp_xml_removedocument @hDoc1    
 if(@xml_modality is not null) exec sp_xml_removedocument @hDoc2    
 if(@xml_institution is not null) exec sp_xml_removedocument @hDoc3    
 if(@xml_study_types is not null) exec sp_xml_removedocument @hDoc4    
 if(@xml_radiologist is not null) exec sp_xml_removedocument @hDoc5    
 set @return_status=1    
 set @error_code='034'    
 set nocount off    
    
 return 1    
end    
    
GO
