using System.Reflection;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Application.Orders.Queries.GetAllOrders;
using ECommerce.Orders.Application.Orders.Queries.GetOrderById;
using ECommerce.Orders.Application.Products.Commands.CreateProduct;
using ECommerce.Orders.Application.Products.Commands.DeleteProduct;
using ECommerce.Orders.Application.Products.Commands.UpdateProduct;
using ECommerce.Orders.Application.Products.Queries.GetAllProducts;
using ECommerce.Orders.Application.Products.Queries.GetProductById;
using ECommerce.Orders.Contracts.Orders.Commands;
using ECommerce.Orders.Domain;
using ECommerce.Orders.Infrastructure;
using ECommerce.Orders.Infrastructure.Persistence.Repositories;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderSummaryRepository, OrderSummaryRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAllProductsQuery).Assembly)
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly)
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetProductByIdQuery).Assembly)
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(UpdateProductCommand).Assembly)
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(DeleteProductCommand).Assembly)
);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAllOrdersQuery).Assembly)
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly)
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetOrderByIdQuery).Assembly)
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(UpdateOrderCommand).Assembly)
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(DeleteOrderCommand).Assembly)
);


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        //Containers in hetzelfde Docker Compose netwerk praten met elkaar via hun servicenamen, niet via localhost.
        cfg.Host("rabbitmq", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        // Een oplossing van een klassieke race condition
        // Vertelt MassTransit om bij het opstarten niet op te geven, maar het
        // opnieuw te proberen als de verbinding mislukt.
        // Hier: probeer het 5 keer, met 10 seconden tussen elke poging.
        cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

//past automatisch database migraties toe bij het opstarten
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    //Seeding: standaardproducten toevoegen
    if (!dbContext.Products.Any())
    {
        dbContext.Products.Add(new Product { Name = "Laptop Pro 16", Price = 2500.00m });
        dbContext.Products.Add(new Product { Name = "Draadloze Muis", Price = 75.50m });
        dbContext.Products.Add(new Product { Name = "Mechanisch Toetsenbord", Price = 150.00m });
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  //http://localhost:5121/openapi/v1.json
}

app.MapControllers();

// Voorbeeld endpoints
app.MapGet("/", () => "Hello World!");

app.Run();

