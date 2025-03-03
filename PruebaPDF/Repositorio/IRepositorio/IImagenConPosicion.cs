using PruebaPDF.Modelos;

namespace PruebaPDF.Repositorio.IRepositorio
{
    public interface IImagenConPosicion
    {
        IImagenConPosicion AgregarPosicion(int pagina, int x, int y);

        Imagen Construir();
    }
}
