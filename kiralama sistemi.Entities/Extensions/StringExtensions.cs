using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace kiralamaSistemi.Entities.Extensions
{
    public static class StringExtensions
    {
        public static string Clean(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return value.Replace("\r\n", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace("\t", string.Empty)
                        .Replace(lineSeparator, string.Empty)
                        .Replace(paragraphSeparator, string.Empty);
        }
        public static string RemoveWhiteSpaces(this string s)
        {
            if (s == null)
            {
                return s;
            }
            s = s.TrimStart().TrimEnd();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            s = regex.Replace(s, " ");
            return s;
        }
        public static int TryToInt(this string? s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int TryToInt(this int? s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static decimal TryToDecimal(this string s)
        {
            try
            {
                return Convert.ToDecimal(s);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int[] TryParseIntArray(this string s, string split)
        {
            try
            {
                if (s == null)
                {
                    return null;
                }

                string[] splitted = s.Split(split);

                if (splitted.Length > 0)
                {
                    return Array.ConvertAll(splitted, i => int.Parse(i));
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string TryToEmptyString(this string s)
        {
            return s == null ? string.Empty : s.ToString();
        }
        public static string TryToString(this int s)
        {
            return s == 0 ? null : s.ToString();
        }
        public static string TryToString(this decimal s)
        {
            return s == 0 ? null : s.ToString();
        }
        public static string TryToString(this DateTime s)
        {
            return s.ToString();
        }
        public static string TryToString(this byte s)
        {
            return s == 0 ? null : s.ToString();
        }
        public static string NormalizeKey(this string s)
        {
            if (s == null)
            {
                return null;
            }
            return s.Normalize().ToUpperInvariant();
        }
        public static string Range(this string s, string startWith = null, string endWith = null)
        {

            if (s == null || (String.IsNullOrWhiteSpace(startWith) && String.IsNullOrWhiteSpace(endWith)))
            {
                return null;

            }
            else if (String.IsNullOrWhiteSpace(endWith))
            {
                return s.Substring(s.IndexOf(startWith) + 1);

            }
            else if (String.IsNullOrWhiteSpace(startWith))
            {
                return s.Substring(0, s.IndexOf(endWith) - 1);
            }
            else if (!String.IsNullOrWhiteSpace(startWith) && !String.IsNullOrWhiteSpace(endWith) && s.Contains(startWith) && s.Contains(endWith))
            {
                return s.Substring(s.IndexOf(startWith) + 1, s.IndexOf(endWith) - s.IndexOf(startWith) - 1);
            }
            else
            {
                return null;
            }
        }
        public static string GetTitleFromModelState(this string s)
        {
            return Range(s, "[", "]");
        }
        public static string GetDescriptionFromModelState(this string s)
        {

            //Header
            string range;
            if (s.Contains("]") && s.Contains("{")) // head
            {
                range = Range(s, "]", "{");

            }
            else if (s.Contains("]")) // code & head
            {
                range = Range(s, "]");
            }
            else if (s.Contains("{")) // code & head  
            {
                range = Range(s, null, "{");
            }
            else
            {
                range = s;
            }
            return range;

        }
        public static string GetCodeFromModelState(this string s)
        {
            return Range(s, "{", "}");
        }
        public static string DecimalToStringNumber(this decimal number)
        {
            if (Decimal.Compare(number, 0) == 0)
            {
                return "0.00";
            }

            return Math.Round(number, 2).ToString().Replace(',', '.');
        }
        public static string GetMonth(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return String.Empty;
            }

            var Months = new[] { String.Empty, "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            try
            {
                var index = Convert.ToByte(s);
                return Months[index];
            }
            catch (Exception)
            {

                return String.Empty;
            }

        }
        public static double DecimalTryToDouble(this decimal s)
        {
            try
            {
                return Convert.ToDouble(s);
            }
            catch (Exception)
            {
                return 0.00;
            }
        }
        public static string DecimalToStringWithComma(this decimal s)
        {
            try
            {
                return Convert.ToString(s).Replace(",", ".");
            }
            catch (Exception)
            {
                return "0.00";
            }
        }
        public static string? CumleYildizla(this string cumle, int bas = 2, int son = 0)
        {
            try
            {
                if (cumle == null)
                {
                    return null;
                }
                var ad = cumle?.Split(" ");
                for (int k = 0; k < ad?.Length; k++)
                {
                    ad[k] = KelimeYildizla(ad[k], bas, son) ?? "";
                }

                return String.Join(" ", ad ?? Array.Empty<string>());
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string KelimeYildizla(this string kelime, int bas, int son = 0)
        {
            try
            {
                if (kelime == null) return null;
                if (kelime.Length <= bas + son)
                {
                    return kelime;
                }
                return kelime[..bas] + new string('*', kelime.Length - bas - son) + kelime?[(kelime.Length - son)..];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string Random(string v)
        {
            return v + new Random().Next(9999);
        }

    }
}
