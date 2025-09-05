using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmx_examples.Pages.Selects;

public class IndexModel : PageModel
{
    private static readonly Dictionary<string, List<string>> MakeModel;

    static IndexModel()
    {
        MakeModel = new Dictionary<string, List<string>>
        {
            { "Audi", new() { "A1", "A4", "A6" } },
            { "Toyota", new() { "Landcruiser", "Tacoma", "Yaris" } },
            { "BMW", new() { "325i", "325ix", "X5" } }
        };
    }

    public void OnGet()
    {
        Companies = MakeModel.Keys.ToList();
        Company = Companies.First();
        CarModels = MakeModel[Company];
    }

    public List<string> Companies { get; set; }
    public List<string> CarModels { get; set; }
    [FromQuery(Name = "company")] public string Company { get; set; }

    public PartialViewResult OnGetModels()
    {
        CarModels = MakeModel[Company];

        return Partial("_modelSelector", CarModels);
    }
}