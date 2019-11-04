using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Base;

namespace App.Core.Security
{
    public class RowLevelRightData
    {
        /// <summary>
        /// Name of the entity or field, that represents the entity (depends on class usage)
        /// </summary>
        public string Name { get; set; }
        public RowLevelModelPermissionType PermissionType { get; set; }
        public List<string> Entities { get; set; }
    }
}
