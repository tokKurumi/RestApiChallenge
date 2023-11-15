namespace MultithredRest.Helpers
{
    using System.Collections.Specialized;

    public static class NameValueCollectionExtensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection @this)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var key in @this.AllKeys)
            {
                if (key is not null)
                {
                    dictionary.Add(key, @this.Get(key) ?? string.Empty);
                }
            }

            return dictionary;
        }
    }
}