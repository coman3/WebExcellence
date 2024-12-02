using System.Text.Json.Serialization;
using WebExcellence.Api.BooksClient;
using WebExcellence.Application.Configuration;
using WebExcellence.Application.Models;
using WebExcellence.Application.Services;
using WebExcellence.Application.Services.Interfaces;
using WebExcellence.Features.Books;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = WebExcellence.JsonContext.Default;
});

builder.Services.Configure<AgeCategoryOptions>((options) => {
    builder.Configuration.GetSection("AgeCategories").Bind(options);
    options.ValidateCategories();
});

var url = builder.Configuration["API_BASE_URL"];
if (string.IsNullOrEmpty(url))
{
    throw new InvalidOperationException("API_BASE_URL is required");
}

builder.Services.AddReliableBooksHttpClient(url);
builder.Services.AddTransient<IBookService, BookService>();

var app = builder.Build();

app.RegisterBooksEndpoints();

app.Run();