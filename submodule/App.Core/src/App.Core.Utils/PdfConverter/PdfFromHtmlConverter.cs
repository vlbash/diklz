using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace App.Core.Utils.PdfConverter
{
    public class PdfFromHtmlConverter
    {
        private readonly IConverter _converter;

        public PdfFromHtmlConverter(IConverter converter)
        {
            _converter = converter;
        }

        public async Task<byte[]> CreatePDF(string titleName, string htmlText)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 15, Bottom = 15, Left = 25, Right = 15 },
                DocumentTitle = titleName
            };
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlText,
                WebSettings = { DefaultEncoding = "utf-8" },
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            return await Task.FromResult(_converter.Convert(pdf));
        }
    }
}
