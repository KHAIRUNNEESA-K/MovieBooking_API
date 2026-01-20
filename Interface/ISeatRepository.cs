using MovieBooking_API.Models;

namespace MovieBooking_API.Interface
{
    public interface ISeatRepository
    {
        Task<bool> AreSeatsAvailableAsync(int showId);
        Task AddSeatAsync(List<Seat> seats);
        Task<List<Seat>> GetSeatsByShowAndNumbersAsync(int showId, List<string> seatNumbers);
        Task UpdateSeatsAsync(List<Seat> seats);
    }
}
