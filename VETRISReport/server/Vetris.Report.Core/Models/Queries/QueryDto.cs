using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models.Queries
{
    public class QueryDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ObjectName { get; set; }
        public List<QueryDefDto> Columns { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
    public class QueryDefDto
    {
        public string Column { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string SortDirection { get; set; }
        public string Operator { get; set; }
        public Value Value1 { get; set; }
        public Value Value2 { get; set; }
    }
    public class Value
    {
        public string StringValue { get; set; }
        public bool? BooleanValue { get; set; }
        public double? NumberValue { get; set; }
        public DateTime? DateValue { get; set; }
    }
}
