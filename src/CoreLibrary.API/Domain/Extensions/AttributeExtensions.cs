using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CoreLibrary.API.Domain.Extensions;

[ExcludeFromCodeCoverage]
public static class AttributeExtension
{
    public static T? GetCustomAttributeOfType<T>(this PropertyInfo property) where T : Attribute
    {
        var attribute = property.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(T));
        if (attribute == null) return default;
        return (T)attribute;
    }
    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
    public static T? GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var member = enumVal.GetType().GetMember(enumVal.ToString()).FirstOrDefault();
        if (member == null) return default;
        var attribute = member.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        if (attribute == null) return default;
        return (T)attribute;
    }
    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type string that exists on the enum value</returns>
    /// <example><![CDATA[string desc = myEnumVariable.EnumDescription()]]></example>
    public static string? Description(this Enum enumVal)
    {
        var member = enumVal.GetType().GetMember(enumVal.ToString()).FirstOrDefault();
        if (member == null) return default;
        var attribute = member.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
        if (attribute == null) return default;
        return ((DescriptionAttribute)attribute)?.Description;
    }
}