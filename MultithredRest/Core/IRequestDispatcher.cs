namespace MultithredRest.Core
{
    using System.Net;

    public interface IRequestDispatcher
    {
        Task Dispatch(HttpListenerContext context);
    }
}