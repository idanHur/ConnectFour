using GameManager;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    public class PlayerController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Player model)
        {
            if (ModelState.IsValid)
            {
                // Handles the form submission...

                return RedirectToAction("Index", "Home"); // Redirect to a success page or a different page
            }

            // The model is invalid, return the view to display the validation messages.
            return View(model);
        }
    }
}
