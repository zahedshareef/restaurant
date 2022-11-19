using Newtonsoft.Json;

namespace Restaurant.Core.Models;

public class Ingredients
{
    public string Unit { get; set; } = default!;
    public double Amount { get; set; }
    
    [JsonProperty("Ingredient")]
    public string Ingredient { get; set; } = default!;
}