using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Vetris.Reporting.Library.ReportClasses
{

    public class Report
    {

        public int AutoRefresh { get; set; }
        public string Width { get; set; } = "6.5in";
        public Body Body { get; set; }
        public Page Page { get; set; }

        public ReportSections ReportSections { get; set; }
        public DataSources DataSources { get; set; }
        public DataSets DataSets { get; set; }
        public ReportParameters ReportParameters { get; set; }
        public ReportParametersLayout ReportParametersLayout { get; set; }

        public EmbeddedImages EmbeddedImages { get; set; }
        public string ReportUnitType { get; set; } = "Inch";
        public Guid ReportID { get; set; } = Guid.NewGuid();

        public Report()
        {
           
        }
        public Report(bool withParameters)
        {
            if (withParameters)
            {
                ReportSections = new ReportSections();
                ReportParameters = new ReportParameters();
                ReportParametersLayout = new ReportParametersLayout()
                {
                    GridLayoutDefinition = new GridLayoutDefinition() { NumberOfColumns = 4, NumberOfRows = 2, CellDefinitions = new CellDefinitions() }
                };
            } else
            {
                Page = new Page();
                Body = new Body();
            }
        }
        /// <summary>
        /// Report unit type
        /// </summary>
        /// <param name="type">Inch|Centimeter|Point</param>
        /// <returns></returns>
        public Report SetReportPageUnitType(string type)
        {
            ReportUnitType = type;
            return this;
        }
        public Report SetPageSize(double[] size)
        {
            if (size.Length == 2)
            {
                if (ReportUnitType == "Inch")
                {
                    GetPage().PageWidth = string.Format("{0:F5}in", size[0]);
                    GetPage().PageHeight = string.Format("{0:F5}in", size[1]);
                }
                if (ReportUnitType == "Centimeter")
                {
                    GetPage().PageWidth = string.Format("{0:F5}cm", size[0]);
                    GetPage().PageHeight = string.Format("{0:F5}cm", size[1]);
                }
                if (ReportUnitType == "Point")
                {
                    GetPage().PageWidth = string.Format("{0:F5}pt", size[0]);
                    GetPage().PageHeight = string.Format("{0:F5}pt", size[1]);
                }
            }
            return this;
        }
        public Report SetMargin(double margin)
        {

            if (ReportUnitType == "Inch")
            {
                GetPage().TopMargin = string.Format("{0:F5}in", margin);
                GetPage().BottomMargin = string.Format("{0:F5}in", margin);
                GetPage().LeftMargin = string.Format("{0:F5}in", margin);
                GetPage().RightMargin = string.Format("{0:F5}in", margin);
            }
            if (ReportUnitType == "Centimeter")
            {
                GetPage().TopMargin = string.Format("{0:F5}cm", margin);
                GetPage().BottomMargin = string.Format("{0:F5}cm", margin);
                GetPage().LeftMargin = string.Format("{0:F5}cm", margin);
                GetPage().RightMargin = string.Format("{0:F5}cm", margin);
            }
            if (ReportUnitType == "Point")
            {
                GetPage().TopMargin = string.Format("{0:F5}pt", margin);
                GetPage().BottomMargin = string.Format("{0:F5}pt", margin);
                GetPage().LeftMargin = string.Format("{0:F5}pt", margin);
                GetPage().RightMargin = string.Format("{0:F5}pt", margin);
            }

            return this;
        }
        public Report SetMargin(double top, double left, double right, double bottom)
        {

            if (ReportUnitType == "Inch")
            {
                GetPage().TopMargin = string.Format("{0:F5}in", top);
                GetPage().BottomMargin = string.Format("{0:F5}in", bottom);
                GetPage().LeftMargin = string.Format("{0:F5}in", left);
                GetPage().RightMargin = string.Format("{0:F5}in", right);
            }
            if (ReportUnitType == "Centimeter")
            {
                GetPage().TopMargin = string.Format("{0:F5}cm", top);
                GetPage().BottomMargin = string.Format("{0:F5}cm", bottom);
                GetPage().LeftMargin = string.Format("{0:F5}cm", left);
                GetPage().RightMargin = string.Format("{0:F5}cm", right);
            }
            if (ReportUnitType == "Point")
            {
                GetPage().TopMargin = string.Format("{0:F5}pt", top);
                GetPage().BottomMargin = string.Format("{0:F5}pt", bottom);
                GetPage().LeftMargin = string.Format("{0:F5}pt", left);
                GetPage().RightMargin = string.Format("{0:F5}pt", right);
            }

            return this;
        }

        public Page GetPage()
        {
            if (this.ReportParameters != null)
                return this.ReportSections.ReportSection.Page;
            return Page;
        }
        public Body GetBody()
        {
            if (this.ReportParameters != null)
                return this.ReportSections.ReportSection.Body;
            return Body;
        }
        public Report ShowHeader()
        {

            if (GetPage().PageHeader == null) GetPage().PageHeader = new PageHeader();

            return this;
        }
        public Report ShowFooter()
        {

            if (GetPage().PageFooter == null) GetPage().PageFooter = new PageFooter();

            return this;
        }
        public Report SetHeaderHeight(string height)
        {

            if (GetPage().PageHeader == null) GetPage().PageHeader = new PageHeader();
            GetPage().PageHeader.Height = (height ?? "1in").ToCm();
            return this;
        }
        public Report SetFooterHeight(string height)
        {

            if (GetPage().PageFooter == null) GetPage().PageFooter = new PageFooter();
            GetPage().PageFooter.Height = (height ?? "1in").ToCm();
            return this;
        }
        public Report SetBodyHeight(string height)
        {
            GetBody().Height = (height ?? "1in").ToCm();
            return this;
        }
        public Report AddDataSource(string name, string provider, string connectionString)
        {
            if (DataSources == null)
            {
                DataSources = new DataSources() { DataSource = new List<DataSource>() };
            }
            var ds = new DataSource
            {
                Name = name,
                DataSourceID = Guid.NewGuid(),
                ConnectionProperties = new ConnectionProperties
                {
                    DataProvider = provider,
                    ConnectString = connectionString
                }
            };
            DataSources.DataSource.Add(ds);
            return this;
        }
        public Report AddDataSet(string name, string datasourceName, string commandText)
        {
            if (this.DataSets == null)
            {
                DataSets = new DataSets() { DataSet = new List<DataSet>() };
            }

            var ds = this.GetAllObject<DataSource>().FirstOrDefault(i => i.Name == datasourceName);
            if (ds == null) throw new Exception($"Data source '{datasourceName}' not found!");
            var dset = new DataSet
            {
                Name = name,
                Query = new Query { CommandText = commandText, DataSourceName = datasourceName },
                Fields = new Fields
                {
                    Field = MetadataGenerator.GetFields(ds.ConnectionProperties.ConnectString, commandText)
                }
            };
            DataSets.DataSet.Add(dset);
            return this;
        }

        public Report AddTableToBody(int rows, int cols)
        {
            var ds = this.GetAllObject<DataSet>().FirstOrDefault();
            if (ds == null) { throw new Exception("Add dataset first"); }
            if (GetBody().ReportItems.Tablix == null) GetBody().ReportItems.Tablix = new List<Tablix>();
            var table = new Tablix
            {
                Name = NewTableName(),
                DataSetName = ds.Name,
                TablixBody = new TablixBody
                {
                    TablixColumns = new TablixColumns { TablixColumn = new List<TablixColumn>() },
                    TablixRows = new TablixRows { TablixRow = new List<TablixRow>() }
                }
            };
            var columns = table.TablixBody.TablixColumns.TablixColumn;
            var width = (GetPage().PageWidth.ToCm().ToValue() - (GetPage().LeftMargin.ToCm().ToValue() + GetPage().RightMargin.ToCm().ToValue())) / cols;
            var textboxNo = NextTextboxNo();
            for (int ic = 0; ic < cols; ic++)
            {
                columns.Add(new TablixColumn { Width = width.ToString().ToCm() });
                table.TablixColumnHierarchy.TablixMembers.TablixMember.Add(new TablixMember());
            }
            for (int ir = 0; ir < rows; ir++)
            {
                var r = new TablixRow { TablixCells = new TablixCells { TablixCell = new List<TablixCell>() } };
                for (int c = 0; c < cols; c++)
                {
                    var name = $"Textbox{textboxNo++}";
                    r.TablixCells.TablixCell.Add(new TablixCell
                    {
                        CellContents = new CellContents
                        {
                            Textbox = new Textbox
                            {
                                Name= name,
                                DefaultName=name,
                                Paragraphs=new Paragraphs
                                {
                                    Paragraph=new Paragraph
                                    {
                                        TextRuns=new TextRuns
                                        {
                                            TextRun=new TextRun
                                            {
                                                Style=new Style { }
                                            }
                                        }
                                    }
                                },
                                Style=new Style
                                {
                                    Border=new Border
                                    {
                                        Color= "LightGrey",
                                        Style="Solid"
                                    },
                                    PaddingLeft = "2pt",
                                    PaddingRight = "2pt",
                                    PaddingTop = "2pt",
                                    PaddingBottom = "2pt",
                                }
                            }
                        }
                    });
                }
                table.TablixBody.TablixRows.TablixRow.Add(r);
                if (ir == 1)
                {
                    table.TablixRowHierarchy.TablixMembers.TablixMember.Add(new TablixMember { Group = new Group { Name = "Details" } });
                }
                else
                {
                    table.TablixRowHierarchy.TablixMembers.TablixMember.Add(new TablixMember());
                }
            }
            GetBody().ReportItems.Tablix.Add(table);
            return this;
        }

        public Report AddTextBoxToHeader(Action<Report, PageHeader, Textbox> action = null)
        {
            ShowHeader();
            if (GetPage().PageHeader.ReportItems.Textbox == null) GetPage().PageHeader.ReportItems.Textbox = new List<Textbox>();
            var name = NewTextBoxName();
            var textbox = new Textbox
            {
                Name = name,
                DefaultName = name,
                CanGrow = true,
                KeepTogether = true,
                Paragraphs = new Paragraphs
                {
                    Paragraph = new Paragraph
                    {
                        TextRuns = new TextRuns
                        {
                            TextRun = new TextRun
                            {
                                Value = "",
                                Style = new Style
                                {

                                }
                            }
                        }
                    }
                }
            };
            GetPage().PageHeader.ReportItems.Textbox.Add(textbox);
            if (action != null)
            {
                action(this, GetPage().PageHeader, textbox);
            }
            return this;
        }
        public Report AddTextBoxToFooter(Action<Report, PageFooter, Textbox> action = null)
        {
            ShowFooter();
            if (GetPage().PageFooter.ReportItems.Textbox == null) GetPage().PageFooter.ReportItems.Textbox = new List<Textbox>();
            var name = NewTextBoxName();
            var textbox = new Textbox
            {
                Name = name,
                DefaultName = name,
                CanGrow = true,
                KeepTogether = true,
                Paragraphs = new Paragraphs
                {
                    Paragraph = new Paragraph
                    {
                        TextRuns = new TextRuns
                        {
                            TextRun = new TextRun
                            {
                                Value = "",
                                Style = new Style
                                {

                                }
                            }
                        }
                    }
                }
            };
            GetPage().PageFooter.ReportItems.Textbox.Add(textbox);
            if (action != null)
            {
                action(this, GetPage().PageFooter, textbox);
            }
            return this;
        }
        public Report AddTextBoxToTable(string tableName, int rowIndex, int colIndex, Action<Textbox> action = null)
        {
            var table = this.GetAllObject<Tablix>().FirstOrDefault(i => i.Name == tableName);
            if (table == null) throw new Exception($"Table '{tableName}' not found.");
            var row = table.TablixBody.TablixRows.TablixRow[rowIndex];
            if (row == null) throw new Exception($"Table '{tableName}', row index '{rowIndex}' not found.");
            var columns = table.TablixBody.TablixColumns.TablixColumn;
            if (colIndex < 0 || colIndex > columns.Count - 1) throw new Exception($"Table '{tableName}', column index '{colIndex}' not found.");
            var cell = row.TablixCells.TablixCell[colIndex];
            if (cell == null) throw new Exception($"Table '{tableName}', cell at index '{colIndex}' not found.");

            var name = NewTextBoxName();
            var textbox = new Textbox
            {
                Name = name,
                DefaultName = name,
                CanGrow = true,
                KeepTogether = true,
                Paragraphs = new Paragraphs
                {
                    Paragraph = new Paragraph
                    {
                        TextRuns = new TextRuns
                        {
                            TextRun = new TextRun
                            {
                                Style = new Style
                                {

                                }
                            }
                        }
                    }
                }
            };
            if (cell.CellContents == null) cell.CellContents = new CellContents { };
            cell.CellContents.Textbox = textbox;
            if (action != null)
            {
                action(textbox);
            }
            return this;
        }
        public Report BindTextboxValue(string textboxName, string value)
        {
            var textbox = this.GetAllObject<Textbox>().FirstOrDefault(i => i.Name == textboxName);
            if (textbox == null) throw new Exception($"Textbox '{textboxName}' not found.");
            textbox.Paragraphs.Paragraph.TextRuns.TextRun.Value = value;
            return this;
        }

        public Report BindTableCellValue(string tableName,int rowIndex,int colIndex, string value, string horizontalAlignment=null)
        {
            var table = this.GetAllObject<Tablix>().FirstOrDefault(i => i.Name == tableName);
            if (table == null) throw new Exception($"Table '{tableName}' not found.");
            var row = table.TablixBody.TablixRows.TablixRow[rowIndex];
            if (row == null) throw new Exception($"Table '{tableName}', row index '{rowIndex}' not found.");
            var columns = table.TablixBody.TablixColumns.TablixColumn;
            if (colIndex < 0 || colIndex > columns.Count - 1) throw new Exception($"Table '{tableName}', column index '{colIndex}' not found.");
            var cell = row.TablixCells.TablixCell[colIndex];
            if (cell == null) throw new Exception($"Table '{tableName}', cell at index '{colIndex}' not found.");
            Textbox textbox;
            if (cell.CellContents == null) cell.CellContents = new CellContents { };

            if (cell.CellContents.Textbox == null)
            {
                var name = NewTextBoxName();
                textbox = new Textbox
                {
                    Name = name,
                    DefaultName = name,
                    CanGrow = true,
                    KeepTogether = true,
                    Paragraphs = new Paragraphs
                    {
                        Paragraph = new Paragraph
                        {
                            TextRuns = new TextRuns
                            {
                                TextRun = new TextRun
                                {
                                    Style = new Style
                                    {

                                    }
                                }
                            }
                        }
                    }
                };
                cell.CellContents.Textbox = textbox;
            }
            else
            {
                textbox = cell.CellContents.Textbox;
            }
            if (!string.IsNullOrEmpty(horizontalAlignment))
            {
                if (textbox.Paragraphs.Paragraph.Style == null) textbox.Paragraphs.Paragraph.Style = new Style();
                textbox.Paragraphs.Paragraph.Style.TextAlign = horizontalAlignment;
            }
            textbox.Paragraphs.Paragraph.TextRuns.TextRun.Value = value;
            return this;
        }

        public string NewTextBoxName()
        {
            var textboxes = new List<string>();
            var tx = this.GetAllObject<Textbox>();
            if (tx.Count > 0) textboxes.AddRange(tx.Select(i => i.Name).ToList());

            var maxNo = 0;
            textboxes.ForEach(i =>
            {
                var num = Convert.ToInt32(Regex.Match(i, @"\d+$").Value ?? "");
                if (num > maxNo) maxNo = num;
            });
            return $"Textbox{maxNo + 1}";
        }
        public int NextTextboxNo()
        {
            var textboxes = new List<string>();
            var tx = this.GetAllObject<Textbox>();
            if (tx.Count > 0) textboxes.AddRange(tx.Select(i => i.Name).ToList());

            var maxNo = 0;
            textboxes.ForEach(i =>
            {
                var num = Convert.ToInt32(Regex.Match(i, @"\d+$").Value ?? "");
                if (num > maxNo) maxNo = num;
            });
            return maxNo + 1;
        }
        public string NewTableName()
        {
            var textboxes = new List<string>();
            var tx = this.GetAllObject<Tablix>();
            if (tx.Count > 0) textboxes.AddRange(tx.Select(i => i.Name).ToList());

            var maxNo = 0;
            textboxes.ForEach(i =>
            {
                var num = Convert.ToInt32(Regex.Match(i, @"\d+$").Value ?? "");
                if (num > maxNo) maxNo = num;
            });
            return $"Table{maxNo + 1}";
        }

        public string NewLineName()
        {
            var textboxes = new List<string>();
            var tx = this.GetAllObject<Line>();
            if (tx.Count > 0) textboxes.AddRange(tx.Select(i => i.Name).ToList());

            var maxNo = 0;
            textboxes.ForEach(i =>
            {
                var num = Convert.ToInt32(Regex.Match(i, @"\d+$").Value ?? "");
                if (num > maxNo) maxNo = num;
            });
            return $"Line{maxNo + 1}";
        }

        public string NewImageName()
        {
            var textboxes = new List<string>();
            var tx = this.GetAllObject<Image>();
            if (tx.Count > 0) textboxes.AddRange(tx.Select(i => i.Name).ToList());

            var maxNo = 0;
            textboxes.ForEach(i =>
            {
                var num = Convert.ToInt32(Regex.Match(i, @"\d+$").Value ?? "");
                if (num > maxNo) maxNo = num;
            });
            return $"Image{maxNo + 1}";
        }
        public string NewRectangleName()
        {
            var textboxes = new List<string>();
            var tx = this.GetAllObject<Rectangle>();
            if (tx.Count > 0) textboxes.AddRange(tx.Select(i => i.Name).ToList());

            var maxNo = 0;
            textboxes.ForEach(i =>
            {
                var num = Convert.ToInt32(Regex.Match(i, @"\d+$").Value ?? "");
                if (num > maxNo) maxNo = num;
            });
            return $"Rectangle{maxNo + 1}";
        }
        public Report MergeColumn(string tableName, int rowIndex, int startColIndex, int mergeCount)
        {
            var table = this.GetAllObject<Tablix>().FirstOrDefault(i => i.Name == tableName);
            var row = table.TablixBody.TablixRows.TablixRow[rowIndex];
            if (row == null) throw new Exception($"Table '{tableName}', row index '{rowIndex}' not found.");
            // on merge first cell of the startColIndex will have contents
            // other columns in merge columns will be empty
            for (int c = startColIndex; c < startColIndex + mergeCount; c++)
            {
                if (c == startColIndex)
                {
                    var cell = new TablixCell() { CellContents = new CellContents { ColSpan = mergeCount > 1 ? mergeCount : (int?)null } };
                    if (row.TablixCells.TablixCell[c].CellContents != null && row.TablixCells.TablixCell[c].CellContents.Textbox != null)
                    {
                        cell.CellContents.Textbox = row.TablixCells.TablixCell[c].CellContents.Textbox;
                    }
                    else
                    {
                        cell.CellContents.Textbox = new Textbox() { Name = NewTextBoxName() };
                    }
                }
                else
                {
                    row.TablixCells.TablixCell[c] = new TablixCell(); //blank
                }
            }
            return this;
        }

        public Report AddColumn(string tableName, int index = -1, bool after = true)
        {
            var table = this.GetAllObject<Tablix>().FirstOrDefault(i => i.Name == tableName);
            var columns = table.TablixBody.TablixColumns.TablixColumn;
            var rows = table.TablixBody.TablixRows.TablixRow;
            var isAppend = false;
            var width = (GetPage().PageWidth.ToCm().ToValue() - (GetPage().LeftMargin.ToCm().ToValue() + GetPage().RightMargin.ToCm().ToValue())) / (columns.Count + 1);
            if (index == -1)
            {
                index = columns.Count;
                columns.Add(new TablixColumn { Width = width.ToString().ToCm() });
                isAppend = true;
            }
            else
            {
                if (after)
                {
                    if (index < columns.Count - 1)
                    {
                        columns.Insert(index + 1, new TablixColumn { Width = width.ToString().ToCm() });
                        index = index + 1;
                    }
                    else
                    {
                        columns.Add(new TablixColumn { Width = width.ToString().ToCm() });
                        index = columns.Count - 1;
                        isAppend = true;
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        columns.Insert(index - 1, new TablixColumn { Width = width.ToString().ToCm() });
                        index = index - 1;
                    }
                    else if (columns.Count == 0)
                    {
                        columns.Add(new TablixColumn { Width = width.ToString().ToCm() });
                        isAppend = true;
                    }
                    else
                    {
                        columns.Insert(0, new TablixColumn { Width = width.ToString().ToCm() });
                    }
                }
            }

            for (int row = 0; row < rows.Count; row++)
            {
                var r = rows[row];
                if (isAppend)
                {
                    r.TablixCells.TablixCell.Add(new TablixCell { CellContents = new CellContents { } });
                }
                else
                {
                    for (var c = 0; c < columns.Count; c++)
                    {
                        /*
                         *                     *          
                         *          0          1          2        3
                         *     +----------+----------+---------+---------+
                         *        span=2
                         *     +----------+----------+---------+
                         */
                        var hasSpan = false;
                        if (index == c)
                        {
                            if (c > 0)
                            {
                                var cc = c;
                                while (cc-- > 0)
                                {
                                    if (r.TablixCells.TablixCell[cc].CellContents?.ColSpan > 0)
                                    {
                                        if (r.TablixCells.TablixCell[cc].CellContents.ColSpan >= cc)
                                        {
                                            r.TablixCells.TablixCell[cc].CellContents.ColSpan++;
                                            hasSpan = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!hasSpan)
                                r.TablixCells.TablixCell.Insert(c, new TablixCell { CellContents = new CellContents { } });
                            else
                                r.TablixCells.TablixCell.Insert(c, new TablixCell());
                            break;
                        }
                    }
                }
            }
            return this;
        }
        public Report DeleteColumn(string tableName, int index)
        {
            var table = this.GetAllObject<Tablix>().FirstOrDefault(i => i.Name == tableName);
            var columns = table.TablixBody.TablixColumns.TablixColumn;
            var rows = table.TablixBody.TablixRows.TablixRow;
            var column = columns[index];
            columns.RemoveAt(index);
            for (int row = 0; row < rows.Count; row++)
            {
                var r = rows[row];

                for (var c = 0; c < columns.Count + 1; c++)
                {
                    /*
                     *                     *          
                     *          0          1          2        3
                     *     +----------+----------+---------+---------+
                     *        span=2
                     *     +----------+----------+---------+---------+
                     */
                    var hasSpan = false;
                    if (index == c)
                    {
                        if (c > 0)
                        {
                            var cc = c;
                            while (cc-- > 0)
                            {
                                if (r.TablixCells.TablixCell[cc].CellContents?.ColSpan > 0)
                                {
                                    if (r.TablixCells.TablixCell[cc].CellContents.ColSpan >= cc)
                                    {
                                        r.TablixCells.TablixCell[cc].CellContents.ColSpan--;
                                        hasSpan = true;
                                        break;
                                    }
                                }
                            }
                        }
                        r.TablixCells.TablixCell.RemoveAt(c);
                        break;
                    }

                }
            }
            return this;
        }
        public XElement ToXML()
        {
            var _2016 = ReportParameters != null && ReportParameters.ReportParameter != null && ReportParameters.ReportParameter.Count > 0;
            XNamespace defaultNs = _2016 ? "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition": "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";
            XNamespace rdNs = XNamespace.Get("http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            XElement element = new XElement(defaultNs + "Report",
                new XAttribute(XNamespace.Xmlns + "rd", rdNs));

            if (ReportParameters != null)
            {
                element.Add(ReportSections.ToXML(defaultNs, rdNs));
            }
            else
            {
                element.Add(Body.ToXML(defaultNs, rdNs));
                element.Add(new XElement(defaultNs + "Width", Width));
                element.Add(Page.ToXML(defaultNs, rdNs));
            }


            if (DataSources != null)
            {
                element.Add(DataSources.ToXML(defaultNs, rdNs));
            }
            if (DataSets != null)
            {
                element.Add(DataSets.ToXML(defaultNs, rdNs));
            }
            if (EmbeddedImages != null && EmbeddedImages.EmbeddedImage!=null && EmbeddedImages.EmbeddedImage.Count>0)
            {
                element.Add(EmbeddedImages.ToXML(defaultNs, rdNs));
            }
            if (ReportParameters != null && ReportParameters.ReportParameter!=null && ReportParameters.ReportParameter.Count>0)
            {
                element.Add(ReportParameters.ToXML(defaultNs, rdNs));
                if (ReportParametersLayout != null)
                {
                    element.Add(ReportParametersLayout.ToXML(defaultNs, rdNs));
                }
            }
            element.Add(new XElement(defaultNs + "AutoRefresh", AutoRefresh));
            element.Add(new XElement(rdNs + "ReportUnitType", ReportUnitType));
            element.Add(new XElement(rdNs + "ReportID", ReportID.ToString()));
            return element;
        }
    }

    public class ReportItems
    {
        public List<Tablix> Tablix { get; set; }
        public List<Textbox> Textbox { get; set; }
        public List<Line> Line { get; set; }
        public List<Image> Image { get; set; }
        public List<Rectangle> Rectangle { get; set; }
        public ReportItems()
        {
            Tablix = new List<Tablix>();
            Textbox = new List<Textbox>();
            Line = new List<Line>();
            Image = new List<Image>();
            Rectangle = new List<Rectangle>();
        }
        public bool isEmpty()
        {
            return (Textbox == null || Textbox.Count == 0) &&
                (Tablix == null || Tablix.Count == 0) &&
                (Line == null || Line.Count == 0) &&
                (Rectangle == null || Rectangle.Count == 0) &&
                (Image == null || Image.Count == 0);
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ReportItems");
            if (Tablix != null)
            {
                foreach (var item in Tablix)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            if (Textbox != null)
            {
                foreach (var item in Textbox)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            if (Line != null)
            {
                foreach (var item in Line)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            if (Image != null)
            {
                foreach (var item in Image)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }
            if (Rectangle != null)
            {
                foreach (var item in Rectangle)
                {
                    element.Add(item.ToXML(defaultNs, rdNs));
                }
            }

            return element;
        }



    }

    public class Body
    {
        public ReportItems ReportItems { get; set; }
        public string Height { get; set; } = "2in";
        public Style Style { get; set; }
        public Body()
        {
            ReportItems = new ReportItems();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Body");
            if (ReportItems != null && !ReportItems.isEmpty()) element.Add(ReportItems.ToXML(defaultNs, rdNs));
            if (Height != null) element.Add(new XElement(defaultNs + "Height", Height));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));
            return element;
        }
    }



    public class PageHeader
    {
        public string Height { get; set; } = "1in";
        public bool PrintOnFirstPage { get; set; }
        public bool PrintOnLastPage { get; set; }
        public ReportItems ReportItems { get; set; }
        public Style Style { get; set; }
        public PageHeader()
        {
            ReportItems = new ReportItems();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "PageHeader",
                new XElement(defaultNs + "PrintOnFirstPage", PrintOnFirstPage),
                new XElement(defaultNs + "PrintOnLastPage", PrintOnLastPage));
            element.Add(new XElement(defaultNs + "Height", Height));
            if (ReportItems != null && !ReportItems.isEmpty()) element.Add(ReportItems.ToXML(defaultNs, rdNs));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class PageFooter
    {
        public string Height { get; set; } = "1in";
        public bool PrintOnFirstPage { get; set; }
        public bool PrintOnLastPage { get; set; }
        public ReportItems ReportItems { get; set; }
        public Style Style { get; set; }
        public PageFooter()
        {
            ReportItems = new ReportItems();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "PageFooter",
                new XElement(defaultNs + "PrintOnFirstPage", PrintOnFirstPage),
                new XElement(defaultNs + "PrintOnLastPage", PrintOnLastPage));
            element.Add(new XElement(defaultNs + "Height", Height));
            if (ReportItems != null && !ReportItems.isEmpty()) element.Add(ReportItems.ToXML(defaultNs, rdNs));
            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            return element;
        }
    }

    public class Page
    {
        public PageHeader PageHeader { get; set; }
        public PageFooter PageFooter { get; set; }
        public string PageHeight { get; set; }
        public string PageWidth { get; set; }
        public string LeftMargin { get; set; }
        public string RightMargin { get; set; }
        public string TopMargin { get; set; }
        public string BottomMargin { get; set; }
        public string ColumnSpacing { get; set; } = "0.06in";
        public Style Style { get; set; }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "Page");
            element.Add(new XElement(defaultNs + "PageHeight", PageHeight));
            element.Add(new XElement(defaultNs + "PageWidth", PageWidth));
            element.Add(new XElement(defaultNs + "LeftMargin", LeftMargin));
            element.Add(new XElement(defaultNs + "RightMargin", RightMargin));
            element.Add(new XElement(defaultNs + "TopMargin", TopMargin));
            element.Add(new XElement(defaultNs + "BottomMargin", BottomMargin));
            element.Add(new XElement(defaultNs + "ColumnSpacing", ColumnSpacing));
            if (PageHeader != null) element.Add(PageHeader.ToXML(defaultNs, rdNs));
            if (PageFooter != null) element.Add(PageFooter.ToXML(defaultNs, rdNs));


            if (Style != null) element.Add(Style.ToXML(defaultNs, rdNs));

            return element;
        }

    }

    public class ReportSection
    {
        public string Width { get; set; } = "6.5in";
        public Body Body { get; set; }
        public Page Page { get; set; }
        public ReportSection()
        {
            Body = new Body();
            Page = new Page();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ReportSection");

            element.Add(Body.ToXML(defaultNs, rdNs));
            element.Add(new XElement(defaultNs + "Width", Width));
            element.Add(Page.ToXML(defaultNs, rdNs));
            return element;
        }
    }

    public class ReportSections
    {
        public ReportSection ReportSection { get; set; }
        public ReportSections()
        {
            ReportSection = new ReportSection();
        }
        public XElement ToXML(XNamespace defaultNs, XNamespace rdNs)
        {
            XElement element = new XElement(defaultNs + "ReportSections");

            element.Add(ReportSection.ToXML(defaultNs, rdNs));
            return element;
        }
    }



}
