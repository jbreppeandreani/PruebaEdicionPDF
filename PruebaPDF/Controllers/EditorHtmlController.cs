using System.Net;
using System.Text;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaPDF.Modelos;
using PruebaPDF.Repositorio;
using PruebaPDF.Repositorio.IRepositorio;

namespace PruebaPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorHtmlController : ControllerBase
    {
        private readonly IIlustrador _ilustradorRepo;
        private readonly IImagenSinPosicion _imagenSinPosicionRepo;
        private readonly IImagenConPosicion _imagenConPosicionRepo;
        private readonly IImagenSinInicializar _imagenSinIncializarRepo;
        private readonly ILicenciatario _licenciatarioRepo;
        private readonly IPdfDocumentClient _pdfDocumentClientRepo;
        private readonly IConverter _converter;
        public EditorHtmlController(IIlustrador ilustradorRepo, IPdfDocumentClient pdfDocumentClientRepo, IConverter converter, IImagenSinPosicion imagenSinPosicionRepo, IImagenConPosicion imagenConPosicionRepo, IImagenSinInicializar imagenSinIncializarRepo, ILicenciatario licenciatarioRepo)
        {
            _ilustradorRepo = ilustradorRepo;
            _imagenSinPosicionRepo = imagenSinPosicionRepo;
            _imagenConPosicionRepo = imagenConPosicionRepo;
            _imagenSinIncializarRepo = imagenSinIncializarRepo;
            _licenciatarioRepo = licenciatarioRepo;
            _converter = converter;
            _pdfDocumentClientRepo = pdfDocumentClientRepo;
        }

        [HttpPost("ModificarHtml")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarHtml(IFormFile archivoHtml)
        {
            if (archivoHtml == null || archivoHtml.Length == 0)
                return BadRequest("Debe proporcionar un archivo HTML válido.");

            if (Path.GetExtension(archivoHtml.FileName).ToLower() != ".html")
                return BadRequest("El archivo debe ser un HTML.");

            using var reader = new StreamReader(archivoHtml.OpenReadStream(), Encoding.UTF8);
            string htmlContent = await reader.ReadToEndAsync();

            string imagenHtml = "<img src='C:/Users/breppe_j/Downloads/imgPrueba.png' width='100' height='100'>";
            htmlContent = htmlContent.Replace("<body>", $"<body>{imagenHtml}");

            string rutaTemporal = Path.GetTempFileName() + ".html";
            await System.IO.File.WriteAllTextAsync(rutaTemporal, htmlContent, Encoding.UTF8);

            byte[] archivoModificado = await System.IO.File.ReadAllBytesAsync(rutaTemporal);
            System.IO.File.Delete(rutaTemporal);

            return File(archivoModificado, "text/html", "ArchivoModificado.html");
        }

        [HttpPost("ModificarHtmlConImagen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarHtmlConImagen(IFormFile archivoHtml, IFormFile imagen)
        {
            if (archivoHtml == null || archivoHtml.Length == 0)
                return BadRequest("Debe proporcionar un archivo HTML válido.");

            if (imagen == null || imagen.Length == 0)
                return BadRequest("Debe proporcionar una imagen válida.");

            if (Path.GetExtension(archivoHtml.FileName).ToLower() != ".html")
                return BadRequest("El archivo debe ser un HTML.");

            string rutaImagenTemporal = Path.GetTempFileName();
            using (var imagenStream = new FileStream(rutaImagenTemporal, FileMode.Create))
            {
                await imagen.CopyToAsync(imagenStream);
            }

            using var reader = new StreamReader(archivoHtml.OpenReadStream(), Encoding.UTF8);
            string htmlContent = await reader.ReadToEndAsync();

            byte[] imagenBytes = await System.IO.File.ReadAllBytesAsync(rutaImagenTemporal);
            string base64Imagen = Convert.ToBase64String(imagenBytes);
            string imagenHtml = $"<img src='data:image/png;base64,{base64Imagen}' width='100' height='100'>";

            htmlContent = htmlContent.Replace("<body>", $"<body>{imagenHtml}");

            string rutaTemporal = Path.GetTempFileName() + ".html";
            await System.IO.File.WriteAllTextAsync(rutaTemporal, htmlContent, Encoding.UTF8);

            byte[] archivoModificado = await System.IO.File.ReadAllBytesAsync(rutaTemporal);
            System.IO.File.Delete(rutaTemporal);

            return File(archivoModificado, "text/html", "ArchivoModificado.html");
        }

        [HttpPost("ModificarHtmlConVariasImagenes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<GetConstanciaResponse>>> ModificarHtmlConVariasImagenes(IFormFile archivoHtml, List<IFormFile> imagenes)
        {
            if (archivoHtml == null || archivoHtml.Length == 0)
                return BadRequest("Debe proporcionar un archivo HTML válido.");

            if (imagenes == null || imagenes.Count == 0)
                return BadRequest("Debe proporcionar al menos una imagen válida.");

            if (Path.GetExtension(archivoHtml.FileName).ToLower() != ".html")
                return BadRequest("El archivo debe ser un HTML.");

            using var reader = new StreamReader(archivoHtml.OpenReadStream(), Encoding.UTF8);
            string htmlContent = await reader.ReadToEndAsync();

            StringBuilder imagenesHtml = new StringBuilder();

            foreach (var imagen in imagenes)
            {
                string rutaImagenTemporal = Path.GetTempFileName();
                using (var imagenStream = new FileStream(rutaImagenTemporal, FileMode.Create))
                {
                    await imagen.CopyToAsync(imagenStream);
                }

                byte[] imagenBytes = await System.IO.File.ReadAllBytesAsync(rutaImagenTemporal);
                string base64Imagen = Convert.ToBase64String(imagenBytes);
                imagenesHtml.Append($"<img src='data:image/png;base64,{base64Imagen}' width='100' height='100'>");
            }

            htmlContent = htmlContent.Replace("<body>", $"<body>{imagenesHtml}");

            string rutaTemporal = Path.GetTempFileName() + ".html";
            await System.IO.File.WriteAllTextAsync(rutaTemporal, htmlContent, Encoding.UTF8);

            byte[] archivoModificado = await System.IO.File.ReadAllBytesAsync(rutaTemporal);
            System.IO.File.Delete(rutaTemporal);

            return File(archivoModificado, "text/html", "ArchivoModificado.html");
        }

        [HttpPost("ModificarHtmlConVariasImagenesAPdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<Response<GetConstanciaResponse>> ModificarHtmlConVariasImagenesAPdf(IFormFile archivoHtml, List<IFormFile> imagenes, string NroEnvio)
        {
            int error = 0;
            if (archivoHtml == null || archivoHtml.Length == 0)
                error = 1;
            if (imagenes == null || imagenes.Count == 0)
                error = 1;
            if (Path.GetExtension(archivoHtml.FileName).ToLower() != ".html")
                error = 1;

            if (error == 1)
            {
                return new Response<GetConstanciaResponse>()
                {
                    Content = null,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            using var reader = new StreamReader(archivoHtml.OpenReadStream(), Encoding.UTF8);
            string htmlContent = await reader.ReadToEndAsync();

            var getConstanciaResponse = new GetConstanciaResponse
            {
                FileName = $"{NroEnvio}.pdf"
            };

            StringBuilder imagenesHtml = new StringBuilder();
            foreach (var imagen in imagenes)
            {
                string rutaImagenTemporal = Path.GetTempFileName();
                using (var imagenStream = new FileStream(rutaImagenTemporal, FileMode.Create))
                {
                    await imagen.CopyToAsync(imagenStream);
                }
                byte[] imagenBytes = await System.IO.File.ReadAllBytesAsync(rutaImagenTemporal);
                string base64Imagen = Convert.ToBase64String(imagenBytes);
                imagenesHtml.Append($"<img src='data:image/png;base64,{base64Imagen}' width='100' height='100'>");
                System.IO.File.Delete(rutaImagenTemporal);
            }
            htmlContent = htmlContent.Replace("<body>", $"<body>{imagenesHtml}");

            var bytePdf = await _pdfDocumentClientRepo.BuilderAsync(htmlContent);
            Stream streamPdf = new MemoryStream(bytePdf)
            {
                Position = 0
            };
            getConstanciaResponse.Content = streamPdf!;
            getConstanciaResponse.MimeType = "application/pdf";

            return new Response<GetConstanciaResponse>()
            {
                Content = getConstanciaResponse,
                StatusCode = HttpStatusCode.OK
            };

        }

    }
}
