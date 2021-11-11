using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models.Queries
{
    public class CreateViewInputDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string ObjectName { get; set; }
        public string BodyText { get; set; }
        public string Actions { get; set; }
        public List<MetaDataJson> Metadata { get; set; }
        public string OldObjectName { get; set; }
    }
}
