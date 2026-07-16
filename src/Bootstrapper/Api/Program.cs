using Serilog;
using Shared.ExceptionHandlers;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// common services: carter, mediatR, fluentValidation
builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly, typeof(BasketModule).Assembly);

builder.Services.AddMediatRWithAssemblies(typeof(CatalogModule).Assembly, typeof(BasketModule).Assembly);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();

app.MapCarter();

// configure the http request pipeline.
app.UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
