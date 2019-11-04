using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using App.Core.Common.Helpers;

namespace App.Core.Common.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetPropertyGenericArguments(this Type sourceType, Type propertyType, int argumentNumber = 0)
        {
            if (argumentNumber < 0) {
                throw new ArgumentException("Argument " + nameof(argumentNumber) + " cannot be less than zero");
            };

            var typeProperties = sourceType.GetPublicInstanceProperties();
            return ReflectionHelper.GetPropertyGenericArguments(typeProperties, propertyType, argumentNumber);
        }

        public static PropertyInfo[] GetPublicInstanceProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance) ?? new PropertyInfo[0];
        }
    }
}
