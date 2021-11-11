using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Vetris.Report.Core.Extensions;

namespace Vetris.Reporting.Library.ReportClasses
{


    public interface IReportObject
    {
    }
    public interface IHasName
    {
        string Name { get; set; }
    }
    public class TablixColumns : IReportObject
    {
        public List<TablixColumn> TablixColumn { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixColumns");
            if (TablixColumn != null)
            {
                foreach (var item in TablixColumn)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }

            return element;
        }
    }
    public class TablixColumn : IReportObject
    {
        public string Width { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixColumn");

            if (Width != null) element.Add(new XElement(defaultNs + "Width", Width));
            return element;
        }
    }

    public class TablixRows : IReportObject
    {
        public List<TablixRow> TablixRow { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixRows");
            if (TablixRow != null)
            {
                foreach (var item in TablixRow)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            return element;
        }
    }
    public class TablixRow : IReportObject
    {
        public string Height { get; set; } = "0.6cm";
        public TablixCells TablixCells { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixRow");
            if (Height != null) element.Add(new XElement(defaultNs + "Height", Height));
            if (TablixCells != null) element.Add(TablixCells.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class TablixCells : IReportObject
    {
        public List<TablixCell> TablixCell { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixCells");
            if (TablixCell != null)
            {
                foreach (var item in TablixCell)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }

            return element;
        }
    }

    public class TablixCell : IReportObject
    {
        public CellContents CellContents { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixCell");
            if (CellContents != null) element.Add(CellContents.ToXML(defaultNs, rdNs));

            return element;
        }
    }
    public class TablixBody : IReportObject
    {
        public TablixColumns TablixColumns { get; set; }
        public TablixRows TablixRows { get; set; }

        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixBody");
            if (TablixColumns != null) element.Add(TablixColumns.ToXML(defaultNs, rdNs));
            if (TablixRows != null) element.Add(TablixRows.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class TablixMembers : IReportObject
    {
        public List<TablixMember> TablixMember { get; set; }
        public TablixMembers()
        {
            TablixMember = new List<TablixMember>();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixMembers");
            if (TablixMember != null)
            {
                foreach (var item in TablixMember)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }

            return element;
        }
    }
    public class TablixMember : IReportObject
    {
        public TablixHeader TablixHeader { get; set; }
        public TablixMembers TablixMembers { get; set; }
        public Group Group { get; set; }
        public SortExpressions SortExpressions { get; set; }
        public string KeepWithGroup { get; set; }


        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixMember");
            if (Group != null) element.Add(Group.ToXML(defaultNs, rdNs));
            if (SortExpressions != null) element.Add(SortExpressions.ToXML(defaultNs, rdNs));
            if (KeepWithGroup != null) element.Add(new XElement(defaultNs + "KeepWithGroup", KeepWithGroup));
            if (TablixHeader != null) element.Add(TablixHeader.ToXML(defaultNs, rdNs));
            if (TablixMembers != null) element.Add(TablixMembers.ToXML(defaultNs, rdNs));
            return element;
        }
    }
    public class Group : IHasName, IReportObject
    {
        public string Name { get; set; }
        public GroupExpressions GroupExpressions { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Group", new XAttribute("Name", Name));
            return element;
        }
    }
    public class TablixColumnHierarchy : IReportObject
    {
        public TablixMembers TablixMembers { get; set; }
        public TablixColumnHierarchy()
        {
            TablixMembers = new TablixMembers();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixColumnHierarchy");
            if (TablixMembers != null) element.Add(TablixMembers.ToXML(defaultNs, rdNs));
            return element;
        }
    }

    public class TablixRowHierarchy : IReportObject
    {
        public TablixMembers TablixMembers { get; set; }
        public TablixRowHierarchy()
        {
            TablixMembers = new TablixMembers();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixRowHierarchy");
            if (TablixMembers != null) element.Add(TablixMembers.ToXML(defaultNs, rdNs));
            return element;
        }

    }

    public class Border : IReportObject
    {
        public string Style { get; set; }
        public string Color { get; set; }
        public string Width { get; set; }
        public virtual XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Border");
            if (!Style.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Style", Style));
            if (!Color.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Color", Color));
            if (!Width.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Width", Width.ToPoint()));

            return element;
        }
        public virtual bool IsEmpty() => Width.IsNullOrEmpty() && Color.IsNullOrEmpty() && Style.IsNullOrEmpty();
    }
    public class TopBorder : Border
    {

        public override XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TopBorder");
            if (!Style.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Style", Style));
            if (!Color.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Color", Color));
            if (!Width.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Width", Width.ToPoint()));

            return element;
        }
    }
    public class LeftBorder : Border
    {
     
        public override XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "LeftBorder");
            if (!Style.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Style", Style));
            if (!Color.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Color", Color));
            if (!Width.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Width", Width.ToPoint()));

            return element;
        }
    }
    public class RightBorder : Border
    {
      
        public override XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "RightBorder");
            if (!Style.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Style", Style));
            if (!Color.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Color", Color));
            if (!Width.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Width", Width.ToPoint()));

            return element;
        }
    }
    public class BottomBorder : Border
    {
        public override XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "BottomBorder");
            if (!Style.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Style", Style));
            if (!Color.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Color", Color));
            if (!Width.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Width", Width.ToPoint()));

            return element;
        }
    }

    public class Style : IReportObject
    {
        public Border Border { get; set; }
        public TopBorder TopBorder { get; set; }
        public LeftBorder LeftBorder { get; set; }
        public RightBorder RightBorder { get; set; }
        public BottomBorder BottomBorder { get; set; }
        public string PaddingLeft { get; set; }
        public string PaddingRight { get; set; }
        public string PaddingTop { get; set; }
        public string PaddingBottom { get; set; }
        public string Format { get; set; }
        public string FontSize { get; set; }
        public string BackgroundColor { get; set; }
        public string TextAlign { get; set; }
        public string VerticalAlign { get; set; }
        public string FontFamily { get; set; }
        public string FontWeight { get; set; }
        public string TextDecoration { get; set; }
        public string Color { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Style");
            if (Border != null && !Border.IsEmpty()) element.Add(Border.ToXML(defaultNs, rdNs));
            if (LeftBorder != null && !Border.IsEmpty())  element.Add(LeftBorder.ToXML(defaultNs, rdNs));
            if (RightBorder != null && !Border.IsEmpty()) element.Add(RightBorder.ToXML(defaultNs, rdNs));
            if (TopBorder != null && !Border.IsEmpty()) element.Add(TopBorder.ToXML(defaultNs, rdNs));
            if (BottomBorder != null && !Border.IsEmpty()) element.Add(BottomBorder.ToXML(defaultNs, rdNs));
            if (!PaddingLeft.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "PaddingLeft", PaddingLeft));
            if (!PaddingRight.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "PaddingRight", PaddingRight));
            if (!PaddingTop.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "PaddingTop", PaddingTop));
            if (!PaddingBottom.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "PaddingBottom", PaddingBottom));
            if (!FontSize.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "FontSize", FontSize));
            if (!FontFamily.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "FontFamily", FontFamily));
            if (!FontWeight.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "FontWeight", FontWeight));
            if (!Format.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Format", Format));
            if (!TextDecoration.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "TextDecoration", TextDecoration));
            if (!Color.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "Color", Color));
            if (!BackgroundColor.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "BackgroundColor", BackgroundColor));
            if (!TextAlign.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "TextAlign", TextAlign));
            if (!VerticalAlign.IsNullOrEmpty()) element.Add(new XElement(defaultNs + "VerticalAlign", VerticalAlign));

            return element;
        }
    }

