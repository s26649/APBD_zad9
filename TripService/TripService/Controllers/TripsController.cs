using Microsoft.AspNetCore.Mvc;
using TripService.Models;
using TripService.Services;

namespace TripService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tripService.GetTrips(page, pageSize);
            return Ok(result);
        }

        [HttpDelete("clients/{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var success = await _tripService.DeleteClient(idClient);
            if (!success)
                return BadRequest("klient ma powiazane wycieczki i nie może zostac usuniety lub nie zostal znaleziony.");
            
            return NoContent();
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] ClientTripDto clientDto)
        {
            var success = await _tripService.AddClientToTrip(idTrip, clientDto);
            if (!success)
                return BadRequest("klient z tym PESELem juz istnieje, jest juz zapisany na wycieczke, wycieczka nie istnieje lub juz sie rozpoczela.");
            
            return CreatedAtAction(nameof(GetTrips), new { idTrip = idTrip }, clientDto);
        }
    }
}