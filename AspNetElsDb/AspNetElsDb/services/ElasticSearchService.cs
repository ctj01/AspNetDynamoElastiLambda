using AspNetElsDb.models;
using AspNetElsDb.options;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;

namespace AspNetElsDb.services;
public class ElasticSearchService
{
    private readonly ElasticsearchClient _client;

    public ElasticSearchService(IOptions<ElasticSearchOptions> options)
    {
        var settings = new ElasticsearchClientSettings(new Uri(options.Value.EndPoint));
        settings.Authentication(new BasicAuthentication(options.Value.User, options.Value.Password)).DefaultIndex("movies");
        _client = new ElasticsearchClient(settings);
    }

    public async Task<List<Movie>> GetMoviesAsync(string query)
    {
        var response = await _client.SearchAsync<Movie>(s => s
            .Index("movies") 
            .From(0)
            .Size(10)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(query)
                )
            )
        );

        if (response.IsValidResponse)
        {
            return response.Documents.ToList();
        }
        throw new Exception($"{response.ElasticsearchServerError.Error}");
    }
}
