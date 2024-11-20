using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;

namespace Cumulative_1.Controllers
{
    public class CoursesPageController : Controller
    {

        // API is responsible for gathering the information from the Database and MVC is responsible for giving an HTTP response
        // as a web page that shows the information written in the View

        // In practice, both the CoursesAPI and CoursesPage controllers
        // should rely on a unified "Service", with an explicit interface

        private readonly CoursesAPIController _api;

        public CoursesPageController(CoursesAPIController api)
        {
            _api = api;
        }


        /// <summary>
        /// When we click on the Courses button in Navugation Bar, it returns the web page displaying all the teachers in the Database school
        /// </summary>
        /// <returns>
        /// List of all Courses in the Database school
        /// </returns>
        /// <example>
        /// GET : api/CoursesPage/List  ->  Gives the list of all Courses in the Database school
        /// </example>

        public IActionResult ListCourses()
        {
            List<Course> Courses = _api.ListCourses();
            return View(Courses);
        }



        /// <summary>
        /// When we Select one Course from the list, it returns the web page displaying the information of the SelectedCourse from the database school
        /// </summary>
        /// <returns>
        /// Information of the SelectedCourse from the database school
        /// </returns>
        /// <example>
        /// GET :api/CoursesPage/Show/{id}  ->  Gives the information of the SelectedCourse
        /// </example>
        /// 
        public IActionResult ShowCourses(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }


    }
}
