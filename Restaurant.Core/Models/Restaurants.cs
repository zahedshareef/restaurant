namespace Restaurant.Core.Models;

public class Restaurants
{
    public long Rid { get; set; }
    public long Cid { get; set; }
    public string Name { get; set; } = default!;
    public string City { get; set; } = default!;
    public long Building { get; set; }
    public string Street { get; set; } = default!;
    public long? Zipcode { get; set; }
}