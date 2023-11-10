namespace MultithredRest.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json;

    public abstract class EndpointBase
    {
        public EndpointBase(HttpMethod method)
        {
            Method = method;
        }

        public HttpMethod Method { get; init; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public string HttpResponseContentType { get; set; } = "application/json";

        public static Dictionary<string, string> DeserializeQuery(NameValueCollection queryString)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (string key in queryString.AllKeys)
            {
                dictionary.Add(key, queryString.Get(key));
            }

            return dictionary;
        }

        public static T? DeserializeBody<T>(HttpListenerRequest request)
        {
            Span<byte> requestBytes = stackalloc byte[(int)request.ContentLength64]; // TODO: fix type cast
            request.InputStream.Read(requestBytes);
            var jsonRequest = Encoding.UTF8.GetString(requestBytes);

            return JsonConvert.DeserializeObject<T>(jsonRequest);
        }

        public abstract Task<ReadOnlyMemory<byte>> GenerateResponse(HttpListenerRequest request);
    }
}