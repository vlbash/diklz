using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Security
{
    internal class EnumConverter
    {
        public AccessLevel ToAccessLevel(EntityAccessLevel entityAccessLevel)
        {
            switch (entityAccessLevel) {
                case EntityAccessLevel.No:
                    return AccessLevel.No;
                case EntityAccessLevel.Partial:
                case EntityAccessLevel.Read:
                    return AccessLevel.Read;
                case EntityAccessLevel.Write:
                    return AccessLevel.Write;
                default:
                    return AccessLevel.No;
            }
        }
        
        public RowLevelModelPermissionType ToRowLevelModelPermissionType(RowLevelAccessType rowLevelAccessType)
        {
            switch (rowLevelAccessType) {
                case RowLevelAccessType.No:
                    return RowLevelModelPermissionType.No;
                case RowLevelAccessType.Default:
                case RowLevelAccessType.Specified:
                    return RowLevelModelPermissionType.Specified;
                case RowLevelAccessType.Except:
                    return RowLevelModelPermissionType.Except;
                case RowLevelAccessType.All:
                    return RowLevelModelPermissionType.All;
                default:
                    return RowLevelModelPermissionType.No;
            }
        }
    }
}
