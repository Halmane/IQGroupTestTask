using IQGroupTestTask;
using IQGroupTestTask.Components;
using MongoDB.Driver;

var cancellationTokenSource = new CancellationTokenSource();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorComponents().AddInteractiveServerComponents();
    var mongoClientLink = builder.Configuration.GetSection("MongoClientLink").Value;

    builder
        .Services.AddLogging(builder => builder.AddConsole())
        .AddSingleton(cancellationTokenSource)
        .AddSingleton<MongoDatabaseUserService>();

    ArgumentNullException.ThrowIfNullOrEmpty(mongoClientLink);

    builder.Services.AddSingleton(new MongoClient(mongoClientLink));

    var app = builder.Build();

    var mongoClient = app.Services.GetService<MongoClient>();
    var mongoDBUserService = app.Services.GetService<MongoDatabaseUserService>();

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
}
catch (Exception ex)
{
    cancellationTokenSource.Cancel();
    Console.WriteLine(ex.Message);
    Console.WriteLine("Press any key to exit app...");
    Console.ReadKey();
}
