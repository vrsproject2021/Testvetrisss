USE [vrsdb]
GO
/****** Object:  UserDefinedFunction [dbo].[InitCap]    Script Date: 20-08-2021 20:56:12 ******/
DROP FUNCTION [dbo].[InitCap]
GO
/****** Object:  UserDefinedFunction [dbo].[InitCap]    Script Date: 20-08-2021 20:56:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
												   										
create FUNCTION [dbo].[InitCap] ( @InputString varchar(4000) ) 
RETURNS VARCHAR(4000)
AS
BEGIN

DECLARE @Index INT
DECLARE @Char CHAR(1)
DECLARE @PrevChar CHAR(1)
DECLARE @OutputString VARCHAR(255)

SET @OutputString = LOWER(@InputString)
SET @Index = 1

WHILE @Index <= LEN(@InputString)
BEGIN
 SET @Char = SUBSTRING(@InputString, @Index, 1)
 SET @PrevChar = CASE WHEN @Index = 1 THEN ' '
 ELSE SUBSTRING(@InputString, @Index - 1, 1)
 END

 IF @PrevChar IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&', '''', '(')
 BEGIN
 IF @PrevChar != '''' OR UPPER(@Char) != 'S'
 SET @OutputString = STUFF(@OutputString, @Index, 1, UPPER(@Char))
 END

 SET @Index = @Index + 1
END

RETURN @OutputString

END

GO
