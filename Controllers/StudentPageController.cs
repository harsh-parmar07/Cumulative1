using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;
using Cumulative_1.Controllers;

namespace Cumulative_1.Controllers
{
    public class StudentPageController : Controller
    {

        // API is responsible for gathering the information from the Database and MVC is responsible for giving an HTTP response
        // as a web page that shows the information written in the View

        // In practice, both the StudentAPI and StudentPage controllers
        // should rely on a unified "Service", with an explicit interface

        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }


        /// <summary>
        /// When we click on the Students button in Navugation Bar, it returns the web page displaying all the teachers in the Database student
        /// </summary>
        /// <returns>
        /// List of all Students in the Database student
        /// </returns>
        /// <example>
        /// GET : api/StudentPage/List  ->  Gives the list of all Students in the Database student
        /// </example>

        public IActionResult ListStudent()
        {
            List<Student> Students = _api.ListStudents();
            return View(Students);
        }



        /// <summary>
        /// When we Select one Student from the list, it returns the web page displaying the information of the SelectedStudent from the database student
        /// </summary>
        /// <returns>
        /// Information of the SelectedStudent from the database student
        /// </returns>
        /// <example>
        /// GET :api/StudentPage/Show/{id}  ->  Gives the information of the SelectedStudent
        /// </example>
        /// 
        public IActionResult ShowStudent(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }


    }
}
