using System;
using System.Data;
using General.Helpers.DataConversion.Exceptions;
using System.Collections.Generic;

namespace General.Helpers {
    public static class ConversionHelper
    {
        public static String Print(this DataRow row) {
            var result = new List<String>();
            foreach (DataColumn col in row.Table.Columns) {
                var rowStr = String.Format("{0}:{1}", col.ColumnName, row.ToString(col.ColumnName));
                result.Add(rowStr);
            }
            return String.Join("\t   ", result.ToArray());
        }

        public static string ToString(this DataRow row, string columnName) {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName)) return null;
            return (string)row[columnName].ToString();
        }

        public static Guid ToGuid(this DataRow row, string columnName) {
            return new Guid(row[columnName].ToString());
        }

        public static Guid? ToNullableGuid(this DataRow row, string columnName) {
            if (row.IsNull(columnName)) return null;
            return (Guid?)new Guid(row[columnName].ToString());
        }

        public static int? ToNullableInt(this DataRow row, string columnName) {
            if (row.IsNull(columnName)) return null;
            if(String.IsNullOrEmpty(row.ToString(columnName))) return null;
            return row.ToInt(columnName);
        }

        public static int ToInt(this DataRow row, string columnName) {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName))
                throw new ArgumentException(string.Format("Unexpected null value for integer field '{0}'", columnName));
            int result = 0;
            if (!Int32.TryParse(row[columnName].ToString().Trim(), out result))
                throw new ArgumentException(string.Format("Value '{0}' in field '{1}' is not in integer: Could not parse", row[columnName], columnName));
            return result;
        }

        public static int NullableIntToInt(int? nullable) {
            return nullable.HasValue ? nullable.Value : 0;
        }

        public static double ToDouble(this DataRow row, string columnName) {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName))
                throw new ArgumentException(string.Format("Unexpected null value for double field '{0}'", columnName));
            return (double)row[columnName];
        }

        public static DateTime ToDateTime(this DataRow row, string columnName) {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName))
                throw new ArgumentException(string.Format("Unexpected null value for datetime field '{0}'", columnName));
            return (DateTime)row[columnName];
        }

        public static T ToEnumValue<T>(this DataRow row, string columnName) {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName))
                throw new UnknownEnumValueException(typeof(T), row[columnName]);
            return (T)Enum.Parse(typeof(T), row.ToString(columnName));
        }

        public static T ToEnum<T>(this DataRow row, string columnName) {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName))
                throw new UnknownEnumValueException(typeof(T), row[columnName]);
            return (T)(row[columnName]);
        }

        public static string ToShortDateString(this DateTime d) {
            return string.Format("{0,2}{1,2}{2}", d.Day, d.Month, d.Year.ToString().Replace("20", "").Replace("19", "").ToInt()).Replace(" ", "0");
        }
    }
}
