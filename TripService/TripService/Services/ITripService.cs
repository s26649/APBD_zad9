using TripService.Models;

namespace TripService.Services;

public interface ITripService
{
    Task<PageResult<TripDto>> GetTrips(int page, int pageSize);
    Task<bool> DeleteClient(int idClient);
    Task<bool> AddClientToTrip(int idTrip, ClientTripDto clientDto);
}