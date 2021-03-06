USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[master_institution_save]
GO
/****** Object:  StoredProcedure [dbo].[master_institution_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************  
*******************************************************  
** Version  : 1.0.0.0  
** Procedure    : master_institution_save : save  
                  institution   
** Created By   : Pavel Guha  
** Created On   : 23/04/2019  

** Modified By   : Amritesh Maity  
** Modified On   : 04/06/2021
*******************************************************/  
/*  
  
 exec master_institution_save  
 '55eeda7b-a02a-4297-8fa3-c9177ff87c78','VIMG','VETTECH IMAGING INC','kdoyle@vcradiology.com','','','Florida','',3930,231,'','','Kevin Doyle','','asdsd','Y',  
 '<device><row><device_id>6b647e10-01de-4494-8c54-b8e2f4672094</device_id><manufacturer><![CDATA[GE Healthcare]]></manufacturer><modality><![CDATA[]]></modality><modality_ae_title><![CDATA[]]></modality_ae_title><row_id>1</row_id></row></device>',  
 '<physician><row><physician_id>00000000-0000-0000-0000-000000000000</physician_id><physician_fname><![CDATA[Kevin]]></physician_fname><physician_lname><![CDATA[Doyle]]></physician_lname><physician_credentials><![CDATA[MD]]></physician_credentials><user_l
ogin_id><![CDATA[kdoyle@vcradiology.com]]></user_login_id><physician_email><![CDATA[amiguy05@yahoo.com]]></physician_email><physician_mobile><![CDATA[+16065719566]]></physician_mobile><user_pacs_user_id><![CDATA[Doylek]]></user_pacs_user_id><user_pacs_pas
sword><![CDATA[9UnEauERhsE=]]></user_pacs_password><row_id>1</row_id></row><row><physician_id>1e8c2dad-6e93-489e-b89a-77605e7c2a38</physician_id><physician_fname><![CDATA[Pavel]]></physician_fname><physician_lname><![CDATA[Guha]]></physician_lname><physic
ian_credentials><![CDATA[DVM]]></physician_credentials><user_login_id><![CDATA[pavelguha@gmail.com]]></user_login_id><physician_email><![CDATA[pavelguha@gmail.com]]></physician_email><physician_mobile><![CDATA[+919831352184]]></physician_mobile><user_pacs
_user_id><![CDATA[Doylek]]></user_pacs_user_id><user_pacs_password><![CDATA[9UnEauERhsE=]]></user_pacs_password><row_id>2</row_id></row></physician>',  
 '11111111-1111-1111-1111-111111111111',18,'','',0  
*/  
CREATE procedure [dbo].[master_institution_save]  
(  
 @id        uniqueidentifier='00000000-0000-0000-0000-000000000000' output,  
 @code                     nvarchar(5)     = '' output,  
 @name                nvarchar(100)   = '',  
 @email_id      nvarchar(50)   = '',  
 @address_Line1     nvarchar(100)   = '',  
 @address_Line2     nvarchar(100)   = '',  
 @city       nvarchar(100)   = '',  
 @zip           nvarchar(20)   = '',  
 @state_id      int     = 0,  
 @country_id      int     = 0,  
 @phone       nvarchar(30)   = '',  
 @mobile       nvarchar(30)   = '',  
 @contact_person_name   nvarchar(100)   = '',  
 @contact_person_mob    nvarchar(100)   = '',  
 @notes                    nvarchar(250)   = '',  
 @salesperson_id           uniqueidentifier='00000000-0000-0000-0000-000000000000',  
 @commission_1st_yr        decimal(5,2)    = 0,-- Added on 4th SEP 2019 @BK  
 @commission_2nd_yr        decimal(5,2)    = 0,-- Added on 4th SEP 2019 @BK  
 @discount_per             decimal(5,2)    = 0,  
 @business_source_id       int             = 0,  
 @accountant_name    nvarchar(250)   = '',-- Added on 3rd SEP 2019 @BK  
 @link_existing_bill_acct  nchar(1)    = 'N',  
 @billing_account_id       uniqueidentifier='00000000-0000-0000-0000-000000000000',  
 @format_dcm_files         nchar(1)    = 'N',  
 @dcm_file_xfer_pacs_mode  nchar(1)        = 'A',  
 @study_img_manual_receive_path  nvarchar(250)   = '',  
 @consult_applicable       nchar(1)        = 'N',  
 @storage_applicable       nchar(1)        = 'N',  
 @custom_report            nchar(1)        = 'N',  
 @logo_img                 image           = null,  
 @image_content_type       nvarchar(20)    = null,  
 @xfer_files_compress      nchar(1)    = 'N',  
 @fax_rpt                  nchar(1)        = 'N',  
 @fax_no                   nvarchar(30)    = '',  
 @rpt_format      nchar(1)    = 'P',  
 @is_active      nchar(1)    = 'Y',  
 @xml_device               ntext           = null,  
 @xml_physician            ntext           = null,   
 @xml_user                 ntext           = null,  
 @xml_tags                 ntext           = null, 
 @xml_inst                 ntext           = null,  
 --@xml_fees               ntext           = null,  
 @updated_by               uniqueidentifier,  
 @menu_id                  int,  
 @user_name                nvarchar(700)   = '' output,  
 @error_code      nvarchar(10)   = '' output,  
 @return_status     int     = 0  output,
 @xml_ins_category         ntext           = null

)  
as  
begin  
 set nocount on   
   
   declare @hDoc1 int,  
		   @hDoc2 int,  
		   @hDoc3 int,  
		   @hDoc4 int,  
		   @hDoc5 int,  
		   @hDoc6 int,
		   @hDoc7 int,
		   @counter bigint,  
		   @rowcount bigint,  
		   @last_code_id int,  
		   @physician_code nvarchar(10),  
		   @salesperson_code nvarchar(10),  
		   @user_role_id int,  
		   @old_billing_account_id uniqueidentifier,  
		   @billing_account_code nvarchar(5),  
		   @billing_account_name nvarchar(100)  
  
   declare @device_id uniqueidentifier,  
		   @manufacturer nvarchar(200),  
		   @modality nvarchar(50),  
		   @modality_ae_title nvarchar(50),  
		   @weight_uom nvarchar(10)  
  
   declare @physician_id uniqueidentifier,  
		  @physician_fname nvarchar(80),  
		  @physician_lname nvarchar(80),  
		  @physician_credentials nvarchar(30),  
		  @physician_name nvarchar(200),  
		  @physician_email nvarchar(500),  
		  @physician_mobile nvarchar(500)  
  
   declare  @user_code nvarchar(10),  
		    @user_login_id nvarchar(50),  
            @user_pwd nvarchar(50),  
            @user_pacs_user_id nvarchar(20),  
		    @user_pacs_password nvarchar(200),  
		    @user_user_id uniqueidentifier,  
		  @user_email_id nvarchar(50),  
		  @user_contact_no nvarchar(20),  
		  @is_user_active nchar(1),  
		  @updated_in_pacs nchar(1),  
		  @old_user_pacs_user_id nvarchar(20),  
		  @old_user_pacs_password nvarchar(200)  
      
   declare @rate_id uniqueidentifier,  
		   @fee_amount money,  
		   @fee_row_id int  
  
   declare @group_id nvarchar(5),  
		   @element_id nvarchar(5),  
		   @default_value nvarchar(250),  
		   @junk_characters nvarchar(100)  
  
 declare @inst_id uniqueidentifier,  
         @inst_name nvarchar(100)  ,
		 @study_count int,
		 @balance money
  
 declare @category_id int

 declare @cd int,  
         @new_code_var nvarchar(8)  
   
 if(isnull(@code,'')= '')  
  begin  
   select @cd = max(convert(int,code)) from institutions  
   set @cd = isnull(@cd,0) + 1  
   select @code=replicate('0',5-len(convert(varchar,@cd)))+convert(varchar,@cd)  
  end  
  
 if(select count(id) from institutions where upper(code) = @code and id<>@id)>0  
  begin  
    select @error_code='074',@return_status=0,@user_name=@name  
    return 0  
  end  
  
 if(select count(id) from institutions where upper(name) = @name and is_active='Y' and id<>@id and isnull(code,'')<>'')>0  
  begin  
    select @error_code='136',@return_status=0,@user_name=@name  
    return 0  
  end  
  
 begin transaction  
 if(@xml_device is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_device   
 if(@xml_physician is not null) exec sp_xml_preparedocument @hDoc2 output,@xml_physician   
 if(@xml_user is not null) exec sp_xml_preparedocument @hDoc3 output,@xml_user   
 --if(@xml_fees is not null) exec sp_xml_preparedocument @hDoc4 output,@xml_fees   
 if(@xml_tags is not null) exec sp_xml_preparedocument @hDoc5 output,@xml_tags   
 if(@xml_inst is not null) exec sp_xml_preparedocument @hDoc6 output,@xml_inst 
 if(@xml_ins_category is not null) exec sp_xml_preparedocument @hDoc7 output,@xml_ins_category 
  
 if(@id = '00000000-0000-0000-0000-000000000000')  
	  begin  
		   set @id =newid()  
		   insert into institutions  
			  (  
			   id,code,name,address_1,address_2,city,state_id,country_id,zip,  
			   email_id,phone_no,mobile_no,contact_person_name,contact_person_mobile,notes,  
			   discount_per,accountant_name,-- Added on 3rd SEP 2019 @BK  
			   business_source_id,format_dcm_files,dcm_file_xfer_pacs_mode,study_img_manual_receive_path,  
			   consult_applicable,storage_applicable,custom_report,logo_img,image_content_type,xfer_files_compress,  
			   fax_rpt,fax_no,rpt_format,is_active,is_new,created_by,date_created  
  
			  )  
			 values  
			  (  
			   @id,@code,@name,  
			   @address_Line1,@address_Line2,@city,@state_id,@country_id,@zip,  
			   @email_id,@phone,@mobile,@contact_person_name,@contact_person_mob,@notes,  
			   @discount_per,@accountant_name,-- Added on 3rd SEP 2019 @BK  
			   @business_source_id,@format_dcm_files,@dcm_file_xfer_pacs_mode,@study_img_manual_receive_path,  
			   @consult_applicable,@storage_applicable,@custom_report,@logo_img,@image_content_type,@xfer_files_compress,  
			   @fax_rpt,@fax_no,@rpt_format,@is_active,'N',@updated_by,getdate()  
			  )  
  
		   if(@@rowcount=0)  
				begin  
					 rollback transaction  
					 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
					 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
					 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
					 if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
					 if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6
					 if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
					 select @return_status=0,@error_code='035'  
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
					 rollback transaction  
					 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
					 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
					 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
					 if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
					 if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6
					 if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
 
					 return 0  
				end  
  
			update institutions  
			set code                          = @code,  
				name                          = @name,  
				email_id                      = @email_id,  
				address_1                     = @address_Line1,  
				address_2                     = @address_Line2,  
				city                          = @city,  
				state_id                      = @state_id,  
				country_id                    = @country_id,  
				zip                           = @zip,  
				phone_no                      = @phone,  
				mobile_no                     = @mobile,  
				contact_person_name           = @contact_person_name,  
				contact_person_mobile         = @contact_person_mob,  
				notes                         = @notes,  
				discount_per                  = @discount_per,  
				accountant_name               = @accountant_name,-- Added on 3rd SEP 2019 @BK  
				discount_updated_by           = @updated_by,  
				discount_updated_on           = getdate(),  
				business_source_id            = @business_source_id,  
				format_dcm_files              = @format_dcm_files,  
				dcm_file_xfer_pacs_mode       = @dcm_file_xfer_pacs_mode,  
				study_img_manual_receive_path = @study_img_manual_receive_path,  
				consult_applicable            = @consult_applicable,  
				storage_applicable            = @storage_applicable,  
				custom_report                 = @custom_report,  
				logo_img                      = @logo_img,  
				image_content_type            = @image_content_type,  
				xfer_files_compress           = @xfer_files_compress,  
				fax_rpt                       = @fax_rpt,  
				fax_no                        = @fax_no,  
				rpt_format                    = @rpt_format,  
				is_active                     = @is_active,  
				is_new                        = 'N',  
				updated_by                    = @updated_by,  
				date_updated                  = getdate()  
			where id = @id  
  
		   if(@@rowcount=0)  
				begin  
					 rollback transaction  
					 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
					 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
					 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
					 if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
					 if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6
					 if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
					 select @return_status=0,@error_code='035'  
					 return 0  
				end  
      end  
  
  
 if(@xml_device is not null)  
  begin  
   set @counter = 1  
   select  @rowcount=count(row_id)    
   from openxml(@hDoc1,'device/row', 2)    
   with( row_id bigint )  
  
   while(@counter <= @rowcount)  
    begin  
     select  @device_id         = device_id,  
       @manufacturer      = manufacturer,  
       @modality          = modality,  
       @modality_ae_title = modality_ae_title,  
       @weight_uom     = weight_uom  
     from openxml(@hDoc1,'device/row',2)  
     with  
     (   
      device_id uniqueidentifier,  
      manufacturer nvarchar(200),  
      modality nvarchar(50),  
      modality_ae_title nvarchar(50),  
      weight_uom nvarchar(10),  
      row_id bigint  
     ) xmlTemp where xmlTemp.row_id = @counter    
     
     if(@device_id = '00000000-0000-0000-0000-000000000000')  
      begin  
        if(rtrim(ltrim(@manufacturer))<>'')  
         begin  
           insert into institution_device_link(device_id,institution_id,manufacturer,modality,modality_ae_title,weight_uom,  
                    created_by,date_created)  
                    values(newid(),@id,@manufacturer,@modality,@modality_ae_title,@weight_uom,  
                     @updated_by,getdate())  
                                                     
           if(@@rowcount=0)  
            begin  
             rollback transaction  
             if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
             if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
             if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
             if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
             if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6
	         if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
             select @error_code='066',@return_status=0,@user_name=@manufacturer + ' ' + @modality + ' ' + @modality_ae_title+ ' ' + @weight_uom  
             return 0  
            end  
          end  
      end  
     else  
      begin  
       update institution_device_link  
       set    manufacturer          = @manufacturer,  
           modality              = @modality,  
           modality_ae_title     = @modality_ae_title,  
           weight_uom    = @weight_uom,  
           updated_by            = @updated_by,  
           date_updated          = getdate()  
       where device_id=@device_id   
       and institution_id = @id  
          
       if(@@rowcount=0)  
        begin  
         rollback transaction  
         if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
            if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
         if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
         if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
         if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6
		 if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
         select @error_code='066',@return_status=0,@user_name=@manufacturer + ' ' + @modality + ' ' + @modality_ae_title+ ' ' + @weight_uom  
         return 0  
        end  
      end  
     set @counter = @counter + 1  
    end  
  end  
  
 delete from institution_device_link  
 where institution_id = @id  
 and device_id not in (select  device_id    
          from openxml(@hDoc1,'device/row', 2)    
         with( device_id uniqueidentifier,  
               row_id bigint ))  
  
 delete from institution_physician_link where institution_id = @id  
  
 if(@xml_physician is not null)  
  begin  
   set @counter = 1  
   select  @rowcount=count(row_id)    
   from openxml(@hDoc2,'physician/row', 2)    
   with( row_id bigint )  
  
   while(@counter <= @rowcount)  
    begin  
     select  @physician_id            = physician_id,  
             @physician_fname         = physician_fname,  
       @physician_lname         = physician_lname,  
       @physician_credentials   = physician_credentials,  
       @physician_email         = physician_email,  
       @physician_mobile        = physician_mobile  
     from openxml(@hDoc2,'physician/row',2)  
     with  
     (   
      physician_id uniqueidentifier,  
      physician_fname nvarchar(80),  
      physician_lname nvarchar(80),  
      physician_credentials nvarchar(30),  
      physician_email nvarchar(500),  
      physician_mobile nvarchar(500),  
      row_id bigint  
     ) xmlTemp where xmlTemp.row_id = @counter    
  
     select @physician_name =  rtrim(ltrim(rtrim(ltrim(@physician_fname)) + ' ' + rtrim(ltrim(@physician_lname)) + ' ' + rtrim(ltrim(@physician_credentials))))   
  
     if(@physician_id <> '00000000-0000-0000-0000-000000000000')  
      begin  
       select @physician_code = code  
       from physicians  
       where id = @physician_id  
  
       insert into institution_physician_link(physician_id,institution_id,physician_fname,physician_lname,physician_credentials,physician_name,  
                 physician_email,physician_mobile,created_by,date_created)  
               values(@physician_id,@id,@physician_fname,@physician_lname,@physician_credentials,@physician_name,  
                    @physician_email,@physician_mobile,@updated_by,getdate())  
                                                     
       if(@@rowcount=0)  
        begin  
         rollback transaction  
         if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
         if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
         if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
         if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
         if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6
         if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
         select @error_code='066',@return_status=0,@user_name=@physician_name  
         return 0  
        end  
  
       update physicians  
       set fname          = @physician_fname,  
        lname          = @physician_lname,  
        credentials    = @physician_credentials,     
        name           = @physician_name,  
        email_id       = isnull(@physician_email,''),  
        institution_id = @id,  
        updated_by     = @updated_by,  
        date_updated   = getdate()  
       where id = @physician_id   
  
       if(@@rowcount=0)  
        begin  
         rollback transaction  
         if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
         if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
         if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
         if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
         if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
         if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
        select @error_code='066',@return_status=0,@user_name=@physician_name  
         return 0  
        end  
  
       --update institution_physician_link  
       --set physician_fname         = @physician_fname,  
       -- physician_lname         = @physician_lname,  
       -- physician_credentials   = @physician_credentials,     
       -- physician_name          = @physician_name,  
       -- physician_email         = isnull(@physician_email,''),  
       -- physician_mobile        = isnull(@physician_mobile,''),  
       -- billing_account_id      = isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000'),  
       -- updated_by              = @updated_by,  
       -- date_updated            = getdate()  
       --where physician_id = @physician_id    
       --and institution_id = @id  
         
       --if(@@rowcount=0)  
       -- begin  
       --  rollback transaction  
       --  if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
       --  if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
       --  if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
       --  if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
       --  select @error_code='066',@return_status=0,@user_name=@physician_name  
       --  return 0  
       -- end  
      end  
     else  
      begin  
        if(select count(physician_id) from institution_physician_link where upper(physician_name) = upper(@physician_name) and institution_id=@id)>0  
         begin  
          rollback transaction  
          if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
          if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
          if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
          if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
          if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
          if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7 
       select @error_code='261',@return_status=0,@user_name=@physician_name  
          return 0  
         end  
         
        set @physician_id= newid()  
  
        insert into institution_physician_link(physician_id,institution_id,physician_fname,physician_lname,physician_credentials,physician_name,  
                                             physician_email,physician_mobile,created_by,date_created)  
                values(@physician_id,@id,@physician_fname,@physician_lname,@physician_credentials,@physician_name,  
                    @physician_email,@physician_mobile,@updated_by,getdate())  
                                                     
        if(@@rowcount=0)  
         begin  
          rollback transaction  
          if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
          if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
          if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
          if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
          if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
          if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
        select @error_code='066',@return_status=0,@user_name=@physician_name  
          return 0  
         end  
  
        select @last_code_id =max(convert(int,substring(code,5,len(code)-4)))   
        from physicians  
  
        set @last_code_id = isnull(@last_code_id,0) + 1  
        set @physician_code = 'PHYS' + convert(varchar,@last_code_id)  
           
        insert into physicians(id,code,fname,lname,credentials,name,institution_id,  
                               email_id,mobile_no,created_by,date_created)   
                     values (@physician_id,@physician_code,@physician_fname,@physician_lname,@physician_credentials,@physician_name,@id,  
                    @physician_email,@physician_mobile,@updated_by,getdate())  
  
        if(@@rowcount=0)  
         begin  
          rollback transaction  
          if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
          if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
          if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
          if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
          if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
          if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
          select @error_code='066',@return_status=0,@user_name=@physician_name  
          return 0  
         end  
  
          
      end  
  
     set @counter = @counter + 1  
    end  
  end  
  
 --delete from institution_user_link where institution_id=@id  
  
 if(@xml_user is not null)  
  begin  
   set @counter = 1  
   select  @rowcount=count(row_id)    
   from openxml(@hDoc3,'user/row', 2)    
   with( row_id bigint )  
  
   while(@counter <= @rowcount)  
    begin  
     select  @user_user_id            = user_user_id,  
             @user_login_id           = user_login_id,  
       @user_pwd                = user_pwd,  
       @user_pacs_user_id       = user_pacs_user_id,  
       @user_pacs_password      = user_pacs_password,  
       @user_email_id           = user_email_id,  
       @user_contact_no         = user_contact_no,  
       @is_user_active          = is_active  
     from openxml(@hDoc3,'user/row',2)  
     with  
     (   
      user_user_id uniqueidentifier,  
      user_login_id nvarchar(50),  
      user_pwd nvarchar(200),  
      user_pacs_user_id nvarchar(20),  
      user_pacs_password nvarchar(200),  
      user_email_id nvarchar(50),  
      user_contact_no  nvarchar(20),  
      is_active nchar(1),  
      row_id bigint  
     ) xmlTemp where xmlTemp.row_id = @counter    
  
  
     if(@is_active ='Y')  
      begin  
       if(select count(id) from users where upper(login_id) = upper(@user_login_id) and id<>@user_user_id)>0  
        begin  
          rollback transaction  
          if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
          if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
          if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
          if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
          if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
          if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
          select @error_code='118',@return_status=0,@user_name=Convert(varchar,@counter)  
          return 0  
        end  
  
       if(select count(user_login_id)   
          from institution_user_link   
          where upper(user_login_id) = upper(@user_login_id)   
          and user_id <> @user_user_id   
          and institution_id=@id)>0  
        begin  
          rollback transaction  
          if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
          if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
          if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
          if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
          if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
          if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
          select @error_code='114',@return_status=0,@user_name=Convert(varchar,@counter)  
          return 0  
        end  
      end  
     else  
      begin  
       set @is_user_active='N'  
      end  
       
  
     if(@user_user_id <> '00000000-0000-0000-0000-000000000000')  
      begin  
       select @user_code = code  
       from users  
       where id = @user_user_id  
  
       set @updated_in_pacs='Y'  
  
       select @old_user_pacs_user_id  = user_pacs_user_id,  
              @old_user_pacs_password = user_pacs_password  
       from institution_user_link  
       where user_id = @user_user_id  
       and institution_id = @id  
  
       if(@old_user_pacs_user_id <> @user_pacs_user_id)  
        begin  
         set @updated_in_pacs ='N'  
        end  
       if(@old_user_pacs_password <> @user_pacs_password)  
        begin  
         set @updated_in_pacs ='N'  
        end  
  
  
       if(select count(user_id) from institution_user_link where user_id = @user_user_id and institution_id = @id)=0  
        begin  
         insert into institution_user_link(user_id,institution_id,user_login_id,user_pwd,user_pacs_user_id,user_pacs_password,  
                         user_email,user_contact_no,updated_in_pacs,granted_rights_pacs,created_by,date_created)  
                        values(@user_user_id,@id,@user_login_id,@user_pwd,@user_pacs_user_id,@user_pacs_password,  
                         @user_email_id,@user_contact_no,'N','EOWIN',@updated_by,getdate())  
        end  
       else  
        begin  
         update institution_user_link  
         set user_login_id      = @user_login_id,  
          user_pwd           = @user_pwd,  
          user_pacs_user_id  = @user_pacs_user_id,  
          user_pacs_password = @user_pacs_password,  
          user_email         = @user_email_id,  
          user_contact_no    = @user_contact_no,  
          updated_in_pacs    = @updated_in_pacs,  
          granted_rights_pacs='EOWIN',  
          updated_by         = @updated_by,  
          date_updated       = getdate()  
         where user_id = @user_user_id  
         and institution_id = @id  
        end  
  
       if(@@rowcount=0)  
        begin  
         rollback transaction  
         if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
         if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
         if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
         if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
         if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
         if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
         select @error_code='113',@return_status=0,@user_name=@user_login_id  
         return 0  
        end  
  
       update users  
       set name          = @user_login_id,  
        login_id      = @user_login_id,  
        password      = @user_pwd,  
        pacs_user_id  = @user_pacs_user_id,     
        pacs_password = @user_pacs_password,  
        email_id      = isnull(@user_email_id,''),  
        contact_no    = isnull(@user_contact_no,''),  
        is_active     = @is_user_active,  
        date_updated  = getdate()  
       where id = @user_user_id   
  
       if(@@rowcount=0)  
        begin  
         rollback transaction  
         if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
         if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
         if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
         if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
         if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
         if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
         select @error_code='113',@return_status=0,@user_name=@user_login_id  
         return 0  
        end  
  
         
      end  
     else  
      begin  
           set @user_user_id= newid()  
        insert into institution_user_link(user_id,institution_id,user_login_id,user_pwd,user_pacs_user_id,user_pacs_password,  
                     user_email,user_contact_no,granted_rights_pacs,created_by,date_created)  
               values(@user_user_id,@id,@user_login_id,@user_pwd,@user_pacs_user_id,@user_pacs_password,  
                   @user_email_id,@user_contact_no,'EOWIN',@updated_by,getdate())  
                                                     
        if(@@rowcount=0)  
         begin  
          rollback transaction  
          if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
          if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
          if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
          if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
          if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
          if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
          select @error_code='113',@return_status=0,@user_name=@physician_name  
          return 0  
         end  
  
        select @user_role_id = id  
        from user_roles  
        where code='IU'  
  
        create table #tmpID(id int)  
  
        insert into #tmpID(id)   
        (select convert(int,substring(code,3,len(code)-2))  
         from users   
         where user_role_id = @user_role_id)  
          
        select @last_code_id =max(id)   
        from #tmpID  
  
        set @last_code_id = isnull(@last_code_id,0) + 1  
        set @user_code = 'IU' + convert(varchar,@last_code_id)  
  
        set @last_code_id = isnull(@last_code_id,0) + 1  
        set @user_code = 'IU' + convert(varchar,@last_code_id)  
  
        drop table #tmpID  
           
        insert into users(id,code,name,login_id,password,email_id,contact_no,user_role_id,  
                          pacs_user_id,pacs_password,is_active,is_visible,created_by,date_created)   
             values (@user_user_id,@user_code,@user_login_id,@user_login_id,@user_pwd,@user_email_id,@user_contact_no,@user_role_id,  
                        @user_pacs_user_id,@user_pacs_password,@is_user_active,'Y',@updated_by,getdate())  
  
        if(@@rowcount=0)  
         begin  
          rollback transaction  
          if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
          if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
          if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
          if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
          if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
          if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
          select @error_code='113',@return_status=0,@user_name=@user_login_id  
          return 0  
         end  
  
        insert into user_menu_rights(user_id,menu_id,update_by,date_updated)  
        (select @user_user_id,menu_id,@updated_by,getdate()  
        from user_role_menu_rights  
        where user_role_id = @user_role_id)  
  
        if(@@rowcount=0)  
         begin  
          rollback transaction  
          if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
          if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
          if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
          if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
          if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
          if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
          select @error_code='124',@return_status=0,@user_name=@user_login_id  
          return 0  
         end  
  
          
      end  
  
     set @counter = @counter + 1  
    end  
  end  
  
 --delete from institution_salesperson_link where institution_id = @id  
  
 --if(isnull(@salesperson_id,'00000000-0000-0000-0000-000000000000') != '00000000-0000-0000-0000-000000000000')  
 -- begin  
 --  if(select count(salesperson_id) from institution_salesperson_link where institution_id=@id and salesperson_id=@salesperson_id)=0  
 --   begin  
 --     insert into institution_salesperson_link(salesperson_id,institution_id,salesperson_fname,salesperson_lname,salesperson_name,  
 --                salesperson_login_email,salesperson_email,salesperson_mobile,salesperson_user_id,  
 --                salesperson_pacs_user_id,salesperson_pacs_password,  
 --                commission_1st_yr,commission_2nd_yr,  
 --                created_by,date_created)  
 --             (select sp.id,@id,sp.fname,sp.lname,rtrim(ltrim(isnull(sp.fname,'') + ' ' + isnull(sp.lname,''))),  
 --               sp.email_id,sp.email_id,sp.mobile_no,u.id,  
 --               sp.pacs_user_id,sp.pacs_password,  
 --               @commission_1st_yr,@commission_2nd_yr,  
 --               @updated_by,getdate()  
 --              from salespersons sp  
 --              inner join users u on u.code=sp.code  
 --              where sp.id = @salesperson_id)  
 --      end  
 --  else  
 --   begin  
 --    update institution_salesperson_link  
 --    set salesperson_id    = @salesperson_id,  
 --        salesperson_fname   = isnull((select fname from salespersons where id = @salesperson_id),''),  
 --     salesperson_lname   = isnull((select lname from salespersons where id = @salesperson_id),''),  
 --     salesperson_name   = isnull((select rtrim(ltrim(isnull(fname,'') + ' ' + isnull(lname,''))) from salespersons where id = @salesperson_id),''),  
 --     salesperson_login_email  = isnull((select email_id from salespersons where id = @salesperson_id),''),  
 --     salesperson_email   = isnull((select email_id from salespersons where id = @salesperson_id),''),  
 --     salesperson_mobile   = isnull((select mobile_no from salespersons where id = @salesperson_id),''),  
 --     salesperson_user_id   = (select id from users where code= ( select code from salespersons where id = @salesperson_id)),  
 --     salesperson_pacs_user_id = isnull((select pacs_user_id from salespersons where id = @salesperson_id),''),  
 --     salesperson_pacs_password = isnull((select pacs_password from salespersons where id = @salesperson_id),''),  
 --     commission_1st_yr   = @commission_1st_yr,-- Added on 4th SEP 2019 @BK  
 --     commission_2nd_yr   = @commission_2nd_yr,-- Added on 4th SEP 2019 @BK  
 --     updated_by     = @updated_by,  
 --     date_updated    = getdate()  
 --    where institution_id = @id  
 --   end  
  
 --  if(@@rowcount=0)  
 --   begin  
 --    rollback transaction  
 --    if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
 --    if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
 --    if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
 --    if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
 --    select @error_code='066',@return_status=0  
 --    return 0  
 --   end  
  
 -- end  
  
 --delete from institution_rates_fee_schedule where institution_id=@id  
  
 --if(@xml_fees is not null)  
 -- begin  
 --  set @counter = 1  
 --  select  @rowcount=count(row_id)    
 --  from openxml(@hDoc4,'fees/row', 2)    
 --  with( row_id bigint )  
  
 --  while(@counter <= @rowcount)  
 --   begin  
 --    select  @rate_id      = rate_id,  
 --            @fee_amount   = fee_amount,  
 --      @fee_row_id   = fee_row_id  
 --    from openxml(@hDoc4,'fees/row',2)  
 --    with  
 --    (   
 --     rate_id uniqueidentifier,  
 --     fee_amount money,  
 --     fee_row_id int,  
 --     row_id bigint  
 --    ) xmlTemp where xmlTemp.row_id = @counter    
  
  
 --    insert into institution_rates_fee_schedule(institution_id,rate_id,fee_amount,discount_per,updated_by,date_updated)  
 --                values(@id,@rate_id,@fee_amount,@discount_per,@updated_by,getdate())  
                                                     
  
 --    if(@@rowcount=0)  
 --     begin  
 --      rollback transaction  
 --      if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
 --      if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
 --      if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
 --      if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
 --      select @error_code='113',@return_status=0,@user_name=convert(varchar,@fee_row_id)  
 --      return 0  
 --     end  
       
  
 --    set @counter = @counter + 1  
 --   end  
 -- end  
  
 delete from institution_dispute_dicom_tags where institution_id=@id  
  
  if(@xml_tags is not null)  
	  begin  
		   set @counter = 1  
		   select  @rowcount=count(row_id)    
		   from openxml(@hDoc5,'tag/row', 2)    
		   with( row_id bigint )  
  
		   while(@counter <= @rowcount)  
			begin  
					 select  @group_id         = group_id,  
							 @element_id       = element_id,  
							 @default_value    = default_value,  
							 @junk_characters  = junk_characters  
					 from openxml(@hDoc5,'tag/row',2)  
					 with  
					 (   
					  group_id nvarchar(5),  
					  element_id nvarchar(5),  
					  default_value nvarchar(250),  
					  junk_characters nvarchar(100),  
					  row_id bigint  
					 ) xmlTemp where xmlTemp.row_id = @counter    
  
					 insert into institution_dispute_dicom_tags(institution_id,group_id,element_id,default_value,junk_characters,updated_by,date_updated)  
								 values(@id,@group_id,@element_id,@default_value,@junk_characters,@updated_by,getdate())  
					 if(@@rowcount=0)  
						  begin  
							   rollback transaction  
							   if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
							   if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
							   if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
							   if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
							   if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
							   select @error_code='247',@return_status=0,@user_name= '(' + @group_id + ',' + @element_id + ')'  
							   return 0  
						  end  
  
					 set @counter = @counter + 1  
			end  
	  end  

  --Institution Category Link
  delete from institution_category_link where institution_id=@id  
  
    if(@xml_ins_category is not null)  
		begin  
			set @counter = 1  
			select  @rowcount=count(row_id)    
			from openxml(@hDoc7,'inst_category/row', 2)    
			with( row_id bigint )  
  
			while(@counter <= @rowcount)  
				begin  
					select  @category_id         = category_id,  
							@inst_id       = institution_id  
					from openxml(@hDoc7,'inst_category/row',2)  
					with  
					(   
					category_id int,  
					institution_id nvarchar(36),  
					row_id bigint  
					) xmlTemp where xmlTemp.row_id = @counter    
  
					insert into institution_category_link(institution_id,category_id,created_by,date_created)  
								values(@inst_id,@category_id,@updated_by,getdate())  
					if(@@rowcount=0)  
					begin  
					rollback transaction  
					if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
					if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
					if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
					if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
					if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
					select @error_code='247',@return_status=0,@user_name= '(' + @group_id + ',' + @element_id + ')'  
					return 0  
					end  
  
					set @counter = @counter + 1  
				end  
		end 
   
 delete from institution_alt_name_link where institution_id=@id  
 if(@xml_inst is not null)  
  begin  
  
   set @counter = 1  
   select  @rowcount=count(row_id)    
   from openxml(@hDoc6,'institution/row', 2)    
   with( row_id bigint )  
  
   while(@counter <= @rowcount)  
    begin  
     select  @inst_id         = inst_id,  
             @inst_name       = inst_name  
     from openxml(@hDoc6,'institution/row',2)  
     with  
     (   
      inst_id uniqueidentifier,  
      inst_name nvarchar(100),  
      row_id bigint  
     ) xmlTemp where xmlTemp.row_id = @counter    
  
     insert into institution_alt_name_link(institution_id,alternate_name,updated_by,date_updated)  
                  values(@id,@inst_name,@updated_by,getdate())  
  
     if(@@rowcount=0)  
      begin  
       rollback transaction  
       if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
       if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
       if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
       if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
       if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
	   if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
       select @error_code='397',@return_status=0,@user_name= @inst_name  
       return 0  
      end  
  
     if(@inst_id <> @id)  
      begin  
       update institution_physician_link set institution_id  = @id where institution_id=@inst_id  
       update institution_device_link set institution_id  = @id where institution_id=@inst_id  
       update institution_user_link set institution_id  = @id where institution_id=@inst_id  
       update study_hdr set institution_id = @id where institution_id=@inst_id  
       update study_hdr_archive set institution_id = @id where institution_id=@inst_id  
  
       delete from institutions where id = @inst_id  
  
       if(@@rowcount=0)  
       begin  
        rollback transaction  
        if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
        if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
        if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
        if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
        if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
		if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
        select @error_code='393',@return_status=0,@user_name= @inst_name  
        return 0  
       end  
      end  
  
       
       
  
     set @counter = @counter + 1  
    end  
  
    
  
     
  
     
  
     
  end  
 
 select @old_billing_account_id = billing_account_id  
 from billing_account_institution_link  
 where institution_id = @id  

 set @old_billing_account_id = isnull(@old_billing_account_id,'00000000-0000-0000-0000-000000000000') 

 if(@is_active = 'Y')  
	  begin  

			if(@old_billing_account_id <> '00000000-0000-0000-0000-000000000000')
				begin
				   if(@old_billing_account_id <> @billing_account_id)
						begin
							if(select count(iid.study_id)
							   from invoice_institution_dtls iid
							   inner join invoice_hdr ih on ih.id = iid.hdr_id
							   where iid.institution_id=@id
							   and iid.billing_account_id = @old_billing_account_id)>0
									begin
										  rollback transaction  
										  if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
										  if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
										  if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
										  if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
										  if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
										  if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7  
										  select @user_name = name from billing_account where id = @old_billing_account_id
										  select @error_code='501',@return_status=0
										  return 0  
									end
						end
				end

		   if(@link_existing_bill_acct ='Y')  
			begin  
			 if(@old_billing_account_id <> @billing_account_id)  
			  begin  
			   if(@old_billing_account_id <> '00000000-0000-0000-0000-000000000000')  
				begin  
				 delete from billing_account_institution_link  
				 where institution_id=@id  
				 and billing_account_id = @old_billing_account_id  
  
				 if(@@rowcount =0)  
				  begin  
				   rollback transaction  
				   if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
				   if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
				   if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
				   select @error_code='222',@return_status=0  
				   return 0  
				  end  
  
				 delete from billing_account_contacts  
				 where institution_id=@id  
				 and billing_account_id = @old_billing_account_id  
  
				 if(@@rowcount =0)  
				  begin  
				   rollback transaction  
				   if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
				   if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
				   if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
				   select @error_code='222',@return_status=0  
				   return 0  
				  end  
				 end  
  
			   insert into billing_account_institution_link(billing_account_id,institution_id,updated_by,date_updated)  
							 values(@billing_account_id,@id,@updated_by,getdate())  
  
			   if(@@rowcount =0)  
				begin  
				 rollback transaction  
				 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
				 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
				 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
				 select @error_code='222',@return_status=0  
				 return 0  
				end  
  
         
			  end  
  
			 delete from billing_account_physicians  
			 where institution_id=@id  
			 and billing_account_id = @old_billing_account_id  
  
           
			 if(@xml_physician is not null)  
			  begin  
			   insert into billing_account_physicians(billing_account_id,institution_id,physician_id,updated_by,date_updated)  
									(select @billing_account_id,@id,physician_id,@updated_by,getdate()  
				from institution_physician_link  
				where institution_id = @id)  
  
				if(@@rowcount =0)  
				begin  
				 rollback transaction  
				 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
				 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
				 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
				 select @error_code='222',@return_status=0  
				 return 0  
				end  
			  end  
			end  
		   else if(@link_existing_bill_acct ='N')  
			begin   
			 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 	 if(@old_billing_account_id <> '00000000-0000-0000-0000-000000000000')  
		  begin  
		   delete from billing_account_institution_link  
		   where institution_id=@id  
		   and billing_account_id = @old_billing_account_id  
  
		   if(@@rowcount =0)  
			begin  
			 rollback transaction  
			 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
			 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
			 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
			 select @error_code='222',@return_status=0  
			 return 0  
			end  
  
		   delete from billing_account_physicians  
		   where institution_id=@id  
		   and billing_account_id = @old_billing_account_id  
  
		   if(@@rowcount =0)  
			begin  
			 rollback transaction  
			 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
			 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
			 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
			 select @error_code='222',@return_status=0  
			 return 0  
			end  
  
		   delete from billing_account_contacts  
		   where institution_id=@id  
		   and billing_account_id = @old_billing_account_id  
  
		   if(@@rowcount =0)  
			begin  
			 rollback transaction  
			 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
			 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
			 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
			 select @error_code='222',@return_status=0  
			 return 0  
			end  
  
		   if(select count(institution_id) from billing_account_institution_link)=0  
			begin  
			 delete from billing_account_rates_fee_schedule  
			 where billing_account_id=@billing_account_id  
			end  
		  end   
  
			 if(select count(id) from billing_account where name = @name)>0  
			  begin  
			   select @billing_account_id   = id,  
					  @billing_account_code = code,  
				   @billing_account_name = name  
			   from billing_account   
			   where name = @name  
			  end  
			 else  
			  begin  
			   set @billing_account_id =newid()  
			   select @cd = max(convert(int,code)) from billing_account  
			   set @cd = isnull(@cd,0) + 1  
			   select @billing_account_code=replicate('0',5-len(convert(varchar,@cd)))+convert(varchar,@cd)  
  
			   if(len(rtrim(ltrim(@name)))>41)   
				begin  
				 set @billing_account_name = substring(@name,1,41)  
				end  
			   else  
				begin  
				 set @billing_account_name = rtrim(ltrim(@name))  
				end  
			  end  
  
			 insert into billing_account  
			  (  
			   id,code,name,qb_name,address_1,address_2,city,state_id,country_id,zip,  
			   login_id,login_pwd,user_email_id,user_mobile_no,notification_pref,  
			   is_active,is_new,update_qb,created_by,date_created  
  
			  )  
			 values  
			  (  
			   @billing_account_id,@billing_account_code,@billing_account_name,@billing_account_name,@address_Line1,@address_Line2,@city,@state_id,@country_id,@zip,  
			   '','','','','B',  
			   @is_active,'Y','Y',@updated_by,getdate()  
			  )  
  
			 if(@@rowcount =0)  
			  begin  
			   rollback transaction  
			   if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
			   if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
			   if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
			   select @error_code='224',@return_status=0  
			   return 0  
			  end  
  
			 insert into billing_account_institution_link(billing_account_id,institution_id,updated_by,date_updated)  
												   values(@billing_account_id,@id,@updated_by,getdate())  
  
			 if(@@rowcount =0)  
			  begin  
			   rollback transaction  
			   if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
			   if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
			   if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
			   select @error_code='224',@return_status=0  
			   return 0  
			  end  
  
			 if(@xml_physician is not null)  
			  begin  
			   insert into billing_account_physicians(billing_account_id,institution_id,physician_id,updated_by,date_updated)  
									(select @billing_account_id,@id,physician_id,@updated_by,getdate()  
				from institution_physician_link  
				where institution_id = @id)  
  
			   if(@@rowcount =0)  
				begin  
				 rollback transaction  
				 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
				 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
				 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
				 select @error_code='222',@return_status=0  
				 return 0  
				end  
			  end  
  
  
			 insert into billing_account_rates_fee_schedule(billing_account_id,rate_id,fee_amount,discount_per,updated_by,date_updated)  
													(select @billing_account_id,id,fee_amount,0,@updated_by,getdate()  
						  from rates_fee_schedule_template)  
			end  
  
     
		   --update contacts  
		    if(select count(institution_id) from billing_account_contacts where institution_id=@id and billing_account_id=@billing_account_id)=0  
		begin  
		 insert into billing_account_contacts (billing_account_id,institution_id,phone_no,fax_no,contact_person_name,contact_person_mobile,contact_person_email_id,  
					updated_by,date_updated)  
				   values(@billing_account_id,@id,isnull(@phone,''),isnull(@mobile,''),isnull(@contact_person_name,''),isnull(@contact_person_mob,''), isnull(@email_id,''),  
					@updated_by,getdate())  
                 
		end  
	   else  
		begin  
		 update billing_account_contacts  
		 set  phone_no                = isnull(@phone,''),  
			  fax_no                  = isnull(@mobile,''),  
		   contact_person_name     = isnull(@contact_person_name,''),  
		   contact_person_mobile   = isnull(@contact_person_mob,''),  
		   contact_person_email_id = isnull(@email_id,''),  
		   updated_by              = @updated_by,  
		   date_updated            = getdate()   
		 where billing_account_id = @billing_account_id  
		 and institution_id = @id    
		end  
  
		   if(@@rowcount =0)  
			begin  
			 rollback transaction  
			 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
			 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
			 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
			 select @error_code='222',@return_status=0  
			 return 0  
			end  
  
		   update institutions  
		   set link_existing_bill_acct='Y',  
			billing_account_id = @billing_account_id  
		   where id =@id  
  
		   if(@@rowcount =0)  
			begin  
			 rollback transaction  
			 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
			 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
			 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
			 select @error_code='222',@return_status=0  
			 return 0  
			end  
  
	  end  
 else  
	  begin  
		   select @study_count = count(id) from study_hdr where invoiced='N' and institution_id = @id
		   select @study_count = @study_count + (select count(id) from study_hdr_archive where invoiced='N' and institution_id =@id)

		   if(@study_count)>0
				begin
					rollback transaction
					if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
					if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
					if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
					if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
					if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
					select @error_code='500',@return_status=0,@user_name=convert(varchar(10),@study_count)
					return 0
				end

		    --check out standing invoices
			select @balance  =  a.total_amount-sum(a.adjusted) 
			from (
					select 'O' adj_source, hdr.id,hdr.invoice_no,hdr.opbal_date invoice_date,billing_cycle_id='00000000-0000-0000-0000-000000000000',billing_cycle='Opening Balance',hdr.opbal_amount total_amount, isnull(aj.adj_amount,0) adjusted 
					from ar_opening_balance hdr with(nolock) 
					left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id 
					left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
					where ba.id = @old_billing_account_id
					UNION ALL
					select 'I' adj_source, hdr.id,hdr.invoice_no,hdr.invoice_date,hdr.billing_cycle_id,billing_cycle=bc.name,hdr.total_amount, isnull(aj.adj_amount,0) adjusted 
					from invoice_hdr hdr with(nolock) 
					left join ar_payments_adj aj with(nolock) on hdr.id=aj.invoice_header_id --and ISNULL(aj.adj_source,'I')='I'
					left join billing_account ba with(nolock) on ba.id=hdr.billing_account_id
					inner join billing_cycle bc on bc.id = hdr.billing_cycle_id
					where ba.id = @old_billing_account_id
					and isnull(aj.adj_amount,0)>=0
					and hdr.approved='Y')a
			group by a.id, a.invoice_no, a.invoice_date,a.billing_cycle_id,a.billing_cycle,a.total_amount
			having a.total_amount-sum(a.adjusted)>0
			order by a.invoice_date

			  if(@balance > 0)
				begin
					rollback transaction
					if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
					if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
					if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
					if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
					if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
					select @error_code='499',@return_status=0,@user_name=convert(varchar(15),round(@balance,2))
					return 0
				end

		   if(select count(institution_id) from billing_account_institution_link  where institution_id=@id)>0  
				begin  
					 delete from billing_account_institution_link  
					 where institution_id=@id  
  
					 if(@@rowcount =0)  
						  begin  
							   rollback transaction  
							   if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
							   if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
							   if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
							   select @error_code='222',@return_status=0  
							   return 0  
						  end  
  
					 delete from billing_account_physicians  
					 where institution_id=@id  
  
					 delete from billing_account_contacts  
					 where institution_id=@id  
  
					 if(select count(institution_id) from billing_account_institution_link)=0  
						  begin  
							   delete from billing_account_rates_fee_schedule  
							   where billing_account_id=@billing_account_id  
						  end  
  
				end  
	  end  
   
 commit transaction  
 if(@xml_device is not null) exec sp_xml_removedocument @hDoc1  
 if(@xml_physician is not null) exec sp_xml_removedocument @hDoc2  
 if(@xml_user is not null) exec sp_xml_removedocument @hDoc3  
 if(@xml_tags is not null) exec sp_xml_removedocument @hDoc5  
 if(@xml_inst is not null) exec sp_xml_removedocument @hDoc6  
 if(@xml_ins_category is not null) exec sp_xml_removedocument @hDoc7
 set @return_status=1  
 set @error_code='034'  
 set nocount off  
  
 return 1  
end  
  
  
GO
