using Microsoft.AspNetCore.Mvc;
using Raftlabs.Application.Extensions;
using Raftlabs.Application.Services;
using Raftlabs.Infra.Client.Extensions;
using Raftlabs.Infra.Client.Models;
using Raftlabs.Library.Caching;
using Raftlabs.Library.Caching.Models;
using Raftlabs.Library.Extensions;
using Raftlabs.Library.Middlewares;
using Raftlabs.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

builder.Services.AddApplication();
builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection("Cache"));
builder.Services.AddCaching();
builder.Services.Configure<ApiClientOptions>(builder.Configuration.GetSection("ApiClient"));
builder.Services.AddApiClient();

builder.Services.AddHttpContextAccessor();
builder.Services.AddLibrary();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}
app.UseMiddleware<UnhandledExceptionsMiddleware>();

app.UseHttpsRedirection();
app.UseMiddleware<HeadersValidationMiddleware>();

app.MapUserEndpoints();


app.Run();
