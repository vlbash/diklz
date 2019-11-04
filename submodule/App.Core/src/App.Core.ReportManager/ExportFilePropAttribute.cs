
namespace App.Core.ReportManager
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ExportFilePropAttribute : System.Attribute
    {
        string header;
        int order;

        public ExportFilePropAttribute(string header, int order = 0)
        {
            this.header = header;
            this.order = order;
        }

        public string GetHeader()
        {
            return header;
        }

        public int GetOrder()
        {
            return order;
        }
    }
}
