using PdfEdit.Pdf;
using PruebaPDF.Repositorio.IRepositorio;
using System.Reflection;

namespace PruebaPDF.Repositorio
{
    /// <summary>
    /// Aplicador de licencia de libreria PdfEdit
    /// </summary>
    public class Licenciatario : ILicenciatario
    {
        private string Licencia { get; }

        private string Serial { get; }

        public Licenciatario()
        {
            //var configuraciones = ConfigurationManager.OpenExeConfiguration((Assembly.GetExecutingAssembly()).Location).AppSettings;
            //this.Licencia = configuraciones.Settings["LicenciaPdfEdit"].Value;
            //this.Serial = configuraciones.Settings["SerialPdfEdit"].Value;
        }

        public void AplicarLicencia(ref PdfDocument documento) => documento.SetLicenseInfo(this.Licencia, this.Serial);
    }
}
