namespace Storefront.Models;

public class TripRecord
{
    public DateTime PickupDatetime { get; set; }
    
    public DateTime DropoffDatetime { get; set; }
    
    public byte PassengerCount { get; set; }
    
    public double TripDistance { get; set; }
    
    public string StoreAndFwdFlag { get; set; } = null!;
    
    public int PULocationID { get; set; }
    
    public int DOLocationID { get; set; }
    
    public decimal FareAmount { get; set; }
    
    public decimal TipAmount { get; set; }
}