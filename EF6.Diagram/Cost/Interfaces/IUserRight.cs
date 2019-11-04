using System.Collections.Generic;

namespace Astum.Core.Data.Interfaces
{
    public interface IUserRight
    {
        Dictionary<string, string> Properties { get; set; }
        string Entity { get; set; }
        bool IsActive { get; set; }
        string Name { get; set; }
    }
}