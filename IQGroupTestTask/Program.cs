using IQGroupTestTask;
using IQGroupTestTask.Components;
using IQGroupTestTask.Tests;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder
    .Services.AddSingleton(new MongoClient("mongodb://localhost:27017"))
    .AddSingleton<Logger>()
    .AddSingleton<IMongoDatabaseExtensionsTest>()
    .AddSingleton<MongoDBUserService>();

var app = builder.Build();

var mongoClient = app.Services.GetService<MongoClient>();
var mongoDBUserService = app.Services.GetService<MongoDBUserService>();
var iMongoDatabaseExtensionsTest = app.Services.GetService<IMongoDatabaseExtensionsTest>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();