namespace MovieBooking_API.Interface
{
    public interface IBookingService
    {
        Task ConfirmBookingAsync(int showId, List<string> seatNumbers);
    }
}
