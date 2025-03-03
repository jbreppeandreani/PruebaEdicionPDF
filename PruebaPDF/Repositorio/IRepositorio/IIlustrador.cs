using PruebaPDF.Modelos;
using PdfEdit.Pdf;
namespace PruebaPDF.Repositorio.IRepositorio
{
    public interface IIlustrador
    {
        PdfDocument Dibujar(PdfDocument pdf, Imagen imagen);
    }
}
