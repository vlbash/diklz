using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using App.Core.Common.Extensions;
using Serilog;

namespace App.Business.Extensions
{
    public static class ExtensionMethods
    {
        public static string GetDisplayName(this PropertyInfo prop, bool isEmptyPropName = false)
        {
            if (prop.CustomAttributes == null || !prop.CustomAttributes.Any())
            {
                return !isEmptyPropName ? prop.Name : string.Empty;
            }

            var displayNameAttribute = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DisplayNameAttribute));

            if (displayNameAttribute?.ConstructorArguments == null ||
                displayNameAttribute.ConstructorArguments.Count == 0)
            {
                return !isEmptyPropName ? prop.Name : string.Empty;
            }

            return displayNameAttribute.ConstructorArguments[0].Value.ToString() ?? (!isEmptyPropName ? prop.Name : string.Empty) ;
        }

        public static bool TryGetPropValue(this object obj, string propName, out object objModel)
        {
            try
            {
                objModel = obj.GetPropValue(propName);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                objModel = null;
                return false;
            }
        }
    }
}
