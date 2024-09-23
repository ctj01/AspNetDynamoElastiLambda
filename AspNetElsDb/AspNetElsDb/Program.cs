using AspNetElsDb.models;
using AspNetElsDb.options;
using AspNetElsDb.services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AwsOptions>(configuration.GetSection("Aws"));

builder.Services.Configure<ElasticSearchOptions>(configuration.GetSection("Elastic")); 
builder.Services.AddSingleton<DynamoDbService>();
builder.Services.AddSingleton<ElasticSearchService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/movies", async (Movie movie, DynamoDbService dynamoDbService) =>
{
    await dynamoDbService.CreateNewMovie(movie);
    return Results.Created($"/api/movies/{movie.Id}", movie);
});

app.MapGet("/api/movies/search", async (string query, ElasticSearchService elasticSearchService) =>
{
    var movies = await elasticSearchService.GetMoviesAsync(query);
    return Results.Ok(movies);
});


await app.RunAsync();