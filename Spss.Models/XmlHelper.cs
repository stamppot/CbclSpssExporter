using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.Globalization;

namespace Spss.Models {
  public class XmlHelper {

    private XContainer _xContainer;

    public XmlHelper() { }

    public XmlHelper(XContainer xDocument) {
      _xContainer = xDocument;
    }

    public int GetIntAttributeValue(IEnumerable<XElement> elements, String attribute) {
      String value = "";
      try {
        value = elements.Single(e => e.HasAttributeValue(attribute)).Value;
      } catch (Exception e) {
        System.Diagnostics.Debug.WriteLine(String.Format("Error: {0}", e.Message));
      }
      return Int32.Parse(value);
    }

    public String GetAttributeValue(IEnumerable<XElement> elements, String attribute) {
      var value = elements.Single(e => e.HasAttributeValue(attribute)).Value;
      return value;
    }

    public Gender GetAttributeGender(IEnumerable<XElement> elements, String attribute) {
      var value = elements.Single(e => e.HasAttributeValue(attribute)).Value;
      return (Gender)Enum.Parse(typeof(Gender), value);
    }

    public SpssDatatype GetAttributeDatatype(IEnumerable<XElement> elements, String attribute) {
      var value = elements.Single(e => e.HasAttributeValue(attribute)).Value;
      return (SpssDatatype)Enum.Parse(typeof(SpssDatatype), value);
    }

    public DateTime GetAttributeDateTime(IEnumerable<XElement> elements, String attribute) {
      var value = elements.Single(e => e.HasAttributeValue(attribute)).Value;
      DateTime result;
      if (value == "0000-00-00")
        result = DateTime.MinValue;
      else
        result = DateTime.Parse(value);
      return result;
    }
  }
}
