using System;
using System.Globalization;

namespace App.Core.Data.Helpers
{
    public class ExpressionCompareHelper
    {
        public bool Compare(string data, string value)
        {
            if (data != null)
            {
                return CultureInfo.InvariantCulture.CompareInfo.IndexOf(data, value, CompareOptions.IgnoreCase) >= 0;
            }
            else
            {
                return false;
            }
        }

        public bool Compare(bool data, string value)
        {
            return data && value == "on";
        }

        public bool Compare(DateTime? data, string value)
        {
            return data.HasValue && data.Value.Date == DateTime.Parse(value).Date;
        }

        public bool Compare(DateTime? data, string[] value)
        {
            return data.HasValue && value.Length == 2 ? (data.Value.Date >= DateTime.Parse(value[0]) && data.Value.Date <= DateTime.Parse(value[1])) : false;
        }

        public bool Compare(long? data, string value)
        {
            return data != null && data.Value == long.Parse(value);
        }
    }
}
