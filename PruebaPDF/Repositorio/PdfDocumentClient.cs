using DinkToPdf.Contracts;
using DinkToPdf;
using PruebaPDF.Repositorio.IRepositorio;

namespace PruebaPDF.Repositorio
{
    public class PdfDocumentClient : IPdfDocumentClient
    {
        private readonly IConverter _converter;

        public PdfDocumentClient(IConverter converter)
        {
            _converter = converter;
        }

        public Task<byte[]> BuilderAsync(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10, Bottom = 10 }
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                UseExternalLinks = true,
                WebSettings = {
                    DefaultEncoding = "utf-8",
                    Background = true,
                    LoadImages = true,
                    EnableIntelligentShrinking= false,
                    PrintMediaType = true
                }
            };

            var htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            var pdfData = _converter.Convert(htmlToPdfDocument);

            return Task.FromResult(pdfData);
        }
    }
}
