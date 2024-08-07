using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SampleAPI.Entities;
using SampleAPI.Handler;
using SampleAPI.Handlers;
using SampleAPI.Repositories;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => {cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());});

//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<SampleApiDbContext>(options => options.UseInMemoryDatabase(databaseName: "SampleDB"));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();//DI
builder.Services.AddScoped<CreateOrderHandler>();//CQRS Injection
builder.Services.AddScoped<GetRecentOrdersHandler>();//CQRS Injection
builder.Services.AddScoped<DeleteOrderHandler>();//CQRS Injection


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();