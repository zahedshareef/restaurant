using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Restaurant.Core.Models;

namespace Restaurant.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _hostEnvironment;
    
    public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment hostEnvironment, IMemoryCache cache)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        _cache = cache;
    }

    public async Task OnGetAsync()
    {
        var restaurantsJson = await new StreamReader(_hostEnvironment.WebRootPath + "/json/restaurant.json").ReadToEndAsync();
        var restaurants = JsonConvert.DeserializeObject<List<Restaurants>>(restaurantsJson);
        _cache.Set("restaurants", restaurants);
        
        var menuJson = await new StreamReader(_hostEnvironment.WebRootPath + "/json/menu.json").ReadToEndAsync();
        var menu = JsonConvert.DeserializeObject<List<Cocktails>>(menuJson);
        _cache.Set("menu", menu);
    }

    public IActionResult OnGetCocktails(string q)
    {
        var menu = _cache.Get<List<Cocktails>>("menu");
        var cocktailDetails = menu?.Select(y => y.Name).ToList();
        
        return cocktailDetails is null ? new JsonResult(new List<Cocktails>()) : new JsonResult(cocktailDetails);
    }

    public PartialViewResult OnPostRestaurants(string cocktail)
    {
        var restaurants = new List<Restaurants>();
        
        if (string.IsNullOrEmpty(cocktail))
            return Partial("_Restaurants", restaurants);

        var menu = _cache.Get<List<Cocktails>>("menu");
        var cocktailDetails = menu?.Where(x => x.Name.Equals(cocktail, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
        if (cocktailDetails is null)
            return Partial("_Restaurants", restaurants);

        var cachedRestaurants = _cache.Get<List<Restaurants>>("restaurants"); 
        restaurants = cachedRestaurants?.Where(x => x.Cid == cocktailDetails.Cid).ToList();

        return new PartialViewResult
        {
            ViewName = "_Restaurants",
            ViewData = new ViewDataDictionary<List<Restaurants>>(ViewData, restaurants)
        };
    }
}