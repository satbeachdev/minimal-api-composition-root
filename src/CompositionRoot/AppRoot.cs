using API.Endpoints.Customer;
using Application.Services;
using CompositionRoot;
using EndpointHandler.Core.Extensions;
using FluentValidation;
using Interfaces;
using Logging;
using Microsoft.OpenApi.Models;
using Middleware;
using Models.Domain;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Repositories;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepository<Customer>, CustomerRepository>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ILoggingService, LoggingService>();
builder.Services.AddSingleton(typeof(ActivitySource), new ActivitySource(TelemetryConstants.SourceName));

// Add Validators from the Models assembly
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Customer)));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = TelemetryConstants.SourceName, Version = TelemetryConstants.Version });
});

var endpointHandlersAssembly = Assembly.GetAssembly(typeof(GetCustomer));

builder.AddEndpointHandlersFromAssembly(endpointHandlersAssembly);

// Configure important OpenTelemetry settings, the console exporter, and instrumentation library
builder.Services.AddOpenTelemetryTracing(options =>
{
    options.AddSource(new string[] { TelemetryConstants.SourceName, "CustomerService" })
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(TelemetryConstants.SourceName).AddTelemetrySdk())
        .AddAspNetCoreInstrumentation(options =>
        {
            options.Filter = (req) => !req.Request.Path.ToUriComponent().Contains("index.html", StringComparison.OrdinalIgnoreCase)
            && !req.Request.Path.ToUriComponent().Contains("swagger", StringComparison.OrdinalIgnoreCase);
        })
        .AddJaegerExporter();
});

builder.Services.Configure<AspNetCoreInstrumentationOptions>(options =>
{
    options.RecordException = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Log the time each call to all APIs takes
app.UseMiddleware<ApiPerformanceMiddleware>();

// Map all the endpoints to the classes that implement IEndpointHandler
app.MapEndpointsFromAssembly(endpointHandlersAssembly);

app.UseExceptionHandler(ExceptionHandler.Handle);

app.Run();
