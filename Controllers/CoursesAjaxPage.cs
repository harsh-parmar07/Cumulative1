using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Controllers
{
    public class CourseAjaxPage : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
