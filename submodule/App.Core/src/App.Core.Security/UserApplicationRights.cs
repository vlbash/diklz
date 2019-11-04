using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using App.Core.Base;

namespace App.Core.Security
{
    /// <summary>
    /// Represents all user's rights in an application during work
    /// </summary>
    public class UserApplicationRights
    {
        /// <summary>
        /// Entity rights dictionary, where key - is the name of entity
        /// </summary>
        public Dictionary<string, EntityRightData> EntityRights { get; set; }
        /// <summary>
        /// Row level rights dictionary, where key - is the name of entity
        /// </summary>
        public Dictionary<string, RowLevelRightData> RowLevelRights { get; set; }
        /// <summary>
        /// Dictionary - whose values is entity name list. These entities will be included into rights check.
        /// </summary>
        private static readonly Dictionary<Type, List<string>> _checkedEntites = new Dictionary<Type, List<string>>();
        /// <summary>
        /// Represents model properties, that should be checked by rls system.
        /// </summary>
        private static readonly Dictionary<Type, List<RlsPropertyData>> _rlsProperties = new Dictionary<Type, List<RlsPropertyData>>();

        /// <summary>
        /// TEMP. TODO: DELETE
        /// </summary>
        public bool HasFullRights { get; set; }

        /// <summary>
        /// Function is a combination of AssertWritableEntity(string entityName) 
        /// and AssertRlsAllowsEntity(object obj) methods
        /// </summary>
        /// <exception cref="NoRightsException">Throws this exception if 
        /// there is no write right to entity or rls doesn't allow write current object
        /// </exception>
        /// <param name="entityName"></param>
        /// <param name="dto"></param>
        public void AssertCanWriteEntity(string entityName, object obj)
        {
            if (HasFullRights)
            {
                return;
            }
            AssertWritableEntity(entityName);
            AssertRlsAllowsObject(obj);
        }

        public void AssertCanReadTypeData(Type dataType)
        {
            if (HasFullRights)
            {
                return;
            }

            if (!CanReadTypeData(dataType))
            {
                throw new NoRightsException($"Access to {dataType.Name} is denied");
            }
        }

        public void AssertWritableEntity(string entityName)
        {
            if (!IsWritableEntity(entityName))
            {
                throw new NoRightsException($"Write access to {entityName} is denied");
            }
        }

        public void AssertRlsAllowsObject(object obj)
        {
            if (!RlsAllowsAccessToObject(obj))
            {
                throw new NoRightsException($"Row level access to {obj.GetType().Name} is denied");
            }
        }

        public void AssertWriteRights(string entityName, string fieldName)
        {
            var fieldRight = GetFieldRight(entityName, fieldName);
            if (fieldRight != AccessLevel.Write)
            {
                throw new NoRightsException("There is no permission to perform this operation on field " + fieldName);
            }
        }

