using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmx_examples.Pages.ActiveSearch;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    readonly IAntiforgery _antiforgery;
    public string? RequestToken { get; set; }
    public List<Country> Countries { get; set; }

    public IndexModel(IHttpClientFactory factory, IAntiforgery antiforgery)
    {
        _httpClient = factory.CreateClient();
        _antiforgery = antiforgery;
    }

    public async Task OnGet()
    {
        var tokenSet = _antiforgery.GetAndStoreTokens(HttpContext);
        
        var curi = "https://restcountries.com/v3.1/all?fields=name";
        var result = await _httpClient.GetStringAsync(curi);
        ParsCountries(result);

        RequestToken = tokenSet.RequestToken;
    }

    [BindProperty] public string SearchText { get; set; }

    public async Task<PartialViewResult> OnPostSearch()
    {
        var result = await _httpClient.GetStringAsync($"https://restcountries.com/v3.1/name/{SearchText}");
        
        ParsCountries(result);

        return Partial("_searchResult", Countries);
    }

    private void ParsCountries(string result)
    {
        Countries = new List<Country>();
        var json = JsonNode.Parse(result);
        foreach (var country in json.AsArray())
        {
            var name = country["name"];
            var commonName = name["common"];
            var value = commonName.GetValue<string>();
            
            Countries.Add(new(value));
        }
    }
}