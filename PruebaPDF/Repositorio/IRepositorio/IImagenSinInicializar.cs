namespace PruebaPDF.Repositorio.IRepositorio
{
    public interface IImagenSinInicializar
    {
        IImagenSinPosicion Inicializar(int ancho, int alto, string ruta);
    }
}
