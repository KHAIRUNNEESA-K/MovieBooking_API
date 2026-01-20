using MovieBooking_API.Models;

namespace MovieBooking_API.Interface
{
    public interface IShowRepository
    {
        Task<Show> AddShowAsync(string showName);
        Task<Show?> GetShowByIdAsync(int showId);

    }
}
