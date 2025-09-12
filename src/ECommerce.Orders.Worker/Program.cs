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

        // --- MASSTRANSIT CONFIGURATIE ---
        services.AddMassTransit(x =>
        {
            // Registreer je consumer
            x.AddConsumer<CreateOrderCommandConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                // Configureer het ontvangende endpoint. MassTransit maakt automatisch
                // een queue aan voor je consumer.
                cfg.ConfigureEndpoints(context);
            });
        });
        // --- EINDE MASSTRANSIT CONFIGURATIE ---
    })
    .Build();

await host.RunAsync();
