using PdfEdit.Pdf;

namespace PruebaPDF.Repositorio.IRepositorio
{
    public interface ILicenciatario
    {
        void AplicarLicencia(ref PdfDocument documento);
    }
}
