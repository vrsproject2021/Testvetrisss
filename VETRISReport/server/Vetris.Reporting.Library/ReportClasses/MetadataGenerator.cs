using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Vetris.Report.Core.Dependency;
using Vetris.Report.DataAccess;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;

namespace Vetris.Reporting.Library.ReportClasses
{
    public static class MetadataGenerator
    {
      
        public static List<Field> GetFields(string sqlcmd, Dictionary<string, object> parameters=null) {
            var provider = ServiceActivator.GetScope().ServiceProvider;
            var context= (IDatabaseContext)provider.GetService(typeof(IDatabaseContext));

            if (string.IsNullOrEmpty(sqlcmd)) throw new Exception("sqlcmd is required");
            //var sql = Regex.Replace(sqlcmd, @"ORDER BY.*?(?=\s*LIMIT|\s*OFFSET|\)|$)", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var sql = RemoveOrderBy(sqlcmd);
            sql = $"SELECT * FROM ({sql}) _T_ WHERE 1=0";
            var result = new List<Field>();
            context.OpenConnection();
            var q = context.SQL(sql);
           
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var p in parameters)
                {
                    if (!p.Key.StartsWith("@") && sql.Contains($"@{p.Key}"))
                        q.AddParameter($"@{p.Key}", p.Value);
                    else if (p.Key.StartsWith("@") && sql.Contains(p.Key))
                        q.AddParameter(p.Key, p.Value);
                }
            }
            
            var ds = q.ExecuteDataSet();
            if (ds.Tables.Count > 0)
            {
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    result.Add(new Field { DataField = col.ColumnName, TypeName = col.DataType.FullName }); 
                } 
            }
            context.CloseConnection();
            return result;
        }
        public static List<Field> GetFields(string connectionString, string sqlcmd)
        {
            var provider = ServiceActivator.GetScope().ServiceProvider;
            var context = (IDatabaseContext)provider.GetService(typeof(IDatabaseContext));
            if (string.IsNullOrEmpty(connectionString)) throw new Exception("connectionString is required");
            if (string.IsNullOrEmpty(sqlcmd)) throw new Exception("sqlcmd is required");
            var sql = $"SELECT * FROM ({sqlcmd}) T WHERE 1=0";
            var result = new List<Field>();
            
            context
                .WithConnectionString(connectionString)
                .OpenConnection();
            var ds = context.SQL(sql)
                .ExecuteDataSet();
            //SqlConnection _connection = new SqlConnection(connectionString);
            //_connection.Open();
            //SqlCommand _command = new SqlCommand(sql, _connection);
            //_command.CommandType = CommandType.Text;
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = _command;
            //var ds = new System.Data.DataSet();
            //da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    result.Add(new Field { DataField = col.ColumnName, TypeName = col.DataType.FullName });
                }
            }
            context.CloseConnection();
            return result;
        }

        private static string RemoveOrderBy(string sqlSelect)
        {
            TSql120Parser parser = new TSql120Parser(false);
            TextReader rd = new StringReader(sqlSelect);
            IList<ParseError> errors;
            var fragments = parser.Parse(rd, out errors);

            
            var orderby = string.Empty;
            

            if (errors.Count > 0)
            {
                var retMessage = string.Empty;
                foreach (var error in errors)
                {
                    retMessage += error.Column + " - " + error.Message + " - position: " + error.Offset + "; ";
                }

                throw new Exception(retMessage);
            }

            // Extract the query assuming it is a SelectStatement
            var query = ((fragments as TSqlScript).Batches[0].Statements[0] as SelectStatement).QueryExpression;
            // Get the order by clause
          
            orderby = (query as QuerySpecification).OrderByClause.GetString();
            if (!string.IsNullOrEmpty(orderby))
            {
                var start = (query as QuerySpecification).OrderByClause.StartOffset;
                var length = (query as QuerySpecification).OrderByClause.FragmentLength;
                sqlSelect = sqlSelect.Substring(0, start)+ sqlSelect.Substring(start+length);

            }
            return sqlSelect;
            
        }
    }
    public static class Extension
    {
        /// <summary>
        /// Get a string representing the SQL source fragment
        /// </summary>
        /// <param name="statement">The SQL Statement to get the string from, can be any derived class</param>
        /// <returns>The SQL that represents the object</returns>
        public static string GetString(this TSqlFragment statement)
        {
            string s = string.Empty;
            if (statement == null) return string.Empty;

            for (int i = statement.FirstTokenIndex; i <= statement.LastTokenIndex; i++)
            {
                s += statement.ScriptTokenStream[i].Text;
            }

            return s;
        }
    }
}
