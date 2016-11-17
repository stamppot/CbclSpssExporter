using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Helpers
{
    public static class DateTimeExtensions
    {
        public static string DateTimeConversion(this DateTime dt) {
            string strDate;
            strDate = dateParse(dt)
                + " " + timeParse(dt);
            return strDate;
        }

        public static string dateParse(this DateTime dt) {
            string output = dt.Year + "-";
            if (dt.Month < 10)
                output += "0";
            output += dt.Month;
            output += "-" + dt.Day;
            return output;
        }

        public static string timeParse(this DateTime dt) {
            string output = dt.Hour + ":";
            if (dt.Minute < 10)
                output += "0";
            output += dt.Minute;
            return output;
        }
    }
}
