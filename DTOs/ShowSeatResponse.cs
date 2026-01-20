namespace MovieBooking_API.DTOs
{
    public class ShowSeatResponse
    {
        public int ShowId { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public Dictionary<string, List<SeatStatusResponse>> SeatsByRow { get; set; } = new();
    }
}
