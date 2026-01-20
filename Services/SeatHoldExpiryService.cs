using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieBooking_API.Data;
using MovieBooking_API.Models;

public class SeatHoldExpiryService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

    public SeatHoldExpiryService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ReleaseExpiredSeatsAsync();
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task ReleaseExpiredSeatsAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var expiredSeats = await context.Seats
            .Where(s =>
                s.Status == SeatStatus.Held &&
                s.HoldUntil != null &&
                s.HoldUntil < DateTime.UtcNow)
            .ToListAsync();

        if (!expiredSeats.Any())
            return;

        foreach (var seat in expiredSeats)
        {
            seat.Status = SeatStatus.Available;
            seat.HoldUntil = null;
        }

        await context.SaveChangesAsync();
    }
}
