namespace PruebaPDF.Repositorio.IRepositorio
{
    public interface IImagenSinPosicion
    {
        IImagenConPosicion AgregarPosicion(int pagina, int x, int y);
    }
}
