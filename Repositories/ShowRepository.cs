using Microsoft.EntityFrameworkCore;
using MovieBooking_API.Data;
using MovieBooking_API.Interface;
using MovieBooking_API.Models;

namespace MovieBooking_API.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly AppDbContext _context;
        public ShowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Show> AddShowAsync(string showName)
        {
            var show = new Show { ShowName = showName };
            _context.Shows.Add(show);
            await _context.SaveChangesAsync(); 
            return show;
        }

        public async Task<Show?> GetShowByIdAsync(int showId)
        {
            return await _context.Shows
                                 .Include(s => s.Seats)
                                 .FirstOrDefaultAsync(s => s.ShowId == showId);
        }
    }
}
