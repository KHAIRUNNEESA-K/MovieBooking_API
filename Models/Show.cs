namespace MovieBooking_API.Models
{
    public class Show
    {
        public int ShowId { get; set; } 
        public string ShowName { get; set; } = string.Empty;

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();

    }
}
