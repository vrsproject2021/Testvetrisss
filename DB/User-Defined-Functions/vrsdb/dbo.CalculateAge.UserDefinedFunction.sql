USE [vrsdb]
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateAge]    Script Date: 20-08-2021 20:56:12 ******/
DROP FUNCTION [dbo].[CalculateAge]
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateAge]    Script Date: 20-08-2021 20:56:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*******************************************************
** Version		: 1.0.0.0
** Procedure    : CalculateAge : calculate age
** Created By   : Pavel Guha
** Created On   : 06/06/2019
*******************************************************/
CREATE function [dbo].[CalculateAge]  
(@dob datetime,
 @till_date datetime)  
returns varchar(50)  
as  
BEGIN  
    DECLARE @date datetime, @tmpdate datetime, @years int, @months int, @days int  
    DECLARE @Age varchar(50)  
    set @Age=''  
    SELECT @tmpdate = @dob  
      
    SELECT @years = DATEDIFF(yy, @tmpdate, @till_date) - CASE WHEN (MONTH(@dob) > MONTH(@till_date)) OR (MONTH(@dob) = MONTH(@till_date) AND DAY(@dob) > DAY(@till_date)) THEN 1 ELSE 0 END  
    SELECT @tmpdate = DATEADD(yy, @years, @tmpdate)  
    SELECT @months = DATEDIFF(m, @tmpdate, @till_date) - CASE WHEN DAY(@dob) > DAY(@till_date) THEN 1 ELSE 0 END  
    SELECT @tmpdate = DATEADD(m, @months, @tmpdate)  
    SELECT @days = DATEDIFF(d, @tmpdate, @till_date)  
      
    --set @Age=convert(varchar(50),@years)+' Years '+convert(varchar(50),@months)+' Months '+convert(varchar(50),@days)+' Days';  
	if(@months=0 and @days>0) set @months=1
    set @Age=convert(varchar(50),@years)+' Years '+convert(varchar(50),@months)+' Months ';  
    return @Age  
END   
GO
