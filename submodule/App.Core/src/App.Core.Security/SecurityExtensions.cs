using System;
using System.Collections.Generic;
using System.Linq;
using App.Core.Base;
using App.Core.Security.Entities;

namespace App.Core.Security
{
    public static class SecurityExtensions
    {
        public static UserApplicationRights GetUserRights(this ISecurityDbContext context, Guid profileId, Guid? userId = null)
        {
            var rights = new UserApplicationRights
            {
                EntityRights = new Dictionary<string, EntityRightData>(),
                RowLevelRights = new Dictionary<string, RowLevelRightData>()
            };

            if (profileId == Guid.Empty)
            {
                return rights;
            }

            rights.EntityRights = GetEntityRightsModel(context, profileId);
            rights.RowLevelRights = GetRowLevelRightsModel(context, profileId, userId);

            ChangeEntityRightsAccordingToRlsRights(rights);

            return rights;
        }

        private static void ChangeEntityRightsAccordingToRlsRights(UserApplicationRights rights)
        {
            if (rights.EntityRights == null || rights.RowLevelRights == null)
            {
                return;
            }

            foreach (var rowLevelRight in rights.RowLevelRights)
            {
                var rlsRight = rowLevelRight.Value;
                if (rlsRight.PermissionType == RowLevelModelPermissionType.No)
                {
                    if (rights.EntityRights.TryGetValue(rlsRight.Name, out var entRight))
                    {
                        entRight.EntityAccessLevel = EntityAccessLevel.No;
                    }
                }
            }
        }

        private static List<IGrouping<string, EntityRights>> GetEntityRights(ISecurityDbContext context, Guid profileId)
        {
            var rightsQuery = (from prole in context.ProfileRoles
                              .Where(pr => pr.ProfileId == profileId && pr.RecordState != RecordState.D)
                               join role in context.Roles.Where(r => r.IsActive && r.RecordState != RecordState.D)
                               on prole.RoleId equals role.Id
                               join rright in context.RoleRights.Where(rr => rr.RecordState != RecordState.D)
                               on role.Id equals rright.RoleId
                               join right in context.Rights.Where(r => r.IsActive && r.RecordState != RecordState.D)
                               on rright.RightId equals right.Id
                               select new { RoleId = role.Id, RightId = right.Id, right.EntityAccessLevel, right.EntityName })
                               .Union(from pright in context.ProfileRights

                                   .Where(pr => pr.ProfileId == profileId && pr.RecordState != RecordState.D)
                                      join right in context.Rights.Where(r => r.IsActive && r.RecordState != RecordState.D)
                                      on pright.RightId equals right.Id
                                      select new { RoleId = Guid.Empty, RightId = right.Id, right.EntityAccessLevel, right.EntityName });
            var entityRights = rightsQuery.GroupJoin(context.FieldRights,
                                r => new { r.RightId, r.EntityAccessLevel },
                                fr => new { fr.RightId, EntityAccessLevel = EntityAccessLevel.Partial },
                                (r, fr) => new EntityRights
                                {
                                    EntityAccessLevel = r.EntityAccessLevel,
                                    EntityName = r.EntityName,
                                    FieldRights = fr.Select(f => new FieldRights { FieldName = f.FieldName, AccessLevel = f.AccessLevel }).ToList()
                                })
                                //.Select(r => new { r.EntityName, r.EntityAccessLevel, r.FieldRights })
                                .GroupBy(g => g.EntityName)
                                .ToList();

            return entityRights;

        }

        private static Dictionary<string, EntityRightData> GetEntityRightsModel(ISecurityDbContext context, Guid profileId)
        {
            var entityRights = GetEntityRights(context, profileId);
            var entityRightsModel = new Dictionary<string, EntityRightData>();
            foreach (var right in entityRights)
            {
                var maxAccessLevel = right.Max(el => el.EntityAccessLevel);
                var entityRight = new EntityRightData
                {
                    EntityName = right.Key,
                    EntityAccessLevel = maxAccessLevel
                };
                entityRightsModel.Add(right.Key, entityRight);

                if (maxAccessLevel != EntityAccessLevel.Partial)
                {
                    continue;
                }

                // in case of partial access we need combining rights in a special way
                // if there is role with full entity read access, then we need reflection to get all the properties
                var fieldRights = new Dictionary<string, AccessLevel>();
                foreach (var fields in right)
                {
                    if (fields.EntityAccessLevel == EntityAccessLevel.Partial)
                    {
                        foreach (var field in fields.FieldRights)
                        {
                            fieldRights.Add(field.FieldName, field.AccessLevel);
                        }
                    }
                    else if (fields.EntityAccessLevel == EntityAccessLevel.Read)
                    {
                        var properties = context.GetApplicationModels()
                            .Where(model => model.Name == fields.EntityName)
                            .FirstOrDefault()
                            ?.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if (properties != null)
                        {
                            foreach (var prop in properties)
                            {
                                fieldRights.Add(prop.Name, AccessLevel.Read);
                            }
                        }
                    }
                }

                var combinedFieldRights = fieldRights.GroupBy(kv => kv.Key)
                    .Select(g => new { FieldName = g.Key, AccessLevel = g.Max(el => el.Value) })
                    .Where(kv => kv.AccessLevel != AccessLevel.No)
                    .ToDictionary(g => g.FieldName, g => g.AccessLevel);

                if (combinedFieldRights.Count == 0)
                {
                    entityRight.EntityAccessLevel = EntityAccessLevel.No;
                }
                else
                {
                    entityRight.FieldRights = combinedFieldRights;
                }


            }
            return entityRightsModel;
        }

