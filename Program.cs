using System;
using Microsoft.AspNetCore.Builder;
using Serilog;
using TESTPROJESI.Core.Modules;

var builder = WebApplication.CreateBuilder(args);

var modules = ModuleCatalog.CreateDefault();
modules.ConfigureServices(builder);

var app = builder.Build();

modules.ConfigureApplication(app);

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Uygulama başlatılamadı!");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
