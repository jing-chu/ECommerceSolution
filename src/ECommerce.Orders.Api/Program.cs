using System.Reflection;
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
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
   
}

app.MapControllers();

app.Run();

