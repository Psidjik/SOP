using Gateway;
using Gateway.Application;
using Gateway.Contracts;
using Gateway.DomainData;
using Gateway.GraphQL;
using Gateway.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GatewayDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

/*var rabbitSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();
builder.Services.AddSingleton(rabbitSettings);
builder.Services.AddSingleton<RabbitMqPublisher>();*/

builder.Services.AddScoped<IOrdersApi, OrdersApi>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<LoggingAndPerformanceMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGraphQL();

app.UseHttpsRedirection();

app.MapOrdersEndpoints();

app.Run();
