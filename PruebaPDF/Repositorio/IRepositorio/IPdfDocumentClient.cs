namespace PruebaPDF.Repositorio.IRepositorio
{
    public interface IPdfDocumentClient
    {
        Task<byte[]> BuilderAsync(string htmlContent);
    }
}
