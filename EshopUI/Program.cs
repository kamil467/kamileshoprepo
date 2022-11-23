using EshopUI;
using EshopUI.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<CatalogService>();
builder.Services.AddHttpClient(); // added explicitly
builder.Services.AddOptions<CatalogAPISettings>()
    .Bind(builder.Configuration.GetSection(CatalogAPISettings.CatalogAPISection));

// register cors service.
builder.Services.AddCors(o =>
{
    o.AddPolicy("eshop-ui-policy", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// bind and register to service as singleton service.


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseCors("eshop-ui-policy");  // enable in middleware - should come after Routing
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
