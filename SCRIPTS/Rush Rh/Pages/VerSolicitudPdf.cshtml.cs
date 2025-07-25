using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Rush_Rh.Pages
{
    public class VerSolicitudPdfModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Tipo { get; set; }

    public void OnGet()
    {
        // Razor renderiza la vista.
    }
}

}
