using PruebaPDF.Modelos;
using PruebaPDF.Repositorio.IRepositorio;

namespace PruebaPDF.Repositorio
{
    public class ImagenBuilder : IImagenConPosicion, IImagenSinPosicion, IImagenSinInicializar
    {
        private Imagen ImagenAConstruir { get; set; }

        public IImagenSinPosicion Inicializar(int ancho, int alto, string ruta)
        {
            ImagenAConstruir = new Imagen(ancho, alto, ruta);
            return this;
        }

        public IImagenConPosicion AgregarPosicion(int pagina, int x, int y)
        {
            ImagenAConstruir.AgregarPosicion(new Posicion(pagina, x, y));
            return this;
        }

        public Imagen Construir()
        {
            return this.ImagenAConstruir;
        }
    }

    public static class FactoryDeImagenBuilder
    {
        public static IImagenSinInicializar Builder => new ImagenBuilder();
    }
}
