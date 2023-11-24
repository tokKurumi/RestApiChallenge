namespace MultithreadRest.Helpers
{
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

    public static class MemorySerializeExtensions
    {
        public static async Task<ReadOnlyMemory<byte>> SerializeJsonAsync<T>(this T obj, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, obj, cancellationToken: cancellationToken);
            return new ReadOnlyMemory<byte>(stream.GetBuffer(), 0, (int)stream.Length);
        }

        public static ReadOnlyMemory<byte> ConcatenateReadOnlyMemories(this IEnumerable<ReadOnlyMemory<byte>> memories, ReadOnlyMemory<byte> open, ReadOnlyMemory<byte> separator, ReadOnlyMemory<byte> close)
        {
            int separatorLength = separator.Length;
            int openLength = open.Length;
            int closeLength = close.Length;

            // Calculate total length including separators and open/close sequences
            int totalLength = openLength + memories.Sum(memory => memory.Length + separatorLength) - separatorLength + closeLength;

            // Create the result array
            var resultArray = new byte[totalLength];
            int offset = 0;

            // Copy open sequence
            open.Span.CopyTo(resultArray.AsSpan(offset));
            offset += openLength;

            // Concatenate with separators
            foreach (var memory in memories)
            {
                if (offset > openLength)
                {
                    // Copy separator between memories
                    separator.Span.CopyTo(resultArray.AsSpan(offset));
                    offset += separatorLength;
                }

                // Copy memory
                memory.Span.CopyTo(resultArray.AsSpan(offset));
                offset += memory.Length;
            }

            // Copy close sequence
            close.Span.CopyTo(resultArray.AsSpan(offset));

            return resultArray.AsMemory();
        }
    }
}