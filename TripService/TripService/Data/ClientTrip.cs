namespace TripService.Data;

public class ClientTrip
{
    public int IdClient { get; set; }
    public Client Client { get; set; } = new();
    public int IdTrip { get; set; }
    public Trip Trip { get; set; } = new();
    public DateTime RegisteredAt { get; set; }
    public DateTime? PaymentDate { get; set; }
}