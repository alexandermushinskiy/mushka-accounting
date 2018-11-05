using System;
using System.ComponentModel;

namespace Mushka.Core.Extensions
{
    public static class EnumerationExtensions
    {
        public static string GetDescription(this Enum enumerationValue)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])enumerationValue
                .GetType()
                .GetField(enumerationValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}