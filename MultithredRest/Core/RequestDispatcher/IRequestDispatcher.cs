namespace MultithredRest.Core.RequestDispatcher.RequestDispatcher
{
    using System.Net;

    public interface IRequestDispatcher
    {
        Task Dispatch(HttpListenerContext context);
    }
}