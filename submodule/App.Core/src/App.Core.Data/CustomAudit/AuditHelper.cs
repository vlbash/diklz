// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

using App.Core.Data;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace App.Core.Data.CustomAudit
{
    public static class AuditHelper
    {
        public static Dictionary<AuditEntryState, string> AuditEntryStateUa { get; set; } = new Dictionary<AuditEntryState, string>()
        {
            { AuditEntryState.EntityAdded, "Додано" },
            { AuditEntryState.EntityDeleted, "Видалено" },
            { AuditEntryState.EntityModified, "Змінено" },
            { AuditEntryState.EntityCurrent, "Актуалізовано" },
            { AuditEntryState.EntitySoftAdded, "Додано (soft)" },
            { AuditEntryState.EntitySoftDeleted, "Видалено (soft)" },
            { AuditEntryState.RelationshipAdded, "Відносини додано" },
            { AuditEntryState.RelationshipDeleted, "Відносини видалено" }
        };

        public static Audit AutoSaveWithAuditEntryFactory(string createdBy)
        {
            var audit = new Audit();
            audit.Configuration.AutoSavePreAction = (context, audit1) =>
            {
                //(context as CoreDbContext).AuditEntries.AddRange(audit1.Entries.Cast<AuditEntry_Extended>());
                (context as CoreDbContext).AuditEntries.AddRange(audit1.Entries);
            };

            audit.Configuration.AuditEntryFactory = args =>
            {
                //return new AuditEntry_Extended
                return new AuditEntry
                {
                    CreatedBy = createdBy
                };
            };

            return audit;
        }
    }
}
