
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject("webapi", "../CleanArchitecture.WebApi/CleanArchitecture.WebApi.csproj");

await builder.Build().RunAsync();
