using MovieBooking_API.Data;
using MovieBooking_API.Interface;
using MovieBooking_API.Migrations;
using MovieBooking_API.Models;

namespace MovieBooking_API.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly AppDbContext _context;
        private readonly ISeatRepository _seatRepository;
        public BookingService(IBookingRepository bookingRepository, AppDbContext context, ISeatRepository seatRepository)
        {
            _bookingRepository=bookingRepository;
            _context=context;
            _seatRepository=seatRepository;
        }
        public async Task ConfirmBookingAsync(int showId, List<string> seatNumbers)
        {
            if (showId <= 0)
                throw new ArgumentException("Invalid show id");

            if (seatNumbers == null || !seatNumbers.Any())
                throw new ArgumentException("Seat numbers are required");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var seats = await _seatRepository.GetSeatsByShowAndNumbersAsync(showId, seatNumbers);

                if (seats.Count != seatNumbers.Count)
                    throw new KeyNotFoundException("One or more seats not found");

                if (seats.Any(s => s.Status != SeatStatus.Held || s.HoldUntil < DateTime.UtcNow))
                    throw new InvalidOperationException("Seats are no longer available for booking");

                var booking = new Booking
                {
                    ShowId = showId,
                    IsConfirmed = true,
                    BookingTime = DateTime.UtcNow
                };

                await _bookingRepository.CreateBookingAsync(booking);

                foreach (var seat in seats)
                {
                    seat.Status = SeatStatus.Booked;
                    seat.HoldUntil = null;
                    seat.BookingId = booking.BookingId;
                }

                await _seatRepository.UpdateSeatsAsync(seats);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
