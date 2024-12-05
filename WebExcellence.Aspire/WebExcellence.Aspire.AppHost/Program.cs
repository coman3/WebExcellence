var builder = DistributedApplication.CreateBuilder(args);

var baseUrl = builder.AddParameter("ApiBaseUrl");

var apiService = builder.AddProject<Projects.WebExcellence_Aspire_ApiService>("apiservice")
    .WithEnvironment("ApiBaseUrl", baseUrl)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.WebExcellence_Aspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
