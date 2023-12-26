using MultithredRest.Core.Attributes;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;
using MultithredRest.Helpers;
using System.Xml.Serialization;
using Example.Models.Cities;
using System.Xml;
using Example.Data;

namespace Example.Endpoints;

[RegistrateEndpoint]
public class ParseBigFileEndpoint : EndpointBase
{
    private readonly CityDbContext _dbContext;

    public ParseBigFileEndpoint(CityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override string Route => @"/parse";

    public override HttpMethod Method => HttpMethod.Post;

    public override string HttpResponseContentType => "application/json";

    public override async Task<ReadOnlyMemory<byte>> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        string filePath = "Data/data.xml";

        using var reader = XmlReader.Create(filePath);
        var serializer = new XmlSerializer(typeof(CityAddresses));

        if (serializer.Deserialize(reader) is CityAddresses cityAddresses && cityAddresses.Cities is not null)
        {
            await _dbContext.Cities.AddRangeAsync(cityAddresses.Cities);
            var countOfWritten = await _dbContext.SaveChangesAsync(cancellationToken);

            return await new { Message = $"Succefully writen {countOfWritten} entries to database." }.SerializeJsonAsync(cancellationToken);
        }

        return await new { Message = "Something went wrong" }.SerializeJsonAsync(cancellationToken);
    }
}