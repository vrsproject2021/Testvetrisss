using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Vetris.Reporting.Library.ReportClasses
{

    public static class PageSizeInch
    {
        public static readonly double[] Letter = { 8.5, 11.0 };
        public static readonly double[] A4 = { 8.3, 11.7 };
        public static readonly double[] Leagal = { 8.5, 14 };
        public static readonly double[] Tabloid = { 11, 17 };
        public static readonly double[] A3 = { 11.7, 16.5 };
        public static readonly double[] B4 = { 9.8, 13.9 };
        public static readonly double[] B5 = { 6.9, 9.8 };
    }

    public static class UnitExtensions
    {
        public static string ToCm(this string src)
        {
            if (src.EndsWith("in", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}cm", Convert.ToDouble(src.Replace("in", "")) * 2.54);
            }
            if (src.EndsWith("cm", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}cm", Convert.ToDouble(src.Replace("cm", "")));
            }
            if (src.EndsWith("pt", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}cm", Convert.ToDouble(src.Replace("pt", "")) * 0.0352778);
            }
            return string.Format("{0:F5}cm", Convert.ToDouble(src));
        }
        public static string ToInch(this string src)
        {
            if (src.EndsWith("in", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}in", Convert.ToDouble(src.Replace("in", "")));
            }
            if (src.EndsWith("cm", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}in", Convert.ToDouble(src.Replace("cm", "")) * 0.393701);
            }
            if (src.EndsWith("pt", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}in", Convert.ToDouble(src.Replace("pt", "")) / 0.0352778);
            }
            return string.Format("{0:F5}in", Convert.ToDouble(src));
        }
        public static string ToPoint(this string src)
        {
            if (src.EndsWith("in", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}pt", Convert.ToDouble(src.Replace("in", "")) * 72);
            }
            if (src.EndsWith("cm", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}pt", Convert.ToDouble(src.Replace("cm", "")) * 28.3465);
            }
            if (src.EndsWith("pt", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("{0:F5}pt", Convert.ToDouble(src.Replace("pt", "")));
            }
            return string.Format("{0:F5}pt", Convert.ToDouble(src));
        }
        public static double ToValue(this string src)
        {
            return Convert.ToDouble(Regex.Match(src ?? "", @"(\d+(\.\d+)?)").Value ?? "");
        }

        public static List<T> GetAllObject<T>(this object obj) where T : class
        {
            var result = new List<T>();
            if (obj == null) return result;

            Type objType = obj.GetType();

            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    object propValue = property.GetValue(obj, null);
                    if (propValue != null)
                    {
                        if (propValue.GetType().IsGenericType &&
                           propValue.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
                        {
                            foreach (var item in propValue as IEnumerable<object>)
                            {
                                if (item.GetType().Equals(typeof(T)))
                                    result.Add((T)item);
                                else
                                {
                                    var inner = GetAllObject<T>(item);
                                    if (inner.Count > 0) result.AddRange(inner);
                                }

                            }
                        }
                        else if (propValue.GetType().Equals(typeof(T)))
                        {
                            result.Add((T)propValue);
                        }
                        else if (propValue.GetType().Equals(typeof(IList<T>)))
                        {
                            result.AddRange((IList<T>)propValue);
                        }
                        else
                        {
                            var inner = GetAllObject<T>(propValue);
                            if (inner.Count > 0) result.AddRange(inner);
                        }
                    }
                }
                catch
                {

                }

            }
            return result;
        }

        public static SizeF GetDynamicSize(this string  s, int size=10)
        {
            Font f = new Font(FontFamily.GenericSansSerif, size);
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            g.PageUnit = GraphicsUnit.Millimeter;
            SizeF ret = SizeF.Empty;
            ret = g.MeasureString(s, f);
            g.Dispose();
            return ret;
        }
        public static SizeF GetDynamicSize(this string s, int size, FontFamily fontFamily)
        {
            Font f = new Font(fontFamily, size);
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            g.PageUnit = GraphicsUnit.Millimeter;
            SizeF ret = SizeF.Empty;
            ret = g.MeasureString(s, f);
            g.Dispose();
            return ret;
        }
    }

    public enum PageOrientation : int
    {
        Potrait,
        Landscape
    }
    public static class ReportUnitType
    {
        public static readonly string Inch = "Inch";
        public static readonly string Cm = "cm";
        public static readonly string Point = "pt";
    }
    public static class DataType
    {
        public static readonly string String = "System.String";
        public static readonly string DateTime = "System.DateTime";
        public static readonly string Int32 = "System.Int32";
        public static readonly string Int64 = "System.Int64";
        public static readonly string Boolean = "System.Boolean";
        public static readonly string Decimal = "System.Decimal";
        public static readonly string Double = "System.Double";
        public static readonly string Single = "System.Single";
        public static readonly string ByteArray = "System.Byte[]";
        public static readonly string Guid = "System.Guid";
    }


}
