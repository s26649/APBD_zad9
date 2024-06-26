using Microsoft.EntityFrameworkCore;
using TripService.Data;
using TripService.Models;

namespace TripService.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext _context;

        public TripService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<TripDto>> GetTrips(int page, int pageSize)
        {
            var totalTrips = await _context.Trips.CountAsync();
            var trips = await _context.Trips
                .Include(t => t.CountryTrips)
                .ThenInclude(ct => ct.Country)
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.Client)
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tripsDto = trips.Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.CountryTrips.Select(ct => new CountryDto { Name = ct.Country.Name }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDto { FirstName = ct.Client.FirstName, LastName = ct.Client.LastName }).ToList()
            }).ToList();

            var result = new PageResult<TripDto>
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = (int)Math.Ceiling((double)totalTrips / pageSize),
                Data = tripsDto
            };

            return result;
        }

        public async Task<bool> DeleteClient(int idClient)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == idClient);

            if (client == null)
                return false;

            if (client.ClientTrips.Any())
                return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddClientToTrip(int idTrip, ClientTripDto clientDto)
        {
            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null)
                return false;

            if (trip.DateFrom <= DateTime.Now)
                return false;

            var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
            if (existingClient != null)
                return false;

            var isClientInTrip = await _context.ClientTrips
                .AnyAsync(ct => ct.IdTrip == idTrip && ct.Client.Pesel == clientDto.Pesel);
            if (isClientInTrip)
                return false;

            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone,
                Pesel = clientDto.Pesel
            };

            var clientTrip = new ClientTrip
            {
                Client = client,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientDto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}