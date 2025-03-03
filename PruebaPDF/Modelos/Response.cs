using System.Net;

namespace PruebaPDF.Modelos
{
    public class Response<T>
    {
        public T? Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

}
