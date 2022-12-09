using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Polly.Timeout;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Ocelot;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Provider.Polly;
using EShop.CircuitBreaker.Extension;
using Ocelot.Values;
using Polly.Registry;

var builder = WebApplication.CreateBuilder(args);


//Demo-1 named httpclient.
//static IAsyncPolicy<HttpResponseMessage> timeputPolicy () => Policy.TimeoutAsync<HttpResponseMessage>(3, TimeoutStrategy.Optimistic);
//builder.Services.AddHttpClient("catalog-client", client =>
//{
//    var uri = new Uri("http://localhost:5101/api/v2/catalog/");
//    client.BaseAddress = uri;

//}).AddPolicyHandler(timeputPolicy());


// Add services to the container.
builder.Configuration.AddJsonFile(Path.Combine("Configuration", "configuration.json"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PollyWithCircuitBreaker>(); // It should be a singleton instance
builder.Services.AddSingleton<PollyCircuitBreakerAndTimeOut>(); 
// Ocelot with Polly
builder.Services
    .AddOcelot(builder.Configuration)
      //.AddDelegatingHandler<PollyWithTimeOutDelegatingHandler>(true) // Polly based Timeout Policy
      //.AddDelegatingHandler<PollyWithCircuitBreaker>(true)
      .AddDelegatingHandler<PollyCircuitBreakerAndTimeOut>(true);
 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthorization();



app.MapControllers();

//app.MapGet("/timeouttest",  async (IHttpClientFactory clientFactory) =>
//{
//    var httpClient = clientFactory.CreateClient("catalog-client");
//    var response = await httpClient.GetAsync("GetCataLogItemsByPagination");
//    return response;
//});
await app.UseOcelot(); // enable Ocelot in middleware
app.Run();
