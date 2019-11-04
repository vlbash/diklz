using Astum.Core.Data.Enums;

namespace Astum.Core.Data.Interfaces
{
    public interface IDocumentHelper
    {
        string GetRegNumber(string entityName, string counterType, string pattern);
        void SetRegNumber(IDocument document, string entityName = null, RegNumberCounterType counterType = RegNumberCounterType.Increment, string pattern = null);
    }
}
