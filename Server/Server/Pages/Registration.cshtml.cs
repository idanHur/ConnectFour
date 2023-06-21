using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Server.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public Player NewPlayer { get; set; }

        public void OnGet()
        {
        }
    }
}
