using System.ComponentModel.DataAnnotations.Schema;
using Z.EntityFramework.Plus;

namespace App.Core.Data.CustomAudit
{
    public class AuditEntry_Extended : AuditEntry
    {
        [NotMapped]
        // as for now this function doesn't exist in netstandard 2.0
        //public string StateNameUa { get {return AuditHelper.AuditEntryStateUa.GetValueOrDefault(State); } }
        public string StateNameUa
        {
            get
            {
                if (AuditHelper.AuditEntryStateUa.TryGetValue(State, out var retValue)) {
                    return retValue;
                }
                return "";
            }
        }
    }
}
