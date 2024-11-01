using Microsoft.AspNetCore.Mvc;

namespace GiftStore.Areas.Administrator.Controllers
{
    [Area("Administrator")]

    public class CPanelController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
