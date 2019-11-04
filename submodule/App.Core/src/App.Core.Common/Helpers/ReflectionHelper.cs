using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace App.Core.Common.Helpers
{
    public static class ReflectionHelper
    {
        private static List<Type> _EntityTypeList = null;
        private static List<Type> _EnumList = null;


        private static List<Type> EntityList
        {
            get
            {
                if (_EntityTypeList == null)
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies();
                    var myassembly = assembly.Where(x => x.FullName.Contains("App.Data,") || x.FullName.Contains("Astum.Core.Data,") || x.FullName.Contains("Astum.Core.Common,")).ToList();
                    var typeList = myassembly
                        .SelectMany(t => t.GetTypes())
                        .Where(t => t.IsClass).ToList();

                    _EntityTypeList = typeList.Where(t => t.Namespace != null && t.Namespace.Contains("Entities")).ToList();
                };

                return _EntityTypeList;
            }
        }

        public static List<Type> EnumList
        {
            get
            {
                if (_EnumList == null)
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies();
                    var typeList = assembly
                        .SelectMany(t => t.GetTypes())
                        .Where(t => t.IsEnum).ToList();

                    _EnumList = typeList.Where(t => t.Namespace != null).ToList();
                };

                return _EnumList;
            }
        }

        public static IEnumerable<Type> GetPropertyGenericArguments(PropertyInfo[] infos, Type propertyType, int argumentNumber = 0)
        {
            if (argumentNumber < 0) {
                throw new ArgumentException("Argument " + nameof(argumentNumber) + " cannot be less than zero");
            };

            return infos
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == propertyType)
                .Select(prop => prop.PropertyType.GetGenericArguments()[argumentNumber]);
        }
    }
}
