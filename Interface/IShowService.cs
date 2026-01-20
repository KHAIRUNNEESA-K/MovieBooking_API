using MovieBooking_API.DTOs;

namespace MovieBooking_API.Interface
{
    public interface IShowService
    {
        Task<int> CreateShowAsync(string showName);
        Task<ShowSeatResponse?> GetShowWithSeatsAsync(int showId);
    }
}
