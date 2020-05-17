using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2ExplorerCraftTool
{
    static class StringHelper
    {
        public static bool Contains(string source, string pattern)
        {
            if (String.IsNullOrEmpty(source))
                return false;
            return source.RemoveDiacritics().IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
        public static bool Equals(string source, string pattern)
        {
            if (String.IsNullOrEmpty(source))
                return false;
            return source.RemoveDiacritics().Equals(pattern, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string ClearHtmlTags(this string s)
        {
            if (!s.Contains("<") && !s.Contains(">"))
                return s;

            s = s.Replace("<br>", Environment.NewLine);
            //s = s.Replace("<c=@flavor>", String.Empty);
            //s = s.Replace("<c=@reminder>", String.Empty);
            //s = s.Replace("<c=@warning>", String.Empty);
            s = System.Text.RegularExpressions.Regex.Replace(s, @"<c=@.+>", String.Empty);
            s = s.Replace("</c>", String.Empty);

            return s;
        }

        public static string RemoveDiacritics(this string s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
