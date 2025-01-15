using Microsoft.AspNetCore.Mvc;

namespace AgencyMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
