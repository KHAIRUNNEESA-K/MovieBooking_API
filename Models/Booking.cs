namespace MovieBooking_API.Models
{
    public class Booking
    {
        public int BookingId { get; set; }     

        public int ShowId { get; set; }
        public Show Show { get; set; } = null!;

        public bool IsConfirmed { get; set; } = false;

        public DateTime BookingTime { get; set; } = DateTime.UtcNow;

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();

    }
}
