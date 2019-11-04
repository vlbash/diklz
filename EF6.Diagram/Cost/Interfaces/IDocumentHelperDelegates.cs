using System;

namespace Astum.Core.Data.Interfaces
{
    public interface IDocumentHelper_GenerateRegNumber
    {
        Func<string, string> GenerateRegNumber { get; set; }
        Func<string> GetEntityNameKey { get; set; }
    }
}