using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace General.Helpers {
    public class HtmlHelper {

        public string HtmlEncode(String html) {
            return System.Web.HttpUtility.HtmlEncode(html);
        }

        public string HtmlDecode(String html) {
            return System.Web.HttpUtility.HtmlDecode(html);
        }
    }
}
