using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    public class AdminLayoutController : Controller
    {
        public AdminLayoutController()
        {

        }
        public IActionResult AdminLayout()
        {
            return View();
        }

        public PartialViewResult Head()
        {
            return PartialView();
        }
        public PartialViewResult LeftPanel()
        {
            return PartialView();
        }
        public PartialViewResult NavbarVertical()
        {
            return PartialView();
        }
        public PartialViewResult Scripts()
        {
            return PartialView();
        }

    }
}
