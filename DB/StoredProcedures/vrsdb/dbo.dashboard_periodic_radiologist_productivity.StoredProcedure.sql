USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_periodic_radiologist_productivity]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_periodic_radiologist_productivity]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_periodic_radiologist_productivity]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec dashboard_periodic_radiologist_productivity '2021-06-01', '2021-06-30',0,'name'
Create proc [dbo].[dashboard_periodic_radiologist_productivity]
    @from date,
    @to date,
    @modality_id int = 0,
    @orderBy nvarchar(max) = 'name'
As
Begin
    declare @modalities nvarchar(max);
    SELECT @modalities = COALESCE(@modalities + ',', '') + '[' + code + ']'
    FROM modality
    where is_active = 'Y'
          and id = case
                       when ISNULL(@modality_id, 0) = 0 then
                           id
                       else
                           @modality_id
                   end

    declare @orderByQry nvarchar(max);

    declare @modality nvarchar(max),
            @total nvarchar(max);
    SELECT @modality = COALESCE(@modality + ',', '') + 'ISNULL([' + code + '], 0) AS [' + code + ']',
           @total = COALESCE(@total + '+', '') + 'ISNULL([' + code + '], 0)'
    FROM modality
    WHERE is_active = 'Y'
          and id = case
                       when ISNULL(@modality_id, 0) = 0 then
                           id
                       else
                           @modality_id
                   end

    if (@orderBy = 'name')
    begin
        set @orderByQry = 'order by b.radiologist_name';
    end
    else
    begin
        set @orderByQry = 'order by ' + @total + ' desc';
    end

    declare @sqlcommand nvarchar(max);
    set @sqlcommand
        = 'select radiologist_name,' + @modality + ',' + @total
          + ' Total from(   
          select rd.fname+'' ''+ rd.lname radiologist_name,count(1) modality_count,m.code as modality               
          from	vw_studies stu              
				inner join modality m with(nolock) on m.id=stu.modality_id   
				inner join radiologists rd with(nolock) on (stu.dict_radiologist_id = rd.id  
															or stu.prelim_radiologist_id = rd.id
															or stu.final_radiologist_id = rd.id 
															)
          where	m.is_active=''Y'' and rd.is_active=''Y'' and (Convert(date,stu.report_final_on) between @from and @to)  and       
				stu.modality_id = case              
                 when ISNULL(@modality_id, 0) = 0 then              
                  modality_id              
                 else              
                  @modality_id            
                end              
          group by rd.fname,rd.lname, m.code   
          ) a         
          pivot            
           (            
            sum(modality_count)          
            for modality in (' + @modalities + ')            
           ) as b ' + @orderByQry

    EXECUTE sp_executesql @sqlCommand,
                          N'@modality_id int,@from date,@to date',
                          @modality_id = @modality_id,
                          @from = @from,
                          @to = @to
End
GO
