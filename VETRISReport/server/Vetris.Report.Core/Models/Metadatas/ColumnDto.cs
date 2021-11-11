using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models.Metadatas
{
    public class ColumnDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Default { get; set; }
    }
}
