using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Helpers
{
    public class JsonHelper
    {
        public string EscapeJson(string input) {
            input = input.Replace("\\", "\\\\");
            input = input.Replace("\"", "\\\"");
            input = input.Replace("\'", "\\\'");
            input = input.Replace('\n', ' ');
            input = input.Replace('\r', ' ');
            return input;
        }
    }
}
