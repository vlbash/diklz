using System;
using System.Reflection;

namespace App.Core.ReportManager
{
    public static class PropertyHelper
    {
        public static string GetValueByPropType(PropertyInfo item, object listItem)
        {
            string value;
            switch (item.PropertyType.ToString())
            {
                case "System.Boolean":
                    value = item.GetValue(listItem, null).ToString() == "True" ? "так" : "не";
                    break;
                case "System.Nullable`1[System.DateTime]":
                    value = item.GetValue(listItem, null) != null ? ((DateTime)item.GetValue(listItem, null)).ToString("dd.MM.yyyy") : "";
                    break;
                case "System.DateTime":
                    value = ((DateTime)item.GetValue(listItem, null)).ToString("dd.MM.yyyy");
                    break;
                case "System.Enum":
                    value = item.GetValue(listItem, null).ToString();
                    break;
                default:
                    value = item.GetValue(listItem, null).ToString();
                    break;
            }
            return value;
        }
    }
}
