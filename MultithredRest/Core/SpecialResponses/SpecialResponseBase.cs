namespace MultithredRest.Core.SpecialResponses
{
    using System.Net;

    public abstract class SpecialResponseBase
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}