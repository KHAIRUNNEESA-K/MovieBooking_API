using MovieBooking_API.Models;

namespace MovieBooking_API.Interface
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBookingAsync(Booking booking);

    }
}
