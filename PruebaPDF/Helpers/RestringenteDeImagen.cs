using System.Drawing;

namespace PruebaPDF.Helpers
{
    /// <summary>
    /// Clase util para restringir el tamaño de las imagenes en los pdfs creados
    /// </summary>
    public static class RestringenteDeImagen
    {
        public static int ObtenerAnchoRestricto(string rutaDeImagen)
        {
            var imagenEnMemoria = new Bitmap(rutaDeImagen);
            return imagenEnMemoria.Width < 595 ? imagenEnMemoria.Width : 595;
        }

        public static int ObtenerLargoRestricto(string rutaDeImagen)
        {
            var imagenEnMemoria = new Bitmap(rutaDeImagen);
            return imagenEnMemoria.Height < 421 ? imagenEnMemoria.Height : 421;
        }
    }
}
