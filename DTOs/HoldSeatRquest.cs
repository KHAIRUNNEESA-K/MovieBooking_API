namespace MovieBooking_API.DTOs
{
    public class HoldSeatRquest
    {
        public int ShowId { get; set; }
        public List<string> SeatNumbers { get; set; } = new();
    }
}
