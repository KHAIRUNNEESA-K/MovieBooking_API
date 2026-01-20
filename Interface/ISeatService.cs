namespace MovieBooking_API.Interface
{
    public interface ISeatService
    {
        Task PredefinedSeatAsync(int showId);
        Task HoldSeatsAsync(int showId, List<string> seatNumbers);
    }
}
