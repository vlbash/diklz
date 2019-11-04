using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Base;

namespace App.Core.Security
{
    /// <summary>
    /// Represents full user's rights in an application to entities and their fields
    /// </summary>
    public class EntityRightData
    {
        public string EntityName { get; set; }
        public EntityAccessLevel EntityAccessLevel { get; set; }
        public Dictionary<string, AccessLevel> FieldRights { get; set; }
    }
}
