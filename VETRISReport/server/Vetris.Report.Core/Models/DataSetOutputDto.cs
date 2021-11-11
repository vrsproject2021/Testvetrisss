using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class DataSetOutputDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public string BodyText { get; set; }
        public string Actions { get; set; }
        public List<MetaDataJson> Metadata { get; set; }
    }

    public class MetaDataJson
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Default { get; set; }
        public bool CalculateTotal { get; set; }
    }
}
