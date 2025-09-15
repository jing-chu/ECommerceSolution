using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Application.Orders.Commands.CreateOrder;
using ECommerce.Orders.Application.Orders.Commands.DeleteOrder;
using ECommerce.Orders.Application.Orders.Commands.UpdateOrder;
using ECommerce.Orders.Application.Orders.Queries.GetAllOrders;
using ECommerce.Orders.Application.Orders.Queries.GetOrderById;
using ECommerce.Orders.Application.Products.Commands.CreateProduct;
using ECommerce.Orders.Application.Products.Commands.DeleteProduct;
using ECommerce.Orders.Application.Products.Commands.UpdateProduct;
using ECommerce.Orders.Application.Products.Queries.GetAllProducts;
using ECommerce.Orders.Application.Products.Queries.GetProductById;
using ECommerce.Orders.Infrastructure;
using ECommerce.Orders.Infrastructure.Persistence.Repositories;
using ECommerce.Orders.Worker;
using ECommerce.Orders.Worker.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
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

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateOrderCommandConsumer>();

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
       
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

await host.RunAsync();
