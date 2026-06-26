var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();

// configure the http request pipeline.
app.UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
