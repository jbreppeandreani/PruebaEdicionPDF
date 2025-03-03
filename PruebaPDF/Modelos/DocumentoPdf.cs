using System.Reflection.PortableExecutable;
using PdfPrintingNet;
using PdfEdit;
using PruebaPDF.Repositorio;
using PruebaPDF.Repositorio.IRepositorio;
using PdfEdit.Pdf;
using PdfEdit.Pdf.IO;

namespace PruebaPDF.Modelos
{

    public class DocumentoPdf
    {
        private PdfDocument _documento;

        private IIlustrador _ilustrador;

        private ILicenciatario _licenciatario;

        public string Ruta { get; }

        /// <summary>
        /// Si la ruta es de un pdf existente lo carga en memoria, sino crea uno nuevo en esa ruta.
        /// </summary>
        /// <param name="ruta"></param>
        public DocumentoPdf(string ruta, IIlustrador ilustrador = null, ILicenciatario licenciatario = null)
        {
            this._ilustrador = ilustrador ?? new IlustradorDePdf();
            //this._licenciatario = licenciatario ?? new Licenciatario();
            this.Ruta = ruta;
            this._documento = this.Inicializar(this.Ruta);
        }

        /// <summary>
        /// Inserta la imagen en el documento pdf, en las posiciones correspondientes.
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="imagen"></param>
        public void Insertar(Imagen imagen)
        {
            this._documento = this._ilustrador.Dibujar(this._documento, imagen);
        }

        /// <summary>
        /// Inserta las imagenes en el documento pdf, en las posiciones correspondientes.
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="imagenes"></param>
        public void Insertar(List<Imagen> imagenes)
        {
            foreach (var imagen in imagenes)
                this._documento = this._ilustrador.Dibujar(this._documento, imagen);
        }

        public string Guardar(string output = null)
        {
            //this._licenciatario.AplicarLicencia(ref this._documento);
            var rutaDeSalida = string.IsNullOrEmpty(output) ? this.Ruta : output;
            this._documento.Save(rutaDeSalida);

            return rutaDeSalida;
        }

        private PdfDocument Inicializar(string ruta)
        {
            PdfDocument documentoInicializado = null;

            if (!Directory.Exists(Path.GetDirectoryName(ruta)))
                throw new ArgumentException("Ruta invalida, no existe el directorio");

            if (File.Exists(ruta))
                documentoInicializado = PdfReader.Open(ruta);
            else
            {
                documentoInicializado = new PdfDocument();
                documentoInicializado.AddPage();
            }

            return documentoInicializado;
        }
    }
}
