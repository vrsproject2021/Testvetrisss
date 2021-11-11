using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models.Metadatas
{
    public class MetadataDto
    {
        public string Name { get; set; }
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public List<ColumnDto> Columns { get; set; }
    }
}
