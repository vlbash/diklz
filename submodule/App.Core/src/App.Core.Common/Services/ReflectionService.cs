using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Core.Common.Extensions;
using App.Core.Common.Helpers;

namespace App.Core.Common.Services
{
    public class ReflectionService
    {
        private static readonly ConcurrentDictionary<string, PropertyInfo[]> _typeProperties
            = new ConcurrentDictionary<string, PropertyInfo[]>();
        //private static readonly List<string> _exceptPropList = new List<string>()
        //    {
        //        "Id",
        //        "RecordState",
        //        "ModifiedBy",
        //        "ModifiedOn",
        //        "CreatedBy",
        //        "CreatedOn"
        //    };

        /// <summary>
        /// Gets for source type all generic public nonstatic properties of the specified type and extracts generic argument type.
        /// Stores type properties to inner in-memory storage for further quick access
        /// </summary>
        /// <param name="sourceType">Type, which properties are needed</param>
        /// <param name="propertyType">Generic type of needed properties. For example, typeof(List<>)</param>
        /// <param name="argumentNumber">Number of property type generic argument (default = 0)</param>
        /// <returns>Collection of generic arguments types.</returns>
        public IEnumerable<Type> GetPropertyGenericArguments(Type sourceType, Type propertyType, int argumentNumber = 0)
        {
            if (argumentNumber < 0) {
                throw new ArgumentException("Argument " + nameof(argumentNumber) + " cannot be less than zero");
            };

            var typeProperties = GetInnerProperties(sourceType);
            return ReflectionHelper.GetPropertyGenericArguments(typeProperties, propertyType, argumentNumber);
        }

        /// <summary>
        /// Gets all public nonstatic properties and stores them to inner in-memory storage for further quick access
        /// </summary>
        /// <param name="type">Type, which properties are needed</param>
        /// <returns>Property info array for every type public nonstatic property</returns>
        public PropertyInfo[] GetTypeProperties(Type type)
        {
            var properties = GetInnerProperties(type);
            return properties;
        }

        private PropertyInfo[] GetInnerProperties(Type sourceType)
        {
            var key = sourceType.FullName;
            if (!_typeProperties.TryGetValue(key, out var typeProperties)) {
                typeProperties = sourceType.GetPublicInstanceProperties();
                _typeProperties.TryAdd(key, typeProperties);
            }
            return typeProperties;
        }
    }
}
