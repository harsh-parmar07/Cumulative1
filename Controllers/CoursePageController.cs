using Cumulative_1.Controllers;
using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        // GET : CoursePage/List
        public IActionResult List()
        {
            List<Course> Courses = _api.ListCourses();
            return View(Courses);
        }

        // GET : CoursePage/Show/{id}
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            ViewData["Id"] = id;
            return View(SelectedCourse);
        }

        // GET: CoursePage/New
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        // GET: CoursePage/Validation
        [HttpGet]
        public IActionResult Validation()
        {

            if (TempData["ErrorMessage"] != null)
            {
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            }
            return View();
        }

        // POST: CoursePage/Create
        [HttpPost]
        public IActionResult Create(Course CourseData)
        {

            // Check for future start date
            if (!string.IsNullOrEmpty(CourseData.StartDate) && DateTime.Parse(CourseData.StartDate) > DateTime.Now)
            {
                TempData["ErrorMessage"] = "Course start date cannot be in future.";
                return RedirectToAction("Validation");
            }

            // Check for future finish date
            if (!string.IsNullOrEmpty(CourseData.FinishDate) && DateTime.Parse(CourseData.FinishDate) > DateTime.Now)
            {
                TempData["ErrorMessage"] = "Course finish date cannot be in future.";
                return RedirectToAction("Validation");
            }

            // Check for course name field from the input
            if (string.IsNullOrEmpty(CourseData.CourseName))
            {
                TempData["ErrorMessage"] = "Course name cannot be empty";
                return RedirectToAction("Validation");
            }

            else
            {
                int CourseId = _api.AddCourse(CourseData);
                return RedirectToAction("Show", new { id = CourseId });
            }

        }


        // GET : CoursePage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);

            return View(SelectedCourse);
        }

        // POST: StudentPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            string RowsAffected = _api.DeleteCourse(id);

            // redirects to list action
            return RedirectToAction("List");
        }

    }
}
