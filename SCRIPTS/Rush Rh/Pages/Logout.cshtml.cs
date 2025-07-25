using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Rush_Rh.Pages
{
    public class Logout : PageModel
    {
        private readonly ILogger<Logout> _logger;

        public Logout(ILogger<Logout> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            HttpContext.Session.Clear(); // Borra todas las variables de sesión
            return RedirectToPage("/Index"); // Redirige al inicio
        }
    }
}