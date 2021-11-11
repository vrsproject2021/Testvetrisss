SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE verify_dataset
	@body varchar(max),
	@error_line int output,
	@error_message nvarchar(200) output
AS

BEGIN TRY  
	declare @viewbody varchar(max);
	declare @tempview varchar(20);
	select @tempview = '__view__' + CONVERT(nvarchar(10), Round(RAND()*1000000,0));

	set @viewbody = 'CREATE VIEW '+@tempview+ ' AS '+@body+';'
	EXEC (@viewbody)
	set @viewbody = '
		SELECT ColumnName name, ColumnType type, dbo.ProperCase(REPLACE(ColumnName, ''_'', '' '')) description  
							from ( 
							select 
								replace(col.name, '' '', ''_'') ColumnName, 
								column_id ColumnId, 
								case typ.name 
									when ''bigint'' then ''number'' 
									when ''binary'' then ''string'' 
									when ''bit'' then ''boolean'' 
									when ''char'' then ''string'' 
									when ''date'' then ''Date'' 
									when ''datetime'' then ''Date'' 
									when ''datetime2'' then ''Date'' 
									when ''datetimeoffset'' then ''Date'' 
									when ''decimal'' then ''number'' 
									when ''float'' then ''number'' 
									when ''image'' then ''string'' 
									when ''int'' then ''number'' 
									when ''money'' then ''number'' 
									when ''nchar'' then ''string'' 
									when ''ntext'' then ''string'' 
									when ''numeric'' then ''number'' 
									when ''nvarchar'' then ''string'' 
									when ''real'' then ''number'' 
									when ''smalldatetime'' then ''Date'' 
									when ''smallint'' then ''number'' 
									when ''smallmoney'' then ''number'' 
									when ''text'' then ''string'' 
									when ''time'' then ''string'' 
									when ''timestamp'' then ''string'' 
									when ''tinyint'' then ''number'' 
									when ''uniqueidentifier'' then ''string'' 
									when ''varbinary'' then ''string'' 
									when ''varchar'' then ''string'' 
									else ''string''  
								end ColumnType, 
								case  
									when col.is_nullable = 1 and typ.name in (''bigint'', ''bit'', ''date'', ''datetime'', ''datetime2'', ''datetimeoffset'', ''decimal'', ''float'', ''int'', ''money'', ''numeric'', ''real'', ''smalldatetime'', ''smallint'', ''smallmoney'', ''time'', ''tinyint'', ''uniqueidentifier'') 
									then ''?'' 
									else '''' 
								end NullableSign  
							from sys.columns col  
								join sys.types typ on 
									col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id  
							where object_id = object_id('''+@tempview+''')  
							) t 
							order by ColumnId;
	';
	EXEC (@viewbody);
	set @viewbody = 'DROP VIEW '+@tempview+';';
	EXEC (@viewbody);
END TRY 
BEGIN CATCH  
   SELECT @error_line = ERROR_LINE(), @error_message= ERROR_MESSAGE(); 
END CATCH 

GO
