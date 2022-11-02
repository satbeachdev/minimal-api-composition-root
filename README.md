# Minimal API/Composition Root and Dynamic Endpoint Creation

In .NET 6, a new feature called Minimal APIs was introduced. This allows us to create an API with minimum code and no controller classes. The part that I did not like about was that all the examples that I found declared the handler in the API startup code as shown below. This seemed messy to me and would not scale especially once you start adding validation and error handling.

```
// Add new customer
app.MapPost(“/customers”, async ([FromBody] CustomerDto customerDto, [FromServices] ICustomerService customerService, HttpResponse response) =>
{
	var newCustomerDto = customerService.CreateNew(customerDto);

	response.StatusCode = 200;
	response.Headers.Location = $”customers/{newCustomerDto.Id}”;
})
.Accepts<CustomerDto>(“application/json”)
.Produces<CustomerDto>(StatusCodes.Status201Created);

```
Since another goal is to always loosely couple your API classes and services, I implemented the Composition Root pattern to initialize the dependency injection container with all the required services.

## Endpoint Handler ##

The EndpointHandler.Core assembly contains all of the code to dynamically find the endpoint handlers and wire them up as endpoints to the appropiate route. There are two methods to wire up the endpoints. 

### Initialization ###

The *WebApplicationBuilder* extension method, *AddEndpointHandlers()* is called to find all of the classes that implement the *IEndpointHandler* interface in the API assembly and adds them to the dependency injection container.

The *WebApplication* extension method, *MapEndpoints()* is called to create the endpoint mapping similar to the code above using the method attributes. 

```
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    options?.Invoke(builder.Services);

    builder.AddEndpointHandlers();

    _app = builder.Build();
    
    .
    .
    .

    _app.MapEndpoints();
```
### Endpoint Handler Classes ###

## CQRS ##

The CQRS pattern is not implemented in this project but could easily be adapted by simply splitting the commands and queries into separate assemblies.

## Validation ##
Validation is implemented using the excellent [FluentValidation](https://docs.fluentvalidation.net/en/latest/) package for command APIs since they send a command class in the API body. Other validation is handled by overriding the **Validate** method in the handler class.

