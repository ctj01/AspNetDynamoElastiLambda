using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using AspNetElsDb.models;
using AspNetElsDb.options;
using Microsoft.Extensions.Options;

namespace AspNetElsDb.services;

public class DynamoDbService
{
    private readonly AmazonDynamoDBClient _dynamoDbClient;
    
    public DynamoDbService(IOptions<AwsOptions> options)
    {
      
        AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
        {
            ServiceURL = options.Value.DynamoEndPoint
        };
        var cred = new BasicAWSCredentials(options.Value.AccessKey, options.Value.SecretKey);
        _dynamoDbClient = new AmazonDynamoDBClient(cred,  clientConfig);
    }

    public async Task<(bool success, string message)> CreateNewMovie(Movie movie)
    {
        var item = new Document
        {
            ["id"] = movie.Id,
            ["title"] = movie.Title,
            ["genres"] = movie.Genres
        };

        
        try
        {
            var table = Table.LoadTable(_dynamoDbClient, "movies");
            await table.PutItemAsync(item);
            return (true, "movie saved");
        }
        catch (Exception e)
        {
            return (false, "error saving movie");
        }
    }
}