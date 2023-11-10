namespace MultithreadRest.Helpers
{
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

    public static class MemorySerializeExtensions
    {
        public static async Task<ReadOnlyMemory<byte>> SerializeJsonAsync<T>(this T obj)
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, obj);
            return new ReadOnlyMemory<byte>(stream.GetBuffer(), 0, (int)stream.Length);
        }
    }
}