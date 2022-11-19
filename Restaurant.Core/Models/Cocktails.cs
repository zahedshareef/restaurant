namespace Restaurant.Core.Models;

public class Cocktails
{
    public long Cid { get; set; }
    public string Name { get; set; } = default!;
    public List<Ingredients> Ingredients { get; set; } = default!;
    public string Garnish { get; set; } = default!;
    public string Preparation { get; set; } = default!;
}