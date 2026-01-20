using MovieBooking_API.Data;
using MovieBooking_API.Interface;
using MovieBooking_API.Models;

namespace MovieBooking_API.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly AppDbContext _context;
        private const int HoldMinutes = 2;
        public SeatService(ISeatRepository seatRepository,AppDbContext context)
        {
            _seatRepository = seatRepository;
            _context=context;
        }
        public async Task PredefinedSeatAsync(int showId)
        {
            try
            {
                bool seatsExist = await _seatRepository.AreSeatsAvailableAsync(showId);
                if (seatsExist)
                    return;

                var seats = new List<Seat>();

                for (char row = 'A'; row <= 'D'; row++)
                {
                    for (int seatNo = 1; seatNo <= 10; seatNo++)
                    {
                        seats.Add(new Seat
                        {
                            ShowId = showId,
                            SeatNumber = $"{row}{seatNo}",
                            Status = SeatStatus.Available
                        });
                    }
                }

                await _seatRepository.AddSeatAsync(seats);

            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task HoldSeatsAsync(int showId, List<string> seatNumbers)
        {
            if (showId <= 0)
                throw new ArgumentException("Invalid show id");

            if (seatNumbers == null || !seatNumbers.Any())
                throw new ArgumentException("Seat numbers are required");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var seats = await _seatRepository
                    .GetSeatsByShowAndNumbersAsync(showId, seatNumbers);

                if (seats.Count != seatNumbers.Count)
                    throw new KeyNotFoundException("One or more seats not found");

                if (seats.Any(s =>
                    s.Status == SeatStatus.Booked ||
                    (s.Status == SeatStatus.Held && s.HoldUntil > DateTime.UtcNow)))
                {
                    throw new InvalidOperationException(
                        "One or more seats are already booked or held");
                }

                foreach (var seat in seats)
                {
                    seat.Status = SeatStatus.Held;
                    seat.HoldUntil = DateTime.UtcNow.AddMinutes(HoldMinutes);
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
