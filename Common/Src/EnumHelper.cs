#region References
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
#if __XAMARIN__
using ServiceStack.DataAnnotations;
#endif
#endregion References

namespace ShareTrading.Common.Src
{
  public class EnumHelper
  {

    private EnumHelper()
    {

    }
  /// <summary>
  /// Return the description associated with this enum type
  /// If there is a Description attribute for the enum type use that otherwise
  /// use its string value.
  /// </summary>
  /// <param name="enumType"></param>
  /// <returns></returns>
  public static string GetEnumTypeDescription(Type enumType)
  {
    DescriptionAttribute[] attributes = (DescriptionAttribute[])enumType.GetCustomAttributes(typeof(DescriptionAttribute), false);
    if (attributes != null && attributes.Length > 0)
      return attributes[0].Description;
    return enumType.ToString();
  }

  /// <summary>
  /// Return the Description associated with this enum value.
  /// If there is a Description attribute for enum use that otherwise
  /// use its string value.
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static string GetEnumDescription(Enum value)
  {
    if (value == null)
      return String.Empty;
    object[] flagAttrs = value.GetType().GetCustomAttributes(typeof(FlagsAttribute), false);
    if (flagAttrs == null || flagAttrs.Length == 0)
    {
      FieldInfo fi = value.GetType().GetField(value.ToString());

      if (fi != null)
      {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
          return attributes[0].Description;
      }
    }
    return value.ToString();
  }

 
    /// <summary>
    /// Return a List containing all the possible enum values for a given enum type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> EnumToList<T>()
  {
    Type enumType = typeof(T);

    // Can't use generic type constraints on value types,
    // so have to do check like this
    if (enumType.BaseType != typeof(Enum))
      throw new ArgumentException("T must be of type System.Enum");

    Array enumValArray = Enum.GetValues(enumType);
    List<T> enumValList = new List<T>(enumValArray.Length);

    foreach (int val in enumValArray)
    {
      enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
    }

    return enumValList;
  }

}

public class VerboseEnum
{
  /// <summary>
  /// The Value of the Enum
  /// </summary>
  public Enum Value { get; protected set; }

  public override String ToString()
  {
    return EnumHelper.GetEnumDescription(Value);
  }

  /// <summary>
  /// Create a Verbose Enum
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static VerboseEnum Create(Enum value)
  {
    return new VerboseEnum() { Value = value };
  }

  /// <summary>
  /// Return a List of Enums values for the Passed Type
  /// </summary>
  /// <param name="enumType"></param>
  /// <returns></returns>
  public static List<VerboseEnum> getNames(Type enumType)
  {
    List<VerboseEnum> VEList = new List<VerboseEnum>();
    Array x = Enum.GetValues(enumType);
    foreach (Enum y in x)
    {
      if (includeEnum(enumType, y))
      {
        VEList.Add(new VerboseEnum() { Value = y });
      }
    }
    return VEList;
  }

  /// <summary>
  /// Return a List of Enums values for the passed member info.
  /// </summary>
  /// <param name="enumType"></param>
  /// <param name="memberInfo"></param>
  /// <returns></returns>
  public static List<VerboseEnum> getNames(Type enumType, MemberInfo[] memberInfo)
  {
    List<VerboseEnum> VEList = new List<VerboseEnum>();
    foreach (MemberInfo mi in memberInfo)
    {
      Enum e = (Enum)Enum.Parse(enumType, mi.Name);
      if (includeEnum(enumType, e))
        VEList.Add(new VerboseEnum() { Value = e });
    }
    return VEList;
  }

  private static bool includeEnum(Type enumType, Enum e)
  {
#if !__XAMARIN__
    //FieldInfo fi = enumType.GetField(e.ToString());
    //StarrPos.Common.Src.Objects.SecurablePosFunctionAttribute[] sattr = (StarrPos.Common.Src.Objects.SecurablePosFunctionAttribute[])fi.GetCustomAttributes(typeof(StarrPos.Common.Src.Objects.SecurablePosFunctionAttribute), false);
    //if (sattr != null && sattr.Length > 0 && !sattr[0].SecurableFunction)
    //  return false;
    //StarrPos.Common.Src.Objects.InternalPosFunctionAttribute[] attr = (StarrPos.Common.Src.Objects.InternalPosFunctionAttribute[])fi.GetCustomAttributes(typeof(StarrPos.Common.Src.Objects.InternalPosFunctionAttribute), false);
    //if (attr != null && attr.Length > 0 && attr[0].InternalUse && sattr == null)
    //  return false;
#endif
    return true;
  }
}
}
