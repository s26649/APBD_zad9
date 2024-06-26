namespace TripService.Data;

public class Trip
{
    public int IdTrip { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<ClientTrip> ClientTrips { get; set; } = new();
    public List<CountryTrip> CountryTrips { get; set; } = new();
}