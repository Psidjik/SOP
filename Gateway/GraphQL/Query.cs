using Gateway.Contracts.DTOs;
using Gateway.DomainData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Gateway.GraphQL;

public class Query
{
    private readonly GatewayDbContext _context;
    private readonly ILogger<Query> _logger;

    public Query(GatewayDbContext context, ILogger<Query> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<OrderDto>> GetOrdersAsync()
    {
        var correlationId = Guid.NewGuid().ToString();
        var sw = Stopwatch.StartNew();

        _logger.LogInformation("[{CorrelationId}] Начато получение всех заказов", correlationId);

        var orders = await _context.Orders.Include(o => o.Packages).ToListAsync();

        var result = orders.Select(o => new OrderDto(
            o.Id,
            o.Email,
            o.DestinationAddress,
            o.CreatedAt,
            o.Status,
            o.TotalCost,
            o.Packages.Select(p => new PackageDto(
                p.Id,
                p.Weight,
                p.Height,
                p.Width,
                p.Length,
                p.DeliveryCost
            )).ToList()
        )).ToList();

        sw.Stop();
        _logger.LogInformation("[{CorrelationId}] Получено {Count} заказов за {Elapsed} мс", correlationId, result.Count, sw.ElapsedMilliseconds);

        return result;
    }

    public async Task<List<PackageDto>> GetPackagesAsync()
    {
        var correlationId = Guid.NewGuid().ToString();
        var sw = Stopwatch.StartNew();

        _logger.LogInformation("[{CorrelationId}] Начато получение всех посылок", correlationId);

        var packages = await _context.Packages.ToListAsync();

        var result = packages.Select(p => new PackageDto(
            p.Id,
            p.Weight,
            p.Height,
            p.Width,
            p.Length,
            p.DeliveryCost
        )).ToList();

        sw.Stop();
        _logger.LogInformation("[{CorrelationId}] Получено {Count} посылок за {Elapsed} мс", correlationId, result.Count, sw.ElapsedMilliseconds);

        return result;
    }
}
