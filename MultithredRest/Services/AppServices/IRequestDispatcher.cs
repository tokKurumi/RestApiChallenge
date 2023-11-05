namespace MultithredRest.Services.AppServices
{
    using System.Net;

    public interface IRequestDispatcher
    {
        Task<HttpStatusCode> Dispatch(HttpListenerContext context);
    }
}