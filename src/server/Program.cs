using Genovel.Shared.Exceptions;
using Scalar.AspNetCore;
using Serilog;

try
{
    Log.Information("Starting Genovel...");

    // === Create builder ===
    var builder = WebApplication.CreateBuilder(args);

    // Add OpenAPI support
    builder.Services.AddOpenApi();
    // Register custom global exception handler
    builder.Services.AddExceptionHandler<ExceptionHandler>();
    // Register Serilog
    builder.Services.AddSerilog((service, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(service));

    // === Build application ===
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(); // Use Scalar
    }

    app.UseExceptionHandler(options => { }); // Relied on exception handler

    app.UseHttpsRedirection();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


