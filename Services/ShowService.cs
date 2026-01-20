using MovieBooking_API.Data;
using MovieBooking_API.DTOs;
using MovieBooking_API.Interface;
using MovieBooking_API.Models;
using System.Threading.Tasks;

namespace MovieBooking_API.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly ISeatService _seatService;
        private readonly AppDbContext _context;
        public ShowService(IShowRepository showRepository, ISeatService seatService, AppDbContext context)
        {
            _showRepository = showRepository;
            _seatService = seatService;
            _context = context;
        }
        public async Task<int> CreateShowAsync(string showName)
        {
            if (string.IsNullOrWhiteSpace(showName))
            {
                throw new ArgumentException("Show name is required");
            }
                
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var show = await _showRepository.AddShowAsync(showName);

                if (show == null)
                {
                    throw new InvalidOperationException("Failed to create show");
                }

                await _seatService.PredefinedSeatAsync(show.ShowId);

                await transaction.CommitAsync();
                return show.ShowId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ShowSeatResponse?> GetShowWithSeatsAsync(int showId)
        {
            if (showId <= 0)
                throw new ArgumentException("Invalid show id");

            var show = await _showRepository.GetShowByIdAsync(showId);

            if (show == null)
                throw new KeyNotFoundException("Show not found");

            var groupedSeats = show.Seats
                .OrderBy(s => s.SeatNumber)
                .GroupBy(s => s.SeatNumber.Substring(0, 1))
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(s => new SeatStatusResponse
                    {
                        SeatNumber = s.SeatNumber,
                        Status = s.Status.ToString()
                    }).ToList()
                );

            return new ShowSeatResponse
            {
                ShowId = show.ShowId,
                ShowName = show.ShowName ?? string.Empty,
                SeatsByRow = groupedSeats
            };
        }

    }
}
