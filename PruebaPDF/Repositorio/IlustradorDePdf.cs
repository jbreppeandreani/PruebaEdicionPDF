using PruebaPDF.Modelos;
using PruebaPDF.Repositorio.IRepositorio;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using PdfPrintingNet;
using PdfEdit.Pdf;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using PdfEdit.Drawing;

namespace PruebaPDF.Repositorio
{

    public class IlustradorDePdf : IIlustrador
    {
        public PdfDocument Dibujar(PdfDocument pdf, Imagen imagen)
        {
            var paginasDelDocumento = pdf.Pages;

            using (var imagenEnMemoria = new Bitmap(Image.FromFile(imagen.Ruta, true)))
            {
                //var imagenAEscala = RedimensionarImagen(imagenEnMemoria, imagen.Ancho, imagen.Alto);
                var imagenAEscala = RedimensionarImagen(imagenEnMemoria, imagenEnMemoria.Width, imagenEnMemoria.Height);

                foreach (var posicion in imagen.Posiciones)
                    using (var gfx = XGraphics.FromPdfPage(paginasDelDocumento[posicion.Pagina - 1]))
                        gfx.DrawImage(imagenAEscala, posicion.X, posicion.Y, imagen.Ancho, imagen.Alto);
            }

            return pdf;
        }

        private static Bitmap RedimensionarImagen(Image image, int ancho, int alto)
        {
            var rectanguloDestino = new Rectangle(0, 0, ancho, alto);
            var imagenDestino = new Bitmap(ancho, alto);

            imagenDestino.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(imagenDestino))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, rectanguloDestino, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return imagenDestino;
        }
    }
}