        private static List<RlsRights> GetRlsRights(ISecurityDbContext context, Guid profileId)
        {
            var rlsRightsList = (from usedrls in context.ApplicationRowLevelRights.Where(r => r.IsActive)

                                 join rls in context.RowLevelRights.Where(el => el.ProfileId == profileId)
                                 on usedrls.EntityName equals rls.EntityName into appRowLevelRights
                                 from apprls in appRowLevelRights.DefaultIfEmpty()

                                 join rlsobj in context.RowLevelSecurityObjects
                                 on apprls.Id equals rlsobj.RowLevelRightId into secObjects
                                 from securityObject in secObjects.DefaultIfEmpty()

                                 select new RlsRights
                                 {
                                     EntityName = usedrls.EntityName,
                                     // because of left join AccessType actually CAN be null, so disable this warning
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                                     AccessType = apprls.AccessType == null ? RowLevelAccessType.No : apprls.AccessType,
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                                     EntityId = (securityObject.EntityId == null ? Guid.Empty : securityObject.EntityId)
                                 })
                                 .ToList();

            return rlsRightsList;
        }

        private static Dictionary<string, Guid> GetDefaultValues(ISecurityDbContext context, List<RlsRights> rlsRights, Guid? userId)
        {
            var needDefaultValues = userId.HasValue
                   && rlsRights.Select(el => el.AccessType)
                   .Where(el => el == RowLevelAccessType.Default)
                   .Any();

            var defaultValues = new Dictionary<string, Guid>();
            if (needDefaultValues)
            {
                defaultValues = context.EmployeeDefaultValues
                    .Where(el => el.UserId == userId.Value)
                    .GroupBy(el => el.EntityName)
                    .Select(g => new { EntityName = g.Key, ValueId = g.Select(el => el.ValueId).FirstOrDefault() })
                    .ToDictionary(k => k.EntityName, v => v.ValueId);
            }

            return defaultValues;
        }

        private static Dictionary<string, RowLevelRightData> GetRowLevelRightsModel(ISecurityDbContext context, Guid profileId, Guid? userId)
        {
            var rlsRights = GetRlsRights(context, profileId);

            var defaultValues = GetDefaultValues(context, rlsRights, userId);

            var enumConverter = new EnumConverter();
            var rightsModel = new Dictionary<string, RowLevelRightData>();

            var groupedRlsList = rlsRights.GroupBy(el => el.EntityName);
            foreach (var right in groupedRlsList)
            {
                var maxAccessType = right.Max(el => el.AccessType);
                var rlsRight = new RowLevelRightData
                {
                    Name = right.Key,
                    PermissionType = enumConverter.ToRowLevelModelPermissionType(maxAccessType),
                    Entities = new List<string>()
                };
                rightsModel.Add(right.Key, rlsRight);

                if (maxAccessType != RowLevelAccessType.No && maxAccessType != RowLevelAccessType.All)
                {
                    // in case of specific access we need combining rights in a special way
                    // if there is right "Except" we need create combined "Except" rule
                    // otherwise it will be "Specified" rule
                    var rlsExceptObjects = new List<string>();
                    var rlsIncludeObjects = new List<string>();
                    foreach (var rlsr in right)
                    {
                        if (rlsr.AccessType == RowLevelAccessType.Default)
                        {
                            if (defaultValues.TryGetValue(rlsr.EntityName, out var defValue)
                                && defValue != Guid.Empty)
                            {
                                rlsIncludeObjects.Add(defValue.ToString());
                            }
                        }
                        else if (rlsr.AccessType == RowLevelAccessType.Specified && rlsr.EntityId != Guid.Empty)
                        {
                            rlsIncludeObjects.Add(rlsr.EntityId.ToString());
                        }
                        else if (rlsr.AccessType == RowLevelAccessType.Except && rlsr.EntityId != Guid.Empty)
                        {
                            rlsExceptObjects.Add(rlsr.EntityId.ToString());
                        }
                    }

                    rlsExceptObjects = rlsExceptObjects.Distinct().ToList();
                    rlsIncludeObjects = rlsIncludeObjects.Distinct().ToList();

                    if (rlsExceptObjects.Count > 0)
                    {
                        rlsRight.Entities.AddRange(rlsExceptObjects.Except(rlsIncludeObjects));
                    }
                    else
                    {
                        rlsRight.Entities.AddRange(rlsIncludeObjects);
                    }

                    if (rlsRight.Entities.Count == 0)
                    {
                        if (rlsRight.PermissionType == RowLevelModelPermissionType.Specified)
                        {
                            rlsRight.PermissionType = RowLevelModelPermissionType.No;
                        }
                        else if (rlsRight.PermissionType == RowLevelModelPermissionType.Except)
                        {
                            rlsRight.PermissionType = RowLevelModelPermissionType.All;
                        }
                    }
                }
            }

            return rightsModel;
        }

        private class FieldRights
        {
            public string FieldName { get; set; }
            public AccessLevel AccessLevel { get; set; }
        }

        private class EntityRights
        {
            public string EntityName { get; set; }
            public EntityAccessLevel EntityAccessLevel { get; set; }
            public List<FieldRights> FieldRights { get; set; }
        }

        private class RlsRights
        {
            public RowLevelAccessType AccessType { get; set; }
            public string EntityName { get; set; }
            public Guid EntityId { get; set; }
        }
    }
}
