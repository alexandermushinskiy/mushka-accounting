using System;

namespace Mushka.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool CaseInsensitiveContains(this string text, string value)
        {
            return text.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}