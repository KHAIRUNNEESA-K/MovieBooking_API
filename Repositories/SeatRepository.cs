using Microsoft.EntityFrameworkCore;
using MovieBooking_API.Data;
using MovieBooking_API.Interface;
using MovieBooking_API.Models;

namespace MovieBooking_API.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;
        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AreSeatsAvailableAsync(int showId)
        {
            return await _context.Seats.AnyAsync(s => s.ShowId == showId && s.Status == SeatStatus.Available);
        }
        public async Task AddSeatAsync(List<Seat> seats)
        {
            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Seat>> GetSeatsByShowAndNumbersAsync(int showId, List<string> seatNumbers)
        {
            return await _context.Seats
                .Where(s => s.ShowId == showId && seatNumbers.Contains(s.SeatNumber))
                .ToListAsync();
        }
        public async Task UpdateSeatsAsync(List<Seat> seats)
        {
            _context.Seats.UpdateRange(seats);
            await _context.SaveChangesAsync();
        }
    }
}
