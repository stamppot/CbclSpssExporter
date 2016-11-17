using System;
using System.Data;
using General.Helpers.DataConversion.Exceptions;

namespace General.Helpers.DataConversion {
  public static class StringConversionHelper {

    public static bool ToBoolean(this String str) {
      bool result = false;
      if (str == "1")
        str = "True";
      Boolean.TryParse(str, out result);
      return result;
    }
  }
}
