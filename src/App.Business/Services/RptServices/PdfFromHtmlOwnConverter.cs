using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace App.Business.Services.RptServices
{
    public class PdfFromHtmlOwnConverter
    {
        private readonly IConverter _converter;
        private readonly string path;

        public PdfFromHtmlOwnConverter(IConverter converter)
        {
            _converter = converter;
            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public async Task<byte[]> CreatePDF(string titleName, string htmlText)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 15, Bottom = 25, Left = 25, Right = 15 },
                DocumentTitle = titleName
            };
            var objectSettings = new ObjectSettings
            {
                HtmlContent = htmlText,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = new FooterSettings { HtmUrl = Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_Footer.html"), Line = true, Spacing = 5 }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };
            return await Task.FromResult(_converter.Convert(pdf));
        }
    }
}
