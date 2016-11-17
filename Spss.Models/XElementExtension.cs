using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.Globalization;


namespace Spss.Models {
  public static class XElementExtension {

    public static String ReadItemNumber(this XElement element) {
      if (element.Attribute("number") != null)
        return element.Attribute("number").Value;
      return "-1";
    }

    public static bool HasAttribute(this XElement element, String attribute) {
      return element.Attribute(attribute) != null;
    }

    public static bool HasAttributeValue(this XElement element, String attributeValue) {
      return element.Attribute("name") != null && element.Attribute("name").Value == attributeValue;
    }

    public static String AttributeValue(this XElement element, String attribute) {
      if (HasAttribute(element, attribute))
        return element.Attribute(attribute).Value;
      return "";
    }

    public static int ReadIntValue(this XElement element, String attribute) {
      int result;
      try {
        var found = AttributeValue(element, attribute);
        result = Int32.Parse(found);
      } catch (Exception ex) {
        Debug.WriteLine(ex.Message);
        throw;
      }
      return result;
    }

    //public static LayoutType ReadOptionalLayoutType(this XElement itemElement) {
    //  LayoutType layoutType = LayoutType.SingleLineItem;
    //  if (itemElement.Attribute("layout") != null)
    //    layoutType = (LayoutType)Enum.Parse(typeof(LayoutType), itemElement.Attribute("layout").Value.Trim());
    //  return layoutType;
    //}

    //public static ItemType ReadOptionalItemType(this XElement itemElement) {
    //  ItemType itemType = ItemType.Item;
    //  if (itemElement.Attribute("type") != null)
    //    itemType = (ItemType)Enum.Parse(typeof(ItemType), itemElement.Attribute("type").Value.Trim());
    //  return itemType;
    //}

    //public static ItemGroupType ReadOptionalItemGroupType(this XElement element) {
    //  ItemGroupType itemGroupType = ItemGroupType.Group;
    //  if (element.Attribute("type") != null)
    //    itemGroupType = (ItemGroupType)Enum.Parse(typeof(ItemGroupType), element.Attribute("type").Value.Trim());
    //  return itemGroupType;
    //}

    public static String ReadTextNode(this XElement element) {
      var textNode = element.Nodes().OfType<XText>().FirstOrDefault();
      return (textNode != null) ? textNode.Value.Trim().Replace("\n", "") : "";
    }

    public static String ReadChildTextNode(this XElement element, String childElement) {
      var childXElement = element.Element(childElement);
      if (childXElement == null)
        throw new FormatException(String.Format("Questionnaire XML format child element missing: {0}\n Element: {1}", childElement, element.ToString()));

      return childXElement.ReadTextNode();
    }

    public static Guid ReadOrCreateId(this XElement element) {
      var idAttr = element.ReadAttribute("id");
      if (!String.IsNullOrEmpty(idAttr) && (idAttr.Count() == 36))
        return new Guid(idAttr);
      return Guid.NewGuid();
    }

    public static String ReadAttribute(this XElement element, String attribute) {
      return (element.Attribute(attribute) != null) ? element.Attribute(attribute).Value.Trim() : null;
    }

    public static Dictionary<String, String> ReadTranslations(this XElement element) {
      var translationElems = element.Descendants("translation");
      if (translationElems.Any())
        return translationElems.ToDictionary(tElement => tElement.ReadAttribute("culture"),
                      tElement => tElement.ReadTextNode());
      return new Dictionary<string, string> { { "nl-NL", element.Value } };
    }

    public static Dictionary<String, Pair<String>> ReadPairedTranslations(this XElement element) {
      var translationElems = element.Descendants("translation");
      if (translationElems.Any())
        return translationElems.ToDictionary(tElement => tElement.ReadAttribute("culture"),
                      tElement => new Pair<String>(tElement.ReadTextNode(), tElement.ReadAttribute("label")));
      var label = element.ReadAttribute("label");
      if (label == null)
        label = element.Value;
      var textAndLabel = new Pair<String>(element.Value, label);
      return new Dictionary<string, Pair<string>> { { "nl-NL", textAndLabel } };
    }

    public static bool HasTranslations(this XElement element) {
      return (element.Descendants("translation").Any());
    }

    public static IEnumerable<CultureInfo> GetSupportedCultures(this XElement xElement) {
      var cultures =
        (from elem in xElement.Descendants("translation")
         where elem.Attribute("culture") != null
         select elem.Attribute("culture").Value).Distinct()
        .Select(c => CultureInfo.GetCultureInfo(c));
      if (!cultures.Any())
        cultures = new List<CultureInfo> { CultureInfo.GetCultureInfo("nl-NL") };
      return cultures;
    }

    public static Guid GetId(this XElement element) {
      Guid id = Guid.NewGuid();
      if (element.ReadAttribute("id") != null) {
        var strId = element.ReadAttribute("id");
        id = GetId(strId);
      }
      return id;
    }

    private static Guid GetId(String stringId) {
      Guid guid;
      if (stringId.Count() == 36)
        guid = new Guid(stringId);
      else
        throw new FormatException(String.Format("Id (GUID) is in wrong format: {0}", stringId));
      return guid;
    }
  }
}
