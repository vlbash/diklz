using App.Core.Data.Enums;

namespace App.Core.Data.Interfaces
{
    public interface IDocumentHelper
    {
        string GetRegNumber(string entityName, RegNumberCounterType counterType, NumberCounterPattern pattern);
        void SetRegNumber(IDocument document, string entityName = null, RegNumberCounterType counterType = RegNumberCounterType.Increment
            , NumberCounterPattern pattern = NumberCounterPattern.Number);
    }
}
