using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Vetris.Reporting.Library.ReportClasses
{
    public class Filters: IReportObject
    {
        public List<Filter> Filter { get; set; }
        public Filters()
        {
            Filter = new List<Filter>();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Filters");
            if (Filter != null)
            {
                foreach (var item in Filter)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            return element;
        }
    }

    public class Filter : IReportObject
    {
        public string FilterExpression { get; set; }
        public string Operator { get; set; }
        public FilterValues FilterValues { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Filter");
            element.Add(new XElement(defaultNs + "FilterExpression", FilterExpression));
            element.Add(new XElement(defaultNs + "Operator", Operator));
            if (FilterValues != null) element.Add(FilterValues.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class FilterValues : IReportObject
    {
       
        public List<string> FilterValue { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "FilterValues");
            if (FilterValue != null)
            {
                foreach (var item in FilterValue)
                {
                    element.Add(new XElement(defaultNs + "FilterValue", FilterValue));
                }
            }
            return element;
        }
    }
    

}
