USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_modality_revenue_fetch]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[dashboard_modality_revenue_fetch]
GO
/****** Object:  StoredProcedure [dbo].[dashboard_modality_revenue_fetch]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[dashboard_modality_revenue_fetch]
    @month_count int = 0,
    @modality_id int
As
Begin
    declare @from date = CONVERT(date, DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - @month_count, 0)),
            @to date = CONVERT(date, DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE()) - 1, -1));

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


    declare @modality nvarchar(max);
    SELECT @modality = COALESCE(@modality + ',', '') + 'ISNULL([' + code + '], 0.00) AS [' + code + ']'
    FROM modality
    WHERE is_active = 'Y'
          and id = case
                       when ISNULL(@modality_id, 0) = 0 then
                           id
                       else
                           @modality_id
                   end

    declare @sqlcommand nvarchar(max);
    set @sqlcommand
        = 'select Convert(nvarchar(max),[month])+''-''+Convert(nvarchar(max),[year]) month_year ,' + @modality
          + '       
                   
from(        
select month(stu.synched_on) month_number,  FORMAT(stu.synched_on,''MMM'') [month], year(stu.synched_on) [year],round(sum(invoiced_amount),0) invoiced_amount,m.code as modality             
from vw_studies stu            
inner join modality m with(nolock) on m.id=stu.modality_id            
where  stu.invoiced=''Y''  and 
(Convert(date,stu.synched_on) between @from and @to)  and     
stu.modality_id = case            
                            when ISNULL(@modality_id, 0) = 0 then            
                                modality_id            
                            else            
                                @modality_id          
                        end            
group by month(stu.synched_on), FORMAT(stu.synched_on,''MMM''),year(stu.synched_on),m.code            
--order by [year],[month]        
) a         
pivot          
    (          
        sum(invoiced_amount)         
        for modality in (' + @modalities + ')          
    ) as b order by [year],month_number'

    EXECUTE sp_executesql @sqlCommand,
                          N'@modality_id int,@from date,@to date',
                          @modality_id = @modality_id,
                          @from = @from,
                          @to = @to
End
GO
