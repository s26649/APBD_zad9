namespace TripService.Data;

public class CountryTrip
{
    public int IdCountry { get; set; }
    public Country Country { get; set; } = new();
    public int IdTrip { get; set; }
    public Trip Trip { get; set; } = new();
}