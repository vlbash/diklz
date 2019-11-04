using System;
using System.Reflection;

namespace App.Core.Data.Helpers
{
    public static class PropertyHelper
    {
        public static string GetValueByPropType(PropertyInfo prop, object item)
        {
            string value;
            switch (prop.PropertyType.ToString())
            {
                case "System.Boolean":
                    value = prop.GetValue(item, null).ToString() == "True" ? "так" : "ні";
                    break;
                case "System.Nullable`1[System.DateTime]":
                    value = prop.GetValue(item, null) != null ? ((DateTime)prop.GetValue(item, null)).ToString("dd.MM.yyyy") : "";
                    break;
                case "System.DateTime":
                    value = ((DateTime)prop.GetValue(item, null)).ToString("dd.MM.yyyy");
                    break;
                case "System.Enum":
                    value = prop.GetValue(item, null).ToString();
                    break;
                default:
                    var propValue = prop.GetValue(item, null);
                    value = propValue == null ? string.Empty : propValue.ToString();

                    break;
            }
            return value;
        }
    }
}
