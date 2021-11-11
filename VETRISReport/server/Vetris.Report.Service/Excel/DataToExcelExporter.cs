using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vetris.Report.Core.Configurations;
using Vetris.Report.Core.Models;
using Vetris.Report.Core.Models.Queries;
using Vetris.Report.Core.Session;
using Vetris.Report.DataAccess;

namespace Vetris.Report.Service.Excel
{
    public class DataToExcelExporter : EpPlusExcelExporterBase, IDataToExcelExporter
    {
        public DataToExcelExporter(IDatabaseContext database, IAppFolders folders, ISessionInfo session) : base(database, folders, session)
        {

        }

        public FileDto ExportToFile(QueryDto input)
        {
            _db.OpenConnection();
            var db = _db.QueryFactory();
            var dq = db.Query("data_sets")
                          .Where("id", input.Id);
            var _dataset = dq.Get<DataSetsModel>().FirstOrDefault();
            var metadata = _dataset.ToOutputDto();

            var query = db.Query(input.ObjectName);
            query = query.Select(input.Columns.Select(i => i.Column).ToArray());
            foreach (var c in input.Columns)
            {
                if (c.Type == "string")
                {
                    if (c.Operator == "eq") query = query.Where(c.Column, c.Value1.StringValue);
                    if (c.Operator == "neq") query = query.WhereNot(c.Column, c.Value1.StringValue);
                    if (c.Operator == "like") query = query.WhereLike(c.Column, c.Value1.StringValue);
                    if (c.Operator == "notlike") query = query.WhereNotLike(c.Column, c.Value1.StringValue);
                }
                if (c.Type == "number" && c.Value1 != null && c.Value1.NumberValue.HasValue)
                {
                    if (c.Operator == "eq") query = query.Where(c.Column, c.Value1.NumberValue);
                    if (c.Operator == "neq") query = query.WhereNot(c.Column, c.Value1.NumberValue);
                    if (c.Operator == "gt") query = query.Where(c.Column, ">", c.Value1.NumberValue);
                    if (c.Operator == "lt") query = query.Where(c.Column, "<", c.Value1.NumberValue);
                    if (c.Operator == "gte") query = query.Where(c.Column, ">=", c.Value1.NumberValue);
                    if (c.Operator == "lte") query = query.Where(c.Column, "<=", c.Value1.NumberValue);

                    if (c.Value2 != null && c.Value2.NumberValue.HasValue)
                    {
                        if (c.Operator == "between") query = query.WhereBetween(c.Column, c.Value1.NumberValue, c.Value2.NumberValue);
                    }

                }
                if (c.Type == "Date" && c.Value1 != null && c.Value1.DateValue.HasValue)
                {
                    if (c.Operator == "eq") query = query.WhereDate(c.Column, c.Value1.DateValue);
                    if (c.Operator == "neq") query = query.WhereNotDate(c.Column, c.Value1.DateValue);
                    if (c.Operator == "gt") query = query.WhereDate(c.Column, ">", c.Value1.DateValue);
                    if (c.Operator == "lt") query = query.WhereDate(c.Column, "<", c.Value1.DateValue);
                    if (c.Operator == "gte") query = query.WhereDate(c.Column, ">=", c.Value1.DateValue);
                    if (c.Operator == "lte") query = query.WhereDate(c.Column, "<=", c.Value1.DateValue);
                    if (c.Value2 != null && c.Value2.DateValue.HasValue)
                    {
                        if (c.Operator == "between") query = query.WhereBetween(c.Column, c.Value1.DateValue, c.Value2.DateValue);
                    }
                }


                if (c.Type == "boolean" && c.Value1 != null && c.Value1.BooleanValue.HasValue)
                {
                    if (c.Operator == "eq") query = query.Where(c.Column, c.Value1.BooleanValue);
                    if (c.Operator == "neq") query = query.WhereNot(c.Column, c.Value1.BooleanValue);
                }

            }
            var ascending_cols = input.Columns.Where(i => i.SortDirection == "asc").Select(i => i.Column).ToArray();
            var descending_cols = input.Columns.Where(i => i.SortDirection == "desc").Select(i => i.Column).ToArray();

            if (ascending_cols.Length > 0) query = query.OrderBy(ascending_cols);
            if (descending_cols.Length > 0) query = query.OrderByDesc(descending_cols);

            var data = query.Get();

            _db.CloseConnection();

            return CreateExcelPackage($"{input.Title}_{DateTime.Now.ToString("yyyy_MMM_dd_HH_mm_ss")}.xlsx", excel =>
            {

                var sheet = excel.Workbook.Worksheets.Add(input.Title);
                sheet.OutLineApplyStyle = true;

                AddHeader(sheet, input.Columns.Select(i => i.Title).ToArray());
                var row = 2;
                foreach (var rowdata in data)
                {
                    var index = 1;
                    foreach (var col in rowdata)
                    {
                        sheet.Cells[row, index++].Value = col.Value;
                    }
                    row++;
                }
                for (var i = 1; i <= input.Columns.Count; i++)
                {
                    if (input.Columns[i-1].Type == "Date")
                        sheet.Column(i).Style.Numberformat.Format = "dd-MMM-yy HH:mm";
                    if (input.Columns[i - 1].Type == "number")
                    {
                        sheet.Column(i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        if (input.Columns[i - 1].Title.Contains("Amount"))
                        {
                            sheet.Column(i).Style.Numberformat.Format = @"_ $ #,##0.00_ ;_ ($ #,##0.00)_ ;_ * "" - ""??_ ;_ @_ ";
                        }
                    }
                    sheet.Column(i).AutoFit();
                }
                if(metadata!=null && metadata.Metadata != null)
                {
                    var hasformula = false;
                    for (var i = 1; i <= input.Columns.Count; i++)
                    {
                        if (input.Columns[i - 1].Type == "number")
                        {
                            var col = metadata.Metadata.FirstOrDefault(x => x.Name.Equals(input.Columns[i - 1].Column, StringComparison.InvariantCultureIgnoreCase));
                            if (col != null && col.CalculateTotal)
                            {
                                var colname = GetExcelColumnName(i);
                                sheet.Cells[row, i].Formula= $"=SUM({colname}{2}:{colname}{row-1})";
                                sheet.Cells[row, i].Style.Font.Bold = true;
                                sheet.Cells[row, i].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                sheet.Cells[row, i].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                                hasformula = true;
                            }
                        }
                    }
                    if (hasformula) {
                        sheet.Workbook.CalcMode = OfficeOpenXml.ExcelCalcMode.Automatic; 
                    }
                }
                
            });
        }

        /// <summary>
        /// Convert Excel column index into name. Column start from 1
        /// </summary>
        /// <param name="columnNumber">Column name/number
        /// <returns>Column name</returns>
        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
