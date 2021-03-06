USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_settings_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_settings_save]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_settings_save]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************              
*******************************************************              
** Version  : 1.0.0.0              
** Procedure    : dashboard_settings_save : save dashboard settings save              
** Created By   : AM               
** Created On   : 14/06/2021            
*******************************************************/
CREATE Procedure [dbo].[dashboard_settings_save]
    @xml_data ntext,
    @xml_data_aging ntext,
    @menu_id int,
    @updated_by uniqueidentifier,
    @user_name nvarchar(500) = '' output,
    @error_code nvarchar(10) = '' output,
    @return_status int = 0 output
As
Begin
    set nocount on
    set datefirst 1

    declare @hDoc int,
            @hDoc2 int,
            @counter int,
            @rowcount int

    declare @id int,
            @parent_id int,
            @menu_desc nvarchar(150),
            @nav_url nvarchar(150),
            @icon nvarchar(150),
            @display_index int,
            @refresh_time int,
            @is_enabled varchar(1),
            @is_default varchar(1),
            @is_refresh_button varchar(1),
            @title nvarchar(250)

    declare @aging_id int,
            @dashboard_menu_id int,
            @key nvarchar(100),
            @slot_count int,
            @slot_1 int,
            @slot_2 int,
            @slot_3 int

    exec common_check_record_lock @menu_id = @menu_id,
                                  @record_id = @menu_id,
                                  @user_id = @updated_by,
                                  @user_name = @user_name output,
                                  @error_code = @error_code output,
                                  @return_status = @return_status output

    if (@return_status = 0)
    begin
        return 0
    end

    begin transaction
    exec sp_xml_preparedocument @hDoc output, @xml_data
    set @counter = 1
    select @rowcount = count(row_id)
    from
        openxml(@hDoc, 'dashboard_settings/row', 2) with (row_id bigint)

    while (@counter <= @rowcount)
    begin
        select @id = id,
               @parent_id = parent_id,
               @menu_desc = menu_desc,
               @nav_url = nav_url,
               @icon = icon,
               @display_index = display_index,
               @refresh_time = refresh_time,
               @is_enabled = is_enabled,
               @is_default = is_default,
               @is_refresh_button = is_refresh_button,
               @title = title
        from
            openxml(@hDoc, 'dashboard_settings/row', 2)
            with
            (
                id int,
                parent_id int,
                menu_desc nvarchar(150),
                nav_url nvarchar(150),
                icon nvarchar(150),
                display_index int,
                refresh_time int,
                is_enabled varchar(1),
                is_default varchar(1),
                is_refresh_button varchar(1),
                title nvarchar(250),
                row_id int
            ) xmlTemp
        where xmlTemp.row_id = @counter


        update sys_dashboard_settings
        set parent_id = @parent_id,
            menu_desc = @menu_desc,
            nav_url = @nav_url,
            icon = @icon,
            display_index = @display_index,
            refresh_time = @refresh_time,
            is_enabled = @is_enabled,
            is_default = @is_default,
            is_refresh_button = @is_refresh_button,
            title = @title,
            updated_by = @updated_by,
            date_updated = getdate()
        where id = @id

        if (@@rowcount = 0)
        begin
            rollback transaction
            select @user_name = menu_desc
            from sys_dashboard_settings
            where id = @id
            select @error_code = '425',
                   @return_status = 0
            return 0
        end

        set @counter = @counter + 1
    end
    --Aging Update        

    exec sp_xml_preparedocument @hDoc2 output, @xml_data_aging
    set @counter = 1
    select @rowcount = count(row_id)
    from
        openxml(@hDoc2, 'dashboard_settings_aging/row', 2) with (row_id bigint)

    while (@counter <= @rowcount)
    begin
        select @aging_id = id,
               @dashboard_menu_id = dashboard_menu_id,
               @key = [key],
               @slot_count = slot_count,
               @slot_1 = slot_1,
               @slot_2 = slot_2,
               @slot_3 = slot_3
        from
            openxml(@hDoc2, 'dashboard_settings_aging/row', 2)
            with
            (
                id int,
                dashboard_menu_id int,
                [key] nvarchar(100),
                slot_count int,
                slot_1 int,
                slot_2 int,
                slot_3 int,
                row_id int
            ) xmlTemp
        where xmlTemp.row_id = @counter


        update sys_dashboard_settings_aging
        set [key] = @key,
            slot_count = @slot_count,
            slot_1 = @slot_1,
            slot_2 = @slot_2,
            slot_3 = @slot_3,
            updated_by = @updated_by,
            date_updated = getdate()
        where id = @aging_id

        if (@@rowcount = 0)
        begin
            rollback transaction
            select @user_name = [key]
            from sys_dashboard_settings_aging
            where id = @aging_id
            select @error_code = '425',
                   @return_status = 0
            return 0
        end

        set @counter = @counter + 1
    end

    if
    (
        select count(record_id)
        from sys_record_lock
        where record_id = @menu_id
              and menu_id = @menu_id
    ) = 0
    begin
        exec common_lock_record @menu_id = @menu_id,
                                @record_id = @menu_id,
                                @user_id = @updated_by,
                                @error_code = @error_code output,
                                @return_status = @return_status output

        if (@return_status = 0)
        begin
            return 0
        end
    end

    commit transaction
    exec sp_xml_removedocument @hDoc
    exec sp_xml_removedocument @hDoc2
    set @return_status = 1
    set @error_code = '034'
    set nocount off
    return 1
End
GO
