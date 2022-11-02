using API;
using EndpointHandler.Core.Extensions;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Middleware;

public class ApiStartup
{
    private WebApplication _app;

    public ApiStartup(string[] args, Action<IServiceCollection> options)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        options?.Invoke(builder.Services);

        // Find all the classes in this assembly that implement the IEndpointHandler interface
        builder.AddEndpointHandlers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer API", Version = "v1" });
        });

        // Add Validators from the Models assembly
        builder.Services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.FullName != null && a.FullName.Contains("Models")));

        _app = builder.Build();

        // Enable Swagger UI
        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
        }

        // Log the time each call to all APIs takes
        _app.UseMiddleware<ApiPerformanceMiddleware>();

        // Map all the endpoints to the classes that implement IEndpointHandler
        _app.MapEndpoints();
        
        _app.UseExceptionHandler(ExceptionHandler.Handle);
    }

    public Task StartAsync()
    {
        return _app.RunAsync();
    }
}