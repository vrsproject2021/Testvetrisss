using System;
using System.Collections.Generic;
using System.Text;
using Vetris.Report.Core.Models.Metadatas;
using Vetris.Report.Core.Models.Queries;

namespace Vetris.Report.Core.Models.Reports
{
    public class DatasetObjectDto
    {
        public List<TableObject> Tables { get; set; }
        public List<RelationObject> Relations { get; set; }
        public List<GroupObject> Groups { get; set; }
        public List<WhereCondition> WhereConditions { get; set; }
    }


    public class TableObject
    {
        public string name { get; set; }
        public List<ColumnDto> Columns { get; set; }
        public List<string> SelectedColums { get; set; }
    }
    public class RelationObject
    {
        /// <summary>
        /// INNER, LEFT, RIGHT
        /// </summary>
        public string JoinType { get; set; }
        public string LeftTable { get; set; }
        public string LeftColumns { get; set; }
        public string RightTable { get; set; }
        public string RightColumns { get; set; }
    }
    public class GroupObject
    {
        /// <summary>
        /// "invoice_detail.invoice_hdr_id"
        /// </summary>        
        public string Key { get; set; }
        /// <summary>
        /// "sum|average|min|max"
        /// </summary>
        public string Function { get; set; }
        /// <summary>
        /// "invoice_detail.price*invoice_detail.qty"
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// "number(dec)|comma(dec)|currency(symbol,dec)"
        /// </summary>
        public string Format { get; set; }
    }

    public class WhereCondition
    {
        /// <summary>
        /// "AND|OR"
        ///     e.g.
        ///     
        ///     AND [ condition1, condition2 ]
        ///     OR [ condition2, condition 2 ]
        /// </summary>        
        public string Condition { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Operator { get; set; }
        public Value Value1 { get; set; }
        public Value Value2 { get; set; }
        public List<WhereCondition> WhereConditions { get; set; }

    }
    
}
