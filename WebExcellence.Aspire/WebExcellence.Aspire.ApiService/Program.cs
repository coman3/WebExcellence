using WebExcellence.Api.BooksClient;
using WebExcellence.Application.Configuration;
using WebExcellence.Application.Services.Interfaces;
using WebExcellence.Application.Services;
using WebExcellence.Aspire.ApiService.Features.Books;
using WebExcellence.Aspire.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = WebExcellence.JsonContext.Default;
});

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AgeCategoryOptions>((options) => {
    builder.Configuration.GetSection("AgeCategories").Bind(options);
    options.ValidateCategories();
});

var url = builder.Configuration["ApiBaseUrl"];
if (string.IsNullOrEmpty(url))
{
    throw new InvalidOperationException("ApiBaseUrl is required");
}

builder.Services.AddReliableBooksHttpClient(url);
builder.Services.AddTransient<IBookService, BookService>();

var app = builder.Build();
app.RegisterBooksEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.Run();