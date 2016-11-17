using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Helpers
{
    // needs genericparsing.dll
    public class CsvHelper
    {
        public char FindColumnSeparator(String csv) {
            char tab = '\t';
            char semicolon = ';';
            char result = tab;

            int tabCount = csv.Count(c => c == tab);
            int semiColonCount = csv.Count(c => c == semicolon);
            if (tabCount < semiColonCount)
                result = semicolon;
            return result;
        }
    }
}
