using Microsoft.AspNetCore.Mvc;
using MovieBooking_API.DTOs;
using MovieBooking_API.Interface;
using MovieBooking_API.Services;

namespace MovieBooking_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;
        private readonly ISeatService _seatServices;
        private readonly IBookingService _bookingService;
        public ShowsController(IShowService showService, ISeatService seatServices, IBookingService bookingService)
        {
            _showService = showService;
            _seatServices=seatServices;
            _bookingService=bookingService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateShow([FromBody] CreateShowRequest request)
        {
            try
            {
                int showId = await _showService.CreateShowAsync(request.ShowName);

                return Ok(new
                {
                    success = true,
                    showId,
                    showName = request.ShowName,
                    message = "Show created successfully with predefined seats"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("{showId}")]
        public async Task<IActionResult> GetShowWithSeat(int showId)
        {
            try
            {
                var showWithSeats = await _showService.GetShowWithSeatsAsync(showId);

                return Ok(new
                {
                    success = true,
                    message = "Seat status retrieved successfully",
                    seatStatus = showWithSeats
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpPost("holdSeats")]
        public async Task<IActionResult> HoldSeats([FromBody] HoldSeatRquest request)
        {
            try
            {
                await _seatServices.HoldSeatsAsync(request.ShowId,request.SeatNumbers);

                return Ok(new
                {
                    success = true,
                    message = "Seats held successfully",
                    showId = request.ShowId,
                    seats = request.SeatNumbers
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmBooking([FromBody] ConfirmBookingRequest request)
        {
            try
            {
                await _bookingService.ConfirmBookingAsync(request.ShowId, request.SeatNumbers);

                return Ok(new
                {
                    success = true,
                    message = "Booking confirmed successfully",
                    showId = request.ShowId,
                    seats = request.SeatNumbers
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


    }
}
