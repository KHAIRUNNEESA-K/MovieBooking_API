using System.ComponentModel.DataAnnotations;

namespace MovieBooking_API.Models
{
    public class Seat
    {
        public int SeatId { get; set; }

        public int ShowId { get; set; }               
        public Show Show { get; set; } = null!;

        public string SeatNumber { get; set; } = string.Empty;

        public SeatStatus Status { get; set; } = SeatStatus.Available;

        public DateTime? HoldUntil { get; set; }

        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }

        [Timestamp]                               // EF Core concurrency
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
