using MultithredRest.Core.Attributes;
using MultithredRest.Core.Endpoint;
using MultithredRest.Core.HttpServer;
using System.Xml.Serialization;
using Example.Models.Cities;
using System.Xml;
using Example.Data;
using MultithredRest.Core.Result;

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

    public override async Task<IActionResult> GenerateResponseAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        string filePath = "Data/data.xml";

        using var reader = XmlReader.Create(filePath);
        var serializer = new XmlSerializer(typeof(CityAddresses));

        if (serializer.Deserialize(reader) is CityAddresses cityAddresses && cityAddresses.Cities is not null)
        {
            await _dbContext.Cities.AddRangeAsync(cityAddresses.Cities);
            var countOfWritten = await _dbContext.SaveChangesAsync(cancellationToken);

            return await OkAsync(new { Message = $"Succefully writen {countOfWritten} entries to database." });
        }

        return await BadRequestAsync(new { Message = "Something went wrong" });
    }
}