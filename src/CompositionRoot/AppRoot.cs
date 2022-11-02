using Application.Services;
using Interfaces;
using Logging;
using Models.Domain;
using Repositories;

var api = new ApiStartup(args, options =>
{
    options.AddSingleton<IRepository<Customer>, CustomerRepository>();
    options.AddTransient<ICustomerService, CustomerService>();
    options.AddTransient<ILoggingService, LoggingService>();
});

await api.StartAsync();
