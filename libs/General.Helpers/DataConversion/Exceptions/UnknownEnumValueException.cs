using System;

namespace General.Helpers.DataConversion.Exceptions {
  public class UnknownEnumValueException : ArgumentException {
    public UnknownEnumValueException(Type enumType, object enumValue)
      : base("Unknown enum value exception") {
      EnumType = enumType;
      EnumValue = enumValue;
    }

    public Type EnumType { get; private set; }
    public object EnumValue { get; private set; }
  }
}
