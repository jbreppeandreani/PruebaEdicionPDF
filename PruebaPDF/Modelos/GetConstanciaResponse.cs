namespace PruebaPDF.Modelos
{
    public class GetConstanciaResponse
    {
        public Stream Content { get; set; } = default!;
        public string MimeType { get; set; } = default!;
        public string FileName { get; set; } = default!;
    }
}
