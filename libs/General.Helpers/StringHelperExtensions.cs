using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace General.Helpers
{
    public static class StringHelperExtensions {

        public static string Format(this string format, params object[] arguments) {
            return string.Format(format, arguments);
        }

        public static string F(this string format, params object[] arguments) {
            return format.Format(arguments);
        }

        public static string StripNonAlfaNumeric(this string input, bool allowDot) {
            StringBuilder sb = new StringBuilder();
            string[] Allowed = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,0,1,2,3,4,5,6,7,8,9,-,_,., ".Split(',');
            for (int i = 0; i < input.Length; i++) {
                bool found = false;
                for (int j = 0; j < Allowed.Length; j++) {
                    if (input.Substring(i, 1).ToLower() == Allowed[j]) {
                        found = true;
                    }
                }
                if (found) {
                    sb.Append(input.Substring(i, 1));
                }
            }
            return sb.ToString();
        }

        public static String StripQuotationMarks(this string value) {
            if(value.StartsWith("\""))
                value = value.Substring(1);
            if(value.EndsWith("\""))
                value = value.Substring(0, value.Length-1);
            return value;
        }

        public static List<int> ToNumbers(this string input) {
            var result = new List<int>();
            String[] separators = {",", ";", "." };
            var inputList = input.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var n in inputList) {
                result.Add(Int32.Parse(n.Trim()));
            }
            return result;
        }

        public static string JoinStr(this IEnumerable<string> input) {
            return input.Aggregate(new StringBuilder(), (current, next) => current.Append(", ").Append(next)).ToString();
        }

        public static int ToInt(this string input) {
            int output = 0;
            int.TryParse(input, out output);
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue">return this when cannot parse</param>
        /// <returns></returns>
        public static int ToInt(this string input, int defaultValue) {
            int output = defaultValue;
            int.TryParse(input, out output);
            return output;
        }


        public static string SqlEscape(this string input) {
            input = input.Replace("'", "''");
            input = input.Replace("’", "’’");
            input = input.Replace("‘", "‘‘");
            input = input.Replace("\\", "\\\\");
            return input;
        }

        public static bool IsNumeric(this object numberString) {
            char[] ca = numberString.ToString().ToCharArray();
            for (int i = 0; i < ca.Length; i++) {
                if (!char.IsNumber(ca[i]))
                    if (ca[i] != '.' && ca[i] != ',')
                        return false;
            }
            if (String.IsNullOrEmpty(numberString.ToString()))
                return false;
            return true;
        }

        public static string ReplaceFirst(this string thisString, string substring, string replacement) {
            int pos = thisString.IndexOf(substring);
            if (pos < 0) return thisString;

            return thisString.Substring(0, pos) + replacement + thisString.Substring(pos + substring.Length);
        }

        public static string ReplaceAll(this string thisString, string needle, string replacement) {
            int pos;
            // Avoid a possible infinite loop
            if (needle == replacement) return thisString;
            while ((pos = thisString.IndexOf(needle)) > 0)
                thisString = thisString.Substring(0, pos) + replacement + thisString.Substring(pos + needle.Length);
            return thisString;
        }

        public static string AddSecondsToDateTimeString(string datetimestring) {
            if (!Regex.IsMatch(datetimestring, "(.)+:(.)+:")) // Check if seconds are not already present
                return Regex.Replace(datetimestring, @"(\d\d|\d):(\d\d|\d)", "$&:00");
            else
                return datetimestring;
        }

        public static DateTime ToDate(this string input) {
            var monthToNumbers = new List<string> { "0", "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            var parts = input.Split('-').ToList();
            var date = DateTime.MinValue;
            //if(parts.Count != 3 && !DateTime.TryParse(input, out date)) {
            //    throw new Exception(string.Format("Could not parse date: '{0}'", input));
            //} else {
            //    return date;
            //}

            int day  = parts[0].TrimStart('0').ToInt();
            var mon = parts[1].ToLower().TrimStart('0');
            int year = parts[2].ToInt();
            int month = monthToNumbers.IndexOf(mon);
            if(month == -1)
                month = mon.ToInt();
            return new DateTime(year, month, day);
        }
    }
}
