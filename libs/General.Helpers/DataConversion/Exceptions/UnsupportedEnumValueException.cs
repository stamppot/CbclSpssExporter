using System;

namespace General.Helpers.DataConversion.Exceptions {
  public class UnsupportedEnumValueException : NotSupportedException {
    public UnsupportedEnumValueException(Type enumType, object enumValue)
      : base("Unsupported enum value exception") {
      EnumType = enumType;
      EnumValue = enumValue;
    }

    public Type EnumType { get; private set; }
    public object EnumValue { get; private set; }
  }
}
