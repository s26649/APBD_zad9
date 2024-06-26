namespace TripService.Data;

public class Country
{
    public int IdCountry { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<CountryTrip> CountryTrips { get; set; } = new();
}