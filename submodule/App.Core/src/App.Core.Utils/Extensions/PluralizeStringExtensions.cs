namespace App.Core.Utils.Extensions
{
    //
    // Summary:
    //     String extensions for choosing a singular/plural form from an integer.
    public static class PluralizeStringExtensions
    {
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
                return singular;
            else if ((x == 2 || x == 3 || x == 4) && !dec)
                return plural1;
            else return plural2;
        }
    }
}
