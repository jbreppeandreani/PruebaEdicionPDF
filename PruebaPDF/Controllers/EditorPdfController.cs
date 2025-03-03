using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfEdit.Pdf;
using PruebaPDF.Modelos;
using PruebaPDF.Repositorio.IRepositorio;

namespace PruebaPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorPdfController : ControllerBase
    {
        private readonly IIlustrador _ilustradorRepo;
        private readonly IImagenSinPosicion _imagenSinPosicionRepo;
        private readonly IImagenConPosicion _imagenConPosicionRepo;
        private readonly IImagenSinInicializar _imagenSinIncializarRepo;
        private readonly ILicenciatario _licenciatarioRepo;
        public EditorPdfController(IIlustrador ilustradorRepo, IImagenSinPosicion imagenSinPosicionRepo, IImagenConPosicion imagenConPosicionRepo, IImagenSinInicializar imagenSinIncializarRepo, ILicenciatario licenciatarioRepo)
        {
            _ilustradorRepo = ilustradorRepo;
            _imagenSinPosicionRepo = imagenSinPosicionRepo;
            _imagenConPosicionRepo = imagenConPosicionRepo;
            _imagenSinIncializarRepo = imagenSinIncializarRepo;
            _licenciatarioRepo = licenciatarioRepo;
        }

        [HttpPost("AgregarImagenAPdfViaRuta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AgregarImagenAPdfViaRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return BadRequest("La ruta del archivo no puede estar vacia.");

            if (!System.IO.File.Exists(ruta))
                return NotFound("No se ha encontrado el archivo en la ruta especificada.");

            if (Path.GetExtension(ruta).ToLower() != ".pdf")
                return BadRequest("El archivo especificado no es un PDF valido.");

            try
            {
                var documentoPdf = new DocumentoPdf(ruta);

                string rutaImagenAInsertar = "C:/Users/breppe_j/Downloads/imgPrueba.png";
                Imagen imagenAInsertar = new(100, 100, rutaImagenAInsertar);
                Posicion posicion = new(1, 250, 450);

                imagenAInsertar.AgregarPosicion(posicion);
                documentoPdf.Insertar(imagenAInsertar);
                documentoPdf.Guardar();

                return Ok("Se ha modificado el archivo correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al modificar el PDF: {ex.Message}");
            }
        }


        [HttpPost("AgregarImagenAPdfViaFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AgregarImagenAPdfViaFile(IFormFile archivoPdf)
        {
            if (archivoPdf == null || archivoPdf.Length == 0)
                return BadRequest("Debe proporcionar un archivo PDF valido.");

            if (Path.GetExtension(archivoPdf.FileName).ToLower() != ".pdf")
                return BadRequest("El archivo debe ser un PDF.");

            string rutaTemporal = Path.GetTempFileName();
            using (var stream = new FileStream(rutaTemporal, FileMode.Create))
            {
                await archivoPdf.CopyToAsync(stream);
            }

            try
            {
                var documentoPdf = new DocumentoPdf(rutaTemporal);

                string rutaImagenAInsertar = "C:/Users/breppe_j/Downloads/imgPrueba.png";
                Imagen imagenAInsertar = new(100, 100, rutaImagenAInsertar);
                Posicion posicion = new(1, 250, 450);

                imagenAInsertar.AgregarPosicion(posicion);

                documentoPdf.Insertar(imagenAInsertar);
                documentoPdf.Guardar();

                byte[] archivoModificado = System.IO.File.ReadAllBytes(rutaTemporal);
                return File(archivoModificado, "application/pdf", "ArchivoModificado.pdf");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el PDF: {ex.Message}");
            }
            finally
            {
                if (System.IO.File.Exists(rutaTemporal))
                    System.IO.File.Delete(rutaTemporal);
            }
        }
        [HttpPost("AgregarImagenAPdfConImagen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AgregarImagenAPdfConImagen(IFormFile archivoPdf, IFormFile imagen)
        {
            if (archivoPdf == null || archivoPdf.Length == 0)
                return BadRequest("Debe proporcionar un archivo PDF valido.");

            if (imagen == null || imagen.Length == 0)
                return BadRequest("Debe proporcionar una imagen valida.");

            if (Path.GetExtension(archivoPdf.FileName).ToLower() != ".pdf")
                return BadRequest("El archivo debe ser un PDF.");

            string rutaPdfTemporal = Path.GetTempFileName();
            string rutaImagenTemporal = Path.GetTempFileName();

            using (var pdfStream = new FileStream(rutaPdfTemporal, FileMode.Create))
            {
                await archivoPdf.CopyToAsync(pdfStream);
            }

            using (var imagenStream = new FileStream(rutaImagenTemporal, FileMode.Create))
            {
                await imagen.CopyToAsync(imagenStream);
            }

            try
            {
                var documentoPdf = new DocumentoPdf(rutaPdfTemporal);

                Imagen imagenAInsertar = new Imagen(100, 100, rutaImagenTemporal);
                Posicion posicion = new Posicion(1, 250, 450);

                imagenAInsertar.AgregarPosicion(posicion);
                documentoPdf.Insertar(imagenAInsertar);
                documentoPdf.Guardar();

                byte[] archivoModificado = System.IO.File.ReadAllBytes(rutaPdfTemporal);
                return File(archivoModificado, "application/pdf", "ArchivoModificado.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el PDF: {ex.Message}");
            }
            finally
            {
                if (System.IO.File.Exists(rutaPdfTemporal))
                    System.IO.File.Delete(rutaPdfTemporal);

                /*if (System.IO.File.Exists(rutaImagenTemporal))
                    System.IO.File.Delete(rutaImagenTemporal);*/
            }
        }

    }
}
