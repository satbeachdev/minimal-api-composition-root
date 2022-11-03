using API.Endpoints.Customer;
using Application.Services;
using EndpointHandler.Core.Extensions;
using FluentValidation;
using Interfaces;
using Logging;
using Microsoft.OpenApi.Models;
using Middleware;
using Models.Domain;
using Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepository<Customer>, CustomerRepository>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ILoggingService, LoggingService>();

// Add Validators from the Models assembly
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Customer)));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer API", Version = "v1" });
});

var endpointHandlersAssembly = Assembly.GetAssembly(typeof(GetCustomer));

builder.AddEndpointHandlers(endpointHandlersAssembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Log the time each call to all APIs takes
app.UseMiddleware<ApiPerformanceMiddleware>();

// Map all the endpoints to the classes that implement IEndpointHandler
app.MapEndpoints(endpointHandlersAssembly);

app.UseExceptionHandler(ExceptionHandler.Handle);

app.Run();
