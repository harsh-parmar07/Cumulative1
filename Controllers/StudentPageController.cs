using Cumulative_1.Controllers;
using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Cumulative_1.Controllers
{
    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }

        // GET : StudentPage/List
        public IActionResult List()
        {
            List<Student> Students = _api.ListStudents();
            return View(Students);
        }

        // GET : StudentPage/Show
        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            ViewData["Id"] = id;
            return View(SelectedStudent);
        }

        // GET: StudentPage/New
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        // GET: StudentPage/Validation
        [HttpGet]
        public IActionResult Validation()
        {

            if (TempData["ErrorMessage"] != null)
            {
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            }
            return View();
        }

        // POST: StudentPage/Create
        [HttpPost]
        public IActionResult Create(Student StudentData)
        {
            string EmployeeNumberPattern = @"^N\d{4}$";

            // Check for the student number validation
            if (string.IsNullOrEmpty(StudentData.StudentNumber))
            {
                TempData["ErrorMessage"] = "Student number cannot be empty";
                return RedirectToAction("Validation");
            }

            // Check for the student number number pattern
            if (!string.IsNullOrEmpty(StudentData.StudentNumber) && !Regex.IsMatch(StudentData.StudentNumber, EmployeeNumberPattern))
            {
                TempData["ErrorMessage"] = "Student number should start with 'N' followed by 4 digits. Eg: N1234";
                return RedirectToAction("Validation");
            }

            // Check for the student number which exist already
            if (!string.IsNullOrEmpty(StudentData.StudentNumber) && Regex.IsMatch(StudentData.StudentNumber, EmployeeNumberPattern))
            {
                List<Student> Students = _api.ListStudents();
                foreach (Student CurrentStudent in Students)
                {
                    if (CurrentStudent.StudentNumber == StudentData.StudentNumber)
                    {
                        TempData["ErrorMessage"] = "This student number has already been taken by the student";
                        return RedirectToAction("Validation");
                    }
                }
            }

            // Check for enrol date
            if (string.IsNullOrEmpty(StudentData.EnrolDate))
            {
                TempData["ErrorMessage"] = "Enrol Date cannot be empty.";
                return RedirectToAction("Validation");
            }
            // Check for future enrol date
            if (!string.IsNullOrEmpty(StudentData.EnrolDate) && DateTime.Parse(StudentData.EnrolDate) > DateTime.Now)
            {
                TempData["ErrorMessage"] = "Enrol Date cannot be in future.";
                return RedirectToAction("Validation");
            }
            // Check for student name field from the input and respond with appropriate error message
            if (string.IsNullOrEmpty(StudentData.StudentFName) && string.IsNullOrEmpty(StudentData.StudentLName))
            {
                TempData["ErrorMessage"] = "Student first and last name cannot be empty";
                return RedirectToAction("Validation");
            }
            else if (string.IsNullOrEmpty(StudentData.StudentFName))
            {
                TempData["ErrorMessage"] = "Student first name cannot be empty";
                return RedirectToAction("Validation");
            }
            else if (string.IsNullOrEmpty(StudentData.StudentLName))
            {
                TempData["ErrorMessage"] = "Student last name cannot be empty";
                return RedirectToAction("Validation");
            }

            else
            {
                int StudentId = _api.AddStudent(StudentData);

                // redirects to "Show" action on "Student" cotroller with id parameter supplied
                return RedirectToAction("Show", new { id = StudentId });
            }

        }

        // GET : StudentPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        // POST: StudentPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            string RowsAffected = _api.DeleteStudent(id);

            // redirects to list action
            return RedirectToAction("List");
        }

        // GET : StudentPage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            ViewData["Id"] = id;
            return View(SelectedStudent);

        }

        // POST: StudentPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string StudentFName, string StudentLName, string StudentNumber, string EnrolDate)
        {
            string EmployeeNumberPattern = @"^N\d{4}$";
            Student UpdateStudent = new Student();

            UpdateStudent.StudentFName = StudentFName;
            UpdateStudent.StudentLName = StudentLName;
            UpdateStudent.StudentNumber = StudentNumber;
            UpdateStudent.EnrolDate = EnrolDate.ToString();

            // Check for enrol date
            if (string.IsNullOrEmpty(UpdateStudent.EnrolDate))
            {
                TempData["ErrorMessage"] = "Enrol Date cannot be empty.";
                return RedirectToAction("Validation");
            }

            // Check for future enrol date
            if (!string.IsNullOrEmpty(UpdateStudent.EnrolDate) && DateTime.Parse(UpdateStudent.EnrolDate) > DateTime.Now)
            {
                TempData["ErrorMessage"] = "Enrol Date cannot be in future.";
                return RedirectToAction("Validation");
            }

            // Check for the student number validation
            if (string.IsNullOrEmpty(UpdateStudent.StudentNumber))
            {
                TempData["ErrorMessage"] = "Student number cannot be empty";
                return RedirectToAction("Validation");
            }

            // Check for the student number number pattern
            if (!string.IsNullOrEmpty(UpdateStudent.StudentNumber) && !Regex.IsMatch(UpdateStudent.StudentNumber, EmployeeNumberPattern))
            {
                TempData["ErrorMessage"] = "Student number should start with 'N' followed by 4 digits. Eg: N1234";
                return RedirectToAction("Validation");
            }

            // Check for the student number which exist already
            if (!string.IsNullOrEmpty(UpdateStudent.StudentNumber) && Regex.IsMatch(UpdateStudent.StudentNumber, EmployeeNumberPattern))
            {
                List<Student> Students = _api.ListStudents();
                foreach (Student CurrentStudent in Students)
                {
                    if (UpdateStudent.StudentId == null && CurrentStudent.StudentNumber == UpdateStudent.StudentNumber)
                    {
                        TempData["ErrorMessage"] = "This student number has already been taken by the student";
                        return RedirectToAction("Validation");
                    }
                }
            }

            // Check for student name field from the input and respond with appropriate error message
            if (string.IsNullOrEmpty(UpdateStudent.StudentFName) && string.IsNullOrEmpty(UpdateStudent.StudentLName))
            {
                TempData["ErrorMessage"] = "Student first and last name cannot be empty";
                return RedirectToAction("Validation");
            }
            else if (string.IsNullOrEmpty(UpdateStudent.StudentFName))
            {
                TempData["ErrorMessage"] = "Student first name cannot be empty";
                return RedirectToAction("Validation");
            }
            else if (string.IsNullOrEmpty(UpdateStudent.StudentLName))
            {
                TempData["ErrorMessage"] = "Student last name cannot be empty";
                return RedirectToAction("Validation");
            }

            else
            {
                _api.UpdateStudent(id, UpdateStudent);

                // redirects to show student
                return RedirectToAction("Show", new { id = id });
            }

        }

    }
}
