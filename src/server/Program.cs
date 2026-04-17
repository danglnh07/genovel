using Carter;
using FluentValidation;
using Genovel.Profiles;
using Genovel.Shared.Behaviors;
using Genovel.Shared.Exceptions;
using Marten;
using Scalar.AspNetCore;
using Serilog;

try
{
    Log.Information("Starting Genovel...");

    // === Create builder ===
    var builder = WebApplication.CreateBuilder(args);

    // Check if connection string is set
    var conn = builder.Configuration.GetConnectionString("DefaultConn") ?? throw new Exception("Connection string is not set");

    // Add OpenAPI support
    builder.Services.AddOpenApi();
    // Register custom global exception handler
    builder.Services.AddExceptionHandler<ExceptionHandler>();
    // Register Serilog
    builder.Services.AddSerilog((service, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(service));
    // Register validators
    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    // Register MediatR
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });
    // Register Marten
    builder.Services.AddMarten(config =>
    {
        config.Connection(conn);
    }).UseLightweightSessions();
    // Register Carter
    builder.Services.AddCarter();
    // Register AutoMapper
    builder.Services.AddAutoMapper(config =>
    {
        config.AddProfile<StoryProfile>();
    });

    // === Build application ===
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(); // Use Scalar
    }

    app.MapCarter();

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


