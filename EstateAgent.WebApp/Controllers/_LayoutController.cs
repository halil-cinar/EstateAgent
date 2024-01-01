using EstateAgent.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    
    public class _LayoutController : Controller
    {
        public _LayoutController() { 
        
        
        }
        public IActionResult _Layout()
        {
            return View(new PropertySearchModel
            {
                Search="a"
            });
        }

        public PartialViewResult LogoPartial()
        {
            ViewBag.LogoId = 25;
            return PartialView();
        }
    }
}