        // TODO: think about further ways to optimize getting an object property value
        public bool RlsAllowsAccessToObject(object obj)
        {
            if (HasFullRights)
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            var type = obj.GetType();
            if (!TryGetPropertyRights(type, out var properties))
            {
                return false;
            }

            foreach (var propData in properties)
            {
                if (propData?.PropertyInfo != null)
                {
                    var propValue = "";
                    if (propData.PropertyInfo.PropertyType == typeof(Guid)) {
                        var guid = (Guid)propData.PropertyInfo.GetValue(obj);
                        if (guid != Guid.Empty)
                        {
                            propValue = guid.ToString();
                        }
                    } else
                    {
                        propValue = propData.PropertyInfo.GetValue(obj).ToString();
                    }

                    if (!RlsAllowsAccessToEntity(propData.EntityName, propValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool RlsAllowsAccessToEntity(string entityName, string entityId)
        {
            if (HasFullRights)
            {
                return true;
            }

            if (string.IsNullOrEmpty(entityName) || string.IsNullOrEmpty(entityId) || !RowLevelRights.TryGetValue(entityName, out var rlsRight))
            {
                return true;
            }

            if (rlsRight.PermissionType == RowLevelModelPermissionType.All)
            {
                return true;
            }
            else if (rlsRight.PermissionType == RowLevelModelPermissionType.Specified)
            {
                //return rlsRight.Entities.Exists(x => entityId.Contains(x));
                return rlsRight.Entities.Contains(entityId);
            }
            else if (rlsRight.PermissionType == RowLevelModelPermissionType.Except)
            {
                //return !rlsRight.Entities.Exists(x => entityId.Contains(x));
                return !rlsRight.Entities.Contains(entityId);
            }

            return false;
        }

        public bool IsWritableEntity(string entityName)
        {
            if (HasFullRights)
            {
                return true;
            }

            if (!TryGetEntityRight(entityName, out var entityRight))
            {
                return false;
            }

            if (entityRight.EntityAccessLevel == EntityAccessLevel.Write)
            {
                return true;
            }
            else if (entityRight.EntityAccessLevel == EntityAccessLevel.Partial)
            {
                // TODO: think, what to do
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if there is an access to the given type
        /// The check perfomed, using RightsCheckList attribute
        /// If there is no such attribute on the given type, access is granted
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public bool CanReadTypeData(Type dataType)
        {
            if (HasFullRights)
            {
                return true;
            }

            if (dataType == null)
            {
                throw new ArgumentNullException();
            }

            var entitiesToCheck = GetEntitiesToCheck(dataType);
            if (entitiesToCheck == null || entitiesToCheck.Count == 0)
            {
                return true;
            }

            foreach (var entityName in entitiesToCheck)
            {
                if (!EntityRights.TryGetValue(entityName, out var right) || right.EntityAccessLevel == EntityAccessLevel.No)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets all names of entities set in RightsCheckList attribute
        /// Includes entity itself if it's present in entities rights
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private List<string> GetEntitiesToCheck(Type dataType)
        {
            if (!_checkedEntites.TryGetValue(dataType, out var entitiesToCheck))
            {
                foreach (var custAttr in dataType.GetCustomAttributes(false))
                {
                    if (custAttr is RightsCheckListAttribute rightsAttr)
                    {
                        entitiesToCheck = rightsAttr.CheckedEntities;
                    }
                }

                if (EntityRights.ContainsKey(dataType.Name))
                {
                    if (entitiesToCheck == null)
                    {
                        entitiesToCheck = new List<string>(1) { dataType.Name };
                    } else
                    {
                        entitiesToCheck.Add(dataType.Name);
                    }
                }
                _checkedEntites[dataType] = entitiesToCheck;
            }

            return entitiesToCheck;
        }

        private bool TryGetEntityRight(string entityName, out EntityRightData right)
        {
            right = null;
            if (EntityRights == null || string.IsNullOrEmpty(entityName))
            {
                return false;
            }

            return EntityRights.TryGetValue(entityName, out right);
        }

        /// <summary>
        /// The method extracts data from RlsRight attributes
        /// For example:
        /// [RlsRight("Region", "RegionId")] 
        /// creates RlsPropertyData instance with EntityName = "Region" and PropertyName = "RegionId"
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propRights"></param>
        /// <returns></returns>
        private bool TryGetPropertyRights(Type type, out List<RlsPropertyData> propRights)
        {
            if (_rlsProperties.TryGetValue(type, out propRights))
            {
                return true;
            }

            propRights = new List<RlsPropertyData>();
            foreach (RlsRightAttribute rightsAttr in type.GetCustomAttributes(typeof(RlsRightAttribute), false))
            {
                if (!string.IsNullOrEmpty(rightsAttr.TypeName) && !string.IsNullOrEmpty(rightsAttr.PropertyName))
                {
                    var propertyInfo = type.GetProperty(rightsAttr.PropertyName);
                    //if (propertyInfo.PropertyType == typeof(Guid))
                    //{
                        var propData = new RlsPropertyData
                        {
                            EntityName = rightsAttr.TypeName,
                            PropertyName = rightsAttr.PropertyName,
                            PropertyInfo = propertyInfo
                        };
                        propRights.Add(propData);
                    //}
                }
            }

            _rlsProperties[type] = propRights;

            return true;
        }

        /// <summary>
        /// Gets AccessLevel for entity field
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public AccessLevel GetFieldRight(string entityName, string fieldName)
        {
            if (HasFullRights)
            {
                return AccessLevel.Write;
            }

            if (!TryGetEntityRight(entityName, out var right))
            {
                return AccessLevel.No;
            }

            if (right.EntityAccessLevel == EntityAccessLevel.Partial && !string.IsNullOrEmpty(fieldName))
            {
                if (!right.FieldRights.TryGetValue(fieldName, out var fieldRight))
                {
                    return AccessLevel.No;
                }
                else
                {
                    return fieldRight;
                }
            }
            else
            {
                var enumConverter = new EnumConverter();
                return enumConverter.ToAccessLevel(right.EntityAccessLevel);
            }
        }

        /// <summary>
        /// Gets EntityAccessLevel by entity name
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public EntityAccessLevel GetEntityAccessLevel(string entityName)
        {
            if (HasFullRights)
            {
                return EntityAccessLevel.Write;
            }

            if (!TryGetEntityRight(entityName, out var right))
            {
                return EntityAccessLevel.No;
            }

            return right.EntityAccessLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<RowLevelRightData> GetTypeFieldsRlsRights(Type dataType)
        {
            if (dataType == null)
            {
                throw new ArgumentNullException();
            }

            var rlsList = new List<RowLevelRightData>();
            if (!TryGetPropertyRights(dataType, out var rights))
            {
                return rlsList;
            }

            foreach (var propRight in rights)
            {
                var rlsRight = new RowLevelRightData
                {
                    Name = propRight.PropertyName,
                    PermissionType = RowLevelModelPermissionType.All,
                    Entities = new List<string>(0)
                };
                if (RowLevelRights.TryGetValue(propRight.EntityName, out var right))
                {
                    rlsRight.PermissionType = right.PermissionType;
                    rlsRight.Entities.AddRange(right.Entities);
                }
                if (HasFullRights)
                {
                    rlsRight.PermissionType = RowLevelModelPermissionType.All;
                }
                rlsList.Add(rlsRight);
            }

            return rlsList;
        }

        private class RlsPropertyData
        {
            public string EntityName { get; set; }
            public string PropertyName { get; set; }
            public PropertyInfo PropertyInfo { get; set; }
        }
    }
}
