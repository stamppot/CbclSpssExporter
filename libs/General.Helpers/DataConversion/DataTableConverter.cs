using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using General.Helpers;

namespace General.Helpers.DataConversion {
    public static class DataTableConverter {

        public static String ToCsv(this DataTable sourceTable) {
            return ToCsv(sourceTable, "\t");
        }

        public static String ToCsv(this DataTable sourceTable, String columnSeparator) {
            if(columnSeparator != "\t" || columnSeparator != ";")
                columnSeparator = "\t";
            var sb = new StringBuilder();

            var header = GetColumnNames(sourceTable);
            sb.Append(String.Join(columnSeparator, header.ToArray())).Append("\n");

            var rowData = new List<String>();
            foreach (DataRow row in sourceTable.Rows) {
                foreach (DataColumn column in sourceTable.Columns) {
                    rowData.Add(row[column].ToString());
                }
                sb.Append(String.Join(columnSeparator, rowData.ToArray())).Append("\n");
                rowData.Clear();
            }
            return sb.ToString();
        }

        public static List<String> GetColumnNames(this DataTable table) {
            var result = new List<String>();
            for (int i = 0; i < table.Columns.Count; i++)
                result.Add(String.Format(@"""{0}""", table.Columns[i].ColumnName));
            return result;
        }

        public static String PrintColumnNames(this DataTable table) {
            return PrintColumnNames(table, ", ");
        }

        public static String PrintColumnNames(this DataTable table, String separator) {
            return String.Join(separator, GetColumnNames(table).ToArray<String>());
        }

        public static string JsonString(this DataTable Dt) {
            string[] StrDc = new string[Dt.Columns.Count];

            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++) {
                StrDc[i] = Dt.Columns[i].Caption;
                //HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
                if (Dt.Columns[i].DataType == System.Type.GetType("System.Int32"))
                    HeadStr += "\"" + StrDc[i] + "\" : " + StrDc[i] + i.ToString() + ",";
                else
                    HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();

            Sb.Append(@"{""success"" : true, ""data"" : [");
            for (int i = 0; i < Dt.Rows.Count; i++) {
                string TempStr = HeadStr;
                Sb.Append("{");
                for (int j = 0; j < Dt.Columns.Count; j++) {
                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString(), Dt.Rows[i][j].ToString().Replace(@"""", @"\""").Replace("\n", "\\n").Replace("\r", "\\r"));
                }
                Sb.Append(TempStr + "},");
            }
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]}");
            return Sb.ToString();
        }

        public static string JsonString(this DataTable Dt, string staticJSON) {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++) {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();

            Sb.Append(@"{""success"" : true, " + staticJSON + @", ""data"" : [");
            for (int i = 0; i < Dt.Rows.Count; i++) {

                string TempStr = HeadStr;
                Sb.Append("{");
                for (int j = 0; j < Dt.Columns.Count; j++) {
                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().Replace(@"""", @"\"""));
                }
                Sb.Append(TempStr + "},");
            }
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]}");
            return Sb.ToString();
        }


        public static string ToFORMJSON(this DataTable dt) {
            StringBuilder sbOutput = new StringBuilder();

            if (dt.Rows.Count == 0)
                sbOutput.Append(@"{""success"":true,""data"":{}}");
            else {
                sbOutput.Append(@"{""success"":true,");
                // Process the rows
                sbOutput.Append(@"""data"":");
                // if (dt.Rows.Count > 1)
                sbOutput.Append("["); // Make this an array

                var jsonHelper = new JsonHelper();
                for (int i = 0; i < dt.Rows.Count; i++) {
                    sbOutput.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++) {
                        sbOutput.Append(@"""" + dt.Columns[j].ColumnName + @""":""" + jsonHelper.EscapeJson(dt.Rows[i][j].ToString()) + @"""");
                        if (j < dt.Columns.Count - 1)
                            sbOutput.Append(",");
                    }
                    sbOutput.Append("}");
                    if (i < dt.Rows.Count - 1)
                        sbOutput.Append(",");
                }

                // Finalize
                //if (dt.Rows.Count > 1)
                sbOutput.Append("]"); // Close the array
                sbOutput.Append(" } ");
            }
            return sbOutput.ToString();
        }

        public static string ToJSON(this DataTable dt) {
            StringBuilder sbOutput = new StringBuilder();
            // Add the rowcount
            sbOutput.Append("({\"total\":\"" + dt.Rows.Count.ToString() + "\",");
            var jsonHelper = new JsonHelper();

            // Process the rows
            sbOutput.Append("\"results\":[");
            for (int i = 0; i < dt.Rows.Count; i++) {
                sbOutput.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++) {
                    sbOutput.Append("\"" + jsonHelper.EscapeJson(dt.Columns[j].ColumnName) + "\":\"" + jsonHelper.EscapeJson(dt.Rows[i][j].ToString()) + "\"");
                    if (j < dt.Columns.Count - 1)
                        sbOutput.Append(",");
                }
                sbOutput.Append("}");
                if (i < dt.Rows.Count - 1)
                    sbOutput.Append(",");
            }

            // Finalize
            sbOutput.Append(" ]}) ");
            return sbOutput.ToString();
        }

        public static string ToJSONArray(this DataTable dt) {
            StringBuilder sbOutput = new StringBuilder();
            sbOutput.Append("[");
            var jsonHelper = new JsonHelper();
            for (int i = 0; i < dt.Rows.Count; i++) {
                sbOutput.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++) {
                    sbOutput.Append("\"" + jsonHelper.EscapeJson(dt.Columns[j].ColumnName) + "\":\"" + jsonHelper.EscapeJson(dt.Rows[i][j].ToString()) + "\"");
                    if (j < dt.Columns.Count - 1)
                        sbOutput.Append(",");
                }
                sbOutput.Append("}");
                if (i < dt.Rows.Count - 1)
                    sbOutput.Append(",");
            }

            // Finalize
            sbOutput.Append("]");
            return sbOutput.ToString();

        }
    }
}
