using System;
using System.Text.RegularExpressions;

namespace App.Core.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        /// <summary>
        /// Extended version of the string.Contains() method, 
        /// accepting a [StringComparison] object to perform different kind of comparisons
        /// This extension method actually is present in .net core and .net framework
        /// but is absent in .net standard 2.0.3 as for now
        /// </summary>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }

        //
        // Summary:
        //     Choose either the singular or plural form of a word based on an integer value.
        //
        // Parameters:
        //   num:
        //
        //   singular:
        //     Singular version of the word.
        //
        //   plural1:
        //     Plual version of the word for 2,3,4.
        //
        //   plural2:
        //     Plual version of the word for 5-9.
        public static string Pluralize(this int num, string singular, string plural1, string plural2)
        {
            return ((long)num).Pluralize(singular, plural1, plural2);
        }

        //
        // Summary:
        //     Choose either the singular or plural form of a word based on an integer value.
        //
        // Parameters:
        //   num:
        //
        //   singular:
        //     Singular version of the word.
        //
        //   plural1:
        //     Plual version of the word for 2,3,4.
        //
        //   plural2:
        //     Plual version of the word for 5-9.
        public static string Pluralize(this long num, string singular, string plural1, string plural2)
        {
            var x = num % 10;
            var dec = num / 10 == 1;
            if (x == 1 && !dec)
            {
                return singular;
            }
            else if ((x == 2 || x == 3 || x == 4) && !dec)
            {
                return plural1;
            }
            else
            {
                return plural2;
            }
        }
    }
}