    public class TextRun : IReportObject
    {
        public string Value { get; set; }

        public Style Style { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TextRun");
            element.Add(new XElement(defaultNs + "Value", Value));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class TextRuns : IReportObject
    {
        public TextRun TextRun { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TextRuns");
            if (TextRun != null) element.Add(TextRun.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class Paragraph : IReportObject
    {
        public TextRuns TextRuns { get; set; }
        public Style Style { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Paragraph");
            if (TextRuns != null) element.Add(TextRuns.ToXML(defaultNs, rdNs));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class Paragraphs : IReportObject
    {
        public Paragraph Paragraph { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Paragraphs");
            if (Paragraph != null) element.Add(Paragraph.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class Textbox : IHasName, IReportObject
    {
        public string Name { get; set; }
        public bool CanGrow { get; set; }
        public bool KeepTogether { get; set; }
        public Paragraphs Paragraphs { get; set; }
        public string DefaultName { get; set; }
        public Style Style { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }

        public void setValue(string value)
        {
            Paragraphs.Paragraph.TextRuns.TextRun.Value = value;
        }
        public void setFontSize(string value)
        {
            Paragraphs.Paragraph.TextRuns.TextRun.Style.FontSize = value.ToPoint();
        }
        public void setColor(string value)
        {
            Paragraphs.Paragraph.TextRuns.TextRun.Style.Color = value;
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Textbox", new XAttribute("Name", Name));
            element.Add(new XElement(defaultNs + "CanGrow", CanGrow));
            element.Add(new XElement(defaultNs + "KeepTogether", KeepTogether));
            if (Paragraphs != null) element.Add(Paragraphs.ToXML(defaultNs, rdNs));

            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            if (DefaultName != null) element.Add(new XElement(rdNs + "DefaultName", DefaultName));
            if (Top != null) element.Add(new XElement(defaultNs + "Top", Top));
            if (Left != null) element.Add(new XElement(defaultNs + "Left", Left));
            if (Height != null) element.Add(new XElement(defaultNs + "Height", Height));
            if (Width != null) element.Add(new XElement(defaultNs + "Width", Width));

            return element;
        }
    }

    public class CellContents : IReportObject
    {
        public Textbox Textbox { get; set; }
        public int? ColSpan { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "CellContents");
            if (Textbox != null) element.Add(Textbox.ToXML(defaultNs, rdNs));
            if (ColSpan > 0) element.Add(new XElement(defaultNs + "ColSpan", ColSpan));

            return element;
        }
    }

    public class TablixCornerCell : IReportObject
    {
        public CellContents CellContents { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixCornerCell");
            if (CellContents != null) element.Add(CellContents.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class TablixCornerRow : IReportObject
    {
        public TablixCornerCell TablixCornerCell { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixCornerRow");
            if (TablixCornerCell != null) element.Add(TablixCornerCell.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class TablixCornerRows : IReportObject
    {
        public TablixCornerRow TablixCornerRow { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixCornerRows");
            if (TablixCornerRow != null) element.Add(TablixCornerRow.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class TablixCorner : IReportObject
    {
        public TablixCornerRows TablixCornerRows { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixCorner");
            if (TablixCornerRows != null) element.Add(TablixCornerRows.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class Tablix : IHasName, IReportObject
    {
        public string Name { get; set; }
        public TablixBody TablixBody { get; set; }
        public TablixColumnHierarchy TablixColumnHierarchy { get; set; }
        public TablixRowHierarchy TablixRowHierarchy { get; set; }
        public SortExpressions SortExpressions { get; set; }
        public Filters Filters { get; set; }
        public string DataSetName { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public Style Style { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public int? ZIndex { get; set; }
        public bool RepeatColumnHeaders { get; set; }
        public bool FixedColumnHeaders { get; set; }
        public bool KeepTogether { get; set; }
        public bool IsMatrix => TablixColumnHierarchy != null || TablixRowHierarchy != null;
        public TablixCorner TablixCorner { get; set; }
        public Tablix()
        {
            TablixBody = new TablixBody();
            TablixColumnHierarchy = new TablixColumnHierarchy();
            TablixRowHierarchy = new TablixRowHierarchy();
            Filters = new Filters();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Tablix", new XAttribute("Name", Name));

            if (TablixBody != null) element.Add(TablixBody.ToXML(defaultNs, rdNs));
            if (TablixColumnHierarchy != null) element.Add(TablixColumnHierarchy.ToXML(defaultNs, rdNs));
            if (TablixRowHierarchy != null) element.Add(TablixRowHierarchy.ToXML(defaultNs, rdNs));


            if (DataSetName != null) element.Add(new XElement(defaultNs + "DataSetName", DataSetName));
            if (Top != null) element.Add(new XElement(defaultNs + "Top", Top));
            if (Left != null) element.Add(new XElement(defaultNs + "Left", Left));
            if (Height != null) element.Add(new XElement(defaultNs + "Height", Height));
            if (Width != null) element.Add(new XElement(defaultNs + "Width", Width));
            if (ZIndex != null) element.Add(new XElement(defaultNs + "ZIndex", ZIndex));
            
            //element.Add(new XElement(defaultNs + "RepeatColumnHeaders", RepeatColumnHeaders));
            //element.Add(new XElement(defaultNs + "FixedColumnHeaders", FixedColumnHeaders));

            element.Add(new XElement(defaultNs + "RepeatColumnHeaders", true));
            element.Add(new XElement(defaultNs + "FixedColumnHeaders", true));
            element.Add(new XElement(defaultNs + "KeepTogether", true));
            //element.Add(new XElement(defaultNs + "KeepTogether", KeepTogether));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            if (TablixCorner != null) element.Add(TablixCorner.ToXML(defaultNs, rdNs));

            if (Filters != null && Filters.Filter != null && Filters.Filter.Count > 0) element.Add(Filters.ToXML(defaultNs, rdNs));
            if (SortExpressions != null) element.Add(SortExpressions.ToXML(defaultNs, rdNs));
            return element;
        }
    }

    public class Line : IHasName, IReportObject
    {
        public string Name { get; set; }
        public string Top { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public int ZIndex { get; set; }
        public Style Style { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Line", new XAttribute("Name", Name));


            if (Top != null) element.Add(new XElement(defaultNs + "Top", Top));
            if (Height != null) element.Add(new XElement(defaultNs + "Height", Height));
            if (Width != null) element.Add(new XElement(defaultNs + "Width", Width));
            element.Add(new XElement(defaultNs + "ZIndex", ZIndex));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class Image : IHasName, IReportObject
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Value { get; set; }
        public string Sizing { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public int ZIndex { get; set; }
        public Style Style { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Image", new XAttribute("Name", Name));


            if (Source != null) element.Add(new XElement(defaultNs + "Source", Source));
            if (Value != null) element.Add(new XElement(defaultNs + "Value", Value));
            if (Sizing != null) element.Add(new XElement(defaultNs + "Sizing", Sizing));


            if (Top != null) element.Add(new XElement(defaultNs + "Top", Top));
            if (Left != null) element.Add(new XElement(defaultNs + "Left", Left));
            if (Height != null) element.Add(new XElement(defaultNs + "Height", Height));
            if (Width != null) element.Add(new XElement(defaultNs + "Width", Width));
            element.Add(new XElement(defaultNs + "ZIndex", ZIndex));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class Rectangle : IHasName, IReportObject
    {
        public string Name { get; set; }
        public bool KeepTogether { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public int ZIndex { get; set; }
        public Style Style { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Rectangle", new XAttribute("Name", Name));


            element.Add(new XElement(defaultNs + "KeepTogether", KeepTogether));

            if (Top != null) element.Add(new XElement(defaultNs + "Top", Top));
            if (Left != null) element.Add(new XElement(defaultNs + "Left", Left));
            if (Height != null) element.Add(new XElement(defaultNs + "Height", Height));
            if (Width != null) element.Add(new XElement(defaultNs + "Width", Width));
            element.Add(new XElement(defaultNs + "ZIndex", ZIndex));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class ConnectionProperties : IReportObject
    {
        public string DataProvider { get; set; }
        public string ConnectString { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ConnectionProperties");
            if (DataProvider != null) element.Add(new XElement(defaultNs + "DataProvider", DataProvider));
            element.Add(new XElement(defaultNs + "ConnectString", ConnectString ?? "/* Local connection string */"));

            return element;
        }
    }

    public class DataSource : IHasName, IReportObject
    {
        public string Name { get; set; }
        public ConnectionProperties ConnectionProperties { get; set; }
        public Guid DataSourceID { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "DataSource", new XAttribute("Name", Name));
            if (ConnectionProperties != null) element.Add(ConnectionProperties.ToXML(defaultNs, rdNs));
            if (DataSourceID != null) element.Add(new XElement(rdNs + "DataSourceID", DataSourceID.ToString()));

            return element;
        }
    }

    public class DataSources : IReportObject
    {
        public List<DataSource> DataSource { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "DataSources");

            if (DataSource != null)
            {
                foreach (var item in DataSource)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }

            return element;
        }
    }

    public class Query : IReportObject
    {
        public string DataSourceName { get; set; }
        public string CommandText { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Query");


            if (DataSourceName != null) element.Add(new XElement(defaultNs + "DataSourceName", DataSourceName));
            if (CommandText != null) element.Add(new XElement(defaultNs + "CommandText", CommandText));

            return element;
        }
    }

    public class Field : IReportObject
    {
        public string DataField { get; set; }
        public string TypeName { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Field", new XAttribute("Name", DataField));


            if (DataField != null) element.Add(new XElement(defaultNs + "DataField", DataField));
            if (TypeName != null) element.Add(new XElement(rdNs + "TypeName", TypeName));

            return element;
        }
    }

    public class Fields : IReportObject
    {
        public List<Field> Field { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Fields");

            if (Field != null)
            {
                foreach (var item in Field)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }

            return element;
        }
    }

    public class DataSetInfo : IReportObject
    {
        public string DataSetName { get; set; }
        public string SchemaPath { get; set; }
        public string TableName { get; set; }
        public string TableAdapterFillMethod { get; set; }
        public string TableAdapterGetDataMethod { get; set; }
        public string TableAdapterName { get; set; }

        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(rdNs + "DataSetInfo");


            if (DataSetName != null) element.Add(new XElement(rdNs + "DataSetName", DataSetName));
            if (SchemaPath != null) element.Add(new XElement(rdNs + "SchemaPath", SchemaPath));
            if (TableName != null) element.Add(new XElement(rdNs + "TableName", TableName));
            if (TableAdapterFillMethod != null) element.Add(new XElement(rdNs + "TableAdapterFillMethod", TableAdapterFillMethod));
            if (TableAdapterGetDataMethod != null) element.Add(new XElement(rdNs + "TableAdapterGetDataMethod", TableAdapterGetDataMethod));
            if (TableAdapterName != null) element.Add(new XElement(rdNs + "TableAdapterName", TableAdapterName));

            return element;
        }
    }

    public class DataSet : IHasName, IReportObject
    {
        public string Name { get; set; }
        public Query Query { get; set; }
        public Fields Fields { get; set; }
        public DataSetInfo DataSetInfo { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "DataSet", new XAttribute("Name", Name));


            if (Query != null) element.Add(Query.ToXML(defaultNs, rdNs));
            if (Fields != null) element.Add(Fields.ToXML(defaultNs, rdNs));
            if (DataSetInfo != null) element.Add(DataSetInfo.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class DataSets : IReportObject
    {
        public List<DataSet> DataSet { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "DataSets");

            if (DataSet != null)
            {
                foreach (var item in DataSet)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }

            return element;
        }
    }

    public class EmbeddedImage : IHasName, IReportObject
    {
        public string Name { get; set; }
        public string MIMEType { get; set; }
        public string ImageData { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "EmbeddedImage", new XAttribute("Name", Name));


            if (MIMEType != null) element.Add(new XElement(defaultNs + "MIMEType", MIMEType));
            if (ImageData != null) element.Add(new XElement(defaultNs + "ImageData", ImageData));

            return element;
        }
    }

    public class EmbeddedImages : IReportObject
    {
        public List<EmbeddedImage> EmbeddedImage { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "EmbeddedImages");
            if (EmbeddedImage != null)
            {
                foreach (var item in EmbeddedImage)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }

            return element;
        }
    }

    #region Report Parameters
    public class ParameterValue : IReportObject
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ParameterValue");


            element.Add(new XElement(defaultNs + "Value", Value));
            element.Add(new XElement(defaultNs + "Label", Label));
            return element;
        }
    }

    public class ParameterValues : IReportObject
    {
        public List<ParameterValue> ParameterValue { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ParameterValues");
            if (ParameterValue != null)
            {
                foreach (var item in ParameterValue)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            return element;
        }
    }

    public class ValidValues : IReportObject
    {
        public ParameterValues ParameterValues { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ValidValues");
            if (ParameterValues != null) element.Add(ParameterValues.ToXML(defaultNs, rdNs));
            return element;
        }
    }

    public class ReportParameter : IHasName, IReportObject
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Prompt { get; set; }
        public string InputValue { get; set; }
        public ValidValues ValidValues { get; set; }
        public bool? Nullable { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ReportParameter", new XAttribute("Name", Name));
            element.Add(new XElement(defaultNs + "DataType", DataType));
            element.Add(new XElement(defaultNs + "Prompt", Prompt));
            if (ValidValues != null) element.Add(ValidValues.ToXML(defaultNs, rdNs));
            if (Nullable != null)
                element.Add(new XElement(defaultNs + "Nullable", Nullable));
            return element;
        }
    }

    public class ReportParameters : IReportObject
    {
        public List<ReportParameter> ReportParameter { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ReportParameters");
            if (ReportParameter != null)
            {
                foreach (var item in ReportParameter)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            return element;
        }
    }

    public class CellDefinition : IReportObject
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public string ParameterName { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "CellDefinition");
            element.Add(new XElement(defaultNs + "ColumnIndex", ColumnIndex));
            element.Add(new XElement(defaultNs + "RowIndex", RowIndex));
            element.Add(new XElement(defaultNs + "ParameterName", ParameterName));

            return element;
        }
    }

    public class CellDefinitions : IReportObject
    {
        public List<CellDefinition> CellDefinition { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "CellDefinitions");
            if (CellDefinition != null)
            {
                foreach (var item in CellDefinition)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            return element;
        }
    }

    public class GridLayoutDefinition : IReportObject
    {
        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public CellDefinitions CellDefinitions { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "GridLayoutDefinition");
            element.Add(new XElement(defaultNs + "NumberOfColumns", NumberOfColumns));
            element.Add(new XElement(defaultNs + "NumberOfRows", NumberOfRows));
            if (CellDefinitions != null)
                element.Add(CellDefinitions.ToXML(defaultNs, rdNs));
            return element;
        }
    }

    public class ReportParametersLayout : IReportObject
    {
        public GridLayoutDefinition GridLayoutDefinition { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ReportParametersLayout");
            if (GridLayoutDefinition != null)
                element.Add(GridLayoutDefinition.ToXML(defaultNs, rdNs));
            return element;
        }
    }
    #endregion

    public class TablixHeader : IReportObject
    {
        public string Size { get; set; }
        public CellContents CellContents { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "TablixHeader");
            if (Size != null) element.Add(new XElement(defaultNs + "Size", Size.ToCm()));
            if (CellContents != null) element.Add(CellContents.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class GroupExpressions : IReportObject
    {
        public List<string> GroupExpression { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "GroupExpressions");
            if (GroupExpression != null)
            {
                foreach (var item in GroupExpression)
                {
                    element.Add(defaultNs + "GroupExpression", item);
                }
            }


            return element;
        }
    }

    public class SortExpression : IReportObject
    {
        public string Value { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "SortExpression");
            element.Add(new XElement(defaultNs + "Value", Value));

            return element;
        }
    }

    public class SortExpressions : IReportObject
    {
        public SortExpression SortExpression { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "SortExpressions");
            if (SortExpression != null) element.Add(SortExpression.ToXML(defaultNs, rdNs));

            return element;
        }
    }
}
